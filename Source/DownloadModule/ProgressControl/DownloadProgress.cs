using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;


using System.Collections;

using System.ComponentModel.Design.Serialization;

using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;


namespace Controls
{
    public partial class DownloadProgress : UserControl
    {
        public delegate void SimpleDelegate(int i);
        public delegate void SimpleDelegate2(String i);

        private void UpdateProgress_(int i)
        {
            this.progressBar2.Value = i;
        }

        private void UpdateLabel1_(String i)
        {
            this.label1.Text = i;
        }

        private void UpdateLabel2_(String i)
        {
            this.label2.Text = i;
        }

        public void UpdateProgress(int i)
        {
            this.BeginInvoke(new SimpleDelegate(UpdateProgress_), i);
     
        }

        public void UpdateLabel1(String str)
        {
            this.BeginInvoke(new SimpleDelegate2(UpdateLabel1_), str);
        }


        public void UpdateLabel2(String str)
        {
            this.BeginInvoke(new SimpleDelegate2(UpdateLabel2_), str);
        }





        public override
         string Text
        {
            get
            {
                return this.groupBox1.Text;
            }
            set
            {
                this.groupBox1.Text = value;
            }
        }
        public String dialogName_;











        public DownloadProgress()
        {
            InitializeComponent();
        }




        public DownloadProgress(String name)
        {
            InitializeComponent();
            this.groupBox1.Text = name;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }
    }
}
