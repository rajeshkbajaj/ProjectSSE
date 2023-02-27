namespace Covidien.CGRS.VTS
{
    partial class ResetDeviceHoursForm
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
            this.mUpdateButton = new System.Windows.Forms.Button();
            this.mCancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mUpdateButton
            // 
            this.mUpdateButton.Location = new System.Drawing.Point(133, 12);
            this.mUpdateButton.Name = "mUpdateButton";
            this.mUpdateButton.Size = new System.Drawing.Size(75, 23);
            this.mUpdateButton.TabIndex = 0;
            this.mUpdateButton.TabStop = false;
            this.mUpdateButton.Text = "Update";
            this.mUpdateButton.UseVisualStyleBackColor = true;
            this.mUpdateButton.Click += new System.EventHandler(this.mUpdateButton_Click);
            // 
            // mCancelButton
            // 
            this.mCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.mCancelButton.Location = new System.Drawing.Point(214, 12);
            this.mCancelButton.Name = "mCancelButton";
            this.mCancelButton.Size = new System.Drawing.Size(75, 23);
            this.mCancelButton.TabIndex = 1;
            this.mCancelButton.TabStop = false;
            this.mCancelButton.Text = "Cancel";
            this.mCancelButton.UseVisualStyleBackColor = true;
            // 
            // ResetDeviceHoursForm
            // 
            this.AcceptButton = this.mUpdateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.mCancelButton;
            this.ClientSize = new System.Drawing.Size(422, 47);
            this.ControlBox = false;
            this.Controls.Add(this.mCancelButton);
            this.Controls.Add(this.mUpdateButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ResetDeviceHoursForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reset Device Timers";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button mUpdateButton;
        private System.Windows.Forms.Button mCancelButton;
    }
}