using Serilog;
using System;
using System.IO;
using System.Windows.Forms;

namespace Covidien.CGRS.ESS
{
    public partial class LogControl : UserControl
    {
        private LogViewer LogViewer1;

        public LogControl( string logName)
        {
            InitializeComponent();

            SaveLogButton.Text = Properties.Resources.SAVE_TO_FILE_BUTTON;
            UploadLogButton.Text = Properties.Resources.UPLOAD_LOG_BUTTON;
            UploadStatusLabel.Text = Properties.Resources.LOG_UPLOADED_STATUS;

            LogNameLabel.Text = logName;
            LogViewer1 = null;

            if (DeviceLogManagement.Instance.IsDeviceLogAvailable(logName))
            {
                ShowLogIsAvailable();
            }
        }
        public string LogName()
        {
            return LogNameLabel.Text;
        }

        public void ShowLogIsAvailable()
        {
            ViewLogButton.Enabled = true;
            SaveLogButton.Enabled = true;

            if (DeviceLogManagement.Instance.IsDeviceLogStored(LogNameLabel.Text))
                {
                    UploadStatusLabel.Visible = true;
                    UploadLogButton.Visible = false;
                }
                else
                {
                    UploadStatusLabel.Visible = false;
                    UploadLogButton.Visible = true;
                    UploadLogButton.Enabled = true;
                }
        }

        private void ViewLogButton_Click(object sender, EventArgs e)
        {
            Log.Information($"LogControl:mViewLogButton_Click");
            if (AuthenticationService.Instance().GetLogAccessCredentials() == AuthenticationService.CREDENTIAL_STATUS.APPROVED)
            {
                // get log container
                string logHtml = DeviceLogManagement.Instance.GetDeviceLogAsHtml(LogNameLabel.Text);

                // launch log view display
                if (LogViewer1 != null)
                {
                    this.Controls.Remove(LogViewer1);
                    LogViewer1.Close();
                }

                // launch browser to display log
                LogViewer1 = new LogViewer(LogNameLabel.Text);
                LogViewer1.DocumentText(logHtml);
                LogViewer1.Show();
            }
            else
                MessageBox.Show(Properties.Resources.FUNCTION_RESTRICTED_MSG, Properties.Resources.ESS_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        private void SaveLogButton_Click(object sender, EventArgs e)
        {
            Log.Information($"LogControl:mSaveLogButton_Click");
            if (AuthenticationService.Instance().GetLogAccessCredentials() == AuthenticationService.CREDENTIAL_STATUS.APPROVED)
            {
                // show save file dialog
                SaveFileDialog dialog = new SaveFileDialog
                {
                    Filter = Properties.Resources.SAVE_FILE_DIALOG_FILTER,
                    Title = Properties.Resources.SAVE_FILE_DIALOG_TITLE + " " + LogNameLabel.Text
                };
                dialog.ShowDialog();

                // send save log file command
                if (dialog.FileName != "")
                {
                    FileStream fs = (FileStream)dialog.OpenFile();

                    // call business layer to save log to file
                    DeviceLogManagement.Instance.SaveLogToStream(LogNameLabel.Text, fs);

                    fs.Close();
                }
            }
            else
                MessageBox.Show(Properties.Resources.FUNCTION_RESTRICTED_MSG, Properties.Resources.ESS_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        private void UploadLogButton_Click(object sender, EventArgs e)
        {
            Log.Information($"LogControl:mUploadLogButton_Click");
            UploadLogButton.Visible = false;
            UploadStatusLabel.Visible = true;
            DeviceLogManagement.Instance.SendLogToServer(LogNameLabel.Text);
        }
    }
}
