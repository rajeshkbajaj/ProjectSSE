using System;
using System.ComponentModel;
using System.Windows.Forms;
using PB980Mock.WebServices;
using System.Diagnostics;  

namespace PB980Mock
{
    public partial class PB980Mock : Form
    {
        private WebServer server;

        public PB980Mock()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            server = new WebServer(new LogRichTextBox(this.LogRichTextBox1));
            Closing += FormClosingEventCancle_Closing;

        }

        private void FormClosingEventCancle_Closing(object sender, CancelEventArgs e)
        {
           server.Close();
        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("PB980Mock_DownloadConsole.exe"); 

        }
    }

 
}
