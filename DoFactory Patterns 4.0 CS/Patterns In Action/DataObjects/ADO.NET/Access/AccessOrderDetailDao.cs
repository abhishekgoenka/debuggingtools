using System;
using System.Collections.Generic;
using BusinessObjects;
using System.Data;

namespace DataObjects.AdoNet.Access
{
    /// <summary>
    /// Microsoft Access specific data access object that handles data access of order details.
    /// </summary>

    public class AccessOrderDetailDao : IOrderDetailDao
    {
        /// <summary>
        /// Gets a list of order details for a given order.
        /// </summary>
        /// <param name="orderId">Unique order identifier.</param>
        /// <returns>List of order details.</returns>
        public List<OrderDetail> GetOrderDetails(int orderId)
        {
            string sql =
            @"SELECT OrderId, O.ProductId, ProductName, O.UnitPrice, Quantity, Discount
                FROM [OrderDetail] O INNER JOIN [Product] P ON O.ProductId = P.ProductId 
               WHERE OrderId = @OrderId ";

            object[] parms = { "@OrderId", orderId };

            return Db.ReadList(sql, Make, parms);
        }

        /// <summary>
        /// Creates order detail object from IDataReader.
        /// </summary>
        private static Func<IDataReader, OrderDetail> Make = reader =>
          new OrderDetail
          {
              OrderId = reader["OrderId"].AsId(),
              ProductId = reader["OrderId"].AsId(),
              ProductName = reader["ProductName"].AsString(),
              Quantity = reader["Quantity"].AsInt(),
              UnitPrice = reader["UnitPrice"].AsDouble(),
              Discount = reader["Discount"].AsDouble()
          };

        /// <summary>
        /// Creates query parameter list from order detail object.
        /// </summary>
        /// <param name="orderDetail">The order detail</param>
        /// <returns>Name value parameter list</returns>
        private object[] Take(OrderDetail orderDetail)
        {
            return new object[]  
            {
                "OrderId", orderDetail.OrderId,
                "ProductId", orderDetail.ProductId,
                "ProductName", orderDetail.ProductName,
                "Quantity", orderDetail.Quantity,
                "UnitPrice", orderDetail.UnitPrice,
                "Discount", orderDetail.Discount
            };
        }
    }
}
