using Serilog;
using System;
using System.Windows.Forms;

namespace Covidien.CGRS.ESS
{
    public partial class SetDeviceAccessKeyForm : Form
    {
        public SetDeviceAccessKeyForm()
        {
            InitializeComponent();
        }

        private void AccessKeyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                UpdateButton_Click(sender, null);
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            Log.Information($"SetDeviceAccessKeyForm:mUpdateButton_Click Entry");
            if (string.IsNullOrEmpty(AccessKeyTextBox.Text) == false)
            {
                // call bridge function
                DeviceManagement.Instance.SetDeviceAccessKey(AccessKeyTextBox.Text);
                this.Close();
            }
            Log.Information($"SetDeviceAccessKeyForm:mUpdateButton_Click Exit");
        }

    }
}
