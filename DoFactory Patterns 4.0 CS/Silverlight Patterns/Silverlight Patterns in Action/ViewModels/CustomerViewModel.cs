using System;
using System.Windows.Input;
using Silverlight_Patterns_in_Action.Web;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel.DomainServices.Client;
using Silverlight_Patterns_in_Action.Web.Services.Web;

namespace Silverlight_Patterns_in_Action.ViewModels
{
    /// <summary>
    /// ViewModel for Customers page.
    /// </summary>
    public class CustomerViewModel : ViewModelBase
    {
        private ActionDomainContext context;

        /// <summary>
        /// Fires when customer list is loading.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> Loading;

        /// <summary>
        /// Fires when customers have been loaded.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> Loaded;

        /// <summary>
        /// Fires when adding customer.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> Adding;

        /// <summary>
        /// Firest when editing a customer.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> Editing;

        /// <summary>
        /// Fires when cancel operation begins.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> Canceling;

        /// <summary>
        /// Fires when cancel operation has comleted.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> Canceled;

        /// <summary>
        /// Fires when deleting a custome begins.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> Deleting;

        /// <summary>
        /// Fires when deleting a customer has completed.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> Deleted;

        /// <summary>
        /// Fires when saving a customer begins.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> Saving;

        /// <summary>
        /// Fires when saving a customer has completed.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> Saved;

        private readonly RelayCommand _addCommand;
        private readonly RelayCommand _editCommand;
        private readonly RelayCommand _deleteCommand;
        private readonly RelayCommand _saveCommand;
        private readonly RelayCommand _cancelCommand;

        /// <summary>
        /// The Add command.
        /// </summary>
        public ICommand AddCommand { get { return _addCommand; } }

        /// <summary>
        /// The Edit command.
        /// </summary>
        public ICommand EditCommand { get { return _editCommand; } }

        /// <summary>
        /// The Delete command.
        /// </summary>
        public ICommand DeleteCommand { get { return _deleteCommand; } }

        /// <summary>
        /// The Save command.
        /// </summary>
        public ICommand SaveCommand { get { return _saveCommand; } }

        /// <summary>
        /// The Cancel command.
        /// </summary>
        public ICommand CancelCommand { get { return _cancelCommand; } }


        /// <summary>
        /// Constructor of Customer ViewModel
        /// </summary>
        public CustomerViewModel()
        {
            if (DesignerProperties.IsInDesignTool) return;

            // init commands.
            _addCommand = new RelayCommand(OnAdd);
            _editCommand = new RelayCommand(OnEdit);
            _deleteCommand = new RelayCommand(OnDelete);
            _saveCommand = new RelayCommand(OnSave);
            _cancelCommand = new RelayCommand(OnCancel);

            UpdateState(false);

            RaiseEvent(Loading);

            // Load customers from database
            context = new ActionDomainContext();
            Customers = context.Customers;  // important.
            context.Load(context.GetCustomersQuery(), GetCustomersQueryCallback, true);
        }

        private void GetCustomersQueryCallback(LoadOperation loadOperation)
        {
            if (loadOperation.HasError)
            {
                Status = loadOperation.Error.Message;
                loadOperation.MarkErrorAsHandled();
                return;
            }

            if (loadOperation.TotalEntityCount > 0)
            {
                foreach (var customer in Customers)
                {
                    CurrentCustomer = customer;
                    break;
                }
            }

            RaiseEvent(Loaded);
        }

        // Enable/disable commanding buttons as a group
        private void UpdateState(bool editing)
        {
            _addCommand.IsEnabled = !editing;
            _editCommand.IsEnabled = !editing;
            _deleteCommand.IsEnabled = !editing;

            _cancelCommand.IsEnabled = editing;
            _saveCommand.IsEnabled = editing;
        }

        private void OnEdit(object parameter)
        {
            Status = "";

            RaiseEvent(Editing);

            UpdateState(true);
        }

        private void OnSave(object parameter)
        {
            Status = "";

            RaiseEvent(Saving);

            context.SubmitChanges(SubmitChangesCallback, null);

            RaiseEvent(Saved);
        }

        private void SubmitChangesCallback(SubmitOperation submitOperation)
        {
            if (submitOperation.HasError)
            {
                Status = "Data was missing or incorrect. Please try again"; // submitOperation.Error.Message;
                submitOperation.MarkErrorAsHandled();
                return;
            }

            UpdateState(false);

            Status = "Customer was saved";
        }

        private void OnDelete(object parameter)
        {
            Status = "";

            // Do not simply raise the Deleting event, because we are giving the 
            // user the opportunity to cancel the delete, and we need to check for this.
            if (Deleting != null)
            {
                var args = new ViewModelEventArgs();
                Deleting(this, args);

                // Check if user choose to cancel deletion.
                if (args.Cancel) return;
            }

            // Start by checking if user has any orders. If so, delete is not allowed.
            int customerId = CurrentCustomer.CustomerId;
            context.GetOrderCountByCustomer(customerId, GetOrderCountByCustomerCallback, false);
        }

        private void GetOrderCountByCustomerCallback(InvokeOperation<int> invokeOperation)
        {
            if (invokeOperation.HasError)
            {
                Status = "An error occured"; 
                invokeOperation.MarkErrorAsHandled();
                return;
            }

            // Customer has orders, and cannot be deleted.
            if (invokeOperation.Value > 0)
            {
                Status = "Customer has orders and cannot be deleted.";
                return;
            }

            // Go ahead, and delete customer.
            context.Customers.Remove(CurrentCustomer);
            context.SubmitChanges();

            Status = "Previously displayed customer was deleted";

            RaiseEvent(Deleted);
        }

        // Cancels add or edit operation
        private void OnCancel(object parameter)
        {
            Status = "";

            RaiseEvent(Canceling);

            context.RejectChanges();

            RaiseEvent(Canceled);
 
            UpdateState(false);
        }

        // Start add operation
        private void OnAdd(object parameter)
        {
            Status = "";

            // Create new customer object and add to list of customers.
            var customer = new Customer();
            context.Customers.Add(customer);

            CurrentCustomer = customer;

            RaiseEvent(Adding);
  
            UpdateState(true);
        }

        #region  INotifyPropertyChanged properties

        private IEnumerable<Customer> _customers;

        /// <summary>
        /// List of customers.
        /// </summary>
        public IEnumerable<Customer> Customers
        {
            get { return _customers; }
            set
            {
                if (_customers != value)
                {
                    _customers = value;
                    OnPropertyChanged("Customers");
                }
            }
        }

        private Customer _currentCustomer;

        /// <summary>
        /// The currently selected customer.
        /// </summary>
        public Customer CurrentCustomer
        {
            get { return _currentCustomer; }
            set
            {
                if (_currentCustomer != value)
                {
                    _currentCustomer = value;
                    OnPropertyChanged("CurrentCustomer");

                    Status = "";
                }
            }
        }

        private string _status;

        /// <summary>
        /// String displaying textual status information.
        /// </summary>
        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        #endregion
    }
}
