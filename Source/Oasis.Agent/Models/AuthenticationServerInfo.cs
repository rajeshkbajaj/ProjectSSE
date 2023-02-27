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

    public class AuthenticationServerInfo
    {
        [JsonProperty("iamServerUrl")]
        public string IAMServerUrl { get; set; } = default;

        [JsonProperty("ciamServerUrl")]
        public string CIAMServerUrl { get; set; } = default;

        [JsonProperty("iamClientId")]
        public string IAMClientId { get; set; } = default;

        [JsonProperty("ciamClientId")]
        public string CIAMClientId { get; set; } = default;

        public string CallbackUrl { get; set; } = default;
    };
}
