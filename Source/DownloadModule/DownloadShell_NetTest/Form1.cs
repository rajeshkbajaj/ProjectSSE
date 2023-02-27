using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DownloadShell_Net;

namespace DownloadShell_NetTest
{
    public partial class Form1 : Form
    {
        DownloadShell_Net.DownloadShell_Net downloadShell_Net;


        public delegate void SimpleDelegate(String i,int j);

        public void AppendToTextbox(string s, int i)
        {
            richTextBox1.AppendText(s);

        }

        public void AppendToTextbox(string s)
        {
            this.BeginInvoke(new SimpleDelegate(AppendToTextbox), s,1);

        }


        void OnDownloadComplete(string status)
        {
            AppendToTextbox("OnDownloadComplete(\""+status+"\")\n");
        }

        void OnStatusComponentUpdate(string cpu, string component, string message, int percentComplete, bool stillGood)
        {
            AppendToTextbox("OnStatusComponentUpdate(\"" + cpu + "\",\"" + component + "\",\"" + message + "\"," + percentComplete + ","+"true"+")\n");
        }

        void OnStartComponentUpdate(string cpu, string component, string message)
        {
            AppendToTextbox("OnStartComponentUpdate(\""+cpu+"\",\""+component+"\",\""+message+"\")\n");
        }

        void OnStartDownload()
        {
            AppendToTextbox("OnStartDownload()\n");
        }

        private void FormClosingEventCancle_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            downloadShell_Net.Dispose(); //Must be called prior to destructor.
            System.Windows.Forms.Application.Exit();
        }

        public Form1()
        {
            InitializeComponent();
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FormClosingEventCancle_Closing);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            downloadShell_Net = new DownloadShell_Net.DownloadShell_Net("C:\\VikingTFTPRoot2");
            downloadShell_Net.OnDownloadComplete        += new DownloadShell_Net.OnDownloadCompleteDelegate(OnDownloadComplete);
            downloadShell_Net.OnOnStatusComponentAction += new DownloadShell_Net.OnStatusComponentUpdateDelegate(OnStatusComponentUpdate);
            downloadShell_Net.OnStartComponentAction    += new DownloadShell_Net.OnStartComponentUpdateDelegate(OnStartComponentUpdate);
            downloadShell_Net.OnStartDownload           += new DownloadShell_Net.OnStartDownloadDelegate(OnStartDownload);

            downloadShell_Net.Start();
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
