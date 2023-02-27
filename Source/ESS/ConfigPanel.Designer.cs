namespace Covidien.CGRS.ESS
{
    partial class ConfigPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer Components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (Components != null))
            {
                Components.Dispose();
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
            this.ConfigPanelStatusGroupBox = new System.Windows.Forms.GroupBox();
            this.ConfigPanelStatusLabel = new System.Windows.Forms.Label();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.DeviceSettingsPanelProgressBar = new System.Windows.Forms.ProgressBar();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.PasswordTextBox = new PasscodeTextBox();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.ClearButton = new System.Windows.Forms.Button();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.PasscodeInfoLabel = new System.Windows.Forms.Label();
            this.ReenterPasswordTextBox = new PasscodeTextBox();
            this.ReenterPasswordInfoLabel = new System.Windows.Forms.Label();
            this.WarningLabel = new System.Windows.Forms.Label();
            this.UserEmailLabel = new System.Windows.Forms.Label();
            this.UserEmailInfoLabel = new System.Windows.Forms.Label();
            this.NoRadioButton = new System.Windows.Forms.RadioButton();
            this.YesRadioButton = new System.Windows.Forms.RadioButton();
            this.ConfigPanelStatusGroupBox.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mConfigPanelStatusGroupBox
            // 
            this.ConfigPanelStatusGroupBox.Controls.Add(this.ConfigPanelStatusLabel);
            this.ConfigPanelStatusGroupBox.Location = new System.Drawing.Point(41, 17);
            this.ConfigPanelStatusGroupBox.Name = "mConfigPanelStatusGroupBox";
            this.ConfigPanelStatusGroupBox.Size = new System.Drawing.Size(495, 76);
            this.ConfigPanelStatusGroupBox.TabIndex = 33;
            this.ConfigPanelStatusGroupBox.TabStop = false;
            this.ConfigPanelStatusGroupBox.Text = "Status";
            // 
            // mConfigPanelStatusLabel
            // 
            this.ConfigPanelStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConfigPanelStatusLabel.Location = new System.Drawing.Point(6, 21);
            this.ConfigPanelStatusLabel.Name = "mConfigPanelStatusLabel";
            this.ConfigPanelStatusLabel.Size = new System.Drawing.Size(483, 18);
            this.ConfigPanelStatusLabel.TabIndex = 25;
            this.ConfigPanelStatusLabel.Text = "Status:";
            // 
            // groupBox1
            // 
            this.GroupBox1.Controls.Add(this.DeviceSettingsPanelProgressBar);
            this.GroupBox1.Controls.Add(this.StatusLabel);
            this.GroupBox1.Location = new System.Drawing.Point(55, 27);
            this.GroupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.GroupBox1.Name = "groupBox1";
            this.GroupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.GroupBox1.Size = new System.Drawing.Size(660, 94);
            this.GroupBox1.TabIndex = 34;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Status";
            // 
            // mDeviceSettingsPanelProgressBar
            // 
            this.DeviceSettingsPanelProgressBar.Location = new System.Drawing.Point(13, 55);
            this.DeviceSettingsPanelProgressBar.Margin = new System.Windows.Forms.Padding(4);
            this.DeviceSettingsPanelProgressBar.Name = "mDeviceSettingsPanelProgressBar";
            this.DeviceSettingsPanelProgressBar.Size = new System.Drawing.Size(639, 18);
            this.DeviceSettingsPanelProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.DeviceSettingsPanelProgressBar.TabIndex = 29;
            this.DeviceSettingsPanelProgressBar.Visible = false;
            // 
            // mStatusLabel
            // 
            this.StatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusLabel.Location = new System.Drawing.Point(8, 26);
            this.StatusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.StatusLabel.Name = "mStatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(644, 25);
            this.StatusLabel.TabIndex = 25;
            this.StatusLabel.Text = "Status:";
            // 
            // mPasswordTextBox
            // 
            this.PasswordTextBox.AcceptsReturn = true;
            this.PasswordTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordTextBox.Location = new System.Drawing.Point(307, 310);
            this.PasswordTextBox.MaxLength = 6;
            this.PasswordTextBox.Name = "mPasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(288, 33);
            this.PasswordTextBox.TabIndex = 17;
            this.PasswordTextBox.UseSystemPasswordChar = true;
            this.PasswordTextBox.TextChanged += new System.EventHandler(this.PasswordTextBox_TextChanged);
            this.PasswordTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(PasswordTextBox.PasswordTextBox_KeyPress);
            // 
            // mPasswordLabel
            // 
            this.PasswordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordLabel.Location = new System.Drawing.Point(83, 258);
            this.PasswordLabel.Name = "mPasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(222, 24);
            this.PasswordLabel.TabIndex = 8;
            this.PasswordLabel.Text = "Offline Passcode:";
            this.PasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mClearButton
            // 
            this.ClearButton.Enabled = false;
            this.ClearButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClearButton.Location = new System.Drawing.Point(192, 438);
            this.ClearButton.Name = "mClearButton";
            this.ClearButton.Size = new System.Drawing.Size(160, 50);
            this.ClearButton.TabIndex = 19;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // mSubmitButton
            // 
            this.SubmitButton.Enabled = false;
            this.SubmitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SubmitButton.Location = new System.Drawing.Point(372, 438);
            this.SubmitButton.Name = "mSubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(215, 50);
            this.SubmitButton.TabIndex = 18;
            this.SubmitButton.Text = "Submit";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // mTitleLabel
            // 
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(61, 163);
            this.TitleLabel.Name = "mTitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(410, 37);
            this.TitleLabel.TabIndex = 13;
            this.TitleLabel.Text = "Change Offline Passcode:";
            // 
            // mPasscodeInfoLabel
            // 
            this.PasscodeInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasscodeInfoLabel.Location = new System.Drawing.Point(106, 355);
            this.PasscodeInfoLabel.Name = "mPasscodeInfoLabel";
            this.PasscodeInfoLabel.Size = new System.Drawing.Size(468, 31);
            this.PasscodeInfoLabel.TabIndex = 10;
            this.PasscodeInfoLabel.Text = "(Enter 6 digit passcode for future offline login)";
            // 
            // mReenterPasswordTextBox
            // 
            this.ReenterPasswordTextBox.AcceptsReturn = true;
            this.ReenterPasswordTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReenterPasswordTextBox.Location = new System.Drawing.Point(307, 258);
            this.ReenterPasswordTextBox.MaxLength = 6;
            this.ReenterPasswordTextBox.Name = "mReenterPasswordTextBox";
            this.ReenterPasswordTextBox.Size = new System.Drawing.Size(288, 32);
            this.ReenterPasswordTextBox.TabIndex = 16;
            this.ReenterPasswordTextBox.UseSystemPasswordChar = true;
            this.ReenterPasswordTextBox.TextChanged += new System.EventHandler(this.PasswordTextBox_TextChanged);
            this.ReenterPasswordTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ReenterPasswordTextBox.PasswordTextBox_KeyPress);
            // 
            // mReenterPasswordInfoLabel
            // 
            this.ReenterPasswordInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReenterPasswordInfoLabel.Location = new System.Drawing.Point(83, 310);
            this.ReenterPasswordInfoLabel.Name = "mReenterPasswordInfoLabel";
            this.ReenterPasswordInfoLabel.Size = new System.Drawing.Size(222, 24);
            this.ReenterPasswordInfoLabel.TabIndex = 20;
            this.ReenterPasswordInfoLabel.Text = "Reenter Passcode:";
            this.ReenterPasswordInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mWarningLabel
            // 
            this.WarningLabel.AutoSize = true;
            this.WarningLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WarningLabel.ForeColor = System.Drawing.Color.Red;
            this.WarningLabel.Location = new System.Drawing.Point(175, 386);
            this.WarningLabel.Name = "mWarningLabel";
            this.WarningLabel.Size = new System.Drawing.Size(0, 29);
            this.WarningLabel.TabIndex = 31;
            // 
            // mUserEmailLabel
            // 
            this.UserEmailLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserEmailLabel.Location = new System.Drawing.Point(12, 215);
            this.UserEmailLabel.Name = "mUserEmailLabel";
            this.UserEmailLabel.Size = new System.Drawing.Size(222, 24);
            this.UserEmailLabel.TabIndex = 31;
            this.UserEmailLabel.Text = "Email:";
            this.UserEmailLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // mUserEmailInfoLabel
            // 
            this.UserEmailInfoLabel.AutoSize = true;
            this.UserEmailInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserEmailInfoLabel.Location = new System.Drawing.Point(242, 215);
            this.UserEmailInfoLabel.Name = "mUserEmailInfoLabel";
            this.UserEmailInfoLabel.Size = new System.Drawing.Size(158, 29);
            this.UserEmailInfoLabel.TabIndex = 35;
            this.UserEmailInfoLabel.Text = "label_emailId";
            // 
            // NoRadioButton
            // 
            this.NoRadioButton.AutoSize = true;
            this.NoRadioButton.Checked = true;
            this.NoRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NoRadioButton.ForeColor = System.Drawing.Color.Black;
            this.NoRadioButton.Location = new System.Drawing.Point(582, 165);
            this.NoRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.NoRadioButton.Name = "NoRadioButton";
            this.NoRadioButton.Size = new System.Drawing.Size(60, 29);
            this.NoRadioButton.TabIndex = 14;
            this.NoRadioButton.TabStop = true;
            this.NoRadioButton.Text = "No";
            this.NoRadioButton.UseVisualStyleBackColor = true;
            this.NoRadioButton.CheckedChanged += new System.EventHandler(this.No_CheckedChanged);
            // 
            // YesRadioButton
            // 
            this.YesRadioButton.AutoSize = true;
            this.YesRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YesRadioButton.ForeColor = System.Drawing.Color.Black;
            this.YesRadioButton.Location = new System.Drawing.Point(477, 163);
            this.YesRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.YesRadioButton.Name = "YesRadioButton";
            this.YesRadioButton.Size = new System.Drawing.Size(70, 29);
            this.YesRadioButton.TabIndex = 15;
            this.YesRadioButton.TabStop = true;
            this.YesRadioButton.Text = "Yes";
            this.YesRadioButton.UseVisualStyleBackColor = true;
            this.YesRadioButton.CheckedChanged += new System.EventHandler(this.Yes_CheckedChanged);
            // 
            // ConfigPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.NoRadioButton);
            this.Controls.Add(this.YesRadioButton);
            this.Controls.Add(this.UserEmailInfoLabel);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.TitleLabel);
            this.Controls.Add(this.PasscodeInfoLabel);
            this.Controls.Add(this.ReenterPasswordTextBox);
            this.Controls.Add(this.ReenterPasswordInfoLabel);
            this.Controls.Add(this.WarningLabel);
            this.Controls.Add(this.UserEmailLabel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ConfigPanel";
            this.Size = new System.Drawing.Size(773, 556);
            this.ConfigPanelStatusGroupBox.ResumeLayout(false);
            this.GroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox ConfigPanelStatusGroupBox;
        private System.Windows.Forms.Label ConfigPanelStatusLabel;

        private PasscodeTextBox PasswordTextBox;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Button SubmitButton;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label PasscodeInfoLabel;
        private PasscodeTextBox ReenterPasswordTextBox;
        private System.Windows.Forms.Label ReenterPasswordInfoLabel;
        private System.Windows.Forms.Label WarningLabel;
        private System.Windows.Forms.Label UserEmailLabel;

        private System.Windows.Forms.GroupBox GroupBox1;
        private System.Windows.Forms.ProgressBar DeviceSettingsPanelProgressBar;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Label UserEmailInfoLabel;
        private System.Windows.Forms.RadioButton NoRadioButton;
        private System.Windows.Forms.RadioButton YesRadioButton;
    }
}
