
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
/*
 *  1.2.10.424  20/05/2018  Versione iniziale
 *  
 */
namespace PileSW
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
            Application.Run(new frmMain(args));
        }
   
    }
}
