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

    public class Software
    {
        public string Name { get; set; } = default;

        public string PartNumber { get; set; } = default;

        public string Revision { get; set; } = default;

        public string CRC { get; set; } = default;

        public string ComparisonOrder { get; set; } = default;

        public string Language { get; set; } = default;

        public string Status { get; set; } = default;

        public string Oid { get; set; } = default;

        public bool CriticalRelease { get; set; } = default;
        
        public long FileSize { get; set; } = default;

        public List<string> CountryExclusions { get; set; } = default;
    }
}