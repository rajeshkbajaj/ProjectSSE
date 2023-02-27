namespace Covidien.CGRS.ESS
{
    partial class SetDeviceAccessKeyForm
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
            this.Label1 = new System.Windows.Forms.Label();
            this.AccessKeyTextBox = new System.Windows.Forms.TextBox();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.CancelButton1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(13, 13);
            this.Label1.Name = "label1";
            this.Label1.Size = new System.Drawing.Size(147, 13);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Enter new device access key";
            // 
            // mAccessKeyTextBox
            // 
            this.AccessKeyTextBox.Location = new System.Drawing.Point(13, 30);
            this.AccessKeyTextBox.Name = "mAccessKeyTextBox";
            this.AccessKeyTextBox.Size = new System.Drawing.Size(170, 20);
            this.AccessKeyTextBox.TabIndex = 1;
            this.AccessKeyTextBox.TabStop = false;
            // 
            // mUpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(16, 57);
            this.UpdateButton.Name = "mUpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(75, 23);
            this.UpdateButton.TabIndex = 2;
            this.UpdateButton.TabStop = false;
            this.UpdateButton.Text = "Update";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // mCancelButton
            // 
            this.CancelButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton1.Location = new System.Drawing.Point(98, 56);
            this.CancelButton1.Name = "mCancelButton";
            this.CancelButton1.Size = new System.Drawing.Size(75, 23);
            this.CancelButton1.TabIndex = 3;
            this.CancelButton1.TabStop = false;
            this.CancelButton1.Text = "Cancel";
            this.CancelButton1.UseVisualStyleBackColor = true;
            // 
            // SetDeviceAccessKeyForm
            // 
            this.AcceptButton = this.UpdateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton1;
            this.ClientSize = new System.Drawing.Size(195, 96);
            this.ControlBox = false;
            this.Controls.Add(this.CancelButton1);
            this.Controls.Add(this.UpdateButton);
            this.Controls.Add(this.AccessKeyTextBox);
            this.Controls.Add(this.Label1);
            this.Name = "SetDeviceAccessKeyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Device Access Key";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.TextBox AccessKeyTextBox;
        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.Button CancelButton1;
    }
}