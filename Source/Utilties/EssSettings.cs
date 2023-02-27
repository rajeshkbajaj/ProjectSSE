// ------------------------------------------------------------------------------
//                    Copyright (c) 2022 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Utilties
{
    using System.Security.Cryptography.X509Certificates;
    using System.Net;

    public class EssSettings
    {
        // Updating default values for members, if essConfigSetting.json doesn't contians any of the field.
        public virtual string CommsLogFilePath { get; set; } = "C:\\Logs" ;

        public virtual string TempPkgStorePath { get; set; } = "C:\\savedPackage";

        public virtual string TempSoftwareDownloadPath { get; set; } = "C:\\VikingTFTPRoot";

        public virtual string BinariesFolder { get; set; } = "bin";

        public virtual string ConfigFolder { get; set; } = "config";

        public virtual string DownloadManifestFile { get; set; } = "config\\download.xml";

        public virtual string SoftwareSavedPath { get; set; } = "C:\\savedPackage";

        public virtual string VentIpAddress { get; set; } = "192.168.0.12";

        public virtual int VentPortNumber { get; set; } = 80;

        public virtual string DefaultSWUpdatePkgName { get; set; } = "BdSoftware";

        public virtual string DummyVentSerialNo { get; set; } = "35B1400283";

        public virtual string CertificateSender { get; set; } = default;

        public virtual string CertificateSubject { get; set; } = default;

        public virtual StoreName CertificateStoreName { get; set; } = StoreName.My;

        public virtual StoreLocation CertificateStoreLocation { get; set; } = StoreLocation.LocalMachine;

        public virtual SecurityProtocolType SecurityProtocolType { get; set; } = SecurityProtocolType.Tls12;

        public virtual string EssCertificateCommonName { get; set; } = "ESS";

        public virtual string PrivateKeyContainerName { get; set; } = "ESS_KeyContainer";

        public virtual int OasisAgentPortNumber { get; set; } = 9020;

        public virtual int OasisAgentMaxConnectAttempts { get; set; } = 3;

        public virtual int OasisAgentRestOperationTimeoutInSeconds { get; set; } = 180;

        public virtual string EssClientAppId { get; set; } = "AF15E9D1-6AA6-4A60-AF90-58ABABD9619F";

        public virtual int OasisServerAvailabilityTimeoutInMinutes { get; set; } = 15;

        public virtual int OasisServerStatusCheckIntervalInMilliseconds { get; set; } = 1000;

        public virtual int LogFileUploadChunkSizeInBytes { get; set; } = 1048576;

        public virtual int OasisAgentClientCertificateRenewalInDays { get; set; } = 60;

        public virtual string LicenseInfoPath { get; set; } = "ESSLicense.info";

        public virtual string LogDefinitionFile { get; set; } = "VikingLogs-Group1.xml";

        public virtual string LogInflationJsFile { get; set; } = "MakeHtmlTable.js";

        public virtual int TempFilesExpireDays { get; set; } = 90;

        public virtual int SplashScreenTimeOutInSec { get; set; } = 2;
        
        public virtual int StartUpTimeOutInSec { get; set; } = 600;
        
        public virtual string SeriLogsFilePath { get; set; } = "C:\\Logs\\ESS_Application.log";
    }
}
