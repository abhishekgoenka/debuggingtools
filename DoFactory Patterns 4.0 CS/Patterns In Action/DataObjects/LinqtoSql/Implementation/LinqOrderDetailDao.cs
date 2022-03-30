using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObjects;
using DataObjects.LinqToSql.ModelMapper;

namespace DataObjects.LinqToSql.Implementation
{
    /// <summary>
    /// Linq-to-Sql implementation of the IOrderDetailDao interface.
    /// </summary>
    public class LinqOrderDetailDao : IOrderDetailDao
    {
        /// <summary>
        /// Gets the orderdetails for a given order.
        /// </summary>
        /// <param name="orderId">Unique order identifier.</param>
        /// <returns>List of orderdetails.</returns>
        public List<OrderDetail> GetOrderDetails(int orderId)
        {
            using (var context = DataContextFactory.CreateContext())
            {
                return context.OrderDetailEntities.Where(d => d.OrderId == orderId)
                         .Select(d => Mapper.Map(d)).ToList();
            }
        }
    }
}
