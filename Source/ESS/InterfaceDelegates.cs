using System.Collections.Generic;
using Covidien.CGRS.PcAgentInterfaceBusiness;

namespace Covidien.CGRS.ESS
{
    public class InterfaceDelegates
    {
        /// <summary>
        ///  Callback delegate invoked in response to the PC Agent returning authentication results
        /// </summary>
        /// <param name="result"></param>
        public delegate void AuthenticationResults(bool result,string reason);

        /// <summary>
        /// Callback delegate invoked in response to the PC Agent providing a list of supported
        /// device models
        /// </summary>
        /// <param name="models"></param>
        public delegate void SupportedDeviceModels(List<string> models); 

        /// <summary>
        /// Callback delegate invoked in response to PC Agent providing updates on device connection status
        /// </summary>
        /// <param name="e"></param>
        public enum DeviceStateChangeEvent { CONNECT, DISCONNECT, CANCEL, DEVICE_CONNECTION_ESTABLISHED, DEVICE_CONNECTION_FAILED, DEVICE_SESSION_OPENED, DEVICE_DISCONNECTED, DUMMY_CONNECT }

        public delegate void DeviceExistsEvent(bool actionSuccessful);

        public delegate void DeviceConnectionStatusChange(DeviceStateChangeEvent e);

        public delegate void DeviceSummaryInformation(string model, string serialNum, string softwareVersion, string deviceKeyType, string softwarePartnum);

        public delegate void GetDeviceComponentInfoCallback(List<DeviceComponentInfo> components);

        public delegate void DeviceLogList(bool actionSuccessful, List<string> logNames);

        public delegate void DeviceLogLoaded(bool actionSuccessful, string logName);

        public delegate void AllDeviceLogsLoaded(bool actionSuccessful);

        public delegate void DeviceLogUploadedToServer(bool actionSuccessful, string logName);

        public delegate void AllDeviceLogsUploadedToServer(bool actionSuccessful);

        public delegate void ClearDeviceLogsCallback(bool actionSuccessful, string result);

        public delegate void DeviceFlashProcessStarted(string cpu);

        public delegate void DeviceFlashProgressStatus(string cpu, string component, string message);

        public delegate void DeviceFlashProcessComplete();

        public delegate void OtherPackageDownloadProcessComplete(bool actionSuccessful);
        public delegate void DeviceFlashFailed(string errMsg);

        public delegate void Notifications(int nCount);

        public delegate void NotificationProcessed();

        public delegate void AllNotificationsProcessed();

        public delegate void OpenDocumentCallback(bool actionSuccessful, string filePath);

    }
}
