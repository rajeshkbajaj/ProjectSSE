namespace Covidien.CGRS.ESS
{
    partial class FlashProgressControl
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
            this.ComponentName = new System.Windows.Forms.Label();
            this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.ActivityLabel1 = new System.Windows.Forms.Label();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.ActivityLabel2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mComponentName
            // 
            this.ComponentName.AutoSize = true;
            this.ComponentName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComponentName.Location = new System.Drawing.Point(10, 10);
            this.ComponentName.Name = "mComponentName";
            this.ComponentName.Size = new System.Drawing.Size(108, 15);
            this.ComponentName.TabIndex = 0;
            this.ComponentName.Text = "Component Name";
            // 
            // mProgressBar
            // 
            this.ProgressBar1.Location = new System.Drawing.Point(13, 37);
            this.ProgressBar1.Name = "mProgressBar";
            this.ProgressBar1.Size = new System.Drawing.Size(471, 23);
            this.ProgressBar1.TabIndex = 1;
            // 
            // mActivityLabel1
            // 
            this.ActivityLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActivityLabel1.Location = new System.Drawing.Point(217, 10);
            this.ActivityLabel1.Name = "mActivityLabel1";
            this.ActivityLabel1.Size = new System.Drawing.Size(267, 23);
            this.ActivityLabel1.TabIndex = 2;
            this.ActivityLabel1.Text = "Activity Description";
            this.ActivityLabel1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Panel1.Location = new System.Drawing.Point(16, 98);
            this.Panel1.Name = "panel1";
            this.Panel1.Size = new System.Drawing.Size(468, 2);
            this.Panel1.TabIndex = 3;
            // 
            // mActivityLabel2
            // 
            this.ActivityLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActivityLabel2.Location = new System.Drawing.Point(16, 72);
            this.ActivityLabel2.Name = "mActivityLabel2";
            this.ActivityLabel2.Size = new System.Drawing.Size(468, 18);
            this.ActivityLabel2.TabIndex = 4;
            this.ActivityLabel2.Text = "Activity Description";
            this.ActivityLabel2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FlashProgressControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ActivityLabel2);
            this.Controls.Add(this.Panel1);
            this.Controls.Add(this.ActivityLabel1);
            this.Controls.Add(this.ProgressBar1);
            this.Controls.Add(this.ComponentName);
            this.Name = "FlashProgressControl";
            this.Size = new System.Drawing.Size(500, 103);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label ComponentName;
        private System.Windows.Forms.ProgressBar ProgressBar1;
        private System.Windows.Forms.Label ActivityLabel1;
        private System.Windows.Forms.Panel Panel1;
        private System.Windows.Forms.Label ActivityLabel2;
    }
}
