#region

using System.Windows;
using System.Windows.Threading;
using Auditor.Utilities;

#endregion

namespace Auditor
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = string.Format("An exception occurred: {0}",
                Diagnostics.GetExceptionMessage(e.Exception, true));
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            //Write log containing our exception information
            e.Handled = true;
        }
    }
}