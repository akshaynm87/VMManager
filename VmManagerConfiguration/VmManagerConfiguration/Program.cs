using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace VmManagerConfiguration
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new VMManagerConfigUI());
            }
            catch (Exception e)
            {
                Debug.WriteLine("ERROR start " + e.StackTrace+ e.Message);
            }

            
            }
    }
}
