using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObjects;

namespace DataObjects
{
    /// <summary>
    /// Defines methods to access order details.
    /// </summary>
    /// <remarks>
    /// This is a database-independent interface. Implementations are database specific.
    /// </remarks>
    public interface IOrderDetailDao
    {
        /// <summary>
        /// Gets a list of order details for a given order.
        /// </summary>
        /// <param name="orderId">Unique order identifier.</param>
        /// <returns>List of order details.</returns>
        List<OrderDetail> GetOrderDetails(int orderId);
    }
}
