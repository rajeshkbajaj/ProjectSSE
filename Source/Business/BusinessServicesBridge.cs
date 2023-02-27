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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Xml;
    using Oasis.Agent;
    using Oasis.Agent.Interfaces;
    using Utilties;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using CryptoServices;
    using System.Net.Http;
    using System.Net.Security;
    using Oasis.Agent.Models;
    using System.Security.Cryptography;
    using Oasis.Agent.Models.Responses;   
    using Oasis.Agent.Models.Enums;
    using System.Windows.Forms;
    using Serilog;

    public class BusinessServicesBridge
    {
        private const string EST_SST_DIAGNOSTIC_LOG_NAME = "estsst";    // must be sure to ignore case when comparing!
        private const string SYS_DIAGNOSTIC_LOG_NAME = "sysdiag";
        private const string SYS_COMM_LOG_NAME = "syscomm";
        private readonly string[] DiagnosticLogs = new string[] { SYS_DIAGNOSTIC_LOG_NAME, SYS_COMM_LOG_NAME, EST_SST_DIAGNOSTIC_LOG_NAME } ;

        private const string ALARM_LOG_NAME = "Alarm";    
        private const string GENEVENT_LOG_NAME = "GenEvent";
        private const string PATIENT_DATA_LOG_NAME = "PatientData";
        private const string SETTINGS_LOG_NAME = "Settings";
        private const string SERVICE_LOG_NAME = "Service";
        private const string DEVICE_INFO_LOG_NAME = "DeviceInfo";
        //Standard Logs
        private readonly string[] DeviceLogNames = new string[] { ALARM_LOG_NAME, GENEVENT_LOG_NAME, PATIENT_DATA_LOG_NAME, SETTINGS_LOG_NAME, SERVICE_LOG_NAME, SYS_DIAGNOSTIC_LOG_NAME, SYS_COMM_LOG_NAME, EST_SST_DIAGNOSTIC_LOG_NAME, DEVICE_INFO_LOG_NAME };

        private const string OTHER_PACKAGE = "othersoftware";
        private const string VENTILATOR_PACKAGE = "ventilator";
        private const string RSA_SERVER_NOT_RUNNING = "OASIS Agent is not Running, please check in 'Services' and Restart ESS";
        private const string RSA_CERT_INVALID = "Application Certificate is not available or expired.";
        private const string RELAUNCH_APP = "Re-Launch ESS Application with 'Run as administrator'";

        private const string SoftwareString = "SOFTWARE";
        private const string HardwareString = "HARDWARE";

        private const string SW_UPDATE_LOCAL_PKG_WRN = " Description may not be accurate as it is a local Package";
        private const string SW_UPDATE_STARTED_MSG = "start";
        private const string SW_UPDATE_FAILED_MSG = "failed";

        public DeviceUserInterfaceServices Device { get; private set; }
        public bool IsDeviceInDownloadMode { get; private set; } = false;
        public bool IsServerAvialable { get; private set; } = false;
        public string OtherSoftwarePkgSavedPath { get; private set; }
        public bool IsLogUploadRunning { get; private set; } = false;

        private VentInterfaceServices DeviceInterface { get; set; }
        private ISessionManager sessionManager = null;
        private ICertificateService certificateService = null;
        private IAgentInterface agentInterface = null;
        private X509Certificate2 clientCertificate = null;
        private OasisCredentialInformation oasisCredentialInformation = null;
        private bool DownloadInProgress = false;
        private int LogsUploadNotificationSentCount = 0;
        private bool DeviceConnected = false;
        private string SoftwareDownloadPackageName = null;     
        private EssSettings Settings;
        private bool IsRetrieveSoftwareRunning;
        private string OfflineUserName;
        private bool GetServerStatusInProgress;

        /// <summary>
        /// Holds access info to the retrieved logs -- already downloaded from device
        /// </summary>
        private Hashtable StoredLogs;
        private Dictionary<string, LogCargoContainer> UploadPendingToServerLogs;
        private Hashtable LogsTransactionTable;
        private List<Software> SoftwarePkgList = null;
        /// <summary>
        /// The variable provides the locking object
        /// </summary>
        private static readonly object MSyncRoot = new object();

        private string ESSWorkingDirectory;
        private DownloadShell_Net.DownloadShell_Net DownloadShellNet;
        private readonly Hashtable DeviceListsByType = new Hashtable();  // will then contain hashtable of devices by serial numbers which is of DeviceDataFromServer
        private bool DiagnosticDeviceLogsUploadMode = false;
        private string DeviceCountryCode = null;

        /// <summary>
        /// The delegate to be invoked when a connection has occurred.  Pass "null" to clear it.
        /// </summary>
        public RestrictedFunctionManager RestrictedFunctionManager { get; set; }

        public UserInterfaceDelegates.ConnectCallback ConnectCallback  { get; set; }
        public UserInterfaceDelegates.SessionCreateCallback SessionCreateCallback { get; set; }
        public UserInterfaceDelegates.DeviceExistsEventCallback DeviceExistsEventCB { get; set; }
        public UserInterfaceDelegates.GetDeviceLogsListCallback GetDeviceLogsListCallback { get; set; }
        public UserInterfaceDelegates.DeviceLogLoadedCallback DeviceLogLoadedCallback { get; set; }

        public UserInterfaceDelegates.AllLogsUploadedToServerCallback AllLogsUploadedToServerCallbackEvent { get; set; }
        public UserInterfaceDelegates.LogUploadedToServerCallback LogUploadedToServerCallbackEvent { get; set; }

        public UserInterfaceDelegates.ClearDeviceLogsCallback ClearDeviceLogsCallbackEvent { get; set; }

        public UserInterfaceDelegates.NotificationsCallback NotificationsCallback { get; set; }
        public UserInterfaceDelegates.NotificationProcessedCallback NotificationProcessedCallback { get; set; }
        public UserInterfaceDelegates.AllNotificationsProcessedCallback AllNotificationsProcessedCallback { get; set; }

        public UserInterfaceDelegates.FlashProcessStartedCallback DownloadStartedCallback { get; set; } = null;
        public UserInterfaceDelegates.FlashProgressCallback DownloadProgressCallback { get; set; } = null;
        public UserInterfaceDelegates.FlashProcessCompleteCallback DownloadCompleteCallback { get; set; } = null;
        public UserInterfaceDelegates.DeviceFlashFailedCallback DownloadFailedCallback { get; set; } = null;
        public UserInterfaceDelegates.OtherPackageDownloadCallback OtherPkgDownloadCallback { get; set; } = null;

        /// <summary>
        /// mSingleton and Instance provides the private variable and public access to it - to provide the Singleton.
        /// The double-check method for lazy evaluation of the Singleton properly supports multi-threaded applications
        /// </summary>
        private static volatile BusinessServicesBridge MSingleton;
        public static BusinessServicesBridge Instance
        {
            get
            {
                if (null == MSingleton)
                {
                    lock (MSyncRoot)
                    {
                        if (null == MSingleton)
                        {
                            MSingleton = new BusinessServicesBridge();
                        }
                    }
                }

                return MSingleton;
            }
        }

        private BusinessServicesBridge() { }

        public string GetDeviceSerialNumber()
        {
            if (Device != null && Device.DeviceInfo != null)
                return Device.DeviceInfo.SerialNumber;

            return null;
        }

        /// <summary>
        /// Returns true if a user logged in
        /// </summary>
        /// <returns></returns>
        public bool IsUserLoggedIn()
        {
            return RestrictedFunctionManager.IsUserLoggedIn();
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="essSettings"></param>
        public void Initialize(EssSettings essSettings)
        {
            Log.Information($"BusinessServicesBridge::Initialize entry");
            Settings = essSettings;

            // Create if Logs directory is not present
            CheckAndCreateDirectory(Settings.CommsLogFilePath);

            RestrictedFunctionManager = new RestrictedFunctionManager();
            Device = new DeviceUserInterfaceServices(Settings.CommsLogFilePath) ;
            DeviceInterface = new VentInterfaceServices()
            {
                UiCallbacks = Device
            };
            Device.ServerIFace = DeviceInterface;

            if (!IPAddress.TryParse(Settings.VentIpAddress, out IPAddress ipAddr))
            {
                Log.Error($"BusinessServicesBridge:Initialize Failed to parse IP Address {Settings.VentIpAddress}");
            }

            DeviceInterface.IdentifyServer( ipAddr, Settings.VentPortNumber) ;
            Device.LogDefinitionFilename = Path.Combine(Environment.CurrentDirectory, Settings.LogDefinitionFile);
            Device.LogInflationJsFilename = Path.Combine(Environment.CurrentDirectory, Settings.LogInflationJsFile);

            StoredLogs = new Hashtable();   // might be able to reduce to list of strings.
            LogsTransactionTable = new Hashtable();
            UploadPendingToServerLogs = new Dictionary<string, LogCargoContainer>();
            DownloadInProgress = false;
            ESSWorkingDirectory = Directory.GetCurrentDirectory();

            //delete temporary directories
            DeleteTempSwDownloadDirectory();
            DeleteTempPkgDirectory(); ;
            Log.Information($"BusinessServicesBridge::Initialize exit");
        }

        /// <summary>
        /// Must only be called when connected to a device.
        /// </summary>
        /// <param name="restrictedFuncId"></param>
        /// <returns></returns>
        public RestrictedFunctionManager.RestrictionAlleviation IsFunctionRestricted(RestrictedFunctionManager.RestrictedFunctions restrictedFuncId)
        {
            //if ((null != Device && null != Device.DeviceInfo) && (null != Device.DeviceInfo.SerialNumber))
            if (null != Device?.DeviceInfo?.SerialNumber)
            {
                return (RestrictedFunctionManager.IsFunctionRestricted(Device?.DeviceInfo?.SerialNumber, restrictedFuncId));
            }

            return RestrictedFunctionManager.RestrictionAlleviation.ACCESS_DENIED;
        }

        /// <summary>
        /// IsCovidienUser
        /// </summary>
        /// <returns></returns>
        public bool IsCovidienUser()
        {
            if (agentInterface != null && agentInterface.Session != null && agentInterface.Session.AuthorizeResponse != null)
            {
                return agentInterface.Session.AuthorizeResponse.CovidienUser;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// GetLoggedInUserName
        /// </summary>
        /// <returns></returns>
        public string GetLoggedInUserName()
        {
            if (agentInterface != null && agentInterface.Session != null && agentInterface.Session.AuthorizeResponse != null)
            {
                return agentInterface.Session.AuthorizeResponse.UserName;
            }
            else
            {
                return OfflineUserName;
            }
        }
        /// <summary>
        /// GetUserCountryCode
        /// </summary>
        /// <returns></returns>
        public string GetUserCountryCode()
        {
            return agentInterface?.Session?.AuthorizeResponse?.Country;
        }

        /// <summary>
        /// this call issues GetHeaders request to get software package notifications from the server based on a application provided device serial #
        /// </summary>
        /// <param name="deviceSerialNumber"></param>
        public bool RetrieveSoftwarePackagesFromServer()
        {
            return RetrieveSoftwarePackagesFromServer(SoftwareListRetrievalCompleteCB);
        }

        /// <summary>
        /// device serial number is set through this API
        /// </summary>
        /// <param name="deviceSerialNumber"></param>
        public void SetDeviceSerialNumber(string deviceSerialNumber)
        {
            Device.SetDeviceSerialNumber(deviceSerialNumber);
            if (IsUserLoggedIn() == true)
            {
                Device.UpdateUserRights(RestrictedFunctionManager, Device.DeviceInfo, agentInterface.Session.AuthorizeResponse);
            }
        }

        /// <summary>
        /// GetSupportedDeviceTypes
        /// </summary>
        /// <returns></returns>
        public List<string> GetSupportedDeviceTypes()
        {
            List<string> retval = new List<string>
            {                // retval.Add( ModalConstants.KeyToDisplayString( ModalConstants.PB980_VENTILATOR ) ) ;    // need to find where this re-enters the system
                ModalConstants.PB980_VENTILATOR    // need to find where this re-enters the system
            };

            return retval;
        }

        /// <summary>
        /// returns the license file path
        /// </summary>
        /// <returns></returns>
        public string GetLicenseInfoPath()
        {
            return Path.Combine (Environment.CurrentDirectory, Settings.LicenseInfoPath);
        }

        /// <summary>
        /// Creates directories if they do not exist as required
        /// </summary>
        /// <param name="dirPath">full name and path of the directory</param>
        /// <returns></returns>
        private void CheckAndCreateDirectory(string dirPath)
        {
            // this call creates the directory if it does not exist yet
            Directory.CreateDirectory(dirPath);
            return;
        }

        /// <summary>
        /// Delete expired files on PC file system
        /// Expiration time limit set by application layer
        /// </summary>
        /// <param name="olderThanInDays"></param>
        public void DeleteExpiredFilesOnPC(int olderThanInDays)
        {
            try
            {
                TimeSpan lowerLimitTimeSpan = new TimeSpan(olderThanInDays, 0, 0, 0);
                if (!string.IsNullOrEmpty(Settings.CommsLogFilePath))
                {
                    DirectoryInfo source = new DirectoryInfo(Settings.CommsLogFilePath);
                    DirectoryInfo sourceSavedPackage = new DirectoryInfo(Settings.SoftwareSavedPath);
                    // Get info of each file into the directory
                    if (source.Exists)
                    {
                        foreach (FileInfo fi in source.GetFiles())
                        {
                            if (fi.LastWriteTime < (DateTime.Now - lowerLimitTimeSpan))
                            {
                                fi.Delete();
                            }
                        }
                    }

                    if (sourceSavedPackage.Exists)
                    {
                        //Deleting saved packages over same time duration
                        foreach (DirectoryInfo di in sourceSavedPackage.GetDirectories())
                        {
                            if (di.LastWriteTime < (DateTime.Now - lowerLimitTimeSpan))
                            {
                                di.Delete(true);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"BusinessServices::DeleteExpiredFilesOnPC Exception Occurred: {e.Message}");
            }
        }


        /// <summary>
        /// This should become a factory which depends on what kind of device we are connecting to
        /// </summary>
        /// <param name="ipAddr"></param>
        /// <param name="port"></param>
        /// <param name="connectCallback"></param>
        /// <param name="sessionCreateCallback"></param>
        /// <returns>Currently return null because the PB980 is a webservice, so a true connection is not fully relevant</returns>
        public void ConnectToDevice(IPAddress ipAddr, int port, UserInterfaceDelegates.ConnectCallback connectCallback, UserInterfaceDelegates.SessionCreateCallback sessionCreateCallback, UserInterfaceDelegates.DeviceExistsEventCallback deviceExistsCallback )
        {
            //save callbacks
            ConnectCallback = connectCallback;
            SessionCreateCallback = sessionCreateCallback;
            DeviceExistsEventCB = deviceExistsCallback;

            // Save for later when we need to use the device model... we need to convert it back into a key from the display string
            // string deviceModelKey = ModalConstants.DisplayStringToKey( deviceModel ) ;

            // Rev 1 - can only connect to a single device, so attempt to connect causes us to clear old information
            Device.ResetDevice();
            LogsTransactionTable.Clear();
            IsDeviceInDownloadMode = false;
            CleanupDownloadDirs();

            // Make our own separate calls so the UI doesn't need to know anything about this part of the work
            // and so that we automatically store this information to the server.
            Log.Information($"BusinessServices::ConnectToDevice Connecting to Device");

            try
            {
                DeviceInterface.IdentifyServer(ipAddr, port);

                //this will listen to disconnect and call appropriate callbacks
                ConnectToDeviceAsync();
            }
            catch (Exception e)
            {
                //any exception -> device connection failure
                Log.Error($"BusinessServices::ConnectToDevice Failure {e.Message}");
                ConnectCallback(false);
            }
        }

        /// <summary>
        /// ConnectToDeviceAsync
        /// </summary>
        private void ConnectToDeviceAsync()
        {
            Log.Information($"BusinessServicesBridge::ConnectToDeviceAsync entry");
            //do this async
            BackgroundWorker deviceConnectionHelper = new BackgroundWorker();
            deviceConnectionHelper.DoWork += new DoWorkEventHandler(ConnectToDeviceWorker);
            deviceConnectionHelper.RunWorkerAsync(null);
            Log.Information($"BusinessServicesBridge::ConnectToDeviceAsync exit");

        }

        /// <summary>
        /// ConnectToDeviceWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectToDeviceWorker(object sender, DoWorkEventArgs e)
        {
            Log.Information($"BusinessServices::ConnectToDeviceWorker entry Connecting to Device");
            bool result = DeviceInterface.ConnectToServer();

            //socket connection failed, no need to go further
            if (result == false)
            {
                Log.Information($"BusinessServices::ConnectToDeviceWorker Checking Download Mode");
                //try and see if vent is in download mode 
                CheckDownloadMode();
            }
            else
            {
                //socket connection succeeded, but overall success only after getting device info
                Log.Information($"BusinessServices::ConnectToDeviceWorker Calling GetDeviceInfo");
                GetDeviceInfo();
            }

            if (IsDeviceInDownloadMode || DeviceConnected)
            {
                DeviceExistsEventCB?.Invoke(true);

                MonitorDeviceConnection();
            }
            Log.Information($"BusinessServices::ConnectToDeviceWorker exit");
        }

        private void MonitorDeviceConnection()
        {
            DeviceInterface.StartServerMonitoring();
        }

        private void GetDeviceInfo()
        {
            // Make our own separate calls so the UI doesn't need to know anything about this part of the work
            // and so that we automatically store this information to the server.
            try
            {
                Log.Information($"BusinessServices::GetDeviceInfo entry");

                if (!IsDeviceInDownloadMode)
                {
                    Device.GetDeviceInfo(ProcessDeviceInformation);
                }

                InformDeviceConnectionStatus();
            }
            catch (Exception e)
            {
                Log.Error($"BusinessServices::GetDeviceInfo Exception {e.Message}");
                ConnectCallback?.Invoke(false);
            }
            Log.Information($"BusinessServices::GetDeviceInfo exit");
        }

        /// <summary>
        /// DisconnectFromDevice
        /// </summary>
        /// <returns></returns>
        public bool DisconnectFromDevice()
        {
            // Done... because the Vent is a WebService with no long-standing connection
            // Although we probably ought to set a state flag and also clear out the state variables
            DeviceInterface.DisconnectFromServer();
            DeviceConnected = false;
            return( true ) ;
        }

        /// <summary>
        /// ProcessDeviceInformation
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="actionSuccessful"></param>
        /// <param name="deviceInfo"></param>
        public void ProcessDeviceInformation( string transactionId, bool actionSuccessful, DeviceInformation deviceInfo )
        {
            Log.Information($"BusinessServices::ProcessDeviceInformation entry status:{actionSuccessful}");
            if ( actionSuccessful )
            {
                Device.DeviceInfo = deviceInfo;

                if  ( null != deviceInfo?.SerialNumber )
                {
                    Log.Information($"BusinessServices::ProcessDeviceInformation Calling ConvertDeviceInfoToLog");

                    // Per new reqts, device info should behave like a log file.  Our challenge is that although it is a log file, it then has a race
                    // condition if the user tries to save it to the server, because until we know if the server / RSA even knows about the device
                    // (per response to GetDeviceStatus()), we cannot really save information that may be dumped on the floor (if dev is unknown and not yet created).
                    Device.ConvertDeviceInfoToLog();
                    Device.UpdateDeviceRights( RestrictedFunctionManager ) ;
                    Device.UpdateUserRights(RestrictedFunctionManager, Device.DeviceInfo, agentInterface.Session.AuthorizeResponse);
                }                
            }
            Log.Information($"BusinessServices::ProcessDeviceInformation exit");
        }


        /// <summary>
        /// Registers the callback for all notifications processed event
        /// </summary>
        /// <param name="allNotificationsProcessedCallback"></param>
        public void RegisterForNotificationsEvents(UserInterfaceDelegates.NotificationsCallback notificationsCallback,
                                                    UserInterfaceDelegates.NotificationProcessedCallback notificationProcessedCallback,
                                                    UserInterfaceDelegates.AllNotificationsProcessedCallback allNotificationsProcessedCallback)
        {
            NotificationsCallback = notificationsCallback;
            NotificationProcessedCallback = notificationProcessedCallback;
            AllNotificationsProcessedCallback = allNotificationsProcessedCallback;
        }

        /// <summary>
        /// This service gets the logs from the device, individual worker threads are spawned per log
        /// This service first gets a) DeviceLogsList b) Pulls all logs present in the log list from the device
        /// </summary>
        public void GetDeviceLogs(UserInterfaceDelegates.GetDeviceLogsListCallback getDeviceLogsListCallback, UserInterfaceDelegates.DeviceLogLoadedCallback deviceLogLoadedCallback)
        {
            GetDeviceLogsListCallback = getDeviceLogsListCallback;
            DeviceLogLoadedCallback = deviceLogLoadedCallback;

            Log.Information($"BusinessServices::GetDeviceLogs entry");

            //do this async
            BackgroundWorker LogListRetriever = new BackgroundWorker();
            LogListRetriever.DoWork += new DoWorkEventHandler(GetDeviceLogsListWorker);
            LogListRetriever.RunWorkerAsync(null); 
        }

        /// <summary>
        /// GetDeviceLogsListWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetDeviceLogsListWorker(object sender, DoWorkEventArgs e)
        {
            // Waiting till pending logs upload is completed...
            // This is special scenario, if device is disconnected  and connected while uploading logs, 
            // then only this flag will be true.
            while (IsLogUploadRunning == true)
                Task.Delay(500);

            Device.GetDeviceLogsList(BSGetDeviceLogsListCallback);
        }

        /// <summary>
        /// This function is called as a response to Get Device Logs List
        /// Upon the response, retrieval of all logs is auto-initiated
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="actionSuccessful"></param>
        /// <param name="deviceLogIds"></param>
        private void BSGetDeviceLogsListCallback(string transactionId, bool actionSuccessful, List<string> deviceLogIds)
        {
            Log.Information($"BusinessServices::BSGetDeviceLogsListCallback status:{actionSuccessful}");
            GetDeviceLogsListCallback?.Invoke(transactionId, actionSuccessful, deviceLogIds);

            if (actionSuccessful == true)
            {
                PullLogs(deviceLogIds);
            }
        }

        /// <summary>
        /// PullLog
        /// </summary>
        /// <param name="logName"></param>
        private void PullLog( string logName )
        {
            Log.Information($"BusinessServices::PullLog entry name:{logName}");
            if ( !string.IsNullOrEmpty( logName ) )
            {
                //treat all logs the same
                Device.GetDeviceLog(logName, PullDeviceLogCallback);
            }
            Log.Information($"BusinessServices::PullLog exit");
        }

        /// <summary>
        /// PullLogWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PullLogWorker( object sender, DoWorkEventArgs e )
        {
            Log.Information($"BusinessServices::PullLogWorker entry");
            object[] parameters = e.Argument as object[];
            List<string> lognames = (List<string>)parameters[0];
            foreach (string logName in lognames)
            {
                PullLog(logName);
               
            }
            e.Result = true;
            Log.Information($"BusinessServices::PullLogWorker exit");
        }

        /// <summary>
        /// Always go pull the three key diagnostic logs
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="actionSuccessful"></param>
        /// <param name="deviceLogIds"></param>
        private void PullLogs( List<string> deviceLogIds )
        {
            Log.Information($"BusinessServices::PullLogs entry");
            BackgroundWorker LogInvoker = new BackgroundWorker();

            //started a thread and that is responsible for pulling all logs
            LogInvoker.DoWork += new DoWorkEventHandler(PullLogWorker);
            object paramObj = new List<string>(deviceLogIds);
            object[] parameters = new object[] { paramObj };

            LogInvoker.RunWorkerAsync(parameters);
            Log.Information($"BusinessServices::PullLogs exit");
        }

        /// <summary>
        /// This function is called, whenever a log file is downloaded from the device
        /// In this function, the following things happen
        /// a. The original file in xml is auto-stored b. The raw file in xml is auto-stored c. The The decoded file in HTML is auto-stored on the file system
        /// d. The raw and html files are auto-uploaded for diagnostic logs only e. Other logs (Alarm, Patient etc.) are uploaded manually
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="success"></param>
        /// <param name="logCargo"></param>
        private void PullDeviceLogCallback( string transactionId, bool success, LogCargoContainer logCargo )
        {
            Log.Information($"BusinessServices::PullDeviceLogCallback entry log name: {logCargo?.Name} status {success}");
            if ( success )
            {
                ///TODO: currently doing the same as DiagLogs
                ///should be changed to upload on manually clicking Upload buttong on the GUI
                if (!logCargo.IsOrigSaved)
                {
                    Device.SaveOriginalFile(logCargo);
                }

                if (!logCargo.IsRawSaved)
                {
                    // Thinking that maybe (only maybe), this should be moved to the business layer
                    // because it is storing the log to a local file, and not really interacting with the device
                    Device.SaveRawGzipFile(logCargo);
                }
                if (!logCargo.IsDecodedSaved)
                {
                    // See notes re Raw log
                    Device.SaveHtmlGzipFile(logCargo);
                }
            }

            DeviceLogLoadedCallback?.Invoke(transactionId, success, logCargo?.Name);
            Log.Information($"BusinessServices::PullDeviceLogCallback exit log name: {logCargo.Name}");
        }

        /// <summary>
        /// IsLogSavedToServer
        /// </summary>
        /// <param name="logName"></param>
        /// <returns></returns>
        public bool IsLogSavedToServer( string logName )
        {
            return( StoredLogs.ContainsKey( logName) ) ;
        }

        /// <summary>
        /// SaveLogToServer
        /// </summary>
        /// <param name="logName"></param>
        public void SaveLogToServer( string logName )
        {
            Log.Information($"BusinessServices::SaveLogToServer entry log name: {logName}");
            if ( !StoredLogs.ContainsKey( logName ) )
            {
                if  ( Device.IsLogRetrieved( logName ) )
                {
                    LogCargoContainer logCargo = (LogCargoContainer)Device.RetrievedLogs[ logName ] ;

                    if  ( !logCargo.IsDecoded )
                    {
                        try
                        {
                            Log.Information($"BusinessServices::SaveLogToServer decoding log: {logName}");
                            Device.DecodeLogFile( logCargo ) ;
                        }
                        catch( Exception )
                        {
                            // ignore a barf, but the save function needs to still check decoded flag
                        }
                    }

                    SaveLogToServer(logCargo ) ;
                }
                // else we shouldn't have been here - would be a UI b
            }
            Log.Information($"BusinessServices::SaveLogToServer exit log name: {logName}");
        }

        /// <summary>
        /// SaveLogToServer
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="logCargo"></param>
        public void SaveLogToServer( LogCargoContainer logCargo )
        {
            SaveLogToServerAndContinue( logCargo) ;
        }

        /// <summary>
        /// SaveLogToServerAndContinue
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="logCargo"></param>
        /// <returns></returns>
        public bool SaveLogToServerAndContinue( LogCargoContainer logCargo)
        {
            Log.Information($"BusinessServices::SaveLogToServerAndContinue entry log: {logCargo.Name}");
            var uploadResult = false;
            if (!string.IsNullOrEmpty(logCargo.Name))
            {
                if (logCargo.IsRawSaved &&
                    UpLoadLogFile(logCargo.Name, logCargo.LogRawFilename, logCargo.DevId))
                {
                    uploadResult = true;
                }

                if (logCargo.IsDecodedSaved &&
                    UpLoadLogFile(logCargo.Name, logCargo.LogDecodedFilename, logCargo.DevId))
                {
                    uploadResult = true;
                }

                if (uploadResult)
                {
                    StoredLogs.Add(logCargo.Name, logCargo);    // assuming for the moment this is sufficient
                }
            }
            NotifyLogUploadStatus(uploadResult, logCargo.Name);

            Log.Information($"BusinessServices::SaveLogToServerAndContinue exit status: {uploadResult}");
            return uploadResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionSuccessful"></param>
        /// <param name="logName"></param>
        private void NotifyLogUploadStatus(bool actionSuccessful, string logName)
        {
            Log.Information($"BusinessServices::NotifyLogUploadStatus entry log: {logName} status: {actionSuccessful}");

            //notify single log upload status
            LogsUploadNotificationSentCount++;
            LogUploadedToServerCallbackEvent?.Invoke(actionSuccessful, logName);

            var isLogsUploadCompleted = IsLogsUploadToServerComplete();
            var devLogsCount = GetDeviceLogsCount(DiagnosticDeviceLogsUploadMode);
            // notify overall log upload status
            // if 1 log upload fails - everything fails
            if (actionSuccessful == false)
            {
                AllLogsUploadedToServerCallbackEvent?.Invoke(false);
            }
            else if (isLogsUploadCompleted== true && (LogsUploadNotificationSentCount == devLogsCount))
            {
                //if one log upload fails - notify failure and do not continue
                AllLogsUploadedToServerCallbackEvent?.Invoke(true);
            }
            Log.Information($"BusinessServices::NotifyLogUploadStatus exit");
        }

        /// <summary>
        /// This function uses the DeviceUserInterface instance to see if all the logs have been imported/ downloaded
        /// </summary>
        /// <returns></returns>
        public bool IsDeviceLogsImportComplete()
        {
            return Device.IsDeviceLogsImportComplete();
        }

        /// <summary>
        /// Checks to see if the logs upload to server is complete
        /// </summary>
        /// <returns></returns>
        private bool IsLogsUploadToServerComplete()
        {
            bool result;
            if (DiagnosticDeviceLogsUploadMode == true)
            {
                result = IsDiagnosticDeviceLogsUploadToServerComplete();
            }
            else
            {
                Log.Information($"BusinessServices::IsLogsUploadToServerComplete {UploadPendingToServerLogs.Count} vs {StoredLogs.Count}");
                result = UploadPendingToServerLogs.Count == StoredLogs.Count;
            }
            Log.Information($"BusinessServices::IsLogsUploadToServerComplete exit status:{result}");
            return result;
        }

        /// <summary>
        /// Checks to see if all the diagnostic logs are stored on the server
        /// </summary>
        /// <returns></returns>
        private bool IsDiagnosticDeviceLogsUploadToServerComplete()
        {
            int diagLogsCount = GetDeviceLogsCount(true);
            int uploadedDiagLogsCount = 0;
            foreach (string storedLog in StoredLogs.Keys)
            {
                LogCargoContainer logCargo = (LogCargoContainer)StoredLogs[storedLog];
                if (IsDiagnosticDeviceLog(logCargo.Name) == true)
                {
                    uploadedDiagLogsCount++;
                }
            }
            Log.Information($"BusinessServices::IsDiagnosticToServerComplete exit status:{diagLogsCount == uploadedDiagLogsCount}, {diagLogsCount} vs {uploadedDiagLogsCount}");
            return (diagLogsCount == uploadedDiagLogsCount);
        }

        public bool UploadAllLogsToServer(UserInterfaceDelegates.LogUploadedToServerCallback logUploadedCallback, UserInterfaceDelegates.AllLogsUploadedToServerCallback allLogsUploadedCallback, bool diagnosticDeviceLogsUploadOnly)
        {
            if (!IsLogUploadRunning)
            {
                foreach (DictionaryEntry data in Device.RetrievedLogs)
                {
                    UploadPendingToServerLogs.Add((string)data.Key, (LogCargoContainer)data.Value);
                }

                Task.Run(() => UploadDeviceLogsToServerWorker(logUploadedCallback, allLogsUploadedCallback, diagnosticDeviceLogsUploadOnly));

                return true;
            }
            else
            {
                Log.Information($"UploadDeviceLogsToServer: Logs upload is already in progress. Ignoring this request.");
            }
            return false;
        }

        /// <summary>
        ///  This function uploads the logs to the server using RSA
        ///  Only logs which are not uploaded are uploaded in this function, as some logs need to auto uploaded
        /// </summary>
        public void UploadDeviceLogsToServerWorker(UserInterfaceDelegates.LogUploadedToServerCallback logUploadedCallback, UserInterfaceDelegates.AllLogsUploadedToServerCallback allLogsUploadedCallback, bool diagnosticDeviceLogsUploadOnly)
        {
            try
            {
                IsLogUploadRunning = true;

                AllLogsUploadedToServerCallbackEvent = allLogsUploadedCallback;
                LogUploadedToServerCallbackEvent = logUploadedCallback;
                DiagnosticDeviceLogsUploadMode = diagnosticDeviceLogsUploadOnly;
                LogsUploadNotificationSentCount = 0;

                var info = diagnosticDeviceLogsUploadOnly ? "UploadAllDiagLogsToServer" : "UploadAllLogsToServer";
                Log.Information($"BusinessServices::UploadDeviceLogsToServerWorker entry uploading {info}");

                //notify already uploaded logs
                foreach (string storedLog in StoredLogs.Keys)
                {
                    LogCargoContainer logCargo = (LogCargoContainer)StoredLogs[storedLog];
                    if (DiagnosticDeviceLogsUploadMode && !IsDiagnosticDeviceLog(logCargo.Name))
                    {
                        continue;
                    }

                    NotifyLogUploadStatus(true, storedLog);
                }

                //clear StoredLogs to upload all logs.

                UploadRemainingLogsToServer();
            }
            catch (Exception ex)
            {
                NotifyLogUploadStatus(false, "");
                Log.Error($"Failed to upload Log files. Exception:{ex.Message}");
            }
            finally
            {
                UploadPendingToServerLogs.Clear();
                IsLogUploadRunning = false;
            }
            Log.Information($"BusinessServices::UploadDeviceLogsToServerWorker exit");
        }

        private void UploadRemainingLogsToServer()
        {
            Log.Information($"BusinessServices::UploadRemainingLogsToServer entry cnt: {UploadPendingToServerLogs.Keys.Count}");
            foreach (string logName in UploadPendingToServerLogs?.Keys)
            {
                var logCargo = UploadPendingToServerLogs[logName];

                if (DiagnosticDeviceLogsUploadMode && !IsDiagnosticDeviceLog(logCargo.Name))
                {
                    Log.Information($"BusinessServices::UploadRemainingLogsToServer {logCargo.Name}...Skip");
                    continue;
                }

                if (!StoredLogs.ContainsKey(logCargo.Name))
                {
                    Log.Information($"BusinessServices::UploadRemainingLogsToServer {logCargo.Name}...Uploading");
                    SaveLogToServerAndContinue(logCargo);
                    //upload one log only, call upload again through call back (if successful) to upload other logs
                    //break;
                }
            }
            Log.Information($"BusinessServices::UploadRemainingLogsToServer exit");
        }

        /// <summary>
        /// Handles Logs upload using agentInterface.
        /// </summary>
        /// <param name="agentInterface"></param>
        /// <param name="logName"></param>
        /// <param name="logFile"></param>
        /// <param name="SerialNumber"></param>
        /// <returns></returns>
        private bool UpLoadLogFile(string logName, string logFile, string SerialNumber)
        {
            try
            {
                Log.Information($"BusinessServices::UpLoadLogFile Waiting for Server name: {logName} file:{logFile} dev:{SerialNumber}");
                WaitUntilOasisServerIsAvailable(TimeSpan.FromMinutes(Settings.OasisServerAvailabilityTimeoutInMinutes),
                    TimeSpan.FromMilliseconds(Settings.OasisServerStatusCheckIntervalInMilliseconds),
                    agentInterface.Session.AuthorizeResponse.DeviceTypeGUIDS.Values);

                var digitSigVal = GetSha256HashDigitalSig(File.ReadAllBytes(logFile), null, null);
                using (var fileStream = File.OpenRead(logFile))
                {
                    var chunkSize = Settings.LogFileUploadChunkSizeInBytes;
                    var numberOfChunks = (((int)fileStream.Length - 1) / chunkSize) + 1;
                    var logFileDate = DateTime.Now;
                    var timeStampedFileName = logFileDate.ToString("yyMMdd_HHmmss") + "_" + Path.GetFileName(fileStream.Name);

                    Log.Information($"BusinessServices::UpLoadLogFile InitUploadLogRequest calling..: {logName}");

                    // Initialize the upload
                    var uploadFileInit = agentInterface.InitUploadLogRequest(SerialNumber,
                                ModalConstants.Instance.GetConstant(ModalConstants.PB980_VENTILATOR), logName, timeStampedFileName, logFile, numberOfChunks, chunkSize, logFileDate, digitSigVal);

                    Log.Information($"BusinessServices::UpLoadLogFile InitUploadLogRequest resp: {uploadFileInit.Success}");
                    if (uploadFileInit.Success)
                    {
                        var bytesRead = 0;
                        var buffer = new byte[chunkSize];
                        var i = 0;

                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            WaitUntilOasisServerIsAvailable( TimeSpan.FromMinutes(Settings.OasisServerAvailabilityTimeoutInMinutes),
                                TimeSpan.FromMilliseconds(Settings.OasisServerStatusCheckIntervalInMilliseconds),
                                agentInterface.Session.AuthorizeResponse.DeviceTypeGUIDS.Values);

                            var digitalSig = GetSha256HashDigitalSig(buffer, 0, bytesRead);

                            Log.Information($"BusinessServices::UpLoadLogFile UploadChunk cnt: {i+1}");

                            var retryResp = agentInterface.UploadLogFileChunkRequest(logName, uploadFileInit.Value.TaskID, i + 1,
                                bytesRead, buffer, digitalSig);
                            Log.Information($"BusinessServices::UpLoadLogFile UploadChunk resp: {retryResp.Success}");

                            return retryResp.Success;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                Log.Information($"BusinessServices::UpLoadLogFile status:true");
                // Finished uploading the log, closed the file
                return true;
            }
            catch (IOException ioExp)
            {
                Log.Error($"BusinessServices::UpLoadLogFile failed to read the log file {logFile}, exception: {ioExp.Message}");
                return false;
            }
        }

        /// <summary>
        /// WaitUntilOasisServerIsAvailable
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="statusCheckInterval"></param>
        /// <param name="deviceTypeGuids"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private bool WaitUntilOasisServerIsAvailable(TimeSpan timeout, TimeSpan statusCheckInterval, IEnumerable<string> deviceTypeGuids)
        {
            Log.Information($"BusinessServices::WaitUntilOasisServerIsAvailable Status Entry");
            if (timeout <= TimeSpan.Zero)
            {
                Log.Error($"BusinessServices::WaitUntilOasisServerIsAvailable timeout must be greater than 0");
                throw new ArgumentException("timeout must be greater than 0");
            }

            var dueTime = DateTime.Now.Add(timeout);

            while (DateTime.Now < dueTime)
            {
                if (!GetServerStatusInProgress)
                {
                    GetServerStatusInProgress = true;
                    var statusResp = agentInterface.GetServerStatus(deviceTypeGuids);
                    GetServerStatusInProgress = false;

                    if (statusResp.Success)
                    {
                        IsServerAvialable = bool.Parse(statusResp.Value.ServerAvailable);
                        Log.Information($"BusinessServices::WaitUntilOasisServerIsAvailable GetServerStatus Rem: {statusResp.Value.RemainingSize.Value}:{statusResp.Value.RemainingTime.Value}, Upload:{statusResp.Value.UploadSizeRemaining}:{statusResp.Value.UploadTimeRemaining} ");
                        // ARON: This right here is a prime example of why you'd want to separate the OASIS Agent domain objects from
                        // our business domain objects.  OASIS for some reason transmits these values as strings, when they should be integers.
                        // Since we're using the OASIS domain objects here, we have to do stupid things like this; where we're inline converting strings to integers.
                        if (long.Parse(statusResp.Value.RemainingSize.Value) <= 0 ||
                            long.Parse(statusResp.Value.RemainingTime.Value) <= 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        Log.Error($"BusinessServices::WaitUntilOasisServerIsAvailable Failed Status: {statusResp.Message}");
                    }
                }
                Thread.Sleep(statusCheckInterval);
            }

            Log.Warning($"BusinessServices::WaitUntilOasisServerIsAvailable Timeout of '{timeout}' expired on {nameof(WaitUntilOasisServerIsAvailable)}");

            return false;
        }

        /// <summary>
        /// GetSha256HashDigitalSig
        /// </summary>
        /// <param name="input"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private string GetSha256HashDigitalSig(byte[] input, int? offset, int? length)
        {
            using (var sha256Hash = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                if (offset.HasValue && length.HasValue)
                {
                    var data = sha256Hash.ComputeHash(input, offset.Value, length.Value);
                    return BitConverter.ToString(data).Replace("-", "").ToLower();
                }
                else
                {
                    var data = sha256Hash.ComputeHash(input);
                    return BitConverter.ToString(data).Replace("-", "").ToLower();
                }
            }
        }
        /// <summary>
        /// Returns All Standard logs ,DeviceInfo is not counted in Standard logs
        /// </summary>
        public void GetDeviceLogNames(List<string> deviceLogNamesArray)
        {
            if (deviceLogNamesArray != null)
            {
                foreach (string logName in DeviceLogNames)
                {
                    deviceLogNamesArray.Add(logName);
                }
            }
        }

        /// <summary>
        /// Returns number of diagnostic logs in the retrieved set of logs
        /// </summary>
        /// <returns></returns>
        public int GetDeviceLogsCount(bool diagnosticLogsOnly)
        {
            int count = 0;
            foreach (string key in UploadPendingToServerLogs?.Keys)
            {
                LogCargoContainer logCargo = (LogCargoContainer)UploadPendingToServerLogs[key];
                if (diagnosticLogsOnly == true)
                {
                    if (IsDiagnosticDeviceLog(logCargo.Name) == true)
                    {
                        count++;
                    }
                }
                else
                {
                    count++;
                }

            }
            return count;
        }

        /// <summary>
        /// Checks to see if the log name is one of the diagnostic log names
        /// </summary>
        /// <param name="logCargo"></param>
        /// <returns></returns>
        private bool IsDiagnosticDeviceLog(string logName)
        {
            return (logName.Equals("SysDiag", StringComparison.OrdinalIgnoreCase) ||
                    logName.Equals("SysComm", StringComparison.OrdinalIgnoreCase) ||
                    logName.Equals("EstSst", StringComparison.OrdinalIgnoreCase));
        }


        /// <summary>
        /// This function is used to concatenate all logs into a single file on local hard drive as specified by the user
        ///  Uses business services bridge instance service
        /// </summary>
        /// <param name="fileNameWithPath">File Name with full path where the log contents need to be stored</param>
        public void SaveAllLogsToFile(string fileNameWithPath)
        {
            Device.SaveAllLogsToFile(fileNameWithPath);
        }

        /// <summary>
        /// This function is used to clear all logs on the device (PB980)
        ///  Uses business services bridge instance service
        /// </summary>
        public void ClearDeviceLogs()
        {
            Device.ClearDeviceLogs();
        }

        /// <summary>
        /// GetSoftwareDownloadList
        /// </summary>
        /// <returns></returns>
        public List<Software> GetSoftwareDownloadList()
        {
            return SoftwarePkgList;
        }

        /// <summary>
        /// Sends a download status message to RSA to RSS - started, installed, failed, not attempted
        /// </summary>
        /// <param name="statusString"></param>
        /// <param name="statusDetails">default '-', it is clearing the cached info.</param>
        private void SendDownloadStatusNotification(string statusString, string statusDetails="-")
        {
            var pkgInfo = GetPackageDetails(SoftwareDownloadPackageName);

            statusDetails= (pkgInfo != null) ? statusDetails : statusDetails + SW_UPDATE_LOCAL_PKG_WRN;
        
            agentInterface.UpdateAck(Device.DeviceInfo.SerialNumber, ModalConstants.PB980_VENTILATOR_GUID,
                                                             DeviceCountryCode, UpdateAckType.UpdateSoftware,
                                                             pkgInfo?.Name, pkgInfo?.PartNumber, pkgInfo?.Revision, statusString, statusDetails);
        }

        /// <summary>
        /// Splits the package into Partnumber and Revision.
        /// </summary>
        /// <param name="swpkgName"></param>
        /// <param name="partNumber">out param</param>
        /// <param name="revision">out param</param>
        private void SplitPartNumbRev(string swpkgName, out string partNumber, out string revision)
        {
            partNumber = revision = "";
            try
            {
                // swpkgName should contian Partnumber_Revision. If not, it will throw exception.  
                var pkgInfo = swpkgName.Split('_');
                partNumber = pkgInfo[pkgInfo.Length - 2];
                revision = pkgInfo[pkgInfo.Length - 1];
            }
            catch (Exception e)
            {
                Log.Error($"BusinessServices::SplitPartNumbRev failed to get details for {swpkgName} Exception:{e.Message}");
            }
        }

        /// <summary>
        /// Provides the Package details based on package name
        /// </summary>
        /// <param name="swpkgName"></param>
        /// <returns>Software</returns>
        private Software GetPackageDetails(string swpkgName)
        {
            Software softwarePkg = default;
            try
            {
                SplitPartNumbRev(swpkgName, out string partNumber, out string revision);
                if (partNumber != "" && revision != "")
                {
                    foreach (var pkg in SoftwarePkgList)
                    {
                        if (pkg.PartNumber == partNumber && pkg.Revision == revision)
                        {
                            softwarePkg = pkg;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"BusinessServices::GetPackageDetails failed to get details for {swpkgName} Exception:{e.Message}");
            }
            return softwarePkg;
        }

        /// <summary>
        /// OnDownloadComplete
        /// </summary>
        /// <param name="status"></param>
        private void OnDownloadComplete(string status)
        {
            Log.Information($"BusinessServices::OnDownloadComplete, entry Status: {status}");
            bool failure = false;
            string reportedStatus;
            if (status.ToLower().Contains("success"))
            {
                reportedStatus = "installed";
            }
            else
            {
                reportedStatus = "failed";
                failure = true;
            }

            DownloadShellNet.Dispose();

            if (DownloadInProgress == true)
            {
                SendDownloadStatusNotification(reportedStatus);
            }

            DownloadInProgress = false;

            CleanupDownloadDirs();

            if (failure == true)
            {
                DownloadFailedCallback?.Invoke(status);
            }
            else
            {
                DownloadCompleteCallback?.Invoke();
            }
            Log.Information($"BusinessServices::OnDownloadComplete, exit Status: {status}");
        }

        /// <summary>
        /// resets the working directory of the application to the default working directory.
        /// when a device update happens, the Downloader.dll changes the working directory of the application to download directory
        /// this function is called, once the download is complete, to reset the working directory in order to try and delete download directory
        /// </summary>
        private void ResetWorkingDirectory()
        {
            try
            {
                Directory.SetCurrentDirectory(ESSWorkingDirectory);
            }
            catch (Exception e)
            {
                Log.Error($"BusinessServices::ResetWorkingDirectory:: Exception Occurred:{e.Message}");
            }
        }

        /// <summary>
        /// deletes the contents of the directory from which device update is performed
        /// the sub-folders are deleted, to ensure that PB980 software is deleted
        /// currently, the output.txt handle is closed after the downloader threads exit.
        /// due to that, some times, the function could throw an exception, but, 
        /// subsequent calls to to this function cleanup the contents completely.
        /// </summary>
        private void DeleteTempSwDownloadDirectory()
        {
            FileSystemUtil.DeleteDirectory(Settings.TempSoftwareDownloadPath);           
        }

        /// <summary>
        /// deletes the tempPackages directory folder
        /// </summary>
        private void DeleteTempPkgDirectory()
        {
            FileSystemUtil.DeleteDirectory(Settings.TempPkgStorePath);
        }

        /// <summary>
        /// OnStatusComponentUpdate
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="component"></param>
        /// <param name="message"></param>
        private void OnStatusComponentUpdate(string cpu, string component, string message)
        {
            Log.Information($"BusinessServices::OnStatusComponentUpdate cpu:{cpu}, cmp: {component}, msg : {message}");
            DownloadProgressCallback?.Invoke(cpu, component, message);
        }

        private void OnStartDownload(string cpu)
        {
            Log.Information($"BusinessServices::OnStartDownload cpu:{cpu}");
            DownloadStartedCallback?.Invoke(cpu);
        }

        private void CheckDownloadMode()
        {
            Log.Information($"BusinessServices::CheckDownloadMode entry");
            try
            {
                bool isReady = false;
                //create new
                DownloadShellNet = new DownloadShell_Net.DownloadShell_Net(Settings.TempSoftwareDownloadPath);
                DownloadShellNet.ListenForConfig();

                int nTries = 30; //1 minute

                while ((nTries > 0) && (DownloadShellNet.IsDeviceInfoRetrieved() == false))
                {
                    Thread.Sleep(2000);
                    --nTries;
                }

                //collect as much device information in  minute and check to see if we have information
                isReady = DownloadShellNet.IsDeviceInfoRetrieved();

                Hashtable deviceInfoTable = DownloadShellNet.GetDeviceInfo();
                isReady = ((deviceInfoTable != null) && (deviceInfoTable.Count > 0));

                //dispose off the object
                DownloadShellNet.Dispose();

                //resetting current working directory as downloader library changes it
                CleanupDownloadDirs();

                string deviceInfoXmlInDownloadMode = "";
                if (isReady == true)
                {
                    deviceInfoXmlInDownloadMode = BuildDeviceInfoInDownloadMode(deviceInfoTable);
                }

                if (string.IsNullOrEmpty(deviceInfoXmlInDownloadMode) == false)
                {
                    IsDeviceInDownloadMode = true;

                    Log.Information($"BusinessServices::CheckDownloadMode {deviceInfoXmlInDownloadMode}");

                    try
                    {
                        DeviceInformation deviceInfo = DeviceInformation.LoadDeviceInfo(deviceInfoXmlInDownloadMode);

                        //set the device information in Device.DeviceInfo, issue getstat etc.
                        ProcessDeviceInformation("transactionId", true, deviceInfo);
                    }
                    catch
                    {
                        Device.DeviceInfo = null;
                    }

                    InformDeviceConnectionStatus();
                }
                else
                {
                    Log.Error($"BusinessServices::CheckDownloadMode invalid mode:{deviceInfoXmlInDownloadMode}");
                    ConnectCallback?.Invoke(false);
                }
            }
            catch (Exception e)
            {
                Log.Error($"BusinessServices::CheckDownloadMode Exception:{e.Message}");
                ConnectCallback?.Invoke(false);
            }
            Log.Information($"BusinessServices::CheckDownloadMode exit");
        }

        /// <summary>
        /// BuildDeviceInfoInDownloadMode
        /// </summary>
        /// <param name="deviceInfoTable"></param>
        /// <returns></returns>
        private string BuildDeviceInfoInDownloadMode(Hashtable deviceInfoTable)
        {
            //get components
            string deviceInfoXmlInDownloadMode = "<body>";

            //decode device info
            if (deviceInfoTable.Count > 0)
            {
                string BdConfig = (string)deviceInfoTable["BD"];
                string GuiConfig = (string)deviceInfoTable["GUI"];

                if (string.IsNullOrEmpty(BdConfig) == false)
                {
                    string[] bdSTringArray = BdConfig.Split('|');

                    //PB980|SERIAL#|BdPcbA 10101 20202|PB.BD.FLASH.NORMAL 10101 20202|BdSoftware 10101 20202
                    string serialNumber = bdSTringArray[1].Trim();
                    deviceInfoXmlInDownloadMode += string.Format("<device_identity><model>{0}</model><serial_number>{1}</serial_number></device_identity>", bdSTringArray[0].Trim(), serialNumber);

                    deviceInfoXmlInDownloadMode += "<components>";

                    string trimmedStringBd = bdSTringArray[2].Trim();
                    string[] bdComponentArray = trimmedStringBd.Split(' ');
                    deviceInfoXmlInDownloadMode += string.Format("<component type='HARDWARE'><name>{0}</name><part_number>{1}</part_number><revision>{2}</revision></component>",
                                                                    bdComponentArray[0], bdComponentArray[1], bdComponentArray[2]);
                    trimmedStringBd = bdSTringArray[3].Trim();
                    bdComponentArray = trimmedStringBd.Split(' ');
                    deviceInfoXmlInDownloadMode += string.Format("<component type='SOFTWARE'><name>{0}</name><part_number>{1}</part_number><revision>{2}</revision></component>",
                                                                    bdComponentArray[0], bdComponentArray[1], bdComponentArray[2]);

                    trimmedStringBd = bdSTringArray[4].Trim();
                    bdComponentArray = trimmedStringBd.Split(' ');
                    deviceInfoXmlInDownloadMode += string.Format("<component type='SOFTWARE'><name>{0}</name><part_number>{1}</part_number><revision>{2}</revision></component>",
                                                                    bdComponentArray[0], bdComponentArray[1], bdComponentArray[2]);

                    deviceInfoXmlInDownloadMode += string.Format("<component type='HARDWARE'><name>MasterVent</name><serial_number>{0}</serial_number><part_number>{1}</part_number><revision>{2}</revision></component>", serialNumber, bdComponentArray[1], bdComponentArray[2]);
                }

                if (string.IsNullOrEmpty(GuiConfig) == false)
                {
                    //GuiPcbA 10101 20202|PB.GUI.FLASH.NORMAL 10101 20202
                    string[] guiSTringArray = GuiConfig.Split('|');
                    string trimmedStringGui = guiSTringArray[0].Trim();
                    string[] guiComponentArray = trimmedStringGui.Split(' ');
                    deviceInfoXmlInDownloadMode += string.Format("<component type='HARDWARE'><name>{0}</name><part_number>{1}</part_number><revision>{2}</revision></component>",
                                                                    guiComponentArray[0], guiComponentArray[1], guiComponentArray[2]);

                    trimmedStringGui = guiSTringArray[1].Trim();
                    guiComponentArray = trimmedStringGui.Split(' ');
                    deviceInfoXmlInDownloadMode += string.Format("<component type='SOFTWARE'><name>{0}</name><part_number>{1}</part_number><revision>{2}</revision></component>",
                                                                    guiComponentArray[0], guiComponentArray[1], guiComponentArray[2]);
                }

                if((string.IsNullOrEmpty(BdConfig) == false) || (string.IsNullOrEmpty(GuiConfig) == false))
                    deviceInfoXmlInDownloadMode += "</components>";
            }

            deviceInfoXmlInDownloadMode += "</body>";

            return deviceInfoXmlInDownloadMode;
        }

        private void InformDeviceConnectionStatus()
        {
            // Code WILL work for ESS, but maybe not other devices.  Works because VTS is serial in interacting with the device.
            //we are connected when a) we get device info or b) in download mode
            if ((null != Device.DeviceInfo))
            {
                //connected and obtained device info - overall success
                Log.Information("BusinessServices::InformDeviceConnectionStatus Success");
                DeviceConnected = true;

                ConnectCallback?.Invoke(true);

                SessionCreateCallback?.Invoke("transactionId", true, "sessionId");
            }
            else
            {
                Log.Error("BusinessServices::InformDeviceConnectionStatus Failed to get Device Info");
                ConnectCallback?.Invoke(false);
            }
        }

        /// <summary>
        /// FetchSoftwareDownloadPackageSync
        /// </summary>
        /// <param name="part_Rev"></param>
        /// <param name="targetPackagePath"></param>
        /// <returns></returns>
        public bool FetchSoftwareDownloadPackageSync(string part_Rev, string targetPackagePath)
        {
            try
            {
                var packgeOid = GetPackageDetails(part_Rev)?.Oid;

                Log.Information($"BusinessServices::FetchSoftwareDownloadPackageSync Downloading package {packgeOid}");
                // This only fetches the response object, now we need to store the results
                var task = Task.Run(() => agentInterface.FetchSoftwarePackage(packgeOid));
                task.Wait();
                var response = task.Result;
                Log.Information($"BusinessServices::FetchSoftwareDownloadPackageSync package {packgeOid} status {response.Success}");
                if (response.Success)
                {
                    FileSystemUtil.DeleteDirectory(Settings.TempPkgStorePath);
                    FileSystemUtil.CreateFileIfNotExists(Settings.TempPkgStorePath);

                    File.WriteAllBytes(Settings.TempPkgStorePath, response.Value.Bytes);

                    if (UnarchiveAndValidatePackageContents(Settings.TempPkgStorePath, targetPackagePath))
                    {
                        return true;
                    }
                    else
                    {
                        Log.Error($"BusinessServices::FetchSoftwareDownloadPackageSync failed to Unpack/Verify {packgeOid} : {targetPackagePath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"BusinessServices::FetchSoftwareDownloadPackageSync Exception:{ex.Message}");
            }
            return false;
        }

        /// <summary>
        /// DoSoftwareDownload
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isLocalBrowsedPackage"></param>
        /// <param name="downloadStartCallback"></param>
        /// <param name="progressCallback"></param>
        /// <param name="doneCallback"></param>
        /// <param name="failedCallback"></param>
        /// <param name="otherpackagedownloadcallback"></param>
        public void DoSoftwareDownload(string name, bool isLocalBrowsedPackage, UserInterfaceDelegates.FlashProcessStartedCallback downloadStartCallback, UserInterfaceDelegates.FlashProgressCallback progressCallback, UserInterfaceDelegates.FlashProcessCompleteCallback doneCallback, UserInterfaceDelegates.DeviceFlashFailedCallback failedCallback,UserInterfaceDelegates.OtherPackageDownloadCallback otherpackagedownloadcallback)
        {
            Log.Information($"BusinessServices::DoSoftwareDownload entry pkg:{name}, local:{isLocalBrowsedPackage}");
            DownloadStartedCallback = downloadStartCallback;
            DownloadProgressCallback = progressCallback;
            DownloadCompleteCallback = doneCallback;
            DownloadFailedCallback = failedCallback;
            OtherPkgDownloadCallback = otherpackagedownloadcallback;

            SoftwareDownloadPackageName = name;
            var pkgUnarchiveStatus = false;
            var targetDir = Settings.TempSoftwareDownloadPath;
            
            //Extracting and Validating Package
            if (isLocalBrowsedPackage && File.Exists(SoftwareDownloadPackageName))
            {
                pkgUnarchiveStatus = UnarchiveAndValidatePackageContents(SoftwareDownloadPackageName, targetDir);

                // Extracting SW Package Name from local selected file path after package extracting is completed.
                SoftwareDownloadPackageName = Path.GetFileNameWithoutExtension(name);
            }
            else if (SoftwarePkgList != null && SoftwarePkgList.Count > 0)
            {
                pkgUnarchiveStatus = FetchSoftwareDownloadPackageSync(SoftwareDownloadPackageName, targetDir);
            }

            //Downloading SW to Vent
            if (pkgUnarchiveStatus)
            {
                ContinueSoftwareDownload(targetDir);
            }
            else
            {   //unarchive or check failed, clear temp download dir
                DeleteTempPkgDirectory();
                DownloadFailedCallback?.Invoke("Invalid Update Package");
                Log.Error($"BusinessServices::DoSoftwareDownload Error not starting download operation");
            }
            Log.Information($"BusinessServices::DoSoftwareDownload exit pkg:{name}");
        }

        /// <summary>
        /// a) The function copies contents from TempPackages folder into a specific package folder
        /// b) Specific package folder is cleaned up before the copy operation is performed
        /// b) Returns success or failure 
        /// </summary>
        private bool SaveOtherPackage(string sourcePath)
        {
            Log.Information($"BusinessServices::SaveOtherPackage entry path:{sourcePath}");
            //path to save package
            OtherSoftwarePkgSavedPath = GetSoftwareSavePath(Settings.SoftwareSavedPath, SoftwareDownloadPackageName);
            try
            {
                //delete the destination so that we are always starting clean
                FileSystemUtil.DeleteDirectory(OtherSoftwarePkgSavedPath);

                //Moving to Saved Packages Path
                FileSystemUtil.DirectoryCopy(sourcePath, OtherSoftwarePkgSavedPath, true);

                Log.Information($"BusinessServices::SaveOtherPackage exit");
                return true;
            }
            catch (Exception e)
            {
                // cleanup download directory for incomplete or erratic copies
                FileSystemUtil.DeleteDirectory(OtherSoftwarePkgSavedPath);
                Log.Error($"BusinessServices::SaveOtherPackage Exception:{e.Message}");
                return false;
            }
        }

       /// <summary>
        /// a) The function creates the Downloader instance
        /// b) This function interacts with the device to download software into the device asynchronoulsy
        /// c) Sends a download started notification to the server
        /// </summary>
        private void StartDeviceUpdate(string targetDir)
        {
            Log.Information($"BusinessServices::StartDeviceUpdate entry path:{targetDir}");
            // Then do the download
            DownloadShellNet = new DownloadShell_Net.DownloadShell_Net(targetDir);

            DownloadShellNet.OnStartDownload += new DownloadShell_Net.OnStartDownloadDelegate(OnStartDownload);
            DownloadShellNet.OnDownloadStatusUpdate += new DownloadShell_Net.OnStatusComponentUpdateDelegate(OnStatusComponentUpdate);
            DownloadShellNet.OnDownloadComplete += new DownloadShell_Net.OnDownloadCompleteDelegate(OnDownloadComplete);

            DownloadShellNet.Start();

            DownloadInProgress = true;

            DeviceInterface.StopServerMonitoring();
            SendDownloadStatusNotification(SW_UPDATE_STARTED_MSG);
            Log.Information($"BusinessServices::StartDeviceUpdate exit");
        }

        /// <summary>
        /// a) Machine check is performed to check if the software downloading from server 
        /// is done for the vent which is connected in the beginning
        /// b) The Device information is retrieved from the vent synchronously and checked 
        /// with the device information obtained during connection
        /// c) Failure is returned when device information does not match
        /// </summary>
        private bool PerformMachineCheck()
        {
            Log.Information($"BusinessServices::PerformMachineCheck entry");

            bool retVal = true;
            try
            {
                if (!IsDeviceInDownloadMode)
                {
                    //do a serial number check - so that the software retrieved for is same as the device being upgraded
                    DeviceInformation currentDeviceInfo = Device.GetDeviceInformationSync();
                    if ((currentDeviceInfo == null) ||
                        (string.Equals(currentDeviceInfo.SerialNumber, Device.DeviceInfo.SerialNumber) == false))
                    {
                        retVal = false;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"BusinessServices::PerformMachineCheck Exception: {e.Message}");
                retVal = false;
            }

            Log.Information($"BusinessServices::PerformMachineCheck exit ret:{retVal}");
            return retVal;
        }

        /// <summary>
        ///     a) Unarchives package contents from the server provided package into TempPackage directory
        ///     b) Once unarchived, the content is checked for valid directories and manifest file presence
        ///     c) Failure is returned whenever one of the above steps fails.
        /// </summary>
        private bool UnarchiveAndValidatePackageContents(string srcPath, string destinationPath)
        {
            Log.Information($"BusinessServices::UnarchiveAndValidatePackageContents, Entry");
            bool retVal;
            try
            {
                // First inflate the gz thing from loc A to loc B
                var ou = File.ReadAllBytes(srcPath);

                FileSystemUtil.DeleteDirectory(destinationPath);

                // download to temp storage path
                SoftwareManifestUtil.UnarchiveToFolder(destinationPath, SoftwareManifestUtil.Gunzip(ou));
                retVal = CheckDownloadPackageValidity(destinationPath);
            }
            catch (Exception e)
            {
                Log.Error($"BusinessServices::UnarchivePackageContents Exception: {e.Message}");
                retVal = false;
            }
            Log.Information($"BusinessServices::UnarchiveAndValidatePackageContents, exit ret:{retVal}");
            return retVal;
        }

        /// <summary>
        /// a) The function which processes he unarchived package contents inside TempPackage directory
        /// b) It reads the tag inside download.xml and either 
        ///       i) copies the "othersoftware" to local file system synchronously from TempPackage directory
        ///       ii) copies the "ventilatorsoftware" from TempPackage to VikingTftpRoot directory and start the device update asynchronously
        /// c) Deletes the TempPackage directory once the contents are moved
        /// d) Callbacks with return codes or message are called appropirately
        /// </summary>
        private void ProcessObtainedPackage(string targetDir)
        {
            Log.Information($"BusinessServices::ProcessObtainedPackage, entry path:{targetDir}");

            //check what kind of download content it is - vent software, other software
            string configfilePath = GetManifestFileNameWithPath(targetDir);
            string node = GetConfigFileTag(configfilePath);

            if (!(string.IsNullOrEmpty(node)) && node.Equals(OTHER_PACKAGE, StringComparison.OrdinalIgnoreCase))
            {
                //this is the other third party software
                //move it to other software saved packages folder
                bool status = SaveOtherPackage(targetDir);

                //cleanup temp storage folder contents after save to a different location
                DeleteTempPkgDirectory();

                OtherPkgDownloadCallback?.Invoke(status);
            }
            else
            {
                StartDeviceUpdate(targetDir);
            }
            Log.Information($"BusinessServices::ProcessObtainedPackage, exit");
        }

        /// <summary>
        /// a) Performs machine check
        /// b) Unpacks the content into tempDownloadDir. 
        /// c) If OtherSoftware, Copies the contents from tempDownloadDir to SavedPackages dir
        /// d) Else, copies the contents from tempDownloadDir to DownloadDir and starts the download
        /// Cleans up tempDownloadDir
        /// </summary>
        private void ContinueSoftwareDownload(string targetDir)
        {
            Log.Information($"BusinessServices::ContinueSoftwareDownload, entry path:{targetDir}");

            //step 1 - machine to be updated should be the same as the one to ESS is docked
            if (PerformMachineCheck() == true)
            {
                //step 2 - final step, well-formed package - save or perform device update
                ProcessObtainedPackage(targetDir);
            }
            else
            {
                //unarchive or check failed, clear temp download dir
                DeleteTempPkgDirectory();
                DownloadFailedCallback?.Invoke("Machine Check Failed");
            }
            Log.Information($"BusinessServices::ContinueSoftwareDownload, exit");
        }

        /// <summary>
        /// a) Reads manifest file, download.xml, contents in XML format, 
        /// b) Returns the root node of the manifest file or null in case of failure
        /// </summary>
        public static string GetConfigFileTag(string configfilePath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                string fileText = File.ReadAllText(configfilePath);
                fileText = fileText.Trim();
                var validXMlChars = fileText.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray();
                string fileTextValidXml = new string(validXMlChars);
                doc.LoadXml(fileTextValidXml);
                doc.PreserveWhitespace = false;
                
                return doc.DocumentElement.Name;
            }
            catch(Exception e)
            {
                Log.Error($"BusinessServices::GetConfigFileTag, path:{configfilePath} Exception:{e.Message}");
                return null;
            }
        }

        /// <summary>
        /// a) Does basic package content checks.
        /// b) Returns true when config/, bin/ folders are found and that config/download.xml file is present 
        /// </summary>
        public bool CheckDownloadPackageValidity(string dirPath)
        {
            //rules are the packge should contain download.xml in config\ dir
            string binDirectoryPath = Path.Combine(dirPath, Settings.BinariesFolder);
            string configDirectoryPath = Path.Combine(dirPath, Settings.ConfigFolder);
            string configfilePath = GetManifestFileNameWithPath(dirPath);

            return (Directory.Exists(binDirectoryPath) && Directory.Exists(configDirectoryPath) && CheckDownloadPackageConfigfileValidity(configfilePath));
        }

        /// <summary>
        /// a) Returns manifest file name with path information based on the passed-in root directory.
        /// </summary>
        public string GetManifestFileNameWithPath(string pkgDir)
        {
            return (Path.Combine(pkgDir, Settings.DownloadManifestFile));
        }

        /// <summary>
        /// a) Returns a fullly formed software save directory path, based on a root dir and a package name
        /// </summary>
        public static string GetSoftwareSavePath(string rootDir, string packageName)
        {
            //path to save package
            return Path.Combine(rootDir, Path.GetFileNameWithoutExtension(packageName));
        }

        /// <summary>
        /// CheckDownloadPackageConfigfileValidity
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static bool CheckDownloadPackageConfigfileValidity(string filepath)
        {
            return (File.Exists(filepath));
        }

        /// <summary>
        /// ExtractDocumentsFromComponents
        /// </summary>
        /// <param name="docs"></param>
        /// <param name="deviceInformation"></param>
        /// <returns></returns>
        private Hashtable ExtractDocumentsFromComponents( Hashtable docs, DeviceInformation deviceInformation )
        {
            if  ( ( null != deviceInformation )  &&  ( null != deviceInformation.Components ) )
            {
                if  ( null == docs )
                {
                    docs = new Hashtable();
                }

                // go through the list of documents by their "name" (and they must also have a "location"
                foreach( DeviceComponentInfo compInfo in deviceInformation.Components )
                {
                    if  ( 0 == string.Compare( "DOCUMENT", compInfo.Type, StringComparison.InvariantCulture ) )
                    {
                        if  ( !string.IsNullOrEmpty( compInfo.Name ) )
                        {
                            if  ( !docs.ContainsKey( compInfo.Name ) )
                            {
                                foreach( KeyValuePair<string, string> kvp in compInfo.OtherAttributes )
                                {
                                    if  ( 0 == string.Compare( "LOCATION", kvp.Key ) )
                                    {
                                        docs.Add( compInfo.Name, kvp.Value ) ;
                                    }
                                    // else wrong attribute, so ignore it
                                }
                            }
                            // else already exists, so do not add again.
                        }
                        // else no name, so ignore it
                    }
                    // else not a document, so ignore it
                }
            }

            return( docs ) ;
        }



        /// <summary>
        /// Adding this whole new wing of code to support manual entry of device ids to pre-fetch information
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<string,string>> GetKnownDevices()
        {
            List<KeyValuePair<string,string>> knownDevices = new List<KeyValuePair<string, string>>() ;

            foreach( Hashtable deviceList in DeviceListsByType.Values )
            {
                foreach( DeviceDataFromServer deviceData in deviceList.Values )
                {
                    KeyValuePair<string, string> devId = new KeyValuePair<string, string>(deviceData.DeviceType, deviceData.SerialNumber);
                    knownDevices.Add( devId );
                }
            }

            return( knownDevices ) ;
        }

        /// <summary>
        /// list of device types, list of software releases, list of documents (name, location).
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<string, List<KeyValuePair<string, List<KeyValuePair<string, string>>>>>> GetKnownDocuments()
        {
            Hashtable devTypes = new Hashtable();

            foreach( string devTypeStr in DeviceListsByType.Keys )
            {
                Hashtable deviceList = (Hashtable) DeviceListsByType[ devTypeStr ] ;

                Hashtable releases;

                if  ( devTypes.ContainsKey( devTypeStr ) )
                {
                    releases = (Hashtable) devTypes[ devTypeStr ] ;
                }
                else
                {
                    releases = new Hashtable();
                    devTypes.Add( devTypeStr, releases ) ;
                }

                foreach( DeviceDataFromServer deviceData in deviceList.Values )
                {
                    // check the device data itself
                    if  ( ( null != deviceData.DeviceInfo )  &&  !string.IsNullOrEmpty( deviceData.DeviceInfo.SoftwareVersion ) )
                    {
                        Hashtable releaseDocs;

                        if  ( releases.ContainsKey( deviceData.DeviceInfo.SoftwareVersion ) )
                        {
                            releaseDocs = (Hashtable) releases[ deviceData.DeviceInfo.SoftwareVersion ] ;
                        }
                        else
                        {
                            releaseDocs = new Hashtable();
                            releases.Add( deviceData.DeviceInfo.SoftwareVersion, releaseDocs ) ;
                        }

                        // ignore the return value
                        ExtractDocumentsFromComponents( releaseDocs, deviceData.DeviceInfo ) ;
                    }

                    // check the available software updates
                    if  ( null != deviceData.SoftwareUpdates )
                    foreach( SoftwareUpdateInfo softwareUpdateInfo in deviceData.SoftwareUpdates )
                    {
                        Hashtable releaseDocs;

                        if  ( releases.ContainsKey( softwareUpdateInfo.Info ) )
                        {
                            releaseDocs = (Hashtable) releases[ softwareUpdateInfo.Info ] ;
                        }
                        else
                        {
                            releaseDocs = new Hashtable();
                            releases.Add( softwareUpdateInfo.Info, releaseDocs ) ;
                        }

                        if  ( null != softwareUpdateInfo.DocumentPackages )
                        foreach( DocumentPackage documentPackage in softwareUpdateInfo.DocumentPackages )
                        {
                            if  ( !string.IsNullOrEmpty( documentPackage.Name )  &&  !string.IsNullOrEmpty( documentPackage.Location ) )
                            {
                                releaseDocs.Add( documentPackage.Name, documentPackage.Location ) ;
                            }
                        }
                    }
                }
            }

            List<KeyValuePair<string, List<KeyValuePair<string, List<KeyValuePair<string, string>>>>>> retval = new List<KeyValuePair<string, List<KeyValuePair<string, List<KeyValuePair<string, string>>>>>>();
                
            foreach( string devTypeStr in devTypes.Keys )
            {
                List<KeyValuePair<string, List<KeyValuePair<string, string>>>> devTypeList = new List<KeyValuePair<string, List<KeyValuePair<string, string>>>>();

                int cntR = 0;
                Hashtable releases = (Hashtable) devTypes[ devTypeStr ] ;
                foreach( string releaseName in releases.Keys )
                {
                    List<KeyValuePair<string, string>> releaseDocList = new List<KeyValuePair<string, string>>();

                    int cntD = 0;
                    Hashtable releaseDocs = (Hashtable)releases[releaseName];
                    foreach( string name in releaseDocs.Keys )
                    {
                        cntD++;
                        releaseDocList.Add( new KeyValuePair<string, string>( name, (string) releaseDocs[ name ] ) ) ;
                    }
                    if  ( 0 < cntD )  // only add the item if there is anything to add.
                    {
                        cntR++;
                        devTypeList.Add( new KeyValuePair<string, List<KeyValuePair<string, string>>>( releaseName, releaseDocList ) ) ;
                    }
                }
                if  ( 0 < cntR )  // only add the item if there is anything to add.
                {
                    retval.Add( new KeyValuePair<string, List<KeyValuePair<string, List<KeyValuePair<string, string>>>>>( devTypeStr, devTypeList ) ) ;
                }
            }

            return( retval ) ;
        }


        // NOTE: we DO NOT continually try to re-sync with server.  Once you have data (or have started the process), that is the snapshot that you get.
        /// <summary>
        /// GetDeviceDataFromServer
        /// </summary>
        /// <param name="devType"></param>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public DeviceDataFromServer GetDeviceDataFromServer( string devType, string serialNumber )
        {
            Hashtable DeviceListBySerialNumber;
            if  ( DeviceListsByType.ContainsKey( devType ) )
            {
                DeviceListBySerialNumber = (Hashtable) DeviceListsByType[ devType ] ;
            }
            else
            {
                DeviceListBySerialNumber = new Hashtable();
                DeviceListsByType.Add( devType, DeviceListBySerialNumber ) ;
            }

            if  ( DeviceListBySerialNumber.ContainsKey( serialNumber ) )
            {
                return( (DeviceDataFromServer) DeviceListBySerialNumber[ serialNumber ] ) ;
            }
            else  // first time, begin the long journey to get all the relevant info
            {
                DeviceDataFromServer deviceData = new DeviceDataFromServer( devType, serialNumber ) ;
                DeviceListBySerialNumber.Add( serialNumber, deviceData ) ;

                return( deviceData ) ;
            }
        }

        /// <summary>
        /// ClearDeviceLogs
        /// </summary>
        /// <param name="clearDeviceLogsCallback"></param>
        public void ClearDeviceLogs(UserInterfaceDelegates.ClearDeviceLogsCallback clearDeviceLogsCallback)
        {
            ClearDeviceLogsCallbackEvent = clearDeviceLogsCallback;
            Device.ClearDeviceLogs(ClearDeviceLogsCallbackEvent);
        }

        /// <summary>
        /// User disconnect from device
        /// </summary>
        public void DisconnectDevice()
        {
            StopSoftwareDownloadProcess();
            DisconnectFromDevice();
        }

        /// <summary>
        /// Stop the software download process if already started
        /// </summary>
        public void StopSoftwareDownloadProcess()
        {
            //if software download in progress, send "failed" message because the device state has changed
            if (DownloadInProgress == true)
            {
                DownloadShellNet.Stop();
                DownloadShellNet.Dispose();

                SendDownloadStatusNotification(SW_UPDATE_FAILED_MSG);
                DownloadInProgress = false;
                
                CleanupDownloadDirs();
            }
        }

        /// <summary>
        /// SetDeviceCountryCode
        /// </summary>
        /// <param name="countryCode"></param>
        public void SetDeviceCountryCode(string countryCode)
        {
            DeviceCountryCode = countryCode;
        }

        /// <summary>
        /// AuthorizeToServerUsingOfflinePasscode
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passcode"></param>
        /// <returns></returns>
        public AuthorizeResp AuthorizeToServerUsingOfflinePasscode(string userId, string passcode)
        {
            OfflineUserName = userId;
            agentInterface.AuthorizeServer(null, null, userId, passcode, oasisCredentialInformation.EssClientAppId,
                            oasisCredentialInformation.EssAgentMachineId, oasisCredentialInformation.EssVersion, oasisCredentialInformation.HostName,
                            oasisCredentialInformation.HostId, null);
            Log.Information($"BusinessServices::AuthorizeToServerUsingOfflinePasscode user: {userId} Resp: {agentInterface.Session.AuthorizeResponse}");
            if (agentInterface.Session.AuthorizeResponse != null && agentInterface.Session.AuthorizeResponse.Result.Success)
            {
                RestrictedFunctionManager.SetUserLoggedIn(true);
            }
            return agentInterface.Session.AuthorizeResponse;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="essSettings"></param>
        /// <returns></returns>
        public bool EstablishSession()
        {
            Log.Information($"BusinessServices::EstablishSession entry host:{IdentificationHelper.GetHostId()} hostname:{IdentificationHelper.GetHostName()}");
            IRestHelper restHelper = new RestHelper();
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            sessionManager = sessionManager ?? new SessionManager(Settings, restHelper, jsonSerializerSettings);
            certificateService = certificateService?? new CertificateService(Settings);
            oasisCredentialInformation = new OasisCredentialInformation
            {
                EssClientAppId = Settings.EssClientAppId,
                EssAgentMachineId = IdentificationHelper.GetHostId(),
                EssVersion = VersionHelper.ExecutingAssemblyVersion,
                HostId = IdentificationHelper.GetHostId(),
                HostName = IdentificationHelper.GetHostName()
            };
            try
            {
                agentInterface = sessionManager.EstablishSession(oasisCredentialInformation, clientCertificate, ServerCertificateCustomValidationCallback);
            }
            catch (Exception ex)
            {
                Log.Error($"BusinessServices::EstablishSession Exception:{ex.Message}");
                MessageBox.Show(RSA_SERVER_NOT_RUNNING);
                Application.Exit();
            }
            Log.Information($"BusinessServices::EstablishSession exit status:{agentInterface != null}");
            return agentInterface != null;
        }

        /// <summary>
        /// IsRSASessionOpened
        /// </summary>
        /// <returns></returns>
        public bool IsRSASessionOpened()
        {
            return sessionManager.IsRSASessionOpened();
        }

        /// <summary>
        /// CloseSession
        /// </summary>
        public void CloseSession()
        {
            sessionManager.CloseSession();
        }

        /// <summary>
        ///     RetrieveSoftwarePackagesFromServerSync()
        ///     This method fetches the Software Packages List from OASIS server.
        /// </summary>
        /// <param name="resp">Software List Reference. </param>
        /// <returns>bool</returns>
        public bool RetrieveSoftwarePackagesFromServer(UserInterfaceDelegates.SoftwareListRetrievalCompleteCallback getSoftwareFromServerCallback)
        {
            if (!IsRetrieveSoftwareRunning && !IsLogUploadRunning)
            {
                Task.Run(() => RetrieveSoftwarePackagesFromServerWorker(getSoftwareFromServerCallback));
                return true;
            }
            else
            {
                Log.Warning($"BusinessServices::RetrieveSoftwarePackagesFromServer: Software fetching:{IsRetrieveSoftwareRunning}, {IsLogUploadRunning} is already in progress. Ignoring this request.");
            }
            return false;
        }

        private void SoftwareListRetrievalCompleteCB(List<Software> softwarePkgList)
        {
            AllNotificationsProcessedCallback();
        }

        /// <summary>
        /// RetrieveSoftwarePackagesFromServerWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RetrieveSoftwarePackagesFromServerWorker(UserInterfaceDelegates.SoftwareListRetrievalCompleteCallback getSoftwareFromServerCallback)
        {
            IsRetrieveSoftwareRunning = true;

            Log.Information($"BusinessServices::RetrieveSoftwarePackagesFromServerWorker entry");

            string devSerialNo = (Device.IsDummyDeviceConnected == true) ? Settings.DummyVentSerialNo: Device?.DeviceInfo?.SerialNumber;

            if (devSerialNo == null)
            {
                Log.Error($"BusinessServices::RetrieveSoftwarePackagesFromServerWorker exit, Device Information is either NULL or not valid.");
                IsRetrieveSoftwareRunning = false;
            }
            else
            {
                try
                {
                    if (IsAuthenticationCompleted())
                    {
                        Log.Information($"BusinessServices::RetrieveSoftwarePackagesFromServerWorker Waiting for Server status");
                        WaitUntilOasisServerIsAvailable(TimeSpan.FromMinutes(Settings.OasisServerAvailabilityTimeoutInMinutes),
                                            TimeSpan.FromMilliseconds(Settings.OasisServerStatusCheckIntervalInMilliseconds),
                                            agentInterface.Session.AuthorizeResponse.DeviceTypeGUIDS.Values);

                        Response<SoftwareOidResp> softwareOidResp = default;
                        for (var retryAttempts = 0; retryAttempts < Settings.OasisAgentMaxConnectAttempts; retryAttempts++)
                        {
                            agentInterface.StatDevice(devSerialNo, ModalConstants.PB980_VENTILATOR_GUID, DeviceCountryCode);

                            Log.Information($"BusinessServices::RetrieveSoftwarePackagesFromServerWorker GetSoftwareOid calling");
                            softwareOidResp = agentInterface.GetSoftwareOid(devSerialNo, ModalConstants.PB980_VENTILATOR_GUID);
                            if (softwareOidResp.Success)
                            {
                                SoftwarePkgList = softwareOidResp.Value.SoftwareList;
                                getSoftwareFromServerCallback(SoftwarePkgList);
                                break;
                            }
                            else
                            {
                                Log.Error($"BusinessServices::RetrieveSoftwarePackagesFromServerWorker Call to agent interface '{nameof(agentInterface.GetSoftwareOid)}' failed with message: {softwareOidResp.Message}");
                            }
                        }

                        var info = softwareOidResp != default ? softwareOidResp.Message : "";
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"BusinessServices::RetrieveSoftwarePackagesFromServerWorker Exception: {ex.Message}");
                }
                finally
                {
                    IsRetrieveSoftwareRunning = false;
                }
            }
            Log.Information($"BusinessServices::RetrieveSoftwarePackagesFromServerWorker exit cnt:{SoftwarePkgList?.Count}");
            getSoftwareFromServerCallback?.Invoke(SoftwarePkgList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void GetServerStatus()
        {
            if (GetServerStatusInProgress || agentInterface==null)
                return;

            var GuiIds = new List<string>() { };
            GetServerStatusInProgress = true;
            var statusResp = agentInterface.GetServerStatus(GuiIds);
            GetServerStatusInProgress = false;

            if (statusResp != null)
            {
                IsServerAvialable = bool.Parse(statusResp.Value.ServerAvailable);
            }            
        }

        public AuthenticationServerInfo GetAuthServerInfo()
        {
            return agentInterface.Session.CreateSessionResponse.AuthServerInfo;
        }

        /// <summary>
        /// FetchAndStoreCertificate
        /// </summary>
        /// <returns></returns>
        public void FetchAndStoreCertificate()
        {
            Log.Information($"BusinessServices::FetchAndStoreCertificate entry");
           
            // If clientCertificate is not present or valid or expired, then get certificate 
            if ((clientCertificate == null) ||
                (!certificateService.IsCertificateValid(clientCertificate)) ||
                (DateTime.Now.AddDays(Settings.OasisAgentClientCertificateRenewalInDays) > clientCertificate.NotAfter))
            {
                if (IsAuthenticationCompleted())
                {
                    // Ensure that we have a valid certificate
                    if (clientCertificate != null)
                    {
                        // The certificate is not valid
                        Log.Information($"BusinessServices::FetchAndStoreCertificate A client certificate exists, but is invalid or Expired (failed the IsCertificateValid call). {clientCertificate}");
                        RequestAndStoreAgentClientCertificate(agentInterface);
                    }
                    else
                    {
                        // Try to retrieve the certificate from the store
                        clientCertificate = certificateService.ReadCertificateFromStore(Settings.CertificateSubject);

                        if (clientCertificate == null)
                        {
                            // No certificate, request one from the agent
                            Log.Error($"BusinessServices::FetchAndStoreCertificate A client certificate does not exist, even in the certificate store.");

                            RequestAndStoreAgentClientCertificate(agentInterface);
                        }
                    }

                    if (clientCertificate != null)
                    {
                        // Updating Certificate information to AgentInterface class
                        agentInterface.SetClientCertificate(clientCertificate);
                    }
                }
            }
            // else.... clientCertificate is valid.. 

            Log.Information($"BusinessServices::FetchAndStoreCertificate exit");
        }

        private bool IsAuthenticationCompleted()
        {
            if (agentInterface != null && agentInterface.Session != null)
            {
                return agentInterface.Session.IsAuthenticationCompleted();
            }
            else
            {
                return false;
            }
         }

        /// <summary>
        /// changes working dir to default and cleansup the device download directory
        /// this is needed after completion of use of Downloader.dll as it changes the working directory
        /// </summary>
        private void CleanupDownloadDirs()
        {
            //resetting current working directory as downloader library changes it
            ResetWorkingDirectory();
            DeleteTempSwDownloadDirectory();
            DeleteTempPkgDirectory();
        }

        /// <summary>
        /// AuthorizeToServerUsingToken
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public AuthorizeResp AuthorizeToServerUsingToken(string accessToken, string refreshToken, string user)
        {
            agentInterface.AuthorizeServer(accessToken, refreshToken, user, null, oasisCredentialInformation.EssClientAppId,
                oasisCredentialInformation.EssAgentMachineId, oasisCredentialInformation.EssVersion, oasisCredentialInformation.HostName,
                oasisCredentialInformation.HostId, null);

            Log.Information($"BusinessServices::AuthorizeToServerUsingToken user:{user} Resp:{agentInterface.Session.AuthorizeResponse}");
            if (agentInterface.Session.AuthorizeResponse != null && agentInterface.Session.AuthorizeResponse.Result.Success)
            {
                RestrictedFunctionManager.SetUserLoggedIn(true);
            }

            return agentInterface.Session.AuthorizeResponse;
        }

        /// <summary>
        /// SetOfllinePasscodeToServer
        /// </summary>
        /// <param name="username"></param>
        /// <param name="passcode"></param>
        /// <returns></returns>
        public bool SetOfflinePasscodeToServer(string passcode)
        {
            var response = agentInterface.SetOfflinePasscodeServer( passcode);
            var resp = response.Success != false &&
                response.Value.Result.Success != false &&
                response.Value.Result.Expired != true;

            Log.Information($"BusinessServices::SetOfflinePasscodeToServer ret: {resp}");
            return resp;
        }

        /// <summary>
        /// CheckServerStatus
        /// </summary>
        /// <param name="getRsaStatusCallback"></param>
        public void CheckServerStatus(UserInterfaceDelegates.GetRsaStatusCallback getRsaStatusCallback)
        {             
            Task.Run(() => CheckServerStatusWorker(getRsaStatusCallback));
        }

        /// <summary>
        /// ClearDeviceLogsUploadStatus
        /// </summary>
        public void ClearDeviceLogsUploadStatus()
        {
            Log.Information($"BusinessServices::ClearDeviceLogsUploadStatus called");
            Instance.StoredLogs.Clear();
        }

        /// <summary>
        /// Updates the Device configuration information to OASIS Agent.
        /// </summary>
        /// <param name="devInfo"></param>
        public void SyncDeviceConfig(DeviceInformation devInfo)
        {
            var systemConfigs = new SystemConfigs(){ Configuration = new List<Configuration>() };

            var hwConfiguration = new Configuration { Type=HardwareString, ConfigurationComponent = new List<ConfigurationComponent>() };
            var swConfiguration = new Configuration { Type=SoftwareString, ConfigurationComponent = new List<ConfigurationComponent>() };

            foreach (var componentInfo in devInfo.Components)
            {
                var configComponent = new ConfigurationComponent
                {
                    ComponentType = componentInfo.Type,
                    ComponentName = componentInfo.Name,
                    PartNumber = componentInfo.PartNumber,
                    SoftwareRevision = componentInfo.Revision,
                    SerialNumber = componentInfo.SerialNumber
                };

                if (componentInfo.Type.ToUpper() == swConfiguration.Type)
                {
                    swConfiguration.ConfigurationComponent.Add(configComponent);
                }
                else if (componentInfo.Type.ToUpper() == hwConfiguration.Type)
                {
                    hwConfiguration.ConfigurationComponent.Add(configComponent);
                }
            }

            systemConfigs.Configuration.Add(hwConfiguration);
            systemConfigs.Configuration.Add(swConfiguration);
            agentInterface.SyncDeviceConfig(ModalConstants.PB980_VENTILATOR_GUID, devInfo.SerialNumber, systemConfigs, DeviceCountryCode);
        }

        /// <summary>
        /// CheckServerStatusWorker
        /// </summary>
        /// <param name="getRsaStatusCallback"></param>
        /// <returns></returns>
        private void CheckServerStatusWorker(UserInterfaceDelegates.GetRsaStatusCallback getRsaStatusCallback)
        {
            if (GetServerStatusInProgress == true)
                return;
           
            IEnumerable< string > deviceTypeGuids = new List<string>() { ModalConstants.PB980_VENTILATOR_GUID };
            RsaStatus status = new RsaStatus
            {
                ServerStatus = RsaStatus.RSA_SERVER_STATUS.UNKNOWN
            };
            if (agentInterface != null)
            {
                GetServerStatusInProgress = true;
                var statusResp = agentInterface.GetServerStatus(deviceTypeGuids);
                GetServerStatusInProgress = false;

                if (statusResp.Success)
                {
                    IsServerAvialable = bool.Parse(statusResp.Value.ServerAvailable);
                    Log.Information($"BusinessServices::GetServerStatus server:{IsServerAvialable} Response {statusResp.Value.UploadTimeRemaining}, {statusResp.Value.RemainingTime.Value}");
                    status.ServerStatus = Instance.IsServerAvialable? RsaStatus.RSA_SERVER_STATUS.CONNECTED : RsaStatus.RSA_SERVER_STATUS.DISCONNECTED;
                    status.AgentStatus = RsaStatus.RSA_SERVER_STATUS.CONNECTED;
                    status.DownloadTime = long.Parse(statusResp.Value.RemainingTime.Value);
                    status.UploadTime = long.Parse(statusResp.Value.UploadTimeRemaining);
                    status.DataReady = bool.Parse(statusResp.Value.DataReady);
                    status.InsufficientSpace = (statusResp.Value.InsufficientSpace != null &&
                                                bool.Parse(statusResp.Value.InsufficientSpace));
                }
                else
                {
                    Log.Error($"BusinessServices::GetServerStatus Response Failed {statusResp.Message} ");
                    status.ServerStatus = RsaStatus.RSA_SERVER_STATUS.DISCONNECTED;
                }
            }
            getRsaStatusCallback(status);
        }

        /// <summary>
        /// Request certificate from Server and stores in Windows CertificateService
        /// </summary>
        /// <param name="agentInterface"></param>
        /// <returns></returns>
        /// <exception cref="OasisAgentClientCertificateException"></exception>
        private void RequestAndStoreAgentClientCertificate(IAgentInterface agentInterface)
        {
            Log.Information($"BusinessServices::RequestAndStoreAgentClientCertificate req new client certificate from the Agent");

            var certificateRequestResult = agentInterface.GetClientCertificate(Settings.EssCertificateCommonName,
                                    VersionHelper.ExecutingAssemblyVersion,
                                    IdentificationHelper.GetHostName(),
                                    IdentificationHelper.GetHostId());

            if (certificateRequestResult.Success)
            {
                Log.Information($"BusinessServices::RequestAndStoreAgentClientCertificate ret: {certificateRequestResult.Success}");
                // Assign our in-memory version of the certificate
                try
                {
                    clientCertificate = certificateService.CreateCertificate(certificateRequestResult.Value.CryptoCertificate,
                        certificateRequestResult.Value.CryptoCertificateKey,
                        Settings.PrivateKeyContainerName);
                }
                catch (Exception ex)
                {
                    Log.Error($"BusinessServices::RequestAndStoreAgentClientCertificate Failed to Create Certificate {ex.Message}. Login as Admin ");
                    MessageBox.Show($"Error:{ex.Message}\n{RELAUNCH_APP}");
                }
                if (clientCertificate != null)
                {
                    var valid = certificateService.IsCertificateValid(clientCertificate);
                    Log.Information($"BusinessServices::RequestAndStoreAgentClientCertificate client certificate Validation: {valid}");

                    if (valid)
                    {
                        try
                        {
                            // Store the certificate
                            certificateService.StoreCertificate(clientCertificate);
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"BusinessServices::RequestAndStoreAgentClientCertificate Failed to Store Certificate {ex.Message}");
                            MessageBox.Show(RSA_CERT_INVALID+"\n"+RELAUNCH_APP);
                            Application.Exit();
                        }
                    }
                }
                else
                {
                    Log.Error($"BusinessServices::RequestAndStoreAgentClientCertificate Either Client Certificate is null or not valid");
                }
                
            }
            else
            {
                Log.Error($"BusinessServices::RequestAndStoreAgentClientCertificate The request for a new client certificate from the OASIS Agent failed, with message: {certificateRequestResult.Message}");
            }
        }

        /// <summary>
        /// ServerCertificateCustomValidationCallback
        /// </summary>
        /// <param name="httpRequestMessage"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private bool ServerCertificateCustomValidationCallback(HttpRequestMessage httpRequestMessage, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return certificate.Issuer == Settings.CertificateSender;
        }
    }
}
