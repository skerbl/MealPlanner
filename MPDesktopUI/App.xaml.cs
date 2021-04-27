﻿using System.Windows;
using System.Windows.Threading;
using MPData.Services;

namespace MPDesktopUI
{
    public partial class App : Application
    {
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string exceptionMessageText =
                $"An exception occurred: {e.Exception.Message}\r\n\r\nat: {e.Exception.StackTrace}";

            LoggingService.Log(e.Exception);

            // TODO: Create a Window to display the exception information.
            MessageBox.Show(exceptionMessageText, "Unhandled Exception", MessageBoxButton.OK);
        }
    }
}
