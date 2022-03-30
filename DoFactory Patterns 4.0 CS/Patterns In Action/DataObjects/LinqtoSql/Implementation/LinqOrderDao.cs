using System;
using System.Collections.Generic;
using System.Linq;

using DataObjects.LinqToSql.ModelMapper;
using BusinessObjects;

namespace DataObjects.LinqToSql.Implementation
{
    /// <summary>
    /// Linq-to-Sql implementation of the IOrderDao interface.
    /// </summary>
    public class LinqOrderDao : IOrderDao
    {
        /// <summary>
        /// Gets order given an order identifier.
        /// </summary>
        /// <param name="orderId">Order identifier.</param>
        /// <returns>The order.</returns>
        public Order GetOrder(int orderId)
        {
            using (var context = DataContextFactory.CreateContext())
            {
                return Mapper.Map(context.OrderEntities.SingleOrDefault(o => o.OrderId == orderId));
            }
        }

        /// <summary>
        /// Gets all orders for a given customer.
        /// </summary>
        /// <param name="customerId">Unique customer identifier.</param>
        /// <returns>List of orders.</returns>
        public List<Order> GetOrdersByCustomer(int customerId)
        {
            using (var context = DataContextFactory.CreateContext())
            {
                return context.OrderEntities
                    .Where(o => o.CustomerId == customerId)
                    .Select(c => Mapper.Map(c)).ToList();
            }
        }

        /// <summary>
        /// Gets the orders between a given data range.
        /// </summary>
        /// <param name="dateFrom">Start date.</param>
        /// <param name="dateThru">End date.</param>
        /// <returns></returns>
        public List<Order> GetOrdersByDate(DateTime dateFrom, DateTime dateThru)
        {
            using (var context = DataContextFactory.CreateContext())
            {
                return context.OrderEntities
                    .Where(o => o.OrderDate >= dateFrom && o.OrderDate <= dateThru)
                    .Select(c => Mapper.Map(c)).ToList();
            }
        }
    }
}
