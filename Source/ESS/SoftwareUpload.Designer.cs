namespace Covidien.CGRS.ESS
{
    partial class SoftwareUpload
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
            this.PackageLabel = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.ProgressPanel = new System.Windows.Forms.Panel();
            this.CancelButton = new System.Windows.Forms.Button();
            this.MessageTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // mPackageLabel
            // 
            this.PackageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PackageLabel.ForeColor = System.Drawing.Color.MidnightBlue;
            this.PackageLabel.Location = new System.Drawing.Point(4, 18);
            this.PackageLabel.Name = "mPackageLabel";
            this.PackageLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.PackageLabel.Size = new System.Drawing.Size(568, 18);
            this.PackageLabel.TabIndex = 0;
            this.PackageLabel.Text = "Selected Package: PB980-REV-AUTOBUILD.pkg";
            this.PackageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mStartButton
            // 
            this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.Location = new System.Drawing.Point(189, 65);
            this.StartButton.Name = "mStartButton";
            this.StartButton.Size = new System.Drawing.Size(95, 24);
            this.StartButton.TabIndex = 2;
            this.StartButton.TabStop = false;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // mProgressPanel
            // 
            this.ProgressPanel.AutoScroll = true;
            this.ProgressPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ProgressPanel.Location = new System.Drawing.Point(7, 108);
            this.ProgressPanel.Name = "mProgressPanel";
            this.ProgressPanel.Size = new System.Drawing.Size(559, 243);
            this.ProgressPanel.TabIndex = 3;
            // 
            // mCancelButton
            // 
            this.CancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelButton.Location = new System.Drawing.Point(290, 65);
            this.CancelButton.Name = "mCancelButton";
            this.CancelButton.Size = new System.Drawing.Size(95, 24);
            this.CancelButton.TabIndex = 4;
            this.CancelButton.TabStop = false;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // mMessageTextBox
            // 
            this.MessageTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.MessageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MessageTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MessageTextBox.ForeColor = System.Drawing.Color.DarkRed;
            this.MessageTextBox.Location = new System.Drawing.Point(7, 53);
            this.MessageTextBox.Multiline = true;
            this.MessageTextBox.Name = "mMessageTextBox";
            this.MessageTextBox.ReadOnly = true;
            this.MessageTextBox.Size = new System.Drawing.Size(559, 49);
            this.MessageTextBox.TabIndex = 5;
            this.MessageTextBox.TabStop = false;
            this.MessageTextBox.Text = "Do not disconnect any cables or attempt to close this application while the softw" +
                "are \r\nupload is in progress; interruption to the upload may render the ventilato" +
                "r inoperable.";
            this.MessageTextBox.Visible = false;
            // 
            // SoftwareUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MessageTextBox);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.ProgressPanel);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.PackageLabel);
            this.Name = "SoftwareUpload";
            this.Size = new System.Drawing.Size(574, 424);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label PackageLabel;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Panel ProgressPanel;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.TextBox MessageTextBox;
    }
}