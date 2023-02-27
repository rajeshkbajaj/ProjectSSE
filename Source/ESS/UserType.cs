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

    public partial class UserTypeControl : UserControl
    {
        public UserTypeControl()
        {
            InitializeComponent();
        }

        public void StartTimer(int seconds)
        {
            Timer1.Interval = seconds * 1000;
            Timer1.Enabled = true;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Timer1.Enabled = false;

            // generate done event
            ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.SPLASH_SCREEN_TIMEOUT);
        }

        private void SplashScreenControl_Load(object sender, EventArgs e)
        {
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (InternalUserButton.Checked == true)
            {
                ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.USER_TYPE_SELECTED_MED);
            }
            else if (ExternalUserButton.Checked == true)
            {
                ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.USER_TYPE_SELECTED_NONMED);
            }
        }
    }
}
