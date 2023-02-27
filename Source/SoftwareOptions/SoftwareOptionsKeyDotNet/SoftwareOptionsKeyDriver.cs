using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;

namespace SoftwareOptionsKeyDotNet
{
    public class SoftwareOptionsKeyDriver
    {
        [DllImport("sok.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static IntPtr SoftwareOptionsKey_getInstance();

        [DllImport("sok.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void SoftwareOptionsKey_deleteInstance(IntPtr pSoftwareOptionsKeyDriver);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("sok.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void SoftwareOptionsKey_updateKey(IntPtr pSoftwareOptionsKeyDriver, string encryptedKey, string ventSN);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("sok.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void SoftwareOptionsKey_setOptionState(IntPtr pSoftwareOptionsKeyDriver, int id, int state);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("sok.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static int SoftwareOptionsKey_getOptionState(IntPtr pSoftwareOptionsKeyDriver, int id);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("sok.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private extern static void SoftwareOptionsKey_getEncryptedKey(IntPtr pSoftwareOptionsKeyDriver, StringBuilder encryptedKey, int length);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("sok.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void SoftwareOptionsKey_setKeyExpiryDate(IntPtr pSoftwareOptionsKeyDriver, int id, int day, int month, int year);
        
        [SuppressUnmanagedCodeSecurity]
        [DllImport("sok.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
	    private extern static int SoftwareOptionsKey_getKeyExpiryDay(IntPtr pSoftwareOptionsKeyDriver, int id);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("sok.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static int SoftwareOptionsKey_getKeyExpiryMonth(IntPtr pSoftwareOptionsKeyDriver, int id);


        [SuppressUnmanagedCodeSecurity]
        [DllImport("sok.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static int SoftwareOptionsKey_getKeyExpiryYear(IntPtr pSoftwareOptionsKeyDriver, int id);

        private readonly IntPtr thisObj;

        public SoftwareOptionsKeyDriver()
        {
            thisObj = SoftwareOptionsKey_getInstance();
        }

        ~SoftwareOptionsKeyDriver()
        {
            SoftwareOptionsKey_deleteInstance(thisObj);
        }

        public void UpdateOptionsKey(string encryptedKey, string ventSN)
        {
            SoftwareOptionsKey_updateKey(thisObj, encryptedKey, ventSN);
        }

        public void SetOptionState(int id, int state)
        {
            SoftwareOptionsKey_setOptionState(thisObj, id, state);
        }

        public int GetOptionState(int id)
        {
            int state = SoftwareOptionsKey_getOptionState(thisObj, id);
            return state;
        }

        public string GetEncryptedKey()
        {
            StringBuilder encryptedString = new StringBuilder(1024);

            SoftwareOptionsKey_getEncryptedKey(thisObj, encryptedString, 1024);

            string myEncryptedKey = encryptedString.ToString();

            return myEncryptedKey;
        }

        public void SetExpiryDate(int id, int day, int month, int year)
        {
            SoftwareOptionsKey_setKeyExpiryDate(thisObj, id, day, month, year);
        }

        public int GetExpiryDay(int id)
        {
            return SoftwareOptionsKey_getKeyExpiryDay(thisObj, id);
        }

        public int GetExpiryMonth(int id)
        {
            return SoftwareOptionsKey_getKeyExpiryMonth(thisObj, id);
        }

        public int GetExpiryYear(int id)
        {
            return SoftwareOptionsKey_getKeyExpiryYear(thisObj, id);
        }
    }
}
