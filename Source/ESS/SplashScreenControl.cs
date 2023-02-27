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
    using System.Threading.Tasks;
    using System.Drawing;
    using Serilog;

    public partial class SplashScreenControl : UserControl
    {
        public SplashScreenControl()
        {
            InitializeComponent();
            this.Label1.Text = "Version: "+ ESS_Main.Instance.GetESSVersion();
        }

        public void StartTimer(int seconds)
        {
            Timer1.Interval = seconds * 1000;
            Timer1.Enabled = true;
        }

        public void DisplayLabel(string data)
        {
            Log.Information($"SplashScreenControl:DisplayLabel Entry  data:{data}");
            AuthenticationWaitLabel.Enabled = true;
            AuthenticationWaitLabel.Text = data;
            AuthenticationWaitLabel.Show();
            Task.Run(() => Blink(AuthenticationWaitLabel));
            Log.Information($"SplashScreenControl:DisplayLabel Exit  data:{data}");
        }

        public void HideLabel()
        {
            Log.Information($"SplashScreenControl:HideLabel");
            AuthenticationWaitLabel.Hide();
            AuthenticationWaitLabel.Enabled = false;
            Timer1.Enabled = false;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Timer1.Enabled = false;

            // generate done event
            ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.SPLASH_SCREEN_TIMEOUT);
        }

        private async void Blink(Label label)
        {
            while (label.Enabled)
            {
                await Task.Delay(350);
                label.ForeColor = label.ForeColor == Color.Blue ? Color.Gray: Color.Blue;
            }
        }

        private void SplashScreenControl_Load(object sender, EventArgs e)
        {

        }

        private void VersionTextbox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
