namespace Covidien.CGRS.ESS
{
    partial class DeviceSerialNumberControl
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
            this.SubmitButton = new System.Windows.Forms.Button();
            this.EnterSerialNumberTextBox = new System.Windows.Forms.TextBox();
            this.EnterDeviceSerialLabel = new System.Windows.Forms.Label();
            this.ReEnterSerialNumberTextBox = new System.Windows.Forms.TextBox();
            this.ReEnterDeviceSerialLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mSubmitButton
            // 
            this.SubmitButton.Enabled = false;
            this.SubmitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SubmitButton.Location = new System.Drawing.Point(149, 343);
            this.SubmitButton.Name = "mSubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(288, 50);
            this.SubmitButton.TabIndex = 11;
            this.SubmitButton.Text = "Submit";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // mEnterSerialNumberTextBox
            // 
            this.EnterSerialNumberTextBox.AcceptsReturn = true;
            this.EnterSerialNumberTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnterSerialNumberTextBox.Location = new System.Drawing.Point(149, 151);
            this.EnterSerialNumberTextBox.Name = "mEnterSerialNumberTextBox";
            this.EnterSerialNumberTextBox.Size = new System.Drawing.Size(288, 29);
            this.EnterSerialNumberTextBox.TabIndex = 10;
            this.EnterSerialNumberTextBox.TextChanged += new System.EventHandler(this.EnterSerialNumberTextBox_TextChanged);
            // 
            // mEnterDeviceSerialLabel
            // 
            this.EnterDeviceSerialLabel.AutoSize = true;
            this.EnterDeviceSerialLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnterDeviceSerialLabel.Location = new System.Drawing.Point(145, 106);
            this.EnterDeviceSerialLabel.Name = "mEnterDeviceSerialLabel";
            this.EnterDeviceSerialLabel.Size = new System.Drawing.Size(244, 24);
            this.EnterDeviceSerialLabel.TabIndex = 9;
            this.EnterDeviceSerialLabel.Text = "Enter Device Serial Number";
            // 
            // mReEnterSerialNumberTextBox
            // 
            this.ReEnterSerialNumberTextBox.AcceptsReturn = true;
            this.ReEnterSerialNumberTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReEnterSerialNumberTextBox.Location = new System.Drawing.Point(149, 253);
            this.ReEnterSerialNumberTextBox.Name = "mReEnterSerialNumberTextBox";
            this.ReEnterSerialNumberTextBox.Size = new System.Drawing.Size(288, 29);
            this.ReEnterSerialNumberTextBox.TabIndex = 13;
            this.ReEnterSerialNumberTextBox.TextChanged += new System.EventHandler(this.ReEnterSerialNumberTextBox_TextChanged);
            // 
            // mReEnterDeviceSerialLabel
            // 
            this.ReEnterDeviceSerialLabel.AutoSize = true;
            this.ReEnterDeviceSerialLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReEnterDeviceSerialLabel.Location = new System.Drawing.Point(145, 208);
            this.ReEnterDeviceSerialLabel.Name = "mReEnterDeviceSerialLabel";
            this.ReEnterDeviceSerialLabel.Size = new System.Drawing.Size(274, 24);
            this.ReEnterDeviceSerialLabel.TabIndex = 12;
            this.ReEnterDeviceSerialLabel.Text = "Re-Enter Device Serial Number";
            // 
            // DeviceSerialNumberControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ReEnterSerialNumberTextBox);
            this.Controls.Add(this.ReEnterDeviceSerialLabel);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.EnterSerialNumberTextBox);
            this.Controls.Add(this.EnterDeviceSerialLabel);
            this.Name = "DeviceSerialNumberControl";
            this.Size = new System.Drawing.Size(578, 508);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SubmitButton;
        private System.Windows.Forms.TextBox EnterSerialNumberTextBox;
        private System.Windows.Forms.Label EnterDeviceSerialLabel;
        private System.Windows.Forms.TextBox ReEnterSerialNumberTextBox;
        private System.Windows.Forms.Label ReEnterDeviceSerialLabel;
    }
}
