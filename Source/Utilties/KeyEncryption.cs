// ------------------------------------------------------------------------------
//                    Copyright (c) 2021 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Utilties
{
    using System;
    using System.Globalization;

    public static class KeyEncryption
    {
        public static string HtmlEncode(string unHtmlString)
        {
            var escaped = "";
            var c = unHtmlString.ToCharArray();
            var max = c.Length;

            for (int i = 0; i < max; i++)
            {
                if ((48 <= c[i] && c[i] <= 57) ||//0-9
                        (65 <= c[i] && c[i] <= 90) ||//ABC...XYZ
                        (97 <= c[i] && c[i] <= 122) || //abc...xyz
                        (c[i] == '~' || c[i] == '-' || c[i] == '_' || c[i] == '.')
                        )
                {
                    escaped += c[i];
                }
                else
                {
                    escaped += "%";
                    //converts char 255 to string "FF"
                    escaped += Convert.ToByte(c[i]).ToString("x2");
                }
            }
            return escaped;
        }


        public static string HtmlDecode(string htmlPassedInString)
        {
            var ret = "";
            var lengthConverted = 0;
            var htmlString = htmlPassedInString;

            while (lengthConverted < htmlPassedInString.Length)
            {
                //try to convert
                var found = htmlString.IndexOf("%");
                if (found == -1)
                {
                    ret += htmlString;
                    //nothign to convert, break
                    break;
                }
                ret += htmlString.Substring(0, found);
                //convert next 2 chars to string - these will be in hex
                var hexString = htmlString.Substring(found + 1, 2);
                //convert hex string to single char
                var newChar = (char) int.Parse(hexString, NumberStyles.HexNumber);
                ret += newChar.ToString();

                //3 chars are covered above
                lengthConverted += found + 3;
                htmlString = htmlString.Substring(found + 3);
            }
            return ret;
        }
    }
}
