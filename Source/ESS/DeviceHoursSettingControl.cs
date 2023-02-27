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
    using Serilog;
    using System.Drawing;
    using System.Windows.Forms;

    partial class DeviceHoursSettingControl : DeviceSettingControl
    {
        public DeviceHoursSettingControl(string displayName, Point topLeft, DeviceSettingConfigurationParameters deviceSettingConfigParams) :
            base(displayName, topLeft, deviceSettingConfigParams)
        {
        }

        protected override void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //only accept numbers
            //Allow navigation keyboard arrows
            Log.Information($"DeviceHoursSettingControl:mTextBox_KeyDown Entry ");
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.PageUp:
                case Keys.PageDown:
                case Keys.Back:
                case Keys.Delete:
                    e.SuppressKeyPress = false;
                    return;
                default:
                    break;
            }

            //Handle pasted Text ctrl+v is accepted
            if (e.Control && e.KeyCode == Keys.V)
            {
                //Preview paste data (removing non-number characters)
                string pasteText = Clipboard.GetText();
                string strippedText = "";
                for (int i = 0; i < pasteText.Length; i++)
                {
                    if (char.IsDigit(pasteText[i]))
                        strippedText += pasteText[i].ToString();
                }

                if (strippedText != pasteText)
                {
                    //There were non-numbers in the pasted text
                    e.SuppressKeyPress = true;
                }
                else
                    e.SuppressKeyPress = false;
            }

            //Block non-number characters
            bool nonNumber = (((e.KeyCode < Keys.D0) || (e.KeyCode > Keys.D9)) &&
                              ((e.KeyCode < Keys.NumPad0) || (e.KeyCode > Keys.NumPad9)));
            if (nonNumber)
            {
                e.SuppressKeyPress = true;
            }
            Log.Information($"DeviceHoursSettingControl:mTextBox_KeyDown Exit ");
            return;
        }
    }
}
