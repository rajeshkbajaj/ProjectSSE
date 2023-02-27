// ------------------------------------------------------------------------------
//                    Copyright (c) 2022 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Covidien.CGRS.ESS
{
    using System;
    using System.Drawing;
    using Covidien.CGRS.PcAgentInterfaceBusiness;
    using Serilog;

    partial class DeviceSettingControlESTDate : DeviceSettingControl
    {
        public DeviceSettingControlESTDate(string displayName, Point topLeft, DeviceSettingConfigurationParameters deviceSettingConfigParams) :
            base(displayName, topLeft, deviceSettingConfigParams)
        {
        }

        public override string GetValueFromDevice()
        {
            //get EST status - if never run - show empty date
            
            string estStatus = BusinessServicesBridge.Instance.Device.GetSettingValueFromDeviceSync(DeviceSettingConfigurationParameters.mGetSettingValueCommandString,
                                                                                                        DeviceUserInterfaceServices.GET_EST_STATUS_PARAMETER_STRING);
            Log.Information($"DevicDeviceSettingControlESTDateeSettingControl:GetValueFromDevice estStatus :{estStatus}");
            if (!string.IsNullOrEmpty(estStatus))
            {
                if (string.Compare(estStatus, "Never Run", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return "";
                }
            }

            return BusinessServicesBridge.Instance.Device.GetSettingValueFromDeviceSync(DeviceSettingConfigurationParameters.mGetSettingValueCommandString,
                                                                                        DeviceSettingConfigurationParameters.mGetSettingValueXmlParameterString);
        }

    }
}