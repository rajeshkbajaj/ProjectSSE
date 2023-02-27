// ------------------------------------------------------------------------------
//                    Copyright (c) 2021 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Oasis.Agent.Models.Responses
{
    using System.Collections.Generic;

    public class AuthorizeResp : ResultBase
    {
        public Session ExternalSystemSession { get; set; } = default;
        public Dictionary<string, string> DeviceTypeGUIDS { get; set; } = default;

        public List<Permission> ExternalSystemUserPermissions { get; set; } = default;

        public List<string> Training { get; set; } = default;

        public string Country { get; set; } = default;

        public string CountryName { get; set; } = default;

        public string UserFirstName { get; set; } = default;

        public string UserLastName { get; set; } = default;

        public bool Deactive { get; set; } = default;

        public string PasswordExpiration { get; set; } = default;

        public bool UserLocked { get; set; } = default;

        public bool CovidienUser { get; set; } = default;

        public string UserName { get; set; } = default;

        public bool IsPasscodeSet { get; set; } = default;

        public int CustomError { get; set; } = default;

        public string CurrentAgentVersion { get; set; } = default;

        public string CurrentVLEXVersion { get; set; } = default;
    }    
}
