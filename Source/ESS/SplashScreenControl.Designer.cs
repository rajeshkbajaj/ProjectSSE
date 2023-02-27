namespace Covidien.CGRS.ESS
{
    partial class SplashScreenControl
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
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.AuthenticationWaitLabel = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // authenticationWaitLabel
            // 
            this.AuthenticationWaitLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.AuthenticationWaitLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.AuthenticationWaitLabel.Location = new System.Drawing.Point(582, 321);
            this.AuthenticationWaitLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.AuthenticationWaitLabel.Name = "authenticationWaitLabel";
            this.AuthenticationWaitLabel.Size = new System.Drawing.Size(343, 62);
            this.AuthenticationWaitLabel.TabIndex = 0;
            this.AuthenticationWaitLabel.Text = "Authentication process is in progress. Please wait..";
            // 
            // label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("HYHeadLine-Medium", 12.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.Label1.Location = new System.Drawing.Point(579, 281);
            this.Label1.Name = "label1";
            this.Label1.Size = new System.Drawing.Size(79, 19);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "Version: ";
            // 
            // SplashScreenControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Covidien.CGRS.ESS.Properties.Resources.PB980_ESS_splash_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.AuthenticationWaitLabel);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SplashScreenControl";
            this.Size = new System.Drawing.Size(1053, 670);
            this.Load += new System.EventHandler(this.SplashScreenControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer Timer1;
        private System.Windows.Forms.Label AuthenticationWaitLabel;
        private System.Windows.Forms.Label Label1;
    }
}
