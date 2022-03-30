using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObjects;

namespace DataObjects.AdoNet.Oracle
{
    /// <summary>
    /// Oracle specific data access object that handles data access of order details.
    /// </summary>
    public class OracleOrderDetailDao : IOrderDetailDao
    {
         /// <summary>
        /// Gets a list of order details for a given order. Stubbed.
        /// </summary>
        /// <param name="orderId">Unique order identifier.</param>
        /// <returns>List of order details.</returns>
        public List<OrderDetail> GetOrderDetails(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}
