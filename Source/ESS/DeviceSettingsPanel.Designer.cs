namespace Covidien.CGRS.ESS
{
    partial class DeviceSettingsPanel
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
            this.DeviceSettingsPanelStatusGroupBox = new System.Windows.Forms.GroupBox();
            this.DeviceSettingsPanelProgressBar = new System.Windows.Forms.ProgressBar();
            this.DeviceSettingsPanelStatusLabel = new System.Windows.Forms.Label();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.DeviceSettingsPanelStatusGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mDeviceSettingsPanelStatusGroupBox
            // 
            this.DeviceSettingsPanelStatusGroupBox.Controls.Add(this.DeviceSettingsPanelProgressBar);
            this.DeviceSettingsPanelStatusGroupBox.Controls.Add(this.DeviceSettingsPanelStatusLabel);
            this.DeviceSettingsPanelStatusGroupBox.Location = new System.Drawing.Point(41, 17);
            this.DeviceSettingsPanelStatusGroupBox.Name = "mDeviceSettingsPanelStatusGroupBox";
            this.DeviceSettingsPanelStatusGroupBox.Size = new System.Drawing.Size(495, 76);
            this.DeviceSettingsPanelStatusGroupBox.TabIndex = 33;
            this.DeviceSettingsPanelStatusGroupBox.TabStop = false;
            this.DeviceSettingsPanelStatusGroupBox.Text = "Status";
            // 
            // mDeviceSettingsPanelProgressBar
            // 
            this.DeviceSettingsPanelProgressBar.Location = new System.Drawing.Point(10, 45);
            this.DeviceSettingsPanelProgressBar.Name = "mDeviceSettingsPanelProgressBar";
            this.DeviceSettingsPanelProgressBar.Size = new System.Drawing.Size(479, 15);
            this.DeviceSettingsPanelProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.DeviceSettingsPanelProgressBar.TabIndex = 29;
            this.DeviceSettingsPanelProgressBar.Visible = false;
            // 
            // mDeviceSettingsPanelStatusLabel
            // 
            this.DeviceSettingsPanelStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeviceSettingsPanelStatusLabel.Location = new System.Drawing.Point(6, 21);
            this.DeviceSettingsPanelStatusLabel.Name = "mDeviceSettingsPanelStatusLabel";
            this.DeviceSettingsPanelStatusLabel.Size = new System.Drawing.Size(483, 18);
            this.DeviceSettingsPanelStatusLabel.TabIndex = 25;
            this.DeviceSettingsPanelStatusLabel.Text = "Status:";
            // 
            // mRefreshButton
            // 
            this.RefreshButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RefreshButton.Location = new System.Drawing.Point(318, 404);
            this.RefreshButton.Name = "mRefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(106, 30);
            this.RefreshButton.TabIndex = 34;
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // mUpdateButton
            // 
            this.UpdateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpdateButton.Location = new System.Drawing.Point(430, 404);
            this.UpdateButton.Name = "mUpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(106, 30);
            this.UpdateButton.TabIndex = 35;
            this.UpdateButton.Text = "Submit";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // DeviceSettingsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UpdateButton);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.DeviceSettingsPanelStatusGroupBox);
            this.Name = "DeviceSettingsPanel";
            this.Size = new System.Drawing.Size(580, 452);
            this.DeviceSettingsPanelStatusGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox DeviceSettingsPanelStatusGroupBox;
        private System.Windows.Forms.Label DeviceSettingsPanelStatusLabel;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.ProgressBar DeviceSettingsPanelProgressBar;
    }
}
