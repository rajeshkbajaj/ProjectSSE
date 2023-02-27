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
    public class LicenseTerm
    {
        public string Type { get; set; } = default;// LimitedCount, LimitedDuration, Unlimited, null

        public int? Limit { get; set; } // optional

        public int? Used { get; set; } // optional

        public int Remaining { get; set; } = default;

        public string StartDateTime { get; set; } // optional

        public string EndDateTime { get; set; } // 2015-12-01 00:00:00
    }
}