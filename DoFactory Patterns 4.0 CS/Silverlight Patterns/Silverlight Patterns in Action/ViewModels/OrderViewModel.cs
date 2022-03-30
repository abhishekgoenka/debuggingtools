using System;
using System.Windows.Input;
using Silverlight_Patterns_in_Action.Web;
using System.ComponentModel;
using System.ServiceModel.DomainServices.Client;
using Silverlight_Patterns_in_Action.Web.Services.Web;

namespace Silverlight_Patterns_in_Action.ViewModels
{
    /// <summary>
    /// The ViewModel for Orders page.
    /// </summary>
    public class OrderViewModel : ViewModelBase
    {
        private ActionDomainContext context;

        /// <summary>
        /// Fires when customes have been loaded.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> CustomersLoaded;

        /// <summary>
        /// Fires when orders have been loaded.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> OrdersLoaded;

        /// <summary>
        /// Fires when order details have been loaded.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> OrderDetailsLoaded;


        private readonly RelayCommand _filterCommand;
        private readonly RelayCommand _clearCommand;

        /// <summary>
        /// The Filter command. Used in filtering customer records.
        /// </summary>
        public ICommand FilterCommand { get { return _filterCommand; } }

        /// <summary>
        /// The Clear command. Used to clear (undo) the current filter.
        /// </summary>
        public ICommand ClearCommand { get { return _clearCommand; } }

        /// <summary>
        /// Constructor for the OrderViewModel.
        /// </summary>
        public OrderViewModel()
        {
            if (DesignerProperties.IsInDesignTool) return;

            _filterCommand = new RelayCommand(OnFilter);
            _filterCommand.IsEnabled = true;

            _clearCommand = new RelayCommand(OnClearFilter);
            _clearCommand.IsEnabled = false;

            
            context = new ActionDomainContext();

            // Associate EntitySets with Customer, Order, OrderDetail collections.
            Customers = context.Customers;
            Orders = context.Orders;
            OrderDetails = context.OrderDetails;

            LoadCustomers();
        }

        #region Commanding

        // Performs filter operation.
        private void OnFilter(object parameter)
        {
            string name = Filter ?? "";
            LoadCustomers(name);

            _clearCommand.IsEnabled = true;
        }

        // Clears current filter and reloads all customers.
        private void OnClearFilter(object parameter)
        {
            LoadCustomers();

            Filter = "";
            _clearCommand.IsEnabled = false;
        }

        #endregion

        #region Async Entity loading

        // Starts 'chaining' load of customers, orders, orderdetails.
        private void LoadCustomers(string name = null)
        {
            Customers.Clear();

            // Either get all customers or filter by name.
            if (string.IsNullOrEmpty(name))
                context.Load(context.GetCustomersQuery(), GetCustomersQueryCallback, true);
            else
                context.Load(context.FindCustomersQuery(name), GetCustomersQueryCallback, true);
        }

        public void GetCustomersQueryCallback(LoadOperation loadOperation)
        {
            if (loadOperation.HasError)
            {
                Status = "Customers retrieval failed"; 
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

                LoadOrders();
            }

            RaiseEvent(CustomersLoaded);

            Status = loadOperation.TotalEntityCount + " customers retrieved";
        }

        // Load orders for current customer.
        private void LoadOrders()
        {
            Orders.Clear();

            if (CurrentCustomer != null)
                context.Load(context.GetOrdersByCustomerQuery(CurrentCustomer.CustomerId), GetOrdersByCustomerQueryCallback, true);
        }

        public void GetOrdersByCustomerQueryCallback(LoadOperation loadOperation)
        {
            if (loadOperation.HasError)
            {
                Status = "Orders retrieval failed"; // loadOperation.Error.Message;
                loadOperation.MarkErrorAsHandled();
                return;
            }

            if (loadOperation.TotalEntityCount > 0)
            {
                foreach (var order in Orders)
                {
                    CurrentOrder = order;
                    break;
                }

                LoadOrderDetails();
            }

            RaiseEvent(OrdersLoaded);
        }

        // Load order details for current order
        private void LoadOrderDetails()
        {
            OrderDetails.Clear();

            if (CurrentOrder != null)
                context.Load(context.GetOrderDetailsByOrderQuery(CurrentOrder.OrderId), GetOrderDetailsByOrderQueryCallback, true);
        }

        public void GetOrderDetailsByOrderQueryCallback(LoadOperation loadOperation)
        {
            if (loadOperation.HasError)
            {
                Status = "Order Details retrieval failed"; // loadOperation.Error.Message;
                loadOperation.MarkErrorAsHandled();
                return;
            }

            RaiseEvent(OrderDetailsLoaded);
        }

        #endregion

        #region INotifyPropertyChanged properties

        private string _status;

        /// <summary>
        /// String displaying current status.
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

        private string _filter;

        /// <summary>
        /// The filter (of customer name).
        /// </summary>
        public string Filter
        {
            get { return _filter; }
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    OnPropertyChanged("Filter");
                }
            }
        }

        private EntitySet<Customer> _customers;

        /// <summary>
        /// Collection of customers.
        /// </summary>
        public EntitySet<Customer> Customers
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

                    LoadOrders();
                }
            }
        }

        private EntitySet<Order> _orders;

        /// <summary>
        /// Collection of orders.
        /// </summary>
        public EntitySet<Order> Orders
        {
            get { return _orders; }
            set
            {
                if (_orders != value)
                {
                    _orders = value;
                    OnPropertyChanged("Orders");
                }
            }
        }

        private Order _currentOrder;

        /// <summary>
        /// Currently selected order.
        /// </summary>
        public Order CurrentOrder
        {
            get { return _currentOrder; }
            set
            {
                if (_currentOrder != value)
                {
                    _currentOrder = value;
                    OnPropertyChanged("CurrentOrder");

                    LoadOrderDetails();
                }
            }
        }

        private EntitySet<OrderDetail> _orderDetails;

        /// <summary>
        /// Collection of order details.
        /// </summary>
        public EntitySet<OrderDetail> OrderDetails
        {
            get { return _orderDetails; }
            set
            {
                if (_orderDetails != value)
                {
                    _orderDetails = value;
                    OnPropertyChanged("OrderDetails");
                }
            }
        }
        #endregion
    }
}
