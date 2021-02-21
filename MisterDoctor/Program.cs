using System;
using System.Windows.Forms;
using MisterDoctor.Forms;

namespace MisterDoctor
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainForm = new FormMain();

            mainForm.Setup();

            Application.Run(mainForm);
        }
    }
}
