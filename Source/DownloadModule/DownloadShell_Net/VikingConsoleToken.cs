using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace DownloadShell_Net
{
    class VikingConsoleToken
    {
        private IntPtr thisObj;

        [DllImport("VikingConsoleDriver.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern System.IntPtr VikingConsoleDriver_getValue(IntPtr inx, int val);


        [DllImport("VikingConsoleDriver.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int VikingConsoleDriver_getLength(IntPtr inx);


        public VikingConsoleToken(IntPtr inP)
        {
            thisObj = inP;
        }
        public int getLength()
        {
            return VikingConsoleDriver_getLength(thisObj);
        }


        public string getValue(int iVal)
        {
            IntPtr ptr = VikingConsoleDriver_getValue(thisObj, iVal);
            string val = Marshal.PtrToStringAnsi(ptr);
            return val;
        }

    }
}
