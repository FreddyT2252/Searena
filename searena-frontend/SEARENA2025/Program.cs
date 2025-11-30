using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetEnv;

namespace SEARENA2025
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Env.TraversePath().Load();

            // Enable DPI awareness untuk .NET Framework 4.7+
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            try
            {
                DatabaseHelper.InitializeDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inisialisasi database: {ex.Message}\n\n" +
                    "Pastikan PostgreSQL sudah berjalan dan konfigurasi koneksi sudah benar.",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Application.Run(new DashboardItem());
        }

        // Import DPI awareness function
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}