using System;
using System.IO;
using System.Windows.Forms;
using System.Security.Permissions;

namespace Covidien.CGRS.ESS
{
    partial class LogViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogViewer));
            this.WebBrowser1 = new System.Windows.Forms.WebBrowser();
            this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PrintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mWebBrowser
            // 
            this.WebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WebBrowser1.Location = new System.Drawing.Point(0, 24);
            this.WebBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.WebBrowser1.Name = "mWebBrowser";
            this.WebBrowser1.Size = new System.Drawing.Size(784, 538);
            this.WebBrowser1.TabIndex = 0;
            this.WebBrowser1.TabStop = false;
            // 
            // mMenuStrip
            // 
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem});
            this.MenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip1.Name = "mMenuStrip";
            this.MenuStrip1.Size = new System.Drawing.Size(784, 24);
            this.MenuStrip1.TabIndex = 1;
            // 
            // mFileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveAsToolStripMenuItem,
            this.PrintToolStripMenuItem,
            this.CloseToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "mFileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.FileToolStripMenuItem.Text = "&Options";
            // 
            // mSaveAsToolStripMenuItem
            // 
            this.SaveAsToolStripMenuItem.Name = "mSaveAsToolStripMenuItem";
            this.SaveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveAsToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.SaveAsToolStripMenuItem.Text = "Save &As...";
            this.SaveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // mPrintToolStripMenuItem
            // 
            this.PrintToolStripMenuItem.Name = "mPrintToolStripMenuItem";
            this.PrintToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.PrintToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.PrintToolStripMenuItem.Text = "&Print...";
            this.PrintToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // mCloseToolStripMenuItem
            // 
            this.CloseToolStripMenuItem.Name = "mCloseToolStripMenuItem";
            this.CloseToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.CloseToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.CloseToolStripMenuItem.Text = "&Exit";
            this.CloseToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // LogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.WebBrowser1);
            this.Controls.Add(this.MenuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(80, 60);
            this.Name = "LogViewer";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Log Viewer";
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        // Displays the Save dialog box. 
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = "C:\\";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Filter = "HTML files (*.html;*.htm)|*.html;*.htm|All files (*.*)|*.*";
            saveFileDialog.AddExtension = true;
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.FileName = ESS_Main.Instance.GetVentSerialNumber() + "_" + Title + "_" + DateTime.Now.ToString("MMddyyy-HHmmss");

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream stream = saveFileDialog.OpenFile();
                if (stream != null)
                {
                    StreamWriter writer = new StreamWriter(stream);
                    writer.Write(WebBrowser1.DocumentText);
                    writer.Close();
                }
            }

            //File.WriteAllText(path, browser.Document.Body.Parent.OuterHtml, Encoding.GetEncoding(browser.Document.Encoding));
        }

        // Displays the Print dialog box. 
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WebBrowser1.ShowPrintDialog();
        }

        // Displays the close dialog box. 
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //close the form
            this.Close();
        }

        private System.Windows.Forms.WebBrowser WebBrowser1;
        private MenuStrip MenuStrip1;

        private ToolStripMenuItem FileToolStripMenuItem, SaveAsToolStripMenuItem, PrintToolStripMenuItem, CloseToolStripMenuItem; 
    }
}