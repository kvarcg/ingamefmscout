using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace FMScout
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            this.Exit += new ExitEventHandler(App_Exit);
        }

        public void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Globals.getGlobalFuncs().logging.update(string.Format("An error occured with Msg: {0}", e.Exception.Message));
            Globals.getGlobalFuncs().logging.update(string.Format("Error Stack Trace: {0}", e.Exception.StackTrace));
            System.Diagnostics.Debug.WriteLine(string.Format("An error occured with Msg: {0}", e.Exception.Message));
            System.Diagnostics.Debug.WriteLine(string.Format("Error Stack Trace: {0}", e.Exception.StackTrace));
            e.Handled = true;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            this.DispatcherUnhandledException -= new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
        }
    }
}
