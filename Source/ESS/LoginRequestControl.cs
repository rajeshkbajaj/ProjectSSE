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
    using Serilog;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class LoginRequestControl : UserControl
    {
        private enum LoginDialogMode
        { 
            LOGIN_CLEAR, 
            LOGIN_CANCEL
        };

        private LoginDialogMode LoginMode = LoginDialogMode.LOGIN_CLEAR;
        private readonly UserControl ParentControl = null;
        private DeviceCountryControl DeviceCountryControlPanel;
        private bool IsAuthenticating;

        public LoginRequestControl()
        {
            InitializeComponent();
            Initialize();
        }
 
        public void Initialize()
        {
            UserNameTextBox.Clear();
            PasswordTextBox.Clear();

            UserNameTextBox.Enabled = true;
            PasswordTextBox.Enabled = true;

            LoginErrorLabel.Text = "";
            LoginErrorLabel.Visible = false;

            IsAuthenticating = false;
            EvaluateTextBoxes();

            DeviceCountryControlPanel = new DeviceCountryControl();
            DeviceCountryControlPanel.DeviceCountrySubmitDone += this.DeviceCountrySubmitted;
            DeviceCountryControlPanel.Visible = false;
            this.Controls.Add(DeviceCountryControlPanel);
        }

        public UserControl GetParentControl()
        {
            return ParentControl;
        }

        private void EvaluateTextBoxes()
        {
            if ((UserNameTextBox.TextLength > 0) || (PasswordTextBox.TextLength > 0))
            {
                //something is there to clear
                SetToClearMode();
            }
            else
            {
                //SetToCancelMode();
                ClearButton.Enabled = false;
            }

            if ((UserNameTextBox.TextLength > 0) && (PasswordTextBox.TextLength == 6) )
            {
                LoginButton.Enabled = true;
            }
            else
            {
                LoginButton.Enabled = false;
            }
        }

        private void UserNameTextBox_TextChanged(object sender, EventArgs e)
        {
            EvaluateTextBoxes();
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            EvaluateTextBoxes();
        }

        private void UserNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (LoginButton.Enabled == true)
                {
                    LoginButton_Click(sender, null);
                }
                else
                {
                    PasswordTextBox.Focus();
                }
            }
        }

        private void PasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (LoginButton.Enabled == true)
                {
                    LoginButton_Click(sender, null);
                }
                else
                {
                    UserNameTextBox.Focus();
                }
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            Log.Information($"LoginRequestControl:mLoginButton_Click Entry");
            Authenticating();
            AuthenticationService.Instance().AuthenticateUser(UserNameTextBox.Text, PasswordTextBox.Text, 
                                                                ESS_Main.Instance, AuthenticationResults);
            Log.Information($"LoginRequestControl:mLoginButton_Click Exit");
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            Log.Information($"LoginRequestControl:mClearButton_Click");
            if (LoginMode == LoginDialogMode.LOGIN_CANCEL)
            {
                if (IsAuthenticating)
                {
                    AuthenticatingDone();
                    AuthenticationService.Instance().CancelAuthenticationRequest();

                    //buttons have text - so clear option
                    SetToClearMode();
                }
                else
                {
                    ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.LOGIN_CANCELED);
                }
            }
            else if (LoginMode == LoginDialogMode.LOGIN_CLEAR)
            {
                //clear everything
                UserNameTextBox.Clear();
                PasswordTextBox.Clear();

                //cancel is an option
                //SetToCancelMode();
            }

            LoginErrorLabel.Text = "";
            LoginErrorLabel.Visible = false;

            EvaluateTextBoxes();
        }

        private void Authenticating()
        {
            Log.Information($"LoginRequestControl:Authenticating");
            IsAuthenticating = true;
            LoginButton.Enabled = false;
            ClearButton.Enabled = false;

            //change to cancel mode
            //SetToCancelMode();

            UserNameTextBox.Enabled = false;
            PasswordTextBox.Enabled = false;
            LoginErrorLabel.Text = "";
            LoginErrorLabel.Visible = false;
        }

        private void AuthenticatingDone()
        {
            Log.Information($"LoginRequestControl:AuthenticatingDone");
            IsAuthenticating = false;
            UserNameTextBox.Enabled = true;
            PasswordTextBox.Enabled = true;
        }

        private void AuthenticationResults(bool results,string reason)
        {
            // late response
            Log.Information($"LoginRequestControl:AuthenticationResults Entry results:{results} reason:{reason}");
            if (IsAuthenticating == false)
            {
                return;
            }

            AuthenticatingDone();

            // valid user
            if (results)
            {
                Initialize();
                if (AuthenticationService.Instance().IsCovidienUser())
                {
                    //show country selection control and then raise user authenticated event
                    ShowControls(false);
                    DeviceCountryControlPanel.SetCountryCode(AuthenticationService.Instance().GetUserCountryCode());
                    DeviceCountryControlPanel.Visible = true;

                }
                else
                {
                    ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.USER_AUTHENTICATED);
                }
            }
            else
            {
                // invalid credentials
                InvalidCredentials(reason);
                ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.USER_AUTHENTICATION_FAILED);
            }
            Log.Information($"LoginRequestControl:AuthenticationResults Exit results:{results} reason:{reason}");
        }

        private void ShowControls(bool visible)
        {
            Log.Information($"LoginRequestControl:ShowControls");
            foreach (Control ctl in this.Controls)
            {
                ctl.Visible = visible;
            }
        }


        private void InvalidCredentials(string reason)
        {
            Log.Information($"LoginRequestControl:InvalidCredentials");
            LoginErrorLabel.Text = string.IsNullOrEmpty(reason) ? Properties.Resources.AUTHENTICATION_FAILED : reason;
            LoginErrorLabel.ForeColor = Color.Red;
            LoginErrorLabel.Visible = true;
            EvaluateTextBoxes();
        }

        private void SetToClearMode()
        {
            Log.Information($"LoginRequestControl:SetToClearMode");
            LoginMode = LoginDialogMode.LOGIN_CLEAR;
            ClearButton.Text = Properties.Resources.CLEAR_BUTTON;
            ClearButton.Enabled = true;
        }

        private void DeviceCountrySubmitted(object sender, EventArgs e)
        {
            Log.Information($"LoginRequestControl:DeviceCountrySubmitted");
            //here the country code is submitted, which means a valid user log in was successful - change state to connect
            ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.USER_AUTHENTICATED);
        }
    }
}
