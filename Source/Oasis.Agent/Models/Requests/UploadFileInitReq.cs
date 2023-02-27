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
    public class UploadFileInitReq : AuthenticationBase
    {
        public DeviceId DeviceID { get; set; } = default;

        public long ChunkSize { get; set; } = default;

        public int ChunkCount { get; set; } = default;

        public string FileType { get; set; } = default;

        public string SubFileType { get; set; } = default;

        public string Date { get; set; } = default;

        public string OriginalFileName { get; set; } = default;

        public string OriginalFileAbsolutePath { get; set; } = default;

        public string OriginalFileDigitalSig { get; set; } = default;
    }
}