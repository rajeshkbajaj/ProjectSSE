namespace Covidien.CGRS.ESS
{
    partial class MainTabControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer Components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (Components != null))
            {
                Components.Dispose();
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
            this.VersionLabel = new System.Windows.Forms.Label();
            this.TabPage4 = new System.Windows.Forms.TabPage();
            this.TabPage3 = new System.Windows.Forms.TabPage();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.MainTabControl1 = new System.Windows.Forms.TabControl();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.MainTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mVersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VersionLabel.Location = new System.Drawing.Point(45, 645);
            this.VersionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.VersionLabel.Name = "mVersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(37, 12);
            this.VersionLabel.TabIndex = 15;
            this.VersionLabel.Text = "Version";
            this.VersionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tabPage4
            // 
            this.TabPage4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabPage4.Location = new System.Drawing.Point(4, 54);
            this.TabPage4.Margin = new System.Windows.Forms.Padding(4);
            this.TabPage4.Name = "tabPage4";
            this.TabPage4.Padding = new System.Windows.Forms.Padding(4);
            this.TabPage4.Size = new System.Drawing.Size(776, 556);
            this.TabPage4.TabIndex = 3;
            this.TabPage4.Text = "Settings";
            this.TabPage4.UseVisualStyleBackColor = true;
            this.TabPage4.Enter += new System.EventHandler(this.TabPage4_Enter);
            // 
            // tabPage3
            // 
            this.TabPage3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabPage3.Location = new System.Drawing.Point(4, 54);
            this.TabPage3.Margin = new System.Windows.Forms.Padding(4);
            this.TabPage3.Name = "tabPage3";
            this.TabPage3.Padding = new System.Windows.Forms.Padding(4);
            this.TabPage3.Size = new System.Drawing.Size(776, 556);
            this.TabPage3.TabIndex = 2;
            this.TabPage3.Text = "Service";
            this.TabPage3.UseVisualStyleBackColor = true;
            this.TabPage3.Enter += new System.EventHandler(this.TabPage3_Enter);
            this.TabPage3.Leave += new System.EventHandler(this.TabPage3_Leave);
            // 
            // tabPage2
            // 
            this.TabPage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabPage2.Location = new System.Drawing.Point(4, 54);
            this.TabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.TabPage2.Name = "tabPage2";
            this.TabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.TabPage2.Size = new System.Drawing.Size(776, 556);
            this.TabPage2.TabIndex = 1;
            this.TabPage2.Text = "Updates";
            this.TabPage2.UseVisualStyleBackColor = true;
            this.TabPage2.Enter += new System.EventHandler(this.TabPage2_Enter);
            // 
            // mMainTabControl
            // 
            this.MainTabControl1.Controls.Add(this.TabPage1);
            this.MainTabControl1.Controls.Add(this.TabPage2);
            this.MainTabControl1.Controls.Add(this.TabPage3);
            this.MainTabControl1.Controls.Add(this.TabPage4);
            this.MainTabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainTabControl1.ItemSize = new System.Drawing.Size(145, 50);
            this.MainTabControl1.Location = new System.Drawing.Point(21, 32);
            this.MainTabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.MainTabControl1.Name = "mMainTabControl";
            this.MainTabControl1.SelectedIndex = 0;
            this.MainTabControl1.Size = new System.Drawing.Size(784, 614);
            this.MainTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.MainTabControl1.TabIndex = 0;
            this.MainTabControl1.SelectedIndexChanged += new System.EventHandler(this.MainTabControl_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.TabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabPage1.Location = new System.Drawing.Point(4, 54);
            this.TabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.TabPage1.Name = "tabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.TabPage1.Size = new System.Drawing.Size(776, 556);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "Logs";
            this.TabPage1.UseVisualStyleBackColor = true;
            this.TabPage1.Enter += new System.EventHandler(this.TabPage1_Enter);
            // 
            // MainTabControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.MainTabControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainTabControl";
            this.Size = new System.Drawing.Size(820, 671);
            this.Load += new System.EventHandler(this.MainTabControl_Load);
            this.MainTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TabControl MainTabControl1;
        private System.Windows.Forms.TabPage TabPage1;
        private System.Windows.Forms.TabPage TabPage2;
        private System.Windows.Forms.TabPage TabPage3;
        private System.Windows.Forms.TabPage TabPage4;
        private System.Windows.Forms.Label VersionLabel;
    }
}

