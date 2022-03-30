using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Silverlight_Patterns_in_Action.Web;
using Silverlight_Patterns_in_Action.ViewModels;
using System.ComponentModel;

namespace Silverlight_Patterns_in_Action.Views
{
    /// <summary>
    /// Customers page. 
    /// Access requires authentication.
    /// </summary>
    public partial class Customers : Page
    {
        public Customers()
        {
            InitializeComponent();

            // Get viewmodel from static resources and attach listeners to viewmodel events.
            var viewModel = Resources["MyCustomerViewModel"] as CustomerViewModel;

            viewModel.Loading += viewModel_Loading;
            viewModel.Loaded += viewModel_Loaded;

            viewModel.Adding += viewModel_Adding;
            viewModel.Editing += viewModel_Editing;
            viewModel.Deleting += viewModel_Deleting;

            viewModel.Saving += viewModel_Saving;
            viewModel.Saved += viewModel_Saved;

            viewModel.Canceling += viewModel_Canceling;
            viewModel.Canceled += viewModel_Canceled;
        }

        // Customers are loading
        private void viewModel_Loading(object sender, ViewModelEventArgs e)
        {
            this.Cursor = Cursors.Wait;
        }

        // Customers are loaded
        private void viewModel_Loaded(object sender, ViewModelEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        // Customer is added
        private void viewModel_Adding(object sender, ViewModelEventArgs e)
        {
            DataGridCustomers.ScrollIntoView(DataGridCustomers.SelectedItem, null);

            DataGridCustomers.IsEnabled = false;
            DataFormCustomer.BeginEdit();
        }

        // Customer is edited
        private void viewModel_Editing(object sender, ViewModelEventArgs e)
        {
            DataGridCustomers.IsEnabled = false;
            DataFormCustomer.BeginEdit();
        }

        // Customer is deleted. Ask user to continue or not.
        private void viewModel_Deleting(object sender, ViewModelEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this customer?", "Delete Customer", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        // Customer changes are about to being saved to database
        private void viewModel_Saving(object sender, ViewModelEventArgs e)
        {
            IEditableObject customer = DataGridCustomers.SelectedItem as Customer;
            customer.EndEdit();

            DataFormCustomer.CommitEdit(true);
        }

        // Customer changes have been saved to database
        private void viewModel_Saved(object sender, ViewModelEventArgs e)
        {
            DataGridCustomers.IsEnabled = true;
        }

        // Add or Edit Customer is being cancelled
        private void viewModel_Canceling(object sender, ViewModelEventArgs e)
        {
            DataFormCustomer.CancelEdit();
        }

        // Cancel operation has completed
        private void viewModel_Canceled(object sender, ViewModelEventArgs e)
        {
            DataGridCustomers.IsEnabled = true;
        }
    }
}
