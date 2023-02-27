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
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Security.Cryptography.X509Certificates;

    using Utilties;

    using Newtonsoft.Json;

    using Oasis.Agent.Interfaces;
    using Oasis.Agent.Models;
    using Oasis.Agent.Models.Enums;
    using Oasis.Agent.Models.Requests;
    using Oasis.Agent.Models.Responses;
    using Oasis.Common;
    using Serilog;

    /// <summary>
    /// This class represents all of the discrete REST operations that we can perform against the OASIS Agent
    /// </summary>
    public class AgentInterface : AgentSession, IAgentInterface
    {

        public IAgentSession Session => this;

        public AgentInterface(IRestHelper restHelper,
            X509Certificate2 clientCertificate,
            EssSettings settings,
            JsonSerializerSettings jsonSerializerSettings,
            Func<HttpRequestMessage, X509Certificate2, X509Chain, System.Net.Security.SslPolicyErrors, bool> serverCertificateCustomValidationCallback = null)
            : base(restHelper, clientCertificate, settings, jsonSerializerSettings, serverCertificateCustomValidationCallback)
        {
        }

        public new void SetClientCertificate(X509Certificate2 x509Certificate2)
        {
            base.SetClientCertificate(x509Certificate2);
        }

        ///// <summary>
        ///// Login to OASIS Agent
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="password"></param>
        ///// <param name="essClientAppId"></param>
        ///// <param name="essVersion"></param>
        ///// <param name="hostName"></param>
        ///// <param name="hostId"></param>
        ///// <param name="associatedDevices"></param>
        ///// <returns></returns>
        //public Response<LoginResp> Login(string userId,
        //string password,
        //string essClientAppId,
        //string essVersion,
        //string hostName,
        //string hostId,
        //IEnumerable<string> associatedDevices)
        //{
        //    // Create the request
        //    var loginRequest = new LoginReq(userId, password, essClientAppId, essVersion,
        //                            hostName, hostId, base.SessionId, associatedDevices);

        //    // Send it to the agent
        //    return base.SendToAgentAsync<LoginResp>(OasisAgentRestEndpoint.Login,
        //        loginRequest);
        //}

        /// <summary>
        /// AuthorizeServer
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <param name="EssClientAppId"></param>
        /// <param name="agentMachineId"></param>
        /// <param name="essVersion"></param>
        /// <param name="hostName"></param>
        /// <param name="hostId"></param>
        /// <param name="associatedDevices"></param>
        /// <returns></returns>
        public Response<AuthorizeResp> AuthorizeServer( string accessToken,
            string refreshToken,
            string username, 
            string passcode,
            string EssClientAppId,
            string agentMachineId,
            string essVersion,
            string hostName,
            string hostId,
            IEnumerable<string> associatedDevices)
        {
            Response<AuthorizeResp> authorizeResp = default;
            if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(refreshToken))
            {
                var authorizeRequest = new AuthorizeReq(accessToken, refreshToken, username, EssClientAppId, essVersion,
                                        hostName, hostId, agentMachineId, SessionId, associatedDevices);

                authorizeResp = SendToAgentAsync<AuthorizeResp>(OasisAgentRestEndpoint.Authorize, authorizeRequest);
            }
            else if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(passcode))
            {
                var authorizeRequest = new LoginReq(username, passcode, EssClientAppId, essVersion,
                        hostName, hostId, SessionId, associatedDevices);

                authorizeResp = SendToAgentAsync<AuthorizeResp>(OasisAgentRestEndpoint.Login, authorizeRequest);
            }

            if (authorizeResp.Value == null)
            {
                AuthorizeResponse = new AuthorizeResp
                {
                    Result = new Result { Success = false, ErrMsg = authorizeResp.Message }
                };
            }
            else
            {
                AuthorizeResponse = authorizeResp.Value;
            }
            return authorizeResp;
        }

        /// <summary>
        /// SetOfflinePasscodeServer
        /// </summary>
        /// <param name="username"></param>
        /// <param name="passcode"></param>
        /// <returns></returns>
        public Response<SetPasscodeResp> SetOfflinePasscodeServer(string passcode) 
        {
            
            Response<SetPasscodeResp> setPasscodeResp = default;
            if (!string.IsNullOrEmpty(AuthorizeResponse.UserName) && !string.IsNullOrEmpty(passcode))
            {
                var setPasscodeReq = new SetPasscodeReq(AuthorizeResponse.UserName, passcode, SessionId);
                setPasscodeResp = SendToAgentAsync<SetPasscodeResp>(OasisAgentRestEndpoint.SetPasscode, setPasscodeReq);
            }
            return setPasscodeResp;
        }

        /// <summary>
        /// This method returns OASIS agents status, whether OASIS agent is busy in caching SW packages or upload logs to OASIS server etc.
        /// </summary>
        public Response<ServerStatusResp> GetServerStatus(IEnumerable<string> deviceTypeGuids)
        {
            var GuiIds = new List<string>();
            foreach (var typeGuid in deviceTypeGuids)
            {
                GuiIds.Add(typeGuid);
            }
            var statusRequest = new ServerStatusReq
            {
                CotNames = new List<string>(),
                DeviceTypeGuids = GuiIds
            };

            return base.SendToAgentAsync<ServerStatusResp>(OasisAgentRestEndpoint.GetServerStatus, statusRequest);
        }

        /// <summary>
        /// Gets store device config information from OASIS agent.
        /// </summary>
        public Response<StatDeviceResp> StatDevice(string serialNumber, string deviceType, string country)
        {
            var statDeviceRequest = new StatDeviceReq
            {
                ClientAppInfo = new ClientAppInfo { AppID = base.EssClientAppId },
                Country = country,
                DeviceID = new DeviceId { DeviceType = deviceType, SerialNumber = serialNumber }
            };

            return base.SendToAgentAsync<StatDeviceResp>(OasisAgentRestEndpoint.StatDevice, statDeviceRequest);
        }

        /// <summary>
        /// Sends updated device configuration information to OASIS Agent
        /// </summary>
        public Response<UploadRunningCfgResp> SyncDeviceConfig(string deviceType, string serialNumber, SystemConfigs systemConfigs, string country)
        {
            var uploadRunningCfgRequest = new UploadRunningCfgReq
            {
                ClientAppInfo = new ClientAppInfo { AppID = base.EssClientAppId },
                Country = country,
                DeviceID = new DeviceId { DeviceType = deviceType, SerialNumber = serialNumber },
                SystemConfigs = systemConfigs
            };

            return base.SendToAgentAsync<UploadRunningCfgResp>(OasisAgentRestEndpoint.UploadRunningCfg, uploadRunningCfgRequest);
        }

        /// <summary>
        /// Posts the client Certificate Installation status to OASIS Agent
        /// </summary>
        /// <param name="certId"></param>
        /// <param name="installStatus"></param>
        /// <param name="installStatusDetail"></param>
        /// <param name="clientType"></param>
        /// <param name="clientVersion"></param>
        /// <param name="hostname"></param>
        /// <param name="hostId"></param>
        /// <returns></returns>
        public Response<ClientCertificateResp> UpdateClientCertificateStatus(int? certId, string installStatus,
                                                                    string installStatusDetail, string clientType,
                                                                    string clientVersion, string hostname,
                                                                    string hostId)
        {
            var clientAppInfo = new ClientAppInfo { AppID = base.EssClientAppId };

            var clientCertificateRequest = new ClientCertificateReq
            {
                ClientAppInfo = clientAppInfo,
                ClientHostId = hostId,
                ClientHostName = hostname,
                ClientType = clientType,
                ClientVersion = clientVersion,
                InstallStatusDetail = installStatusDetail,
                InstallStatus = installStatus,
                Id = certId
            };

            return base.SendToAgentAsync<ClientCertificateResp>(OasisAgentRestEndpoint.UpdateClientCertificate, clientCertificateRequest);
        }

        /// <summary>
        /// Fetches the list of Software Packages along with Oid (i.e. Package mail box reference) from OASIS agent.
        /// </summary>
        public Response<SoftwareOidResp> GetSoftwareOid(string serialNumber, string deviceType)
        {
            var deviceID = new DeviceId
            {
                DeviceType = deviceType,
                SerialNumber = serialNumber
            };

            var softwareOidRequest = new SoftwareOidReq { ClientAppInfo = new ClientAppInfo { AppID = base.EssClientAppId }, DeviceID = deviceID };

            return base.SendToAgentAsync<SoftwareOidResp>(OasisAgentRestEndpoint.GetSoftwareOid, softwareOidRequest);
        }

        public Response<SoftwaresResp>  GetSoftwares(string deviceType)
        {
            var softwareRequest = new SoftwaresReq { ClientAppInfo = new ClientAppInfo { AppID = base.EssClientAppId }, DeviceType = deviceType };
            return base.SendToAgentAsync<SoftwaresResp>(OasisAgentRestEndpoint.GetSoftwares, softwareRequest);
        }

        /// <summary>
        /// Updates the Acknoledgement/device/download status to OASIS Agent.
        /// </summary>
        public Response<UpdateAcknowledgeResp> UpdateAck(string serialNumber,
                                                                    string deviceType,
                                                                    string country,
                                                                    UpdateAckType ackType,
                                                                    string pkgName,
                                                                    string pkgPartNumber,
                                                                    string pkgRevision,
                                                                    string result,
                                                                    string statusDetails)
        {
            var updateAcknowledgeRequest = new UpdateAcknowledgeReq
            {
                ClientAppInfo = new ClientAppInfo { AppID = EssClientAppId },
                Country = country,
                DeviceID = new DeviceId { SerialNumber = serialNumber, DeviceType = deviceType },
                UpdateAck = new UpdateAck
                {
                    AckType = ackType,
                    Status = result,
                    StatusDetail = statusDetails,
                    Name = pkgName,
                    PartNumber = pkgPartNumber,
                    Revision= pkgRevision,
                    Timestamp = DateTime.Now.ToString(),
                }
            };

            return SendToAgentAsync<UpdateAcknowledgeResp>(OasisAgentRestEndpoint.UpdateAcknowledge, updateAcknowledgeRequest);
        }

        /// <summary>
        /// Logs off any currently logged in user on the current OASIS Agent session
        /// </summary>
        public Response<LogoffResp> Logoff()
        {
            var logoffRequest = new LogoffReq();

            var logoffResponse = SendToAgentAsync<LogoffResp>(OasisAgentRestEndpoint.Logoff, logoffRequest);

            if (logoffResponse.Success)
            {
                AuthorizeResponse = null;
            }
            else
            {
                Log.Warning($"AgentSession::CreateSession Logoff Request is Failed {logoffResponse.Message}");
            }

            return logoffResponse;
        }

        /// <summary>
        /// Downloads the specified package from OASIS Agent
        /// </summary>
        public Response<SoftwarePackageResp> FetchSoftwarePackage(string oid)
        {
            var softwarePackageRequest = new SoftwarePackageReq { ClientGUID = EssClientAppId, MailboxOID = oid };

            return SendToAgentAsync<SoftwarePackageResp>(OasisAgentRestEndpoint.GetSoftwarePackage, softwarePackageRequest);
        }

        /// <summary>
        /// Updates the Logs File header information, which file is going to be uploaded, to OASIS agent.
        /// </summary>
        public Response<UploadFileInitResp> InitUploadLogRequest(string serialNumber,
                                                                            string deviceType,
                                                                            string logFileType,
                                                                            string originalFileName,
                                                                            string originalFilepath,
                                                                            int numberOfChunks,
                                                                            long fileChunkSize,
                                                                            DateTime logFileDate,
                                                                            string digitalSigVal)
        {
            var uploadFileInitRequest = new UploadFileInitReq
            {
                DeviceID = new DeviceId { SerialNumber = serialNumber, DeviceType = deviceType },
                ChunkSize = fileChunkSize, // This is the maximum size a chunk will be for this job.
                ChunkCount = numberOfChunks,
                FileType = logFileType, // RFIDLog, ...
                Date = logFileDate.ToShortDateString(),
                OriginalFileName = originalFileName,
                OriginalFileAbsolutePath = originalFilepath,
                OriginalFileDigitalSig = digitalSigVal
            };

            return SendToAgentAsync<UploadFileInitResp>(OasisAgentRestEndpoint.UploadFileInit, uploadFileInitRequest);
        }

        /// <summary>
        /// Uploads the log file to OASIS agent in one or more chunks based on <see cref="InitUploadLogRequest(string, string, string, string, string, int, long, DateTime)"/>
        /// max chunk size.
        /// </summary>
        /// <remarks>Note that the <paramref name="chunkId"/> apparently represents a 1-based counter of the chunks, so make sure to set it correctly when calling this method</remarks>
        public Response<UploadLogFileResp> UploadLogFileChunkRequest(string originalFileName,
                                                                string newTaskId,
                                                                int chunkId,
                                                                int chunkSize,
                                                                byte[] fileChunk, 
                                                                string digitalSig)
        {
            // If the chunk size is less than the buffer size, make sure we acount for that.
            // If it's larger than the buffer size, there's a problem
            if (chunkSize > fileChunk.Length)
            {
                Log.Error($"AgentInterface::UploadLogFileChunkRequest Chunk size mismatch {chunkSize} vs {fileChunk.Length}");
                throw new ArgumentException("Chunk size is greater than the buffer that was passed in", nameof(fileChunk));
            }

            var uploadLogFileRequest = new UploadLogFileReq
            {
                ClientGUID = EssClientAppId,
                TaskID = newTaskId,
                ChunkID = chunkId.ToString(),
                ChunkSize = chunkSize.ToString(),
                FileName = originalFileName,
                ChunkFileDigitalSignature = digitalSig
            };

            uploadLogFileRequest.Bytes = new byte[chunkSize];
            Buffer.BlockCopy(fileChunk, 0, uploadLogFileRequest.Bytes, 0, chunkSize);

            return SendToAgentAsync<UploadLogFileResp>(OasisAgentRestEndpoint.UploadLogFileChunk, uploadLogFileRequest);
        }

        /// <summary>
        /// Query the agent for the session status
        /// </summary>
        public Response<GetSessionStatusResp> GetSessionStatus()
        {
            var getSessionStatusRequest = new GetSessionStatusReq
            {
                SessionID = base.SessionId,
                Client = new ClientAppInfo { AppID = EssClientAppId }
            };

            return SendToAgentAsync<GetSessionStatusResp>(OasisAgentRestEndpoint.GetSessionStatus, getSessionStatusRequest);
        }

        /// <summary>
        /// Get the client certificate from the OASIS Agent
        /// </summary>
        /// <remarks>
        /// This method will fail unless the OASIS Agent is connected to the OASIS Server.
        /// It is recommended that you check the status of the Agent's connection using <see cref="GetServerStatus(List{string})"/> before calling this method.
        /// Additionally, after getting a new client certificate, you must restart the session with the OASIS Agent.
        /// </remarks>
        public Response<ClientCertificateResp> GetClientCertificate(string clientType, string clientVersion, string hostname, string hostId)
        {
            var clientCertificateRequest = new ClientCertificateReq
            {
                ClientAppInfo = new ClientAppInfo { AppID = EssClientAppId },
                ClientHostId = hostId,
                ClientHostName = hostname,
                ClientType = clientType,
                ClientVersion = clientVersion
            };

            return SendToAgentAsync<ClientCertificateResp>(OasisAgentRestEndpoint.GetClientCertificate, clientCertificateRequest);
        }
    }
}