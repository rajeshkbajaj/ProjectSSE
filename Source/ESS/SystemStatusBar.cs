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
    using Covidien.CGRS.PcAgentInterfaceBusiness;
    using Oasis.Agent;
    using Serilog;

    public partial class SystemStatusBar : UserControl
    {
        public SystemStatusBar()
        {
            InitializeComponent();

            UploadStatusLabel.Visible = false;
            DownloadStatusLabel.Visible = false;
            UploadStatusLabel.Text = Properties.Resources.AGENT_STATUS_UPLOAD;
            DownloadStatusLabel.Text = Properties.Resources.AGENT_STATUS_DOWNLOAD;

            //default is red - not connected
            RSAStatus.BackColor = Color.Red;
            RSSStatus.BackColor = Color.Red;

            VersionLabel.Text = Properties.Resources.VERSION_LABEL + ": " + ESS_Main.Instance.GetESSVersion();
        }

        public void RSAConnected(bool RsaStatus, bool RssStatus)
        {
            //rsa - green
            Log.Information($"SystemStatusBar:RSAConnected  Entry RsaStatus:{RsaStatus} RssStatus:{RssStatus}");
            if (RsaStatus == true)
            {
                RSAStatus.BackColor = Color.LimeGreen;
            }
            else
            {
                RSAStatus.BackColor = Color.Red;
            }

            if (RssStatus == true)
            {
                RSSStatus.BackColor = Color.LimeGreen;
            }
            else
            {
                RSSStatus.BackColor = Color.Red;
            }
            UploadStatusLabel.Visible = false;
            DownloadStatusLabel.Visible = false;
            Log.Information($"SystemStatusBar:RSAConnected  Exit RsaStatus:{RsaStatus} RssStatus:{RssStatus}");
        }

        public void UserLoggedIn()
        {
            Log.Information($"SystemStatusBar:UserLoggedIn  Entry");
            //once user loggedin, get agent status
            UploadStatusLabel.Visible = true;
            DownloadStatusLabel.Visible = true;

            //refreshes every 5 seconds
            StatusTimer.Enabled = true;
            GetAgentStatus();
            Log.Information($"SystemStatusBar:UserLoggedIn  Exit");
        }

        public void RsaStatusCallback(RsaStatus status)
        {
            Log.Information($"SystemStatusBar:RsaStatusCallback  Entry Rsa Status :{status.ServerStatus}");
            //calledback - update status
            if (status.ServerStatus == RsaStatus.RSA_SERVER_STATUS.CONNECTED)
            {
                RSSStatus.BackColor = Color.LimeGreen;
            }
            else
            {
                RSSStatus.BackColor = Color.Red;
            }

            if (status.UploadTime == -1.0f)
            {
                UploadStatusLabel.Text = $"{Properties.Resources.AGENT_STATUS_UPLOAD} {Properties.Resources.STATUS_ERROR}";
            }
            else if (status.UploadTime == 0.0f)
            {
                UploadStatusLabel.Text = $"{Properties.Resources.AGENT_STATUS_UPLOAD} {Properties.Resources.STATUS_COMPLETE}";
            }
            else
            {
                UploadStatusLabel.Text = $"{Properties.Resources.AGENT_STATUS_UPLOAD} {status.UploadTime} {Properties.Resources.SECONDS_REMAINING}";
            }

            if (status.DownloadTime == -1.0f)
            {
                DownloadStatusLabel.Text = $"{Properties.Resources.AGENT_STATUS_DOWNLOAD} {Properties.Resources.STATUS_ERROR}";
            }
            else if (status.DownloadTime == 0.0f)
            {
                if (status.DataReady)
                {
                    DownloadStatusLabel.Text = $"{Properties.Resources.AGENT_STATUS_DOWNLOAD} {Properties.Resources.STATUS_COMPLETE}";
                    if (status.InsufficientSpace)
                    {
                        DownloadStatusLabel.Text = $"{DownloadStatusLabel.Text}. {Properties.Resources.STATUS_INSUFFICIENTSPACE}";
                    }

                    ESS_Main.Instance.GenerateSystemEvent(ESS_Main.SystemEvent.AGENT_DOWNLOAD_STATUS_COMPLETE);
                }
                else
                {
                    if (status.InsufficientSpace)
                    {
                        DownloadStatusLabel.Text = $"{Properties.Resources.AGENT_STATUS_DOWNLOAD} {Properties.Resources.STATUS_DOWNLOADING} {Properties.Resources.STATUS_INSUFFICIENTSPACE}";
                    }
                    else if (status.ServerStatus != RsaStatus.RSA_SERVER_STATUS.CONNECTED)
                    {
                        DownloadStatusLabel.Text = $"{Properties.Resources.AGENT_STATUS_DOWNLOAD} {Properties.Resources.STATUS_DOWNLOADING_HALT} {Properties.Resources.STATUS_CONNECT_INTERNET}";
                    }
                    else
                    {
                        DownloadStatusLabel.Text = $"{Properties.Resources.AGENT_STATUS_DOWNLOAD} {Properties.Resources.STATUS_DOWNLOADING} {Properties.Resources.STATUS_DO_NOT_DISCONNECT}";
                    }
                }
            }
            else
            {
                DownloadStatusLabel.Text = $"{Properties.Resources.AGENT_STATUS_DOWNLOAD} {status.DownloadTime} {Properties.Resources.SECONDS_REMAINING}";
            }

            if (status.InsufficientSpace)
            {
                Log.Error($"SystemStatusBar:RsaStatusCallback, insufficient space in PC, free some disk space.");
            }

            //free memory
            status = null;
            Log.Information($"SystemStatusBar:RsaStatusCallback  Exit");
            Refresh();
        }

        private void GetAgentStatus()
        {
            Log.Information($"SystemStatusBar:GetAgentStatus  Entry");
            BusinessServicesBridge.Instance.CheckServerStatus(RsaStatusCallback);
            Log.Information($"SystemStatusBar:GetAgentStatus  Exit");
        }

        private void AgentStatusTimerExpired(object sender, EventArgs e)
        {
            Log.Information($"SystemStatusBar:AgentStatusTimerExpired  Entry");
            GetAgentStatus();
            Log.Information($"SystemStatusBar:AgentStatusTimerExpired  Exit");
        }

        public void SessionClosed()
        {
        }

        public void AddDeviceRequestMessage(string tag, string message)
        {
            Log.Debug($"Dummy AddDeviceRequestMessage {tag}, {message}");
        }
        public void RemoveDeviceRequestMessage(string tag)
        {
            Log.Debug($"Dummy RemoveDeviceRequestMessage {tag}");
        }

        public void DeviceConnected(bool connected, bool downloadMode)
        {
            Log.Debug($"Dummy DeviceConnected {connected}, {downloadMode}");
        }

    }
}
