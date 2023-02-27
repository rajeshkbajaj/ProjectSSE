using System;
using System.Threading;
using System.Windows.Forms;
using Serilog;

namespace Covidien.CGRS.ESS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        static readonly Mutex MutexObj = new Mutex(true, "ESSApplication");       
        [STAThread]
        static void Main()
        {
            if (MutexObj.WaitOne(TimeSpan.Zero, true))
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(ESS_Main.Instance);
                }
                catch (Exception ex){ Log.Error($"ESS application terminated with Error: {ex.Message}"); }
                finally { MutexObj.ReleaseMutex(); }
            }
            else
            {
                MessageBox.Show("ESS Application is already running. Only one ESS instance is allowed at a time", "PB980 Enhanced Service Software");
            }
        }
    }
}
