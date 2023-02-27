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
    using System;
    using System.Drawing;
    using Covidien.CGRS.PcAgentInterfaceBusiness;
    using Serilog;
    using SoftwareOptionsKeyDotNet;
    using Utilties;

    class DeviceSettingControlExtendEsm : RadioButtonDeviceSettingControl
    {
        private const int ESM_OPTION_ID = 13;

        public DeviceSettingControlExtendEsm(string displayName, Point topLeft, DeviceSettingConfigurationParameters deviceSettingConfigParams) :
                                                base(displayName, topLeft, deviceSettingConfigParams)
        {
        }

        public override void UpdatePrivileges()
        {
            //control only visible for covidien user
            this.Visible = (AuthenticationService.Instance().IsCovidienUser() == true);
            Log.Information($"DeviceSettingControlExtendEsm:UpdatePrivileges IsCovidienUser :{AuthenticationService.Instance().IsCovidienUser()}");
        }

        public override string SetValueOnDevice()
        {
            Log.Information($"DeviceSettingControlExtendEsm:SetValueOnDevice Entry");
            try
            {
                // a) get the key from device, b) set day, month, year for option13 (= ESM) c) generate new key d) set new key on device
                string optionsKey = BusinessServicesBridge.Instance.Device.GetSettingValueFromDeviceSync(DeviceUserInterfaceServices.GET_OPTIONS_KEY_COMMAND_STRING,
                                                                                DeviceUserInterfaceServices.OPTIONS_KEY_VALUE_XML_PARAMETER_STRING);

                if (optionsKey.StartsWith("error", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return "ERROR";
                }


                //decode the htmlencoded key
                optionsKey = KeyEncryption.HtmlDecode(optionsKey);

                SoftwareOptionsKeyDriver pSoftwareOptions = new SoftwareOptionsKeyDriver();
                string deviceSerialNumber = BusinessServicesBridge.Instance.GetDeviceSerialNumber();

                if (deviceSerialNumber == null)
                {
                    return "ERROR";
                }

                pSoftwareOptions.UpdateOptionsKey(optionsKey, deviceSerialNumber);

                //if esm is enabled for longer time than 7 days, disregard this request
                int currentEsmState = pSoftwareOptions.GetOptionState(ESM_OPTION_ID);
                DateTime newDateTime = DateTime.Today.AddDays(7);
                if (currentEsmState == 1)
                {
                    //esm enabled - check its expiry - if mroe than 7 days, no need to take any action
                    int day = pSoftwareOptions.GetExpiryDay(ESM_OPTION_ID);
                    int month = pSoftwareOptions.GetExpiryMonth(ESM_OPTION_ID);
                    int year = pSoftwareOptions.GetExpiryYear(ESM_OPTION_ID);

                    if (day == 0)
                    {
                        //enabled for ever - do not change anything
                        return "SUCCESS";
                    }

                    //now it has date which will expire, if it expires after a week, do not change anything
                    DateTime expiryTime = new DateTime(year + 2000, month, day);
                    //is it enabled forever or more than 7 days from now
                    if (expiryTime >= newDateTime)
                    {
                        return "SUCCESS";
                    }
                }

                //enable ESM
                pSoftwareOptions.SetOptionState(ESM_OPTION_ID, 1);
                
                //set expiry date 1 week from now
                pSoftwareOptions.SetExpiryDate(ESM_OPTION_ID, newDateTime.Day, newDateTime.Month, (newDateTime.Year - 2000));
                string newOptionsKey = pSoftwareOptions.GetEncryptedKey();
                newOptionsKey = KeyEncryption.HtmlEncode(newOptionsKey);

                string setAction = BusinessServicesBridge.Instance.Device.SetSettingValueOnDeviceSync(DeviceUserInterfaceServices.SET_OPTIONS_KEY_COMMAND_STRING,
                                                                                                        DeviceUserInterfaceServices.OPTIONS_KEY_VALUE_XML_PARAMETER_STRING,
                                                                                                        newOptionsKey);
                return setAction;
            }
            catch (Exception)
            {
                Log.Error($"DeviceSettingControlExtendEsm:SetValueOnDevice Exception");
                return "ERROR";
            }
        }
    }
}
