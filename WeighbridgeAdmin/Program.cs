using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using WeighbridgeAdmin.Data;
using WeighbridgeAdmin.Forms;

namespace WeighbridgeAdmin
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // DevExpress starts on the "Basic" skin, which is deliberately
            // plain - the grids look like the DataGridViews they replaced until
            // a real skin is selected.  One string, changed here, restyles them.
            WindowsFormsSettings.DefaultFont = new Font("Segoe UI", 9F);
            UserLookAndFeel.Default.SetSkinStyle("WXI");

            // Configuration is read the first time Repository.Current is touched,
            // so a bad config file surfaces here rather than as an unhandled
            // exception dialog part way through startup.
            Repository repository;
            try
            {
                repository = Repository.Current;
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show(
                    "The configuration file is not valid.\r\n\r\n" +
                    ex.Message + "\r\n\r\n" +
                    "Check WeighbridgeAdmin.exe.config.",
                    "Weighbridge Administration",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // Check the database is reachable before showing the main form,
            // otherwise the operator just gets an unhandled exception dialog.
            string errorMessage;
            if (!repository.TestConnection(out errorMessage))
            {
                MessageBox.Show(
                    "Cannot connect to the weighbridge database.\r\n\r\n" +
                    errorMessage + "\r\n\r\n" +
                    "Check the WeighbridgeDb connection string in WeighbridgeAdmin.exe.config, " +
                    "and make sure Database\\CreateDatabase.sql has been run.",
                    "Weighbridge Administration",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            Application.Run(new MainForm());
        }
    }
}
