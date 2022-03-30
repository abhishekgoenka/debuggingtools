using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

using WPFModel.BusinessModelObjects;

namespace WPFModel.Provider
{
    /// <summary>
    /// Provider interface to data Services.
    /// </summary>
    public interface IProvider
    {
        void Login(string userName, string password);
        void Logout();

        ObservableCollection<CustomerModel> GetCustomers(string sortExpression);
        CustomerModel GetCustomer(int customerId);

        int AddCustomer(CustomerModel customer);
        int UpdateCustomer(CustomerModel customer);
        int DeleteCustomer(int customerId);

        ObservableCollection<OrderModel> GetOrders(int customerId);
    }
}
