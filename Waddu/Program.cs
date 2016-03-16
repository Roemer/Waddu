using System;
using System.Windows.Forms;
using Waddu.Core;
using Waddu.UI.Forms;

namespace Waddu
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var m = new Mapper("mappings.xml");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
