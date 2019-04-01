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
            GlobalFuncs globalFuncs = Globals.getGlobalFuncs();
            Exception exc = e.Exception;
            globalFuncs.logging.setErrorLog(ref exc, false);
            MessageBoxResult res = MessageBox.Show("An exception was thrown and has been caught. Please check the error logs. You want to quit?", "FMAssistant Exception thrown", MessageBoxButton.YesNo, MessageBoxImage.Error);

            if (res == MessageBoxResult.Yes)
            {
                this.Shutdown(0);
            }
            e.Handled = true;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            this.DispatcherUnhandledException -= new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
        }
    }
}
