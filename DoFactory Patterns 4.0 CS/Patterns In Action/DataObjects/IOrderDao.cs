using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BusinessObjects;

namespace DataObjects
{
    /// <summary>
    /// Defines methods to access orders.
    /// </summary>
    /// <remarks>
    /// This is a database-independent interface. Implementations are database specific.
    /// </remarks>
    public interface IOrderDao
    {
        /// <summary>
        /// Gets an specific order.
        /// </summary>
        /// <param name="orderId">Unique order identifier.</param>
        /// <returns>Order.</returns>
        Order GetOrder(int orderId);

        /// <summary>
        /// Gets all orders for a customer.
        /// </summary>
        /// <param name="customerId">Unique customer identifier.</param>
        /// <returns>List of orders.</returns>
        List<Order> GetOrdersByCustomer(int customerId);

        /// <summary>
        /// Gets a list of orders placed within a date range.
        /// </summary>
        /// <param name="dateFrom">Date range begin date.</param>
        /// <param name="dateThru">Date range end date.</param>
        /// <returns>List of orders.</returns>
        List<Order> GetOrdersByDate(DateTime dateFrom, DateTime dateThru);
    }
}
