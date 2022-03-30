using System;
using System.Windows.Input;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.ComponentModel;

namespace Silverlight_Patterns_in_Action.ViewModels
{
    /// <summary>
    /// ViewModel for Login page.
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        /// <summary>
        /// Fires when user is logging in.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> LoggingIn;

        /// <summary>
        /// Fires when user has successfully logged in.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> LoggedIn;

        /// <summary>
        /// Fires when user login has failed.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> LoginFailed;

        private readonly RelayCommand _loginCommand;

        /// <summary>
        /// The login command.
        /// </summary>
        public ICommand LoginCommand { get { return _loginCommand; } }

        /// <summary>
        /// Constructor of LoginViewModel.
        /// </summary>
        public LoginViewModel()
        {
            if (DesignerProperties.IsInDesignTool) return;

            _loginCommand = new RelayCommand(OnLogin);
            _loginCommand.IsEnabled = true;

            UpdateLoginState(false);
        }

        /// <summary>
        /// The user name of authenticated user.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The password of authenticated user.
        /// </summary>
        public string Password { get; set; }

        private void OnLogin(object parameter)
        {
            RaiseEvent(LoggingIn);

            var loginParms = new LoginParameters(UserName, Password, false, null);
            WebContext.Current.Authentication.Login(loginParms, LoginCallback, null);
        }

        private void LoginCallback(LoginOperation loginOperation)
        {
            if (loginOperation.LoginSuccess)
            {
                UpdateLoginState(true);
                RaiseEvent(LoggedIn);
                return;
            }

            RaiseEvent(LoginFailed);
        }

        /// <summary>
        /// Performs a logout of the authenticated user.
        /// </summary>
        public void Logout()
        {
            try
            {
                WebContext.Current.Authentication.Logout(false);
                UpdateLoginState(false);

                // These are bound properties. Next time user logs in, these fields should be blank.
                UserName = "";
                Password = "";
            }
            catch
            {
                /* do nothing */
            }
        }
        
        private void UpdateLoginState(bool isLoggedIn)
        {
            // Activate / deactive bound menu items (customers and orders)
            AdminLoggedIn = isLoggedIn;

            // Adjust text and url to Login menu item.
            LoginText = isLoggedIn ? "Logout" : "Login";
            LoginUri = new Uri( isLoggedIn ? "/Logout" : "/Login", UriKind.Relative);
        }


        private Uri _loginUri;

        /// <summary>
        /// The login/logout menu item url.
        /// </summary>
        public Uri LoginUri
        {
            get { return _loginUri; }
            set
            {
                if (_loginUri != value)
                {
                    _loginUri = value;
                    OnPropertyChanged("LoginUri");
                }
            }
        }

        private string _loginText;

        /// <summary>
        /// The login/logout menu item text.
        /// </summary>
        public string LoginText
        {
            get { return _loginText; }
            set
            {
                if (_loginText != value)
                {
                    _loginText = value;
                    OnPropertyChanged("LoginText");
                }
            }
        }

        private bool _adminLoggedIn;

        /// <summary>
        /// Flag indicating whether administrator is logged in or not.
        /// </summary>
        public bool AdminLoggedIn
        {
            get { return _adminLoggedIn; }
            set
            {
                if (_adminLoggedIn != value)
                {
                    _adminLoggedIn = value;
                    OnPropertyChanged("AdminLoggedIn");
                }
            }
        }
    }
}
