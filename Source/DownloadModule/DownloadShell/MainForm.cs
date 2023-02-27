using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Timers;

namespace SoftwareDownloadConsole
{



    public partial class Form1 : Form
    {
        private System.Timers.Timer m_flashMessageTimer;





        private void BarcodeTimerElapsedFlashCallback(object sender, ElapsedEventArgs e)
        {

                m_flashMessageTimer.Stop();

                label1.Text = "";
                    label1.Hide();

        }





        private void DisplayText(string msg, double fontSize)
        {

            m_flashMessageTimer.Stop();

            double screenWidth = (double)Screen.PrimaryScreen.Bounds.Right - (double)Screen.PrimaryScreen.Bounds.Left;

            float scale = (float)(((double)Bounds.Right - (double)Bounds.Left) / screenWidth);

            label1.Font = new Font("Microsoft Sans Serif", Math.Max((float)fontSize * scale, 12), FontStyle.Regular);

            label1.Text = msg;

            double centerRL = (Math.Abs((double)(this.richTextBox1.Bounds.Right - this.richTextBox1.Bounds.Left)) - label1.Width) / 2.0;
            double centerTB = (Math.Abs((double)(this.richTextBox1.Bounds.Top - this.richTextBox1.Bounds.Bottom)) - label1.Height) / 2.0;

  
            label1.Left = (int)(centerRL);
            label1.Top = (int)centerTB;


            label1.Show();

            m_flashMessageTimer.Start();
        }



        bool bRunning = true;
        bool bFinished = false;


        public Form1()
        {
            InitializeComponent();

            this.downloadProgressGUI.Text = "GUI";
            this.downloadProgressBD.Text = "BD";

            myThread = new System.Threading.Thread(new
                    System.Threading.ThreadStart(myStartingMethod));

            myThread.Start();
        }


        System.Threading.Thread myThread;



      //public delegate void SimpleDelegate2(String i);


        public void OnStart(string i)
        {
            this.BeginInvoke(new SimpleDelegate2(OnStart_), i);

        }


        private void OnStart_(String str)
        {

            this.LogMessage(str+".............................................");
            DisplayText(str, 100);
            this.downloadProgressGUI.UpdateProgress(0);
            this.downloadProgressGUI.UpdateLabel1("");
            this.downloadProgressGUI.UpdateLabel2("");

            this.downloadProgressBD.UpdateProgress(0);
            this.downloadProgressBD.UpdateLabel1("");
            this.downloadProgressBD.UpdateLabel2("");
            //this.richTextBox1.Clear();

        }



        public void OnFinished(string i)
        {
            this.BeginInvoke(new SimpleDelegate2(OnFinished_), i);

        }


        private void OnFinished_(String str)
        {
            this.LogMessage(str);
            DisplayText(str, 100);
            this.downloadProgressGUI.UpdateProgress(100);
            this.downloadProgressGUI.UpdateLabel1(str);
            this.downloadProgressGUI.UpdateLabel2("done...");

            this.downloadProgressBD.UpdateProgress(100);
            this.downloadProgressBD.UpdateLabel1(str);
            this.downloadProgressBD.UpdateLabel2("done...");

            button4.ForeColor = Color.Red;
            button4.BackColor = Color.Black;
            button4.Text = "Stopped";
            baseobj.stopDownloadServer();
        }

 
 





        public delegate void SimpleDelegate2(String i);

        void LogMessage(String str)
        {

            string[] lines = richTextBox1.Text.Split(Environment.NewLine.ToCharArray());

            if (lines.Length > 1000)
            {
                int a_line = 0;
                int start_index = richTextBox1.GetFirstCharIndexFromLine(a_line);
                int count = richTextBox1.Lines[a_line].Length;

                // Eat new line chars
                if (a_line < richTextBox1.Lines.Length - 1)
                {
                    count += richTextBox1.GetFirstCharIndexFromLine(a_line + 1) -
                        ((start_index + count - 1) + 1);
                }

                richTextBox1.Text = richTextBox1.Text.Remove(start_index, count);
            }

            this.richTextBox1.AppendText(str + Environment.NewLine);
            this.richTextBox1.Update();

        }


        public static   int callee(ref object obj,string sn, int n)
        {

            ((Form1)obj).BeginInvoke(new SoftwareDownloadConsole.Form1.SimpleDelegate2(((Form1)obj).LogMessage), "testing");
            //System.Console.WriteLine("Managed: " + str);
            return 0;
        }

        VikingConsoleDriver baseobj;

        void myStartingMethod()
        {

            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);


            baseobj = new VikingConsoleDriver("C:\\VikingTFTPRoot");           //This function must be called on start.
            baseobj.setDelay(0);
            baseobj.reset();


            while (bRunning)
            {

                VikingConsoleToken token;

                if ((token = baseobj.getNextToken()) !=  null)
                {
                    int nElements = token.getLength();

                    if (nElements == 0) continue;

                    string str = token.getValue(0);

                    try
                    {
                        if (token.getValue(0).Equals("REMOTEFILEIO"))
                        {
                            if (token.getValue(2).Equals("ERROR") && nElements >3)
                            {
                                this.BeginInvoke(new SimpleDelegate2(LogMessage), token.getValue(3));
                            }
                        }

                        else if (token.getValue(0).Equals("GUI"))
                        {
                            if (token.getValue(1).Equals("SET") && nElements > 3)
                            {
                                if (token.getValue(2).Equals("PROGRESS"))
                                {
                                    this.downloadProgressGUI.UpdateProgress(Int32.Parse(token.getValue(3)));
                                }
                                else if (token.getValue(2).Equals("LABEL_2"))
                                {
                                    this.downloadProgressGUI.UpdateLabel1(token.getValue(3));
                                }
                                else if (token.getValue(2).Equals("LABEL_1"))
                                {
                                    this.downloadProgressGUI.UpdateLabel2(token.getValue(3));
                                }
                            }
                            else if (token.getValue(1).Equals("APPEND") && token.getValue(2).Equals("TEXTBOX_1") && nElements > 3)
                            {
                                this.BeginInvoke(new SimpleDelegate2(LogMessage), "<GUI> "+token.getValue(3));
                            }
                            else if (token.getValue(1).Equals("FINISH"))
                            {
                                OnFinished("Download Complete");
                            }
                            else if (token.getValue(1).Equals("START"))
                            {
                                OnStart("Download Started");
                            }

                        }

                        else if (token.getValue(0).Equals("BD"))
                        {
                            if (token.getValue(1).Equals("SET") && nElements > 3)
                            {

                                if (token.getValue(2).Equals("PROGRESS"))
                                {
                                    this.downloadProgressBD.UpdateProgress(Int32.Parse(token.getValue(3)));
                                }
                                else if (token.getValue(2).Equals("LABEL_2"))
                                {
                                    this.downloadProgressBD.UpdateLabel1(token.getValue(3));
                                }
                                else if (token.getValue(2).Equals("LABEL_1"))
                                {
                                    this.downloadProgressBD.UpdateLabel2(token.getValue(3));
                                }
                            }
                            else if (token.getValue(1).Equals("APPEND") && token.getValue(2).Equals("TEXTBOX_1") && nElements > 3)
                            {
                                this.BeginInvoke(new SimpleDelegate2(LogMessage), "<BD > "+token.getValue(3));
                            }
                            else if (token.getValue(1).Equals("FINISH"))
                            {
                                OnFinished("Download Complete");
                            }
                            else if (token.getValue(1).Equals("START"))
                            {
                                OnStart("Download Started");
                            }
                        }
                    }
                    catch(System.Exception)
                    {
                        /* Do Nothing*/
                    }
                }
                else
                {
                   // baseobj.reset();
                }
            }
            bFinished = true;
        }












        void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {

        }




        private void Form1_Load(object sender, EventArgs e)
        {
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool temp = baseobj.isStarted();

            if (temp == true)
            {
                baseobj.stopDownloadServer();
            }

            this.richTextBox1.Clear();

            if (temp == true)
            {
                baseobj.startDownloadServer();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool temp = baseobj.isStarted();

            if (temp == true)
            {
                baseobj.stopDownloadServer();
            }
            baseobj.reset();


            this.richTextBox1.Clear();


            this.downloadProgressGUI.UpdateProgress(0);
            this.downloadProgressGUI.UpdateLabel1("");
            this.downloadProgressGUI.UpdateLabel2("");

            this.downloadProgressBD.UpdateProgress(0);
            this.downloadProgressBD.UpdateLabel1("");
            this.downloadProgressBD.UpdateLabel2("");

            if (temp == true)
            {
                baseobj.startDownloadServer();
            }

            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bRunning = false;
            while (bFinished == false)
            {
                Thread.Sleep(100);
            }
            System.Windows.Forms.Application.Exit();
        }

        private void FormClosingEventCancle_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bRunning = false;
            while (bFinished == false)
            {
                Thread.Sleep(100);
            }
            System.Windows.Forms.Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
       
            if (baseobj.isStarted())
            {
                button4.ForeColor = Color.Red;
                button4.BackColor = Color.Black;
                button4.Text = "Stopped";
                baseobj.stopDownloadServer();
                m_flashMessageTimer.Enabled = false;
                label1.Text = "";
                label1.Hide();
            }
            else
            {
                button4.ForeColor = Color.Black;
                button4.BackColor = Color.Green;
                button4.Text = "Running";
                baseobj.startDownloadServer();
                m_flashMessageTimer.Enabled = false;
                label1.Text = "";
                label1.Hide();

            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            m_flashMessageTimer = new System.Timers.Timer(5000) { SynchronizingObject = this };
            m_flashMessageTimer.Elapsed += BarcodeTimerElapsedFlashCallback;
            m_flashMessageTimer.Enabled = false;
            label1.Text = "";
            label1.Hide();
        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked == true && baseobj.getTriggerState() == false)
            {
                baseobj.enableTrigger();
            }
            else if(baseobj.getTriggerState() == true)
            {
                baseobj.disableTrigger();
            }
        }

    }
}
