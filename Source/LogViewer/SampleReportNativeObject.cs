using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LogViewer
{
    public class SampleReportNativeObject
    {
        public UInt16 u16val1;
        public UInt16 u16val2;
        public UInt32 u32val3;

        public SampleReportNativeObject( UInt16 v1, UInt16 v2, UInt32 v3 )
        {
            u16val1 = v1;
            u16val2 = v2;
            u32val3 = v3;
        }

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(u16val1);
                    writer.Write(u16val2);
                    writer.Write(u32val3);
                }
                return m.ToArray();
            }
        }
    }
}
