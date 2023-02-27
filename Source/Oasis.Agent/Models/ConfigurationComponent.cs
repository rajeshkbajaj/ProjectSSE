// ------------------------------------------------------------------------------
//                    Copyright (c) 2021 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Oasis.Agent.Models
{
    using System.Collections.Generic;

    public class ConfigurationComponent
    {
        public string FileName { get; set; } = default;
        public string Md5 { get; set; } = default;
        public string Oid { get; set; } = default;
        public string Status { get; set; } = default;
        public string PartNumber { get; set; } = default;
        public string SerialNumber { get; set; } = default;
        public string ComparisonOrder { get; set; } = default;
        public string ComponentName { get; set; } = default;
        public string SoftwareRevision { get; set; } = default;
        public string Language { get; set; } = default;
        public string ComponentType { get; set; } = default;
        public bool LastestSoftwareUpgrade { get; set; } = default;
        public bool Resync { get; set; }
        public List<string> Right { get; set; } = default;
        public List<string> RegulatoryExclusion { get; set; } = default;
        public string Timestamp { get; set; } = default;
        public string SoftwareType { get; set; } = default;
    }
}