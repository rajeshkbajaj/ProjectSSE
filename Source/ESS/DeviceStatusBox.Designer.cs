namespace Covidien.CGRS.ESS
{
    partial class DeviceStatusBox
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
            this.DeviceSerialNumberLabel = new System.Windows.Forms.Label();
            this.DeviceStatusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mDeviceSerialNumberLabel
            // 
            this.DeviceSerialNumberLabel.AutoSize = true;
            this.DeviceSerialNumberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeviceSerialNumberLabel.Location = new System.Drawing.Point(4, 4);
            this.DeviceSerialNumberLabel.Name = "mDeviceSerialNumberLabel";
            this.DeviceSerialNumberLabel.Size = new System.Drawing.Size(134, 16);
            this.DeviceSerialNumberLabel.TabIndex = 0;
            this.DeviceSerialNumberLabel.Text = "DeviceSerialNumber";
            // 
            // mDeviceStatusLabel
            // 
            this.DeviceStatusLabel.AutoSize = true;
            this.DeviceStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeviceStatusLabel.Location = new System.Drawing.Point(164, 4);
            this.DeviceStatusLabel.Name = "mDeviceStatusLabel";
            this.DeviceStatusLabel.Size = new System.Drawing.Size(43, 16);
            this.DeviceStatusLabel.TabIndex = 1;
            this.DeviceStatusLabel.Text = "status";
            // 
            // DeviceStatusBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DeviceStatusLabel);
            this.Controls.Add(this.DeviceSerialNumberLabel);
            this.Name = "DeviceStatusBox";
            this.Size = new System.Drawing.Size(310, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DeviceSerialNumberLabel;
        private System.Windows.Forms.Label DeviceStatusLabel;
    }
}
