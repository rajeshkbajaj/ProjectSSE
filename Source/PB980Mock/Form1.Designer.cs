namespace PB980Mock
{
    partial class PB980Mock
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
            this.LogGroupBox1 = new System.Windows.Forms.GroupBox();
            this.LogRichTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.LogGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LogGroupBox1
            // 
            this.LogGroupBox1.Controls.Add(this.button1);
            this.LogGroupBox1.Controls.Add(this.LogRichTextBox1);
            this.LogGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.LogGroupBox1.Name = "LogGroupBox1";
            this.LogGroupBox1.Size = new System.Drawing.Size(958, 519);
            this.LogGroupBox1.TabIndex = 0;
            this.LogGroupBox1.TabStop = false;
            this.LogGroupBox1.Text = "Log";
            this.LogGroupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // LogRichTextBox1
            // 
            this.LogRichTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LogRichTextBox1.Location = new System.Drawing.Point(48, 20);
            this.LogRichTextBox1.Name = "LogRichTextBox1";
            this.LogRichTextBox1.Size = new System.Drawing.Size(767, 493);
            this.LogRichTextBox1.TabIndex = 0;
            this.LogRichTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(850, 84);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Download";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // PB980Mock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 543);
            this.Controls.Add(this.LogGroupBox1);
            this.Name = "PB980Mock";
            this.Text = "PB980Mock";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.LogGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox LogGroupBox1;
        private System.Windows.Forms.RichTextBox LogRichTextBox1;
        private System.Windows.Forms.Button button1;
    }
}

