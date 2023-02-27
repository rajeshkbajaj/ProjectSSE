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
    using System.Collections.Generic;
    using Covidien.CGRS.BinaryLogDecoder;

    public class LogCargoContainer
    {
        public string Name { get; set; }
        public string DevType { get; set; }
        public string DevId { get; set; }

        public bool IsRawSaved { get; set; }
        public bool IsOrigSaved { get; set; }

        public bool IsDecoded { get; set; }
        public bool IsDecodedSaved { get; set; }

        public string LogRawFilename { get; set; }
        public string LogDecodedFilename { get; set; }
        public string LogLocalStoreFileName { get; set; }

        public string LogXmlStr { get; set; }
        public string LogRawHeader { get; set; }
        public string LogRawBody { get; set; }
        public string LogXsltTransform { get; set; }
        public List<KeyValuePair<string, string>> LogRawMetaTags { get; set; }

        public byte[] LogRawBytes;  // likely will ultimately replace the logXmlStr/header/body/meta.

        public string DecodeType { get; set; }
        public string DecodeVersion { get; set; }

        public LogDefinition LogDefn { get; set; }
        public RenderDefinition RenderDefn { get; set; }

        public string LogDecodedHeader { get; set; }
        public string JsonHead { get; set; }
        public string RenderHeaderHead { get; set; }
        public string RenderHeaderBody { get; set; }

        public string LogDecodedBody { get; set; }
        public string JsonBody { get; set; }
        public string RenderBodyHead { get; set; }
        public string RenderBodyBody { get; set; }

    }
}
