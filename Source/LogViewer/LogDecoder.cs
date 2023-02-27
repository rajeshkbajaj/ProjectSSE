using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Covidien.Ipi.InformaticsCore;

namespace LogViewer
{
    public class LogDecoder
    {
        static private List<LogDefinition> logDefns ;

        static public List<LogDefinition> GetLogDefns()
        {
            return( logDefns ) ;
        }

        static public void LoadLogDefinitions( string filename )
        {
            logDefns = LogDefinition.LoadDefinitions( filename ) ;
        }

        static public LogDefinition GetLogDefinition( string logName )
        {
            foreach( LogDefinition log in logDefns )
            {
                if  ( logName.Equals( log.Name ) )
                {
                    return( log ) ;
                }
            }
            return( null ) ;
        }

        static public RenderDefinition GetRenderDefinition( LogDefinition log, string renderName )
        {
            foreach( RenderDefinition render in log.Renders )
            {
                if  ( renderName.Equals( render.Name ) )
                {
                    return( render ) ;
                }
            }
            return( null ) ;
        }



        static public BinaryDataDecodeContext[] PrepDecoders( List<FieldDefinition> fields, List<RenderFieldDefinition> renderFields )
        {
            // Sanity check.  Mostly useful for logs without a header
            if  ( ( null == fields )  ||  ( 0 == fields.Count) )
            {
                return( null ) ;
            }


            // 1. Need a hash-table for cross-table matching (to add rendering (aka formatting) into the reading part)
/* Currently unnecessary - AND am allowing multiple fields with the same name, would need to handle that if re-introduced
            Hashtable fieldTable = new Hashtable();
            foreach( FieldDefinition field in fields )
            {
                fieldTable.Add( field.Name, field ) ;
            }
*/
            Hashtable renderTable = new Hashtable();
            foreach( RenderFieldDefinition field in renderFields )
            {
                renderTable.Add( field.Name, field ) ;
            }


            // 2. Need input ordering to be sorted (by data order) to ensure we properly decode the table

            List<FieldDefinition> FieldList = fields.OrderBy(x => x.DataOrder).ToList();


            // 3. Generate an array of BinaryDataDecodeContext[], one for each field

            BinaryDataDecodeContext[] results = new BinaryDataDecodeContext[ FieldList.Count ] ;
            int cnt = 0;
            foreach( FieldDefinition field in FieldList )
            {
                results[ cnt ] = new BinaryDataDecodeContext() ;
                results[ cnt ].Field = field ;

                switch( field.DataType.Name )
                {
                    case "Byte":
                    case "UInt8":
                        results[ cnt ].ReadDelegate = BinaryReaderDelegates.ReadUInt8 ;
                        break;
                    case "UInt16" :
                        results[ cnt ].ReadDelegate = BinaryReaderDelegates.ReadUInt16 ;
                        break;
                    case "UInt32" :
                        results[ cnt ].ReadDelegate = BinaryReaderDelegates.ReadUInt32 ;
                        break;
                    default:
                        throw( new Exception( string.Format( "LogDecoder::PrepDecoders field {0}; bad data type {1};", field.Name, field.DataType ) ) ) ;
                        break;
                }

                results[ cnt ].RequestedBytes = field.NumBytes ;
                
                if  ( renderTable.ContainsKey( field.Name ) )
                {
                    results[ cnt ].Format = ((RenderFieldDefinition) renderTable[ field.Name ]).Format ;
                    results[ cnt ].OuterFormat = ((RenderFieldDefinition) renderTable[ field.Name ]).OuterFormat ;
                }

                // NOTE: FOR THIS FIRST PASS, WE ARE GOING TO LEAVE Decoder and SupportData as null
                // The idea will be to provide additional processing, enum elaboration, etc.

                cnt++;
            }

            return( results ) ;
        }


        static public BinaryMemoryDataDecodeContext[] PrepUnionDecoders( List<FieldDefinition> fields, List<RenderFieldDefinition> renderFields )
        {
            // Sanity check.  Mostly useful for logs without a header
            if  ( ( null == fields )  ||  ( 0 == fields.Count) )
            {
                return( null ) ;
            }


            // 1. Need a hash-table for cross-table matching (to add rendering (aka formatting) into the reading part)

            Hashtable fieldTable = new Hashtable();
            Hashtable renderTable = new Hashtable();

            foreach( FieldDefinition field in fields )
            {
                fieldTable.Add( field.Name, field ) ;
            }

            foreach( RenderFieldDefinition field in renderFields )
            {
                renderTable.Add( field.Name, field ) ;
            }


            // 2. Need input ordering to be sorted (by data order) to ensure we properly decode the table

            List<FieldDefinition> FieldList = fields.OrderBy(x => x.DataOrder).ToList();


            // 3. Generate an array of BinaryDataDecodeContext[], one for each field

            BinaryMemoryDataDecodeContext[] results = new BinaryMemoryDataDecodeContext[ FieldList.Count ] ;
            int cnt = 0;
            foreach( FieldDefinition field in FieldList )
            {
                results[ cnt ] = new BinaryMemoryDataDecodeContext() ;
                results[ cnt ].Field = field ;

                switch( field.DataType.Name )
                {
                    case "Byte":
                    case "UInt8":
                        results[ cnt ].ReadDelegate = BinaryReaderDelegates.ReadUInt8 ;
                        break;
                    case "UInt16" :
                        results[ cnt ].ReadDelegate = BinaryReaderDelegates.ReadUInt16 ;
                        break;
                    case "UInt32" :
                        results[ cnt ].ReadDelegate = BinaryReaderDelegates.ReadUInt32 ;
                        break;
                    default:
                        throw( new Exception( string.Format( "LogDecoder::PrepUnionDecoders field {0}; bad data type {1};", field.Name, field.DataType ) ) ) ;
                        break;
                }

                results[ cnt ].RequestedBytes = field.NumBytes ;
                
                if  ( renderTable.ContainsKey( field.Name ) )
                {
                    results[ cnt ].Format = ((RenderFieldDefinition) renderTable[ field.Name ]).Format ;
                    results[ cnt ].OuterFormat = ((RenderFieldDefinition) renderTable[ field.Name ]).OuterFormat ;
                }

                // NOTE: FOR THIS FIRST PASS, WE ARE GOING TO LEAVE Decoder and SupportData as null
                // The idea will be to provide additional processing, enum elaboration, etc.

                cnt++;
            }

            return( results ) ;
        }





        public class UnionDataDecoderSupportData
        {
            public List<UnionSetDefinition> UnionSets;
            public List<UnionDefinition> UnionDefns; 
            public List<KeyValuePair<string, BinaryMemoryDataDecodeContext[]>> UnionDecoders ;
            public string UnionSetName ;
        }


        static public string UnionDataDecoder( ByteConverterData data, UnionDataDecoderSupportData supportData )
        {
            StringBuilder sbBody = new StringBuilder();

            // First determine if we have the right union set
            UnionSetDefinition unionSet = null;
            foreach( UnionSetDefinition unionSetDefn in supportData.UnionSets )
            {
                if  ( supportData.UnionSetName.Equals( unionSetDefn.Name ) )
                {
                    unionSet = unionSetDefn;
                    break;
                }
            }
            if  ( null == unionSet )
            {
                return( null ) ;
            }

// Note: may alter supportData to hold a hashtable for more efficient access.
// This may necessitate going back one step farther into the defn objects.

            // Second, go perform a minimal decode to figure out which of the union choices is appropriate.
            // Take first successful if bad XML defn.  Remember, a -1 high value will be our catch-all.  If no default, then return as a hex displayed string of bytes (or is that going to be in format?)
            UnionChoiceDefinition unionChoice = null ;
            UnionDefinition unionDefn = null;
            BinaryMemoryDataDecodeContext[] unionDecoders = null;
            bool done = false;
            foreach( UnionChoiceDefinition unionChoiceDefn in unionSet.UnionChoices )
            {
                if  ( ( -1 == unionChoiceDefn.LowValue )  &&  ( -1 == unionChoiceDefn.HighValue ) )
                {
                    unionChoice = unionChoiceDefn;
                    continue;
                }

// NOTE: this is not efficient... AND THEN it stupidly assumes the first object is the controlling object.
                // for the given choice, find it's controlling element and determine if there is a (range) match
                foreach( KeyValuePair<string, BinaryMemoryDataDecodeContext[]> unionDecoder in supportData.UnionDecoders )
                {
                    if  ( unionChoiceDefn.Name.Equals( unionDecoder.Key ) )
                    {
                        BinaryMemoryDataDecodeContext[] decoders = unionDecoder.Value ;
                        if  ( decoders[0].Field.Name.Equals( unionChoiceDefn.CtrlField ) )
                        {
                            int val = 0 ;

                            // Then want to decode the first field...
                            if  ( 0 < decoders[0].Field.BitLength )
                            {
                                val = BitManager.ReadInt(data.UInt8s, 0, decoders[0].Field.BitOffset, decoders[0].Field.BitLength);
                            }
                            else
                            {
                                // else go read the value "normally" and check
                                val = -1;
                            }

                            if  ( ( unionChoiceDefn.LowValue <= val )  &&  ( val <= unionChoiceDefn.HighValue ) )
                            {
                                done = true;
                                unionChoice = unionChoiceDefn;
                                unionDecoders = decoders;

                                foreach( UnionDefinition unionDefinition in supportData.UnionDefns )
                                {
                                    if  ( unionChoiceDefn.Name.Equals( unionDefinition.Name ) )
                                    {
                                        unionDefn = unionDefinition;
                                        break;
                                    }
                                }


                                break;
                            }
                        }

                        break;
                    }
                }

                if  ( done )
                    break;
            }


            string[] partialResults = new string[ unionDecoders.Length + unionDefn.RenderExtraFields.Count ] ;

            int posn = 0;
            int len = 0;

            for( int i = 0 ;   i < unionDecoders.Length ;   i++ )
            {
// For now, do not allow unions within unions...
                if  ( !string.IsNullOrEmpty( unionDecoders[i].Field.UnionSetName ) )
                {
throw new Exception( "Unions within Unions NOT YET ALLOWED" ) ;
                    /*
                    ByteConverterData rslt = new ByteConverterData( unionDecoders[i].RequestedBytes ) ;
                    for( int j = 0 ;   j < unionDecoders[i].RequestedBytes ;   j++ )
                    {
                        rslt.UInt8s[j] = br.ReadByte();
                    }
                    posn += unionDecoders[i].RequestedBytes;

                    // Now go decode this union....
                    // partialResults[ i + 1 ] = UnionDataDecoder( rslt, new UnionDataDecoderSupportData() { UnionDecoders = UnionDecoders, UnionSetName = unionDecoders[i].Field.UnionSetName } ) ;
                    */
                }
                else
                {
// Still not handling bit positions properly.

                    string rsltFormat = unionDecoders[i].OuterFormat ?? "{0}" ;
                    if  ( 0 < unionDecoders[i].Field.BitLength )
                    {
                        partialResults[i + unionDefn.RenderExtraFields.Count] = string.Format(rsltFormat, BitManager.ReadInt(data.UInt8s, posn, unionDecoders[i].Field.BitOffset, unionDecoders[i].Field.BitLength));
                        int byteSteps = 0;
                        int bitSteps = unionDecoders[i].Field.BitOffset + unionDecoders[i].Field.BitLength ;
                        while( bitSteps >= 8 )
                        {
                            posn++;
                            bitSteps -= 8;
                        }
                    }
                    else
                    {
                        partialResults[i + unionDefn.RenderExtraFields.Count] = string.Format(rsltFormat, unionDecoders[i].ReadDelegate(data.UInt8s, posn, unionDecoders[i].RequestedBytes, unionDecoders[i].Decoder, unionDecoders[i].SupportData, unionDecoders[i].Format, out len));
                        posn += len;
                    }
                }
            }

            for( int i = 0 ;   i <  unionDefn.RenderExtraFields.Count ;   i++ )
            {
                partialResults[i] = EvaluatePseudoFormat( unionDefn.RenderExtraFields[i], partialResults, unionDefn.RenderExtraFields.Count ) ;
            }

            sbBody.Append( String.Format( "[ {0} ]", string.Join( ",", partialResults ) ) ) ;

            
            return( sbBody.ToString() ) ;
        }


        static public string EvaluatePseudoFormat( RenderExtraDefinition extraDefinition, string[] partialResults, int offset )
        {
            string preFormat = extraDefinition.PreFormat;
            string indexedFields = extraDefinition.Elements;
            string[] elms = indexedFields.Split( ',' ) ;
            int[] elmIndexes = new int[ elms.Length ] ;

            int tmpInt;
            int cnt = 0;
            foreach( string elm in elms )
            {
                if  ( Int32.TryParse( elm, out tmpInt ) )
                {
                    elmIndexes[ cnt++ ] = tmpInt ;
                }
                else
                {
                    elmIndexes[ cnt++ ] = -1 ;
                }
            }

            string replacedStr = Regex.Replace( preFormat, @"%\d+%",
                delegate(Match match)
                {
                    string v = match.ToString();
                    string v2 = v.Substring( 1, v.Length - 2 ) ;
                    int tmpInt2;
                    if  ( Int32.TryParse( v2, out tmpInt2 ) )
                    {
                        return( partialResults[ offset + tmpInt2 ] ) ;
                    }
                    return( v ) ;
                }
            ) ;

            string replacedStr2 = Regex.Replace( replacedStr, @"{\d+}",
                delegate(Match match)
                {
                    string v = match.ToString();
                    string v2 = v.Substring( 1, v.Length - 2 ) ;
                    int tmpInt2;
                    if  ( Int32.TryParse( v2, out tmpInt2 ) )
                    {
                        return( partialResults[ offset + elmIndexes[ tmpInt2 ] ] ) ;
                    }
                    return( v ) ;
                }
            ) ;

// Only allowing for integers to be formatted.
            string replacedStr3 = Regex.Replace( replacedStr2, @"{\d+:.*}",
                delegate(Match match)
                {
                    string v = match.ToString();
                    int posn = v.IndexOf( ":" ) ;   // know it must exist due to the regex match
                    string v2 = v.Substring( 1, posn - 1 ) ;
                    int tmpInt2;
                    if  ( Int32.TryParse( v2, out tmpInt2 ) )
                    {
                        return( string.Format( v, Int32.Parse( partialResults[ offset + elmIndexes[ tmpInt2 ] ] ) ) ) ;
                    }
                    return( v ) ;
                }
            ) ;


            string rslt = EvaluatePseudoFormatCond( replacedStr3, partialResults, offset ) ;

            return( rslt ) ;
        }

        // Only intending to handle strings of format "(1==%2%?A:B)" or nested "(1==%2%?(2==%4%?A:B):(2==%4%?C:D))"
        // Note: no intended nesting within the first left-hand conditional
        static public string EvaluatePseudoFormatCond( string preFormat, string[] partialResults, int offset )
        {
            string rslt = "";
            
            while( preFormat != rslt )
            {
                rslt = preFormat ;
                preFormat = Regex.Replace( rslt, @"\(.*==.*?.*:.*\)",
                    delegate(Match match)
                    {
                        string v = match.ToString();
                        string v1 = v.Substring(1) ;

                        string v2 = EvaluatePseudoFormatCond( v1, partialResults, offset ) ;

                        if  ( v1 == v2 )
                        {
                            int posn2 = v2.IndexOf( "==" ) ;
                            int posn3 = v2.IndexOf( "?", posn2 ) ;
                            int posn4 = v2.IndexOf( ":", posn3 ) ;
                            int posn5 = v2.IndexOf( ")", posn3 ) ;
                            string leftSide = v2.Substring( 0, posn2 ) ;
                            string rightSide = v2.Substring( posn2 + 2, posn3 - posn2 - 2 ) ;
                            string trueAns = v2.Substring( posn3 + 1, posn4 - posn3 - 1 ) ;
                            string falseAns = v2.Substring( posn4 + 1, posn5 - posn4 - 1 ) ;
                            string remainder = "";
                            if  ( v2.Length > posn5 )
                            {
                                remainder = v2.Substring( posn5 + 1 ) ;
                            }

                            if  ( leftSide == rightSide )
                            {
                                return( trueAns + remainder ) ;
                            }
                            else
                            {
                                return( falseAns + remainder ) ;
                            }
                        }
                        else
                        {
                            return( "(" + v2 ) ;
                        }
                    }
                );
            }

            return( preFormat ) ;
        }




        static public void LoadLogFile( string src, LogDefinition logDefn, RenderDefinition renderDefn, out string headJSON, out string bodyJSON )
        {
            BinaryDataDecodeContext[] HeaderDecoders = PrepDecoders( logDefn.Header.Fields, renderDefn.RenderFields ) ;
            BinaryDataDecodeContext[] BodyDecoders = PrepDecoders( logDefn.Fields, renderDefn.RenderFields ) ;

            List<KeyValuePair<string,BinaryMemoryDataDecodeContext[]>> UnionDecoders = new List<KeyValuePair<string,BinaryMemoryDataDecodeContext[]>>() ;
            foreach( UnionDefinition unionDefn in logDefn.Unions )
            {
                BinaryMemoryDataDecodeContext[] unionDecoder = PrepUnionDecoders( unionDefn.Fields, unionDefn.RenderFields ) ;
                UnionDecoders.Add( new KeyValuePair<string, BinaryMemoryDataDecodeContext[]>( unionDefn.Name, unionDecoder ) ) ;
            }



            StringBuilder sbHead = new StringBuilder();
            StringBuilder sbBody = new StringBuilder();

            using( BinaryReader br = new BinaryReader( File.Open( src, FileMode.Open ) ) )
            {
                int cnt = 0;
                int len ;
                int posn = 0;
                int length = (int) br.BaseStream.Length;

                string[] partialResults = new string[ HeaderDecoders.Length + 1 ] ;

// TODO: beef this up to include the estimated length of all of the header fields
                if  ( posn < br.BaseStream.Length )
                {
                    partialResults[ 0 ] = cnt.ToString() ;
                    for( int i = 0 ;   i < HeaderDecoders.Length ;   i++ )
                    {
                        // DO NOT EXPECT ANY UNIONS WITHIN THE HEADER STRUCTURES AT THE MOMENT
                        string rsltFormat = HeaderDecoders[i].OuterFormat ?? "{0}" ;
                        partialResults[i+1] = string.Format( rsltFormat, HeaderDecoders[i].ReadDelegate( br, HeaderDecoders[i].RequestedBytes, HeaderDecoders[i].Decoder, HeaderDecoders[i].SupportData, HeaderDecoders[i].Format, out len ) ) ;
                        posn += len;
                    }
                    sbHead.Append( String.Format( "[ {0} ]", string.Join( ",", partialResults ) ) ) ;
                }
                headJSON = sbHead.ToString() ;

                partialResults = new string[ BodyDecoders.Length + 1 ] ;
// TODO: beef this up to include the estimated length of all of the body fields
                while( posn < br.BaseStream.Length )
                {
                    partialResults[ 0 ] = cnt.ToString() ;
                    for( int i = 0 ;   i < BodyDecoders.Length ;   i++ )
                    {
                        if  ( !string.IsNullOrEmpty( BodyDecoders[i].Field.UnionSetName ) )
                        {
                            ByteConverterData rslt = new ByteConverterData( BodyDecoders[i].RequestedBytes ) ;
                            for( int j = 0 ;   j < BodyDecoders[i].RequestedBytes ;   j++ )
                            {
                                rslt.UInt8s[j] = br.ReadByte();
                            }
                            posn += BodyDecoders[i].RequestedBytes;

                            // Now go decode this union....
                            partialResults[ i + 1 ] = UnionDataDecoder( rslt,
                                new UnionDataDecoderSupportData()
                                    {
                                            UnionSets = logDefn.UnionSets,
                                            UnionDefns = logDefn.Unions,
                                            UnionDecoders = UnionDecoders,
                                            UnionSetName = BodyDecoders[i].Field.UnionSetName
                                    } ) ;
                        }
                        else
                        {
                            
                            string rsltFormat = BodyDecoders[i].OuterFormat ?? "{0}" ;
                            partialResults[i+1] = string.Format( rsltFormat, BodyDecoders[i].ReadDelegate( br, BodyDecoders[i].RequestedBytes, BodyDecoders[i].Decoder, BodyDecoders[i].SupportData, BodyDecoders[i].Format, out len ) ) ;
                            posn += len;
                        }
                    }

                    sbBody.Append( String.Format("{0} [ {1} ]", (0 == cnt++ ? "" : ",\n "), string.Join( ",", partialResults ) ) ) ;
                }
                bodyJSON = string.Format("[ {0} ]", sbBody.ToString() ) ;
            }
        }
    }
}
