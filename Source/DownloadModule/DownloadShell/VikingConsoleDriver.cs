using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Security;

namespace SoftwareDownloadConsole
{
    class VikingConsoleDriver
    {
        //int (* callback)(void *, char *, int)
        //public delegate int callback(ref object FormObj,string str, int length);
        //VIKINGCONSOLEDRIVER_API void VikingConsoleDriver_testCallback(void *in)

        [SuppressUnmanagedCodeSecurity]
        public delegate int callback(ref object obj,string str, int length);

     //   [SuppressUnmanagedCodeSecurity]
     //   [DllImport("VikingConsoleDriver.dll",CallingConvention=CallingConvention.Cdecl, CharSet = CharSet.Auto)]
     //   public static extern void VikingConsoleDriver_testCallback(IntPtr pVikingConsoleDriver, callback pre, ref object  obj);

     
        [DllImport("VikingConsoleDriver.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void VikingConsoleDriver_finish(IntPtr pVikingConsoleDriver);

    
        [DllImport("VikingConsoleDriver.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static IntPtr VikingConsoleDriver_init(string path);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("VikingConsoleDriver.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static int VikingConsoleDriver_startDownloadServer(IntPtr pVikingConsoleDriver);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("VikingConsoleDriver.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static int VikingConsoleDriver_stopDownloadServer(IntPtr pVikingConsoleDriver);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("VikingConsoleDriver.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void VikingConsoleDriver_reset(IntPtr inx);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("VikingConsoleDriver.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static IntPtr VikingConsoleDriver_getNextToken(IntPtr inx);

     
        [DllImport("VikingConsoleDriver.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void VikingConsoleDriver_setDelay(IntPtr inx, int x);




        [SuppressUnmanagedCodeSecurity]
        [DllImport("VikingConsoleDriver.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static int VikingConsoleDriver_enableTrigger(IntPtr pVikingConsoleDriver);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("VikingConsoleDriver.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static int VikingConsoleDriver_disableTrigger(IntPtr pVikingConsoleDriver);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("VikingConsoleDriver.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static int VikingConsoleDriver_oneShotTrigger(IntPtr pVikingConsoleDriver);

        
        [SuppressUnmanagedCodeSecurity]
        [DllImport("VikingConsoleDriver.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static int VikingConsoleDriver_getTriggerState(IntPtr pVikingConsoleDriver);



        IntPtr thisObj;
        ///////////
        bool bStarted = false;
        bool bTriggerEnabled = false;

        // Need to uncomment for testing.

        //public void testCallback(callback call,object obj)
        //{
        //    try
        //    {
        //        //IntPtr intptr_delegate = Marshal.GetFunctionPointerForDelegate(call);
        //      //  VikingConsoleDriver_testCallback(thisObj, call, ref obj);

        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ex.Message.ToString();

        //        const string caption = "Form Closing";
        //        var result = MessageBox.Show(message, caption,
        //                                     MessageBoxButtons.YesNo,
        //                                     MessageBoxIcon.Question);
              
        //        //TEST CODE

        //    }
        //    finally{
            

        //    }

        //}

        public bool isStarted()
        {
            return bStarted;
        }

        public int startDownloadServer()
        {


            bStarted = true;
            return VikingConsoleDriver_startDownloadServer(thisObj);
        }


        public int stopDownloadServer()
        {
            bStarted = false;
            return VikingConsoleDriver_stopDownloadServer(thisObj);
        }

        public int enableTrigger()
        {
            bTriggerEnabled = true;
            return VikingConsoleDriver_enableTrigger(thisObj);
        }

        public int disableTrigger()
        {
            bTriggerEnabled = false;
            return VikingConsoleDriver_disableTrigger(thisObj);
        }

        public int oneShotTrigger()
        {
            return VikingConsoleDriver_oneShotTrigger(thisObj);
        }


        public bool getTriggerState()
        {
            return (VikingConsoleDriver_getTriggerState(thisObj) == 1);
        }


        ~VikingConsoleDriver()
        {
            VikingConsoleDriver_finish(thisObj);
        }


        public VikingConsoleDriver(string path)
        {
            thisObj = VikingConsoleDriver_init(path);
        }



        public void reset()
        {
         //TODO   VikingConsoleDriver_reset(thisObj);
        }


        public VikingConsoleToken getNextToken()
        {
            IntPtr temp = VikingConsoleDriver_getNextToken(thisObj);
            if (temp == IntPtr.Zero)
            {
                return null;
            }
            return new VikingConsoleToken(temp);
        }



        public void setDelay(int x)
        {
            VikingConsoleDriver_setDelay(thisObj, x);
        }
    }
}
