namespace Covidien.Ipi.InformaticsCore
{
    public class BitManager
    {
        static byte[] bitMasks = new byte[] { (byte) 0x00, (byte) 0x01, (byte) 0x03, (byte) 0x07, (byte) 0x0f, (byte) 0x1f, (byte) 0x3f, (byte) 0x7f, (byte) 0xff } ;

        static public int ReadInt( byte[] buffer, int byteOffset, int bitOffset, int numBits )
        {
            int val = 0 ;
            int bitEndPosn = bitOffset + numBits ;
            if  ( bitEndPosn <= 8 )
            {
                val = ( ( ( bitMasks[ numBits ] << ( 8 - bitEndPosn ) )  &  buffer[ byteOffset ] ) >> ( 8 - bitEndPosn ) ) ;
            }
            else
            {
                int bitsUsed = 8 - bitOffset ;
                numBits -= bitsUsed ;
                val = ( bitMasks[ bitsUsed ]  &  buffer[ byteOffset++ ] ) ;

                while( numBits >= 8 )
                {
                    val = (val <<= 8) + buffer[byteOffset++];
                    numBits -= 8;
                }
                
                if  ( 0 < numBits )
                {
                    val = (val << numBits) + (((bitMasks[numBits] << (8 - numBits)) & buffer[byteOffset]) >> (8 - numBits));
                }
            }

            return( val ) ;
        }
    }
}
