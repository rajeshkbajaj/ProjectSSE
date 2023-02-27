namespace Covidien.CGRS.ESS
{
    partial class DeviceCountryControl
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
            this.DeviceCountryComboBox = new System.Windows.Forms.ComboBox();
            this.DeviceCountryLabel = new System.Windows.Forms.Label();
            this.DeviceCountrySubmitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mDeviceCountryComboBox
            // 
            this.DeviceCountryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DeviceCountryComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeviceCountryComboBox.FormattingEnabled = true;
            this.DeviceCountryComboBox.Location = new System.Drawing.Point(234, 255);
            this.DeviceCountryComboBox.Name = "mDeviceCountryComboBox";
            this.DeviceCountryComboBox.Size = new System.Drawing.Size(348, 33);
            this.DeviceCountryComboBox.TabIndex = 9;
            this.DeviceCountryComboBox.SelectedIndexChanged += new System.EventHandler(DeviceCountryComboBox_SelectedIndexChanged);
            // 
            // mDeviceCountryLabel
            // 
            this.DeviceCountryLabel.AutoSize = true;
            this.DeviceCountryLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeviceCountryLabel.Location = new System.Drawing.Point(230, 208);
            this.DeviceCountryLabel.Name = "mDeviceCountryLabel";
            this.DeviceCountryLabel.Size = new System.Drawing.Size(195, 24);
            this.DeviceCountryLabel.TabIndex = 10;
            this.DeviceCountryLabel.Text = "Select Device Country";
            // 
            // mDeviceCountrySubmitButton
            // 
            this.DeviceCountrySubmitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeviceCountrySubmitButton.Location = new System.Drawing.Point(408, 404);
            this.DeviceCountrySubmitButton.Name = "mDeviceCountrySubmitButton";
            this.DeviceCountrySubmitButton.Size = new System.Drawing.Size(174, 50);
            this.DeviceCountrySubmitButton.TabIndex = 11;
            this.DeviceCountrySubmitButton.Text = "Submit";
            this.DeviceCountrySubmitButton.UseVisualStyleBackColor = true;
            this.DeviceCountrySubmitButton.Click += new System.EventHandler(DeviceCountrySubmitButton_Click);
            // 
            // DeviceCountryControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DeviceCountrySubmitButton);
            this.Controls.Add(this.DeviceCountryLabel);
            this.Controls.Add(this.DeviceCountryComboBox);
            this.Name = "DeviceCountryControl";
            this.Size = new System.Drawing.Size(790, 544);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox DeviceCountryComboBox;
        private System.Windows.Forms.Label DeviceCountryLabel;
        private System.Windows.Forms.Button DeviceCountrySubmitButton;

    }
}
