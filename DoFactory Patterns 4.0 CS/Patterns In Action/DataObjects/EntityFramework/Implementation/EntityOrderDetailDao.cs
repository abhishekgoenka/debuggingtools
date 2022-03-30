using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObjects;
using DataObjects.EntityFramework.ModelMapper;

namespace DataObjects.EntityFramework.Implementation
{
    /// <summary>
    /// Entity Framework implementation of the IOrderDetailDao interface.
    /// </summary>
    public class EntityOrderDetailDao : IOrderDetailDao
    {
        /// <summary>
        /// Gets the orderdetails for a given order.
        /// </summary>
        /// <param name="orderId">Unique order identifier.</param>
        /// <returns>List of orderdetails.</returns>
        public List<OrderDetail> GetOrderDetails(int orderId)
        {
            using (var context = DataObjectFactory.CreateContext())
            {
                var orderDetails = context.OrderDetailEntities.Include("Product").Where(d => d.OrderId == orderId).ToList();

                var list = new List<OrderDetail>();
                foreach (var orderDetail in orderDetails)
                    list.Add(Mapper.Map(orderDetail));

                return list;
            }
        }
    }
}
