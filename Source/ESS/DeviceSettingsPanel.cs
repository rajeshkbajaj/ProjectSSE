using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Covidien.CGRS.ESS
{
    public partial class DeviceSettingsPanel : UserControl
    {
        private List<DeviceSettingControl> ComponentList;
        private bool IsDummyDeviceConnected;
        private bool DeviceInDownloadMode;

        public DeviceSettingsPanel()
        {
            InitializeComponent();
            DeviceInDownloadMode = false;
            UpdateButton.Enabled = false;
            DeviceSettingsPanelProgressBar.Visible = false;
            Setup(DeviceManagement.Instance.GetDeviceHoursSettingConfigurationList());
            DeviceDisconnected();
        }

        private void Setup(List<KeyValuePair<string, DeviceSettingConfigurationParameters>> components)
        {
            Log.Information($"DeviceSettingsPanel:Setup Entry Component count :{components.Count}");
            int number = components.Count;
            Point pos = new Point(0, 100);
            ComponentList = new List<DeviceSettingControl>();
            int tabIndex = 0;

            foreach (KeyValuePair<string, DeviceSettingConfigurationParameters> component in components)
            {
                DeviceSettingControl control = DeviceSettingControlFactory.CreateDeviceSettingControl(component.Key, component.Value, pos);
                if (control != null)
                {
                    control.SetTabIndex(++tabIndex);
                    ComponentList.Add(control);
                    pos.Y += control.Size.Height;
                }
            }

            foreach (DeviceSettingControl control in ComponentList)
            {
                this.Controls.Add(control);
                control.SettingValueChangedCallback += this.SettingValueChanged;
            }
            Log.Information($"DeviceSettingsPanel:Setup Exit Component count :{components.Count}");
        }

        public void UserLoggedIn(bool isOnlineLogin)
        {
            //update user rights
            //get initial values
            foreach (DeviceSettingControl control in ComponentList)
            {
                control.UpdatePrivileges();
            }
            Log.Information($"DeviceSettingsPanel:UserLoggedIn");
        }


        public void DummyDeviceControls()
        {
            IsDummyDeviceConnected = true;
            //not in download mode anymore
            DeviceInDownloadMode = false;
            Log.Information($"DeviceSettingsPanel:DeviceConnected");
            RefreshButton.Enabled = false;
            //get initial values
            foreach (DeviceSettingControl control in ComponentList)
            {
                control.DeviceDisconnected();
            }
            DeviceSettingsPanelStatusLabel.Text = Properties.Resources.FUNCTIONALITY_DISABLED_USING_DUMMY_DEVICE;
        }

        public void DeviceConnected()
        {
            IsDummyDeviceConnected = false;
            //not in download mode anymore
            DeviceInDownloadMode = false;
            Log.Information($"DeviceSettingsPanel:DeviceConnected");
            RefreshButton.Enabled = true;
            //get initial values
            foreach (DeviceSettingControl control in ComponentList)
            {
                control.DeviceConnected();
            }

            RefreshValues();
        }

        public void DeviceConnectedInDownloadMode()
        {
            //treat this as disconnect for Device Settings panel
            Log.Information($"DeviceSettingsPanel:DeviceConnectedInDownloadMode");
            DeviceInDownloadMode = true;
            DeviceDisconnected();
        }

        private void RefreshValues()
        {
            Log.Information($"DeviceSettingsPanel:RefreshValues  Entry mDeviceInDownloadMode :{DeviceInDownloadMode} ");
            if (DeviceInDownloadMode == true || IsDummyDeviceConnected == true)
                return;
            
            DeviceSettingsPanelProgressBar.Maximum = ComponentList.Count;
            DeviceSettingsPanelProgressBar.Value = 0;
            DeviceSettingsPanelProgressBar.Visible = true;
            DeviceSettingsPanelStatusLabel.Text = Properties.Resources.DEVICE_SETTING_RETRIEVAL_IN_PROGRESS;
            //update status
            bool refreshStatus = true;
            //show progress
            foreach (DeviceSettingControl control in ComponentList)
            {
                bool status = control.RefreshValue();
                if (status == false)
                {
                    refreshStatus = false;
                }

                DeviceSettingsPanelProgressBar.Value++;
            }

            DeviceSettingsPanelStatusLabel.Text = ((refreshStatus == true) ? Properties.Resources.DEVICE_SETTING_RETRIEVAL_SUCCESS : Properties.Resources.DEVICE_SETTING_RETRIEVAL_ERROR);
            //hide it again
            DeviceSettingsPanelProgressBar.Visible = false;

            UpdateButton.Enabled = false;
            Log.Information($"DeviceSettingsPanel:RefreshValues  Exit mDeviceInDownloadMode :{DeviceInDownloadMode} ");
        }

        internal void Initialize()
        {
            throw new NotImplementedException();
        }

        public void DeviceDisconnected()
        {
            //clear
            Log.Information($"DeviceSettingsPanel:DeviceDisconnected");
            DeviceSettingsPanelStatusLabel.Text = "";
            UpdateButton.Enabled = false;
            RefreshButton.Enabled = false;
            foreach (DeviceSettingControl control in ComponentList)
            {
                control.DeviceDisconnected();
            }
        }

        public void DeviceRegistered(bool registrationStatus)
        {
            Log.Information($"DeviceSettingsPanel:DeviceRegistered mDeviceInDownloadMode :{DeviceInDownloadMode} registrationStatus:{registrationStatus} ");
            if (DeviceInDownloadMode == true)
                return;

            //update user rights
            //get initial values
            foreach (DeviceSettingControl control in ComponentList)
            {
                control.UpdatePrivileges();
            }
        }

        public void EnableControls()
        {
        }

        public void DisableControls()
        {
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshValues();
        }

        public void SettingValueChanged(object sender, EventArgs e)
        {
            Log.Information($"DeviceSettingsPanel:SettingValueChanged");
            int nChangeCount = 0;
            foreach (DeviceSettingControl control in ComponentList)
            {
                if (control.IsSettingValueChanged() == true)
                    nChangeCount++;
            }

            UpdateButton.Enabled = (nChangeCount > 0);
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            Log.Information($"DeviceSettingsPanel:mUpdateButton_Click Entry");
            if (MessageBox.Show(Properties.Resources.MESSAGE_BOX_CONFIRMATION_TEXT_STRING, Properties.Resources.MESSAGE_BOX_CONFIRMATION_TITLE_STRING, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int nChangeCount = 0;
                foreach (DeviceSettingControl control in ComponentList)
                {
                    if (control.IsSettingValueChanged() == true)
                        nChangeCount++;
                }

                if (nChangeCount > 0)
                {
                    //init progress bar
                    DeviceSettingsPanelProgressBar.Maximum = nChangeCount;
                    DeviceSettingsPanelProgressBar.Value = 0;
                    DeviceSettingsPanelProgressBar.Visible = true;

                    DeviceSettingsPanelStatusLabel.Text = Properties.Resources.DEVICE_PARAMETER_SETTING_IN_PROGRESS;

                    //update status
                    bool updateStatus = true;
                    //show progress
                    foreach (DeviceSettingControl control in ComponentList)
                    {
                        if (control.IsSettingValueChanged() == true)
                        {
                            bool status = control.SubmitValue();
                            if (status == false)
                            {
                                updateStatus = false;
                            }

                            DeviceSettingsPanelProgressBar.Value++;
                        }
                    }

                    DeviceSettingsPanelStatusLabel.Text = ((updateStatus == true) ? Properties.Resources.DEVICE_SETTING_SUCCESS : Properties.Resources.DEVICE_SETTING_FAILURE);
                    //hide it again
                    DeviceSettingsPanelProgressBar.Visible = false;

                    UpdateButton.Enabled = false;
                    Log.Information($"DeviceSettingsPanel:mUpdateButton_Click Exit");
                }
            }
        }
    }
}
