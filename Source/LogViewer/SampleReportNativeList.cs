using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LogViewer
{
    public class SampleReportNativeList
    {
        public List<SampleReportNativeObject> nativeData = new List<SampleReportNativeObject>() ;

        private int v1 = 0;
        private int v2 = 100;
        private int v3 = 1000;

        public void AddRow()
        {
            nativeData.Add( new SampleReportNativeObject( (UInt16) v1++, (UInt16) v2++, (UInt32) v3++ ) ) ;
        }

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                foreach( SampleReportNativeObject tmp in nativeData )
                {
                    byte[] tmpBytes = tmp.Serialize();
                    int offset = (int) m.Length;
                    int count = tmpBytes.Length;
                    m.Write(tmpBytes, 0, count );
                }
                return m.ToArray();
            }
        }
    }
}
