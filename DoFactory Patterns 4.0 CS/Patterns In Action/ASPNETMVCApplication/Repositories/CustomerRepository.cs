using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPNETMVCApplication.ActionServiceReference;
using ASPNETMVCApplication.Repositories.Core;

namespace ASPNETMVCApplication.Repositories
{
    /// <summary>
    /// Customer Repository. Good example of CRUD operations.
    /// </summary>
    public class CustomerRepository : RepositoryBase, ICustomerRepository
    {
        /// <summary>
        /// Gets list of customers.
        /// </summary>
        /// <param name="criterion"></param>
        /// <returns></returns>
        public List<Customer> GetList(Criterion criterion = null)
        {
            var request = new CustomerRequest().Prepare();
            request.LoadOptions = new string[] { "Customers" };
            request.Criteria = new CustomerCriteria { SortExpression = criterion.OrderByExpression };

            var response = Client.GetCustomers(request);

            Correlate(request, response);

            return response.Customers.ToList();
        }

        /// <summary>
        /// Gets an individual customer by id.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public Customer Get(int customerId)
        {
            var request = new CustomerRequest().Prepare();
            request.LoadOptions = new string[] { "Customer" };
            request.Criteria = new CustomerCriteria { CustomerId = customerId };

            var response = Client.GetCustomers(request);

            Correlate(request, response);

            return response.Customer;
        }

        /// <summary>
        /// Inserts a new customers.
        /// </summary>
        /// <param name="customer"></param>
        public void Insert(Customer customer)
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
        /// Updates an existing customer.
        /// </summary>
        /// <param name="customer"></param>
        public void Update(Customer customer)
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
        /// Deletes an existing customer.
        /// </summary>
        /// <param name="customerId"></param>
        public void Delete(int customerId)
        {
            var request = new CustomerRequest().Prepare();

            request.Action = "Delete";
            request.Criteria = new CustomerCriteria { CustomerId = customerId };

            var response = Client.SetCustomers(request);

            Correlate(request, response);

            if (response.Acknowledge == AcknowledgeType.Failure)
                throw new ApplicationException(response.Message);

            // return response.RowsAffected;
        }

       

        // Ancillary / Extraneous method
        public List<Customer> GetCustomerListWithOrderStatistics(Criterion criterion)
        {
            var request = new CustomerRequest().Prepare();
            request.LoadOptions = new string[] { "Customers" };
            request.Criteria = new CustomerCriteria { SortExpression = criterion.OrderByExpression, IncludeOrderStatistics = true };

            var response = Client.GetCustomers(request);

            Correlate(request, response);

            return response.Customers.ToList();
        }


        /// <summary>
        /// Gets a customer with associated list of orders.
        /// </summary>
        /// <param name="criterion"></param>
        /// <returns></returns>
        public Customer GetCustomerWithOrders(Criterion criterion)
        {
            var request = new CustomerRequest().Prepare();
            request.LoadOptions = new string[] { "Customer", "Orders" };
            request.Criteria = new CustomerCriteria { CustomerId = int.Parse(criterion.Filters.Single(f => f.Attribute.ToLower() == "customerid").Operand.ToString()) };

            var response = Client.GetCustomers(request);

            Correlate(request, response);

            return response.Customer;
        }

        #region Not implemented members

        public int GetCount(Criterion criterion = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}