// ------------------------------------------------------------------------------
//                    Copyright (c) 2021 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Oasis.Agent.Models.Requests
{
    using System.Collections.Generic;

    public class UploadRunningCfgReq : AuthenticationBase
    {
        public DeviceId DeviceID { get; set; } = default;

        public string Region { get; set; } = default;

        public string Country { get; set; } = default;

        public string FacilityID { get; set; } = default;

        public SystemConfigs SystemConfigs { get; set; } = default;

        public ClientAppInfo ClientAppInfo { get; set; } = default;

        public List<DeviceSetting> DeviceSettings { get; set; } = default;
    }
}