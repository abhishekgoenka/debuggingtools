using System.Windows;
using System.Windows.Controls;
using Silverlight_Patterns_in_Action.ViewModels;

namespace Silverlight_Patterns_in_Action.Views
{
    public partial class Logout : Page
    {
        public Logout()
        {
            InitializeComponent();

            // Get viewmodel and logout user.
            var viewModel = Application.Current.Resources["MyLoginViewModel"] as LoginViewModel;
            viewModel.Logout();
        }
    }
}
