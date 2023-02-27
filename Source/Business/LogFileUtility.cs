// ------------------------------------------------------------------------------
//                    Copyright (c) 2022 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Covidien.CGRS.PcAgentInterfaceBusiness
{
    using System.Xml.Xsl;
    using System.IO;
    using System.IO.Compression;

    public static class LogFileUtility
    {
        public static void SaveStringToFile(string sourceString, string resultFilePath)
        {
            using (MemoryStream memStream = new MemoryStream(System.Text.Encoding.ASCII.GetBytes(sourceString)))
            {
                using (FileStream stream = new FileStream(resultFilePath, FileMode.Create))
                {
                    stream.Write(memStream.ToArray(), 0, (int)memStream.Length);
                }
            }
        }

        public static void ApplyXSLTransformationOnFile(string xslTransformFilePath, string sourceFilePath, string resultFilePath)
        {
            // Load the style sheet and execute the transform
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(xslTransformFilePath);
            xslt.Transform(sourceFilePath, resultFilePath);
        }

        public static void XSLTransformIntoHTMLGzipFile(string xslTransformFilePath, string sourceFilePath, string resultFilePath)
        {
            // Load the style sheet and execute the transform
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(xslTransformFilePath);

            //public void Transform(string inputUri, XsltArgumentList arguments, Stream results);
            using (FileStream outFile = new FileStream(resultFilePath, FileMode.Create))
            {
                using (GZipStream gz = new GZipStream(outFile, CompressionMode.Compress, false))
                {
                    xslt.Transform(sourceFilePath, null, gz);
                }
            }
        }

        public static void GzipAFile(string sourceFilePath, string resultFilePath)
        {
            using (FileStream inFile = File.OpenRead(sourceFilePath))
            {
                using (FileStream outFile = new FileStream(resultFilePath, FileMode.Create))
                {
                    using (GZipStream gz = new GZipStream(outFile, CompressionMode.Compress, false))
                    {
                        inFile.CopyTo(gz);
                    }
                }
            }
        }

        public static void GunzipFile(string sourceFilePath, string resultFilePath)
        {
            using (FileStream inFile = File.OpenRead(sourceFilePath))
            {
                using (FileStream outFile = new FileStream(resultFilePath, FileMode.Create))
                {
                    using (GZipStream gz = new GZipStream(inFile, CompressionMode.Decompress, false))
                    {
                        gz.CopyTo(outFile);
                    }
                }
            }
        }
    };

}
