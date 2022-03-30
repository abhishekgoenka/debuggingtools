using System.Collections.ObjectModel;
using System.Windows.Input;

using WPFModel.BusinessModelObjects;
using WPFModel.Provider;

namespace WPFViewModel
{
    /// <summary>
    /// ViewModel for Customer.
    /// </summary>
    /// <remarks>
    /// MV Patterns: MV-VM Design Pattern.
    /// </remarks>
    public class CustomerViewModel : ViewModelBase
    {
        private IProvider _provider;

        /// <summary>
        /// Observable collection of customers.
        /// </summary>
        public ObservableCollection<CustomerModel> Customers { private set; get; }

        /// <summary>
        /// Command model for adding a customer.
        /// </summary>
        public CommandModel AddCommandModel{ private set; get; }

        /// <summary>
        /// Command model for editing a customer.
        /// </summary>
        public CommandModel EditCommandModel { private set; get; }

        /// <summary>
        /// Command model for deletign a customer.
        /// </summary>
        public CommandModel DeleteCommandModel { private set; get; }

        /// <summary>
        /// Constructor. Creates a new Customer ViewModel.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public CustomerViewModel(IProvider provider)
        {
            _provider = provider;

            Customers = new ObservableCollection<CustomerModel>();

            AddCommandModel = new AddCommand(this);
            EditCommandModel = new EditCommand(this);
            DeleteCommandModel = new DeleteCommand(this);
        }

        /// <summary>
        /// Indicates whether the customer data has been loaded.
        /// </summary>
        public bool IsLoaded { private set; get; }

        /// <summary>
        /// Gets a new customer.
        /// </summary>
        public CustomerModel NewCustomerModel
        {
            get { return new CustomerModel(_provider); }
        }

        /// <summary>
        /// Indicates whether a new customer can be added.
        /// </summary>
        public bool CanAdd
        {
            get { return IsLoaded; }
        }

        /// <summary>
        /// Indicates whether a customer is currently selected.
        /// </summary>
        public bool CanEdit
        {
            get { return IsLoaded && CurrentCustomer != null; }
        }

        /// <summary>
        /// Indicates whether a customer is selected that can be deleted.
        /// </summary>
        public bool CanDelete
        {
            get { return IsLoaded && CurrentCustomer != null; }
        }

        /// <summary>
        /// Indicates whether a customer is selected and orders can be viewed.
        /// </summary>
        public bool CanViewOrders
        {
            get { return IsLoaded && CurrentCustomer != null; }
        }

        /// <summary>
        /// Retrieves and displays customers in given sort order.
        /// </summary>
        /// <param name="sortExpression">Sort order.</param>
        public void LoadCustomers()
        {
            string sortExpression = "CompanyName ASC";
            foreach(var customer in _provider.GetCustomers(sortExpression))
                Customers.Add(customer);
            
            if (Customers.Count > 0)
                CurrentCustomer = Customers[0];

            IsLoaded = true;
        }

        /// <summary>
        /// Clear customers from display.
        /// </summary>
        public void UnloadCustomers()
        {
            Customers.Clear();

            CurrentCustomer = null;
            IsLoaded = false;
        }

        private CustomerModel _currentCustomerModel;
        public CustomerModel CurrentCustomer
        {
            get { return _currentCustomerModel; }
            set
            {
                if (_currentCustomerModel != value)
                {
                    _currentCustomerModel = value;
                    OnPropertyChanged("CurrentCustomer");
                }
            }
        }

        #region Private Command classes

        /// <summary>
        /// Private implementation of the Add Command.
        /// </summary>
        private class AddCommand : CommandModel
        {
            private CustomerViewModel _viewModel;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="viewModel"></param>
            public AddCommand(CustomerViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            public override void OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                var customer = e.Parameter as CustomerModel;
                
                // Check that all values have been entered.
                e.CanExecute =
                    (!string.IsNullOrEmpty(customer.Company)
                  && !string.IsNullOrEmpty(customer.City)
                  && !string.IsNullOrEmpty(customer.Country));

                e.Handled = true;
            }

            public override void OnExecute(object sender, ExecutedRoutedEventArgs e)
            {
                var customer = e.Parameter as CustomerModel;
                customer.Add();

                _viewModel.Customers.Add(customer);
                _viewModel.CurrentCustomer = customer;
            }
        }

        /// <summary>
        /// Private implementation of the Edit command
        /// </summary>
        private class EditCommand : CommandModel
        {
            private CustomerViewModel _viewModel;

            public EditCommand(CustomerViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            public override void OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                var customer = e.Parameter as CustomerModel;

                // Check that all values have been set
                e.CanExecute = (customer.CustomerId > 0
                  && !string.IsNullOrEmpty(customer.Company)
                  && !string.IsNullOrEmpty(customer.City)
                  && !string.IsNullOrEmpty(customer.Country));

                e.Handled = true;
            }

            public override void OnExecute(object sender, ExecutedRoutedEventArgs e)
            {
                var customerModel = e.Parameter as CustomerModel;
                customerModel.Update();
            }
        }

        /// <summary>
        /// Private implementation of the Delete command
        /// </summary>
        private class DeleteCommand : CommandModel
        {
            private CustomerViewModel _viewModel;

            public DeleteCommand(CustomerViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            public override void OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = _viewModel.CanDelete;
                e.Handled = true;
            }

            public override void OnExecute(object sender, ExecutedRoutedEventArgs e)
            {
                var customerModel = _viewModel.CurrentCustomer;

                if (customerModel.Delete() > 0)
                {
                    _viewModel.Customers.Remove(customerModel);
                    e.Handled = true;
                }
            }
        }

        #endregion
    }
}
