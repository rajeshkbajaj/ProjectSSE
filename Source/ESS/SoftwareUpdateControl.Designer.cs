namespace Covidien.CGRS.ESS
{
    partial class SoftwareUpdateControl
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
            this.SelectPackageLabel = new System.Windows.Forms.Label();
            this.PackageSelectionComboBox = new System.Windows.Forms.ComboBox();
            this.ViewDocumentButton = new System.Windows.Forms.Button();
            this.FlashDeviceButton = new System.Windows.Forms.Button();
            this.SelectDocumentComboBox = new System.Windows.Forms.ComboBox();
            this.SelectDocumentPackageLabel = new System.Windows.Forms.Label();
            this.SoftwareUpdateControlStatusGroupBox = new System.Windows.Forms.GroupBox();
            this.SoftwareDownLoadProgressBar = new System.Windows.Forms.ProgressBar();
            this.SoftwareDownloadStatusLabel = new System.Windows.Forms.Label();
            this.LanguageSelectionComboBox = new System.Windows.Forms.ComboBox();
            this.LanguagesLabel = new System.Windows.Forms.Label();
            this.SoftwareUpdateControlStatusGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mSelectPackageLabel
            // 
            this.SelectPackageLabel.AutoSize = true;
            this.SelectPackageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectPackageLabel.Location = new System.Drawing.Point(235, 164);
            this.SelectPackageLabel.Name = "mSelectPackageLabel";
            this.SelectPackageLabel.Size = new System.Drawing.Size(79, 20);
            this.SelectPackageLabel.TabIndex = 0;
            this.SelectPackageLabel.Text = "Packages";
            // 
            // mPackageSelectionComboBox
            // 
            this.PackageSelectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PackageSelectionComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PackageSelectionComboBox.FormattingEnabled = true;
            this.PackageSelectionComboBox.Location = new System.Drawing.Point(239, 198);
            this.PackageSelectionComboBox.Name = "mPackageSelectionComboBox";
            this.PackageSelectionComboBox.Size = new System.Drawing.Size(199, 28);
            this.PackageSelectionComboBox.TabIndex = 1;
            this.PackageSelectionComboBox.TabStop = false;
            this.PackageSelectionComboBox.SelectedIndexChanged += new System.EventHandler(this.PackageSelectionComboBox_SelectedIndexChanged);
            this.PackageSelectionComboBox.Click += new System.EventHandler(this.PackageSelectionComboBox_Click);
            // 
            // mViewDocumentButton
            // 
            this.ViewDocumentButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ViewDocumentButton.Location = new System.Drawing.Point(449, 322);
            this.ViewDocumentButton.Name = "mViewDocumentButton";
            this.ViewDocumentButton.Size = new System.Drawing.Size(84, 40);
            this.ViewDocumentButton.TabIndex = 2;
            this.ViewDocumentButton.TabStop = false;
            this.ViewDocumentButton.Text = "View";
            this.ViewDocumentButton.UseVisualStyleBackColor = true;
            this.ViewDocumentButton.Enabled = false;
            // 
            // mFlashDeviceButton
            // 
            this.FlashDeviceButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FlashDeviceButton.Location = new System.Drawing.Point(449, 190);
            this.FlashDeviceButton.Name = "mFlashDeviceButton";
            this.FlashDeviceButton.Size = new System.Drawing.Size(105, 40);
            this.FlashDeviceButton.TabIndex = 3;
            this.FlashDeviceButton.TabStop = false;
            this.FlashDeviceButton.Text = "Proceed";
            this.FlashDeviceButton.UseVisualStyleBackColor = true;
            this.FlashDeviceButton.Click += new System.EventHandler(this.FlashDeviceButton_Click);
            // 
            // mSelectDocumentComboBox
            // 
            this.SelectDocumentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectDocumentComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectDocumentComboBox.FormattingEnabled = true;
            this.SelectDocumentComboBox.Location = new System.Drawing.Point(45, 330);
            this.SelectDocumentComboBox.Name = "mSelectDocumentComboBox";
            this.SelectDocumentComboBox.Size = new System.Drawing.Size(393, 28);
            this.SelectDocumentComboBox.TabIndex = 7;
            this.SelectDocumentComboBox.TabStop = false;
            this.SelectDocumentComboBox.Enabled = false;
            // 
            // mSelectDocumentPackageLabel
            // 
            this.SelectDocumentPackageLabel.AutoSize = true;
            this.SelectDocumentPackageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectDocumentPackageLabel.Location = new System.Drawing.Point(41, 295);
            this.SelectDocumentPackageLabel.Name = "mSelectDocumentPackageLabel";
            this.SelectDocumentPackageLabel.Size = new System.Drawing.Size(136, 20);
            this.SelectDocumentPackageLabel.TabIndex = 6;
            this.SelectDocumentPackageLabel.Text = "Select Document:";
            // 
            // mSoftwareUpdateControlStatusGroupBox
            // 
            this.SoftwareUpdateControlStatusGroupBox.Controls.Add(this.SoftwareDownLoadProgressBar);
            this.SoftwareUpdateControlStatusGroupBox.Controls.Add(this.SoftwareDownloadStatusLabel);
            this.SoftwareUpdateControlStatusGroupBox.Location = new System.Drawing.Point(41, 17);
            this.SoftwareUpdateControlStatusGroupBox.Name = "mSoftwareUpdateControlStatusGroupBox";
            this.SoftwareUpdateControlStatusGroupBox.Size = new System.Drawing.Size(495, 76);
            this.SoftwareUpdateControlStatusGroupBox.TabIndex = 33;
            this.SoftwareUpdateControlStatusGroupBox.TabStop = false;
            this.SoftwareUpdateControlStatusGroupBox.Text = "Status";
            // 
            // mSoftwareDownLoadProgressBar
            // 
            this.SoftwareDownLoadProgressBar.Location = new System.Drawing.Point(10, 45);
            this.SoftwareDownLoadProgressBar.Name = "mSoftwareDownLoadProgressBar";
            this.SoftwareDownLoadProgressBar.Size = new System.Drawing.Size(479, 15);
            this.SoftwareDownLoadProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.SoftwareDownLoadProgressBar.TabIndex = 28;
            this.SoftwareDownLoadProgressBar.Visible = false;
            // 
            // mSoftwareDownloadStatusLabel
            // 
            this.SoftwareDownloadStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SoftwareDownloadStatusLabel.Location = new System.Drawing.Point(6, 21);
            this.SoftwareDownloadStatusLabel.Name = "mSoftwareDownloadStatusLabel";
            this.SoftwareDownloadStatusLabel.Size = new System.Drawing.Size(483, 25);
            this.SoftwareDownloadStatusLabel.TabIndex = 25;
            this.SoftwareDownloadStatusLabel.Text = "Status:";
            this.SoftwareDownloadStatusLabel.Visible = false;
            // 
            // mLanguageSelectionComboBox
            // 
            this.LanguageSelectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LanguageSelectionComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LanguageSelectionComboBox.FormattingEnabled = true;
            this.LanguageSelectionComboBox.Location = new System.Drawing.Point(41, 198);
            this.LanguageSelectionComboBox.Name = "mLanguageSelectionComboBox";
            this.LanguageSelectionComboBox.Size = new System.Drawing.Size(183, 28);
            this.LanguageSelectionComboBox.TabIndex = 34;
            this.LanguageSelectionComboBox.TabStop = false;
            this.LanguageSelectionComboBox.SelectedIndexChanged += new System.EventHandler(this.LanguageSelectionComboBox_SelectedIndexChanged);
            // 
            // mLanguagesLabel
            // 
            this.LanguagesLabel.AutoSize = true;
            this.LanguagesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LanguagesLabel.Location = new System.Drawing.Point(37, 164);
            this.LanguagesLabel.Name = "mLanguagesLabel";
            this.LanguagesLabel.Size = new System.Drawing.Size(89, 20);
            this.LanguagesLabel.TabIndex = 35;
            this.LanguagesLabel.Text = "Languages";
            // 
            // SoftwareUpdateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LanguagesLabel);
            this.Controls.Add(this.LanguageSelectionComboBox);
            this.Controls.Add(this.SoftwareUpdateControlStatusGroupBox);
            this.Controls.Add(this.SelectDocumentComboBox);
            this.Controls.Add(this.SelectDocumentPackageLabel);
            this.Controls.Add(this.FlashDeviceButton);
            this.Controls.Add(this.ViewDocumentButton);
            this.Controls.Add(this.PackageSelectionComboBox);
            this.Controls.Add(this.SelectPackageLabel);
            this.Name = "SoftwareUpdateControl";
            this.Size = new System.Drawing.Size(580, 452);
            this.SoftwareUpdateControlStatusGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SelectPackageLabel;
        private System.Windows.Forms.ComboBox PackageSelectionComboBox;
        private System.Windows.Forms.Button ViewDocumentButton;
        private System.Windows.Forms.Button FlashDeviceButton;
        private System.Windows.Forms.ComboBox SelectDocumentComboBox;
        private System.Windows.Forms.Label SelectDocumentPackageLabel;
        private System.Windows.Forms.GroupBox SoftwareUpdateControlStatusGroupBox;
        private System.Windows.Forms.ProgressBar SoftwareDownLoadProgressBar;
        private System.Windows.Forms.Label SoftwareDownloadStatusLabel;
        private System.Windows.Forms.ComboBox LanguageSelectionComboBox;
        private System.Windows.Forms.Label LanguagesLabel;
    }
}
