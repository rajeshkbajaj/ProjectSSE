namespace Covidien.CGRS.ESS
{
    partial class ViewDocumentDialog
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
            this.ModelSelectionComboBox = new System.Windows.Forms.ComboBox();
            this.ModelLabel = new System.Windows.Forms.Label();
            this.DocumentsLabel = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ViewDocumentButton = new System.Windows.Forms.Button();
            this.DocumentSelectionComboBox = new System.Windows.Forms.ComboBox();
            this.PackageSelectionComboBox = new System.Windows.Forms.ComboBox();
            this.PackageLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mModelSelectionComboBox
            // 
            this.ModelSelectionComboBox.AllowDrop = true;
            this.ModelSelectionComboBox.FormattingEnabled = true;
            this.ModelSelectionComboBox.Location = new System.Drawing.Point(12, 25);
            this.ModelSelectionComboBox.Name = "mModelSelectionComboBox";
            this.ModelSelectionComboBox.Size = new System.Drawing.Size(153, 21);
            this.ModelSelectionComboBox.TabIndex = 11;
            this.ModelSelectionComboBox.TabStop = false;
            this.ModelSelectionComboBox.SelectedIndexChanged += new System.EventHandler(this.ModelSelectionComboBox_SelectedIndexChanged);
            // 
            // mModelLabel
            // 
            this.ModelLabel.AutoSize = true;
            this.ModelLabel.Location = new System.Drawing.Point(9, 8);
            this.ModelLabel.Name = "mModelLabel";
            this.ModelLabel.Size = new System.Drawing.Size(36, 13);
            this.ModelLabel.TabIndex = 10;
            this.ModelLabel.Text = "Model";
            // 
            // mDocumentsLabel
            // 
            this.DocumentsLabel.AutoSize = true;
            this.DocumentsLabel.Location = new System.Drawing.Point(6, 110);
            this.DocumentsLabel.Name = "mDocumentsLabel";
            this.DocumentsLabel.Size = new System.Drawing.Size(61, 13);
            this.DocumentsLabel.TabIndex = 8;
            this.DocumentsLabel.Text = "Documents";
            // 
            // mCloseButton
            // 
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(90, 162);
            this.CloseButton.Name = "mCloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 7;
            this.CloseButton.TabStop = false;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            // 
            // mViewDocumentButton
            // 
            this.ViewDocumentButton.Location = new System.Drawing.Point(9, 162);
            this.ViewDocumentButton.Name = "mViewDocumentButton";
            this.ViewDocumentButton.Size = new System.Drawing.Size(75, 23);
            this.ViewDocumentButton.TabIndex = 6;
            this.ViewDocumentButton.TabStop = false;
            this.ViewDocumentButton.Text = "View";
            this.ViewDocumentButton.UseVisualStyleBackColor = true;
            this.ViewDocumentButton.Click += new System.EventHandler(this.ViewDocumentButton_Click);
            // 
            // mDocumentSelectionComboBox
            // 
            this.DocumentSelectionComboBox.FormattingEnabled = true;
            this.DocumentSelectionComboBox.Location = new System.Drawing.Point(12, 127);
            this.DocumentSelectionComboBox.Name = "mDocumentSelectionComboBox";
            this.DocumentSelectionComboBox.Size = new System.Drawing.Size(153, 21);
            this.DocumentSelectionComboBox.TabIndex = 12;
            this.DocumentSelectionComboBox.TabStop = false;
            // 
            // mPackageSelectionComboBox
            // 
            this.PackageSelectionComboBox.FormattingEnabled = true;
            this.PackageSelectionComboBox.Location = new System.Drawing.Point(12, 74);
            this.PackageSelectionComboBox.Name = "mPackageSelectionComboBox";
            this.PackageSelectionComboBox.Size = new System.Drawing.Size(153, 21);
            this.PackageSelectionComboBox.TabIndex = 14;
            this.PackageSelectionComboBox.TabStop = false;
            this.PackageSelectionComboBox.SelectedIndexChanged += new System.EventHandler(this.PackageSelectionComboBox_SelectedIndexChanged);
            // 
            // mPackageLabel
            // 
            this.PackageLabel.AutoSize = true;
            this.PackageLabel.Location = new System.Drawing.Point(6, 57);
            this.PackageLabel.Name = "mPackageLabel";
            this.PackageLabel.Size = new System.Drawing.Size(101, 13);
            this.PackageLabel.TabIndex = 13;
            this.PackageLabel.Text = "Packages Available";
            // 
            // ViewDocumentDialog
            // 
            this.AcceptButton = this.ViewDocumentButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(178, 197);
            this.ControlBox = false;
            this.Controls.Add(this.PackageSelectionComboBox);
            this.Controls.Add(this.PackageLabel);
            this.Controls.Add(this.DocumentSelectionComboBox);
            this.Controls.Add(this.ModelSelectionComboBox);
            this.Controls.Add(this.ModelLabel);
            this.Controls.Add(this.DocumentsLabel);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ViewDocumentButton);
            this.Name = "ViewDocumentDialog";
            this.Text = "Choose Document Dialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ModelSelectionComboBox;
        private System.Windows.Forms.Label ModelLabel;
        private System.Windows.Forms.Label DocumentsLabel;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button ViewDocumentButton;
        private System.Windows.Forms.ComboBox DocumentSelectionComboBox;
        private System.Windows.Forms.ComboBox PackageSelectionComboBox;
        private System.Windows.Forms.Label PackageLabel;
    }
}