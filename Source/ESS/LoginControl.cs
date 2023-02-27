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

    public partial class LoginControl : Form
    {
        protected LoginControl()
        {
            InitializeComponent();

            UserNameLabel.Text = Properties.Resources.USER_NAME_LABEL;
            PasswordLabel.Text = Properties.Resources.PASSWORD_LABEL;
            LoginButton.Text = Properties.Resources.LOGIN_BUTTON;
            CancelButton1.Text = Properties.Resources.CANCEL_BUTTON;
            MessageLabel.Text = Properties.Resources.AUTHENTICATION_REQUIRED;

            UserNameTextBox.Focus();
        }

        static public bool GetAccess()
        {
            Log.Information($"LoginControl:GetAccess Entry");
            MAccessGranted = null;
            MLoginControl = new LoginControl();
            MLoginControl.ShowDialog();

            MLoginControl = null;
            Log.Information($"LoginControl:GetAccess Exit MAccessGranted:{MAccessGranted}");
            return (MAccessGranted == true);
        }
        static private LoginControl MLoginControl;
        static private bool? MAccessGranted;

        /// <summary>
        /// InvalidCredentials()
        /// </summary>
        public void InvalidCredentials()
        {
            Log.Information($"LoginControl:InvalidCredentials");
            MessageLabel.Text = Properties.Resources.AUTHENTICATION_FAILED;
            MessageLabel.ForeColor = Color.Red;
            MessageLabel.Visible = true;
            LoginButton.Enabled = true;
            UserNameTextBox.Enabled = true;
            PasswordTextBox.Enabled = true;
        }

        /// <summary>
        /// mLoginButton_Click()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_Click(object sender, EventArgs e)
        {
            Log.Information($"LoginControl:mLoginButton_Click");
            if ((UserNameTextBox.Text.Length > 0) && (PasswordTextBox.Text.Length > 0))
            {
                AuthenticationService.Instance().AuthenticateUser(UserNameTextBox.Text, PasswordTextBox.Text, ESS_Main.Instance, AuthenticationResults);
                MessageLabel.Text = Properties.Resources.AUTHENTICATING;
                MessageLabel.Visible = true;
                LoginButton.Enabled = false;
                UserNameTextBox.Enabled = false;
                PasswordTextBox.Enabled = false;
             }
            else
            {
                MessageLabel.Text = Properties.Resources.MISSING_LOGIN_CREDENTIALS;
                MessageLabel.Visible = true;
            }
        }

        /// <summary>
        /// mCancelButton_Click()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            Log.Information($"LoginControl:mCancelButton_Click");
            if (LoginButton.Enabled == true)
            {
                // cancel dialog
                MAccessGranted = false;
                this.Close();
            }
            else
            {
                // cancel login attempt
                AuthenticationService.Instance().CancelAuthenticationRequest();
                MessageLabel.Visible = false;
                LoginButton.Enabled = true;
                UserNameTextBox.Enabled = true;
                PasswordTextBox.Enabled = true;
            }
        }

        private void AuthenticationResults(bool results,string reason)
        {
            UserNameTextBox.Enabled = false;
            PasswordTextBox.Enabled = false;
            Log.Information($"LoginControl:AuthenticationResults results:{results} reason:{reason}");
            if (results)
            {
                // valid user
                MAccessGranted = true;
                this.Close();
            }
            else
            {
                // invalid credentials
                MessageBox.Show(Properties.Resources.FUNCTION_RESTRICTED_MSG, Properties.Resources.ESS_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                MAccessGranted = false;
                InvalidCredentials();
                LoginButton.Enabled = true;
                UserNameTextBox.Enabled = true;
                PasswordTextBox.Enabled = true;
            }
        }

        private void UserNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                LoginButton_Click(sender, null);
            }
        }

        private void PasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                LoginButton_Click(sender, null);
            }
        }

    }
}
