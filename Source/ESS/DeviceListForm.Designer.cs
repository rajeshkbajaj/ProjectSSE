namespace Covidien.CGRS.ESS
{
    partial class DeviceListForm
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
            this.components = new System.ComponentModel.Container();
            this.AddDeviceButton = new System.Windows.Forms.Button();
            this.ViewDocumentButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.DeviceTypesGroupBox = new System.Windows.Forms.GroupBox();
            this.DeviceTypePanel = new System.Windows.Forms.Panel();
            this.IndividualDevicesGroupBox = new System.Windows.Forms.GroupBox();
            this.DevicePanel = new System.Windows.Forms.Panel();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.DeviceTypesGroupBox.SuspendLayout();
            this.IndividualDevicesGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mAddDeviceButton
            // 
            this.AddDeviceButton.Location = new System.Drawing.Point(12, 12);
            this.AddDeviceButton.Name = "mAddDeviceButton";
            this.AddDeviceButton.Size = new System.Drawing.Size(75, 23);
            this.AddDeviceButton.TabIndex = 2;
            this.AddDeviceButton.Text = "Add Device";
            this.AddDeviceButton.UseVisualStyleBackColor = true;
            this.AddDeviceButton.Click += new System.EventHandler(this.AddDeviceButton_Click);
            // 
            // mViewDocumentButton
            // 
            this.ViewDocumentButton.Location = new System.Drawing.Point(94, 12);
            this.ViewDocumentButton.Name = "mViewDocumentButton";
            this.ViewDocumentButton.Size = new System.Drawing.Size(75, 23);
            this.ViewDocumentButton.TabIndex = 1;
            this.ViewDocumentButton.Text = "View";
            this.ViewDocumentButton.UseVisualStyleBackColor = true;
            this.ViewDocumentButton.Click += new System.EventHandler(this.ViewDocumentButton_Click);
            // 
            // mCloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(176, 13);
            this.CloseButton.Name = "mCloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 0;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // mDeviceTypesGroupBox
            // 
            this.DeviceTypesGroupBox.Controls.Add(this.DeviceTypePanel);
            this.DeviceTypesGroupBox.Location = new System.Drawing.Point(12, 42);
            this.DeviceTypesGroupBox.Name = "mDeviceTypesGroupBox";
            this.DeviceTypesGroupBox.Size = new System.Drawing.Size(157, 290);
            this.DeviceTypesGroupBox.TabIndex = 4;
            this.DeviceTypesGroupBox.TabStop = false;
            this.DeviceTypesGroupBox.Text = "Device Types";
            // 
            // mDeviceTypePanel
            // 
            this.DeviceTypePanel.AutoScroll = true;
            this.DeviceTypePanel.Location = new System.Drawing.Point(7, 20);
            this.DeviceTypePanel.Name = "mDeviceTypePanel";
            this.DeviceTypePanel.Size = new System.Drawing.Size(144, 264);
            this.DeviceTypePanel.TabIndex = 0;
            // 
            // mIndividualDevicesGroupBox
            // 
            this.IndividualDevicesGroupBox.Controls.Add(this.DevicePanel);
            this.IndividualDevicesGroupBox.Location = new System.Drawing.Point(176, 42);
            this.IndividualDevicesGroupBox.Name = "mIndividualDevicesGroupBox";
            this.IndividualDevicesGroupBox.Size = new System.Drawing.Size(351, 290);
            this.IndividualDevicesGroupBox.TabIndex = 5;
            this.IndividualDevicesGroupBox.TabStop = false;
            this.IndividualDevicesGroupBox.Text = "Individual Devices";
            // 
            // mDevicePanel
            // 
            this.DevicePanel.AutoScroll = true;
            this.DevicePanel.Location = new System.Drawing.Point(7, 20);
            this.DevicePanel.Name = "mDevicePanel";
            this.DevicePanel.Size = new System.Drawing.Size(338, 264);
            this.DevicePanel.TabIndex = 0;
            // 
            // timer1
            // 
            this.Timer1.Interval = 1000;
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // DeviceListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(539, 344);
            this.ControlBox = false;
            this.Controls.Add(this.IndividualDevicesGroupBox);
            this.Controls.Add(this.DeviceTypesGroupBox);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ViewDocumentButton);
            this.Controls.Add(this.AddDeviceButton);
            this.Name = "DeviceListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Device Work List";
            this.DeviceTypesGroupBox.ResumeLayout(false);
            this.IndividualDevicesGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AddDeviceButton;
        private System.Windows.Forms.Button ViewDocumentButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.GroupBox DeviceTypesGroupBox;
        private System.Windows.Forms.Panel DeviceTypePanel;
        private System.Windows.Forms.GroupBox IndividualDevicesGroupBox;
        private System.Windows.Forms.Panel DevicePanel;
        private System.Windows.Forms.Timer Timer1;
    }
}