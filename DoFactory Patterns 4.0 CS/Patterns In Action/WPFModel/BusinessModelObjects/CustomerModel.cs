using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using System.Configuration;
using System.Collections.ObjectModel;

using WPFModel.Provider;

namespace WPFModel.BusinessModelObjects
{
    /// <summary>
    /// Model of the customer. 
    /// </summary>
    public class CustomerModel : BaseModel
    {
        private IProvider _provider;

        private int _customerId = 0;
        private string _company;
        private string _city;
        private string _country;
        private string _version;

        private ObservableCollection<OrderModel> _orders;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="provider">The provider for the customer.</param>
        public CustomerModel(IProvider provider)
        {
            _provider = provider;
        }
        /// <summary>
        /// Adds a new customer.
        /// </summary>
        public int Add()
        {
            return  _provider.AddCustomer(this);;
        }

        /// <summary>
        /// Deletes a customer.
        /// </summary>
        public int Delete()
        {
            var orders = _provider.GetOrders(this.CustomerId);
            if (orders == null || orders.Count == 0) 
                return _provider.DeleteCustomer(this.CustomerId);
            else
                return 0; // Nothing deleted because customer has orders.
                
        }

        /// <summary>
        /// Updates a customer.
        /// </summary>
        public int Update()
        {
            return _provider.UpdateCustomer(this);
        }

        /// <summary>
        /// Gets or sets customerId
        /// </summary>
        public int CustomerId
        {
            get { ConfirmOnUIThread(); return _customerId; }
            set { ConfirmOnUIThread(); if (_customerId != value) { _customerId = value; Notify("CustomerId"); } }
        }

        /// <summary>
        /// Gets or sets customer name.
        /// </summary>
        public string Company
        {
            get { ConfirmOnUIThread(); return _company; }
            set { ConfirmOnUIThread(); if (_company != value) { _company = value; Notify("Company"); } }
        }

        /// <summary>
        /// Gets or sets customer city.
        /// </summary>
        public string City
        {
            get { ConfirmOnUIThread(); return _city; }
            set { ConfirmOnUIThread(); if (_city != value) { _city = value; Notify("City"); } }
        }

        /// <summary>
        /// Gets or set customer country.
        /// </summary>
        public string Country
        {
            get { ConfirmOnUIThread(); return _country; }
            set { ConfirmOnUIThread(); if (_country != value) { _country = value; Notify("Country"); } }
        }

        /// <summary>
        /// Gets or sets list of orders associated with customer.
        /// </summary>
        public ObservableCollection<OrderModel> Orders
        {
            get { ConfirmOnUIThread(); LazyloadOrders(); return _orders; }
            set { ConfirmOnUIThread(); _orders = value; Notify("Orders"); }
        }

        // Private helper that performs lazy loading of orders.
        private void LazyloadOrders()
        {
            if (_orders == null) 
            {
                Orders = _provider.GetOrders(this.CustomerId) ?? new ObservableCollection<OrderModel>();
            }
        }

        /// <summary>
        /// Gets or sets version number
        /// </summary>
        public string Version
        {
            get { ConfirmOnUIThread(); return _version; }
            set { ConfirmOnUIThread(); if (_version != value) { _version = value; Notify("Version"); } }
        }
    }
}
