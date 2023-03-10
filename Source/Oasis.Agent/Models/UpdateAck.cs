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
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    using Oasis.Agent.Models.Enums;

    public class UpdateAck
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public UpdateAckType AckType { get; set; } = UpdateAckType.Unknown;

        public string ScheduleJobGUID { get; set; } = default;

        public string Name { get; set; } = default;

        public string PartNumber { get; set; } = default;

        public string Revision { get; set; } = default;

        public string Timestamp { get; set; } = default;

        public string Status { get; set; } = default;

        public string StatusDetail { get; set; } = default;
    }
}