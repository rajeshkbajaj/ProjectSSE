namespace Covidien.CGRS.ESS
{
    partial class LoginRequestControl
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
            this.PasswordTextBox = new PasscodeTextBox();
            this.UserNameTextBox = new System.Windows.Forms.TextBox();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.UserNameLabel = new System.Windows.Forms.Label();
            this.ClearButton = new System.Windows.Forms.Button();
            this.LoginButton = new System.Windows.Forms.Button();
            this.PictureBox2 = new System.Windows.Forms.PictureBox();
            this.LoginErrorLabel = new System.Windows.Forms.Label();
            this.PasscodeInfoLabel = new System.Windows.Forms.Label();
            this.TitleLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // mPasswordTextBox
            // 
            this.PasswordTextBox.AcceptsReturn = true;
            this.PasswordTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordTextBox.Location = new System.Drawing.Point(299, 317);
            this.PasswordTextBox.MaxLength = 6;
            this.PasswordTextBox.Name = "mPasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(288, 29);
            this.PasswordTextBox.TabIndex = 7;
            this.PasswordTextBox.UseSystemPasswordChar = true;
            this.PasswordTextBox.TextChanged += new System.EventHandler(this.PasswordTextBox_TextChanged);
            this.PasswordTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PasswordTextBox_KeyDown);
            this.PasswordTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(PasswordTextBox.PasswordTextBox_KeyPress);

            // 
            // mUserNameTextBox
            // 
            this.UserNameTextBox.AcceptsReturn = true;
            this.UserNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserNameTextBox.Location = new System.Drawing.Point(299, 259);
            this.UserNameTextBox.Name = "mUserNameTextBox";
            this.UserNameTextBox.Size = new System.Drawing.Size(288, 29);
            this.UserNameTextBox.TabIndex = 6;
            this.UserNameTextBox.TextChanged += new System.EventHandler(this.UserNameTextBox_TextChanged);
            this.UserNameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UserNameTextBox_KeyDown);
            // 
            // mPasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordLabel.Location = new System.Drawing.Point(130, 317);
            this.PasswordLabel.Name = "mPasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(156, 24);
            this.PasswordLabel.TabIndex = 5;
            this.PasswordLabel.Text = "Offline Passcode:";
            // 
            // mUserNameLabel
            // 
            this.UserNameLabel.AutoSize = true;
            this.UserNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserNameLabel.Location = new System.Drawing.Point(130, 262);
            this.UserNameLabel.Name = "mUserNameLabel";
            this.UserNameLabel.Size = new System.Drawing.Size(62, 24);
            this.UserNameLabel.TabIndex = 4;
            this.UserNameLabel.Text = "Email:";
            // 
            // mClearButton
            // 
            this.ClearButton.Enabled = false;
            this.ClearButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClearButton.Location = new System.Drawing.Point(192, 438);
            this.ClearButton.Name = "mClearButton";
            this.ClearButton.Size = new System.Drawing.Size(160, 50);
            this.ClearButton.TabIndex = 10;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // mLoginButton
            // 
            this.LoginButton.Enabled = false;
            this.LoginButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginButton.Location = new System.Drawing.Point(372, 438);
            this.LoginButton.Name = "mLoginButton";
            this.LoginButton.Size = new System.Drawing.Size(215, 50);
            this.LoginButton.TabIndex = 9;
            this.LoginButton.Text = "Login";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // pictureBox2
            // 
            this.PictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PictureBox2.Image = global::Covidien.CGRS.ESS.Properties.Resources.ESS_Branding;
            this.PictureBox2.Location = new System.Drawing.Point(152, 54);
            this.PictureBox2.Name = "pictureBox2";
            this.PictureBox2.Size = new System.Drawing.Size(482, 153);
            this.PictureBox2.TabIndex = 13;
            this.PictureBox2.TabStop = false;
            // 
            // mLoginErrorLabel
            // 
            this.LoginErrorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginErrorLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LoginErrorLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LoginErrorLabel.Location = new System.Drawing.Point(19, 383);
            this.LoginErrorLabel.Name = "mLoginErrorLabel";
            this.LoginErrorLabel.Size = new System.Drawing.Size(733, 18);
            this.LoginErrorLabel.TabIndex = 11;
            this.LoginErrorLabel.Text = "Login error message";
            this.LoginErrorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mPasscodeInfoLabel
            // 
            this.PasscodeInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasscodeInfoLabel.Location = new System.Drawing.Point(128, 343);
            this.PasscodeInfoLabel.Name = "mPasscodeInfoLabel";
            this.PasscodeInfoLabel.Size = new System.Drawing.Size(130, 20);
            this.PasscodeInfoLabel.TabIndex = 15;
            this.PasscodeInfoLabel.Text = "(6 digit code )";
            // 
            // mTitleLabel
            // 
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(82, 218);
            this.TitleLabel.Name = "mTitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(450, 28);
            this.TitleLabel.TabIndex = 16;
            this.TitleLabel.Text = "Login Using Offline Passcode";
            // 
            // LoginRequestControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TitleLabel);
            this.Controls.Add(this.PasscodeInfoLabel);
            this.Controls.Add(this.PictureBox2);
            this.Controls.Add(this.LoginErrorLabel);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.UserNameTextBox);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.UserNameLabel);
            this.MinimumSize = new System.Drawing.Size(790, 544);
            this.Name = "LoginRequestControl";
            this.Size = new System.Drawing.Size(790, 544);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PasscodeTextBox PasswordTextBox;
        private System.Windows.Forms.TextBox UserNameTextBox;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.Label UserNameLabel;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.PictureBox PictureBox2;
        private System.Windows.Forms.Label LoginErrorLabel;
        private System.Windows.Forms.Label PasscodeInfoLabel;
        private System.Windows.Forms.Label TitleLabel;
    }
}
