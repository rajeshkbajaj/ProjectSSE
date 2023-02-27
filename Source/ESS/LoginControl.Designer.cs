namespace Covidien.CGRS.ESS
{
    partial class LoginControl
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
            this.UserNameLabel = new System.Windows.Forms.Label();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.UserNameTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.LoginButton = new System.Windows.Forms.Button();
            this.CancelButton1 = new System.Windows.Forms.Button();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.PictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // mUserNameLabel
            // 
            this.UserNameLabel.AutoSize = true;
            this.UserNameLabel.Location = new System.Drawing.Point(29, 122);
            this.UserNameLabel.Name = "mUserNameLabel";
            this.UserNameLabel.Size = new System.Drawing.Size(60, 13);
            this.UserNameLabel.TabIndex = 0;
            this.UserNameLabel.Text = "User Name";
            // 
            // mPasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Location = new System.Drawing.Point(32, 161);
            this.PasswordLabel.Name = "mPasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(53, 13);
            this.PasswordLabel.TabIndex = 1;
            this.PasswordLabel.Text = "Password";
            // 
            // mUserNameTextBox
            // 
            this.UserNameTextBox.AcceptsReturn = true;
            this.UserNameTextBox.Location = new System.Drawing.Point(96, 114);
            this.UserNameTextBox.Name = "mUserNameTextBox";
            this.UserNameTextBox.Size = new System.Drawing.Size(159, 20);
            this.UserNameTextBox.TabIndex = 2;
            this.UserNameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UserNameTextBox_KeyDown);
            // 
            // mPasswordTextBox
            // 
            this.PasswordTextBox.AcceptsReturn = true;
            this.PasswordTextBox.Location = new System.Drawing.Point(96, 158);
            this.PasswordTextBox.Name = "mPasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(159, 20);
            this.PasswordTextBox.TabIndex = 3;
            this.PasswordTextBox.UseSystemPasswordChar = true;
            this.PasswordTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PasswordTextBox_KeyDown);
            // 
            // mLoginButton
            // 
            this.LoginButton.Location = new System.Drawing.Point(57, 197);
            this.LoginButton.Name = "mLoginButton";
            this.LoginButton.Size = new System.Drawing.Size(75, 23);
            this.LoginButton.TabIndex = 4;
            this.LoginButton.Text = "Login";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // mCancelButton
            // 
            this.CancelButton1.Location = new System.Drawing.Point(166, 196);
            this.CancelButton1.Name = "mCancelButton";
            this.CancelButton1.Size = new System.Drawing.Size(75, 23);
            this.CancelButton1.TabIndex = 5;
            this.CancelButton1.Text = "Cancel";
            this.CancelButton1.UseVisualStyleBackColor = true;
            this.CancelButton1.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // mMessageLabel
            // 
            this.MessageLabel.ForeColor = System.Drawing.Color.DarkBlue;
            this.MessageLabel.Location = new System.Drawing.Point(35, 81);
            this.MessageLabel.Name = "mMessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(220, 23);
            this.MessageLabel.TabIndex = 6;
            this.MessageLabel.Text = "Message Text";
            this.MessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PictureBox1.Image = global::Covidien.CGRS.ESS.Properties.Resources.logo_covidien;
            this.PictureBox1.Location = new System.Drawing.Point(35, 11);
            this.PictureBox1.Name = "pictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(72, 52);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBox1.TabIndex = 7;
            this.PictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.PictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PictureBox2.Image = global::Covidien.CGRS.ESS.Properties.Resources.VTS_Branding;
            this.PictureBox2.Location = new System.Drawing.Point(113, 11);
            this.PictureBox2.Name = "pictureBox2";
            this.PictureBox2.Size = new System.Drawing.Size(142, 50);
            this.PictureBox2.TabIndex = 8;
            this.PictureBox2.TabStop = false;
            // 
            // LoginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 236);
            this.ControlBox = false;
            this.Controls.Add(this.PictureBox2);
            this.Controls.Add(this.PictureBox1);
            this.Controls.Add(this.MessageLabel);
            this.Controls.Add(this.CancelButton1);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.UserNameTextBox);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.UserNameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "LoginControl";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Authentication required";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label UserNameLabel;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.TextBox UserNameTextBox;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.Button CancelButton1;
        private System.Windows.Forms.Label MessageLabel;
        private System.Windows.Forms.PictureBox PictureBox1;
        private System.Windows.Forms.PictureBox PictureBox2;
    }
}
