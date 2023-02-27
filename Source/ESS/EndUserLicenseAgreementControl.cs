using System;
using System.Windows.Forms;
using System.IO;
using Covidien.CGRS.PcAgentInterfaceBusiness;
using Serilog;

namespace Covidien.CGRS.ESS
{
    public partial class EndUserLicenseAgreementControl : UserControl
    {
        public EndUserLicenseAgreementControl()
        {
            InitializeComponent();

            ContinueButton.Enabled = false;
            LoadAgreementInformation();
        }

        public void HideControls()
        {
            this.Visible = false;
        }

        public void ShowControls()
        {
            this.Visible = true;
        }

        private void LoadAgreementInformation()
        {
            string licenseFilePath = BusinessServicesBridge.Instance.GetLicenseInfoPath();
            Log.Information($"EndUserLicenseAgreementControl:LoadAgreementInformation Entry licenseFilePath :{licenseFilePath}");
            try
            {
                //open file, get text and set the control
                StreamReader inFile = new StreamReader(licenseFilePath);
                AgreementText.Text = inFile.ReadToEnd();
                inFile.Close();
            }
            catch (Exception e)
            {
                Log.Error($"LoadAgreementInformation Exception:{e.Message}");
                ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.END_USER_LICENSE_ERROR);
            }
            Log.Information($"EndUserLicenseAgreementControl:LoadAgreementInformation Exit licenseFilePath :{licenseFilePath}");
        }

        private void AcceptCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Log.Information($"EndUserLicenseAgreementControl:mAcceptCheckBox_CheckedChanged");
            ContinueButton.Enabled = AcceptCheckBox.CheckState == CheckState.Checked;
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            Log.Information($"EndUserLicenseAgreementControl:mContinueButton_Click");
            ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.END_USER_LICENSE_ACCEPT);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Log.Information($"EndUserLicenseAgreementControl:mCancelButton_Click");
            ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.END_USER_LICENSE_CANCEL);
        }
    }
}
