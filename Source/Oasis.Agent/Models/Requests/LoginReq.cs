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

    public class LoginReq
    {
        public string User { get; set; } = default;

        public string Passcode { get; set; } = default;

        public string AccessToken { get; set; } = default;

        public string RefreshToken { get; set; } = default;
        
        public string ClientVersion { get; set; } = default;

        public string ClientHostName { get; set; } = default;

        public string ClientMachineId { get; set; } = default;

        public string AgentMachineId { get; set; } = default;

        public string SessionGUID { get; set; } = default;

        public IEnumerable<string> AutoESSSerialNumbers { get; set; } = default;

        public bool EnablePrepSteps { get; set; } = default;

        public ClientAppInfo Client { get; set; } = default;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LoginReq" /> class.
        /// </summary>
        public LoginReq()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LoginReq" /> class.
        /// </summary>
        /// <param name="usr"></param>
        /// <param name="pswd"></param>
        /// <param name="appId"></param>
        public LoginReq(string usr, string pswd, string appId, string clientVersion,
                        string clientHostName, string clientMachineId,
                        string sessionGUID, IEnumerable<string> autoESSSerialNumbers)
        {
            User = usr;
            Passcode = pswd;
            Client = new ClientAppInfo { AppID = appId };
            ClientVersion = clientVersion;
            ClientHostName = clientHostName;
            ClientMachineId = clientMachineId;
            SessionGUID = sessionGUID;
            AutoESSSerialNumbers = autoESSSerialNumbers;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LoginReq" /> class.
        /// </summary>
        /// <param name="usr"></param>
        /// <param name="pswd"></param>
        /// <param name="clntInfo"></param>
        public LoginReq(string usr, string pswd, ClientAppInfo clntInfo, string clientVersion,
                        string clientHostName, string clientMachineId,
                        string sessionGUID, IEnumerable<string> autoESSSerialNumbers)
        {
            User = usr;
            Passcode = pswd;
            Client = clntInfo;
            ClientVersion = clientVersion;
            ClientHostName = clientHostName;
            ClientMachineId = clientMachineId;
            SessionGUID = sessionGUID;
            AutoESSSerialNumbers = autoESSSerialNumbers;
        }
    }
}