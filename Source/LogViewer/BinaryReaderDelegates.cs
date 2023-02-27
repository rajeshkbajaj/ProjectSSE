using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LogViewer
{
    public delegate string ReadBinaryData( BinaryReader br, int reqBytes, LogDataDecoder decoder, object supportData, string format, out int length ) ;
    public delegate string ReadBinaryMemoryData( byte[] memBytes, int offset, int reqBytes, LogDataDecoder decoder, object supportData, string format, out int length ) ;

    public class BinaryDataDecodeContext
    {
        public FieldDefinition Field;
        public ReadBinaryData ReadDelegate;
        public int RequestedBytes;
        public LogDataDecoder Decoder;
        public object SupportData;
        public string Format;
        public string OuterFormat;
    }

    public class BinaryMemoryDataDecodeContext
    {
        public FieldDefinition Field;
        public ReadBinaryMemoryData ReadDelegate;
        public int RequestedBytes;
        public LogDataDecoder Decoder;
        public object SupportData;
        public string Format;
        public string OuterFormat;
    }


    public class BinaryReaderDelegates
    {
        public static string DecodeConsumeUInt8( ByteConverterData rslt, int reqBytes, LogDataDecoder decoder, object supportData, string format )
        {
            if  ( null != decoder )
            {
                return( decoder( rslt, supportData ) ) ;
            }
            else
            {
                string rsltStr = "" ;
                if  ( null == format )
                {
                    for( int i = 0 ;   i < reqBytes ;   i++ )
                    {
                        rsltStr += rslt.UInt8s[i].ToString() ;
                    }
                }
                else
                {
                    for( int i = 0 ;   i < reqBytes ;   i++ )
                    {
                        rsltStr += string.Format( format, rslt.UInt8s[i] ) ;
                    }
                }
                return( rsltStr ) ;
            }
        }

        public static string ReadUInt8( BinaryReader br, int reqBytes, LogDataDecoder decoder, object supportData, string format, out int length )
        {
            const int unitLength = 1;
            ByteConverterData rslt = new ByteConverterData( reqBytes ) ;

            for( int i = 0 ;   i < reqBytes ;   i++ )
            {
                rslt.UInt8s[i] = br.ReadByte();
            }
            length = reqBytes ;

            return( DecodeConsumeUInt8( rslt, reqBytes, decoder, supportData, format ) ) ;
        }

        public static string ReadUInt8( byte[] memBytes, int offset, int reqBytes, LogDataDecoder decoder, object supportData, string format, out int length )
        {
            const int unitLength = 1;
            ByteConverterData rslt = new ByteConverterData( reqBytes ) ;

            Buffer.BlockCopy( memBytes, offset, rslt.UInt8s, 0, reqBytes ) ;
            length = reqBytes ;

            return( DecodeConsumeUInt8( rslt, reqBytes, decoder, supportData, format ) ) ;
        }



        public static string DecodeConsumeUInt16( ByteConverterData rslt, int reqBytes, LogDataDecoder decoder, object supportData, string format )
        {
            const int unitLength = 2;
            if (null != decoder)
            {
                return( decoder( rslt, supportData ) ) ;
            }
            else
            {
                string rsltStr = "";
                if  ( null == format )
                {
                    for( int i = 0 ;   i < reqBytes / unitLength ;   i++ )
                    {
                        rsltStr += rslt.UInt16s[i].ToString();
                    }
                }
                else
                {
                    for( int i = 0 ;   i < reqBytes / unitLength ;   i++ )
                    {
                        rsltStr += string.Format(format, rslt.UInt16s[i]);
                    }
                }
                return( rsltStr ) ;
            }
        }

        public static string ReadUInt16( BinaryReader br, int reqBytes, LogDataDecoder decoder, object supportData, string format, out int length )
        {
            const int unitLength = 2;
            ByteConverterData rslt = new ByteConverterData( reqBytes ) ;

            length = 0 ;
            for( int i = 0 ;   i < reqBytes / unitLength ;   i++ )
            {
                UInt16 i1 = (UInt16)br.ReadByte();
                UInt16 i2 = (UInt16)br.ReadByte();
                rslt.UInt16s[i] = (UInt16) ((i1 << 8) + i2) ;
                length += unitLength ;
            }

            return( DecodeConsumeUInt16( rslt, reqBytes, decoder, supportData, format ) ) ;
        }

        public static string ReadUInt16( byte[] memBytes, int offset, int reqBytes, LogDataDecoder decoder, object supportData, string format, out int length )
        {
            const int unitLength = 1;
            ByteConverterData rslt = new ByteConverterData( reqBytes ) ;

            Buffer.BlockCopy( memBytes, offset, rslt.UInt8s, 0, reqBytes ) ;
            length = reqBytes ;

            return( DecodeConsumeUInt16( rslt, reqBytes, decoder, supportData, format ) ) ;
        }



        public static string DecodeConsumeUInt32( ByteConverterData rslt, int reqBytes, LogDataDecoder decoder, object supportData, string format )
        {
            const int unitLength = 4;
            if  ( null != decoder )
            {
                return( decoder( rslt, supportData ) ) ;
            }
            else
            {
                string rsltStr = "" ;
                if  ( null == format )
                {
                    for( int i = 0 ;   i < reqBytes / unitLength ;   i++ )
                    {
                        rsltStr += rslt.UInt32s[i].ToString();
                    }
                }
                else
                {
                    for( int i = 0 ;   i < reqBytes / unitLength ;   i++ )
                    {
                        rsltStr += string.Format(format, rslt.UInt32s[i]);
                    }
                }
                return( rsltStr ) ;
            }
        }

        public static string ReadUInt32( BinaryReader br, int reqBytes, LogDataDecoder decoder, object supportData, string format, out int length )
        {
            const int unitLength = 4;
            ByteConverterData rslt = new ByteConverterData( reqBytes ) ;

            length = 0 ;
            for( int i = 0 ;   i < reqBytes / unitLength ;   i++ )
            {
                UInt32 i1 = (UInt32) br.ReadByte();
                UInt32 i2 = (UInt32) br.ReadByte();
                UInt32 i3 = (UInt32) br.ReadByte();
                UInt32 i4 = (UInt32) br.ReadByte();
                UInt32 i5 = (UInt32) ((((((i1 << 8) + i2) << 8) + i3) << 8) + i4);
                rslt.UInt32s[i] = i5;
                length += unitLength ;
            }

            return( DecodeConsumeUInt32( rslt, reqBytes, decoder, supportData, format ) ) ;
        }

        public static string ReadUInt32( byte[] memBytes, int offset, int reqBytes, LogDataDecoder decoder, object supportData, string format, out int length )
        {
            const int unitLength = 1;
            ByteConverterData rslt = new ByteConverterData( reqBytes ) ;

            Buffer.BlockCopy( memBytes, offset, rslt.UInt8s, 0, reqBytes ) ;
            length = reqBytes ;

            return( DecodeConsumeUInt32( rslt, reqBytes, decoder, supportData, format ) ) ;
        }



        public static string DecodeConsumeUInt64( ByteConverterData rslt, int reqBytes, LogDataDecoder decoder, object supportData, string format )
        {
            const int unitLength = 8;
            if  ( null != decoder )
            {
                return( decoder( rslt, supportData ) ) ;
            }
            else
            {
                string rsltStr = "" ;
                if  ( null == format )
                {
                    for( int i = 0 ;   i < reqBytes / unitLength ;   i++ )
                    {
                        rsltStr += rslt.UInt64s[i].ToString();
                    }
                }
                else
                {
                    for( int i = 0 ;   i < reqBytes / unitLength ;   i++ )
                    {
                        rsltStr += string.Format(format, rslt.UInt64s[i]);
                    }
                }
                return( rsltStr ) ;
            }
        }

        public static string ReadUInt64( BinaryReader br, int reqBytes, LogDataDecoder decoder, object supportData, string format, out int length )
        {
            const int unitLength = 8;
            ByteConverterData rslt = new ByteConverterData( reqBytes ) ;

            length = 0 ;
            for( int i = 0 ;   i < reqBytes / unitLength ;   i++ )
            {
                UInt64 val = 0 ;
                for( int j = 0 ;   j < unitLength ;   j++ )
                {
                    val = (val << 8) + (UInt64) br.ReadByte();
                }
                rslt.UInt64s[i] = val ;
                length += unitLength ;
            }

            return( DecodeConsumeUInt64( rslt, reqBytes, decoder, supportData, format ) ) ;
        }

        public static string ReadUInt64( byte[] memBytes, int offset, int reqBytes, LogDataDecoder decoder, object supportData, string format, out int length )
        {
            const int unitLength = 1;
            ByteConverterData rslt = new ByteConverterData( reqBytes ) ;

            Buffer.BlockCopy( memBytes, offset, rslt.UInt8s, 0, reqBytes ) ;    // shouldn't matter to plug into byte array vs UInt64 array, all start at same memory location
            length = reqBytes ;

            return( DecodeConsumeUInt64( rslt, reqBytes, decoder, supportData, format ) ) ;
        }

/*
        public static string ReadInt8(BinaryReader br, LogDataDecoder decoder, object supportData, out int length)
        {
//            int rslt = (int) br.ReadByte(); // need to test / correct as probably not doing a signed conversion
            length = 1;
            ByteConverterData rslt = new ByteConverterData() ;
            rslt.int8Val = br.ReadByte();
            if  ( null != decoder )
            {
                return( decoder( rslt, supportData ) ) ;
            }
            else
            {
                return( rslt.ToString() ) ;
            }
        }

        public static string ReadInt16(BinaryReader br, LogDataDecoder decoder, object supportData, out int length)
        {
            length = 2;
            ByteConverterData rslt = new ByteConverterData() ;
            rslt.int16Val = br.ReadByte();
            if  ( null != decoder )
            {
                return( decoder( rslt, supportData ) ) ;
            }
            else
            {
                return( rslt.ToString() ) ;
            }
        }

        public static string ReadInt32(BinaryReader br, LogDataDecoder decoder, object supportData, out int length)
        {
            length = 4;
            ByteConverterData rslt = new ByteConverterData() ;
            rslt.int32Val = br.ReadByte();
            if  ( null != decoder )
            {
                return( decoder( rslt, supportData ) ) ;
            }
            else
            {
                return( rslt.ToString() ) ;
            }
        }

        public static string ReadInt64(BinaryReader br, LogDataDecoder decoder, object supportData, out int length)
        {
            length = 8;
            ByteConverterData rslt = new ByteConverterData() ;
            rslt.int64Val = br.ReadByte();
            if  ( null != decoder )
            {
                return( decoder( rslt, supportData ) ) ;
            }
            else
            {
                return( rslt.ToString() ) ;
            }
        }
 */
    }
}
