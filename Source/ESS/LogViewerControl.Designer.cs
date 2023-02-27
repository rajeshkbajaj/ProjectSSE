namespace Covidien.CGRS.ESS
{
    partial class LogViewerControl
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
            this.ClearDeviceLogsButton = new System.Windows.Forms.Button();
            this.ViewLogButton = new System.Windows.Forms.Button();
            this.LogsStatusLabel = new System.Windows.Forms.Label();
            this.ViewLogLabel = new System.Windows.Forms.Label();
            this.LogsLoadProgressBar = new System.Windows.Forms.ProgressBar();
            this.DeviceInfoButton = new System.Windows.Forms.Button();
            this.UploadAllLogsButton = new System.Windows.Forms.Button();
            this.LogsListBox = new System.Windows.Forms.ListBox();
            this.SaveLogsToFileButton = new System.Windows.Forms.Button();
            this.StatusGroupBox = new System.Windows.Forms.GroupBox();
            this.StatusGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mClearDeviceLogsButton
            // 
            this.ClearDeviceLogsButton.Enabled = false;
            this.ClearDeviceLogsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClearDeviceLogsButton.Location = new System.Drawing.Point(350, 309);
            this.ClearDeviceLogsButton.Name = "mClearDeviceLogsButton";
            this.ClearDeviceLogsButton.Size = new System.Drawing.Size(186, 45);
            this.ClearDeviceLogsButton.TabIndex = 21;
            this.ClearDeviceLogsButton.TabStop = false;
            this.ClearDeviceLogsButton.Text = "Clear Device Logs";
            this.ClearDeviceLogsButton.UseVisualStyleBackColor = true;
            this.ClearDeviceLogsButton.Click += new System.EventHandler(this.ClearDeviceLogsButton_Click);
            // 
            // mViewLogButton
            // 
            this.ViewLogButton.Enabled = false;
            this.ViewLogButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ViewLogButton.Location = new System.Drawing.Point(350, 258);
            this.ViewLogButton.Name = "mViewLogButton";
            this.ViewLogButton.Size = new System.Drawing.Size(186, 45);
            this.ViewLogButton.TabIndex = 24;
            this.ViewLogButton.TabStop = false;
            this.ViewLogButton.Text = "View Log";
            this.ViewLogButton.UseVisualStyleBackColor = true;
            this.ViewLogButton.Click += new System.EventHandler(this.ViewLogButton_Click);
            // 
            // mLogsStatusLabel
            // 
            this.LogsStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogsStatusLabel.Location = new System.Drawing.Point(6, 21);
            this.LogsStatusLabel.Name = "mLogsStatusLabel";
            this.LogsStatusLabel.Size = new System.Drawing.Size(483, 25);
            this.LogsStatusLabel.TabIndex = 25;
            this.LogsStatusLabel.Text = "Status:";
            this.LogsStatusLabel.Visible = false;
            // 
            // mViewLogLabel
            // 
            this.ViewLogLabel.Enabled = false;
            this.ViewLogLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ViewLogLabel.Location = new System.Drawing.Point(37, 130);
            this.ViewLogLabel.Name = "mViewLogLabel";
            this.ViewLogLabel.Size = new System.Drawing.Size(138, 28);
            this.ViewLogLabel.TabIndex = 26;
            this.ViewLogLabel.Text = "Log";
            // 
            // mLogsLoadProgressBar
            // 
            this.LogsLoadProgressBar.Location = new System.Drawing.Point(10, 45);
            this.LogsLoadProgressBar.Name = "mLogsLoadProgressBar";
            this.LogsLoadProgressBar.Size = new System.Drawing.Size(479, 15);
            this.LogsLoadProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.LogsLoadProgressBar.TabIndex = 28;
            this.LogsLoadProgressBar.Visible = false;
            // 
            // mDeviceInfoButton
            // 
            this.DeviceInfoButton.Enabled = false;
            this.DeviceInfoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeviceInfoButton.Location = new System.Drawing.Point(350, 360);
            this.DeviceInfoButton.Name = "mDeviceInfoButton";
            this.DeviceInfoButton.Size = new System.Drawing.Size(186, 45);
            this.DeviceInfoButton.TabIndex = 29;
            this.DeviceInfoButton.TabStop = false;
            this.DeviceInfoButton.Text = "Device Information";
            this.DeviceInfoButton.UseVisualStyleBackColor = true;
            this.DeviceInfoButton.Click += new System.EventHandler(this.DeviceInfoButton_Click);
            // 
            // mUploadAllLogsButton
            // 
            this.UploadAllLogsButton.Enabled = false;
            this.UploadAllLogsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UploadAllLogsButton.Location = new System.Drawing.Point(350, 156);
            this.UploadAllLogsButton.Name = "mUploadAllLogsButton";
            this.UploadAllLogsButton.Size = new System.Drawing.Size(186, 45);
            this.UploadAllLogsButton.TabIndex = 22;
            this.UploadAllLogsButton.TabStop = false;
            this.UploadAllLogsButton.Text = "Upload all logs";
            this.UploadAllLogsButton.UseVisualStyleBackColor = true;
            this.UploadAllLogsButton.Click += new System.EventHandler(this.UploadAllLogsButton_Click);
            // 
            // mLogsListBox
            // 
            this.LogsListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogsListBox.FormattingEnabled = true;
            this.LogsListBox.ItemHeight = 24;
            this.LogsListBox.Location = new System.Drawing.Point(41, 160);
            this.LogsListBox.Name = "mLogsListBox";
            this.LogsListBox.Size = new System.Drawing.Size(303, 244);
            this.LogsListBox.TabIndex = 31;
            // 
            // mSaveLogsToFileButton
            // 
            this.SaveLogsToFileButton.Enabled = false;
            this.SaveLogsToFileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveLogsToFileButton.Location = new System.Drawing.Point(350, 207);
            this.SaveLogsToFileButton.Name = "mSaveLogsToFileButton";
            this.SaveLogsToFileButton.Size = new System.Drawing.Size(186, 45);
            this.SaveLogsToFileButton.TabIndex = 23;
            this.SaveLogsToFileButton.TabStop = false;
            this.SaveLogsToFileButton.Text = "Save All Logs To File...";
            this.SaveLogsToFileButton.UseVisualStyleBackColor = true;
            this.SaveLogsToFileButton.Click += new System.EventHandler(this.SaveLogsToFileButton_Click);
            // 
            // mStatusGroupBox
            // 
            this.StatusGroupBox.Controls.Add(this.LogsLoadProgressBar);
            this.StatusGroupBox.Controls.Add(this.LogsStatusLabel);
            this.StatusGroupBox.Location = new System.Drawing.Point(41, 17);
            this.StatusGroupBox.Name = "mStatusGroupBox";
            this.StatusGroupBox.Size = new System.Drawing.Size(495, 76);
            this.StatusGroupBox.TabIndex = 32;
            this.StatusGroupBox.TabStop = false;
            this.StatusGroupBox.Text = "Status";
            // 
            // LogViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.StatusGroupBox);
            this.Controls.Add(this.LogsListBox);
            this.Controls.Add(this.DeviceInfoButton);
            this.Controls.Add(this.ViewLogLabel);
            this.Controls.Add(this.ViewLogButton);
            this.Controls.Add(this.SaveLogsToFileButton);
            this.Controls.Add(this.UploadAllLogsButton);
            this.Controls.Add(this.ClearDeviceLogsButton);
            this.MinimumSize = new System.Drawing.Size(578, 504);
            this.Name = "LogViewerControl";
            this.Size = new System.Drawing.Size(578, 508);
            this.StatusGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ClearDeviceLogsButton;
        private System.Windows.Forms.Button ViewLogButton;
        private System.Windows.Forms.Label LogsStatusLabel;
        private System.Windows.Forms.Label ViewLogLabel;
        private System.Windows.Forms.ProgressBar LogsLoadProgressBar;
        private System.Windows.Forms.Button DeviceInfoButton;
        private System.Windows.Forms.Button UploadAllLogsButton;
        private System.Windows.Forms.ListBox LogsListBox;
        private System.Windows.Forms.Button SaveLogsToFileButton;
        private System.Windows.Forms.GroupBox StatusGroupBox;
    }
}
