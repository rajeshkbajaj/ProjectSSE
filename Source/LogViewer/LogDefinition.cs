using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace LogViewer
{
    public class LogDefinition
    {
        public String Name { get; set; }
        public String SrcFile { get; set; }
        public List<KeyValuePair<string, string>> MetaData { get; set; }
        public HeaderDefinition Header { get; set; }
        public List<FieldDefinition> Fields { get; set; }
        public List<UnionDefinition> Unions { get; set; }
        public List<UnionSetDefinition> UnionSets { get; set; }
        public List<RenderDefinition> Renders { get; set; }

// Now that it has gone through a first pass implementation, should probably have a Header and a Body, both of which look very similar in structure, in both XML and Code
// At this point, have purposefully only allowed the header a single render definition, but could see using multiples, with exactly the same name choice as in the body
// so on the outside, you only make one of the choices, and if the head (or body) doesn't support the given render-name, that info/table is not displayed.

        /// <summary>
        /// ctor()
        /// </summary>
        /// <param name="name">Name of the message</param>
        public LogDefinition( string name )
        {
            Name = name;

            MetaData = new List<KeyValuePair<string, string>>();
            Header = new HeaderDefinition();
            Fields = new List<FieldDefinition>();
            Unions = new List<UnionDefinition>();
            UnionSets = new List<UnionSetDefinition>();
            Renders = new List<RenderDefinition>();
        }


        /// <summary>
        /// Loads a file which describes (potentially) multiple messages
        /// </summary>
        /// <param name="filename">Filename to be loaded</param>
        /// <returns>List of DeviceDefinition objects</returns>
        public static List<LogDefinition> LoadDefinitions( string filename )
        {
            List<LogDefinition> results = new List<LogDefinition>();
            LogDefinition logDefn = null;

            string xmlStr = System.IO.File.ReadAllText(filename);

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(xmlStr);
            }
            catch( Exception e )
            {
                throw;
            }


            XmlNodeList logDefinitions = xmlDoc.SelectNodes( "//LogDefinition" ) ;
            foreach( XmlElement logDefinition in logDefinitions )
            {
                string logName = logDefinition.GetAttribute("name");
                if ( !string.IsNullOrEmpty( logName ) )
                {
                    logDefn = new LogDefinition(logName);
                    logDefn.SrcFile = filename;
                    results.Add(logDefn);
                }
                else
                {
                    continue;  // if the message is not named, then ignore it
                }

                string xPathPrefix = string.Format( "//LogDefinition[@name='{0}']/meta", logName ) ;
                XmlNodeList metaData = xmlDoc.SelectNodes( xPathPrefix ) ;
                foreach( XmlElement metaDatum in metaData )
                {
                    string metaName = metaDatum.GetAttribute( "name" ) ;
                    string content = metaDatum.GetAttribute( "content" ) ;
                    if  ( !string.IsNullOrEmpty( metaName )  &&  !string.IsNullOrEmpty( content ) )
                    {
                        logDefn.MetaData.Add( new KeyValuePair<string, string>( metaName, content ) ) ;
                    }
                }

                HeaderDefinition headerDefn = null;
                xPathPrefix = string.Format( "//LogDefinition[@name='{0}']/header", logName ) ;
                XmlNodeList headers = xmlDoc.SelectNodes( xPathPrefix ) ;
                foreach( XmlElement header in headers )
                {
                    headerDefn = new HeaderDefinition() ;
                    if  ( headerDefn.Itor( xPathPrefix, header ) )
                    {
                        logDefn.Header = headerDefn ;
                    }
                }

                FieldDefinition fieldDefn = null ;
                xPathPrefix = string.Format( "//LogDefinition[@name='{0}']/fields/Field", logName ) ;
                XmlNodeList fields = xmlDoc.SelectNodes( xPathPrefix ) ;
                foreach( XmlElement field in fields )
                {
                    fieldDefn = new FieldDefinition() ;
                    if  ( fieldDefn.Itor( xPathPrefix, field ) )
                    {
                        logDefn.Fields.Add( fieldDefn ) ;
                    }
                }

                UnionDefinition unionDefn = null ;
                xPathPrefix = string.Format( "//LogDefinition[@name='{0}']/unions/union", logName ) ;
                XmlNodeList unions = xmlDoc.SelectNodes( xPathPrefix ) ;
                foreach( XmlElement union in unions )
                {
                    string unionName = union.GetAttribute("name");
                    if ( !string.IsNullOrEmpty( unionName ) )
                    {
                        unionDefn = new UnionDefinition( unionName ) ;
                    }
                    else
                    {
                        continue;  // if the message is not named, then ignore it
                    }

                    // Note: must pass the Itor() in order to be added to the list.
                    if  ( unionDefn.Itor( xPathPrefix, union ) )
                    {
                        logDefn.Unions.Add( unionDefn ) ;
                    }
                }

                UnionSetDefinition unionSetDefn = null ;
                xPathPrefix = string.Format( "//LogDefinition[@name='{0}']/unionSets/unionSet", logName ) ;
                XmlNodeList unionSets = xmlDoc.SelectNodes( xPathPrefix ) ;
                foreach( XmlElement unionSet in unionSets )
                {
                    string unionSetName = unionSet.GetAttribute("name");
                    if (!string.IsNullOrEmpty(unionSetName))
                    {
                        unionSetDefn = new UnionSetDefinition( unionSetName ) ;
                    }
                    else
                    {
                        continue;  // if the message is not named, then ignore it
                    }

                    // Note: must pass the Itor() in order to be added to the list.
                    if  ( unionSetDefn.Itor( xPathPrefix, unionSet ) )
                    {
                        logDefn.UnionSets.Add( unionSetDefn ) ;
                    }
                }

                RenderDefinition renderDefn = null ;
                xPathPrefix = string.Format( "//LogDefinition[@name='{0}']/RenderDescriptions/RenderDescription", logName ) ;
                XmlNodeList renders = xmlDoc.SelectNodes( xPathPrefix ) ;
                foreach( XmlElement render in renders )
                {
                    string renderName = render.GetAttribute("name");
                    if ( !string.IsNullOrEmpty( renderName ) )
                    {
                        renderDefn = new RenderDefinition( renderName ) ;
                    }
                    else
                    {
                        continue;  // if the message is not named, then ignore it
                    }

                    // Note: must pass the Itor() in order to be added to the list.
                    if  ( renderDefn.Itor( xPathPrefix, render ) )
                    {
                        logDefn.Renders.Add( renderDefn ) ;
                    }
                }
            }

            return( results ) ;
        }
    }
}
