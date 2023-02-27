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
    using System;
    using System.Net.Http;
    using System.Security.Cryptography.X509Certificates;

    using Utilties;

    using Newtonsoft.Json;

    using Oasis.Agent.Exceptions;
    using Oasis.Agent.Interfaces;

    public class SessionManager : ISessionManager
    {
        private readonly IRestHelper restHelper;
        private readonly JsonSerializerSettings jsonSerializerSettings;
        private readonly EssSettings settings;
        private AgentInterface agentInterface = default;

        /// <summary>
        /// Creates an instance of the Agent Session Manager
        /// </summary>
        /// <param name="options">Settings</param>
        /// <param name="restHelper">REST helper implementation used to make actual REST calls</param>
        /// <param name="jsonSerializerSettings">JSON serialization / deserialization settings</param>
        public SessionManager(EssSettings options, IRestHelper restHelper, JsonSerializerSettings jsonSerializerSettings)
        {
            settings = options;
            this.restHelper = restHelper;
            this.jsonSerializerSettings = jsonSerializerSettings;           
        }


        public void CloseSession()
        {
            agentInterface.CloseSession();
            agentInterface = null;            
        }

        public bool IsRSASessionOpened()
        {
            return agentInterface != null;
        }

        /// <summary>
        /// Establishes a session with the OASIS Agent
        /// </summary>
        /// <returns><see cref="IDisposable"/> <see cref="IAgentInterface"/> that is used to communication with the OASIS Agent</returns>
        public IAgentInterface EstablishSession(OasisCredentialInformation oasisCredentialInformation,
                X509Certificate2 clientCertificate = null,
                Func<HttpRequestMessage, X509Certificate2, X509Chain, System.Net.Security.SslPolicyErrors, bool> serverCertificateCustomValidationCallback = null)
        {
            // Create the agent interface if not exists.
            if (agentInterface == null)
            {
                agentInterface = new AgentInterface(restHelper, clientCertificate, settings, jsonSerializerSettings, serverCertificateCustomValidationCallback);
            }

            if (agentInterface.AuthorizeResponse == null ||
                agentInterface.AuthorizeResponse.Result.Success == false)
            {
                var createSessionResponse = agentInterface.CreateSession(oasisCredentialInformation.EssClientAppId);

                //IEnumerable<string> associatedDevices = null;

                if (createSessionResponse.Success)
                {
                    agentInterface.CreateSessionResponse = createSessionResponse.Value;

                    return agentInterface;
                }
                else
                {
                    throw new CouldNotEstablishSessionException(createSessionResponse.Message);
                }
            }
            else
            {
                return agentInterface;
            }
        }
    }
}