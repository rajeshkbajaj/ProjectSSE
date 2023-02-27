using System.Collections.Generic;
using Covidien.CGRS.PcAgentInterfaceBusiness;
using Oasis.Agent.Models;
using Serilog;

namespace Covidien.CGRS.ESS
{
    public class SoftwarePackageManagement
    {
        /// <summary>
        /// The variable provides the locking object
        /// </summary>
        private static readonly object MSyncRoot = new object();

        /// <summary>
        /// Singleton
        /// </summary>
        /// <returns>Instance</returns>
        private static volatile SoftwarePackageManagement MSingleton;
        public static SoftwarePackageManagement Instance
        {
            get
            {
                if (null == MSingleton)
                {
                    lock (MSyncRoot)
                    {
                        if (null == MSingleton)
                        {
                            MSingleton = new SoftwarePackageManagement();
                        }
                    }
                }

                return MSingleton;
            }
        }

        /// <summary>
        /// Ctor registers for Notification Processing Complete event
        /// </summary>
        SoftwarePackageManagement()
        {
            //auto register for this event
            BusinessServicesBridge.Instance.RegisterForNotificationsEvents(NotificationsCallback, 
                                                                                                    NotificationProcessedCallback, 
                                                                                                    AllNotificationsProcessed);
        }


        /// <summary>
        /// This function is called by BussinessServicesBridge when notifications are received
        /// </summary>
        private void NotificationsCallback(int nNumberOfNotifications)
        {
            Log.Information($"SoftwarePackageManagement:NotificationsCallback  Entry Noof Notifications:{nNumberOfNotifications}");
            if (ESS_Main.Instance.InvokeRequired)
            {
                ESS_Main.Instance.Invoke(myNotificationsCallback, nNumberOfNotifications);
            }
            else
            {
                myNotificationsCallback(nNumberOfNotifications);
            }
            Log.Information($"SoftwarePackageManagement:NotificationsCallback  Exit Noof Notifications:{nNumberOfNotifications}");
        }

        /// <summary>
        /// This function is called by BussinessServicesBridge when a notification is processed
        /// </summary>
        private void NotificationProcessedCallback()
        {
            Log.Information($"SoftwarePackageManagement:NotificationProcessedCallback  Entry");
            if (ESS_Main.Instance.InvokeRequired)
            {
                ESS_Main.Instance.Invoke(myNotificationProcessedCallback);
            }
            else
            {
                myNotificationProcessedCallback();
            }
            Log.Information($"SoftwarePackageManagement:NotificationProcessedCallback  Exit");
        }

        /// <summary>
        /// This function is called by BussinessServicesBridge when all notifications are processed
        /// </summary>
        private void AllNotificationsProcessed()
        {
            Log.Information($"SoftwarePackageManagement:AllNotificationsProcessed  Entry");
            if (ESS_Main.Instance.InvokeRequired)
            {
                ESS_Main.Instance.Invoke(myNotificationProcessingCompleteCallback);
            }
            else
            {
                myNotificationProcessingCompleteCallback();
            }
            Log.Information($"SoftwarePackageManagement:AllNotificationsProcessed  Exit");
        }

        /// <summary>
        /// Registers any function callback which want to listen to notification processing complete event
        /// </summary>
        /// <param name="notificationProcessingCompleteCallback"></param>
        public void RegisterForNotificationsEvents(InterfaceDelegates.Notifications notificationsCallback,
                                                    InterfaceDelegates.NotificationProcessed notificationProcessedCallback,
                                                    InterfaceDelegates.AllNotificationsProcessed allNotificationsProcessedCallback)
        {
            Log.Information($"SoftwarePackageManagement:RegisterForNotificationsEvents");
            myNotificationsCallback = notificationsCallback;
            myNotificationProcessedCallback = notificationProcessedCallback;
            myNotificationProcessingCompleteCallback = allNotificationsProcessedCallback;
        }

        /// <summary>
        /// GetSoftwareUpdatePackages()
        /// </summary>
        /// <returns>List of all software update packages</returns>
        public List<Software> GetSoftwareUpdatePackages()
        {
            Log.Information($"SoftwarePackageManagement:GetSoftwareUpdatePackages");
            return BusinessServicesBridge.Instance.GetSoftwareDownloadList(); ;
        }

        /// <summary>
        /// ReprogramDevice - Starts the file download process
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="startedCallback"></param>
        /// <param name="progressCallback"></param>
        /// <param name="doneCallback"></param>
        public void ReprogramDevice(string packageName,
                                    bool isLocalBrwsePackage,
                                    InterfaceDelegates.DeviceFlashProcessStarted startedCallback,
                                    InterfaceDelegates.DeviceFlashProgressStatus progressCallback, 
                                    InterfaceDelegates.DeviceFlashProcessComplete doneCallback,
                                    InterfaceDelegates.DeviceFlashFailed failedCallback,InterfaceDelegates.OtherPackageDownloadProcessComplete otherpackagedownloadcallback)
        {
            Log.Information($"SoftwarePackageManagement:ReprogramDevice  Entry packageName:{packageName} isLocalBrwsePackage:{isLocalBrwsePackage}");
            myDeviceFlashStartedCallback = startedCallback;
            myDeviceFlashProgressCallback = progressCallback;
            myDeviceFlashCompleteCallback = doneCallback;
            myDeviceFlashFailedCallback = failedCallback;
            myOtherPackageDownloadCallback = otherpackagedownloadcallback;

//            SoftwarePackageManagement.Instance.FetchVentUpgradePossiblePackages()
            // tell business layer to flash device
            //BusinessServicesBridge.Instance.DoSoftwareDownload(packageName, ReprogrammingProgressCallback, ReprogrammingCompleteCallback, ReprogrammingFailedCallback);
            BusinessServicesBridge.Instance.DoSoftwareDownload(packageName,
                                                                isLocalBrwsePackage,
                                                                ReprogrammingStartedCallback, 
                                                                ReprogrammingProgressCallback, 
                                                                ReprogrammingCompleteCallback,
                                                                ReprogrammingFailedCallback, ReprogrammingOtherPackageDownloadCallback);
            Log.Information($"SoftwarePackageManagement:ReprogramDevice  Exit packageName:{packageName} isLocalBrwsePackage:{isLocalBrwsePackage}");
        }

        InterfaceDelegates.DeviceFlashProcessStarted myDeviceFlashStartedCallback;
        InterfaceDelegates.DeviceFlashProgressStatus myDeviceFlashProgressCallback;
        InterfaceDelegates.DeviceFlashProcessComplete myDeviceFlashCompleteCallback;
        InterfaceDelegates.DeviceFlashFailed myDeviceFlashFailedCallback;

        InterfaceDelegates.Notifications myNotificationsCallback;
        InterfaceDelegates.NotificationProcessed myNotificationProcessedCallback;
        InterfaceDelegates.AllNotificationsProcessed myNotificationProcessingCompleteCallback;

        InterfaceDelegates.OtherPackageDownloadProcessComplete myOtherPackageDownloadCallback;

        private void ReprogrammingStartedCallback(string cpu)
        {
            Log.Information($"SoftwarePackageManagement:ReprogrammingStartedCallback  Entry cpu:{cpu}");
            if (ESS_Main.Instance.InvokeRequired)
            {
                ESS_Main.Instance.Invoke(myDeviceFlashStartedCallback, cpu);
            }
            else
            {
                myDeviceFlashStartedCallback(cpu);
            }
            Log.Information($"SoftwarePackageManagement:ReprogrammingStartedCallback  Exit cpu:{cpu}");
        }

        /// <summary>
        /// ReprogammingProgressCallback - called with updates to the reprogramming process, invokes a thread safe ESS callback to update the display
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="component"></param>
        /// <param name="msg"></param>
        private void ReprogrammingProgressCallback(string cpu, string component, string msg)
        {
            Log.Information($"SoftwarePackageManagement:ReprogrammingProgressCallback  Entry cpu:{cpu} component:{component} msg:{msg}");
            if (ESS_Main.Instance.InvokeRequired)
            {
                ESS_Main.Instance.Invoke(myDeviceFlashProgressCallback, cpu, component, msg);
            }
            else
            {
                myDeviceFlashProgressCallback(cpu, component, msg);
            }
            Log.Information($"SoftwarePackageManagement:ReprogrammingProgressCallback  Exit cpu:{cpu} component:{component} msg:{msg}");
        }

        /// <summary>
        /// ReprogammingCompleteCallback - called when the reprogramming process is complete, invokes a thread safe ESS callback to update the display
        /// </summary>
        private void ReprogrammingCompleteCallback()
        {
            Log.Information($"SoftwarePackageManagement:ReprogrammingCompleteCallback  Entry");
            if (ESS_Main.Instance.InvokeRequired)
            {
                ESS_Main.Instance.Invoke(myDeviceFlashCompleteCallback);
            }
            else
            {
                myDeviceFlashCompleteCallback();
            }
            Log.Information($"SoftwarePackageManagement:ReprogrammingCompleteCallback  Exit");
        }

        private void ReprogrammingFailedCallback(string errorMessage)
        {
            Log.Information($"SoftwarePackageManagement:ReprogrammingFailedCallback  Entry errorMessage:{errorMessage}");
            if (ESS_Main.Instance.InvokeRequired)
            {
                ESS_Main.Instance.Invoke(myDeviceFlashFailedCallback, errorMessage);
            }
            else
            {
                myDeviceFlashFailedCallback(errorMessage);
            }
            Log.Information($"SoftwarePackageManagement:ReprogrammingFailedCallback  Exit errorMessage:{errorMessage}");
        }

        private void ReprogrammingOtherPackageDownloadCallback(bool actionSuccessful)
        {
            Log.Information($"SoftwarePackageManagement:ReprogrammingOtherPackageDownloadCallback  Entry actionSuccessful:{actionSuccessful}");
            if (ESS_Main.Instance.InvokeRequired)
            {
                ESS_Main.Instance.Invoke(myOtherPackageDownloadCallback,actionSuccessful);
            }
            else
            {
                myOtherPackageDownloadCallback(actionSuccessful);
            }
            Log.Information($"SoftwarePackageManagement:ReprogrammingOtherPackageDownloadCallback  Exit actionSuccessful:{actionSuccessful}");
        }

        /// <summary>
        /// PreloadedDocuments - returns a sorted set of all documents currently cached on the system
        /// </summary>
        /// <returns>List(models) of List(packages) of list(documents)</returns>
        public List<KeyValuePair<string, List<KeyValuePair<string, List<KeyValuePair<string, string>>>>>> PreloadedDocuments()
        {
            Log.Information($"SoftwarePackageManagement:ReprogrammingOtherPackageDownloadCallback");
            return BusinessServicesBridge.Instance.GetKnownDocuments();
        }
 
        /// <summary>
        /// DeviceDataOnAgent - 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="serialNum"></param>
        /// <returns>Returns true if the device information block has been retrieved for a device</returns>
        public bool? DeviceDataOnAgent(string model, string serialNum)
        {
            Log.Information($"SoftwarePackageManagement:DeviceDataOnAgent Entry model:{model} serialNum:{serialNum}");
            DeviceDataFromServer data = BusinessServicesBridge.Instance.GetDeviceDataFromServer(model, serialNum);
            bool? retVal = data.Exists;
            if (retVal == true)
            {
                if (data.OutstandingRequestCnt > 0)
                    retVal = null;
            }
            Log.Information($"SoftwarePackageManagement:DeviceDataOnAgent Exit model:{model} serialNum:{serialNum}");

            return retVal;
        }

        /// <summary>
        /// SoftwarePackageCachedForDevice
        /// </summary>
        /// <param name="model"></param>
        /// <param name="serialNum"></param>
        /// <returns>Returns true if there is a software update package for a specific device on the system</returns>
        public bool SoftwarePackageCachedForDevice(string model, string serialNum)
        {
            bool retVal = false;
            DeviceDataFromServer data = BusinessServicesBridge.Instance.GetDeviceDataFromServer(model, serialNum);
            Log.Information($"SoftwarePackageManagement:SoftwarePackageCachedForDevice Entry model:{model} serialNum:{serialNum}");
            if (data.Exists == true)
            {
                List<SoftwareUpdateInfo> infoList = data.SoftwareUpdates;
                foreach (SoftwareUpdateInfo info in infoList)
                {
                    if (info.SoftwarePackages.Count > 0)
                        retVal = true;
                }
            }
            Log.Information($"SoftwarePackageManagement:SoftwarePackageCachedForDevice Exit model:{model} serialNum:{serialNum}");
            return retVal;
        }

        /// <summary>
        /// DocumentsCachedForModel
        /// </summary>
        /// <param name="model"></param>
        /// <param name="serialNum"></param>
        /// <returns>Returns true if there are documents cached for a particular model of device</returns>
        public bool DocumentsCachedForModel(string model, string serialNum)
        {
            bool retVal = false;
            DeviceDataFromServer data = BusinessServicesBridge.Instance.GetDeviceDataFromServer(model, serialNum);
            Log.Information($"SoftwarePackageManagement:DocumentsCachedForModel Entry model:{model} serialNum:{serialNum}");
            if (data.Exists == true)
            {
                List<SoftwareUpdateInfo> infoList = data.SoftwareUpdates;
                foreach (SoftwareUpdateInfo info in infoList)
                {
                    if (info.DocumentPackages.Count > 0)
                        retVal = true;
                }
            }
            Log.Information($"SoftwarePackageManagement:DocumentsCachedForModel Exit model:{model} serialNum:{serialNum}");
            return retVal;
        }

        /// <summary>
        /// gets software packages from server for a specific serial number
        /// </summary>
        /// <param name="deviceSerialNumber"></param>
        public bool RetrieveSoftwarePackagesFromServer()
        {
            Log.Information($"SoftwarePackageManagement:RetrieveSoftwarePackagesFromServer ");
            return BusinessServicesBridge.Instance.RetrieveSoftwarePackagesFromServer();
        }

        public void SetDeviceSerialNumber(string deviceSerialNumber)
        {
            Log.Information($"SoftwarePackageManagement:SetDeviceSerialNumber deviceSerialNumber:{deviceSerialNumber}");
            BusinessServicesBridge.Instance.SetDeviceSerialNumber(deviceSerialNumber);
        }

        public string GetOtherSoftwareSavePath()
        {
            Log.Information($"SoftwarePackageManagement:GetOtherSoftwareSavePath");
            return BusinessServicesBridge.Instance.OtherSoftwarePkgSavedPath;
        }
    }
}
