using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Covidien.CGRS.Comms;

namespace Covidien.CGRS.VTS
{
    public partial class AgentStatusControl : UserControl
    {
        public AgentStatusControl()
        {
            InitializeComponent();
            SetStatusUnavailable();

            JobStatusStrings = new Hashtable();
            JobStatusStrings.Add(RsaStatus.RSA_JOB_STATUS.UNKNOWN, VTS.Properties.Resources.STATUS_UNKNOWN);
            JobStatusStrings.Add(RsaStatus.RSA_JOB_STATUS.COMPLETE, VTS.Properties.Resources.STATUS_COMPLETE);
            JobStatusStrings.Add(RsaStatus.RSA_JOB_STATUS.SCHEDULED, VTS.Properties.Resources.STATUS_SCHEDULED);
            JobStatusStrings.Add(RsaStatus.RSA_JOB_STATUS.RUNNING, VTS.Properties.Resources.STATUS_RUNNING);
            JobStatusStrings.Add(RsaStatus.RSA_JOB_STATUS.ERROR, VTS.Properties.Resources.STATUS_ERROR);

            AgentStatusStrings = new Hashtable();
            AgentStatusStrings.Add(RsaStatus.RSA_AGENT_STATUS.ACTIVE, VTS.Properties.Resources.STATUS_ACTIVE);
            AgentStatusStrings.Add(RsaStatus.RSA_AGENT_STATUS.IDLE, VTS.Properties.Resources.STATUS_IDLE);
            AgentStatusStrings.Add(RsaStatus.RSA_AGENT_STATUS.RECEIVING, VTS.Properties.Resources.STATUS_RECEIVING);
            AgentStatusStrings.Add(RsaStatus.RSA_AGENT_STATUS.TRANSMITTING, VTS.Properties.Resources.STATUS_TRANSMITTING);
            AgentStatusStrings.Add(RsaStatus.RSA_AGENT_STATUS.UNKNOWN, VTS.Properties.Resources.STATUS_UNKNOWN);

            ServerStatusStrings = new Hashtable();
            ServerStatusStrings.Add(RsaStatus.RSA_SERVER_STATUS.CONNECTED, VTS.Properties.Resources.STATUS_CONNECTED);
            ServerStatusStrings.Add(RsaStatus.RSA_SERVER_STATUS.DISCONNECTED, VTS.Properties.Resources.STATUS_DISCONNECTED);
            ServerStatusStrings.Add(RsaStatus.RSA_SERVER_STATUS.UNKNOWN, VTS.Properties.Resources.STATUS_UNKNOWN);
        }

        private Hashtable JobStatusStrings;
        private Hashtable AgentStatusStrings;
        private Hashtable ServerStatusStrings;

        public void EnableControls()
        {
        }

        public void DisableControls()
        {
        }

        public void UserLoggedIn()
        {
            //get agent status to begin with
            GetAgentStatus();
        }

        public void UserLoginCancel()
        {
            SetStatusUnavailable();
        }

        private void SetStatusUnavailable()
        {
            mAgentStatusText.Text = VTS.Properties.Resources.STATUS_UNAVAILABLE;
            mServerStatusText.Text = VTS.Properties.Resources.STATUS_UNAVAILABLE;
            mJobStatusText.Text = VTS.Properties.Resources.STATUS_UNAVAILABLE;
            mUploadStatusText.Text = VTS.Properties.Resources.STATUS_UNAVAILABLE;
            mDownloadStatusText.Text = VTS.Properties.Resources.STATUS_UNAVAILABLE;
            mUploadStatusProgressBar.Visible = false;
            mDownloadStatusProgressBar.Visible = false;
        }

        private void RsaStatusCallback(string transactionId, bool actionSuccessful, string sessionId, RsaStatus status)
        {
            //calledback - update status
            if(AgentStatusStrings.ContainsKey(status.AgentStatus))
            {
                mAgentStatusText.Text = (string)AgentStatusStrings[status.AgentStatus];
            }
            else
            {
                mAgentStatusText.Text = VTS.Properties.Resources.STATUS_UNKNOWN;
            }

            if(JobStatusStrings.ContainsKey(status.JobStatus))
            {
                mJobStatusText.Text = (string)JobStatusStrings[status.JobStatus];
            }
            else
            {
                mJobStatusText.Text = VTS.Properties.Resources.STATUS_UNKNOWN;
            }

            if(ServerStatusStrings.ContainsKey(status.ServerStatus))
            {
                mServerStatusText.Text = (string)ServerStatusStrings[status.ServerStatus];
            }
            else
            {
                mServerStatusText.Text = VTS.Properties.Resources.STATUS_UNKNOWN;
            }

            if (status.UploadTime == -1.0f)
            {
                mUploadStatusText.Text = VTS.Properties.Resources.STATUS_ERROR;
            }
            else if (status.UploadTime == 0.0f)
            {
                mUploadStatusText.Text = VTS.Properties.Resources.STATUS_COMPLETE;
            }
            else
            {
                mUploadStatusText.Text = string.Format("{0} second(s)", status.UploadTime);
            }

            if (status.DownloadTime == -1.0f)
            {
                mDownloadStatusText.Text = VTS.Properties.Resources.STATUS_ERROR;
            }
            else if (status.DownloadTime == 0.0f)
            {
                mDownloadStatusText.Text = VTS.Properties.Resources.STATUS_COMPLETE;
            }
            else
            {
                mDownloadStatusText.Text = string.Format("{0} second(s)", status.DownloadTime);
            }


            mUploadStatusProgressBar.Visible = false;
            mDownloadStatusProgressBar.Visible = false;

            //free memory
            status = null;
        }

        private void GetAgentStatus()
        {
            //gets agent status
            SessionManagement.Instance().CheckRsaStatus(RsaStatusCallback);
        }

        private void AgentStatusTimerExpired(object sender, EventArgs e)
        {
            GetAgentStatus();
        }

        /// <summary>
        /// called when this control gets focus, when user enters this tab
        /// this mechanism is used to enable/ disable timer when user opens/ closes tab
        /// when tab is closed, timer is not running, timer runs only when user open the AgentStatus control
        /// this way, get status is updated only when user is interested in it
        /// </summary>
        public void EnterControl()
        {
            //refresh status immediately and then every period (2.5 seconds)
            GetAgentStatus();
            mAgentStatusTimer.Enabled = true;
        }

        /// <summary>
        /// called when this control loses focus, when user leaves this tab
        /// this mechanism is used to enable/ disable timer when user opens/ closes tab
        /// when tab is closed, timer is not running, timer runs only when user open the AgentStatus control
        /// this way, get status is updated only when user is interested in it
        /// </summary>
        public void LeaveControl()
        {
            mAgentStatusTimer.Enabled = false;
        }

    }
}
