namespace Covidien.CGRS.ESS
{
    partial class LogControl
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
            this.LogNameLabel = new System.Windows.Forms.Label();
            this.ViewLogButton = new System.Windows.Forms.Button();
            this.SaveLogButton = new System.Windows.Forms.Button();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.UploadLogButton = new System.Windows.Forms.Button();
            this.UploadStatusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mLogNameLabel
            // 
            this.LogNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogNameLabel.Location = new System.Drawing.Point(13, 4);
            this.LogNameLabel.Name = "mLogNameLabel";
            this.LogNameLabel.Size = new System.Drawing.Size(230, 23);
            this.LogNameLabel.TabIndex = 0;
            this.LogNameLabel.Text = "Unique Log Name";
            this.LogNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mViewLogButton
            // 
            this.ViewLogButton.Enabled = false;
            this.ViewLogButton.Location = new System.Drawing.Point(256, 3);
            this.ViewLogButton.Name = "mViewLogButton";
            this.ViewLogButton.Size = new System.Drawing.Size(75, 23);
            this.ViewLogButton.TabIndex = 2;
            this.ViewLogButton.TabStop = false;
            this.ViewLogButton.Text = "View Log";
            this.ViewLogButton.UseVisualStyleBackColor = true;
            this.ViewLogButton.Click += new System.EventHandler(this.ViewLogButton_Click);
            // 
            // mSaveLogButton
            // 
            this.SaveLogButton.Enabled = false;
            this.SaveLogButton.Location = new System.Drawing.Point(333, 3);
            this.SaveLogButton.Name = "mSaveLogButton";
            this.SaveLogButton.Size = new System.Drawing.Size(75, 23);
            this.SaveLogButton.TabIndex = 3;
            this.SaveLogButton.TabStop = false;
            this.SaveLogButton.Text = "Save to File";
            this.SaveLogButton.UseVisualStyleBackColor = true;
            this.SaveLogButton.Click += new System.EventHandler(this.SaveLogButton_Click);
            // 
            // panel1
            // 
            this.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Panel1.Location = new System.Drawing.Point(15, 33);
            this.Panel1.Margin = new System.Windows.Forms.Padding(0);
            this.Panel1.Name = "panel1";
            this.Panel1.Size = new System.Drawing.Size(470, 2);
            this.Panel1.TabIndex = 4;
            // 
            // mUploadLogButton
            // 
            this.UploadLogButton.Enabled = false;
            this.UploadLogButton.Location = new System.Drawing.Point(410, 3);
            this.UploadLogButton.Name = "mUploadLogButton";
            this.UploadLogButton.Size = new System.Drawing.Size(75, 23);
            this.UploadLogButton.TabIndex = 5;
            this.UploadLogButton.TabStop = false;
            this.UploadLogButton.Text = "Upload Log";
            this.UploadLogButton.UseVisualStyleBackColor = true;
            this.UploadLogButton.Click += new System.EventHandler(this.UploadLogButton_Click);
            // 
            // mUploadStatusLabel
            // 
            this.UploadStatusLabel.AutoSize = true;
            this.UploadStatusLabel.Location = new System.Drawing.Point(417, 9);
            this.UploadStatusLabel.Name = "mUploadStatusLabel";
            this.UploadStatusLabel.Size = new System.Drawing.Size(53, 13);
            this.UploadStatusLabel.TabIndex = 6;
            this.UploadStatusLabel.Text = "Uploaded";
            this.UploadStatusLabel.Visible = false;
            // 
            // LogControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UploadStatusLabel);
            this.Controls.Add(this.UploadLogButton);
            this.Controls.Add(this.Panel1);
            this.Controls.Add(this.SaveLogButton);
            this.Controls.Add(this.ViewLogButton);
            this.Controls.Add(this.LogNameLabel);
            this.Name = "LogControl";
            this.Size = new System.Drawing.Size(500, 36);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LogNameLabel;
        private System.Windows.Forms.Button ViewLogButton;
        private System.Windows.Forms.Button SaveLogButton;
        private System.Windows.Forms.Panel Panel1;
        private System.Windows.Forms.Button UploadLogButton;
        private System.Windows.Forms.Label UploadStatusLabel;
    }
}
