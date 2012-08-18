using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows;
using LanManager.OSNativeUtils.Environment.Windows;
using System.Linq;
using LanManager.OSNativeUtils.Security.Windows;

namespace LanManager.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["machineId"]))
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // Add an Application Setting.
                config.AppSettings.Settings.Add("machineId", Guid.NewGuid().ToString());

                // Save the configuration file.
                config.Save(ConfigurationSaveMode.Modified);
                // Force a reload of a changed section.
                ConfigurationManager.RefreshSection("appSettings");
            }

            if (Desktop.GetCurrent().DesktopName != "LanManager")
            {
                Desktop original = Desktop.GetCurrent();

                Desktop novo = Desktop.CreateDesktop("LanManager");//Process.GetCurrentProcess().MainModule.FileName
                Process p = Desktop.CreateProcess(Application.ResourceAssembly.Location, novo.DesktopName);
                novo.Show();
                WinSecureEnvironment.Ativar();
                p.WaitForExit();
                WinSecureEnvironment.Desativar();
                original.Show();
                novo.Close();
                Current.Shutdown();
            }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LanManager.BLL.Log.Write(e.ExceptionObject.ToString());
        }
    }
}
