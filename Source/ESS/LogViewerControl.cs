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
    using System.Linq;
    using System.Windows.Forms;
    using System.IO;
    using Serilog;
    using Covidien.CGRS.PcAgentInterfaceBusiness;

    public partial class LogViewerControl : UserControl
    {
        private List<string> LogNames;
        private bool LogsRetrievalOngoing = false;
        private bool LogsRetrievalComplete = false;
        private bool IsDeviceRegistered = false;
        private bool IsUserLoggedIn = false;
        private LogViewer LogViewer1;
        private DateTime LogsRetrieved;
        private string LogsSaveDir;
        private bool IsDummyDeviceConnected = false;

        public LogViewerControl()
        {
            InitializeComponent();
            LogViewer1 = null;
            LogsSaveDir = "c:\\";
            LogNames = new List<string>();
            DeviceDisconnected();
        }

        public void Initialize()
        {
            LogsListBox.Items.Clear();

            LogsLoadProgressBar.Value = 0;
            LogsLoadProgressBar.Style = ProgressBarStyle.Marquee;
            LogsStatusLabel.Text = BusinessServicesBridge.Instance.IsLogUploadRunning? 
                   Properties.Resources.DEVICE_LOGS_UPLOAD_INPROGRESS:Properties.Resources.REQUESTING_DEVICE_LOGS_STATUS_MSG;
            
            DisableControls();

            ESS_Main.Instance.GetStatusBar().AddDeviceRequestMessage(Properties.Resources.REQUESTING_DEVICE_LOGS_STATUS_MSG,
                                                                 Properties.Resources.REQUESTING_DEVICE_LOGS_STATUS_MSG);

            GetDeviceLogs();
        }

        public void EnableControls()
        {
            if (IsUserLoggedIn == false)
            {
                Log.Warning($"LogViewerControl:EnableControls All controls are disabled without user login");
                return;
            }

            Log.Information($"LogViewerControl:EnableControls");
            ViewLogLabel.Enabled = true;
            ClearDeviceLogsButton.Enabled = true;

            if (AuthenticationService.Instance().GetLogUploadCredentials() == AuthenticationService.CREDENTIAL_STATUS.APPROVED)
            {
                UploadAllLogsButton.Enabled = true;
            }

            //save and view (print) functions are restricted - only enabled when ESM is enabled
            if (AuthenticationService.Instance().GetLogAccessCredentials() == AuthenticationService.CREDENTIAL_STATUS.APPROVED)
            {
                SaveLogsToFileButton.Enabled = true;
                ViewLogButton.Enabled = true;
            }
            DeviceInfoButton.Enabled = true;
        }

        public void DummyDeviceControls()
        {
            Log.Information($"LogViewerControl:DisableControls");
            IsDummyDeviceConnected = true;
            UploadAllLogsButton.Enabled = false;
            SaveLogsToFileButton.Enabled = false;
            ViewLogButton.Enabled = false;
            ClearDeviceLogsButton.Enabled = false;
            ViewLogLabel.Enabled = false;
            DeviceInfoButton.Enabled = false;
            LogsStatusLabel.Visible = true;
            LogsStatusLabel.Text = Properties.Resources.FUNCTIONALITY_DISABLED_USING_DUMMY_DEVICE;
        }

        public void DisableControls()
        {
            Log.Information($"LogViewerControl:DisableControls");
            UploadAllLogsButton.Enabled = false;
            SaveLogsToFileButton.Enabled = false;
            ViewLogButton.Enabled = false;
            ClearDeviceLogsButton.Enabled = false;
            ViewLogLabel.Enabled = false;
        }

        public void DeviceConnected()
        {
            Log.Information($"LogViewerControl:DeviceConnected");
            IsDummyDeviceConnected = false;
            DeviceInfoButton.Enabled = true;
            LogsStatusLabel.Text = "";
            LogsStatusLabel.Visible = true;
            LogsLoadProgressBar.Visible = false;
        }

        public void DeviceConnectedInDownloadMode()
        {
            Log.Information($"LogViewerControl:DeviceConnectedInDownloadMode");
            LogsStatusLabel.Text = "";
            DeviceInfoButton.Enabled = true;
            DisableControls();
        }

        public void DeviceDisconnected()
        {
            Log.Information($"LogViewerControl:DeviceDisconnected");
            LogNames.Clear();
            LogsListBox.Items.Clear();

            DisableControls();
            DeviceInfoButton.Enabled = false;
            LogsRetrievalOngoing = false;

            LogsStatusLabel.Visible = true;
            LogsLoadProgressBar.Visible = false;

            IsDeviceRegistered = false;
            LogsRetrievalOngoing = false;
            LogsRetrievalComplete = false;
            LogsStatusLabel.Text = "";
        }

        public void UserLoggedIn(bool isOnlineLogin)
        {
            Log.Information($"LogViewerControl:UserLoggedIn");
            IsUserLoggedIn = true;
        }

        public void DeviceRegistered(bool registrationStatus)
        {
            IsDeviceRegistered = registrationStatus;
            Log.Information($"LogViewerControl:DeviceRegistered {IsDeviceRegistered}");
        }

        private void GetDeviceLogs()
        {
            Log.Information($"LogViewerControl:GetDeviceLogs Entry");
            //initiate log retrieval
            DeviceLogManagement.Instance.GetDeviceLogs(DeviceLogsListCallback, DeviceLogLoadedCallback, AllDeviceLogsLoadedCallback);
            LogsRetrievalOngoing = true;

            LogsStatusLabel.Visible = true;

            //this will be marquee
            LogsLoadProgressBar.Visible = true;
            Log.Information($"LogViewerControl:GetDeviceLogs Exit");
        }

        private void DeviceLogsListCallback(bool actionSuccessful, List<string> logNames)
        {
            if (IsDummyDeviceConnected)
                return;

            Log.Information($"LogViewerControl:DeviceLogsListCallback Entry actionSuccessful:{actionSuccessful}");
            DisableControls();
            ESS_Main.Instance.GetStatusBar().RemoveDeviceRequestMessage(Properties.Resources.REQUESTING_DEVICE_LOGS_STATUS_MSG);
            
            if (actionSuccessful == false)
            {
                ESS_Main.Instance.GetStatusBar().AddDeviceRequestMessage(Properties.Resources.DEVICE_LOG_RETRIEVAL_FAILURE, Properties.Resources.DEVICE_LOG_RETRIEVAL_FAILURE);
                LogsStatusLabel.Visible = true;
                LogsStatusLabel.Text = Properties.Resources.DEVICE_LOG_RETRIEVAL_FAILURE;
                return;
            } 
            
            // add names to dropdownlist once all logs have been downloaded w/o error
            LogNames = logNames;
            LogsLoadProgressBar.Style = ProgressBarStyle.Marquee;
            LogsLoadProgressBar.Maximum = LogNames.Count;
            LogsLoadProgressBar.Visible = true;
            Log.Information($"LogViewerControl:DeviceLogsListCallback Exit actionSuccessful:{actionSuccessful}");
        }

        private void DeviceLogLoadedCallback(bool actionSuccessful, string logName)
        {
            //if there is no retrieval ongoing, do not consider this call
            Log.Information($"LogViewerControl:DeviceLogLoadedCallback Entry actionSuccessful:{actionSuccessful} logName:{logName}");
            if (LogsRetrievalOngoing == false || IsDummyDeviceConnected)
                return;

            if (actionSuccessful == true)
            {
                LogsStatusLabel.Text = Properties.Resources.REQUESTING_DEVICE_LOGS_STATUS_MSG;
                LogsLoadProgressBar.Style = ProgressBarStyle.Continuous;
                if (LogsLoadProgressBar.Value < LogsLoadProgressBar.Maximum) 
                { LogsLoadProgressBar.Value++; }
            }
            Log.Information($"LogViewerControl:DeviceLogLoadedCallback Exit actionSuccessful:{actionSuccessful} logName:{logName}");
        }

        private void AllDeviceLogsLoadedCallback(bool actionSuccessful)
        {
            Log.Information($"LogViewerControl:AllDeviceLogsLoadedCallback Entry actionSuccessful:{actionSuccessful}");
            //if there is no retrieval ongoing, do not consider this call
            if (LogsRetrievalOngoing == false || IsDummyDeviceConnected)
                return;

            //all logs downloaded callback - the operation must be complete (success/ failure)
            LogsRetrievalOngoing = false;

            LogsLoadProgressBar.Visible = false;
            ESS_Main.Instance.GetStatusBar().RemoveDeviceRequestMessage(Properties.Resources.REQUESTING_DEVICE_LOGS_STATUS_MSG);

            if (actionSuccessful == false)
            {
                ESS_Main.Instance.GetStatusBar().AddDeviceRequestMessage(Properties.Resources.DEVICE_LOG_RETRIEVAL_FAILURE, Properties.Resources.DEVICE_LOG_RETRIEVAL_FAILURE);
                LogsStatusLabel.Text = Properties.Resources.DEVICE_LOG_RETRIEVAL_FAILURE;
                DisableControls();
            }
           else
            {
                //show only device logs in the combo box
                List<string> deviceLogNames = new List<string>();
                DeviceLogManagement.Instance.GetDeviceLogNames(deviceLogNames);
                string[] deviceLogsNamesForDisplay = deviceLogNames.ToArray();

                LogNames.ForEach(delegate(string name)
                {
                    if((deviceLogsNamesForDisplay != null) && 
                        (deviceLogsNamesForDisplay.Length > 0) && 
                        deviceLogsNamesForDisplay.Contains(name, StringComparer.OrdinalIgnoreCase))
                    {
                        //Checking is Log name is one of device log names ,then add to Listbox Items,to avoid Coredumps and extra logs on user GUI
                        LogsListBox.Items.Add(name + Properties.Resources.LOG_NAME_POSTPEND);
                    }                    
                });

                //this event could be used to enable the buttons, viz. view log, save all, upload all, clear all buttons
                if (LogsListBox.Items.Count > 0)
                {
                    LogsListBox.SelectedIndex = 0;
                }

                LogsRetrieved = DateTime.Now;

                LogsStatusLabel.Text = Properties.Resources.DEVICE_LOG_RETRIEVAL_SUCCESS;

                EnableControls();

                LogsRetrievalComplete = true;

                UploadAllLogsToServer();// Auto Uploading all the logs 
                Log.Information($"LogViewerControl:AllDeviceLogsLoadedCallback Exit actionSuccessful:{actionSuccessful}");
            }
        }

        private void ClearDeviceLogsCallback(bool actionSuccessful, string result)
        {
            if (IsDummyDeviceConnected)
                return;

            Log.Information($"LogViewerControl:ClearDeviceLogsCallback Entry actionSuccessful:{actionSuccessful} result:{result}");
            if (actionSuccessful == true)
            {
                LogsStatusLabel.Text = "Clear Logs successful";
            }
            else
            {
                LogsStatusLabel.Text = "Clear Logs failed." + string.Format("{0}", result);
            }
            Log.Information($"LogViewerControl:ClearDeviceLogsCallback Exit actionSuccessful:{actionSuccessful} result:{result}");
        }

        private void ClearDeviceLogsButton_Click(object sender, EventArgs e)
        {
            Log.Information($"LogViewerControl:mClearDeviceLogsButton_Click Entry");
            if (AuthenticationService.Instance().GetLogAccessCredentials() == AuthenticationService.CREDENTIAL_STATUS.APPROVED)
            {
                if (MessageBox.Show(Properties.Resources.CLEAR_LOGS_CONFIRMATION_MSG, Properties.Resources.ESS_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                {
                    // call bridge function to clear the log
                    DeviceLogManagement.Instance.ClearDeviceLogs(ClearDeviceLogsCallback);
                }
            }
            else
                MessageBox.Show(Properties.Resources.FUNCTION_RESTRICTED_MSG, Properties.Resources.ESS_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            Log.Information($"LogViewerControl:mClearDeviceLogsButton_Click Exit");
        }

        private void DeviceLogUpLoadedToServerCallback(bool actionSuccessful, string logName)
        {
            if (IsDummyDeviceConnected)
                return;

            Log.Information($"LogViewerControl:DeviceLogUpLoadedToServerCallback Entry actionSuccessful:{actionSuccessful} logName:{logName}");
            if (actionSuccessful == true)
            {
                LogsStatusLabel.Text = Properties.Resources.DEVICE_LOGS_UPLOAD_INPROGRESS;
                LogsLoadProgressBar.Style = ProgressBarStyle.Continuous;
                if (LogsLoadProgressBar.Value < LogsLoadProgressBar.Maximum)
                { LogsLoadProgressBar.Value++; }
            }
            else
            {
                SetControlsToLogsUploadMode(false, false, logName + " upload to server failed");
            }
            Log.Information($"LogViewerControl:DeviceLogUpLoadedToServerCallback Exit actionSuccessful:{actionSuccessful} logName:{logName}");
        }

        private void AllDeviceLogsUpLoadedToServerCallback(bool actionSuccessful)
        {
            if (IsDummyDeviceConnected)
                return;

            Log.Information($"LogViewerControl:AllDeviceLogsUpLoadedToServerCallback Entry actionSuccessful:{actionSuccessful}");
            LogsLoadProgressBar.Value = 0;
            if (actionSuccessful == true)
            {
                SetControlsToLogsUploadMode(false, false, Properties.Resources.DEVICE_LOG_UPLOAD_SUCCESS);
            }
            else
            {
                SetControlsToLogsUploadMode(false, false, Properties.Resources.DEVICE_LOG_UPLOAD_FAILURE);
            }
            Log.Information($"LogViewerControl:AllDeviceLogsUpLoadedToServerCallback Exit actionSuccessful:{actionSuccessful}");
        }

        private void UploadAllLogsButton_Click(object sender, EventArgs e)
        {
            if (LogsRetrievalComplete)
            {
                DeviceLogManagement.Instance.ClearDeviceLogsUploadStatus();
                UploadAllLogsToServer();
            }
            else
            {
                Log.Warning($"LogViewerControl:mUploadAllLogsButton_Click Log Retrieval is not completed. Skipping Upload operation");
            }
        }

        public void UploadAllLogsToServer()
        {
            Log.Information($"LogViewerControl:UploadAllLogsToServer Entry ");
            LogsStatusLabel.Visible = true;
            LogsLoadProgressBar.Maximum = LogNames.Count;
            LogsLoadProgressBar.Value = 0;
            SetControlsToLogsUploadMode(false, true, Properties.Resources.DEVICE_LOGS_UPLOAD_INPROGRESS);

            string status = DeviceLogManagement.Instance.UploadAllLogsToServer(DeviceLogUpLoadedToServerCallback, AllDeviceLogsUpLoadedToServerCallback, false);
            if (!string.IsNullOrEmpty(status))
            {
                //error condition
                SetControlsToLogsUploadMode(false, false, Properties.Resources.DEVICE_LOG_UPLOAD_FAILURE + ":" + status);
            }
            Log.Information($"LogViewerControl:UploadAllLogsToServer Exit ");

        }

        private void SetControlsToLogsUploadMode(bool diagnosticLogsUploadMode, bool uploadStarted, string uploadStatusString)
        {
            Log.Information($"LogViewerControl:SetControlsToLogsUploadMode diagnosticLogsUploadMode:{diagnosticLogsUploadMode} uploadStarted:{uploadStarted} uploadStatusString:{uploadStatusString}");
            LogsStatusLabel.Text = uploadStatusString;
            if (uploadStarted == true)
            {
                LogsLoadProgressBar.Visible = true;
                UploadAllLogsButton.Enabled = false;
            }
            else
            {
                UploadAllLogsButton.Enabled = true;
                LogsLoadProgressBar.Visible = false;
            }
        }

        private void SaveLogsToFileButton_Click(object sender, EventArgs e)
        {
            Log.Information($"LogViewerControl:mSaveLogsToFileButton_Click Entry");
            SaveFileDialog fileDialog = new SaveFileDialog
            {
                InitialDirectory = LogsSaveDir,
                RestoreDirectory = true,
                Filter = "HTML files (*.html;*.htm)|*.html;*.htm|All files (*.*)|*.*",
                AddExtension = true,
                OverwritePrompt = true,

                FileName = ESS_Main.Instance.GetVentSerialNumber() + "_" + LogsRetrieved.ToString("MMddyyy-HHmmss")
            };

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DeviceLogManagement.Instance.SaveAllLogsToFile(fileDialog.FileName);
                    LogsSaveDir = Path.GetDirectoryName(fileDialog.FileName);
                }
                catch (Exception ex)
                {
                    Log.Error($"LogViewerControl:mSaveLogsToFileButton_Click Exception:{ex.Message}");
                    MessageBox.Show("Error: Could not read file from disk. " + ex.Message);
                }
            }
            Log.Information($"LogViewerControl:mSaveLogsToFileButton_Click Exit");

        }

        private void ViewLogButton_Click(object sender, EventArgs e)
        {
            //string logName = mViewLogSelectionComboBox.SelectedItem.ToString().Replace(ESS.Properties.Resources.LOG_NAME_POSTPEND,"");
            string logName = LogsListBox.SelectedItem.ToString().Replace(Properties.Resources.LOG_NAME_POSTPEND, "");

            // get log container
            string logHtml = DeviceLogManagement.Instance.GetDeviceLogAsHtml(logName);
            Log.Information($"LogViewerControl:mViewLogButton_Click Entry logName:{logName} logHtml:{logHtml}");
            // launch log view display
            if (LogViewer1 != null)
            {
                this.Controls.Remove(LogViewer1);
                LogViewer1.Close();
            }

            // launch browser to display log
            LogViewer1 = new LogViewer(logName);
            LogViewer1.DocumentText(logHtml);
            LogViewer1.Show();
            Log.Information($"LogViewerControl:mViewLogButton_Click Exit ");
        }

        private void DeviceInfoButton_Click(object sender, EventArgs e)
        {
            // get log container
            string logHtml = DeviceLogManagement.Instance.GetDeviceInfoAsHtml();
            Log.Information($"LogViewerControl:mDeviceInfoButton_Click Entry logHtml:{logHtml}");
            // launch log view display
            if (LogViewer1 != null)
            {
                this.Controls.Remove(LogViewer1);
                LogViewer1.Close();
            }

            // launch browser to display log
            LogViewer1 = new LogViewer("DeviceInfo");
            LogViewer1.DocumentText(logHtml);
            LogViewer1.Show();
            Log.Information($"LogViewerControl:mDeviceInfoButton_Click Exit logHtml:{logHtml}");
        }
     }
}
