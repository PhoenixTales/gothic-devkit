using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Font2Targa
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Font2Targa_Main());
        }
    }
}
