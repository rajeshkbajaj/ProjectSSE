namespace Oasis.Common
{
    using Oasis.Common.Attributes;

    public enum OasisAgentRestEndpoint
    {
        [OasisAgentEndpoint("/Agent/CCVersion", OasisAgentEndpointMethodType.Post, true)]
        CCVersion,

        [OasisAgentEndpoint("/Agent/ChangePassword", OasisAgentEndpointMethodType.Post, true)]
        ChangePassword,

        [OasisAgentEndpoint("/Agent/CloseSession", OasisAgentEndpointMethodType.Post, false)]
        CloseSession,

        [OasisAgentEndpoint("/Agent/CreateDevice", OasisAgentEndpointMethodType.Post, true)]
        CreateDevice,

        [OasisAgentEndpoint("/Agent/CreateSession", OasisAgentEndpointMethodType.Post, false)]
        CreateSession,

        [OasisAgentEndpoint("/Agent/DownloadDeviceSettings", OasisAgentEndpointMethodType.Post, true)]
        DownloadDeviceSettings,

        [OasisAgentEndpoint("/Agent/DownloadFeatureLicense", OasisAgentEndpointMethodType.Post, true)]
        DownloadFeatureLicense,

        [OasisAgentEndpoint("/Agent/DownloadFileChunk", OasisAgentEndpointMethodType.Post, true)]
        DownloadFileChunk,

        [OasisAgentEndpoint("/Agent/DownloadFileInit", OasisAgentEndpointMethodType.Post, true)]
        DownloadFileInit,

        [OasisAgentEndpoint("/Agent/DownloadSavedLogFile", OasisAgentEndpointMethodType.Post, true)]
        DownloadSavedLogFile,

        [OasisAgentEndpoint("/Agent/DownloadSoftware", OasisAgentEndpointMethodType.Post, true)]
        DownloadSoftware,

        [OasisAgentEndpoint("/Agent/DownloadSoftwareFile", OasisAgentEndpointMethodType.Post, true)]
        DownloadSoftwareFile,

        [OasisAgentEndpoint("/Agent/DownloadUserSettingFiles", OasisAgentEndpointMethodType.Post, true)]
        DownloadUserSettingFiles,

        [OasisAgentEndpoint("/Agent/GetClientCertificate", OasisAgentEndpointMethodType.Post, false)]
        GetClientCertificate,

        [OasisAgentEndpoint("/Agent/GetCountryList", OasisAgentEndpointMethodType.Post, true)]
        GetCountryList,

        [OasisAgentEndpoint("/Agent/GetDeploymentGroupSoftwareStreaming", OasisAgentEndpointMethodType.Post, true)]
        GetDeploymentGroupSoftwareStreaming,

        [OasisAgentEndpoint("/Agent/GetDeviceCertificate", OasisAgentEndpointMethodType.Post, true)]
        GetDeviceCertificate,

        [OasisAgentEndpoint("/Agent/GetDeviceList", OasisAgentEndpointMethodType.Post, true)]
        GetDeviceList,

        [OasisAgentEndpoint("/Agent/GetDeviceTypes", OasisAgentEndpointMethodType.Post, true)]
        GetDeviceTypes,

        [OasisAgentEndpoint("/Agent/GetFacilityList", OasisAgentEndpointMethodType.Post, true)]
        GetFacilityList,

        [OasisAgentEndpoint("/Agent/GetMatchedCfg", OasisAgentEndpointMethodType.Post, true)]
        GetMatchedCfg,

        [OasisAgentEndpoint("/Agent/GetServerStatus", OasisAgentEndpointMethodType.Post, false)]
        GetServerStatus,

        [OasisAgentEndpoint("/Agent/GetSessionStatus", OasisAgentEndpointMethodType.Post, false)]
        GetSessionStatus,

        [OasisAgentEndpoint("/Agent/GetSoftwareOid", OasisAgentEndpointMethodType.Post, true)]
        GetSoftwareOid,

        [OasisAgentEndpoint("/Agent/GetSoftwarePackage", OasisAgentEndpointMethodType.Post, true)]
        GetSoftwarePackage,

        [OasisAgentEndpoint("/Agent/GetSoftwarePackageStreamingOutput", OasisAgentEndpointMethodType.Post, true)]
        GetSoftwarePackageStreamingOutput,

        [OasisAgentEndpoint("/Agent/GetSoftwares", OasisAgentEndpointMethodType.Post, true)]
        GetSoftwares,

        [OasisAgentEndpoint("/Agent/GetSoftwaresForDevice", OasisAgentEndpointMethodType.Post, true)]
        GetSoftwaresForDevice,

        [OasisAgentEndpoint("/Agent/Login", OasisAgentEndpointMethodType.Post, false)]
        Login,

        [OasisAgentEndpoint("/Agent/Logoff", OasisAgentEndpointMethodType.Post, false)]
        Logoff,

        [OasisAgentEndpoint("/Agent/Authorize", OasisAgentEndpointMethodType.Post, false)]
        Authorize,

        [OasisAgentEndpoint("/Agent/SetPasscode", OasisAgentEndpointMethodType.Post, false)]
        SetPasscode,

        [OasisAgentEndpoint("/Agent/RenewClientCertificate", OasisAgentEndpointMethodType.Post, false)]
        RenewClientCertificate,

        [OasisAgentEndpoint("/Agent/RenewDeviceCertificate", OasisAgentEndpointMethodType.Post, true)]
        RenewDeviceCertificate,

        [OasisAgentEndpoint("/Agent/SnReprogram", OasisAgentEndpointMethodType.Post, true)]
        SnReprogram,

        [OasisAgentEndpoint("/Agent/StatDevice", OasisAgentEndpointMethodType.Post, true)]
        StatDevice,

        [OasisAgentEndpoint("/Agent/UpdateAcknowledge", OasisAgentEndpointMethodType.Post, true)]
        UpdateAcknowledge,

        [OasisAgentEndpoint("/Agent/UpdateClientCertificate", OasisAgentEndpointMethodType.Post, true)]
        UpdateClientCertificate,

        [OasisAgentEndpoint("/Agent/UpdateDeviceCertificate", OasisAgentEndpointMethodType.Post, true)]
        UpdateDeviceCertificate,

        [OasisAgentEndpoint("/Agent/UpdateHardwareUpdateTime", OasisAgentEndpointMethodType.Post, true)]
        UpdateHardwareUpdateTime,

        [OasisAgentEndpoint("/Agent/UploadFileChunk", OasisAgentEndpointMethodType.Post, true)]
        UploadFileChunk,

        [OasisAgentEndpoint("/Agent/UploadFileInit", OasisAgentEndpointMethodType.Post, true)]
        UploadFileInit,

        [OasisAgentEndpoint("/Agent/UploadLogFileChunk", OasisAgentEndpointMethodType.Post, true)]
        UploadLogFileChunk,

        [OasisAgentEndpoint("/Agent/UploadRunningCfg", OasisAgentEndpointMethodType.Post, true)]
        UploadRunningCfg,

        [OasisAgentEndpoint("/Agent/UploadStreamingLogFile", OasisAgentEndpointMethodType.Post, true)]
        UploadStreamingLogFile
    }
}