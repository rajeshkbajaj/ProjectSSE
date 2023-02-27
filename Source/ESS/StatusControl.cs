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
    using System.Windows.Forms;

    public partial class StatusControl : UserControl
    {
        public StatusControl()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
        }

        public void SetStatusText(string status)
        {
            Log.Information($"StatusControl:HideLabel status:{status}");
            SwStatusLabel.Text = status;
        }

        public void HideStatus()
        {
            Log.Information($"StatusControl:hideStatus");
            SwStatusLabel.Visible = false;
            SwStatusLabelLabel.Visible = false;
        }

        public void ShowStatus()
        {
            Log.Information($"StatusControl:showStatus");
            SwStatusLabel.Visible = true;
            SwStatusLabelLabel.Visible = true;
        }

        public void EnableControls()
        {
            Log.Information($"StatusControl:EnableControls");
            SwStatusLabel.Enabled = true;
            SwStatusLabelLabel.Enabled = true;
        }

        public void DisableControls()
        {
            Log.Information($"StatusControl:DisableControls");
            SwStatusLabel.Enabled = false;
            SwStatusLabelLabel.Enabled = false;
        }
    }
}
