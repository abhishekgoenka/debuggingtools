using System.Collections.Generic;
using System.Linq;

using DataObjects.LinqToSql.ModelMapper;
using BusinessObjects;
using System.Linq.Dynamic;

namespace DataObjects.LinqToSql.Implementation
{
    /// <summary>
    /// Linq-to-Sql implementation of the ICustomerDao interface.
    /// </summary>
    public class LinqCustomerDao : ICustomerDao
    {
        /// <summary>
        /// Gets list of customers in given sortorder.
        /// </summary>
        /// <param name="sortExpression">The required sort order.</param>
        /// <returns>List of customers.</returns>
        public List<Customer> GetCustomers(string sortExpression)
        {
            using (var context = DataContextFactory.CreateContext())
            {
                return context.CustomerEntities.OrderBy(sortExpression).Select(c => Mapper.Map(c)).ToList();
            }
        }

        /// <summary>
        /// Gets a customer given a customer identifier.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>The customer.</returns>
        public Customer GetCustomer(int customerId)
        {
            using (var context = DataContextFactory.CreateContext())
            {
                return Mapper.Map(context.CustomerEntities
                            .SingleOrDefault(p => p.CustomerId == customerId));
            }
        }

        /// <summary>
        /// Gets customer given an order.
        /// </summary>
        /// <param name="orderId">The identifier for the order for which customer is requested.</param>
        /// <returns>The customer.</returns>
        public Customer GetCustomerByOrder(int orderId)
        {
            using (var context = DataContextFactory.CreateContext())
            {
                return Mapper.Map(context.CustomerEntities.SelectMany(c => context.OrderEntities
                    .Where(o => c.CustomerId == o.CustomerId && o.OrderId == orderId),
                     (c, o) => c).SingleOrDefault(c => true));
            }
        }

        /// <summary>
        /// Gets a list of customers with order summary statistics.
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        public List<Customer> GetCustomersWithOrderStatistics(string sortExpression)
        {
            using (var context = DataContextFactory.CreateContext())
            {
                var customers = context.CustomerEntities.Select(c =>
                    new Customer
                    {
                        CustomerId = c.CustomerId,
                        Company = c.CompanyName,
                        City = c.City,
                        Country = c.Country,
                        Version = c.Version.AsBase64String(),
                        NumOrders = context.OrderEntities.Where(o => o.CustomerId == c.CustomerId).Count(),
                        LastOrderDate = context.OrderEntities.Where(o => o.CustomerId == c.CustomerId).Max(o => o.OrderDate)
                    }).OrderBy(sortExpression.Replace("CompanyName", "Company")); // Fixup CompanyName because entity and object are different 

                // Exclude customers without orders
                return customers.Where(c => c.NumOrders > 0).ToList();
            }
        }

        /// <summary>
        /// Inserts a new customer record to the database.
        /// </summary>
        /// <param name="customer">The customer to be inserted.</param>
        public void InsertCustomer(Customer customer)
        {
            var entity = Mapper.Map(customer);

            using (var context = DataContextFactory.CreateContext())
            {
                context.CustomerEntities.InsertOnSubmit(entity);
                context.SubmitChanges();

                // update business object with new version and id
                customer.CustomerId = entity.CustomerId;
                customer.Version = VersionConverter.ToString(entity.Version);
            }
        }

        /// <summary>
        /// Updates a customer record in the database.
        /// </summary>
        /// <param name="customer">The customer with updated values.</param>
        /// <returns>Number of rows affected.</returns>
        public void UpdateCustomer(Customer customer)
        {
            var entity = Mapper.Map(customer);

            using (var context = DataContextFactory.CreateContext())
            {
                context.CustomerEntities.Attach(entity, true);
                context.SubmitChanges();

                // Update business object with new version
                customer.Version = VersionConverter.ToString(entity.Version);
            }
        }

        /// <summary>
        /// Deletes a customer record from the database.
        /// </summary>
        /// <param name="customer">The customer to be deleted.</param>
        /// <returns>Number of rows affected.</returns>
        public void DeleteCustomer(Customer customer)
        {
            var entity = Mapper.Map(customer);

            using (var context = DataContextFactory.CreateContext())
            {
                context.CustomerEntities.Attach(entity, false);
                context.CustomerEntities.DeleteOnSubmit(entity);
                context.SubmitChanges();
            }
        }
    }
}
