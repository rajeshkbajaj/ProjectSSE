using System.Drawing;
using Covidien.CGRS.PcAgentInterfaceBusiness;
using Serilog;

namespace Covidien.CGRS.ESS
{
    public static class DeviceSettingControlFactory
    {
        public static DeviceSettingControl CreateDeviceSettingControl(string displayString, DeviceSettingConfigurationParameters configParams, Point pos)
        {
            Log.Information($"DeviceSettingControlFactory:CreateDeviceSettingControl Entry displayString:{displayString} FuncID :{configParams}");
            DeviceSettingControl control = null;
            switch(configParams.mFunctionId)
            {
                case RestrictedFunctionManager.RestrictedFunctions.SET_OPERATIONAL_HOURS:
                case RestrictedFunctionManager.RestrictedFunctions.SET_PREVENTIVE_MAINTENANCE_DUE_HOURS:
                    {
                        control = new DeviceHoursSettingControl(displayString, pos, configParams);
                        break;
                    }
                case RestrictedFunctionManager.RestrictedFunctions.SET_OPTIONS_KEY:
                    {
                        control = new DeviceSettingControlOptionsKey(displayString, pos, configParams);
                        break;
                    }
                case RestrictedFunctionManager.RestrictedFunctions.OVERRIDE_EST_RESULT:
                    {
                        control = new DeviceSettingControlOverrideEST(displayString, pos, configParams);
                        break;
                    }
                case RestrictedFunctionManager.RestrictedFunctions.EXTEND_ENHANCED_SERVICE_MODE:
                    {
                        control = new DeviceSettingControlExtendEsm(displayString, pos, configParams);
                        break;
                    }
                case RestrictedFunctionManager.RestrictedFunctions.SET_EST_PERFORMED_DATE:
                    {
                        control = new DeviceSettingControlESTDate(displayString, pos, configParams);
                        break;
                    }
                default:
                    {
                        control = new DeviceSettingControl(displayString, pos, configParams);
                        break;
                    }
            }
            Log.Information($"DeviceSettingControlFactory:CreateDeviceSettingControl Exit displayString:{displayString} FuncID :{configParams}");
            return control;
        }
    }
}
