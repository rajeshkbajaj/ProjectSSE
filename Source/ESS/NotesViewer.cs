using Serilog;
using System.Windows.Forms;

namespace Covidien.CGRS.ESS
{
    public partial class NotesViewer : Form
    {
        public NotesViewer()
        {
            InitializeComponent();
            this.Text = Properties.Resources.NOTES_VIEWER_FORM_TITLE;
        }

        public void DocumentText(string htmlText)
        {
            Log.Information($"NotesViewer:DocumentText htmlText:{htmlText}");
            WebBrowser1.DocumentText = htmlText;
        }

	    public bool DisplayFile( string fName )
	    {
            bool exists = System.IO.File.Exists(fName);
            Log.Information($"NotesViewer:DisplayFile Entry fileName:{fName} exists:{exists}");

            if (exists)
            {
                WebBrowser1.Navigate(fName);
            }
            else
            {
                MessageBox.Show(Properties.Resources.FILE_NOT_FOUND_ERROR_MSG + ":" + fName, Properties.Resources.ESS_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Log.Information($"NotesViewer:DisplayFile Exit fileName:{fName} exists:{exists}");

            return exists;
	    }
    }
}
