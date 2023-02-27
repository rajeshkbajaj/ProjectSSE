namespace Covidien.CGRS.ESS
{
    partial class SystemStatusBar
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.StatusTimer = new System.Windows.Forms.Timer(this.components);
            this.SystemStatusStrip = new System.Windows.Forms.StatusStrip();
            this.RSAStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.RSSStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.UploadStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.DownloadStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.SystemStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mStatusTimer
            // 
            this.StatusTimer.Interval = 10000;
            this.StatusTimer.Tick += new System.EventHandler(this.AgentStatusTimerExpired);
            // 
            // mSystemStatusStrip
            // 
            this.SystemStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RSAStatus,
            this.RSSStatus,
            this.UploadStatusLabel,
            this.DownloadStatusLabel});
            this.SystemStatusStrip.Location = new System.Drawing.Point(0, -4);
            this.SystemStatusStrip.Name = "mSystemStatusStrip";
            this.SystemStatusStrip.Size = new System.Drawing.Size(784, 24);
            this.SystemStatusStrip.TabIndex = 0;
            this.SystemStatusStrip.Text = "statusStrip1";
            // 
            // mRSAStatus
            // 
            this.RSAStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.RSAStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.RSAStatus.Name = "mRSAStatus";
            this.RSAStatus.Size = new System.Drawing.Size(43, 19);
            this.RSAStatus.Text = "Agent";
            // 
            // mRSSStatus
            // 
            this.RSSStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.RSSStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.RSSStatus.Name = "mRSSStatus";
            this.RSSStatus.Size = new System.Drawing.Size(43, 19);
            this.RSSStatus.Text = "Server";
            // 
            // mUploadStatusLabel
            // 
            this.UploadStatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.UploadStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.UploadStatusLabel.Name = "mUploadStatusLabel";
            this.UploadStatusLabel.Size = new System.Drawing.Size(90, 19);
            this.UploadStatusLabel.Text = "Agent Upload: ";
            // 
            // mDownloadStatusLabel
            // 
            this.DownloadStatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.DownloadStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.DownloadStatusLabel.Name = "mDownloadStatusLabel";
            this.DownloadStatusLabel.Size = new System.Drawing.Size(103, 19);
            this.DownloadStatusLabel.Text = "Agent Download:";
            // 
            // mVersionLabel
            // 
            this.VersionLabel.Location = new System.Drawing.Point(653, 2);
            this.VersionLabel.Name = "mVersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(115, 16);
            this.VersionLabel.TabIndex = 1;
            this.VersionLabel.Text = "Version";
            this.VersionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // SystemStatusBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.SystemStatusStrip);
            this.Name = "SystemStatusBar";
            this.Size = new System.Drawing.Size(784, 20);
            this.SystemStatusStrip.ResumeLayout(false);
            this.SystemStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer StatusTimer;
        private System.Windows.Forms.StatusStrip SystemStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel RSAStatus;
        private System.Windows.Forms.ToolStripStatusLabel RSSStatus;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.ToolStripStatusLabel UploadStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel DownloadStatusLabel;
    }
}
