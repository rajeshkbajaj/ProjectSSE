using Serilog;
using System.Drawing;
using System.Windows.Forms;

namespace Covidien.CGRS.ESS
{
    public partial class DeviceStatusBox : UserControl
    {
        public DeviceStatusBox( string modelNum, string serialNum )
        {
            InitializeComponent();
            DeviceSerialNumberLabel.Text = serialNum;
            Model = modelNum;
            DeviceStatusLabel.Text = Properties.Resources.UNKNOWN_STATUS;
        }

        public string Model { get; set; }

        public string SerialNumber()
        {
            return DeviceSerialNumberLabel.Text;
        }

        public bool? CheckDeviceStatus()
        {
            bool? dataExists = SoftwarePackageManagement.Instance.DeviceDataOnAgent(Model, DeviceSerialNumberLabel.Text);
            Log.Information($"DeviceStatusBox:CheckDeviceStatus dataExists :{dataExists}");
            if (dataExists == true)
            {
                DeviceStatusLabel.Text = Properties.Resources.LOADED_STATUS;
                DeviceStatusLabel.ForeColor = Color.Green;
            }
            else if (dataExists == false)
            {
                DeviceStatusLabel.Text = Properties.Resources.FAILED_STATUS;
                DeviceStatusLabel.ForeColor = Color.Red;
            }
            else
            {
                DeviceStatusLabel.Text = Properties.Resources.REQUESTING_STATUS;
            }

            return dataExists;
        }

        public bool RequestPending()
        {
            Log.Information($"DeviceStatusBox:RequestPending");
            return DeviceStatusLabel.Text == Properties.Resources.REQUESTING_STATUS;
        }
    }
}
