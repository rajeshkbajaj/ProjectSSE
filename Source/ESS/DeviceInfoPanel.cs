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
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using Utilties;
    using System.Net;
    using Serilog;

    public partial class DeviceInfoPanel : UserControl
    {
        private enum DeviceState { DISCONNECTED, CONNECTING, CONNECTED, DISCONNECTING };
        private EssSettings Settings { get; set; }
        private DeviceState DeviceStateVal { get; set; }
        private string VentSerialNumber { get; set; }
        private readonly Font ConnectFont;
        private readonly Font DisconnectFont;
        private bool IsDummyDeviceConnected;

        public DeviceInfoPanel(EssSettings essSettings)
        {
            Settings = essSettings;
            InitializeComponent();
            DeviceInfoGroupBox.Text = Properties.Resources.DEVICE_INFO;

            ConnectFont = DeviceConnectButton.Font;
            DisconnectFont = new Font(ConnectFont.Name, 12, ConnectFont.Style,
                ConnectFont.Unit, ConnectFont.GdiCharSet,
                ConnectFont.GdiVerticalFont);

            DeviceStateVal = DeviceState.DISCONNECTED;
            ResetDeviceInfo();
        }

        private void SkipDeviceConnect_Click(object sender, EventArgs e)
        {
            Log.Information($"DeviceInfoPanel:mDeviceConnectButton_Click Entry ");
            
            ProcessStateChangeEvent(InterfaceDelegates.DeviceStateChangeEvent.DUMMY_CONNECT);
            DeviceConnectButton.Font = ConnectFont;
            DeviceConnectButton.Text = Properties.Resources.BACK_BUTTON;
            SkipDeviceConnect.Enabled = false;
            SkipDeviceConnect.Hide();
            Log.Information($"DeviceInfoPanel:mDeviceConnectButton_Click Exit ");
        }
		
        private void DeviceConnectButton_Click(object sender, EventArgs e)
        {
            Log.Information($"DeviceInfoPanel:mDeviceConnectButton_Click Entry ");
            if (DeviceConnectButton.Text == Properties.Resources.CONNECT_BUTTON)
            {
                SkipDeviceConnect.Enabled = false;
                SkipDeviceConnect.Hide();
                ProcessStateChangeEvent(InterfaceDelegates.DeviceStateChangeEvent.CONNECT);
            }
            else if (DeviceConnectButton.Text == Properties.Resources.CANCEL_BUTTON)
            {
                ProcessStateChangeEvent(InterfaceDelegates.DeviceStateChangeEvent.CANCEL);
            }
            else if(DeviceConnectButton.Text == Properties.Resources.BACK_BUTTON)
            {
                StateConnected(InterfaceDelegates.DeviceStateChangeEvent.DISCONNECT);
            }
            else
            {
                //show message box while disconnecting
                if (MessageBox.Show(ESS_Main.Instance, Properties.Resources.DEVICE_DISCONNECT_CONFIRM_MESSAGE,
                                      Properties.Resources.ESS_TITLE,
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ProcessStateChangeEvent(InterfaceDelegates.DeviceStateChangeEvent.DISCONNECT);
                }
            }
            Log.Information($"DeviceInfoPanel:mDeviceConnectButton_Click Exit ");
        }

        private void ProcessStateChangeEvent(InterfaceDelegates.DeviceStateChangeEvent evt)
        {
            Log.Information($"DeviceInfoPanel:ProcessStateChangeEvent Entry evt  :{evt} mDeviceState :{DeviceStateVal}");
            switch (DeviceStateVal)
            {
                case DeviceState.CONNECTED:
                    StateConnected(evt);
                    break;

                case DeviceState.CONNECTING:
                    StateConnecting(evt);
                    break;

                case DeviceState.DISCONNECTED:
                    StateDisconnected(evt);
                    break;

                case DeviceState.DISCONNECTING:
                    StateDisconnecting(evt);
                    break;
            }
            Log.Information($"DeviceInfoPanel:ProcessStateChangeEvent Exit evt  :{evt} mDeviceState :{DeviceStateVal}");
        }

        private void DeviceExistsEventCallback(bool actionSuccessful)
        {
            var eve = (actionSuccessful == true) ? ESS_Main.SystemEvent.DEVICE_REGISTRATION_SUCCESSFUL : ESS_Main.SystemEvent.DEVICE_REGISTRATION_FAILURE;
            ESS_Main.Instance.GenerateSystemEvent(eve);
        }

        private void StateConnected(InterfaceDelegates.DeviceStateChangeEvent evt)
        {
            Log.Information($"DeviceInfoPanel:StateConnected Entry evt  :{evt}");
            if ((evt == InterfaceDelegates.DeviceStateChangeEvent.DISCONNECT) || 
                (evt == InterfaceDelegates.DeviceStateChangeEvent.DEVICE_DISCONNECTED))
            {
                //show message box while disconnecting
                if (evt == InterfaceDelegates.DeviceStateChangeEvent.DEVICE_DISCONNECTED)
                {
                    MessageBox.Show(ESS_Main.Instance, Properties.Resources.DEVICE_NOT_REACHABLE_MESSAGE,
                                          Properties.Resources.ESS_TITLE,
                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                HandleDeviceDisconnect();                
                ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.DEVICE_DISCONNECTED);
            }
            else
            {
                // error
                Log.Error($"DeviceInfoPanel:StateConnected  evt  :{evt}");
                Debug.Assert(false);
            }
            Log.Information($"DeviceInfoPanel:StateConnected Exit evt  :{evt}");
        }

        public void HandleDeviceDisconnect()
        {
            Log.Information($"DeviceInfoPanel:HandleDeviceDisconnect Entry");
            DeviceStateVal = DeviceState.DISCONNECTED;

            //remove all connected oriented messages
            //add disconnected message
            // update status bar
            ESS_Main.Instance.GetStatusBar().RemoveDeviceRequestMessage(Properties.Resources.CONNECTING_STATUS_MSG);
            ESS_Main.Instance.GetStatusBar().RemoveDeviceRequestMessage(Properties.Resources.DEVICE_CONNECT_SUCCESS);
            ESS_Main.Instance.GetStatusBar().RemoveDeviceRequestMessage(Properties.Resources.DEVICE_CONNECT_FAILURE);
            ESS_Main.Instance.GetStatusBar().RemoveDeviceRequestMessage(Properties.Resources.CONNECTING_STATUS_MSG);
            ESS_Main.Instance.GetStatusBar().RemoveDeviceRequestMessage(Properties.Resources.DEVICE_CONNECTED_STATUS);
            
            DeviceConnectButton.Font = ConnectFont;
            DeviceConnectButton.Text = Properties.Resources.CONNECT_BUTTON;
            EnableControls();
            DevicePictureBox.Image = (Bitmap)Properties.Resources.PB980_faded;
            ResetDeviceInfo();
            SkipDeviceConnect.Enabled = true;
            SkipDeviceConnect.Show();
            Log.Information($"DeviceInfoPanel:HandleDeviceDisconnect Exit");
        }

        private void StateDisconnected(InterfaceDelegates.DeviceStateChangeEvent evt)
        {
            Log.Information($"DeviceInfoPanel:StateDisconnected Entry evt:{evt}");
            if (evt == InterfaceDelegates.DeviceStateChangeEvent.CONNECT)
            {
                IsDummyDeviceConnected = false;
                DeviceStateVal = DeviceState.CONNECTING;

                DeviceConnectButton.Font = DisconnectFont;
                DeviceConnectButton.Text = Properties.Resources.CONNECTING_BUTTON;
                DisableControls();

                ESS_Main.Instance.GetStatusBar().RemoveDeviceRequestMessage(Properties.Resources.DEVICE_DISCONNECTED_STATUS);
                // update status bar
                ESS_Main.Instance.GetStatusBar().AddDeviceRequestMessage(Properties.Resources.CONNECTING_STATUS_MSG,
                                                                     Properties.Resources.CONNECTING_STATUS_MSG);

                var portNumber = Settings.VentPortNumber;
                if (!IPAddress.TryParse(Settings.VentIpAddress, out IPAddress ipAddr))
                {
                    Log.Error($"DeviceInfoPanel:StateDisconnected Failed to parse IP Address {Settings.VentIpAddress}");
                }
                // send connection request
                DeviceManagement.Instance.ConnectToDevice(ipAddr, portNumber, ProcessStateChangeEvent, DeviceExistsEventCallback);
            }
            else if (evt == InterfaceDelegates.DeviceStateChangeEvent.DUMMY_CONNECT)
            {
                IsDummyDeviceConnected = true;
                ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.DUMMY_DEVICE_CONNECTED);
            }
            else
            {
                // error
                Log.Error($"DeviceInfoPanel:StateDisconnected  evt:{evt}");
                Debug.Assert(false);
            }
            Log.Information($"DeviceInfoPanel:StateDisconnected Exit evt:{evt}");
        }

        private void StateConnecting(InterfaceDelegates.DeviceStateChangeEvent evt)
        {
            Log.Information($"DeviceInfoPanel:StateConnecting Entry evt:{evt}");
            ESS_Main.Instance.GetStatusBar().RemoveDeviceRequestMessage(Properties.Resources.CONNECTING_STATUS_MSG);
            if (evt == InterfaceDelegates.DeviceStateChangeEvent.CANCEL)
            {
                DeviceConnectButton.Font = ConnectFont;
                DeviceConnectButton.Text = Properties.Resources.CONNECT_BUTTON;
                // cancel connection request

                // status change will only occur upon confirmation of cancel?
            }
            else if (evt == InterfaceDelegates.DeviceStateChangeEvent.DEVICE_CONNECTION_ESTABLISHED)
            {
                // update status bar
                ESS_Main.Instance.GetStatusBar().AddDeviceRequestMessage(Properties.Resources.DEVICE_CONNECT_SUCCESS,
                                                                     Properties.Resources.DEVICE_CONNECT_SUCCESS);
            }
            else if (evt == InterfaceDelegates.DeviceStateChangeEvent.DEVICE_SESSION_OPENED)
            {
                DeviceStateVal = DeviceState.CONNECTED;

                // Update this panel
                DevicePictureBox.Image = (Bitmap)Properties.Resources.PB980_connected;

                // register callback to update device info group box information
                DeviceManagement.Instance.GetDeviceInformation(GetDeviceInfo);

                if (DeviceManagement.Instance.IsDeviceInDownloadMode() == true)
                {
                    // send event back to ESS Main ... not sure if more will be needed
                    ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.DEVICE_CONNECTED_IN_DOWNLOAD_MODE);
                }
                else
                {
                    // send event back to ESS Main ... not sure if more will be needed
                    ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.DEVICE_CONNECTED);
                }

                DeviceConnectButton.SuspendLayout();
                DeviceConnectButton.Font = DisconnectFont;
                DeviceConnectButton.Text = Properties.Resources.DISCONNECT_BUTTON;
                EnableControls();
                DeviceConnectButton.ResumeLayout();

                // update status bar
                ESS_Main.Instance.GetStatusBar().AddDeviceRequestMessage(Properties.Resources.DEVICE_CONNECT_SUCCESS,
                                                                     Properties.Resources.DEVICE_CONNECT_SUCCESS);
            }
            else if (evt == InterfaceDelegates.DeviceStateChangeEvent.DEVICE_DISCONNECTED)
            {
                DeviceStateVal = DeviceState.DISCONNECTED;

                EnableControls();
                SkipDeviceConnect.Enabled = true;
                SkipDeviceConnect.Show();

                DeviceConnectButton.Font = ConnectFont;
                DeviceConnectButton.Text = Properties.Resources.CONNECT_BUTTON;
                // failed connection request - likely the device is not physically connected
                MessageBox.Show(ESS_Main.Instance, Properties.Resources.DEVICE_CONNECT_FAILURE_MESSAGE,
                                      Properties.Resources.ESS_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation );

                // update status bar
                ESS_Main.Instance.GetStatusBar().DeviceConnected(false, false);
                // update status bar
                ESS_Main.Instance.GetStatusBar().AddDeviceRequestMessage(Properties.Resources.DEVICE_CONNECT_FAILURE,
                                                                     Properties.Resources.DEVICE_CONNECT_FAILURE);
            }
            else
            {
                // error
                Log.Error($"DeviceInfoPanel:StateConnecting  evt:{evt}");
                Debug.Assert(false);
            }
            Log.Information($"DeviceInfoPanel:StateConnecting Exit evt:{evt}");
        }

        private void StateDisconnecting(InterfaceDelegates.DeviceStateChangeEvent evt)
        {
            // not sure if this is needed
        }

        private void GetDeviceInfo(string model, string serialNum, string softwareVersion, string deviceKeyType, string softwarePartnum)
        {
            Log.Information($"DeviceInfoPanel::GetDeviceInfo model:{model} serialNum:{serialNum} sw:{softwareVersion} key:{deviceKeyType} pn:{softwarePartnum} IsDummyDev:{IsDummyDeviceConnected}");
            DeviceInfoGroupBox.Visible = true;

            if (!string.IsNullOrEmpty(model))
            {
                ModelLabel.Text = Properties.Resources.DEV_INFO_MODEL + " " + model;
                ModelLabel.Visible = true;
            }

            if (!string.IsNullOrEmpty(serialNum))
            {
                SerialNumLabel.Text = Properties.Resources.DEV_INFO_SN + " " + serialNum;
                SerialNumLabel.Visible = true;
                VentSerialNumber = serialNum;
            }

            if (!string.IsNullOrEmpty(softwareVersion))
            {
                SoftwareVersionLabel.Text = Properties.Resources.DEV_INFO_SW_VER + " " + softwareVersion;
                SoftwareVersionLabel.Visible = true;
            }

            if (!string.IsNullOrEmpty(softwarePartnum))
            {
                SoftwarePartnumLabel.Text = Properties.Resources.DEV_INFO_SW_PARTNUM + " " + softwarePartnum;
                SoftwarePartnumLabel.Visible = true;
            }

            if ((AuthenticationService.Instance().IsCovidienUser() == true) && !string.IsNullOrEmpty(deviceKeyType))
            {
                DeviceKeyTypeLabel.Text = Properties.Resources.DEVICE_KEY_TYPE + " " + deviceKeyType;
                DeviceKeyTypeLabel.Visible = true;
            }
            else
            {
                DeviceKeyTypeLabel.Visible = false;
            }
        }

        private void ResetDeviceInfo()
        {
            Log.Information($"DeviceInfoPanel:ResetDeviceInfo ");
            DeviceInfoGroupBox.Visible = false;
            ModelLabel.Text = Properties.Resources.DEV_INFO_MODEL;
            SerialNumLabel.Text = Properties.Resources.DEV_INFO_SN;
            SoftwareVersionLabel.Text = Properties.Resources.DEV_INFO_SW_VER;
            DeviceKeyTypeLabel.Text = Properties.Resources.DEVICE_KEY_TYPE;
        }

        public void UserLoggedIn()
        {
        }

        public void UserLoginCancel()
        {
        }

        public void EnableControls()
        {
            DeviceConnectButton.Enabled = true;
            DeviceConnectButton.Focus();
        }

        public void DisableControls()
        {
            DeviceConnectButton.Enabled = false;
            SkipDeviceConnect.Enabled = false;
            SkipDeviceConnect.Hide();
            DeviceConnectButton.Refresh();
        }

        public string GetVentSerialNumber()
        {
            return VentSerialNumber;
        }
    }
}
