using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using BusinessObjects;
using DataObjects.EntityFramework.ModelMapper;

namespace DataObjects.EntityFramework.Implementation
{
    /// <summary>
    /// Entity Framework implementation of the IOrderDao interface.
    /// </summary>
    public class EntityOrderDao : IOrderDao
    {
        /// <summary>
        /// Gets order given an order identifier.
        /// </summary>
        /// <param name="orderId">Order identifier.</param>
        /// <returns>The order.</returns>
        public Order GetOrder(int orderId)
        {
            using (var context = DataObjectFactory.CreateContext())
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
            using (var context = DataObjectFactory.CreateContext())
            {
                var orders = context.OrderEntities.Where(o => o.CustomerId == customerId).ToList();

                var list = new List<Order>();
                foreach (var order in orders)
                    list.Add(Mapper.Map(order));

                return list;
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
            using (var context = DataObjectFactory.CreateContext())
            {
                var orders = context.OrderEntities.Where(o => o.OrderDate >= dateFrom && o.OrderDate <= dateThru).ToList();

                var list = new List<Order>();
                foreach (var order in orders)
                    list.Add(Mapper.Map(order));

                return list;
            }
        }
    }
}
