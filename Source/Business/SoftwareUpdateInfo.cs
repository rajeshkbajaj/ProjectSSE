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

    public class SoftwarePackage
    {
        // public string Type { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Part { get; set; }
        public string Language { get; set; }
        public string Revision { get; set; }
        public string Length { get; set; }
        public string Location { get; set; }
        public string MD5Value { get; set; }
        public string SHA1Value { get; set; }
    }

    public class DocumentPackage
    {
        public string Name { get; set; }
        public string Part { get; set; }
        public string Revision { get; set; }
        public string Length { get; set; }
        public string MD5Value { get; set; }
        public string SHA1Value { get; set; }
        public string Location { get; set; }
        public string DecryptLocation { get; set; }
        public List<string> SoftwareLinks;
   }

    public class SoftwareUpdateInfo
    {
        public string Info { get; set; }
        public List<SoftwarePackage> SoftwarePackages { get; set; }
        public List<DocumentPackage> DocumentPackages { get; set; }
        public string NotificationId { get; set; }
        public string PertinentIdentifier { get; set; }
        public string PertinentType { get; set; }

        public SoftwareUpdateInfo()
        {
            SoftwarePackages = new List<SoftwarePackage>();
            DocumentPackages = new List<DocumentPackage>();
        }

    }
}
