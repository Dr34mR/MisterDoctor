using System;
using System.Windows.Forms;
using SubstitutionBot.Forms;

namespace SubstitutionBot
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new FormMain();

            try
            {
                form.Setup();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            Application.Run(form);
        }
    }
}
