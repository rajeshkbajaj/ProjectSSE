using Serilog;
using System.Drawing;
using System.Windows.Forms;

namespace Covidien.CGRS.ESS
{
    public partial class DeviceTypeStatusBox : UserControl
    {
        public DeviceTypeStatusBox( string modelNumber )
        {
            InitializeComponent();
            GroupBox1.Text = modelNumber;
            DocumentationPackageStatusLabel.Text = Properties.Resources.PACKAGE_NOT_AVAILABLE;
            SoftwarePackageStatusLabel.Text = Properties.Resources.PACKAGE_NOT_AVAILABLE;
        }

        public string ModelNumber()
        {
            return GroupBox1.Text;
        }

        public void SoftwarePackageIsLoaded()
        {
            Log.Information($"DeviceTypeStatusBox:SoftwarePackageIsLoaded");
            SoftwarePackageStatusLabel.Text = Properties.Resources.LOADED_STATUS;
            SoftwarePackageStatusLabel.ForeColor = Color.Green;
        }

        public void DocumentsAreLoaded()
        {
            Log.Information($"DeviceTypeStatusBox:DocumentsAreLoaded");
            DocumentationPackageStatusLabel.Text = Properties.Resources.LOADED_STATUS;
            DocumentationPackageStatusLabel.ForeColor = Color.Green;
        }
    }
}
