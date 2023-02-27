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
    public class CreateSessionResp : ResultBase
    {
        /// <summary>
        /// Session details
        /// </summary>
        public Session ExternalSystemSession { get; set; } = default;

        public AuthenticationServerInfo AuthServerInfo { get; set; } = default;

        /// <summary>
        /// Version of the OASIS Agent
        /// </summary>
        public string AgentVersion { get; set; } = default;

        /// <summary>
        /// ISO 8601 formatted timestamp representing when the session was created on the OASIS Agent
        /// </summary>
        public string TimeStamp { get; set; } = default;

        /// <summary>
        /// Path the logs that OASIS Agent keeps its logs.  Not used by Ess
        /// </summary>
        public string OasisLogPath { get; set; } = default;

        /// <summary>
        /// URL for the OASIS Server.  Not used by Ess
        /// </summary>
        public string ServerUrl { get; set; } = default;
    }
}