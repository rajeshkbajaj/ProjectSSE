using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Covidien.CGRS.BinaryLogDecoder
{
    public delegate string LogDataDecoder( ByteConverterData data, object supportData ) ;

    [StructLayout(LayoutKind.Explicit)]
    public class ByteConverterData
    {
        public ByteConverterData() { UInt64s = new UInt64[1] ; }
        public ByteConverterData( int numBytes ) { UInt8s = new Byte[ numBytes ] ; }
        // Note: in my C-days I would have allocated against a 64-bit entity (e.g., UInt64), but C# seems to access the
        // array index first, and then consider the field offset, causing an array index error if we attempt to index
        // the bytes on the allocation of UInt64.  However, the opposite direction seems to work OK, without data corruption.

/*
        [FieldOffset(0)]
        public Byte byteVal ;

        [FieldOffset(0)]
        public Byte uint8Val;
*/
/*
        [FieldOffset(0)]
        public UInt16 uint16Val;

        [FieldOffset(0)]
        public UInt32 uint32Val;
*/ 
/*
        [FieldOffset(0)]
        public UInt64 uint64Val;

        [FieldOffset(0)]
        public Byte int8Val;   // this may just be problematic with a byte as underlying class - probably not getting negative values

        [FieldOffset(0)]
        public Int16 int16Val;

        [FieldOffset(0)]
        public Int32 int32Val;

        [FieldOffset(0)]
        public Int64 int64Val;

        [FieldOffset(0)]
        public Single singleVal;

        [FieldOffset(0)]
        public Double doubleVal;


        [FieldOffset(0)]
        public Byte[] Bytes;
*/
        [FieldOffset(0)]
        public Byte[] UInt8s;

        [FieldOffset(0)]
        public UInt16[] UInt16s;

        [FieldOffset(0)]
        public UInt32[] UInt32s;

        [FieldOffset(0)]
        public UInt64[] UInt64s;
/*
        [FieldOffset(0)]
        public Byte[] Int8s;

        [FieldOffset(0)]
        public Int16[] Int16s;

        [FieldOffset(0)]
        public Int32[] Int32s;

        [FieldOffset(0)]
        public Int64[] Int64s;

        [FieldOffset(0)]
        public Single[] Singles;

        [FieldOffset(0)]
        public Double[] Doubles;
*/
    } 


    public class BinaryDataDecoder
    {
        public static string DecodeAlarmLogErrType(ByteConverterData data, object supportData)
        {
            // support data might include a variety of types
            // this code knows the bit packing to be performed

            return( "DecodeAlarmLogErrTye" ) ;
        }
    }
}
