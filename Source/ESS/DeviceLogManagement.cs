// ------------------------------------------------------------------------------
//                    Copyright (c) 2022 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------
namespace Covidien.CGRS.ESS
{
    using System.Collections.Generic;
    using System.IO;
    using Covidien.CGRS.PcAgentInterfaceBusiness;
    using Serilog;

    public class DeviceLogManagement
    {
        /// <summary>
        /// The variable provides the locking object
        /// </summary>
        private static readonly object MSyncRoot = new object();
       
        /// <summary>
        /// Singleton
        /// </summary>
        /// <returns>Instance</returns>
        private static volatile DeviceLogManagement MSingleton;
        public static DeviceLogManagement Instance
        {
            get
            {
                if (null == MSingleton)
                {
                    lock (MSyncRoot)
                    {
                        if (null == MSingleton)
                        {
                            MSingleton = new DeviceLogManagement();
                        }
                    }
                }

                return MSingleton;
            }
        }

        private DeviceLogManagement()
        {
            //auto register for this event
            //PcAgentInterfaceBusiness.BusinessServicesBridge.Instance.RegisterForDeviceExistsEvent(DeviceExistsEventCallback);
        }


        public void GetDeviceLogs(InterfaceDelegates.DeviceLogList logListCallback, InterfaceDelegates.DeviceLogLoaded logLoadedCallback, InterfaceDelegates.AllDeviceLogsLoaded allDeviceLogsLoadedCallback)
        {
            Log.Information($"DeviceLogManagement:GetDeviceLogs ");
            myDeviceLogListCallback = logListCallback;
            myDeviceLogLoadedCallback = logLoadedCallback;
            myAllDeviceLogsLoadedCallback = allDeviceLogsLoadedCallback;
            BusinessServicesBridge.Instance.GetDeviceLogs(DeviceLogsListCallback, DeviceLogLoadedCallback);
        }

        InterfaceDelegates.DeviceLogList myDeviceLogListCallback;
        InterfaceDelegates.DeviceLogLoaded myDeviceLogLoadedCallback;
        InterfaceDelegates.AllDeviceLogsLoaded myAllDeviceLogsLoadedCallback;

        InterfaceDelegates.DeviceLogUploadedToServer myDeviceLogUploadedToServerCallback;
        InterfaceDelegates.AllDeviceLogsUploadedToServer myAllDeviceLogsUploadedToServerCallback;

        InterfaceDelegates.ClearDeviceLogsCallback myDeviceLogsClearedCallback;

        private void DeviceLogsListCallback(string transactionId, bool actionSuccessful, List<string> deviceLogIds)
        {
            Log.Information($"DeviceLogManagement:DeviceLogsListCallback Entry InvokeRequired:{ESS_Main.Instance.InvokeRequired} transactionId:{transactionId}actionSuccessful:{actionSuccessful}");
            if (ESS_Main.Instance.InvokeRequired)
            {
                ESS_Main.Instance.Invoke(myDeviceLogListCallback, actionSuccessful, deviceLogIds);
            }
            else
            {
                myDeviceLogListCallback(actionSuccessful, deviceLogIds);
            }
            Log.Information($"DeviceLogManagement:DeviceLogsListCallback Exit InvokeRequired:{ESS_Main.Instance.InvokeRequired} transactionId:{transactionId}actionSuccessful:{actionSuccessful}");

        }

        private void DeviceLogLoadedCallback(string transactionId, bool actionSuccessful, string deviceLog)
        {
            Log.Information($"DeviceLogManagement:DeviceLogLoadedCallback Entry deviceLog:{deviceLog} transactionId:{transactionId}actionSuccessful:{actionSuccessful}");
            if (ESS_Main.Instance.InvokeRequired)
            {
                ESS_Main.Instance.Invoke(myDeviceLogLoadedCallback, actionSuccessful, deviceLog);
            }
            else
            {
                myDeviceLogLoadedCallback(actionSuccessful, deviceLog);
            }

            if (actionSuccessful == false)
            {
                if (ESS_Main.Instance.InvokeRequired)
                {
                    ESS_Main.Instance.Invoke(myAllDeviceLogsLoadedCallback, actionSuccessful);
                }
                else
                {
                    myAllDeviceLogsLoadedCallback(actionSuccessful);
                }
            }
            else
            {
                //check to see if all the logs have been downloaded - inform the user app if logs download is complete
                if (BusinessServicesBridge.Instance.IsDeviceLogsImportComplete() == true)
                {
                    BusinessServicesBridge.Instance.ClearDeviceLogsUploadStatus();

                    if (ESS_Main.Instance.InvokeRequired)
                    {
                        ESS_Main.Instance.Invoke(myAllDeviceLogsLoadedCallback, actionSuccessful);
                    }
                    else
                    {
                        myAllDeviceLogsLoadedCallback(actionSuccessful);
                    }
                }
            }
            Log.Information($"DeviceLogManagement:DeviceLogLoadedCallback Exit deviceLog:{deviceLog} transactionId:{transactionId}actionSuccessful:{actionSuccessful}");

        }

        public bool IsDeviceLogAvailable(string logName)
        {
            Log.Information($"DeviceLogManagement:IsDeviceLogAvailable  logName:{logName}");

            return BusinessServicesBridge.Instance.Device.IsLogRetrieved(logName);
        }

        public void GetDeviceLog(string logName)
        {
            Log.Information($"DeviceLogManagement:GetDeviceLog  logName:{logName}");
            BusinessServicesBridge.Instance.Device.GetDeviceLog(logName);
        }

        public string GetDeviceInfoAsHtml()
        {
            Log.Information($"DeviceLogManagement:GetDeviceInfoAsHtml ");
            return BusinessServicesBridge.Instance.Device.GetDeviceInfoAsHtml();
        }

        public string GetDeviceLogAsHtml(string logName)
        {
            Log.Information($"DeviceLogManagement:GetDeviceLogAsHtml  logName:{logName}");
            return BusinessServicesBridge.Instance.Device.GetDeviceLogAsHtml(logName);
        }

        public void SaveLogToStream(string logname, Stream fs)
        {
            Log.Information($"DeviceLogManagement:SaveLogToStream  logname:{logname}");
            BusinessServicesBridge.Instance.Device.SaveLogToStream(logname, fs);
        }

        public bool IsDeviceLogStored(string logName)
        {
            Log.Information($"DeviceLogManagement:IsDeviceLogStored  logName:{logName}");
            return BusinessServicesBridge.Instance.IsLogSavedToServer(logName);
        }

        public void SendLogToServer(string logName)
        {
            Log.Information($"DeviceLogManagement:SendLogToServer  logName:{logName}");
            BusinessServicesBridge.Instance.SaveLogToServer(logName);
        }

        /// <summary>
        /// called when a log is uploaded to server
        /// </summary>
        /// <param name="actionSuccessful"></param>
        /// <param name="logName"></param>
        private void DeviceLogUploadedToServerCallback(bool actionSuccessful, string logName)
        {
            Log.Information($"DeviceLogManagement:DeviceLogUploadedToServerCallback  logName:{logName} actionSuccessful:{actionSuccessful}");
            if (ESS_Main.Instance.InvokeRequired)
            {
                object[] paramArray = new object[]{actionSuccessful, logName};
                ESS_Main.Instance.Invoke(myDeviceLogUploadedToServerCallback, paramArray);
            }
            else
            {
                myDeviceLogUploadedToServerCallback(actionSuccessful, logName);
            }
        }

        /// <summary>
        /// called when all the requested devicelogs are uploaded
        /// </summary>
        /// <param name="actionSuccessful"></param>
        private void AllDeviceLogsUploadedToServerCallback(bool actionSuccessful)
        {
            Log.Information($"DeviceLogManagement:AllDeviceLogsUploadedToServerCallback actionSuccessful:{actionSuccessful}");
            if (ESS_Main.Instance.InvokeRequired)
            {
                ESS_Main.Instance.Invoke(myAllDeviceLogsUploadedToServerCallback, actionSuccessful);
            }
            else
            {
                myAllDeviceLogsUploadedToServerCallback(actionSuccessful);
            }
        }

        /// <summary>
        ///  This function is called by the user-app to upload all the logs into the remote server
        ///  Uses business services bridge instance service
        /// </summary>
        public string UploadAllLogsToServer(InterfaceDelegates.DeviceLogUploadedToServer logUploadedToServerCallback, InterfaceDelegates.AllDeviceLogsUploadedToServer allLogsUploadedToServerCallback, bool diagnostiDeviceLogsUploadOnly)
        {
            Log.Information($"DeviceLogManagement:UploadAllLogsToServer Entry");
            if (BusinessServicesBridge.Instance.IsRSASessionOpened() == false)
            {
                Log.Error($"DeviceLogManagement:UploadAllLogsToServer  RSA Session Not Available\"");
                return "RSA Session Not Available";
            }
            myDeviceLogUploadedToServerCallback = logUploadedToServerCallback;
            myAllDeviceLogsUploadedToServerCallback = allLogsUploadedToServerCallback;

            BusinessServicesBridge.Instance.UploadAllLogsToServer(DeviceLogUploadedToServerCallback, AllDeviceLogsUploadedToServerCallback, diagnostiDeviceLogsUploadOnly);
            Log.Information($"DeviceLogManagement:UploadAllLogsToServer Exit");
            return "";
        }

        public int GetDiagnosticDeviceLogsCount()
        {
            Log.Information($"DeviceLogManagement:GetDiagnosticDeviceLogsCount ");
            return BusinessServicesBridge.Instance.GetDeviceLogsCount(true);
        }

        public void GetDeviceLogNames(List<string> deviceLogNamesArray)
        {
            Log.Information($"DeviceLogManagement:GetDeviceLogNames");
            BusinessServicesBridge.Instance.GetDeviceLogNames(deviceLogNamesArray);
            return;
        }

       
        /// <summary>
        /// This function is used to concatenate all logs into a single file on local hard drive as specified by the user
        ///  Uses business services bridge instance service
        /// </summary>
        /// <param name="fileNameWithPath">File Name with full path where the log contents need to be stored</param>
        public void SaveAllLogsToFile(string fileNameWithPath)
        {
            Log.Information($"DeviceLogManagement:SaveAllLogsToFile");
            BusinessServicesBridge.Instance.SaveAllLogsToFile(fileNameWithPath);
        }

        private void ClearDeviceLogsCallback(string transactionId, bool actionSuccessful, string result)
        {
            Log.Information($"DeviceLogManagement:ClearDeviceLogsCallback Entry");
            if (ESS_Main.Instance.InvokeRequired)
            {
                object[] paramArray = new object[] { actionSuccessful, result };
                ESS_Main.Instance.Invoke(myDeviceLogsClearedCallback, paramArray);
            }
            else
            {
                myDeviceLogsClearedCallback(actionSuccessful, result);
            }
            Log.Information($"DeviceLogManagement:ClearDeviceLogsCallback Exit");
        }
 
        /// <summary>
        /// This function is used to clear all logs on the device (PB980)
        ///  Uses business services bridge instance service
        /// </summary>
        public void ClearDeviceLogs(InterfaceDelegates.ClearDeviceLogsCallback clearDeviceLogsCallback)
        {
            Log.Information($"DeviceLogManagement:ClearDeviceLogs");
            myDeviceLogsClearedCallback = clearDeviceLogsCallback;
            BusinessServicesBridge.Instance.ClearDeviceLogs(ClearDeviceLogsCallback);
        }

        /// <summary>
        /// ClearDeviceLogsUploadStatus
        /// </summary>
        public void ClearDeviceLogsUploadStatus()
        {
            BusinessServicesBridge.Instance.ClearDeviceLogsUploadStatus();
        }
    }
}
