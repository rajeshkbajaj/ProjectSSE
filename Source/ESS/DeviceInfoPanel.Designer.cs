namespace Covidien.CGRS.ESS
{
    partial class DeviceInfoPanel
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
            this.DeviceInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.SoftwarePartnumLabel = new System.Windows.Forms.Label();
            this.DeviceKeyTypeLabel = new System.Windows.Forms.Label();
            this.SoftwareVersionLabel = new System.Windows.Forms.Label();
            this.SerialNumLabel = new System.Windows.Forms.Label();
            this.ModelLabel = new System.Windows.Forms.Label();
            this.SkipDeviceConnect = new System.Windows.Forms.Button();
            this.DeviceConnectButton = new System.Windows.Forms.Button();
            this.DevicePictureBox = new System.Windows.Forms.PictureBox();
            this.DeviceInfoGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DevicePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mDeviceInfoGroupBox
            // 
            this.DeviceInfoGroupBox.Controls.Add(this.SoftwarePartnumLabel);
            this.DeviceInfoGroupBox.Controls.Add(this.DeviceKeyTypeLabel);
            this.DeviceInfoGroupBox.Controls.Add(this.SoftwareVersionLabel);
            this.DeviceInfoGroupBox.Controls.Add(this.SerialNumLabel);
            this.DeviceInfoGroupBox.Controls.Add(this.ModelLabel);
            this.DeviceInfoGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeviceInfoGroupBox.Location = new System.Drawing.Point(1, 469);
            this.DeviceInfoGroupBox.Name = "mDeviceInfoGroupBox";
            this.DeviceInfoGroupBox.Size = new System.Drawing.Size(215, 148);
            this.DeviceInfoGroupBox.TabIndex = 10;
            this.DeviceInfoGroupBox.TabStop = false;
            this.DeviceInfoGroupBox.Text = "Device Info";
            // 
            // mSoftwarePartnumLabel
            // 
            this.SoftwarePartnumLabel.AutoSize = true;
            this.SoftwarePartnumLabel.Location = new System.Drawing.Point(1, 73);
            this.SoftwarePartnumLabel.Name = "mSoftwarePartnumLabel";
            this.SoftwarePartnumLabel.Size = new System.Drawing.Size(206, 17);
            this.SoftwarePartnumLabel.TabIndex = 4;
            this.SoftwarePartnumLabel.Text = "SW Part Num: WWWWW99999";
            this.SoftwarePartnumLabel.Visible = false;
            // 
            // mDeviceKeyTypeLabel
            // 
            this.DeviceKeyTypeLabel.AutoSize = true;
            this.DeviceKeyTypeLabel.Location = new System.Drawing.Point(1, 123);
            this.DeviceKeyTypeLabel.Name = "mDeviceKeyTypeLabel";
            this.DeviceKeyTypeLabel.Size = new System.Drawing.Size(143, 17);
            this.DeviceKeyTypeLabel.TabIndex = 3;
            this.DeviceKeyTypeLabel.Text = "DataKey: UNKNOWN";
            this.DeviceKeyTypeLabel.Visible = false;
            // 
            // mSoftwareVersionLabel
            // 
            this.SoftwareVersionLabel.AutoSize = true;
            this.SoftwareVersionLabel.Location = new System.Drawing.Point(3, 98);
            this.SoftwareVersionLabel.Name = "mSoftwareVersionLabel";
            this.SoftwareVersionLabel.Size = new System.Drawing.Size(201, 17);
            this.SoftwareVersionLabel.TabIndex = 2;
            this.SoftwareVersionLabel.Text = "SW Revision: WWWWW99999";
            this.SoftwareVersionLabel.Visible = false;
            // 
            // mSerialNumLabel
            // 
            this.SerialNumLabel.AutoSize = true;
            this.SerialNumLabel.Location = new System.Drawing.Point(1, 49);
            this.SerialNumLabel.Name = "mSerialNumLabel";
            this.SerialNumLabel.Size = new System.Drawing.Size(211, 17);
            this.SerialNumLabel.TabIndex = 1;
            this.SerialNumLabel.Text = "Serial Number: WWWWW99999";
            this.SerialNumLabel.Visible = false;
            // 
            // mModelLabel
            // 
            this.ModelLabel.AutoSize = true;
            this.ModelLabel.Location = new System.Drawing.Point(1, 25);
            this.ModelLabel.Name = "mModelLabel";
            this.ModelLabel.Size = new System.Drawing.Size(100, 17);
            this.ModelLabel.TabIndex = 0;
            this.ModelLabel.Text = "Model: PB 980";
            this.ModelLabel.Visible = false;
            // 
            // SkipDeviceConnect
            // 
            this.SkipDeviceConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SkipDeviceConnect.Location = new System.Drawing.Point(2, 510);
            this.SkipDeviceConnect.Margin = new System.Windows.Forms.Padding(4);
            this.SkipDeviceConnect.Name = "SkipDeviceConnect";
            this.SkipDeviceConnect.Size = new System.Drawing.Size(213, 101);
            this.SkipDeviceConnect.TabIndex = 11;
            this.SkipDeviceConnect.Text = "Skip Device Connection";
            this.SkipDeviceConnect.UseVisualStyleBackColor = true;
            this.SkipDeviceConnect.Click += new System.EventHandler(this.SkipDeviceConnect_Click);
            // mDeviceConnectButton
            // 
            this.DeviceConnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeviceConnectButton.Location = new System.Drawing.Point(3, 31);
            this.DeviceConnectButton.Name = "mDeviceConnectButton";
            this.DeviceConnectButton.Size = new System.Drawing.Size(213, 62);
            this.DeviceConnectButton.TabIndex = 0;
            this.DeviceConnectButton.Text = "Connect";
            this.DeviceConnectButton.UseVisualStyleBackColor = true;
            this.DeviceConnectButton.Click += new System.EventHandler(this.DeviceConnectButton_Click);
            // 
            // mDevicePictureBox
            // 
            this.DevicePictureBox.Image = global::Covidien.CGRS.ESS.Properties.Resources.PB980_faded;
            this.DevicePictureBox.Location = new System.Drawing.Point(3, 119);
            this.DevicePictureBox.Name = "mDevicePictureBox";
            this.DevicePictureBox.Size = new System.Drawing.Size(213, 326);
            this.DevicePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.DevicePictureBox.TabIndex = 5;
            this.DevicePictureBox.TabStop = false;
            // 
            // DeviceInfoPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SkipDeviceConnect);
            this.Controls.Add(this.DeviceInfoGroupBox);
            this.Controls.Add(this.DeviceConnectButton);
            this.Controls.Add(this.DevicePictureBox);
            this.Name = "DeviceInfoPanel";
            this.Size = new System.Drawing.Size(219, 666);
            this.DeviceInfoGroupBox.ResumeLayout(false);
            this.DeviceInfoGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DevicePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox DeviceInfoGroupBox;
        private System.Windows.Forms.Button DeviceConnectButton;
        private System.Windows.Forms.PictureBox DevicePictureBox;
        private System.Windows.Forms.Label SoftwareVersionLabel;
        private System.Windows.Forms.Label SerialNumLabel;
        private System.Windows.Forms.Label ModelLabel;
        private System.Windows.Forms.Label DeviceKeyTypeLabel;
        private System.Windows.Forms.Label SoftwarePartnumLabel;
        private System.Windows.Forms.Button SkipDeviceConnect;
    }
}
