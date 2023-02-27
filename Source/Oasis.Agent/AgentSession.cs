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
    using System.Net;
    using System.Net.Http;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    using Utilties;

    using Newtonsoft.Json;

    using Oasis.Agent.Exceptions;
    using Oasis.Agent.Interfaces;
    using Oasis.Agent.Models;
    using Oasis.Agent.Models.Requests;
    using Oasis.Agent.Models.Responses;
    using Oasis.Common;
    using Serilog;

    /// <summary>
    /// Represents a session with the OASIS Agent
    /// All operations against the Agent must be run through a session, and as such should come through this class.
    /// This class is created by the SessionManager.
    /// </summary>
    public class AgentSession : IDisposable, IAgentSession
    {
        private X509Certificate2 clientCertificate;

        private readonly EssSettings settings;
        private readonly JsonSerializerSettings jsonSerializerSettings;
        private readonly string oasisHostName;
        private readonly HttpClient httpClient;
        private readonly HttpClientHandler httpClientHandler;
        private readonly IRestHelper restHelper;
        private bool disposed;

        public string EssClientAppId { get; set; } = default;

        /// <summary>
        /// Session establishment time, if a session is created
        /// </summary>
        public DateTime? SessionEstablishedTimeStamp { get; private set; } = null;

        /// <summary>
        /// The session ID that should be used in the Token field of <see cref="Models.Authentication"/> in any subsequent calls to the agent
        /// </summary>
        public string SessionId { get; private set; } = default;

        public AuthorizeResp AuthorizeResponse { get; internal set; } = default;

        public CreateSessionResp CreateSessionResponse { get; internal set; } = default;

        public bool IsAuthenticationCompleted()
        {
            if (AuthorizeResponse?.Result?.Success == true)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Creates a new Agent Session
        /// </summary>
        /// <param name="restHelper">REST helper implmentation</param>
        /// <param name="clientCertificate">Client certificate to use for this session</param>
        /// <param name="settings">Auto ESS Settings</param>
        /// <param name="jsonSerializerSettings">JSON serialization / deserialization settings</param>
        /// <remarks>
        /// After obtaining a client certificate in a session that does not have one assigned, the session must be ended and a new session begun using the certificate.
        /// </remarks>
        public AgentSession(IRestHelper restHelper,
            X509Certificate2 clientCertificate,
            EssSettings settings,
            JsonSerializerSettings jsonSerializerSettings,
            Func<HttpRequestMessage, X509Certificate2, X509Chain, System.Net.Security.SslPolicyErrors, bool> serverCertificateCustomValidationCallback = null)
        {
            ServicePointManager.SecurityProtocol = settings.SecurityProtocolType;
            this.restHelper = restHelper;
            this.clientCertificate = clientCertificate;
            this.settings = settings;
            this.jsonSerializerSettings = jsonSerializerSettings;
            this.oasisHostName = IdentificationHelper.GetHostName();
            httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = serverCertificateCustomValidationCallback;

            // If a client certificate is specified, use it.
            if (clientCertificate != null)
            {
                httpClientHandler.ClientCertificates.Add(clientCertificate);
                httpClientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            }
            

            httpClient = new HttpClient(httpClientHandler);

            httpClient.Timeout = TimeSpan.FromSeconds(settings.OasisAgentRestOperationTimeoutInSeconds);

            //string bsCommsLogFilePath = this.settings.CommsLogFilePath.Replace("\\\\", "/");
            //string fullpath = string.Format("{0}/{1}-{2}.txt", bsCommsLogFilePath, "ESS_AgentSession", DateTime.Now.ToString().Replace(':', '-').Replace('/', '_'));
            //mFileWriter = new StreamWriter(fullpath);

        }

        public void SetClientCertificate(X509Certificate2 x509Certificate2)
        {
            if (x509Certificate2 != null && x509Certificate2 != clientCertificate)
            {
                clientCertificate = x509Certificate2;
            }
        }

        /// <summary>
        /// Creates a session with the OASIS Agent
        /// </summary>
        /// <remarks>
        /// Agent sessions last the length of a TCP/IP (HTTP) connection with the agent.
        /// Why do we need sessions?  According to Mark, it's a left-over artifact from some previous software.
        /// These sessions don't do anything, but we still have to establish them and run our interactions with the agent in them.
        /// </remarks>
        /// <param name="EssClientAppId">The application ID to use when establishing a session with the agent</param>
        /// <returns>A response object indicating success or failure</returns>
        public Response<CreateSessionResp> CreateSession(string essClientAppId)
        {
            Log.Information($"AgentSession::CreateSession entry");
            EssClientAppId = essClientAppId;

            // Creates the session request with the client.appid set to our Ess app ID
            var createSessionRequest = new CreateSessionReq(EssClientAppId);
                        
            var response = this.SendToAgentAsync<CreateSessionResp>(OasisAgentRestEndpoint.CreateSession, createSessionRequest);
            
            // Did the call to the agent rest endpoint succeed?
            if (response.Success)
            {
                // Did the session actually get created on the agent?
                if (response.Value.Result.Success)
                {
                    SessionId = response.Value.ExternalSystemSession.SessionID;
                    SessionEstablishedTimeStamp = DateTime.Parse(response.Value.TimeStamp);
                }
                else
                {
                    Log.Error($"AgentSession::CreateSession Failed to establish a session, {response.Value.Result.ErrMsg}");
                }
            }
            else
            {
                Log.Error($"AgentSession::CreateSession Failed to establish a session, {response.Message}");
            }
            Log.Information($"AgentSession::CreateSession exit");
            return response;
        }

        /// <summary>
        /// End the session and dispose of this object
        /// </summary>
        /// <remarks>
        /// Do not make this method virtual.
        /// A derived class should not be able to override this method.
        /// </remarks>
        public virtual void Dispose()
        {
            Dispose(disposing: true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SuppressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    try
                    {
                        httpClient.Dispose();
                    }
                    catch
                    {
                        // Also don't care
                    }
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.

                // Note disposing has been done.
                disposed = true;
            }
        }

        /// <summary>
        /// Close an agent session (only accessible through the dispose method)
        /// </summary>
        public void CloseSession()
        {
            if (AuthorizeResponse?.Result?.Success == true)
            {
                var closeSessionReq = new CloseSessionReq { Client = new ClientAppInfo { AppID = EssClientAppId } };
                SendToAgentAsync<CloseSessionResp>(OasisAgentRestEndpoint.CloseSession, closeSessionReq);
                AuthorizeResponse.Result.Success = false;
            }
        }

        public Response<T> SendToAgentAsync<T>(OasisAgentRestEndpoint oasisAgentRestEndpoint, object data)
        {
            var requestUri = oasisAgentRestEndpoint.GetAgentRestUri(oasisHostName, settings.OasisAgentPortNumber);
            Log.Information($"AgentSession::SendToAgentAsync SendToAgentAsync entry URL:{requestUri}");

            // Attach the session data if this message implements AuthenticationBase
            if (data is AuthenticationBase authBase)
            {
                authBase.Auth = new Authentication { Token = SessionId };
            }

            var jsonData = JsonConvert.SerializeObject(data, jsonSerializerSettings);

            if (clientCertificate == null)
            {
                // Check to see if we need a certificate or not
                if (oasisAgentRestEndpoint.IsClientCertificateRequired().GetValueOrDefault())
                {
                    Log.Error($"AgentSession::SendToAgentAsync exit The call to {requestUri} requires a client certificate.");
                    // Client certificate is missing
                    throw new MissingClientCertificateException($"The call to {requestUri} requires a client certificate. Please recreate the {nameof(AgentSession)} with a valid client certificate.");
                }
            }

            var task = Task.Run(() => restHelper.SendRequestAsync(httpClient,
                requestUri,
                oasisHostName,
                oasisAgentRestEndpoint.GetAgentEndpointHttpMethod(),
                jsonData,
                clientCertificate));
            task.Wait();
            var response = task.Result;

            // Did the REST call succeed?
            if (response.Success)
            {
                var jsonObject = JsonConvert.DeserializeObject<T>(response.Value, jsonSerializerSettings);

                if (jsonObject != null)
                {
                    // If the object we get back supports "Result", check the result
                    if (jsonObject is ResultBase result)
                    {
                        // This contains a result the Agent
                        if (!result.Result.Success)
                        {
                            Log.Error($"AgentSession::SendToAgentAsync exit Call to {requestUri} was successful, but the Result from the OASIS Agent was unsuccessful: {result.Result.ErrMsg}");
                            //return Response<T>.Failed($"Call to {requestUri} was successful, but the Result from the OASIS Agent was unsuccessful: {result.Result.ErrMsg}");
                            return Response<T>.Failed($"Call to {requestUri} was successful, but the Result from the OASIS Agent was unsuccessful: {result.Result.ErrMsg}", jsonObject);
                        }
                    }
                    Log.Information($"AgentSession::SendToAgentAsync exit Success ");
                    return Response<T>.Succeeded(jsonObject);
                }
                else
                {
                    Log.Error($"AgentSession::SendToAgentAsync exit Call to {requestUri} was successful, but no data was returned");
                    return Response<T>.Failed($"Call to {requestUri} was successful, but no data was returned");
                }
            }
            else
            {
                Log.Error($"AgentSession::SendToAgentAsync exit call to {requestUri} failed: {response.Message}");
                return Response<T>.Failed(response.Message);
            }
        }
    }
}