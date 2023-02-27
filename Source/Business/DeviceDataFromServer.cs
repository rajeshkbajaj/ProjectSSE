// ------------------------------------------------------------------------------
//                    Copyright (c) 2022 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Covidien.CGRS.PcAgentInterfaceBusiness
{
    using System.Collections.Generic;

    public class DeviceDataFromServer
    {
        public string DeviceType { get; set; }
        public string SerialNumber { get; set; }

        // null if unknown (e.g., made request, no response yet); true/false are the usual
        public bool? Exists { get; set; }

        // number of outstanding requests
        public int OutstandingRequestCnt { get; set; }

        public DeviceInformation DeviceInfo { get; set; }

        public List<SoftwareUpdateInfo> SoftwareUpdates { get; set; }

        public DeviceDataFromServer( string devType, string serialNumber )
        {
            DeviceType = devType;
            SerialNumber = serialNumber;

            Exists = null;
            OutstandingRequestCnt = 0;
            SoftwareUpdates = new List<SoftwareUpdateInfo>(); // just so we don't have to manage creation elsewhere
        }
    }
}
