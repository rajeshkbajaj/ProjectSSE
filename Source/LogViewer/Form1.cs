using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Xml;
using Covidien.CGRS.BinaryLogDecoder;
using IPI_Core;

namespace LogViewer
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public partial class Form1 : Form
    {
        private string logSrc = "../../SourceDefinitions/VikingLogs-Group1.xml" ;



        public Form1()
        {
            InitializeComponent();


            StringBuilder myArgsStrings = new StringBuilder();
            if ( Program.MCmdLineArgs == null)
            {
                myArgsStrings.Append( "args is null" ) ;
            }
            else
            {
                myArgsStrings.Append( string.Format( "args length is {0}<br/>", Program.MCmdLineArgs.Length ) ) ;
                for( int i = 0 ;   i < Program.MCmdLineArgs.Length ;   i++ )
                {
                    string argument = Program.MCmdLineArgs[i];
                    myArgsStrings.Append( string.Format( "args index {0} is [ {1} ]<br/>", i, argument ) ) ;
                }
            }


            // webBrowser1.GoHome();

            webBrowser1.ScrollBarsEnabled = true;
        }


        private void LoadLogFileHeader( string src, string logName, string renderName )
        {
            LogDefinition logDefn = LogDecoder.GetLogDefinition( logName ) ;
            if  ( null == logDefn )
                return ;

            RenderDefinition renderDefn = LogDecoder.GetRenderDefinition(logDefn, renderName);
            if  ( null == renderDefn )
                return ;

            List<string> headerData = LogDecoder.LoadLogFileHeaderONLY( src, logDefn, renderDefn ) ;
            
            if  ( 0 < logDefn.Header.Renders.Count )
            {
                StringBuilder sb = new StringBuilder();
                sb.Append( "My Meta Data:" ) ;

                int cnt = 0;
                foreach( string rowStr in logDefn.Header.Renders[0].RenderTableRows )
                {
                    // HACK FOR NOW - because I know what I coded in the xml rendering strings
                    int posn1 = rowStr.IndexOf( "<tr><td>") ;
                    int posn2 = rowStr.IndexOf( "</td><td>", posn1 ) ;
                    int posn3 = rowStr.IndexOf( "</td></tr>", posn2 ) ;
                    if  ( ( 0 <= posn1 )  &&  ( 0 < posn2 )  &&  ( 0 < posn3 ) )
                    {
                        string tag = rowStr.Substring( posn1 + 8, posn2 - posn1 - 8 ).Replace( ' ', '_' ) ;
                        string exprStr = rowStr.Substring( posn2 + 9, posn3 - posn2 - 9 ) ;

                        string replacedStr = Regex.Replace( exprStr, @"%\d+%",
                                        delegate(Match match)
                                        {
                                            string v = match.ToString();
                                            string v2 = v.Substring(1, v.Length - 2);
                                            /*
                                            int tmpInt2;
                                            if (Int32.TryParse(v2, out tmpInt2))
                                            {
                                                return( headerData[ tmpInt2 ] ) ;
                                            }
                                            return (v);
                                            */
                                            return( headerData[ Int32.Parse( v2 ) ] ) ;
                                        }
                                    );

                        replacedStr = replacedStr.Replace( "'", "" ) ;

                        sb.Append( string.Format( "\t<meta name=\"{0}\" content=\"{1}\" />\n", tag, replacedStr ) ) ;
                        cnt++;
                    }
                    else
                    {
                        sb.Append( string.Format( "\t<meta name=\"{0}\" content=\"{1}\" />", rowStr, headerData[cnt++] ) ) ;
                    }
                    // jsFInitCode += "rows[ rows.length ] = '" + rowStr + "' ;\n";
                }
                sb.Append( "Completed " + cnt + " meta data items" ) ;

                byte[] bytes = File.ReadAllBytes( src ) ;
                string base64 = Convert.ToBase64String( bytes ) ;

StringBuilder sb2 = new StringBuilder();
for( int i = 112 ;   i < bytes.Length  &&  i < 200 ;   i++ )
  sb2.Append( string.Format( "[{0}] = {1};\r\n", i, bytes[i] ) ) ;

                webBrowser1.DocumentText = sb.ToString() + "\n\r\n" + base64 + ";\r\n" + sb2.ToString() ;
            }
            else
            {
                webBrowser1.DocumentText = " NO HEADER RENDER DEFINITION";
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string src = filename.Text;
            string logName = comboLogNames.SelectedItem.ToString() ;
            string renderName = comboRenderNames.SelectedItem.ToString();

/*
            byte[] bytes = File.ReadAllBytes(src);
            string base64 = Convert.ToBase64String(bytes);

            webBrowser1.DocumentText = base64 + "\r\n";
            if (true)
                return;
*/
            try
            {
                if  ( !string.IsNullOrEmpty( logName )  &&  !string.IsNullOrEmpty( renderName) )
                {
                    LoadLogFileHeader( src, logName, renderName ) ;
                    // LoadLogFile( src, logName, renderName ) ;
                }
                else
                {
                    Console.WriteLine( "ERROR - No info to decode or else to render log" );
                }
            }
            catch( Exception ex )
            {
                Console.WriteLine( "ERROR LOADING FILE Exception:"+ex.Message ) ;
            }
        }

        private void btnLoadLogDefns_Click(object sender, EventArgs e)
        {
            LogDecoder.LoadLogDefinitions( logSrc ) ;

            comboLogNames.Items.Clear();

            foreach( LogDefinition logDefn in LogDecoder.GetLogDefns() )
            {
                comboLogNames.Items.Add( logDefn.Name ) ;

                Console.WriteLine( "\r\n-----------\r\nLog: {0}", logDefn.Name ) ;
                Console.WriteLine( " -- meta: {0}", (null != logDefn.MetaData ? logDefn.MetaData.Count : 0 ) ) ;
                Console.WriteLine( " -- header: {0}", (null != logDefn.Header ? logDefn.Header.Fields.Count : 0 ) ) ;
                Console.WriteLine( " -- fields: {0}", (null != logDefn.Fields ? logDefn.Fields.Count : 0 ) ) ;
                Console.WriteLine( " -- unions: {0}", (null != logDefn.Unions ? logDefn.Unions.Count : 0 ) ) ;
                Console.WriteLine( " -- unionSets: {0}", (null != logDefn.UnionSets ? logDefn.UnionSets.Count : 0 ) ) ;
                Console.WriteLine( " -- renders: {0}", (null != logDefn.Renders ? logDefn.Renders.Count : 0 ) ) ;
            }

            comboLogNames.SelectedIndex = 0;
            // Happens automatically when the selected index is changed.
            // UpdateComboRenderNames( comboLogNames.SelectedItem.ToString() ) ;
        }


        private void UpdateComboRenderNames( string logName )
        {
            foreach( LogDefinition logDefn in LogDecoder.GetLogDefns() )
            {
                if  ( logName.Equals( logDefn.Name ) )
                {
                    comboRenderNames.Items.Clear();
                    foreach( RenderDefinition renderDefn in logDefn.Renders )
                    {
                        comboRenderNames.Items.Add( renderDefn.Name ) ;
                    }
                    comboRenderNames.SelectedIndex = 0;
                }
            }
        }


        private void comboLogNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComboRenderNames( comboLogNames.SelectedItem.ToString() ) ;
        }

        private void btnRecvLog_Click(object sender, EventArgs e)
        {
            string xmlStr = System.IO.File.ReadAllText( filename.Text ) ;

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml( xmlStr ) ;
            }
            catch( Exception)
            {
                throw;
            }

            XmlNodeList logFileDefns = xmlDoc.SelectNodes("//LogFiles/LogFile");
            foreach( XmlElement logFileDefinition in logFileDefns )
            {
                string logName = logFileDefinition.GetAttribute("name");
                if ( !string.IsNullOrEmpty( logName ) )
                {
                    // OK, partial success, we get to work on the rest of it
                }
                else
                {
                    continue;  // if the message is not named, then ignore it
                }

                string xPathPrefix ;

                string transferEncoding = null;
                string contentEncoding = null;

                xPathPrefix = string.Format( "//LogFiles/LogFile[@name='{0}']/head/meta", logName ) ;
                XmlNodeList headerData = xmlDoc.SelectNodes( xPathPrefix ) ;
                foreach( XmlElement metaDatum in headerData )
                {
                    string metaId;

                    metaId = metaDatum.GetAttribute( "http-equiv" ) ;
                    if  ( !string.IsNullOrEmpty( metaId ) )
                    {
                        if  ( metaId.Equals( "transfer-encoding", StringComparison.OrdinalIgnoreCase) )
                        {
                            transferEncoding = metaDatum.GetAttribute( "content" ) ;
                        }
                        else if  ( metaId.Equals( "content-encoding", StringComparison.OrdinalIgnoreCase) )
                        {
                            contentEncoding = metaDatum.GetAttribute( "content" ) ;
                        }
                    }

                    // not currently worried about content-encoding or other meta data fields
                }

                xPathPrefix = string.Format( "//LogFiles/LogFile[@name='{0}']/body", logName ) ;
                XmlNodeList bodyData = xmlDoc.SelectNodes( xPathPrefix ) ;
                foreach( XmlElement bodyDatum in bodyData )
                {
                    string logData = bodyDatum.InnerText;
                    byte[] rawLogData = null;

                    if  ( !string.IsNullOrEmpty( transferEncoding ) )
                    {
                        if  ( transferEncoding.Equals( "base64", StringComparison.OrdinalIgnoreCase ) )
                        {
                            // logData = logData.Replace( " ", "" ).Replace( "\r", "" ).Replace( "\n", "" ) ;
                            // StripChars() is a custom extension in IPI_Core.StringExtensions
                            logData = logData.StripChars( new HashSet<char>( new[] { ' ', '\t', '\n', '\r' } ) ) ;
                            rawLogData = Convert.FromBase64String( logData ) ;
                        }
                        // else need to figure out what to do
                    }
                    // else it is already a readable string, so leave it as is


                    if  ( !string.IsNullOrEmpty( contentEncoding ) )
                    {
                        if  ( transferEncoding.Equals( "gzip", StringComparison.OrdinalIgnoreCase ) )
                        {
                            MemoryStream memStreamA = new MemoryStream( rawLogData.Length ) ;
                            GZipStream gz = new GZipStream( memStreamA, CompressionMode.Decompress );
                            MemoryStream memStreamB = new MemoryStream( rawLogData.Length ) ;
                            byte[] buffer = new byte[8192];
                            while (true)
                            {
                                int delta = gz.Read(buffer, 0, buffer.Length);
                                if (delta > 0)
                                    memStreamB.Write(buffer, 0, delta);
                                if (delta < buffer.Length)
                                    break;
                            }
                            rawLogData = memStreamB.ToArray();
                        }
                        // else need to figure out what to do
                    }
                    // else it is already a readable string, so leave it as is

                    // NOW decode the bytes!!!


/* SAMPLE COMPRESS / DECOMPRESS */
                    Console.WriteLine( "1 Raw = {0} -- {1};", rawLogData.Length, BitConverter.ToString( rawLogData ) ) ;

                    MemoryStream memStream2 = new MemoryStream( rawLogData.Length ) ;
                    GZipStream gz2 = new GZipStream( memStream2, CompressionMode.Compress ) ;
                    gz2.Write( rawLogData, 0, rawLogData.Length ) ;
                    gz2.Close();
                    byte[] rawLogData3 = memStream2.ToArray();

                    Console.WriteLine( "2a Zip = {0} -- {1};", rawLogData3.Length, BitConverter.ToString( rawLogData3 ) ) ;
                    Console.WriteLine( "2b B64 = {0} -- {1};", Convert.ToBase64String( rawLogData3 ).Length, Convert.ToBase64String( rawLogData3 ) ) ;

                    memStream2 = new MemoryStream( rawLogData3 ) ;
                    gz2 = new GZipStream( memStream2, CompressionMode.Decompress ) ;

                    MemoryStream memStream3 = new MemoryStream( rawLogData3.Length ) ;
                    byte[] buf = new byte[8192];
                    while (true)
                    {
                        int delta = gz2.Read( buf, 0, buf.Length);

                        if (delta > 0)
                            memStream3.Write( buf, 0, delta);

                        if (delta < buf.Length)
                            break;
                    }

                    byte[] rawLogData4 = memStream3.ToArray();

                    Console.WriteLine( "3 Std = {0} -- {1};", rawLogData4.Length, BitConverter.ToString( rawLogData4 ) ) ;
                }
            }
        }

        private void btnTestPdf_Click(object sender, EventArgs e)
        {
            string fname ;
            bool exists;

            fname = Environment.CurrentDirectory + @"/../../SourceDefinitions/RMS_Tasks_2012_08_01.pdf";
            exists = System.IO.File.Exists(fname);


            webBrowser1.Navigate(fname);
        }

    }
}
