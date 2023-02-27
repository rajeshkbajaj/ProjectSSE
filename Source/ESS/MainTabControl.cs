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
    using System.Windows.Forms;
    using Covidien.CGRS.PcAgentInterfaceBusiness;
    using Serilog;
    using Utilties;

    public partial class MainTabControl : UserControl
    {
        private readonly LogViewerControl LogViewerControlPanel;
        private readonly SoftwareUpdateControl SoftwareUpdatePanel;
        private readonly ConfigPanel ConfigSettingsPanel;
        private readonly DeviceSettingsPanel DeviceSettingsPanel;
        private readonly StatusControl StatusControlLabel;
        private readonly EssSettings Settings;

        public MainTabControl(EssSettings essSettings)
        {
            Settings = essSettings;
            InitializeComponent();
            LogViewerControlPanel = new LogViewerControl();
            SoftwareUpdatePanel = new SoftwareUpdateControl(Settings);
            StatusControlLabel = new StatusControl();
            ConfigSettingsPanel = new ConfigPanel();
            DeviceSettingsPanel = new DeviceSettingsPanel();
        }

        private void MainTabControl_Load(object sender, EventArgs e)
        {
            this.TabPage1.Controls.Add(LogViewerControlPanel);
            this.TabPage2.Controls.Add(SoftwareUpdatePanel);
            this.TabPage3.Controls.Add(DeviceSettingsPanel);
            this.TabPage4.Controls.Add(ConfigSettingsPanel);

            VersionLabel.Text = Properties.Resources.VERSION_LABEL + ": " + ESS_Main.Instance.GetESSVersion();
        }

        public void DeviceDisconnected()
        {
            Log.Information($"MainTabControl:DeviceDisconnected");
            LogViewerControlPanel.DeviceDisconnected();
            SoftwareUpdatePanel.DeviceDisconnected();
            DeviceSettingsPanel.DeviceDisconnected();
            ConfigSettingsPanel.DeviceDisconnected();
        }
        
        public void DeviceNotConnectedShowServerInfo()
        {
            Log.Information($"MainTabControl:DeviceConnected");
            BusinessServicesBridge.Instance.Device.IsDummyDeviceConnected = true;
            StatusControlLabel.Initialize();
            StatusControlLabel.Visible = true;
            LogViewerControlPanel.Visible = true;
            SoftwareUpdatePanel.Enabled = true;
            ConfigSettingsPanel.Enabled = true;

            LogViewerControlPanel.DummyDeviceControls();
            SoftwareUpdatePanel.DummyDeviceControls();
            DeviceSettingsPanel.DummyDeviceControls();
            ConfigSettingsPanel.DummyDeviceControls();
        }

        public void DeviceConnected()
        {
            Log.Information($"MainTabControl:DeviceConnected");
            BusinessServicesBridge.Instance.Device.IsDummyDeviceConnected = false;
            StatusControlLabel.Initialize();
            StatusControlLabel.Visible = true;
            LogViewerControlPanel.Visible = true;
            SoftwareUpdatePanel.DeviceConnected();
            ConfigSettingsPanel.DeviceConnected();
            SoftwareUpdatePanel.Enabled = BusinessServicesBridge.Instance.IsUserLoggedIn();
            DeviceSettingsPanel.DeviceConnected();
            LogViewerControlPanel.DeviceConnected();
        }

        public void ApplicationClose()
        {
            Log.Information($"MainTabControl:ApplicationClose");
            SoftwareUpdatePanel.ApplicationClose();
        }

        public void DeviceConnectedInDownloadMode()
        {
            Log.Information($"MainTabControl:DeviceConnectedInDownloadMode Entry");
            StatusControlLabel.Initialize();
            StatusControlLabel.Visible = true;
            LogViewerControlPanel.DeviceConnectedInDownloadMode();
            SoftwareUpdatePanel.DeviceConnectedInDownloadMode();
            SoftwareUpdatePanel.Enabled = BusinessServicesBridge.Instance.IsUserLoggedIn();
            DeviceSettingsPanel.DeviceConnectedInDownloadMode();
            Log.Information($"MainTabControl:DeviceConnectedInDownloadMode Exit");
        }

        public void DeviceRegistered(bool registrationStatus)
        {
            Log.Information($"MainTabControl:DeviceRegistered");
            LogViewerControlPanel.DeviceRegistered(registrationStatus);
            SoftwareUpdatePanel.DeviceRegistered(registrationStatus);
            DeviceSettingsPanel.DeviceRegistered(registrationStatus);
        }

        public void AgentDownloadComplete()
        {
            Log.Information($"MainTabControl:AgentDownloadComplete Entry");
            SoftwareUpdatePanel.AgentDownloadComplete();
            Log.Information($"MainTabControl:AgentDownloadComplete Exit");
        }

        public void ShowPanel()
        {
            Log.Information($"MainTabControl:ShowPanel Entry");
            this.Visible = true;
            LogViewerControlPanel.Initialize();
            ///TODO
            ConfigSettingsPanel.Initialize();
            MainTabControl1.SelectTab(0);
            MainTabControl1.Focus();

            ResumeLayout();
            Log.Information($"MainTabControl:ShowPanel Exit");
        }

        public void ShowSoftwareDownloadPanel()
        {
            Log.Information($"MainTabControl:ShowSoftwareDownloadPanel Entry");
            this.Visible = true;
            MainTabControl1.SelectTab(1);
            MainTabControl1.Focus();
            Log.Information($"MainTabControl:ShowSoftwareDownloadPanel Exit");
        }

        public void ShowConfigPanel()
        {
            this.Visible = true;
            MainTabControl1.SelectTab(3);
            MainTabControl1.Focus();
        }

        public void HidePanel()
        {
            Log.Information($"MainTabControl:HidePanel");
            this.Visible = false;
        }

        public void EnableControls()
        {
            Log.Information($"MainTabControl:EnableControls");
            LogViewerControlPanel.EnableControls();
            SoftwareUpdatePanel.EnableControls();
            ConfigSettingsPanel.EnableControls();
            StatusControlLabel.EnableControls();
        }

        public void DisableControls()
        {
            Log.Information($"MainTabControl:DisableControls");
            LogViewerControlPanel.DisableControls();
            SoftwareUpdatePanel.DisableControls();
            ConfigSettingsPanel.DisableControls();
            StatusControlLabel.DisableControls();
        }

        public void SetSwStatus(string aStatus)
        {
            Log.Information($"MainTabControl:setSwStatus");
            StatusControlLabel.SetStatusText(aStatus);
        }

        private void TabPage1_Enter(object sender, EventArgs e)
        {
        }

        private void TabPage2_Enter(object sender, EventArgs e)
        {
        }

        private void TabPage3_Enter(object sender, EventArgs e)
        {
        }

        private void TabPage3_Leave(object sender, EventArgs e)
        {
        }

        private void TabPage4_Enter(object sender, EventArgs e)
        {
        }

        private void TabPage4_Leave(object sender, EventArgs e)
        {
        }

        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        public void UserLoggedIn(bool isOnlineLogin)
        {
            Log.Information($"MainTabControl:UserLoggedIn Entry");
            SoftwareUpdatePanel.Enabled = true;
            SoftwareUpdatePanel.UserLoggedIn(isOnlineLogin);
            SoftwareUpdatePanel.InitializePackageList();

            LogViewerControlPanel.UserLoggedIn(isOnlineLogin);
            DeviceSettingsPanel.UserLoggedIn(isOnlineLogin);
            ConfigSettingsPanel.UserLoggedIn(isOnlineLogin);
            ConfigSettingsPanel.Visible = true;
            Log.Information($"MainTabControl:UserLoggedIn Exit");
        }

        public void UserLogInCancel()
        {
        }

        public void InitializeStatusControl()
        {
            StatusControlLabel.Initialize();
        }

    }
}