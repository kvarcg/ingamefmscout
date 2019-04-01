using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FMScout
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(true));
            //Application.Run(new MiniScoutForm(false));
        }
    }
}
