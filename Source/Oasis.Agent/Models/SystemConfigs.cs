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

    public class SystemConfigs
    {
        public List<Configuration> Configuration { get; set; } = default;
        public List<Feature> Features { get; set; } = default;
        public string DeviceTypeGuid { get; set; } = default;

        public string Name { get; set; } = default;
        public string Revision { get; set; } = default;
        public string Description { get; set; } = default;
        public string UpdateTime { get; set; } = default;
        public string Checksum { get; set; } = default;
        public string Status { get; set; } = default;
    }
}