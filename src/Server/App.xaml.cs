using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using LanManager.BLL;
using Application = System.Windows.Application;

namespace Server
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Write(e.ExceptionObject.ToString());
        }
    }
}
