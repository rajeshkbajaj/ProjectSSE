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
    using Covidien.CGRS.PcAgentInterfaceBusiness;
    using Serilog;
    using System;
    using System.Windows.Forms;
    using static Covidien.CGRS.ESS.ESS_Main;

    public partial class LoginSetPasscode : UserControl
    {
        public LoginSetPasscode()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            PasswordTextBox.Clear();
            PasswordTextBox.Enabled = true;
            EvaluateTextBoxes();
        }

        public void SetUserInfo(string user)
        {
            this.UserEmailValueLabel.Text = user;
        }

        private void EvaluateTextBoxes()
        {
            if (PasswordTextBox.TextLength > 0 || ReenterPasswordTextBox.TextLength >0)
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
                    LoginButton.Enabled = true;
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
                LoginButton.Enabled = false;
            }
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            EvaluateTextBoxes();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            Log.Information($"LoginSetPasscode:mLoginButton_Click");
            BusinessServicesBridge.Instance.SetOfflinePasscodeToServer(PasswordTextBox.Text);
            Instance.GenerateStateChangeEvent(StateChangeEvent.USER_AUTHENTICATED);

        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            Log.Information($"LoginSetPasscode:mClearButton_Click");
            PasswordTextBox.Clear();
            ReenterPasswordTextBox.Clear();
            WarningLabel.Hide();
            EvaluateTextBoxes();
        }
    }
}
