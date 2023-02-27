namespace Covidien.CGRS.ESS
{
    partial class DeviceConnectInstructions
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
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.Timer1.Interval = 5000;
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // mPictureBox
            // 
            this.PictureBox1.BackgroundImage = global::Covidien.CGRS.ESS.Properties.Resources._980_connectivity_5;
            this.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PictureBox1.Location = new System.Drawing.Point(4, 102);
            this.PictureBox1.Name = "mPictureBox";
            this.PictureBox1.Size = new System.Drawing.Size(576, 406);
            this.PictureBox1.TabIndex = 0;
            this.PictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.ForeColor = System.Drawing.Color.Black;
            this.Label1.Location = new System.Drawing.Point(5, 0);
            this.Label1.MaximumSize = new System.Drawing.Size(220, 0);
            this.Label1.Name = "label1";
            this.Label1.Size = new System.Drawing.Size(220, 24);
            this.Label1.TabIndex = 10;
            this.Label1.Text = "Connection Instructions:                 ";
            this.Label1.UseCompatibleTextRendering = true;
            // 
            // mVersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VersionLabel.Location = new System.Drawing.Point(19, 508);
            this.VersionLabel.Name = "mVersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(37, 12);
            this.VersionLabel.TabIndex = 14;
            this.VersionLabel.Text = "Version";
            this.VersionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.ForeColor = System.Drawing.Color.Black;
            this.Label2.Location = new System.Drawing.Point(1, 24);
            this.Label2.Name = "label2";
            this.Label2.Size = new System.Drawing.Size(598, 16);
            this.Label2.TabIndex = 15;
            this.Label2.Text = "1. PC Network must be configured as IP:192.168.0.10 SUBNET:255.255.0.0";
            // 
            // label3
            // 
            this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.ForeColor = System.Drawing.Color.Black;
            this.Label3.Location = new System.Drawing.Point(1, 48);
            this.Label3.Name = "label3";
            this.Label3.Size = new System.Drawing.Size(598, 16);
            this.Label3.TabIndex = 16;
            this.Label3.Text = "2. Connect ethernet cable from PC network interface to back of ventilator (see pic)";
            // 
            // label4
            // 
            this.Label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.ForeColor = System.Drawing.Color.Black;
            this.Label4.Location = new System.Drawing.Point(1, 72);
            this.Label4.Name = "label4";
            this.Label4.Size = new System.Drawing.Size(598, 16);
            this.Label4.TabIndex = 17;
            this.Label4.Text = "3. Ensure PB980 is in Service Mode and click the Connect Button";
            
            // 
            // DeviceConnectInstructions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.PictureBox1);
            this.DoubleBuffered = true;
            this.Location = new System.Drawing.Point(15, 16);
            this.Name = "DeviceConnectInstructions";
            this.Size = new System.Drawing.Size(582, 520);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer Timer1;
        private System.Windows.Forms.PictureBox PictureBox1;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.Label Label3;
        private System.Windows.Forms.Label Label4;
    }
}
