using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Covidien.CGRS.ESS
{
    public partial class AddDeviceForm : Form
    {
        public delegate void AddDeviceCallback(string modelNum, string SerialNum);

        public AddDeviceForm( AddDeviceCallback callback )
        {
            InitializeComponent();
            mCallback = callback;

            List<string> models = DeviceManagement.Instance.GetSupportedDeviceTypes();
            if (models != null)
            {
                ModelSelectionComboBox.Items.Clear();
                models.ForEach(delegate(string model) { ModelSelectionComboBox.Items.Add(model); });
                ModelSelectionComboBox.SelectedIndex = 0;
            }

            ModelLabel.Text = Properties.Resources.DEV_INFO_MODEL;
            SerialNumLabel.Text = Properties.Resources.DEVICE_SERIAL_NUMBER;
            AddDeviceButton.Text = Properties.Resources.ADD_BUTTON;
            CloseButton.Text = Properties.Resources.CLOSE_BUTTON;

            UserEnteredSerialNumber.Text = "";
            UserEnteredSerialNumber.Focus();
        }

        private readonly AddDeviceCallback mCallback;

        private void AddDeviceButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UserEnteredSerialNumber.Text) == false)
            {
                mCallback((string)ModelSelectionComboBox.SelectedItem, UserEnteredSerialNumber.Text);
            }

            UserEnteredSerialNumber.Text = "";
            UserEnteredSerialNumber.Focus();
        }
    }
}
