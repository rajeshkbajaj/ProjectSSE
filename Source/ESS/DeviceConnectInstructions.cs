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

    public partial class DeviceConnectInstructions : UserControl
    {
        private int Frame;

        public DeviceConnectInstructions()
        {
            InitializeComponent();
            VersionLabel.Text = Properties.Resources.VERSION_LABEL + ": " + ESS_Main.Instance.GetESSVersion();
            Frame = 5;
        }

        public void ShowPanel()
        {
            this.Visible = true;
            this.Enabled = true;
            Timer1.Start();
        }

        public void HidePanel()
        {
            this.Visible = false;
            this.Enabled = false;
            Timer1.Stop();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            
            if (1 == Frame)
            {
                Timer1.Interval = 500;
                PictureBox1.BackgroundImage = (Bitmap)Properties.Resources._980_connectivity_2;
                Frame = 2;
            }
            else if (2 == Frame)
            {
                Timer1.Interval = 500;
                PictureBox1.BackgroundImage = (Bitmap)Properties.Resources._980_connectivity_3;
                Frame = 3;
            }
            else if (3 == Frame)
            {
                Timer1.Interval = 500;
                PictureBox1.BackgroundImage = (Bitmap)Properties.Resources._980_connectivity_4;
                Frame = 4;
            }
            else if (4 == Frame)
            {
                Timer1.Interval = 2000;
                PictureBox1.BackgroundImage = (Bitmap)Properties.Resources._980_connectivity_5;
                Frame = 5;
            }
            else if (5 == Frame)
            {
                Timer1.Interval = 500;
                PictureBox1.BackgroundImage = (Bitmap)Properties.Resources._980_connectivity_1;
                Frame = 1;
            }
        }
     }
}
