namespace Covidien.CGRS.PcAgentInterfaceBusiness
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Xml;
    using System.Xml.Xsl;
    using Covidien.CGRS.BinaryLogDecoder;
    using IPI_Core;
    using System.Linq;
    using Oasis.Agent.Models.Responses;
    using Serilog;

    public class DeviceUserInterfaceServices : UserInterfaceServices
    {
        private const string URI_GET_DEVICE_INFO_FMT = "http://{0}:{1}/GetDeviceInformationAsXml";
        private const string URI_GET_DEVICE_LOGS_FMT = "http://{0}:{1}/GetDeviceLogListAsXml";
        private const string URI_SET_DEVICE_KEY_FMT = "http://{0}:{1}/SetDataKey?Key={2}";
        private const string URI_CLEAR_DEVICE_LOGS_FMT = "http://{0}:{1}/ClearLogs";

        private const string URI_GET_SETTING_VALUE_FORMAT = "http://{0}:{1}/{2}";
        private const string URI_SET_SETTING_VALUE_FORMAT = "http://{0}:{1}/{2}?{3}={4}";
        private const string URI_SET_SETTING_VALUE_NO_PARAMS_FORMAT = "http://{0}:{1}/{2}";

        public const string SET_VENT_OPERATIONAL_HOURS = "SetVentOperationalHours";
        public const string SET_COMPRESSOR_OPERATIONAL_HOURS = "SetCompressorOperationalHours";
        public const string SET_VENT_PREVENTATIVE_MAINTENANCE_HOURS = "SetVentPreventativeMaintHours";
        public const string SET_COMPRESSOR_PREVENTATIVE_MAINTENANCE_HOURS = "SetCompressorPreventativeMaintHours";
        public const string SET_CALS_PREVENTATIVE_MAINTENANCE_HOURS = "SetCalsPreventativeMaintenanceHours";
        public const string SET_EST_PREVENTATIVE_MAINTENANCE_HOURS = "SetEstPreventativeMaintenanceHours";
        public const string SET_DEVICE_HOURS_PARAMETER_STRING = "Hours";

        public const string GET_DEVICE_HOURS_AS_XML = "GetDeviceHoursAsXml";

        public const string SET_COMPRESSOR_SERIAL_NUMBER = "SetCompressorSerialNumber";
        public const string SET_COMPRESSOR_SERIAL_NUMBER_PARAMETER_STRING = "CompressorSN";
        public const string GET_COMPRESSOR_SERIAL_NUMBER = "GetCompressorSerialNumber";
        public const string GET_COMPRESSOR_SERIAL_NUMBER_PARAMETER_STRING = "compressor_sn";

        public const string SET_OPTIONS_KEY_COMMAND_STRING = "SetDataKey";
        public const string OPTIONS_KEY_VALUE_XML_PARAMETER_STRING = "Key";

        public const string GET_OPTIONS_KEY_COMMAND_STRING = "GetDataKeyAsXml";

        public const string OVERRIDE_EST_HTTP_COMMAND_STRING = "OverrideEst";
        public const string GET_SELF_TEST_STATUS_HTTP_COMMAND_STRING = "GetSelfTestStatusAsXml";
        public const string GET_EST_STATUS_PARAMETER_STRING = "est_completion_status";
        public const string GET_EST_DATE_PARAMETER_STRING = "est_completion_timestamp";

        public const string GET_VENT_OPERATIONAL_HOURS_PARAMETER_STRING = "vent_operational_hours";
        public const string GET_COMPRESSOR_OPERATIONAL_HOURS_PARAMETER_STRING = "compressor_operational_hours";
        public const string GET_VENT_PM_HOURS_PARAMETER_STRING = "vent_preventative_maintenance_hours";
        public const string GET_COMPRESSOR_PM_HOURS_PARAMETER_STRING = "compressor_preventative_maintenance_hours";

        private string LogFileTimeStamp = DateTime.Now.ToLongTimeString().Replace(':', '_').Replace('/', '_');

        public const string EXTEND_ESM_COMMAND_STRING = "ExtendESM";

        public bool IsDummyDeviceConnected = false;
        private const string RENDER_NAME = "Simple";
        private static readonly object MSyncRoot = new object();
        private readonly Hashtable LogContentTypeMapping ;

        public string LogDefinitionFilename { get; set; }
        public string LogInflationJsFilename { get; set; }

        private readonly string CommsLogFilePath;

        /// <summary>
        ///  Holds access info for the available logs -- available on the device
        /// </summary>
        public Hashtable AvailableLogs;

        /// <summary>
        /// Holds access info to the retrieved logs -- already downloaded from device
        /// </summary>
        public Hashtable RetrievedLogs; // public for mock dev only

        public DeviceInformation DeviceInfo;

        /// <summary>
        /// These calls (container actually) must be set before taking any other action
        /// Because of a mutual dependency (at least for now), it is not part of the ctor().
        /// </summary>
        public ServerInterfaceServices ServerIFace { get; set; }

        public DeviceUserInterfaceServices(string CommsLogPath )
        {
            ConnectCallback = null;
            DisconnectCallback = null;
            InternalMessageCallback = null;

            CommsLogFilePath = CommsLogPath.Replace( "\\\\", "/" ) ;
            LogContentTypeMapping = new Hashtable
            {
                { "application/pbVentLog-diag", "diag/*.dat" },
                { "application/pbVentLog-ui", "ui/*.dat" },

                // Recent change to vent makes us have to use the name for the content type
                { "application/pbVentLog-Alarm", "Alarm" },
                { "application/pbVentLog-GenEvent", "GenEvent" },
                { "application/pbVentLog-PatientData", "PatientData" },
                { "application/pbVentLog-Settings", "Settings" },
                { "application/pbVentLog-Service", "Service" },
                { "application/pbVentLog-SysDiag", "diag/*.dat" },
                { "application/pbVentLog-SysComm", "diag/*.dat" },
                { "application/pbVentLog-EstSst", "diag/*.dat" }
            };

            ResetDevice();
        }

        /// <summary>
        /// ResetDevice
        /// </summary>
        public void ResetDevice()
        {
            AvailableLogs = new Hashtable() ;
            RetrievedLogs = new Hashtable();
            DeviceInfo = new DeviceInformation();
            LogFileTimeStamp = DateTime.Now.ToLongTimeString().Replace(':', '_').Replace('/', '_');
        }

        /// <summary>
        /// GetDeviceInformationSync
        /// </summary>
        /// <returns></returns>
        public DeviceInformation GetDeviceInformationSync()
        {
            Log.Information($"DeviceUIServices::GetDeviceInformationSync entry");

            DeviceInformation retrievedDeviceInfo;
            try
            {
                string Dev_Info_URL = string.Format(URI_GET_DEVICE_INFO_FMT, ServerIFace.ServerIpAddress, ServerIFace.PortNumber);
                string devInfo =DeviceWebRequest(Dev_Info_URL);
                retrievedDeviceInfo = DeviceInformation.LoadDeviceInfo(devInfo);
            }

            catch (Exception e)
            {
                Log.Error($"DeviceUIServices::GetDeviceInformationSync Exception:{e.Message}");
                retrievedDeviceInfo = null;
            }

            Log.Information($"DeviceUIServices::GetDeviceInformationSync exit");
            return retrievedDeviceInfo;
        }

        /// <summary>
        /// GetDeviceInfo
        /// </summary>
        /// <param name="getDeviceInfoCallback"></param>
        /// <returns></returns>
        public string GetDeviceInfo( UserInterfaceDelegates.GetDeviceInfoCallback getDeviceInfoCallback )
        {
            string devInfo = "";
            Log.Information($"DeviceUIServices::GetDeviceInfo entry");
            string transactionGuid = Guid.NewGuid().ToString();

            if ((null == DeviceInfo) || (null == DeviceInfo.PertinentType) || (null == DeviceInfo.SerialNumber))
            {
                try
                {
                    string Dev_Info_URL = string.Format(URI_GET_DEVICE_INFO_FMT, ServerIFace.ServerIpAddress, ServerIFace.PortNumber);
                    devInfo = DeviceWebRequest(Dev_Info_URL);
                    DeviceInfo = DeviceInformation.LoadDeviceInfo(devInfo);
                    getDeviceInfoCallback?.Invoke(transactionGuid, true, DeviceInfo);
                }
                catch (Exception e)
                {
                    Log.Error($"DeviceUIServices::GetDeviceInfo Exception:{e.Message}");
                    DeviceInfo = null;  // ensure it is cleared out from any previous use.  UI should treat null value as failed connection.
                    getDeviceInfoCallback?.Invoke(transactionGuid, false, null);
                }
            }
            else
            {
                getDeviceInfoCallback?.Invoke(transactionGuid, true, DeviceInfo);
            }
            Log.Information($"DeviceUIServices::GetDeviceInfo exit");
            return ( transactionGuid ) ;
        }

        /// <summary>
        /// UpdateDeviceRights
        /// </summary>
        /// <param name="restrictedFunctionManager"></param>
        public void UpdateDeviceRights( RestrictedFunctionManager restrictedFunctionManager )
        {
            UpdateDeviceRights( restrictedFunctionManager, DeviceInfo ) ;
        }

        /// <summary>
        /// UpdateDeviceRights
        /// </summary>
        /// <param name="restrictedFunctionManager"></param>
        /// <param name="deviceInfo"></param>
        // This is VENT centric.  Must adjust when we get to other devices
        public void UpdateDeviceRights( RestrictedFunctionManager restrictedFunctionManager, DeviceInformation deviceInfo )
        {
            Log.Information($"DeviceUIServices::UpdateDeviceRights entry");

            // Update the access rights - as they have come from the server
            restrictedFunctionManager.InitDevice( deviceInfo.SerialNumber ) ;

            // Start by initializing all device rights to false, just in case the device does not give us good info
            for (RestrictedFunctionManager.RestrictedFunctions tmp = RestrictedFunctionManager.RestrictedFunctions.FIRST;
                    tmp < RestrictedFunctionManager.RestrictedFunctions.NUM_RESTRICTED_FUNCTIONS; tmp++)
            {
                restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.DEVICE, tmp, false);
            }

            //logview only in service_key mode
            restrictedFunctionManager.UpdateRight( deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.DEVICE, RestrictedFunctionManager.RestrictedFunctions.LOG_VIEW, false ) ;
            //this is restricted to covidien users only - based on user
            restrictedFunctionManager.UpdateRight( deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.DEVICE, RestrictedFunctionManager.RestrictedFunctions.BROWSE_TO_SELECT_DOWNLOAD_PACKAGE, false ) ;

            //unrestricted functions

            //log upload is unrestricted
            restrictedFunctionManager.UpdateRight( deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.DEVICE, RestrictedFunctionManager.RestrictedFunctions.LOG_UPLOAD, true ) ;
            //device does not restrict software download
            restrictedFunctionManager.UpdateRight( deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.DEVICE, RestrictedFunctionManager.RestrictedFunctions.SOFTWARE_DOWNLOAD, true ) ;

            //device does not restrict PM Hours
            restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.DEVICE, RestrictedFunctionManager.RestrictedFunctions.SET_PREVENTIVE_MAINTENANCE_DUE_HOURS, true);
            //device does not restrict setting options key
            restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.DEVICE, RestrictedFunctionManager.RestrictedFunctions.SET_OPTIONS_KEY, true);

            //restricted functions
            restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.DEVICE, RestrictedFunctionManager.RestrictedFunctions.SET_OPERATIONAL_HOURS, false);
            restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.DEVICE, RestrictedFunctionManager.RestrictedFunctions.SET_COMPRESSOR_SERIAL_NUMBER, false);
            restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.DEVICE, RestrictedFunctionManager.RestrictedFunctions.OVERRIDE_EST_RESULT, false);
            restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.DEVICE, RestrictedFunctionManager.RestrictedFunctions.EXTEND_ENHANCED_SERVICE_MODE, false);
            restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.DEVICE, RestrictedFunctionManager.RestrictedFunctions.VIEW_DEVICE_KEY_TYPE, false);
            restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.DEVICE, RestrictedFunctionManager.RestrictedFunctions.SET_EST_STATUS, false);
            restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.DEVICE, RestrictedFunctionManager.RestrictedFunctions.SET_EST_PERFORMED_DATE, false);
            

            foreach( DeviceComponentInfo compInfo in deviceInfo.Components )
            {
                if  ( compInfo.Name.Equals( "MasterVent" ) )
                {
                    if (compInfo.OtherAttributes != null)
                    {
                        foreach (KeyValuePair<string, string> kvp in compInfo.OtherAttributes)
                        {
                            if (kvp.Key.Equals("options", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] tags = kvp.Value.Split(';');
                                foreach (string tag in tags)
                                {
                                    if (tag.Equals("ESM", StringComparison.OrdinalIgnoreCase))
                                    {
                                        restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.DEVICE, RestrictedFunctionManager.RestrictedFunctions.LOG_VIEW, true);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    break;
                }
            }
            Log.Information($"DeviceUIServices::UpdateDeviceRights exit");
        }

        /// <summary>
        /// UpdateUserRights
        /// </summary>
        /// <param name="restrictedFunctionManager"></param>
        /// <param name="deviceInfo"></param>
        /// <param name="userLogin"></param>
        public void UpdateUserRights( RestrictedFunctionManager restrictedFunctionManager, DeviceInformation deviceInfo, AuthorizeResp userLogin )
        {
            Log.Information($"DeviceUIServices::UpdateUserRights entry");

            // Update the access rights - as they have come from the server
            restrictedFunctionManager.InitDevice( deviceInfo.SerialNumber ) ;

            // Start by initializing all device rights to false, just in case the device does not give us good info
            for (RestrictedFunctionManager.RestrictedFunctions tmp = RestrictedFunctionManager.RestrictedFunctions.FIRST;
                    tmp < RestrictedFunctionManager.RestrictedFunctions.NUM_RESTRICTED_FUNCTIONS; tmp++)
            {
                restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.USER, tmp, false);
            }

            //log upload is unrestricted
            restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.USER, RestrictedFunctionManager.RestrictedFunctions.LOG_UPLOAD, true);

            if ((userLogin != null) && (userLogin.CovidienUser == true))
            {
                //covidien user has unrestricted access
                restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.USER, RestrictedFunctionManager.RestrictedFunctions.LOG_VIEW, true);
                //this is restricted to covidien users only - only covidien user
                restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.USER, RestrictedFunctionManager.RestrictedFunctions.BROWSE_TO_SELECT_DOWNLOAD_PACKAGE, true);

                restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.USER, RestrictedFunctionManager.RestrictedFunctions.SET_OPERATIONAL_HOURS, true);
                restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.USER, RestrictedFunctionManager.RestrictedFunctions.SET_COMPRESSOR_SERIAL_NUMBER, true);
                restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.USER, RestrictedFunctionManager.RestrictedFunctions.OVERRIDE_EST_RESULT, true);
                restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.USER, RestrictedFunctionManager.RestrictedFunctions.EXTEND_ENHANCED_SERVICE_MODE, true);
                restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.USER, RestrictedFunctionManager.RestrictedFunctions.VIEW_DEVICE_KEY_TYPE, true);
            
            }

            //device does not restrict software download
            restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.USER, RestrictedFunctionManager.RestrictedFunctions.SOFTWARE_DOWNLOAD, false);

            if (deviceInfo.Components != null)
            {
                foreach (DeviceComponentInfo compInfo in deviceInfo.Components)
                {
                    if (compInfo.Name.Equals("software", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (string right in compInfo.AccessRights)
                        {
                            if (right.Equals("install", StringComparison.OrdinalIgnoreCase))   // "install" is the equivalent (for us, for now) to Service_Key
                            {
                                restrictedFunctionManager.UpdateRight(deviceInfo.SerialNumber, RestrictedFunctionManager.RestrictionSource.USER, RestrictedFunctionManager.RestrictedFunctions.SOFTWARE_DOWNLOAD, true);
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            Log.Information($"DeviceUIServices::UpdateUserRights exit");
        }

        /// <summary>
        /// GetSettingValueFromDeviceSync
        /// </summary>
        /// <param name="commandString"></param>
        /// <param name="parameterValueString"></param>
        /// <returns></returns>
        public string GetSettingValueFromDeviceSync(string commandString, string parameterValueString)
        {
            //sample output
            //<?xml version='1.0' standalone='yes' ?><hours>50</hours>
            Log.Information($"DeviceUIServices::GetSettingValueFromDeviceSync entry cmd: {commandString}");

            string httpCommandString = string.Format(URI_GET_SETTING_VALUE_FORMAT, ServerIFace.ServerIpAddress, ServerIFace.PortNumber, commandString);
            string result = DeviceWebRequest(httpCommandString);
            result= ParseResult(result, parameterValueString);

            Log.Information($"DeviceUIServices::GetSettingValueFromDeviceSync exit result:{result}");
            return result;
        }

        /// <summary>
        /// SetSettingValueOnDeviceSync
        /// </summary>
        /// <param name="commandString"></param>
        /// <param name="parameterValueString"></param>
        /// <param name="parameterValue"></param>
        /// <returns></returns>
        public string SetSettingValueOnDeviceSync(string commandString, string parameterValueString, string parameterValue)
        {
            Log.Information($"DeviceUIServices::SetSettingValueFromDeviceSync entry cmd: {commandString}");

            string httpCommandString;
            if (string.IsNullOrEmpty(parameterValueString) || string.IsNullOrEmpty(parameterValue))
            {
                httpCommandString = string.Format(URI_SET_SETTING_VALUE_NO_PARAMS_FORMAT, ServerIFace.ServerIpAddress, ServerIFace.PortNumber, commandString);                    
            }
            else
            {
                httpCommandString = string.Format(URI_SET_SETTING_VALUE_FORMAT, ServerIFace.ServerIpAddress, ServerIFace.PortNumber, commandString, parameterValueString, parameterValue);
            }

            Log.Information($"DeviceUIServices::SetSettingValueFromDeviceSync {ServerInterfaceServices.ORIGINATOR_SEND}:{httpCommandString}");

            string result = DeviceWebRequest(httpCommandString);
            result =ParseResult(result, "result");
            Log.Information($"DeviceUIServices::SetSettingValueFromDeviceSync exit result: {result}");
            return result;
        }

        /// <summary>
        /// SetDeviceDataKey
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string SetDeviceDataKey( string key )
        {
            Log.Information($"DeviceUIServices::SetDeviceDataKey entry key: {key}");

            string Dev_Info_URL = string.Format( URI_SET_DEVICE_KEY_FMT, ServerIFace.ServerIpAddress, ServerIFace.PortNumber, key );
            string result = DeviceWebRequest(Dev_Info_URL);
            result =ParseResult(result, "result");
            Log.Information($"DeviceUIServices::SetDeviceDataKey exit result: {result}");
            return result;
        }

        /// <summary>
        /// ClearDeviceLogs
        /// </summary>
        /// <returns></returns>
        public string ClearDeviceLogs()
        {
            return( ClearDeviceLogs( (UserInterfaceDelegates.ClearDeviceLogsCallback) null ) ) ;
        }

        /// <summary>
        /// ClearDeviceLogs
        /// </summary>
        /// <param name="clearDeviceLogsCallback"></param>
        /// <returns></returns>
        public string ClearDeviceLogs(UserInterfaceDelegates.ClearDeviceLogsCallback clearDeviceLogsCallback)
        {
            Log.Information($"DeviceUIServices::ClearDeviceLogs entry");
            bool callSuccess = true;
            string Dev_Info_URL = string.Format(URI_CLEAR_DEVICE_LOGS_FMT, ServerIFace.ServerIpAddress, ServerIFace.PortNumber);
            string result =DeviceWebRequest(Dev_Info_URL);
            result = ParseResult(result, "result");
            if (string.Equals(result, "success", StringComparison.OrdinalIgnoreCase) == false)
            {
                callSuccess = false;
            }
            string transactionGuid = Guid.NewGuid().ToString();
            clearDeviceLogsCallback?.Invoke(transactionGuid, callSuccess, result);
            Log.Information($"DeviceUIServices::ClearDeviceLogs exit Result:{result}");
            return (transactionGuid);
        }

        /// <summary>
        /// ConvertDeviceInfoToLog
        /// </summary>
        public void ConvertDeviceInfoToLog()
        {
            // This is a little bit of a mess, because it, currently, is very different from all of the other logs (which are due to change shortly).
            Log.Information($"DeviceUIServices::ConvertDeviceInfoToLog entry");
            string logName = "DeviceInfo";
            AvailableLogs.Add( logName, new LogAvailability( logName, null, null ) ) ;

            LogCargoContainer logInfoContainer = new LogCargoContainer
            {
                DevType = ModalConstants.Instance.GetDebugKey(DeviceInfo.PertinentType.DeviceClassification),
                DevId = DeviceInfo.SerialNumber,

                // now need to know this "outside" the data actually being retrieved from the device
                // the contextual information has been stripped, we no longer get xml with head & body, just body content
                Name = logName,
                DecodeType = "application/pbVentLog-" + logName,
                DecodeVersion = "1.0"
            };

            // Design the file name/timestamp as of having grabbed the file
            // TODO: to make the destination based on config file.
            string format = "{5}/{0}-{1}-{2}-{3}-{4}.log.xml.gz" ;
            logInfoContainer.LogRawFilename = string.Format( format, DeviceInfo.PertinentType.DeviceClassification,
                DeviceInfo.SerialNumber, logInfoContainer.Name, "raw", LogFileTimeStamp, CommsLogFilePath);


            format = "{5}/{0}-{1}-{2}-{3}-{4}.log.html.gz" ;
            logInfoContainer.LogDecodedFilename = string.Format( format, DeviceInfo.PertinentType.DeviceClassification,
                DeviceInfo.SerialNumber, logInfoContainer.Name, "decoded", LogFileTimeStamp, CommsLogFilePath);

            format = "{5}/{0}-{1}-{2}-{3}-{4}.log.xml";
            logInfoContainer.LogLocalStoreFileName = string.Format(format, DeviceInfo.PertinentType.DeviceClassification,
                DeviceInfo.SerialNumber, logInfoContainer.Name, "orig", LogFileTimeStamp, CommsLogFilePath);

            logInfoContainer.LogXmlStr = DeviceInfo.EncodeAsXml();
// TODO: make the xslt reference relative to some directory
//            logInfoContainer.logDecodedBody = string.Format( @"<?xml version=""1.0"" encoding=""UTF-8""?><?xml-stylesheet type=""text/xsl"" href=""XmlToTableCC.xslt"" ?>{0}", DeviceInfo.EncodeAsXml() ) ;
            logInfoContainer.IsDecoded = true;
            string mypath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            logInfoContainer.LogXsltTransform = Path.Combine(mypath, "DeviceInfoXmlToTable.xslt");

            bool success = SaveOriginalFile(logInfoContainer);
            if (success == true)
            {
                success = SaveRawGzipFile(logInfoContainer);
            }
            if (success == true)
            {
                success = SaveHtmlGzipFile(logInfoContainer);
            }
            if (success == true)
            {
                RetrievedLogs.Add(logName, logInfoContainer);
            }
            Log.Information($"DeviceUIServices::ConvertDeviceInfoToLog exit Result:{success}");
            return;
        }

        /// <summary>
        /// saves xmlstring into a original file on local file system
        /// </summary>
        /// <param name="xmlString"></param>
        /// <param name="logInfoContainer"></param>
        /// <returns></returns>
        public bool SaveOriginalFile(LogCargoContainer logInfoContainer)
        {
            bool success = true;
            try
            {
                LogFileUtility.SaveStringToFile(logInfoContainer.LogXmlStr, logInfoContainer.LogLocalStoreFileName);
                Log.Information($"DeviceUIServices::SaveOriginalFile saved log:{logInfoContainer.LogLocalStoreFileName}");
                logInfoContainer.IsOrigSaved = true;
            }
            catch (Exception e)
            {
                success = false;
                Log.Error($"DeviceUIServices::SaveOriginalFile log:{logInfoContainer.LogLocalStoreFileName} Exception:{e.Message}");
            }
            return success;
        }

        /// <summary>
        /// saves original file into gzipped xml file
        /// </summary>
        /// <param name="logInfoContainer"></param>
        /// <returns></returns>
        public bool SaveRawGzipFile(LogCargoContainer logInfoContainer)
        {
            bool success = true;
            //.raw.xml.gz
            try
            {
                LogFileUtility.GzipAFile(logInfoContainer.LogLocalStoreFileName, logInfoContainer.LogRawFilename);
                logInfoContainer.IsRawSaved = true;
                Log.Information($"DeviceUIServices::SaveRawGzipFile saved log:{logInfoContainer.LogRawFilename}");
            }
            catch (Exception e)
            {
                success = false;
                Log.Error($"DeviceUIServices::SaveRawGzipFile log:{logInfoContainer.LogRawFilename} Exception:{e.Message}");
            }
            return success;
        }

        /// <summary>
        /// SaveHtmlGzipFile
        /// </summary>
        /// <param name="logInfoContainer"></param>
        /// <returns></returns>
        public bool SaveHtmlGzipFile(LogCargoContainer logInfoContainer)
        {
            bool success = true;
            try
            {
                LogFileUtility.XSLTransformIntoHTMLGzipFile(logInfoContainer.LogXsltTransform, logInfoContainer.LogLocalStoreFileName, logInfoContainer.LogDecodedFilename);
                logInfoContainer.IsDecodedSaved = true;
                Log.Information($"DeviceUIServices::SaveHtmlGzipFile saved log:{logInfoContainer.LogDecodedFilename}");
            }
            catch (Exception e)
            {
                success = false;
                Log.Error($"DeviceUIServices::SaveHtmlGzipFile log:{logInfoContainer.LogDecodedFilename} Exception:{e.Message}");
            }
            return success;
        }

        public UserInterfaceDelegates.DeviceLogLoadedCallback DeviceLogLoadedCallback { get; set; }

        /// <summary>
        /// GetDeviceLogsList
        /// </summary>
        /// <param name="getDeviceLogsListCallback"></param>
        /// <returns></returns>
        public string GetDeviceLogsList( UserInterfaceDelegates.GetDeviceLogsListCallback getDeviceLogsListCallback )
        {
            bool success = true;
            string logsInfo = "";
            List<string> logNames = new List<string>() ;

            foreach (string logName in AvailableLogs.Keys)
            {
                logNames.Add(logName);
            }

            // Get a locally cached list if available.  Use must disconnect/re-connect to get a fresh list from the same device
            // [0] location will have been set with the DeviceInfo (masquerading as a log) upon connection
            if  ( 1 >= AvailableLogs.Count )
            {
                try
                {
                    string Dev_Log_URL = string.Format( URI_GET_DEVICE_LOGS_FMT, ServerIFace.ServerIpAddress, ServerIFace.PortNumber ) ;
                    logsInfo = DeviceWebRequest(Dev_Log_URL) ;

                    // Process the log list.
                    XmlDocument xmlDoc = new XmlDocument();
                    try
                    {
                        xmlDoc.LoadXml( logsInfo ) ;
                    }
                    catch (Exception e)
                    {
                        success = false;
                        Log.Error($"DeviceUIServices::GetDeviceLogsList Loadxml Exception:{e.Message}");
                    }

                    XmlNodeList logs = xmlDoc.SelectNodes("//logs/log");
                    foreach( XmlElement logInfo in logs )
                    {
                        string logName = logInfo.GetAttribute( "name" ) ;
                        string proto = logInfo.GetAttribute( "proto" ) ;
                        string logUri = logInfo.GetAttribute("uri");
                        if (!string.IsNullOrEmpty(logName) && !string.IsNullOrEmpty(logUri) && (!AvailableLogs.ContainsKey(logName)))
                        {
                            AvailableLogs.Add( logName, new LogAvailability( logName, proto, logUri ) ) ;
                            logNames.Add( logName ) ;
                        }
                    }
                    // so that list always shows up in a consistent sort order.
                    logNames.Sort();    
                }
                catch( Exception e)
                {
                    AvailableLogs.Clear();
                    success = false;
                    Log.Error($"DeviceUIServices::GetDeviceLogsList Exception:{e.Message}");
                }
            }

            string transactionGuid = Guid.NewGuid().ToString() ;
            getDeviceLogsListCallback?.Invoke(transactionGuid, success, logNames);
            return transactionGuid;
        }

        /// <summary>
        /// FilterXml
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private string FilterXml(string xml)
        {
            var validXMlChars = xml.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray();
            return new string(validXMlChars);
        }

        /// <summary>
        /// ConvertBase64LogFile
        /// </summary>
        /// <param name="logInfoContainer"></param>
        public void ConvertBase64LogFile( LogCargoContainer logInfoContainer )
        {
            // StripChars() is a custom extension in IPI_Core.StringExtensions
            logInfoContainer.LogRawBody = logInfoContainer.LogRawBody.StripChars( new HashSet<char>( new[] { ' ', '\t', '\n', '\r' } ) ) ;
            logInfoContainer.LogRawBytes = Convert.FromBase64String( logInfoContainer.LogRawBody ) ;
        }

        /// <summary>
        /// ConvertXmlLogFile
        /// </summary>
        /// <param name="logInfoContainer"></param>
        public void ConvertXmlLogFile( LogCargoContainer logInfoContainer )
        {
            Log.Information($"DeviceUIServices::ConvertXmlLogFile entry");
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml( logInfoContainer.LogXmlStr ) ;
            }
            catch (Exception e)
            {
                Log.Error($"DeviceUIServices::ConvertXmlLogFile Exception:{e.Message}");
                throw;
            }

// FYI - this for loop is a bad idea, not really designed for more than one item at a time (i.e., there is only a single logInfoContainer)
            XmlNodeList logFileDefns = xmlDoc.SelectNodes("//LogFiles/LogFile");
            foreach( XmlElement logFileDefinition in logFileDefns )
            {
                string logName = logFileDefinition.GetAttribute("name");
                if (!string.IsNullOrEmpty(logName))
                {
                    // OK, partial success, we get to work on the rest of it
                    logInfoContainer.Name = logName ;
                }
                else
                {
                    continue;  // if the message is not named, then ignore it
                }

                string xPathPrefix;

                string transferEncoding = null;
                string contentEncoding = null;
                string contentType = null;

                // OK, this won't be perfect or clean - because only a tag-value, not the meta tag itself
                logInfoContainer.LogRawMetaTags = new List<KeyValuePair<string, string>>();

                xPathPrefix = string.Format("//LogFiles/LogFile[@name='{0}']/head/meta", logName);
                XmlNodeList headerData = xmlDoc.SelectNodes(xPathPrefix);
                foreach (XmlElement metaDatum in headerData)
                {
                    string metaId;

                    metaId = metaDatum.GetAttribute("http-equiv");
                    if (!string.IsNullOrEmpty(metaId))
                    {
                        if (metaId.Equals("transfer-encoding", StringComparison.OrdinalIgnoreCase))
                        {
                            transferEncoding = metaDatum.GetAttribute("content");
                        }
                        else if (metaId.Equals("content-encoding", StringComparison.OrdinalIgnoreCase))
                        {
                            contentEncoding = metaDatum.GetAttribute("content");
                        }
                        else if (metaId.Equals("content-type", StringComparison.OrdinalIgnoreCase))
                        {
                            contentType = metaDatum.GetAttribute("content");
                        }

                        logInfoContainer.LogRawMetaTags.Add( new KeyValuePair<string, string>( metaId, metaDatum.GetAttribute( "content" ) ) ) ;
                    }

                    // not currently worried about content-encoding or other meta data fields
                }


                xPathPrefix = string.Format("//LogFiles/LogFile[@name='{0}']/body", logName);
                XmlNodeList bodyData = xmlDoc.SelectNodes(xPathPrefix);
                XmlElement bodyDatum = null;
                if  ( 0 < bodyData.Count )
                {
                    bodyDatum = (XmlElement) bodyData[ 0 ] ;
                }
                if  ( null != bodyDatum )
                {
                    //string logData = bodyDatum.InnerText;
                    logInfoContainer.LogRawBody = bodyDatum.InnerText;
                    // byte[] rawLogData = null;
                    // will use logRawBytes for the above

                    if  ( !string.IsNullOrEmpty( transferEncoding ) )
                    {
                        if  ( transferEncoding.Equals( "base64", StringComparison.OrdinalIgnoreCase ) )
                        {
                            ConvertBase64LogFile( logInfoContainer ) ;
                        }
                        // else need to figure out what to do
                    }
                    // else it is already a readable string, so leave it as is


                    MemoryStream memStreamFinal;
                    if  ( !string.IsNullOrEmpty( contentEncoding ) )
                    {
                        if  ( contentEncoding.Equals( "gzip", StringComparison.OrdinalIgnoreCase ) )
                        {
                            MemoryStream memStreamA = new MemoryStream( logInfoContainer.LogRawBody.Length ) ;
                            GZipStream gz = new GZipStream( memStreamA, CompressionMode.Decompress ) ;
                            memStreamFinal = new MemoryStream( logInfoContainer.LogRawBody.Length ) ;
                            byte[] buffer = new byte[8192];
                            int bytesRead = buffer.Length;
                            while( bytesRead == buffer.Length )
                            {
                                bytesRead = gz.Read(buffer, 0, buffer.Length);
                                if (bytesRead > 0)
                                    memStreamFinal.Write(buffer, 0, bytesRead);
                            }
                            // RTB temp hoping to just use memory streams after this point
                            // rawLogData = memStreamFinal.ToArray();
                        }
                        // else need to figure out what to do
                        else
                        {
                            memStreamFinal = new MemoryStream( logInfoContainer.LogRawBytes ) ;
                        }
                    }
                    // else it is already a readable string, so leave it as is
                    else
                    {
                        memStreamFinal = new MemoryStream( logInfoContainer.LogRawBytes ) ;
                    }

                    // Figure out what decoder and version to use
                    if  ( !string.IsNullOrEmpty( contentType ) )
                    {
                        string[] data = contentType.Split( ';' ) ;
                        if  ( 0 < data.Length )
                        {
                            logInfoContainer.DecodeType = data[0];
                            if  ( 1 < data.Length )
                            {
                                logInfoContainer.DecodeVersion = data[1];  // not currently doing anything with this yet
                            }
                        }
                    }
                }
            }
            Log.Information($"DeviceUIServices::ConvertXmlLogFile exit");
        }


        /// <summary>
        /// DecodeLogFile processes meta data for transfer and content encoding, and then uses the binary log decoder functionality
        /// </summary>
        /// <param name="logInfoContainer"></param>
        public void DecodeLogFile( LogCargoContainer logInfoContainer )
        {
            Log.Information($"DeviceUIServices::DecodeLogFile entry Log:{logInfoContainer.Name}");
            if  ( null == logInfoContainer.LogRawBytes )
            {
                if  ( null != logInfoContainer.LogXmlStr )
                {
                    ConvertXmlLogFile( logInfoContainer ) ;
                }
                else if  ( null != logInfoContainer.LogRawBody )
                {
                    ConvertBase64LogFile( logInfoContainer ) ;
                }
                // else we're really confused
            }

            if  ( null != logInfoContainer.LogRawBytes )
            {
                // need to appropriately populate logInfoContainer.decodeType and decodeVersion and then use accordingly

                // Interruption to the process, write out the original data to a file
                // We waited until now so we could use the decodeType in the filename
                // And also so we could compress the data
                // Also, the original source could have contained multiple logs

                // StoreRawDeviceLog( logInfoContainer, memStreamFinal, xmlDoc, logInfoContainer.name ) ;
                // StoreRawDeviceLog( logInfoContainer ) ;
// The store raw log is being done elsewhere now.

                string contentTypeDecoder = (string) LogContentTypeMapping[ logInfoContainer.DecodeType ] ;
                if  ( null != contentTypeDecoder )
                {
                    logInfoContainer.LogDefn = LogDecoder.GetLogDefinition( contentTypeDecoder ) ;
                    if (null == logInfoContainer.LogDefn)
                    {
                        Log.Error($"DeviceUIServices::DecodeLogFile exit logInfoContainer.logDefn:null");
                        return;
                    }

                    logInfoContainer.RenderDefn = LogDecoder.GetRenderDefinition( logInfoContainer.LogDefn, RENDER_NAME ) ;
                    if  ( null == logInfoContainer.RenderDefn )
                    {
                        Log.Error($"DeviceUIServices::DecodeLogFile exit logInfoContainer.renderDefn:null");
                        return;
                    }

                    using ( MemoryStream memStreamFinal = new MemoryStream( logInfoContainer.LogRawBytes ) )
                    {
                        using( BinaryReader br = new BinaryReader( memStreamFinal ) )
                        {
                            LogDecoder.LoadLogFile(br, logInfoContainer.LogDefn, logInfoContainer.RenderDefn, out string headJSON, out string bodyJSON);
                            logInfoContainer.JsonHead = headJSON;
                            logInfoContainer.JsonBody = bodyJSON;

                            if  ( 0 < logInfoContainer.LogDefn.Header.Renders.Count )
                            {
                                logInfoContainer.RenderHeaderHead = logInfoContainer.LogDefn.Header.Renders[0].RenderTableHeader;
                                StringBuilder sbHead = new StringBuilder();
                                sbHead.Append("<headRows>");
                                foreach (string rowStr in logInfoContainer.LogDefn.Header.Renders[0].RenderTableRows)
                                {
                                    sbHead.Append(string.Format("<row>{0}</row>", rowStr));
                                }
                                sbHead.Append("</headRows>");
                                logInfoContainer.RenderHeaderBody = sbHead.ToString();
                            }

                            logInfoContainer.RenderBodyHead = logInfoContainer.RenderDefn.RenderTableHeader;
                            StringBuilder sbBody = new StringBuilder();
                            sbBody.Append( "<bodyRows>" ) ;
                            foreach( string rowStr in logInfoContainer.RenderDefn.RenderTableRows )
                            {
                                sbBody.Append( string.Format( "<row>{0}</row>", rowStr ) ) ;
                            }
                            sbBody.Append( "</bodyRows>" ) ;
                            logInfoContainer.RenderBodyBody = sbBody.ToString();
                        }
                    }
                    // StoreDecodedDeviceLog( logInfoContainer ) ;
                }
                // else did not know how to decode the log.

                logInfoContainer.IsDecoded = true;  // yes, even if we didn't know how to decode it... for now at least.
            }
            Log.Information($"DeviceUIServices::DecodeLogFile exit Result:{logInfoContainer.IsDecoded}");
        }

        public bool IsLogRetrieved( string logName )
        {
            return( RetrievedLogs.ContainsKey( logName ) ) ;
        }

        public string GetDeviceLog( string logName )
        {
            return( GetDeviceLog( logName, null ) ) ;
        }

        /// <summary>
        /// Logs from the device are retrieved using worker threads, so this function needs to be thread-safe
        /// Implemented a sync-object pattern to make the function thread-safe
        /// </summary>
        /// <param name="logName"></param>
        /// <param name="getDeviceLogCallback"></param>
        /// <returns></returns>
        public string GetDeviceLog( string logName, UserInterfaceDelegates.GetDeviceLogCallback getDeviceLogCallback )
        {
            Log.Information($"DeviceUIServices::GetDeviceLog entry Log:{logName}");
            string logInfo = "<?xml version=\"1.0\"?><log><header><name>Example Log</name><sw_ver>VIKING_3.1.5.80</sw_ver><file_ver>1.0</file_ver></header></log>";

            LogAvailability availInfo = null;
            LogCargoContainer logInfoContainer = null;
            bool logRetrievalSuccess = true;

            lock (MSyncRoot)
            {
                if (AvailableLogs.ContainsKey(logName))
                {
                    availInfo = (LogAvailability)AvailableLogs[logName];
                }
                else
                {
                    logRetrievalSuccess = false;
                }

                if (logRetrievalSuccess == true)
                {
                    if (RetrievedLogs.ContainsKey(logName))
                    {
                        logInfoContainer = (LogCargoContainer)RetrievedLogs[logName];
                    }
                    else
                    {
                        try
                        {
                            //vent gives the uri segment value. ex: "ftp://192.168.0.12:3021/ExportLogs/alarms_log.xml"
                            string Dev_Log_URL = availInfo.UriSegment;
                            if (Dev_Log_URL.Contains("ftp:"))
                            {
                                Log.Information($"DeviceUIServices::GetDeviceLog fetch Log:{ServerInterfaceServices.ORIGINATOR_SEND}:{Dev_Log_URL}");
                                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Dev_Log_URL);
                                request.Method = WebRequestMethods.Ftp.DownloadFile;
                                request.UseBinary = true;
                                request.UsePassive = false;
                                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                                // the log from the vent is in xml format
                                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                                logInfo = reader.ReadToEnd();
                                Log.Information($"DeviceUIServices::GetDeviceLog {Dev_Log_URL} FTP Read From Stream is complete Length:{logInfo.Length}");
                                reader.Close();
                                response.Close();
                            }
                            else
                            {
                                Log.Error($"DeviceUIServices::GetGeviceLog: Invalid Logs file format. Name: {Dev_Log_URL}");
                            }
                        }
                        catch (Exception e)
                        {
                            // logInfo will be an empty string, will that be properly handled below?
                            logRetrievalSuccess = false;
                            Log.Error($"DeviceUIServices::GetGeviceLog: Log retrieval failure log: {logName} Exception:{e.Message}");
                        }

                        if (logRetrievalSuccess == true)
                        {
                            try
                            {
                                // Process the log file.
                                LogDecoder.LoadLogDefinitions(LogDefinitionFilename);

                                logInfoContainer = new LogCargoContainer
                                {
                                    DevType = ModalConstants.Instance.GetDebugKey(DeviceInfo.PertinentType.DeviceClassification),
                                    DevId = DeviceInfo.SerialNumber,

                                    // now need to know this "outside" the data actually being retrieved from the device
                                    // the contextual information has been stripped, we no longer get xml with head & body, just body content
                                    Name = logName,
                                    DecodeType = "application/pbVentLog-" + logName,
                                    DecodeVersion = "1.0"
                                };
                                string mypath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                                //Getting file extension
                                string filepath = availInfo.UriSegment;
                                string ext = Path.GetExtension(filepath);
                                if (string.IsNullOrEmpty(ext))
                                {
                                    ext="";
                                }

                                logInfoContainer.LogXsltTransform = Path.Combine(mypath, "LogsXmlToTable.xslt");

                                // Design the file name/timestamp as of having grabbed the file
                                string format = "{5}/{0}-{1}-{2}-{3}-{4}.log{6}.gz";
                                logInfoContainer.LogRawFilename = string.Format(format, DeviceInfo.PertinentType.DeviceClassification,
                                    DeviceInfo.SerialNumber, logInfoContainer.Name, "raw", LogFileTimeStamp, CommsLogFilePath,ext);

                                format = "{5}/{0}-{1}-{2}-{3}-{4}.log.html.gz";
                                logInfoContainer.LogDecodedFilename = string.Format(format, DeviceInfo.PertinentType.DeviceClassification,
                                    DeviceInfo.SerialNumber, logInfoContainer.Name, "decoded", LogFileTimeStamp, CommsLogFilePath);

                                //.raw and .decoded file are exported to RSS
                                format = "{5}/{0}-{1}-{2}-{3}-{4}.log{6}";
                                logInfoContainer.LogLocalStoreFileName = string.Format(format, DeviceInfo.PertinentType.DeviceClassification,
                                    DeviceInfo.SerialNumber, logInfoContainer.Name, "orig", LogFileTimeStamp, CommsLogFilePath,ext);


                                RetrievedLogs.Add(logName, logInfoContainer);

                                if (string.Compare(ext, ".xml", true) == 0)
                                    // filter the incoming xml string to valid xml string
                                    logInfoContainer.LogXmlStr = FilterXml(logInfo);
                                else
                                    //No need to filter if it is non - xml file
                                    logInfoContainer.LogXmlStr = logInfo;

                                logInfoContainer.IsDecoded = true;
                                Log.Information($"DeviceUIServices::GetDeviceLogLog Log retrieved from Vent: {logName}");
                            }
                            catch (Exception e)
                            {
                                logRetrievalSuccess = false;
                                Log.Error($"DeviceUIServices::GetDeviceLogLog retrieval failure. Exception: {e.Message}");
                            }
                        }
                    }
                }
            
                string transactionGuid = Guid.NewGuid().ToString();
                getDeviceLogCallback?.Invoke(transactionGuid, logRetrievalSuccess, logInfoContainer);
                DeviceLogLoadedCallback?.Invoke(transactionGuid, logRetrievalSuccess, logInfoContainer.Name);
                Log.Information($"DeviceUIServices::GetDeviceLog exit Log:{logName} Result:{logRetrievalSuccess}");
                return (transactionGuid);
            }
        }

        /// <summary>
        /// This function returns the HTML version of Device Information
        /// </summary>
        /// <returns></returns>
        public string GetDeviceInfoAsHtml()
        {
            return GetDeviceLogAsHtml("DeviceInfo");
        }

        public string GetDeviceLogAsHtml( string logName )
        {
            Log.Information($"DeviceUIServices::GetDeviceLogAsHtml entry Log:{logName}");

            string retStr = string.Format("<h2>ERROR: Unable to convert log '{0}' to HTML</h2>", logName);

            if (RetrievedLogs.ContainsKey(logName))
            {
                LogCargoContainer logCargo = (LogCargoContainer)RetrievedLogs[logName];

                if (!logCargo.IsDecoded)
                {
                    try
                    {
                        DecodeLogFile(logCargo);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"DeviceUIServices::GetDeviceLogAsHtml Log:{logName} Exception:{e.Message}");
                        return retStr;
                    }
                }

                try
                {
                    XslCompiledTransform xslt = new XslCompiledTransform();

                    xslt.Load(logCargo.LogXsltTransform);

                    StringWriter stringWriter = new StringWriter();
                    xslt.Transform(logCargo.LogLocalStoreFileName, null, stringWriter);

                    string output = stringWriter.ToString();
                    stringWriter.Close();
                    Log.Information($"DeviceUIServices::GetDeviceLogAsHtml exit Log:{logName}");
                    return output;
                }
                catch (Exception e)
                {
                    Log.Error($"DeviceUIServices::GetDeviceLogAsHtml Log:{logName} Exception:{e.Message}");
                    return retStr;
                }
            }
            else
            {
                Log.Error($"DeviceUIServices::GetDeviceLogAsHtml log not found in Retrievelist exit Log:{logName}");
                return retStr;
            }
        }


        public void SaveLogToStream( string logName, Stream stream )
        {
            Log.Information($"DeviceUIServices::SaveLogToStream entry Log:{logName}");
            if ( RetrievedLogs.ContainsKey( logName ) )
            {
                LogCargoContainer logCargo = (LogCargoContainer) RetrievedLogs[ logName ] ;

                if  ( !logCargo.IsDecoded )
                {
                    DecodeLogFile( logCargo ) ;
                }

                try
                {
                    // Handle special case of DeviceInfo
                    if  ( logCargo.Name.Equals( "DeviceInfo" ) )
                    {
                        using(StreamReader fStreamReader = new StreamReader( logCargo.LogRawFilename ) )
                        {
                            string tmpStr = fStreamReader.ReadToEnd();
                            stream.Write(Encoding.ASCII.GetBytes( tmpStr ), 0, tmpStr.Length ) ;
                            fStreamReader.Close();
                        }
                    }
                    else
                    {
                        string partialResults = logCargo.JsonHead.Replace( "[", "" ).Replace( "]", "" ).Replace( " ", "" ) ;
                        stream.Write(Encoding.ASCII.GetBytes( partialResults ), 0, partialResults.Length ) ;

                        stream.Write(Encoding.ASCII.GetBytes( "\n" ), 0, 1 ) ;

                        string partialResults2 = logCargo.JsonBody.Replace( "[", "" ).Replace( "]", "" ).Replace( " ", "" ) ;
                        stream.Write(Encoding.ASCII.GetBytes( partialResults2 ), 0, partialResults2.Length ) ;
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"DeviceUIServices::SaveLogToStream Log:{logName} Exception:{e.Message}");
                    // bummer.  Cannot save.
                }
            }
            else
            {
                Log.Error($"DeviceUIServices::SaveLogToStream log not found in Retrievelist exit Log:{logName}");
                // cannot do anything, should never have been here.
            }
            Log.Information($"DeviceUIServices::SaveLogToStream exit Log:{logName}");
        }

        /// <summary>
        /// This function checks to see if all the logs have been imported from the device
        /// Available logs and retrieved logs counts are checked to detect completion of device logs import
        /// This function is thread-safe
        /// </summary>
        /// <returns></returns>
        public bool IsDeviceLogsImportComplete()
        {
            lock (MSyncRoot)
            {
                Log.Information($"DeviceUIServices::IsDeviceLogsImportComplete() Lock cnt: {AvailableLogs.Count} vs {RetrievedLogs.Count}");

                if (AvailableLogs.Count == RetrievedLogs.Count)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// This function convert a xml string into html using xsl tranformation
        /// </summary>
        /// <param name="xslTranformation">xsl transformation to be applied</param>
        /// <param name="xmlString">string with XML content</param>
        /// <returns></returns>
        private string ConvertXmlStringToHtmlString(string xslTranformation, string xmlString)
        {
            Log.Information($"DeviceUIServices::ConvertXmlStringToHtmlString entry Log:{xmlString}");
            string outputHtml = "";
            if (string.IsNullOrEmpty(xslTranformation))
            {
                return outputHtml;
            }

            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(xslTranformation);
            StringWriter htmlString = new StringWriter();

            try
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                {
                    xslt.Transform(reader, null, htmlString);
                }
            }
            catch(Exception e)
            {
                Log.Error($"DeviceUIServices::ConvertXmlStringToHtmlString Exception: {e.Message}");
            }

            outputHtml = htmlString.ToString();
            Log.Information($"DeviceUIServices::ConvertXmlStringToHtmlString exit Result:{outputHtml.Length}");
            htmlString.Close();
            return outputHtml;
        }

        /// <summary>
        /// This function is used to concatenate all logs into a single file in html format on local hard drive as specified by the user
        /// </summary>
        /// <param name="fileNameWithPath">.html File Name with full path where the log contents in html format need to be stored</param>
        public void SaveAllLogsToFile(string fileNameWithPath)
        {
            Log.Information($"DeviceUIServices::SaveAllLogsToFile entry Log:{fileNameWithPath}");

            //open file anew, convert each xml string for each log to html, write into the file
            // open the file
            FileStream file = new FileStream( fileNameWithPath, FileMode.Create );
            string htmlString = "";

            foreach (string key in RetrievedLogs.Keys)
            {
                LogCargoContainer logCargo = (LogCargoContainer) RetrievedLogs[key];
                if (!string.IsNullOrEmpty(logCargo.LogXsltTransform))
                {
                    htmlString = ConvertXmlStringToHtmlString(logCargo.LogXsltTransform, logCargo.LogXmlStr);
                }
                else
                {
                    htmlString = logCargo.LogXmlStr;
                }

                using (MemoryStream memStream = new MemoryStream(Encoding.ASCII.GetBytes(htmlString.ToString())))
                {
                    file.Write(memStream.ToArray(), 0, (int)memStream.Length);
                }
            }

            Log.Information($"DeviceUIServices::SaveAllLogsToFile exit Result:{htmlString.Length}");
            file.Flush();
            file.Close();
        }

        public void SetDeviceSerialNumber(string deviceSerialNumber)
        {
            DeviceInfo = new DeviceInformation(ModalConstants.PB980_VENTILATOR, deviceSerialNumber);
        }

        public int GetDeviceLogsCount()
        {
            //get the count of device logs only - not device info
            int allDeviceLogsCount = 0;
            foreach (string retrievedLog in RetrievedLogs.Keys)
            {
                allDeviceLogsCount++;
            }
            return allDeviceLogsCount;
        }

        /// <summary>
        /// Fetches Device related information using Http interface
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string DeviceWebRequest(string url)
        {
            Log.Information($"DeviceUIServices::DeviceWebRequest entry URL:{url}");
            lock (MSyncRoot)
            {
                HttpWebResponse response = default;
                StreamReader reader = default;
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    response = (HttpWebResponse)request.GetResponse();
                    reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    var devInfo = reader.ReadToEnd();
                    reader.Close();
                    response.Close();
                    Log.Information($"DeviceUIServices::DeviceWebRequest exit Length:{devInfo.Length}");
                    return devInfo;
                }
                catch (Exception e)
                {
                    response?.Close();
                    reader?.Close();
                    Log.Error($"DeviceUIServices::DeviceWebRequest exit Exception:{ e.Message}");
                    return null;
                }
            }
        }

        /// <summary>
        /// Parses Result xml node information and returns true or false.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private static string ParseResult(string data, string resultToken)
        {
            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    var doc = new XmlDocument();
                    doc.LoadXml(data);
                    var xmlNodeList = doc.GetElementsByTagName(resultToken);
                    if (xmlNodeList.Count >= 1)
                    {
                        // Reading first node data, generally only one Result node present.
                        var node = xmlNodeList.Item(0);
                        return node?.InnerText?.Trim();
                    }
                    else
                    {
                        Log.Error($"DeviceUIServices::ParseResultString failed, /result tag isn't found");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"DeviceUIServices::ParseResultString failed, empty string Exception:{ex.Message}");
                }
            }
            else
            {
                Log.Error($"DeviceUIServices::ParseResultString failed, empty string");
            }
            return "";
        }
    }
}
