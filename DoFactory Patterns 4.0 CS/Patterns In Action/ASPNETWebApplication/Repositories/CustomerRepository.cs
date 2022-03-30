using System;
using System.Collections.Generic;
using System.ComponentModel;

using ASPNETWebApplication.ActionServiceReference;

namespace ASPNETWebApplication.Repositories
{
    /// <summary>
    /// Repository for customer related objects.
    /// </summary>
    /// <remarks>
    /// Repository Pattern.
    /// </remarks>
    public class CustomerRepository : RepositoryBase
    {
        /// <summary>
        /// Gets a list of customers.
        /// </summary>
        /// <param name="sortExpression">Desired sort order of the customer list.</param>
        /// <returns>List of customers.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IList<Customer> GetCustomers(string sortExpression)
        {
            var request = new CustomerRequest().Prepare();
            request.LoadOptions = new string[] { "Customers" };
            request.Criteria = new CustomerCriteria { SortExpression = sortExpression };

            var response = Client.GetCustomers(request);

            Correlate(request, response);

            return response.Customers;
        }

        /// <summary>
        /// Gets list of customers, each of which contains order statistics.
        /// </summary>
        /// <param name="sortExpression">Sortorder for returned customer list.</param>
        /// <returns>Sorted customer list with order stats.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IList<Customer> GetCustomersWithOrderStatistics(string sortExpression)
        {
            var request = new CustomerRequest().Prepare();
            request.LoadOptions = new string[] { "Customers" };
            request.Criteria = new CustomerCriteria { SortExpression = sortExpression, IncludeOrderStatistics = true };

            var response = Client.GetCustomers(request);

            Correlate(request, response);

            return response.Customers;
        }

        /// <summary>
        /// Gets a specific customer record.
        /// </summary>
        /// <param name="customerId">Unique customer object.</param>
        /// <returns>Customer object.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public Customer GetCustomer(int customerId)
        {
            var request = new CustomerRequest().Prepare();
            request.LoadOptions = new string[] { "Customer" };
            request.Criteria = new CustomerCriteria { CustomerId = customerId };

            var response = Client.GetCustomers(request);

            Correlate(request, response);

            return response.Customer;
        }

        /// <summary>
        /// Get customer object with all its orders.
        /// </summary>
        /// <param name="customerId">Unique customer identifier.</param>
        /// <returns>Customer object.</returns>
        public Customer GetCustomerWithOrders(int customerId)
        {
            var request = new CustomerRequest().Prepare();
            request.LoadOptions = new string[] { "Customer", "Orders" };
            request.Criteria = new CustomerCriteria { CustomerId = customerId };

            var response = Client.GetCustomers(request);

            Correlate(request, response);

            return response.Customer;
        }

        /// <summary>
        /// Adds a new customer to the database
        /// </summary>
        /// <param name="customer">Customer object to be added.</param>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public void AddCustomer(Customer customer)
        {
            var request = new CustomerRequest().Prepare();
            request.Action = "Create";
            request.Customer = customer;

            var response = Client.SetCustomers(request);

            Correlate(request, response);

            // These messages are for public consumption. Includes validation errors.
            if (response.Acknowledge == AcknowledgeType.Failure)
                throw new ApplicationException(response.Message);
        }

        /// <summary>
        /// Updates a customer in the database.
        /// </summary>
        /// <param name="customer">Customer object with updated values.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public void UpdateCustomer(Customer customer)
        {
            var request = new CustomerRequest().Prepare();
            request.Action = "Update";
            request.Customer = customer;

            var response = Client.SetCustomers(request);

            Correlate(request, response);

            // These messages are for public consumption. Includes validation errors.
            if (response.Acknowledge == AcknowledgeType.Failure)
                throw new ApplicationException(response.Message);
        }

        /// <summary>
        /// Deletes a customer from the database. A customer can only be deleted if no orders were placed.
        /// </summary>
        /// <param name="customerId">Unique customer identifier.</param>
        /// <returns>Number of orders deleted.</returns>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public int DeleteCustomer(int customerId)
        {
            var request = new CustomerRequest().Prepare();
            request.Action = "Delete";
            request.Criteria = new CustomerCriteria { CustomerId = customerId };

            var response = Client.SetCustomers(request);

            Correlate(request, response);

            if (response.Acknowledge == AcknowledgeType.Failure)
                throw new ApplicationException(response.Message);

            return response.RowsAffected;
        }
    }
}