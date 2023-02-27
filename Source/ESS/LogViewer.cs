using Serilog;
using System;
using System.Windows.Forms;

namespace Covidien.CGRS.ESS
{
    public partial class LogViewer : Form
    {
        private readonly string Title = "";
        public LogViewer(string title)
        {
            Title = title;
            InitializeComponent();
            this.Text = Properties.Resources.LOG_VIEW_FORM_TITLE;

            // ESC key closes application
            this.CancelButton = new Button();
            ((Button)this.CancelButton).Click += delegate(object o, EventArgs e) { this.Close(); };
        }

        public void DocumentText( string htmlText )
        {
            Log.Information($"LogViewer:DocumentText Entry htmlText:{htmlText}");
            // If a drive letter followed by a colon.  E.g., C:
            if  ( 1 == htmlText.IndexOf( ":" ) )
            {
                WebBrowser1.Navigate( htmlText ) ;
            }
            else if  ( 0 == htmlText.IndexOf( "file://" ) )
            {
                WebBrowser1.Navigate( htmlText.Substring( 7 ) ) ;
            }
            else
            {
                WebBrowser1.DocumentText = htmlText;
            }
            Log.Information($"LogViewer:DocumentText Exit htmlText:{htmlText}");
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Log.Information($"LogViewer:mCancelButton_Click");
            this.Close();
        }
    }
}
