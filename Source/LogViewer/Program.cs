using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LogViewer
{
    static class Program
    {
        public static string [] MCmdLineArgs ;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main( string[] args )
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MCmdLineArgs = args;

            Application.Run(new Form1());
        }
    }
}
