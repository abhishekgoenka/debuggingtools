using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Silverlight_Patterns_in_Action.ViewModels;

namespace Silverlight_Patterns_in_Action.Views
{
    /// <summary>
    /// Login, authentication page.
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();

            AttachEventHandlers();

            // Set focus on user name
            Dispatcher.BeginInvoke(() => { textBoxUserName.Focus(); });
        }

        // User is logging in.
        private void viewModel_LoggingIn(object sender, ViewModelEventArgs e)
        {
            this.Cursor = Cursors.Wait;
        }

        // User was successfully logged in.
        private void viewModel_LoggedIn(object sender, ViewModelEventArgs e)
        {
            DetachEventHandlers();

            this.Cursor = Cursors.Arrow;
            
            NavigationService.Navigate(new Uri("/Customers", UriKind.Relative));
        }

        // User login failed. Display message.
        private void viewModel_LoginFailed(object sender, ViewModelEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            MessageBox.Show("Invalid Username or Password. Please try again", "Login", MessageBoxButton.OK);

            // Reset controls
            textBoxUserName.Text = "";
            passwordBoxPassword.Password = "";
            textBoxUserName.Focus();
        }

        private void AttachEventHandlers()
        {
            // Get page's viewmodel and attach event handlers to viewmodel events.
            var viewModel = Application.Current.Resources["MyLoginViewModel"] as LoginViewModel;

            viewModel.LoggingIn += viewModel_LoggingIn;
            viewModel.LoggedIn += viewModel_LoggedIn;
            viewModel.LoginFailed += viewModel_LoginFailed;
        }

        private void DetachEventHandlers()
        {
            // Get page's viewmodel and attach event handlers to viewmodel events.
            var viewModel = Application.Current.Resources["MyLoginViewModel"] as LoginViewModel;

            viewModel.LoggingIn -= viewModel_LoggingIn;
            viewModel.LoggedIn -= viewModel_LoggedIn;
            viewModel.LoginFailed -= viewModel_LoginFailed;
        }


    }
}
