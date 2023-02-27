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
    public class ClientCertificateReq : AuthenticationBase
    {
        public int? Id { get; set; }

        public string ClientHostName { get; set; } = default;

        public string ClientHostId { get; set; } = default;

        public string ClientType { get; set; } = default;

        public string ClientVersion { get; set; } = default;

        public string InstallStatus { get; set; } = default;

        public string InstallStatusDetail { get; set; } = default;

        public ClientAppInfo ClientAppInfo { get; set; } = default;
    }
}