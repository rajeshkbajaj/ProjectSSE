namespace Covidien.CGRS.ESS
{
    partial class DeviceSettingControl
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
            this.SettingLabel = new System.Windows.Forms.Label();
            this.SettingValueTextBox = new System.Windows.Forms.TextBox();
            this.RadioButtonGroupBox = new System.Windows.Forms.GroupBox();
            this.RadioButtonNo = new System.Windows.Forms.RadioButton();
            this.RadioButtonYes = new System.Windows.Forms.RadioButton();
            this.RadioButtonGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mSettingLabel
            // 
            this.SettingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SettingLabel.Location = new System.Drawing.Point(53, 5);
            this.SettingLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SettingLabel.Name = "mSettingLabel";
            this.SettingLabel.Size = new System.Drawing.Size(424, 23);
            this.SettingLabel.TabIndex = 0;
            this.SettingLabel.Text = "Setting Name";
            // 
            // mSettingValueTextBox
            // 
            this.SettingValueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingValueTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SettingValueTextBox.Location = new System.Drawing.Point(485, 1);
            this.SettingValueTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.SettingValueTextBox.Name = "mSettingValueTextBox";
            this.SettingValueTextBox.Size = new System.Drawing.Size(228, 22);
            this.SettingValueTextBox.TabIndex = 1;
            this.SettingValueTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // mRadioButtonGroupBox
            // 
            this.RadioButtonGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RadioButtonGroupBox.Controls.Add(this.RadioButtonNo);
            this.RadioButtonGroupBox.Controls.Add(this.RadioButtonYes);
            this.RadioButtonGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RadioButtonGroupBox.Location = new System.Drawing.Point(563, -7);
            this.RadioButtonGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.RadioButtonGroupBox.Name = "mRadioButtonGroupBox";
            this.RadioButtonGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.RadioButtonGroupBox.Size = new System.Drawing.Size(152, 37);
            this.RadioButtonGroupBox.TabIndex = 2;
            this.RadioButtonGroupBox.TabStop = false;
            this.RadioButtonGroupBox.Visible = false;
            // 
            // mRadioButtonNo
            // 
            this.RadioButtonNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RadioButtonNo.Location = new System.Drawing.Point(91, 11);
            this.RadioButtonNo.Margin = new System.Windows.Forms.Padding(4);
            this.RadioButtonNo.Name = "mRadioButtonNo";
            this.RadioButtonNo.Size = new System.Drawing.Size(65, 23);
            this.RadioButtonNo.TabIndex = 1;
            this.RadioButtonNo.TabStop = true;
            this.RadioButtonNo.Text = "No";
            this.RadioButtonNo.UseVisualStyleBackColor = true;
            this.RadioButtonNo.Visible = false;
            // 
            // mRadioButtonYes
            // 
            this.RadioButtonYes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RadioButtonYes.Location = new System.Drawing.Point(16, 11);
            this.RadioButtonYes.Margin = new System.Windows.Forms.Padding(4);
            this.RadioButtonYes.Name = "mRadioButtonYes";
            this.RadioButtonYes.Size = new System.Drawing.Size(70, 23);
            this.RadioButtonYes.TabIndex = 0;
            this.RadioButtonYes.TabStop = true;
            this.RadioButtonYes.Text = "Yes";
            this.RadioButtonYes.UseVisualStyleBackColor = true;
            this.RadioButtonYes.Visible = false;
            // 
            // DeviceSettingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RadioButtonGroupBox);
            this.Controls.Add(this.SettingValueTextBox);
            this.Controls.Add(this.SettingLabel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DeviceSettingControl";
            this.Size = new System.Drawing.Size(773, 31);
            this.RadioButtonGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SettingLabel;
        public System.Windows.Forms.TextBox SettingValueTextBox;
        protected System.Windows.Forms.GroupBox RadioButtonGroupBox;
        protected System.Windows.Forms.RadioButton RadioButtonNo;
        protected System.Windows.Forms.RadioButton RadioButtonYes;
    }
}
