using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LogViewer
{
    public class EnumDataDecoder
    {

        public static string DecodeByteEnumHashtable(ByteConverterData data, object supportData)
        {
            Hashtable lookup = (Hashtable) supportData;
            if  ( lookup.ContainsKey( data.UInt8s[0] ) )
            {
                return( (string) lookup[ data.UInt8s[0] ] ) ;
            }
            else
            {
                return( "'ERR - NO MATCHING ENUM-U8 TO CODE " + (int)data.UInt8s[0] + "'" ) ;
            }
        }

        public static string DecodeUInt16EnumHashtable(ByteConverterData data, object supportData)
        {
            Hashtable lookup = (Hashtable) supportData;
            if  ( lookup.ContainsKey( data.UInt16s[0] ) )
            {
                return( string.Format( "'{0}'", (string) lookup[ data.UInt16s[0] ] ) ) ;
            }
            else
            {
                return( "'ERR - NO MATCHING ENUM-U16 TO CODE " + (int) data.UInt16s[0] + "'" ) ;
            }
        }

        public static string DecodeUInt32EnumHashtable(ByteConverterData data, object supportData)
        {
            Hashtable lookup = (Hashtable) supportData;
            if  ( lookup.ContainsKey( data.UInt32s[0] ) )
            {
                return( string.Format( "'{0}'", (string) lookup[ data.UInt32s[0] ] ) ) ;
            }
            else
            {
                return( "'ERR - NO MATCHING ENUM-U32 TO CODE " + (int) data.UInt32s[0] + "'" ) ;
            }
        }
/*
        public static string DecodeUInt64EnumHashtable(ByteConverterData data, object supportData)
        {
            Hashtable lookup = (Hashtable) supportData;
            if  ( lookup.ContainsKey( data.UInt64s[0] ) )
            {
                return( (string) lookup[ data.UInt64s[0] ] ) ;
            }
            else
            {
                return( "ERR - NO MATCHING ENUM-U64 TO CODE " + (int) data.UInt64s[0] ) ;
            }
        }



        public static string DecodeInt16EnumHashtable(ByteConverterData data, object supportData)
        {
            Hashtable lookup = (Hashtable)supportData;
            if (lookup.ContainsKey(data.Int16s[0]))
            {
                return ((string)lookup[data.Int16s[0]]);
            }
            else
            {
                return ("ERR - NO MATCHING ENUM-I16 TO CODE " + (int)data.Int16s[0]);
            }
        }

        public static string DecodeInt32EnumHashtable(ByteConverterData data, object supportData)
        {
            Hashtable lookup = (Hashtable)supportData;
            if (lookup.ContainsKey(data.Int32s[0]))
            {
                return ((string)lookup[data.Int32s[0]]);
            }
            else
            {
                return ("ERR - NO MATCHING ENUM-I32 TO CODE " + (int)data.Int32s[0]);
            }
        }

        public static string DecodeInt64EnumHashtable(ByteConverterData data, object supportData)
        {
            Hashtable lookup = (Hashtable)supportData;
            if (lookup.ContainsKey(data.Int64s[0]))
            {
                return ((string)lookup[data.Int64s[0]]);
            }
            else
            {
                return ("ERR - NO MATCHING ENUM-I64 TO CODE " + (int)data.Int64s[0]);
            }
        }
*/
    }
}
