namespace Covidien.CGRS.ESS
{
    partial class DeviceTypeStatusBox
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
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.DocumentationPackageStatusLabel = new System.Windows.Forms.Label();
            this.DocumentationPackageLabel = new System.Windows.Forms.Label();
            this.SoftwarePackageStatusLabel = new System.Windows.Forms.Label();
            this.SoftwarePackageLabel = new System.Windows.Forms.Label();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mGroupBox
            // 
            this.GroupBox1.Controls.Add(this.DocumentationPackageStatusLabel);
            this.GroupBox1.Controls.Add(this.DocumentationPackageLabel);
            this.GroupBox1.Controls.Add(this.SoftwarePackageStatusLabel);
            this.GroupBox1.Controls.Add(this.SoftwarePackageLabel);
            this.GroupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.GroupBox1.Location = new System.Drawing.Point(4, 4);
            this.GroupBox1.Name = "mGroupBox";
            this.GroupBox1.Size = new System.Drawing.Size(128, 104);
            this.GroupBox1.TabIndex = 0;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "PB980_VENTILATOR";
            // 
            // mDocumentationPackageStatusLabel
            // 
            this.DocumentationPackageStatusLabel.AutoSize = true;
            this.DocumentationPackageStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DocumentationPackageStatusLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.DocumentationPackageStatusLabel.Location = new System.Drawing.Point(19, 78);
            this.DocumentationPackageStatusLabel.Name = "mDocumentationPackageStatusLabel";
            this.DocumentationPackageStatusLabel.Size = new System.Drawing.Size(39, 15);
            this.DocumentationPackageStatusLabel.TabIndex = 3;
            this.DocumentationPackageStatusLabel.Text = "status";
            // 
            // mDocumentationPackageLabel
            // 
            this.DocumentationPackageLabel.AutoSize = true;
            this.DocumentationPackageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DocumentationPackageLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.DocumentationPackageLabel.Location = new System.Drawing.Point(7, 61);
            this.DocumentationPackageLabel.Name = "mDocumentationPackageLabel";
            this.DocumentationPackageLabel.Size = new System.Drawing.Size(98, 16);
            this.DocumentationPackageLabel.TabIndex = 2;
            this.DocumentationPackageLabel.Text = "Documentation";
            // 
            // mSoftwarePackageStatusLabel
            // 
            this.SoftwarePackageStatusLabel.AutoSize = true;
            this.SoftwarePackageStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SoftwarePackageStatusLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.SoftwarePackageStatusLabel.Location = new System.Drawing.Point(19, 37);
            this.SoftwarePackageStatusLabel.Name = "mSoftwarePackageStatusLabel";
            this.SoftwarePackageStatusLabel.Size = new System.Drawing.Size(39, 15);
            this.SoftwarePackageStatusLabel.TabIndex = 1;
            this.SoftwarePackageStatusLabel.Text = "status";
            // 
            // mSoftwarePackageLabel
            // 
            this.SoftwarePackageLabel.AutoSize = true;
            this.SoftwarePackageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SoftwarePackageLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.SoftwarePackageLabel.Location = new System.Drawing.Point(7, 20);
            this.SoftwarePackageLabel.Name = "mSoftwarePackageLabel";
            this.SoftwarePackageLabel.Size = new System.Drawing.Size(118, 16);
            this.SoftwarePackageLabel.TabIndex = 0;
            this.SoftwarePackageLabel.Text = "Software Package";
            // 
            // DeviceTypeStatusBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GroupBox1);
            this.Name = "DeviceTypeStatusBox";
            this.Size = new System.Drawing.Size(135, 114);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GroupBox1;
        private System.Windows.Forms.Label DocumentationPackageStatusLabel;
        private System.Windows.Forms.Label DocumentationPackageLabel;
        private System.Windows.Forms.Label SoftwarePackageStatusLabel;
        private System.Windows.Forms.Label SoftwarePackageLabel;
    }
}
