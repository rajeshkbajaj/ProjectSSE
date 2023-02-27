using Serilog;
using System;
using System.Windows.Forms;

namespace Covidien.CGRS.ESS
{
    public partial class DeviceSerialNumberControl : UserControl
    {
        public event EventHandler DeviceSerialNumberSubmitDone;

        public DeviceSerialNumberControl()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
            EnterSerialNumberTextBox.Clear();
            ReEnterSerialNumberTextBox.Clear();

            SubmitButton.Enabled = false;
        }

        public void EvaluateTextBoxes()
        {
            if( (EnterSerialNumberTextBox.TextLength > 0 && ReEnterSerialNumberTextBox.TextLength > 0) &&
                (EnterSerialNumberTextBox.Text.Equals(ReEnterSerialNumberTextBox.Text, StringComparison.OrdinalIgnoreCase)) )
            {
                SubmitButton.Enabled = true;
            }
            Log.Information($"DeviceSerialNumberControl:EvaluateTextBoxes mSubmitButton.Enabled :{SubmitButton.Enabled}");
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            SoftwarePackageManagement.Instance.SetDeviceSerialNumber(EnterSerialNumberTextBox.Text);

            DeviceSerialNumberSubmitDone?.Invoke(sender, e);
            Log.Information($"DeviceSerialNumberControl:SubmitButton_Click");
        }

        private void EnterSerialNumberTextBox_TextChanged(object sender, EventArgs e)
        {
            Log.Information($"DeviceSerialNumberControl:EnterSerialNumberTextBox_TextChanged");
            EvaluateTextBoxes();
        }

        private void ReEnterSerialNumberTextBox_TextChanged(object sender, EventArgs e)
        {
            Log.Information($"DeviceSerialNumberControl:ReEnterSerialNumberTextBox_TextChanged");
            EvaluateTextBoxes();
        }

        public string GetDeviceSerialNumber()
        {
            Log.Information($"DeviceSerialNumberControl:GetDeviceSerialNumber Serial No :{EnterSerialNumberTextBox.Text}");
            return EnterSerialNumberTextBox.Text;
        }
    }
}
