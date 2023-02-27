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
    using System.ComponentModel;

    public class ServerStatusResp : ResultBase
    {
        public bool Connected { get; set; }

        [Description("agent_version")]
        public string AgentVersion { get; set; } = default;

        public RemainingTime RemainingTime { get; set; } = default;
        public RemainingSize RemainingSize { get; set; } = default;
        public TotalSize TotalSize { get; set; } = default;
        public PackageStatus PackageStatus { get; set; } = default;
        public UsedSpace UsedSpace { get; set; } = default;

        [Description("server_version")]
        public string ServerVersion { get; set; } = default;

        public string UploadSize { get; set; } = default;
        public string UploadSizeRemaining { get; set; } = default;
        public string UploadTime { get; set; } = default;
        public string UploadTimeRemaining { get; set; } = default;
        public string CacheTimeOut { get; set; } = default;
        public string FreeSpace { get; set; } = default;
        public string LastUpdate { get; set; } = default;
        public string InsufficientSpace { get; set; } = default;
        public string ServerAvailable { get; set; } = default;
        public string SessionStatus { get; set; } = default;
        public string LoggedOnStatus { get; set; } = default;
        public string DataReady { get; set; } = default;
        public string JobStatus { get; set; } = default;
        public string AgentStatus { get; set; } = default;
        public string DateTime { get; set; } = default;
        public string TimeStamp { get; set; } = default;
    }
}