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
    using System.Drawing;
    using Covidien.CGRS.PcAgentInterfaceBusiness;
    using Serilog;
    using Utilties;

    class DeviceSettingControlOptionsKey : DeviceSettingControl
    {
        private const int OPTIONS_KEY_TEXT_BOX_XPOS = 176;
        private const int OPTIONS_KEY_TEXT_BOX_WIDTH = 360;
        public DeviceSettingControlOptionsKey(string displayName, Point topLeft, DeviceSettingConfigurationParameters deviceSettingConfigParams) :
                                                base(displayName, topLeft, deviceSettingConfigParams)
        {
            //set the text box wider
            SetTextBoxXpos(OPTIONS_KEY_TEXT_BOX_XPOS);
            SetTextBoxWidth(OPTIONS_KEY_TEXT_BOX_WIDTH);
        }

        public override string SetValueOnDevice()
        {
            string newOptionsKey = SettingValueTextBox.Text;
            newOptionsKey = KeyEncryption.HtmlEncode(newOptionsKey);

            string setAction = BusinessServicesBridge.Instance.Device.SetSettingValueOnDeviceSync(DeviceUserInterfaceServices.SET_OPTIONS_KEY_COMMAND_STRING,
                                                                                                    DeviceUserInterfaceServices.OPTIONS_KEY_VALUE_XML_PARAMETER_STRING,
                                                                                                    newOptionsKey);
            Log.Information($"DeviceSettingControlOptionsKey:SetValueOnDevice");
            return setAction;
        }
    }
}
