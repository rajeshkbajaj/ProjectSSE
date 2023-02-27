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
    public class SoftwaresReq : AuthenticationBase
    {
        public string DeviceType { get; set; }

        public ClientAppInfo ClientAppInfo { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SoftwaresReq" /> class.
        /// </summary>
        public SoftwaresReq()
        {
            Auth = new Authentication();
            DeviceType = string.Empty;
            ClientAppInfo = new ClientAppInfo();
        }
    }
}