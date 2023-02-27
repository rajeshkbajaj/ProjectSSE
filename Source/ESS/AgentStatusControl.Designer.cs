namespace Covidien.CGRS.VTS
{
    partial class AgentStatusControl
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
            this.mAgentStatusLabel = new System.Windows.Forms.Label();
            this.mServerStatusLabel = new System.Windows.Forms.Label();
            this.mJobStatusLabel = new System.Windows.Forms.Label();
            this.mUploadStatusLabel = new System.Windows.Forms.Label();
            this.mDownloadStatusLabel = new System.Windows.Forms.Label();
            this.mAgentStatusText = new System.Windows.Forms.Label();
            this.mServerStatusText = new System.Windows.Forms.Label();
            this.mJobStatusText = new System.Windows.Forms.Label();
            this.mUploadStatusText = new System.Windows.Forms.Label();
            this.mUploadStatusProgressBar = new System.Windows.Forms.ProgressBar();
            this.mDownloadStatusText = new System.Windows.Forms.Label();
            this.mDownloadStatusProgressBar = new System.Windows.Forms.ProgressBar();
            this.mAgentStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // mAgentStatusLabel
            // 
            this.mAgentStatusLabel.AutoSize = true;
            this.mAgentStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mAgentStatusLabel.Location = new System.Drawing.Point(98, 125);
            this.mAgentStatusLabel.Name = "mAgentStatusLabel";
            this.mAgentStatusLabel.Size = new System.Drawing.Size(107, 20);
            this.mAgentStatusLabel.TabIndex = 0;
            this.mAgentStatusLabel.Text = "Agent Status:";
            // 
            // mServerStatusLabel
            // 
            this.mServerStatusLabel.AutoSize = true;
            this.mServerStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mServerStatusLabel.Location = new System.Drawing.Point(98, 176);
            this.mServerStatusLabel.Name = "mServerStatusLabel";
            this.mServerStatusLabel.Size = new System.Drawing.Size(110, 20);
            this.mServerStatusLabel.TabIndex = 0;
            this.mServerStatusLabel.Text = "Server Status:";
            // 
            // mJobStatusLabel
            // 
            this.mJobStatusLabel.AutoSize = true;
            this.mJobStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mJobStatusLabel.Location = new System.Drawing.Point(98, 229);
            this.mJobStatusLabel.Name = "mJobStatusLabel";
            this.mJobStatusLabel.Size = new System.Drawing.Size(90, 20);
            this.mJobStatusLabel.TabIndex = 0;
            this.mJobStatusLabel.Text = "Job Status:";
            // 
            // mUploadStatusLabel
            // 
            this.mUploadStatusLabel.AutoSize = true;
            this.mUploadStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mUploadStatusLabel.Location = new System.Drawing.Point(98, 284);
            this.mUploadStatusLabel.Name = "mUploadStatusLabel";
            this.mUploadStatusLabel.Size = new System.Drawing.Size(102, 20);
            this.mUploadStatusLabel.TabIndex = 0;
            this.mUploadStatusLabel.Text = "Upload Time:";
            // 
            // mDownloadStatusLabel
            // 
            this.mDownloadStatusLabel.AutoSize = true;
            this.mDownloadStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mDownloadStatusLabel.Location = new System.Drawing.Point(98, 343);
            this.mDownloadStatusLabel.Name = "mDownloadStatusLabel";
            this.mDownloadStatusLabel.Size = new System.Drawing.Size(122, 20);
            this.mDownloadStatusLabel.TabIndex = 0;
            this.mDownloadStatusLabel.Text = "Download Time:";
            // 
            // mAgentStatusText
            // 
            this.mAgentStatusText.AutoSize = true;
            this.mAgentStatusText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mAgentStatusText.Location = new System.Drawing.Point(238, 125);
            this.mAgentStatusText.Name = "mAgentStatusText";
            this.mAgentStatusText.Size = new System.Drawing.Size(64, 20);
            this.mAgentStatusText.TabIndex = 0;
            this.mAgentStatusText.Text = "Inactive";
            // 
            // mServerStatusText
            // 
            this.mServerStatusText.AutoSize = true;
            this.mServerStatusText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mServerStatusText.Location = new System.Drawing.Point(238, 176);
            this.mServerStatusText.Name = "mServerStatusText";
            this.mServerStatusText.Size = new System.Drawing.Size(64, 20);
            this.mServerStatusText.TabIndex = 0;
            this.mServerStatusText.Text = "Inactive";
            // 
            // mJobStatusText
            // 
            this.mJobStatusText.AutoSize = true;
            this.mJobStatusText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mJobStatusText.Location = new System.Drawing.Point(238, 229);
            this.mJobStatusText.Name = "mJobStatusText";
            this.mJobStatusText.Size = new System.Drawing.Size(35, 20);
            this.mJobStatusText.TabIndex = 0;
            this.mJobStatusText.Text = "Idle";
            // 
            // mUploadStatusText
            // 
            this.mUploadStatusText.AutoSize = true;
            this.mUploadStatusText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mUploadStatusText.Location = new System.Drawing.Point(239, 284);
            this.mUploadStatusText.Name = "mUploadStatusText";
            this.mUploadStatusText.Size = new System.Drawing.Size(177, 20);
            this.mUploadStatusText.TabIndex = 0;
            this.mUploadStatusText.Text = "No Uploads In Progress";
            // 
            // mUploadStatusProgressBar
            // 
            this.mUploadStatusProgressBar.Location = new System.Drawing.Point(243, 284);
            this.mUploadStatusProgressBar.Name = "mUploadStatusProgressBar";
            this.mUploadStatusProgressBar.Size = new System.Drawing.Size(204, 20);
            this.mUploadStatusProgressBar.TabIndex = 1;
            // 
            // mDownloadStatusText
            // 
            this.mDownloadStatusText.AutoSize = true;
            this.mDownloadStatusText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mDownloadStatusText.Location = new System.Drawing.Point(239, 343);
            this.mDownloadStatusText.Name = "mDownloadStatusText";
            this.mDownloadStatusText.Size = new System.Drawing.Size(190, 20);
            this.mDownloadStatusText.TabIndex = 0;
            this.mDownloadStatusText.Text = "All Downloads Completed";
            // 
            // mDownloadStatusProgressBar
            // 
            this.mDownloadStatusProgressBar.Location = new System.Drawing.Point(243, 343);
            this.mDownloadStatusProgressBar.Name = "mDownloadStatusProgressBar";
            this.mDownloadStatusProgressBar.Size = new System.Drawing.Size(204, 20);
            this.mDownloadStatusProgressBar.TabIndex = 1;
            // 
            // mAgentStatusTimer
            // 
            this.mAgentStatusTimer.Interval = 2500;
            this.mAgentStatusTimer.Tick += new System.EventHandler(this.AgentStatusTimerExpired);
            // 
            // AgentStatusControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mDownloadStatusProgressBar);
            this.Controls.Add(this.mUploadStatusProgressBar);
            this.Controls.Add(this.mDownloadStatusLabel);
            this.Controls.Add(this.mUploadStatusLabel);
            this.Controls.Add(this.mJobStatusLabel);
            this.Controls.Add(this.mServerStatusLabel);
            this.Controls.Add(this.mUploadStatusText);
            this.Controls.Add(this.mDownloadStatusText);
            this.Controls.Add(this.mJobStatusText);
            this.Controls.Add(this.mServerStatusText);
            this.Controls.Add(this.mAgentStatusText);
            this.Controls.Add(this.mAgentStatusLabel);
            this.MinimumSize = new System.Drawing.Size(578, 508);
            this.Name = "AgentStatusControl";
            this.Size = new System.Drawing.Size(578, 508);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label mAgentStatusLabel;
        private System.Windows.Forms.Label mServerStatusLabel;
        private System.Windows.Forms.Label mJobStatusLabel;
        private System.Windows.Forms.Label mUploadStatusLabel;
        private System.Windows.Forms.Label mDownloadStatusLabel;
        private System.Windows.Forms.Label mAgentStatusText;
        private System.Windows.Forms.Label mServerStatusText;
        private System.Windows.Forms.Label mJobStatusText;
        private System.Windows.Forms.Label mUploadStatusText;
        private System.Windows.Forms.ProgressBar mUploadStatusProgressBar;
        private System.Windows.Forms.Label mDownloadStatusText;
        private System.Windows.Forms.ProgressBar mDownloadStatusProgressBar;
        private System.Windows.Forms.Timer mAgentStatusTimer;
    }
}
