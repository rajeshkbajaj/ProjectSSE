// ------------------------------------------------------------------------------
//                    Copyright (c) 2021 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Oasis.Agent
{
    public class OasisCredentialInformation
    {
        public string EssClientAppId { get; set; } = default;
        public string EssAgentMachineId { get; set; } = default;
        public string EssVersion { get; set; } = default;
        public string HostName { get; set; } = default;
        public string HostId { get; set; } = default;
    }
}