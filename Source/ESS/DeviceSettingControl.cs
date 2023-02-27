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
    using System.Windows.Forms;
    using Covidien.CGRS.PcAgentInterfaceBusiness;
    using Serilog;

    public partial class DeviceSettingControl : UserControl
    {
        protected string DisplayName;
        protected DeviceSettingConfigurationParameters DeviceSettingConfigurationParameters;
        protected bool MIsSettingValueChanged;
        protected bool MSettingChangeMonitor;

        public delegate void ValueChanged(object sender, EventArgs e);

        public ValueChanged SettingValueChangedCallback { get;  set; }

        public DeviceSettingControl(string displayName, Point topLeft, DeviceSettingConfigurationParameters deviceSettingConfigParams)
        {
            InitializeComponent();
            SettingLabel.Text = DisplayName = displayName;
            DeviceSettingConfigurationParameters = deviceSettingConfigParams;

            this.SettingValueTextBox.KeyDown += new KeyEventHandler(this.TextBox_KeyDown);
            this.SettingValueTextBox.TextChanged += this.SettingValueChanged;
            this.Location = topLeft;

            MIsSettingValueChanged = false;

            UpdatePrivileges();

            //monitor setting changes
            MSettingChangeMonitor = true;
        }

        protected virtual void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //Allow navigation keyboard arrows
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.PageUp:
                case Keys.PageDown:
                    e.SuppressKeyPress = false;
                    return;
                default:
                    break;
            }

            //Block non-number characters
            bool modifier = e.Control || e.Alt || e.Shift ||
                            (e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back);
            bool nonNumber = (((e.KeyCode < Keys.D0) || (e.KeyCode > Keys.D9)) &&
                              ((e.KeyCode < Keys.NumPad0) || (e.KeyCode > Keys.NumPad9)));

            //Handle pasted Text
            if (e.Control && e.KeyCode == Keys.V)
            {
                e.SuppressKeyPress = false;
            }
        }

        public virtual void DeviceConnected()
        {
            //enable hours textbox only when the rights allow
            this.SettingValueTextBox.Clear();
            SettingValueTextBox.BackColor = Color.Empty;
            this.SettingValueTextBox.Enabled = true;

            //enable hours textbox only when the rights allow
            UpdatePrivileges();
            Log.Information($"DeviceSettingControl:DeviceConnected ");
        }

        public virtual void DeviceDisconnected()
        {
            this.SettingValueTextBox.Clear();
            SettingValueTextBox.BackColor = Color.Empty;
            this.SettingValueTextBox.Enabled = false;
            Log.Information($"DeviceSettingControl:DeviceDisconnected ");
        }

        public virtual void UpdatePrivileges()
        {
            this.SettingValueTextBox.Enabled = (AuthenticationService.Instance().GetCredentialsForFunction(DeviceSettingConfigurationParameters.mFunctionId) == AuthenticationService.CREDENTIAL_STATUS.APPROVED);
            Log.Information($"DeviceSettingControl:UpdatePrivileges  Privileged:{this.SettingValueTextBox.Enabled}");
        }

        public virtual bool RefreshValue()
        {
            //base implementation
            //get the value - use BusinessServices service
            //set the value - use BusinessServices service
            Log.Information($"DeviceSettingControl:RefreshValue Entry");
            SettingValueTextBox.Clear();
            SettingValueTextBox.BackColor = Color.Empty;

            if (string.IsNullOrEmpty(DeviceSettingConfigurationParameters.mGetSettingValueCommandString))
                return true;

            MSettingChangeMonitor = false;

            try
            {
                string resultStatus = GetValueFromDevice();
                if (resultStatus.StartsWith("error", StringComparison.OrdinalIgnoreCase) == false)
                {
                    SettingValueTextBox.Text = resultStatus;
                    SettingValueTextBox.BackColor = Color.Empty;
                }
                else
                {
                    SettingValueTextBox.BackColor = Color.Red;
                }
            }
            catch
            {
                Log.Error($"DeviceSettingControl:RefreshValue Exception");
                SettingValueTextBox.BackColor = Color.Red;
            }

            MSettingChangeMonitor = true;
            MIsSettingValueChanged = false;
            Log.Information($"DeviceSettingControl:RefreshValue Exit");
            return true;
        }

        public virtual string GetValueFromDevice()
        {
            Log.Information($"DeviceSettingControl:GetValueFromDevice");
            return BusinessServicesBridge.Instance.Device.GetSettingValueFromDeviceSync(DeviceSettingConfigurationParameters.mGetSettingValueCommandString, 
                                                                                        DeviceSettingConfigurationParameters.mGetSettingValueXmlParameterString);
        }

        public virtual bool SubmitValue()
        {
            //base implementation
            //set the value - use BusinessServices service
            Log.Information($"DeviceSettingControl:SubmitValue Entry");
            if (string.IsNullOrEmpty(DeviceSettingConfigurationParameters.mSetSettingValueCommandString) || IsSettingValueChanged() == false)
                return true;

            try
            {
                string resultStatus = SetValueOnDevice();
                if (resultStatus.StartsWith("success", StringComparison.OrdinalIgnoreCase) == true)
                {
                    SettingValueTextBox.BackColor = Color.Empty;
                    MIsSettingValueChanged = false;
                    return true;
                }
                else
                {
                    SettingValueTextBox.BackColor = Color.Red;
                }
            }
            catch
            {
                Log.Error($"DeviceSettingControl:SubmitValue Exception");
                SettingValueTextBox.BackColor = Color.Red;
            }
            Log.Information($"DeviceSettingControl:SubmitValue Exit");
            return false;
        }

        public virtual string SetValueOnDevice()
        {
            Log.Information($"DeviceSettingControl:SetValueOnDevice");
            return BusinessServicesBridge.Instance.Device.SetSettingValueOnDeviceSync(DeviceSettingConfigurationParameters.mSetSettingValueCommandString,
                                                                                        DeviceSettingConfigurationParameters.mSetSettingValueXmlParameterString,
                                                                                        SettingValueTextBox.Text);
        }

        public virtual bool IsSettingValueChanged()
        {
            return MIsSettingValueChanged && (SettingValueTextBox.Text.Length > 0);
        }

        protected virtual void SettingValueChanged(object sender, EventArgs e)
        {
            if (!MSettingChangeMonitor)
                return;

            MIsSettingValueChanged = true;
            //pending change - show it as gold
            if (SettingValueTextBox.Text.Length > 0)
            {
                SettingValueTextBox.BackColor = Color.Gold;
            }
            else
            {
                SettingValueTextBox.BackColor = Color.Empty;
            }

            SettingValueChangedCallback?.Invoke(sender, e);
        }

        public void SetTextBoxWidth(int width)
        {
            SettingValueTextBox.Width = width;
        }

        public int GetTextBoxWidth()
        {
            return SettingValueTextBox.Width;
        }

        public void SetTextBoxXpos(int xpos)
        {
            Point newPos = new Point(xpos, SettingValueTextBox.Location.Y);
            SettingValueTextBox.Location = newPos;
        }

        public virtual void SetTabIndex(int tabIndex)
        {
            //mSettingValueTextBox.TabIndex = tabIndex;
        }
    }
}
