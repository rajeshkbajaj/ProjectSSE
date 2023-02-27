namespace Covidien.CGRS.ESS
{
    partial class ESS_Main
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ESS_Main));
            this.MainPanel1 = new System.Windows.Forms.Panel();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // mMainPanel
            // 
            this.MainPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.MainPanel1.Location = new System.Drawing.Point(0, 0);
            this.MainPanel1.Name = "mMainPanel";
            this.MainPanel1.Size = new System.Drawing.Size(610, 545);
            this.MainPanel1.TabIndex = 2;
            this.MainPanel1.Visible = false;
            // 
            // panel1
            // 
            this.Panel1.Location = new System.Drawing.Point(147, -78);
            this.Panel1.Name = "panel1";
            this.Panel1.Size = new System.Drawing.Size(200, 10);
            this.Panel1.TabIndex = 3;
            // 
            // ESS_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(790, 568);
            this.Controls.Add(this.Panel1);
            this.Controls.Add(this.MainPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ESS_Main";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "PB980 Enhanced Service Software";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.ESS_Main_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MainPanel1;
        private System.Windows.Forms.Panel Panel1;

    }
}