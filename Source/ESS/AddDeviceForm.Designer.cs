namespace Covidien.CGRS.ESS
{
    partial class AddDeviceForm
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
            this.AddDeviceButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.SerialNumLabel = new System.Windows.Forms.Label();
            this.UserEnteredSerialNumber = new System.Windows.Forms.TextBox();
            this.ModelLabel = new System.Windows.Forms.Label();
            this.ModelSelectionComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // mAddDeviceButton
            // 
            this.AddDeviceButton.Location = new System.Drawing.Point(15, 97);
            this.AddDeviceButton.Name = "mAddDeviceButton";
            this.AddDeviceButton.Size = new System.Drawing.Size(75, 23);
            this.AddDeviceButton.TabIndex = 0;
            this.AddDeviceButton.TabStop = false;
            this.AddDeviceButton.Text = "Add";
            this.AddDeviceButton.UseVisualStyleBackColor = true;
            this.AddDeviceButton.Click += new System.EventHandler(this.AddDeviceButton_Click);
            // 
            // mCloseButton
            // 
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(96, 97);
            this.CloseButton.Name = "mCloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.TabStop = false;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            // 
            // mLabel
            // 
            this.SerialNumLabel.AutoSize = true;
            this.SerialNumLabel.Location = new System.Drawing.Point(12, 51);
            this.SerialNumLabel.Name = "mLabel";
            this.SerialNumLabel.Size = new System.Drawing.Size(110, 13);
            this.SerialNumLabel.TabIndex = 2;
            this.SerialNumLabel.Text = "Device Serial Number";
            // 
            // mUserEnteredSerialNumber
            // 
            this.UserEnteredSerialNumber.Location = new System.Drawing.Point(15, 68);
            this.UserEnteredSerialNumber.Name = "mUserEnteredSerialNumber";
            this.UserEnteredSerialNumber.Size = new System.Drawing.Size(156, 20);
            this.UserEnteredSerialNumber.TabIndex = 3;
            this.UserEnteredSerialNumber.TabStop = false;
            // 
            // mModelLabel
            // 
            this.ModelLabel.AutoSize = true;
            this.ModelLabel.Location = new System.Drawing.Point(15, 4);
            this.ModelLabel.Name = "mModelLabel";
            this.ModelLabel.Size = new System.Drawing.Size(36, 13);
            this.ModelLabel.TabIndex = 4;
            this.ModelLabel.Text = "Model";
            // 
            // mModelSelectionComboBox
            // 
            this.ModelSelectionComboBox.AllowDrop = true;
            this.ModelSelectionComboBox.FormattingEnabled = true;
            this.ModelSelectionComboBox.Location = new System.Drawing.Point(18, 21);
            this.ModelSelectionComboBox.Name = "mModelSelectionComboBox";
            this.ModelSelectionComboBox.Size = new System.Drawing.Size(153, 21);
            this.ModelSelectionComboBox.TabIndex = 5;
            this.ModelSelectionComboBox.TabStop = false;
            // 
            // AddDeviceForm
            // 
            this.AcceptButton = this.AddDeviceButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(188, 130);
            this.ControlBox = false;
            this.Controls.Add(this.ModelSelectionComboBox);
            this.Controls.Add(this.ModelLabel);
            this.Controls.Add(this.UserEnteredSerialNumber);
            this.Controls.Add(this.SerialNumLabel);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.AddDeviceButton);
            this.Name = "AddDeviceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Device";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddDeviceButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Label SerialNumLabel;
        private System.Windows.Forms.TextBox UserEnteredSerialNumber;
        private System.Windows.Forms.Label ModelLabel;
        private System.Windows.Forms.ComboBox ModelSelectionComboBox;
    }
}