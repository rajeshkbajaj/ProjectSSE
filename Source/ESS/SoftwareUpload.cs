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
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.Diagnostics;
    using Serilog;

    public partial class SoftwareUpload : UserControl
    {
        public event EventHandler SoftwareUploadCancelClicked;

        private string PackageName;
        private bool IsLocalBorwsedPkg;
        private readonly List<FlashProgressControl> ProgressControl;
        private int NextYLocation;

        public SoftwareUpload(string packageName, string displayName)
        {
            Log.Information($"SoftwareUpload:SoftwareUpload Entry packageName:{packageName} displayName:{displayName}");
            InitializeComponent();
            PackageName = packageName;

            PackageLabel.Text = Properties.Resources.SOFTWARE_UPDATE_TO_STRING + displayName;
            ProgressControl = new List<FlashProgressControl>();
            NextYLocation = 0;

            this.Text = Properties.Resources.SW_UPLOAD_DIALOG_TITLE;
            StartButton.Text = Properties.Resources.SW_UPLOAD_START_BUTTON;
            CancelButton.Text = Properties.Resources.SW_UPLOAD_CANCEL_BUTTON;
            Log.Information($"SoftwareUpload:SoftwareUpload Exit packageName:{packageName} displayName:{displayName}");
        }

        public void ResetView(string packageName, string displayName, bool isLocalBorwsePkg)
        {
            Log.Information($"SoftwareUpload:ResetView Entry packageName:{packageName} displayName:{displayName} isLocalBorwsePkg:{isLocalBorwsePkg}");
            //remove all controls
            ProgressPanel.Controls.Clear();
            ProgressControl.Clear();

            PackageName = packageName;
            IsLocalBorwsedPkg = isLocalBorwsePkg;
            PackageLabel.Text = Properties.Resources.SOFTWARE_UPDATE_TO_STRING + displayName;

            NextYLocation = 0;
            this.Text = Properties.Resources.SW_UPLOAD_DIALOG_TITLE;
            StartButton.Text = Properties.Resources.SW_UPLOAD_START_BUTTON;
            CancelButton.Text = Properties.Resources.SW_UPLOAD_CANCEL_BUTTON;

            this.Visible = true;
            StartButton.Visible = true;
            CancelButton.Visible = true;
            MessageTextBox.Visible = false;
            Log.Information($"SoftwareUpload:ResetView Exit packageName:{packageName} displayName:{displayName} isLocalBorwsePkg:{isLocalBorwsePkg}");
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            Log.Information($"SoftwareUpload:mStartButton_Click");
            // register update and done callbacks
            // start flash process
            StartButton.Visible = false;
            CancelButton.Visible = false;
            MessageTextBox.Visible = true;
            
            //this.TopMost = true;
            SoftwarePackageManagement.Instance.ReprogramDevice(PackageName, IsLocalBorwsedPkg, UpdateStartedCallback, UpdateProgressCallback, UpdateDoneCallback, UpdateFailedCallback, OtherSoftWarepackageDownloadDoneCallback);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Log.Information($"SoftwareUpload:mCancelButton_Click");
            SoftwareUploadCancelClicked?.Invoke(sender, e);
        }

        private void UpdateStartedCallback(string cpu)
        {
            bool newControl = true;
            Log.Information($"SoftwareUpload:UpdateStartedCallback Entry  cpu:{cpu}");

            ProgressControl.ForEach(delegate(FlashProgressControl control)
            {
                if (cpu == control.Component())
                {
                    newControl = false;
                    control.Reset();
                }
            });

            if (newControl)
            {
                FlashProgressControl control = CreateNewControl(cpu);
                control.Reset();
            }
            Log.Information($"SoftwareUpload:UpdateStartedCallback Exit  cpu:{cpu}");
        }

        private void UpdateProgressCallback( string cpu, string component, string msg)
        {
            bool newControl = true;
            Log.Information($"SoftwareUpload:UpdateProgressCallback Entry  cpu:{cpu} component:{component} msg:{msg}");
            ProgressControl.ForEach(delegate(FlashProgressControl control)
            {
                if (cpu == control.Component())
                {
                    newControl = false;
                    control.Update(component, msg);
                }
            });

            if (newControl)
            {
                FlashProgressControl control = CreateNewControl(cpu);
                control.Update(component, msg);
            }
            Log.Information($"SoftwareUpload:UpdateProgressCallback Exit  cpu:{cpu} component:{component} msg:{msg}");
        }

        private FlashProgressControl CreateNewControl(string cpu)
        {
            Log.Information($"SoftwareUpload:CreateNewControl Entry cpu:{cpu}");
            FlashProgressControl newOne = new FlashProgressControl(cpu)
            {
                Location = new System.Drawing.Point(10, NextYLocation)
            };
            NextYLocation += newOne.Size.Height;
            this.ProgressPanel.Controls.Add(newOne);
            ProgressControl.Add(newOne);
            Log.Information($"SoftwareUpload:CreateNewControl Exit cpu:{cpu}");
            return newOne;
        }

        void UpdateDoneCallback()
        {
            Log.Information($"SoftwareUpload:UpdateDoneCallback Entry");
            ProgressControl.ForEach(delegate(FlashProgressControl control)
            {
                control.UpdateDone();
            });

            MessageBox.Show(Properties.Resources.SW_UPLOAD_DONE_MSGBOX_TEXT,
                        Properties.Resources.ESS_TITLE, 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

            // set device state to disconnected
            ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.DEVICE_DISCONNECTED);
            Log.Information($"SoftwareUpload:UpdateDoneCallback Exit");
        }

        void OtherSoftWarepackageDownloadDoneCallback(bool actionSuccessful)
        {
            Log.Information($"SoftwareUpload:OtherSoftWarepackageDownloadDoneCallback Entry  actionSuccessful:{actionSuccessful}");
            if (actionSuccessful)
            {
                string path = SoftwarePackageManagement.Instance.GetOtherSoftwareSavePath();
                path = path.Replace(@"\\", @"\");

                if (MessageBox.Show(Properties.Resources.SW_OTHERPACKAGE_DOWNLOAD_DONE_MSGBOX_TEXT + path + "\n \n Open Package directory ? ", Properties.Resources.ESS_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    StartProcess(path);
                }
            }
            else
            {
                MessageBox.Show(string.Format("{0}", Properties.Resources.SW_OTHERPACKAGE_DOWNLOAD_FAILED_MSGBOX_TEXT), Properties.Resources.ESS_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            SoftwareUploadCancelClicked?.Invoke(this, null);
            Log.Information($"SoftwareUpload:OtherSoftWarepackageDownloadDoneCallback Exit  actionSuccessful:{actionSuccessful}");

        }

        void UpdateFailedCallback(string errMsg)
        {
            Log.Information($"SoftwareUpload:UpdateFailedCallback Entry  errMsg:{errMsg}");
            MessageBox.Show(string.Format("{0}:{1}", Properties.Resources.SW_UPLOAD_FAILED_MSGBOX_TEXT, errMsg), Properties.Resources.ESS_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);

            if (errMsg.Equals(Properties.Resources.SOFTWARE_PACKAGE_INVALID) ||
                errMsg.Equals("Machine Check Failed"))
            {
                SoftwareUploadCancelClicked?.Invoke(this, null);
            }
            else
            {
                // set device state to disconnected
                ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.DEVICE_DISCONNECTED);
            }
            Log.Information($"SoftwareUpload:UpdateFailedCallback Exit errMsg:{errMsg}");
        }

        private void StartProcess(string path)
        {
            Log.Information($"SoftwareUpload:StartProcess Entry  path:{path}");
            ProcessStartInfo StartInformation = new ProcessStartInfo
            {
                FileName = path
            };
            Process.Start(StartInformation);
            Log.Information($"SoftwareUpload:StartProcess Exit  path:{path}");
            //The Independent process being started will be a window ,which can be closed by User. If not closed ,It is separate from ESS that means closing ESS will not affect that process. 
        }
     }
}
