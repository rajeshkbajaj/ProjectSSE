using System;
using System.Runtime.InteropServices;
using System.Security;

namespace DownloadShell_Net
{
    class VikingConsoleDriver
    {

        [SuppressUnmanagedCodeSecurity]
        public delegate int callback(ref object obj,string str, int length);
     
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
        private extern static int VikingConsoleDriver_IsDeviceReady(IntPtr pVikingConsoleDriver);
        
        [SuppressUnmanagedCodeSecurity]
        [DllImport("VikingConsoleDriver.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void VikingConsoleDriver_listenForConfig(IntPtr pVikingConsoleDriver);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("VikingConsoleDriver.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static int VikingConsoleDriver_getTriggerState(IntPtr pVikingConsoleDriver);

        [SuppressUnmanagedCodeSecurity]
       [DllImport("VikingConsoleDriver.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static int VikingConsoleDriver_setDownloadServerWorkingPath(IntPtr pVikingConsoleDriver, string path);


        


        IntPtr thisObj;
        ///////////
        bool bStarted = false;
      //  bool bTriggerEnabled = false;


        public bool setDownloadServerWorkingPath(string path)
        {
            //return true;
            return (VikingConsoleDriver_setDownloadServerWorkingPath(thisObj, path)==0);
        }



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
          //  bTriggerEnabled = true;
            return VikingConsoleDriver_enableTrigger(thisObj);
        }

        public int disableTrigger()
        {
           // bTriggerEnabled = false;
            return VikingConsoleDriver_disableTrigger(thisObj);
        }

        public int oneShotTrigger()
        {
            return VikingConsoleDriver_oneShotTrigger(thisObj);
        }

        public bool isDeviceReady()
        {
            ///todo remove
            int val = VikingConsoleDriver_IsDeviceReady(thisObj);
            return (val!=0);
        }

        public void ListenForConfig()
        {
            VikingConsoleDriver_listenForConfig(thisObj);
            return;
        }

        public bool getTriggerState()
        {
            return (VikingConsoleDriver_getTriggerState(thisObj) == 1);
        }


        ~VikingConsoleDriver()
        {
            //VikingConsoleDriver_finish(thisObj);
        }

        public void Dispose()
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
