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
    using System.Net;
    using Covidien.CGRS.PcAgentInterfaceBusiness;
    using Serilog;

    public class DeviceManagement
    {
        /// <summary>
        /// The variable provides the locking object
        /// </summary>
        private static readonly object MSyncRoot = new object();
        private InterfaceDelegates.SupportedDeviceModels SupportedDeviceModelsDelegate;
        private List<string> DeviceModelsList;
        private InterfaceDelegates.DeviceSummaryInformation DeviceSummaryInfoCallback1;
        /// <summary>
        /// Singleton
        /// </summary>
        /// <returns>Instance</returns>
        private static volatile DeviceManagement MSingleton;
        public static DeviceManagement Instance
        {
            get
            {
                if (null == MSingleton)
                {
                    lock (MSyncRoot)
                    {
                        if (null == MSingleton)
                        {
                            MSingleton = new DeviceManagement();
                        }
                    }
                }

                return MSingleton;
            }
        }

        // Manage Supported Device Models
        // - more than one model not currently supported by Agent

        public void RequestSupportedDeviceTypes(InterfaceDelegates.SupportedDeviceModels callback)
        {
            Log.Information($"DeviceManagement:RequestSupportedDeviceTypes Entry");
            DeviceModelsList = BusinessServicesBridge.Instance.GetSupportedDeviceTypes();

            SupportedDeviceModelsDelegate = (callback != null) ? new InterfaceDelegates.SupportedDeviceModels(callback) : null;
            SupportedDeviceModelsDelegate?.Invoke(DeviceModelsList);

            Log.Information($"DeviceManagement:RequestSupportedDeviceTypes Exit");
        }

        public List<string> GetSupportedDeviceTypes()
        {
            Log.Information($"DeviceManagement:GetSupportedDeviceTypes");
            return DeviceModelsList;
        }

        public List<KeyValuePair<string, string>> GetKnownDevices()
        {
            Log.Information($"DeviceManagement:GetKnownDevices");
            return BusinessServicesBridge.Instance.GetKnownDevices();
        }

        public void ConnectToDevice(IPAddress ipAddr, int port, InterfaceDelegates.DeviceConnectionStatusChange callback, InterfaceDelegates.DeviceExistsEvent deviceExistsCallback)
        {
            Log.Information($"DeviceManagement:ConnectToDevice Entry ipAddr:{ipAddr} port:{port}");
            myDeviceConnectionStatusChangeCallback = new InterfaceDelegates.DeviceConnectionStatusChange(callback);
            myDeviceExistsEventCallback = new InterfaceDelegates.DeviceExistsEvent(deviceExistsCallback);

            BusinessServicesBridge.Instance.Device.DisconnectCallback = new UserInterfaceDelegates.DisconnectCallback(DisconnectCallback);

            BusinessServicesBridge.Instance.ConnectToDevice(ipAddr, port, ConnectCallback, SessionCreatedCallback, DeviceExistsCallback);
            Log.Information($"DeviceManagement:ConnectToDevice Exit ipAddr:{ipAddr} port:{port}");
        }

        InterfaceDelegates.DeviceConnectionStatusChange myDeviceConnectionStatusChangeCallback;
        InterfaceDelegates.DeviceExistsEvent myDeviceExistsEventCallback;
        
        private void ConnectCallback(bool actionSuccessful)
        {
            Log.Information($"DeviceManagement:ConnectCallback Entry actionSuccessful:{actionSuccessful}");
            if (actionSuccessful == true)
            {
                if (ESS_Main.Instance.InvokeRequired)
                {
                    ESS_Main.Instance.Invoke(myDeviceConnectionStatusChangeCallback, InterfaceDelegates.DeviceStateChangeEvent.DEVICE_CONNECTION_ESTABLISHED);
                }
                else
                {
                    myDeviceConnectionStatusChangeCallback(InterfaceDelegates.DeviceStateChangeEvent.DEVICE_CONNECTION_ESTABLISHED);
                }
            }
            else
            {
                DisconnectCallback();
            }
            Log.Information($"DeviceManagement:ConnectCallback Exit actionSuccessful:{actionSuccessful}");
        }

        public void DisconnectDevice()
        {
            Log.Information($"DeviceManagement:DisconnectDevice");

            // inform business services object
            BusinessServicesBridge.Instance.DisconnectDevice();
        }

        private void DisconnectCallback()
        {
            Log.Information($"DeviceManagement:DisconnectCallback Entry InvokeRequired:{ESS_Main.Instance.InvokeRequired}" );
            if (ESS_Main.Instance.InvokeRequired)
            {
                ESS_Main.Instance.Invoke(myDeviceConnectionStatusChangeCallback, InterfaceDelegates.DeviceStateChangeEvent.DEVICE_DISCONNECTED);
            }
            else
            {
                myDeviceConnectionStatusChangeCallback(InterfaceDelegates.DeviceStateChangeEvent.DEVICE_DISCONNECTED);
            }
            Log.Information($"DeviceManagement:DisconnectCallback Exit InvokeRequired:{ESS_Main.Instance.InvokeRequired}");
        }

        private void SessionCreatedCallback(string transactionId, bool actionSuccessful, string sessionId)
        {
            Log.Information($"DeviceManagement:DisconnectCallback Entry InvokeRequired:{ESS_Main.Instance.InvokeRequired} transactionId:{transactionId} actionSuccessful:{actionSuccessful} sessionId:{sessionId}");
            if (ESS_Main.Instance.InvokeRequired)
            {
                ESS_Main.Instance.Invoke(myDeviceConnectionStatusChangeCallback, InterfaceDelegates.DeviceStateChangeEvent.DEVICE_SESSION_OPENED);
            }
            else
            {
                myDeviceConnectionStatusChangeCallback(InterfaceDelegates.DeviceStateChangeEvent.DEVICE_SESSION_OPENED);
            }
            Log.Information($"DeviceManagement:DisconnectCallback Exit InvokeRequired:{ESS_Main.Instance.InvokeRequired} transactionId:{transactionId} actionSuccessful:{actionSuccessful} sessionId:{sessionId}");

        }

        private void DeviceExistsCallback(bool actionSuccessful)
        {
            Log.Information($"DeviceManagement:DeviceExistsCallback Entry InvokeRequired:{ESS_Main.Instance.InvokeRequired} actionSuccessful:{actionSuccessful}");
            if (ESS_Main.Instance.InvokeRequired)
            {
                ESS_Main.Instance.Invoke(myDeviceExistsEventCallback, actionSuccessful);
            }
            else
            {
                myDeviceExistsEventCallback(actionSuccessful);
            }
            Log.Information($"DeviceManagement:DeviceExistsCallback Exit InvokeRequired:{ESS_Main.Instance.InvokeRequired} actionSuccessful:{actionSuccessful}");
        }

        public bool IsDeviceInDownloadMode()
        {
            Log.Information($"DeviceManagement:IsDeviceInDownloadMode");
            return BusinessServicesBridge.Instance.IsDeviceInDownloadMode;
        }

        public void GetDeviceInformation(InterfaceDelegates.DeviceSummaryInformation callback)
        {
            Log.Information($"DeviceManagement:GetDeviceInformation");
            DeviceSummaryInfoCallback1 = callback;
            BusinessServicesBridge.Instance.Device.GetDeviceInfo(GetDeviceInfoCallback);
        }

        private void GetDeviceInfoCallback( string transactionId, bool isSuccessful, DeviceInformation devInfo )
        {
            Log.Information($"DeviceManagement:GetDeviceInfoCallback Entry transactionId:{transactionId} isSuccessful:{isSuccessful} devInfo:{devInfo}");
            // protect against a user who was not actually connected to a device, hence a null
            if  ( !isSuccessful  ||  ( null == devInfo ) )
            {
                // TODO: correct the state model to indicate NOT CONNECTED
                return;
            }

            string model = ModalConstants.Instance.GetDebugKey(devInfo.PertinentType.DeviceClassification);
            string serialNum = devInfo.SerialNumber;
            string softwareVersion = devInfo.SoftwareVersion;
            string deviceKeyType = devInfo.DeviceKeyType;
            string softwarePartnum = devInfo.SoftwarePartnumber;

            if (ESS_Main.Instance.InvokeRequired)
            {
                ESS_Main.Instance.Invoke(DeviceSummaryInfoCallback1, model, serialNum, softwareVersion, deviceKeyType,softwarePartnum);
            }
            else
            {
                DeviceSummaryInfoCallback1(model, serialNum, softwareVersion, deviceKeyType,softwarePartnum);
            }
            BusinessServicesBridge.Instance.SyncDeviceConfig(devInfo);
            Log.Information($"DeviceManagement:GetDeviceInfoCallback Exit transactionId:{transactionId} isSuccessful:{isSuccessful} devInfo:{devInfo}");
        }

        public void SetDeviceAccessKey(string newKey)
        {
            Log.Information($"DeviceManagement:SetDeviceAccessKey newKey:{newKey}");
            BusinessServicesBridge.Instance.Device.SetDeviceDataKey(newKey);
        }

        public List<KeyValuePair<string, DeviceSettingConfigurationParameters>> GetDeviceHoursSettingConfigurationList()
        {
            Log.Information($"DeviceManagement:GetDeviceHoursSettingConfigurationList Entry");
            List<KeyValuePair<string, DeviceSettingConfigurationParameters>> components = new List<KeyValuePair<string, DeviceSettingConfigurationParameters>>();
            DeviceSettingConfigurationParameters ventOH = new DeviceSettingConfigurationParameters(RestrictedFunctionManager.RestrictedFunctions.SET_OPERATIONAL_HOURS, 
                                                                                                        true,
                                                                                                        DeviceUserInterfaceServices.GET_DEVICE_HOURS_AS_XML, 
                                                                                                        DeviceUserInterfaceServices.SET_VENT_OPERATIONAL_HOURS,
                                                                                                        DeviceUserInterfaceServices.GET_VENT_OPERATIONAL_HOURS_PARAMETER_STRING,
                                                                                                        DeviceUserInterfaceServices.SET_DEVICE_HOURS_PARAMETER_STRING);
            components.Add(new KeyValuePair<string, DeviceSettingConfigurationParameters>(Properties.Resources.VENTILATOR_OP_HOURS,ventOH));

            DeviceSettingConfigurationParameters compressorOH = new DeviceSettingConfigurationParameters(RestrictedFunctionManager.RestrictedFunctions.SET_OPERATIONAL_HOURS,
                                                                                                        true,
                                                                                                        DeviceUserInterfaceServices.GET_DEVICE_HOURS_AS_XML,
                                                                                                        DeviceUserInterfaceServices.SET_COMPRESSOR_OPERATIONAL_HOURS,
                                                                                                        DeviceUserInterfaceServices.GET_COMPRESSOR_OPERATIONAL_HOURS_PARAMETER_STRING,
                                                                                                        DeviceUserInterfaceServices.SET_DEVICE_HOURS_PARAMETER_STRING);
            components.Add(new KeyValuePair<string, DeviceSettingConfigurationParameters>(Properties.Resources.COMPRESSOR_OP_HOURS, compressorOH));

            DeviceSettingConfigurationParameters ventPmH = new DeviceSettingConfigurationParameters(RestrictedFunctionManager.RestrictedFunctions.SET_PREVENTIVE_MAINTENANCE_DUE_HOURS,
                                                                                                        true,
                                                                                                        DeviceUserInterfaceServices.GET_DEVICE_HOURS_AS_XML,
                                                                                                        DeviceUserInterfaceServices.SET_VENT_PREVENTATIVE_MAINTENANCE_HOURS,
                                                                                                        DeviceUserInterfaceServices.GET_VENT_PM_HOURS_PARAMETER_STRING,
                                                                                                        DeviceUserInterfaceServices.SET_DEVICE_HOURS_PARAMETER_STRING);
            components.Add(new KeyValuePair<string, DeviceSettingConfigurationParameters>(Properties.Resources.VENTILATOR_PM_HOURS, ventPmH));

            DeviceSettingConfigurationParameters compressorPmH = new DeviceSettingConfigurationParameters(RestrictedFunctionManager.RestrictedFunctions.SET_PREVENTIVE_MAINTENANCE_DUE_HOURS,
                                                                                                        true,
                                                                                                        DeviceUserInterfaceServices.GET_DEVICE_HOURS_AS_XML,
                                                                                                        DeviceUserInterfaceServices.SET_COMPRESSOR_PREVENTATIVE_MAINTENANCE_HOURS,
                                                                                                        DeviceUserInterfaceServices.GET_COMPRESSOR_PM_HOURS_PARAMETER_STRING,
                                                                                                        DeviceUserInterfaceServices.SET_DEVICE_HOURS_PARAMETER_STRING);
            components.Add(new KeyValuePair<string, DeviceSettingConfigurationParameters>(Properties.Resources.COMPRESSOR_PM_HOURS, compressorPmH));


            DeviceSettingConfigurationParameters compressorSerial = new DeviceSettingConfigurationParameters(RestrictedFunctionManager.RestrictedFunctions.SET_COMPRESSOR_SERIAL_NUMBER,
                                                                                                        true,
                                                                                                        DeviceUserInterfaceServices.GET_COMPRESSOR_SERIAL_NUMBER,
                                                                                                        DeviceUserInterfaceServices.SET_COMPRESSOR_SERIAL_NUMBER,
                                                                                                        DeviceUserInterfaceServices.GET_COMPRESSOR_SERIAL_NUMBER_PARAMETER_STRING,
                                                                                                        DeviceUserInterfaceServices.SET_COMPRESSOR_SERIAL_NUMBER_PARAMETER_STRING);

            components.Add(new KeyValuePair<string, DeviceSettingConfigurationParameters>(Properties.Resources.COMPRESSOR_SERIAL_NUMBER, compressorSerial));

            DeviceSettingConfigurationParameters optionsKey = new DeviceSettingConfigurationParameters(RestrictedFunctionManager.RestrictedFunctions.SET_OPTIONS_KEY,
                                                                                            true,
                                                                                            "",
                                                                                            DeviceUserInterfaceServices.SET_OPTIONS_KEY_COMMAND_STRING,
                                                                                            "",
                                                                                            DeviceUserInterfaceServices.OPTIONS_KEY_VALUE_XML_PARAMETER_STRING);
            components.Add(new KeyValuePair<string, DeviceSettingConfigurationParameters>(Properties.Resources.DATA_KEY, optionsKey));


            DeviceSettingConfigurationParameters estDate = new DeviceSettingConfigurationParameters(RestrictedFunctionManager.RestrictedFunctions.SET_EST_STATUS,
                                                                                            false,
                                                                                            DeviceUserInterfaceServices.GET_SELF_TEST_STATUS_HTTP_COMMAND_STRING,
                                                                                            "",
                                                                                            DeviceUserInterfaceServices.GET_EST_STATUS_PARAMETER_STRING,
                                                                                            "");
            components.Add(new KeyValuePair<string, DeviceSettingConfigurationParameters>(Properties.Resources.EST_STATUS, estDate));

            DeviceSettingConfigurationParameters estStatus = new DeviceSettingConfigurationParameters(RestrictedFunctionManager.RestrictedFunctions.SET_EST_PERFORMED_DATE,
                                                                                            false,
                                                                                            DeviceUserInterfaceServices.GET_SELF_TEST_STATUS_HTTP_COMMAND_STRING,
                                                                                            "",
                                                                                            DeviceUserInterfaceServices.GET_EST_DATE_PARAMETER_STRING,
                                                                                            "");
            components.Add(new KeyValuePair<string, DeviceSettingConfigurationParameters>(Properties.Resources.EST_PERFORMED_DATE, estStatus));

            DeviceSettingConfigurationParameters overrideEst = new DeviceSettingConfigurationParameters(RestrictedFunctionManager.RestrictedFunctions.OVERRIDE_EST_RESULT,
                                                                                            true,
                                                                                            "",
                                                                                            DeviceUserInterfaceServices.OVERRIDE_EST_HTTP_COMMAND_STRING
                                                                                            );
            components.Add(new KeyValuePair<string, DeviceSettingConfigurationParameters>(Properties.Resources.OVERRIDE_EST_QUESTION, overrideEst));

            DeviceSettingConfigurationParameters extendESM = new DeviceSettingConfigurationParameters(RestrictedFunctionManager.RestrictedFunctions.EXTEND_ENHANCED_SERVICE_MODE,
                                                                                            true,
                                                                                            "",
                                                                                            DeviceUserInterfaceServices.EXTEND_ESM_COMMAND_STRING
                                                                                            );
            components.Add(new KeyValuePair<string, DeviceSettingConfigurationParameters>(Properties.Resources.EXTEND_ENHANCED_SERVICE_MODE_QUESTION, extendESM));
            Log.Information($"DeviceManagement:GetDeviceHoursSettingConfigurationList Exit");
            return components;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DeviceManagement()
        {
            SupportedDeviceModelsDelegate = null;
            DeviceModelsList = null;
        }
    }
}
