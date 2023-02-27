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

    public class RadioButtonDeviceSettingControl : DeviceSettingControl
    {
        public RadioButtonDeviceSettingControl(string displayName, Point topLeft, DeviceSettingConfigurationParameters deviceSettingConfigParams) :
            base(displayName, topLeft, deviceSettingConfigParams)
        {
            this.SettingValueTextBox.Visible = false;
            this.RadioButtonGroupBox.Visible = true;
            this.RadioButtonNo.Visible = true;
            this.RadioButtonNo.Select();
            this.RadioButtonYes.Visible = true;
            //disable by default
            this.RadioButtonGroupBox.Enabled = false;
            RadioButtonYes.CheckedChanged += this.SettingValueChanged;
            RadioButtonNo.CheckedChanged += this.SettingValueChanged;
        }

        public override bool IsSettingValueChanged()
        {
            return (RadioButtonYes.Checked);
        }

        public override bool RefreshValue()
        {
            Log.Information($"RadioButtonDeviceSettingControl:RefreshValue");
            RadioButtonNo.Select();
            RadioButtonGroupBox.BackColor = Color.Empty;
            return true;
        }

        public override void UpdatePrivileges()
        {
            Log.Information($"RadioButtonDeviceSettingControl:UpdatePrivileges Entry");
            base.UpdatePrivileges();
            RadioButtonGroupBox.Enabled = (AuthenticationService.Instance().GetCredentialsForFunction(DeviceSettingConfigurationParameters.mFunctionId) == AuthenticationService.CREDENTIAL_STATUS.APPROVED);
            Log.Information($"RadioButtonDeviceSettingControl:UpdatePrivileges Exit");
        }

        public override void DeviceConnected()
        {
            Log.Information($"RadioButtonDeviceSettingControl:DeviceConnected Entry");
            this.RadioButtonGroupBox.Enabled = true;
            RadioButtonGroupBox.BackColor = Color.Empty;
            this.RadioButtonNo.Select();
            //enable hours textbox only when the rights allow
            UpdatePrivileges();
            Log.Information($"RadioButtonDeviceSettingControl:DeviceConnected Exit");
        }

        public override void DeviceDisconnected()
        {
            Log.Information($"RadioButtonDeviceSettingControl:DeviceDisconnected");
            this.RadioButtonGroupBox.Enabled = false;
        }

        public override bool SubmitValue()
        {
            //base implementation
            //set the value - use BusinessServices service
            Log.Information($"RadioButtonDeviceSettingControl:SubmitValue Entry");
            if (string.IsNullOrEmpty(DeviceSettingConfigurationParameters.mSetSettingValueCommandString) || (RadioButtonYes.Checked == false))
                return true;

            try
            {
                string resultStatus = SetValueOnDevice();
                if (resultStatus.StartsWith("success", StringComparison.OrdinalIgnoreCase) == true)
                {
                    RadioButtonGroupBox.BackColor = Color.Empty;
                    return true;
                }
                else
                {
                    RadioButtonGroupBox.BackColor = Color.Red;
                }
            }
            catch(Exception ex)
            {
                Log.Error($"RadioButtonDeviceSettingControl:SubmitValue Exception :{ex.Message}");
                RadioButtonGroupBox.BackColor = Color.Red;
            }
            Log.Information($"RadioButtonDeviceSettingControl:SubmitValue Exit");
            return false;
        }

        public override string SetValueOnDevice()
        {
            Log.Information($"RadioButtonDeviceSettingControl:SetValueOnDevice Entry");
            if (RadioButtonYes.Checked == true)
            {
                return BusinessServicesBridge.Instance.Device.SetSettingValueOnDeviceSync(DeviceSettingConfigurationParameters.mSetSettingValueCommandString,
                                                                                            DeviceSettingConfigurationParameters.mSetSettingValueXmlParameterString,
                                                                                            "");
            }
            Log.Information($"RadioButtonDeviceSettingControl:SetValueOnDevice Exit");

            return "SUCCESS";
        }

        protected override void SettingValueChanged(object sender, EventArgs e)
        {
            Log.Information($"RadioButtonDeviceSettingControl:SettingValueChanged");
            if (RadioButtonYes.Checked == true)
            {
                RadioButtonGroupBox.BackColor = Color.Gold;
            }
            else
            {
                RadioButtonGroupBox.BackColor = Color.Empty;
            }
            SettingValueChangedCallback?.Invoke(sender, e);
        }

        public override void SetTabIndex(int tabIndex)
        {
            RadioButtonYes.TabIndex = tabIndex;
        }
    }
}
