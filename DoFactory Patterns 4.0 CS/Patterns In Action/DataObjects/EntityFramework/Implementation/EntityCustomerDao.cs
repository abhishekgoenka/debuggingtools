using System;
using System.Linq;
using System.Collections.Generic;

using BusinessObjects;
using DataObjects.EntityFramework.ModelMapper;
using System.Linq.Dynamic;

namespace DataObjects.EntityFramework.Implementation
{
    /// <summary>
    /// Entity Framework implementation of the ICustomerDao interface.
    /// </summary>
    public class EntityCustomerDao : ICustomerDao
    {
        /// <summary>
        /// Gets list of customers in given sortorder.
        /// </summary>
        /// <param name="sortExpression">The required sort order.</param>
        /// <returns>List of customers.</returns>
        public List<Customer> GetCustomers(string sortExpression)
        {
            using (var context = DataObjectFactory.CreateContext())
            {
                var list = new List<Customer>();

                var customers = context.CustomerEntities.AsQueryable().OrderBy(sortExpression).ToList();
                foreach (var customer in customers)
                    list.Add(Mapper.Map(customer));

                return list;
            }
        }

        /// <summary>
        /// Gets a customer given a customer identifier.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>The customer.</returns>
        public Customer GetCustomer(int customerId)
        {
            using (var context = DataObjectFactory.CreateContext())
            {
                return Mapper.Map(context.CustomerEntities.FirstOrDefault(c => c.CustomerId == customerId));
            }
        }

        /// <summary>
        /// Gets customer given an order.
        /// </summary>
        /// <param name="orderId">The identifier for the order for which customer is requested.</param>
        /// <returns>The customer.</returns>
        public Customer GetCustomerByOrder(int orderId)
        {
            using (var context = DataObjectFactory.CreateContext())
            {
                var order = context.OrderEntities.Where(o => o.OrderId == orderId).SingleOrDefault();
                var customer = context.CustomerEntities.Where(c => c.CustomerId == order.CustomerId).SingleOrDefault();

                return Mapper.Map(customer);
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
            using (var context = DataObjectFactory.CreateContext())
            {
                // Get customers with order information. Only those who have orders.
                var customers = context.CustomerEntities.Include("Orders").Where(c => c.Orders.Count > 0);

                return customers.AsQueryable().Select(c =>
                    new Customer
                    {
                        CustomerId = c.CustomerId,
                        Company = c.CompanyName,
                        City = c.City,
                        Country = c.Country,
                        NumOrders = c.Orders.Where(o => o.Customer.CustomerId == c.CustomerId).Count(),
                        LastOrderDate = c.Orders.Where(o => o.Customer.CustomerId == c.CustomerId).Max(o => o.OrderDate)
                    })
                    .OrderBy(sortExpression.Replace("CompanyName", "Company")) // Fixup CompanyName because entity and object are different 
                    .ToList();
            }
        }

        /// <summary>
        /// Inserts a new customer record to the database.
        /// </summary>
        /// <param name="customer">The customer to be inserted.</param>
        public void InsertCustomer(Customer customer)
        {
            var entity = Mapper.Map(customer);

            using (var context = DataObjectFactory.CreateContext())
            {
                context.CustomerEntities.AddObject(entity);
                context.SaveChanges(); 

                // update business object with new version and id
                customer.CustomerId = entity.CustomerId;

                customer.Version = Convert.ToBase64String(entity.Version);
            }
        }

        /// <summary>
        /// Updates a customer record in the database.
        /// </summary>
        /// <param name="customer">The customer with updated values.</param>
        /// <returns>Number of rows affected.</returns>
        public void UpdateCustomer(Customer customer)
        {
            using (var context = DataObjectFactory.CreateContext())
            {
                var entity = context.CustomerEntities.Where(c => c.CustomerId == customer.CustomerId).SingleOrDefault();
                entity.CompanyName = customer.Company;
                entity.Country = customer.Country;
                entity.City = customer.City;

                context.CustomerEntities.ApplyCurrentValues(entity);
                context.SaveChanges();

                // Update business object with new version
                customer.Version = Convert.ToBase64String(entity.Version);
            }
        }

        /// <summary>
        /// Deletes a customer record from the database.
        /// </summary>
        /// <param name="customer">The customer to be deleted.</param>
        /// <returns>Number of rows affected.</returns>
        public void DeleteCustomer(Customer customer)
        {
            using (var context = DataObjectFactory.CreateContext())
            {
                var entity = context.CustomerEntities.Where(c => c.CustomerId == customer.CustomerId).SingleOrDefault();
                context.CustomerEntities.DeleteObject(entity);
                context.SaveChanges();
            }
        }
    }
}
