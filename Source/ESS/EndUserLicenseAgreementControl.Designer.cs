namespace Covidien.CGRS.ESS
{
    partial class EndUserLicenseAgreementControl
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
            this.AgreementHeader = new System.Windows.Forms.Label();
            this.AgreementDescription = new System.Windows.Forms.Label();
            this.AgreementText = new System.Windows.Forms.TextBox();
            this.AcceptCheckBox = new System.Windows.Forms.CheckBox();
            this.ContinueButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mAgreementHeader
            // 
            this.AgreementHeader.AutoSize = true;
            this.AgreementHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AgreementHeader.Location = new System.Drawing.Point(20, 15);
            this.AgreementHeader.Name = "mAgreementHeader";
            this.AgreementHeader.Size = new System.Drawing.Size(313, 25);
            this.AgreementHeader.TabIndex = 0;
            this.AgreementHeader.Text = "Software License Agreement";
            // 
            // mAgreementDescription
            // 
            this.AgreementDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AgreementDescription.Location = new System.Drawing.Point(22, 50);
            this.AgreementDescription.Name = "mAgreementDescription";
            this.AgreementDescription.Size = new System.Drawing.Size(739, 23);
            this.AgreementDescription.TabIndex = 1;
            this.AgreementDescription.Text = "Please read the following License Agreement. You must accept the terms of this ag" +
                "reement to continue to use this software.";
            // 
            // mAgreementText
            // 
            this.AgreementText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AgreementText.Location = new System.Drawing.Point(25, 90);
            this.AgreementText.Multiline = true;
            this.AgreementText.Name = "mAgreementText";
            this.AgreementText.ReadOnly = true;
            this.AgreementText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.AgreementText.Size = new System.Drawing.Size(736, 395);
            this.AgreementText.TabIndex = 2;
            // 
            // mAcceptCheckBox
            // 
            this.AcceptCheckBox.AutoSize = true;
            this.AcceptCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AcceptCheckBox.Location = new System.Drawing.Point(25, 514);
            this.AcceptCheckBox.Name = "mAcceptCheckBox";
            this.AcceptCheckBox.Size = new System.Drawing.Size(163, 20);
            this.AcceptCheckBox.TabIndex = 3;
            this.AcceptCheckBox.Text = "I accept the agreement";
            this.AcceptCheckBox.UseVisualStyleBackColor = true;
            this.AcceptCheckBox.CheckedChanged += new System.EventHandler(this.AcceptCheckBox_CheckedChanged);
            // 
            // mContinueButton
            // 
            this.ContinueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ContinueButton.Location = new System.Drawing.Point(572, 508);
            this.ContinueButton.Name = "mContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(84, 33);
            this.ContinueButton.TabIndex = 4;
            this.ContinueButton.Text = "Continue";
            this.ContinueButton.UseVisualStyleBackColor = true;
            this.ContinueButton.Click += new System.EventHandler(this.ContinueButton_Click);
            // 
            // mCancelButton
            // 
            this.CancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelButton.Location = new System.Drawing.Point(677, 508);
            this.CancelButton.Name = "mCancelButton";
            this.CancelButton.Size = new System.Drawing.Size(84, 33);
            this.CancelButton.TabIndex = 4;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // EndUserLicenseAgreementControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.ContinueButton);
            this.Controls.Add(this.AcceptCheckBox);
            this.Controls.Add(this.AgreementText);
            this.Controls.Add(this.AgreementDescription);
            this.Controls.Add(this.AgreementHeader);
            this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Name = "EndUserLicenseAgreementControl";
            this.Size = new System.Drawing.Size(790, 564);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label AgreementHeader;
        private System.Windows.Forms.Label AgreementDescription;
        private System.Windows.Forms.TextBox AgreementText;
        private System.Windows.Forms.CheckBox AcceptCheckBox;
        private System.Windows.Forms.Button ContinueButton;
        private System.Windows.Forms.Button CancelButton;
    }
}
