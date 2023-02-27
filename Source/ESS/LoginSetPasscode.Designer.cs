namespace Covidien.CGRS.ESS
{
    partial class LoginSetPasscode
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
            this.PictureBox2 = new System.Windows.Forms.PictureBox();
            this.PasswordTextBox = new PasscodeTextBox();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.ClearButton = new System.Windows.Forms.Button();
            this.LoginButton = new System.Windows.Forms.Button();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.PasscodeInfoLabel = new System.Windows.Forms.Label();
            this.ReenterPasswordTextBox = new PasscodeTextBox();
            this.ReenterPasswordInfoLabel = new System.Windows.Forms.Label();
            this.WarningLabel = new System.Windows.Forms.Label();
            this.UserEmailLabel = new System.Windows.Forms.Label();
            this.UserEmailValueLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.PictureBox2.Image = global::Covidien.CGRS.ESS.Properties.Resources.ESS_Branding;
            this.PictureBox2.Location = new System.Drawing.Point(98, 14);
            this.PictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.PictureBox2.Name = "pictureBox2";
            this.PictureBox2.Size = new System.Drawing.Size(643, 188);
            this.PictureBox2.TabIndex = 1;
            this.PictureBox2.TabStop = false;
            // 
            // mPasswordTextBox
            // 
            this.PasswordTextBox.AcceptsReturn = true;
            this.PasswordTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordTextBox.Location = new System.Drawing.Point(302, 371);
            this.PasswordTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.PasswordTextBox.MaxLength = 6;
            this.PasswordTextBox.Name = "mPasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(383, 33);
            this.PasswordTextBox.TabIndex = 17;
            this.PasswordTextBox.UseSystemPasswordChar = true;
            this.PasswordTextBox.TextChanged += new System.EventHandler(this.PasswordTextBox_TextChanged);
            this.PasswordTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(PasswordTextBox.PasswordTextBox_KeyPress);
            // 
            // mPasswordLabel
            // 
            this.PasswordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordLabel.Location = new System.Drawing.Point(4, 318);
            this.PasswordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PasswordLabel.Name = "mPasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(293, 30);
            this.PasswordLabel.TabIndex = 8;
            this.PasswordLabel.Text = "Offline Passcode:";
            this.PasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mClearButton
            // 
            this.ClearButton.Enabled = false;
            this.ClearButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClearButton.Location = new System.Drawing.Point(210, 512);
            this.ClearButton.Margin = new System.Windows.Forms.Padding(4);
            this.ClearButton.Name = "mClearButton";
            this.ClearButton.Size = new System.Drawing.Size(213, 62);
            this.ClearButton.TabIndex = 19;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // mLoginButton
            // 
            this.LoginButton.Enabled = false;
            this.LoginButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginButton.Location = new System.Drawing.Point(450, 509);
            this.LoginButton.Margin = new System.Windows.Forms.Padding(4);
            this.LoginButton.Name = "mLoginButton";
            this.LoginButton.Size = new System.Drawing.Size(318, 62);
            this.LoginButton.TabIndex = 18;
            this.LoginButton.Text = "Set";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // mTitleLabel
            // 
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(79, 226);
            this.TitleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TitleLabel.Name = "mTitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(600, 35);
            this.TitleLabel.TabIndex = 13;
            this.TitleLabel.Text = "Set Offline Passcode:";
            // 
            // mPasscodeInfoLabel
            // 
            this.PasscodeInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasscodeInfoLabel.Location = new System.Drawing.Point(248, 415);
            this.PasscodeInfoLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PasscodeInfoLabel.Name = "mPasscodeInfoLabel";
            this.PasscodeInfoLabel.Size = new System.Drawing.Size(480, 25);
            this.PasscodeInfoLabel.TabIndex = 14;
            this.PasscodeInfoLabel.Text = "(Enter 6 digit passcode for future offline login use)";
            // 
            // mReenterPasswordTextBox
            // 
            this.ReenterPasswordTextBox.AcceptsReturn = true;
            this.ReenterPasswordTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReenterPasswordTextBox.Location = new System.Drawing.Point(303, 318);
            this.ReenterPasswordTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.ReenterPasswordTextBox.MaxLength = 6;
            this.ReenterPasswordTextBox.Name = "mReenterPasswordTextBox";
            this.ReenterPasswordTextBox.Size = new System.Drawing.Size(383, 32);
            this.ReenterPasswordTextBox.TabIndex = 16;
            this.ReenterPasswordTextBox.UseSystemPasswordChar = true;
            this.ReenterPasswordTextBox.TextChanged += new System.EventHandler(this.PasswordTextBox_TextChanged);
            this.ReenterPasswordTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ReenterPasswordTextBox.PasswordTextBox_KeyPress);
            // 
            // mReenterPasswordInfoLabel
            // 
            this.ReenterPasswordInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReenterPasswordInfoLabel.Location = new System.Drawing.Point(4, 371);
            this.ReenterPasswordInfoLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ReenterPasswordInfoLabel.Name = "mReenterPasswordInfoLabel";
            this.ReenterPasswordInfoLabel.Size = new System.Drawing.Size(293, 30);
            this.ReenterPasswordInfoLabel.TabIndex = 15;
            this.ReenterPasswordInfoLabel.Text = "Reenter Passcode:";
            this.ReenterPasswordInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mWarningLabel
            // 
            this.WarningLabel.AutoSize = true;
            this.WarningLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WarningLabel.ForeColor = System.Drawing.Color.Red;
            this.WarningLabel.Location = new System.Drawing.Point(265, 469);
            this.WarningLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.WarningLabel.Name = "mWarningLabel";
            this.WarningLabel.Size = new System.Drawing.Size(0, 29);
            this.WarningLabel.TabIndex = 9;
            // 
            // mUserEmailLabel
            // 
            this.UserEmailLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserEmailLabel.Location = new System.Drawing.Point(3, 276);
            this.UserEmailLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UserEmailLabel.Name = "mUserEmailLabel";
            this.UserEmailLabel.Size = new System.Drawing.Size(293, 30);
            this.UserEmailLabel.TabIndex = 25;
            this.UserEmailLabel.Text = "Email:";
            this.UserEmailLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // mUserEmailValueLabel
            // 
            this.UserEmailValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserEmailValueLabel.Location = new System.Drawing.Point(302, 276);
            this.UserEmailValueLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UserEmailValueLabel.Name = "mUserEmailValueLabel";
            this.UserEmailValueLabel.Size = new System.Drawing.Size(427, 30);
            this.UserEmailValueLabel.TabIndex = 26;
            this.UserEmailValueLabel.Text = "email_id";
            // 
            // LoginSetPasscode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UserEmailValueLabel);
            this.Controls.Add(this.UserEmailLabel);
            this.Controls.Add(this.WarningLabel);
            this.Controls.Add(this.ReenterPasswordTextBox);
            this.Controls.Add(this.ReenterPasswordInfoLabel);
            this.Controls.Add(this.PasscodeInfoLabel);
            this.Controls.Add(this.TitleLabel);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.PictureBox2);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LoginSetPasscode";
            this.Size = new System.Drawing.Size(790, 594);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PictureBox2;
        private PasscodeTextBox PasswordTextBox;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label PasscodeInfoLabel;
        private PasscodeTextBox ReenterPasswordTextBox;
        private System.Windows.Forms.Label ReenterPasswordInfoLabel;
        private System.Windows.Forms.Label WarningLabel;
        private System.Windows.Forms.Label UserEmailLabel;
        private System.Windows.Forms.Label UserEmailValueLabel;
    }
}
