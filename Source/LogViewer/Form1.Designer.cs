namespace LogViewer
{
    partial class Form1
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
            this.filename = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.btnLoadLogDefns = new System.Windows.Forms.Button();
            this.comboLogNames = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboRenderNames = new System.Windows.Forms.ComboBox();
            this.btnRecvLog = new System.Windows.Forms.Button();
            this.btnTestPdf = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // filename
            // 
            this.filename.Location = new System.Drawing.Point(150, 40);
            this.filename.Name = "filename";
            this.filename.Size = new System.Drawing.Size(443, 20);
            this.filename.TabIndex = 0;
            this.filename.Text = "C:/Logs/CGRS-logs/diag/sysdiag.dat";
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(555, 64);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(1, 113);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(1079, 359);
            this.webBrowser1.TabIndex = 2;
            // 
            // btnLoadLogDefns
            // 
            this.btnLoadLogDefns.Location = new System.Drawing.Point(13, 38);
            this.btnLoadLogDefns.Name = "btnLoadLogDefns";
            this.btnLoadLogDefns.Size = new System.Drawing.Size(92, 23);
            this.btnLoadLogDefns.TabIndex = 3;
            this.btnLoadLogDefns.Text = "Load Log Defns";
            this.btnLoadLogDefns.UseVisualStyleBackColor = true;
            this.btnLoadLogDefns.Click += new System.EventHandler(this.btnLoadLogDefns_Click);
            // 
            // comboLogNames
            // 
            this.comboLogNames.FormattingEnabled = true;
            this.comboLogNames.Location = new System.Drawing.Point(209, 66);
            this.comboLogNames.Name = "comboLogNames";
            this.comboLogNames.Size = new System.Drawing.Size(121, 21);
            this.comboLogNames.TabIndex = 4;
            this.comboLogNames.SelectedIndexChanged += new System.EventHandler(this.comboLogNames_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(147, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Log Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(348, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Render Style";
            // 
            // comboRenderNames
            // 
            this.comboRenderNames.FormattingEnabled = true;
            this.comboRenderNames.Location = new System.Drawing.Point(422, 66);
            this.comboRenderNames.Name = "comboRenderNames";
            this.comboRenderNames.Size = new System.Drawing.Size(121, 21);
            this.comboRenderNames.TabIndex = 7;
            // 
            // btnRecvLog
            // 
            this.btnRecvLog.Location = new System.Drawing.Point(638, 38);
            this.btnRecvLog.Name = "btnRecvLog";
            this.btnRecvLog.Size = new System.Drawing.Size(75, 23);
            this.btnRecvLog.TabIndex = 8;
            this.btnRecvLog.Text = "Recv Log";
            this.btnRecvLog.UseVisualStyleBackColor = true;
            this.btnRecvLog.Click += new System.EventHandler(this.btnRecvLog_Click);
            // 
            // btnTestPdf
            // 
            this.btnTestPdf.Location = new System.Drawing.Point(864, 38);
            this.btnTestPdf.Name = "btnTestPdf";
            this.btnTestPdf.Size = new System.Drawing.Size(75, 23);
            this.btnTestPdf.TabIndex = 9;
            this.btnTestPdf.Text = "Test PDF";
            this.btnTestPdf.UseVisualStyleBackColor = true;
            this.btnTestPdf.Click += new System.EventHandler(this.btnTestPdf_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1078, 471);
            this.Controls.Add(this.btnTestPdf);
            this.Controls.Add(this.btnRecvLog);
            this.Controls.Add(this.comboRenderNames);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboLogNames);
            this.Controls.Add(this.btnLoadLogDefns);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.filename);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox filename;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button btnLoadLogDefns;
        private System.Windows.Forms.ComboBox comboLogNames;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboRenderNames;
        private System.Windows.Forms.Button btnRecvLog;
        private System.Windows.Forms.Button btnTestPdf;
    }
}

