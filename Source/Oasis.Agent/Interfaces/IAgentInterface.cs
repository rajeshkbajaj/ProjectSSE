// ------------------------------------------------------------------------------
//                    Copyright (c) 2021 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Oasis.Agent.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    
    using Utilties;

    using Oasis.Agent.Models;
    using Oasis.Agent.Models.Enums;
    using Oasis.Agent.Models.Responses;

    public interface IAgentInterface : IDisposable
    {
        IAgentSession Session { get; }

        Response<SoftwarePackageResp> FetchSoftwarePackage(string oid);

        Response<ClientCertificateResp> GetClientCertificate(string clientType, string clientVersion, string hostname, string hostId);

        Response<ServerStatusResp> GetServerStatus(IEnumerable<string> deviceTypeGuids);

        Response<GetSessionStatusResp> GetSessionStatus();

        Response<SoftwaresResp> GetSoftwares(string deviceTypeGuid);

        Response<SoftwareOidResp> GetSoftwareOid(string serialNumber, string deviceTypeGuid);

        Response<UploadFileInitResp> InitUploadLogRequest(string serialNumber, string deviceType, string logFileType, string originalFileName, string originalFilepath, int numberOfChunks, long fileChunkSize, DateTime logFileDate, string digitalSig);

//        Response<LoginResp> Login(string userId, string password, string EssClientAppId, string essVersion, string hostName, string hostId, IEnumerable<string> associatedDevices);

        Response<AuthorizeResp> AuthorizeServer(string accessToken, string refreshToken, string username, string passcode, string EssClientAppId, string agentMachineId, string essVersion, string hostName, string hostId, IEnumerable<string> associatedDevices);

        Response<SetPasscodeResp> SetOfflinePasscodeServer(string passcode);

        Response<LogoffResp> Logoff();

        Response<StatDeviceResp> StatDevice(string serialNumber, string deviceType, string country);

        Response<UploadRunningCfgResp> SyncDeviceConfig(string deviceType, string serialNumber, SystemConfigs systemConfigs, string country);

        Response<UpdateAcknowledgeResp> UpdateAck(string serialNumber, string deviceType, string country, UpdateAckType ackType, string pkgName, string pkgPartNumber, string pkgRevision, string result, string statusDetails);

        Response<ClientCertificateResp> UpdateClientCertificateStatus(int? certId, string installStatus, string installStatusDetail, string clientType, string clientVersion, string hostname, string hostId);

        Response<UploadLogFileResp> UploadLogFileChunkRequest(string originalFileName, string newTaskId, int chunkId, int chunkSize, byte[] fileChunk, string digitalSig);
        void SetClientCertificate(X509Certificate2 x509Certificate2);
    }
}