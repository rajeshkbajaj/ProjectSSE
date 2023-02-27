using Serilog;
using System.Drawing;

namespace Covidien.CGRS.ESS
{
    class DeviceSettingControlOverrideEST : RadioButtonDeviceSettingControl
    {
        public DeviceSettingControlOverrideEST(string displayName, Point topLeft, DeviceSettingConfigurationParameters deviceSettingConfigParams) :
                                                base(displayName, topLeft, deviceSettingConfigParams)
        {
        }

        public override void UpdatePrivileges()
        {
            //control only visible for covidien user
            this.Visible = (AuthenticationService.Instance().IsCovidienUser() == true);
            Log.Information($"DeviceSettingControlOverrideEST:UpdatePrivileges Is Covidian :{AuthenticationService.Instance().IsCovidienUser()}");
        }
    }
}
