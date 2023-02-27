namespace Covidien.CGRS.ESS
{
    partial class UserTypeControl
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
            this.Components = new System.ComponentModel.Container();
            this.Timer1 = new System.Windows.Forms.Timer(this.Components);
            this.InternalUserButton = new System.Windows.Forms.RadioButton();
            this.ExternalUserButton = new System.Windows.Forms.RadioButton();
            this.UserTypeSelectionLabel = new System.Windows.Forms.Label();
            this.UserTypeSelectionButton = new System.Windows.Forms.Button();
            this.PictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // MedtronicUserButton
            // 
            this.InternalUserButton.AutoSize = true;
            this.InternalUserButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InternalUserButton.ForeColor = System.Drawing.Color.Black;
            this.InternalUserButton.Location = new System.Drawing.Point(239, 336);
            this.InternalUserButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.InternalUserButton.Name = "MedtronicUserButton";
            this.InternalUserButton.Size = new System.Drawing.Size(270, 29);
            this.InternalUserButton.TabIndex = 0;
            this.InternalUserButton.TabStop = true;
            this.InternalUserButton.Text = "Employee / Internal User";
            this.InternalUserButton.UseVisualStyleBackColor = true;
            this.InternalUserButton.CheckedChanged += new System.EventHandler(this.RadioButton1_CheckedChanged);
            // 
            // NonMedtronicUserButton
            // 
            this.ExternalUserButton.AutoSize = true;
            this.ExternalUserButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExternalUserButton.ForeColor = System.Drawing.Color.Black;
            this.ExternalUserButton.Location = new System.Drawing.Point(239, 398);
            this.ExternalUserButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ExternalUserButton.Name = "NonMedtronicUserButton";
            this.ExternalUserButton.Size = new System.Drawing.Size(322, 29);
            this.ExternalUserButton.TabIndex = 1;
            this.ExternalUserButton.TabStop = true;
            this.ExternalUserButton.Text = "Non Employee / External User";
            this.ExternalUserButton.UseVisualStyleBackColor = true;
            this.ExternalUserButton.CheckedChanged += new System.EventHandler(this.RadioButton2_CheckedChanged);
            // 
            // UserTypeSelectionLabel
            // 
            this.UserTypeSelectionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserTypeSelectionLabel.ForeColor = System.Drawing.Color.Black;
            this.UserTypeSelectionLabel.Location = new System.Drawing.Point(159, 267);
            this.UserTypeSelectionLabel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.UserTypeSelectionLabel.Name = "UserTypeSelectionLabel";
            this.UserTypeSelectionLabel.Size = new System.Drawing.Size(507, 34);
            this.UserTypeSelectionLabel.TabIndex = 2;
            this.UserTypeSelectionLabel.Text = "Please Select the Login User Type";
            // 
            // UserTypeSelectionButton
            // 
            this.UserTypeSelectionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserTypeSelectionButton.ForeColor = System.Drawing.Color.Black;
            this.UserTypeSelectionButton.Location = new System.Drawing.Point(435, 491);
            this.UserTypeSelectionButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.UserTypeSelectionButton.Name = "UserTypeSelectionButton";
            this.UserTypeSelectionButton.Size = new System.Drawing.Size(119, 36);
            this.UserTypeSelectionButton.TabIndex = 3;
            this.UserTypeSelectionButton.Text = "Submit";
            this.UserTypeSelectionButton.UseVisualStyleBackColor = true;
            this.UserTypeSelectionButton.Click += new System.EventHandler(this.Button1_Click);
            // 
            // pictureBox2
            // 
            this.PictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PictureBox2.Image = global::Covidien.CGRS.ESS.Properties.Resources.ESS_Branding;
            this.PictureBox2.Location = new System.Drawing.Point(165, 21);
            this.PictureBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PictureBox2.Name = "pictureBox2";
            this.PictureBox2.Size = new System.Drawing.Size(643, 188);
            this.PictureBox2.TabIndex = 14;
            this.PictureBox2.TabStop = false;
            // 
            // UserTypeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.PictureBox2);
            this.Controls.Add(this.UserTypeSelectionButton);
            this.Controls.Add(this.UserTypeSelectionLabel);
            this.Controls.Add(this.ExternalUserButton);
            this.Controls.Add(this.InternalUserButton);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "UserTypeControl";
            this.Size = new System.Drawing.Size(1053, 670);
            this.Load += new System.EventHandler(this.SplashScreenControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer Timer1;
        private System.Windows.Forms.RadioButton InternalUserButton;
        private System.Windows.Forms.RadioButton ExternalUserButton;
        private System.Windows.Forms.Label UserTypeSelectionLabel;
        private System.Windows.Forms.Button UserTypeSelectionButton;
        private System.Windows.Forms.PictureBox PictureBox2;
    }
}
