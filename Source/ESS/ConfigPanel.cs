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

    public partial class ConfigPanel : UserControl
    {
        private bool IsOnlineLogin;

        public ConfigPanel()
        {
            IsOnlineLogin = false;
            InitializeComponent();

            //start clear and disabled
            ClearControls();
            HidePasscodeControls();
        }

        public void UserLoggedIn(bool isOnlineLogin)
        {
            IsOnlineLogin = isOnlineLogin;
            UserEmailInfoLabel.Text = BusinessServicesBridge.Instance.GetLoggedInUserName();

            // This has to be called at the end, after updating mIsOnlineLogin, mUserEmailInfoLabel
            EnableControls();
        }

        public void DummyDeviceControls()
        {
            // Nothing to be performed here..
        }

        public void DeviceConnected()
        {
            EnableControls();
            NoRadioButton.Checked = true;
        }

        public void DeviceDisconnected()
        {
            //clear
            NoRadioButton.Checked = true;
            DisableControls();
        }

        public void Initialize()
        {
            PasswordTextBox.Clear();
            PasswordTextBox.Enabled = true;
            EvaluateTextBoxes();
        }

        public void DisableControls()
        {
            StatusLabel.Text = "";
            TitleLabel.Enabled = false;
            YesRadioButton.Enabled = false;
            NoRadioButton.Enabled = false;
            HidePasscodeControls();
        }

        public void EnableControls()
        {
            if (IsOnlineLogin)
            {
                StatusLabel.Text = "";
                TitleLabel.Enabled = true;
                YesRadioButton.Enabled = true;
                NoRadioButton.Enabled = true;
                if (YesRadioButton.Checked == true)
                {
                    ShowPasscodeControls();
                }
            }
            else
            {
                StatusLabel.ForeColor = System.Drawing.Color.Red;
                StatusLabel.Text = Properties.Resources.SETTINGS_STATUS_ALLOWED_IN_ONLINE;
                HidePasscodeControls();
                TitleLabel.Enabled = false;
                YesRadioButton.Enabled = false;
                NoRadioButton.Enabled = false;
                UserEmailLabel.Enabled = false;
                UserEmailInfoLabel.Enabled = false;
            }
        }

        private void EvaluateTextBoxes()
        {
            if (PasswordTextBox.TextLength > 0 || ReenterPasswordTextBox.TextLength > 0)
            {
                ClearButton.Enabled = true;
            }
            else
            {
                ClearButton.Enabled = false;
            }

            if (PasswordTextBox.TextLength == 6 &&
                ReenterPasswordTextBox.TextLength == 6)
            {
                if (ReenterPasswordTextBox.Text == PasswordTextBox.Text)
                {
                    SubmitButton.Enabled = true;
                    WarningLabel.Hide();
                }
                else
                {
                    WarningLabel.Text = Properties.Resources.PASSCODE_NOT_MATCH;
                    WarningLabel.Show();
                }
            }
            else
            {
                SubmitButton.Enabled = false;
            }
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            EvaluateTextBoxes();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            Log.Information($"ConfigPanel:mLoginButton_Click");
            if (BusinessServicesBridge.Instance.SetOfflinePasscodeToServer(PasswordTextBox.Text))
            {
                StatusLabel.Text = Properties.Resources.SETTINGS_OFFLINE_CHANGE_PASSWORD_SUCCESS;
            }
            else
            {
                StatusLabel.Text = Properties.Resources.SETTINGS_OFFLINE_CHANGE_PASSWORD_FAILURE;
            }

            this.NoRadioButton.Checked = true;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        private void Yes_CheckedChanged(object sender, EventArgs e)
        {
            ClearControls();
            ShowPasscodeControls();
        }

        private void No_CheckedChanged(object sender, EventArgs e)
        {
            ClearControls();
            HidePasscodeControls();
        }

        private void ShowPasscodeControls()
        {
            PasswordTextBox.Show();
            PasswordLabel.Show();
            ClearButton.Show();
            SubmitButton.Show();
            PasscodeInfoLabel.Show();
            ReenterPasswordTextBox.Show();
            ReenterPasswordInfoLabel.Show();
            WarningLabel.Show();
        }

        private void HidePasscodeControls()
        {
            ConfigPanelStatusLabel.Hide();
            PasswordTextBox.Hide();
            PasswordLabel.Hide();
            ClearButton.Hide();
            SubmitButton.Hide();
            PasscodeInfoLabel.Hide();
            ReenterPasswordTextBox.Hide();
            ReenterPasswordInfoLabel.Hide();
            WarningLabel.Hide();
        }

        private void ClearControls()
        {
            ReenterPasswordTextBox.Clear();
            PasswordTextBox.Clear();
            WarningLabel.Text = "";
            WarningLabel.Hide();
            EvaluateTextBoxes();
        }
    }
}
