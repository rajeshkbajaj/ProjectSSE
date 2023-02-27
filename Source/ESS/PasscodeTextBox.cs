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
    using System.Windows.Forms;

    public class PasscodeTextBox : TextBox
    {
        private const int EM_SHOWBALLOONTIP = 0x1503;

        /// <summary>
        /// Stops tooltips popup
        /// e.g. Caplock is ON popup will be disabled for the digits/passcode.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == EM_SHOWBALLOONTIP)
            {
                m.Result = (IntPtr)0;
                return;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// Ensures to accept only digit keypress allowed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PasswordTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
