using System;
using System.Collections.Generic;
using System.Data;
using BusinessObjects;

namespace DataObjects.AdoNet.SqlServer
{
    /// <summary>
    /// Sql Server specific data access object that handles data access
    /// of customer related orders and order details.
    /// </summary>
    public class SqlServerOrderDao : IOrderDao
    {
        /// <summary>
        /// Gets an order.
        /// </summary>
        /// <param name="orderId">Unique order identifier.</param>
        /// <returns>Order.</returns>
        public Order GetOrder(int orderId)
        {
            string sql =
            @"SELECT OrderId, OrderDate, RequiredDate, Freight, Version
              FROM [Order] 
             WHERE OrderId = @OrderId";

            object[] parms = { "@OrderId", orderId };
            return Db.Read(sql, Make, parms);
        }

        /// <summary>
        /// Gets all orders for a customer.
        /// </summary>
        /// <param name="customerId">Unique customer identifier.</param>
        /// <returns>List of orders.</returns>
        public List<Order> GetOrdersByCustomer(int customerId)
        {
            string sql =
              @" SELECT OrderId, OrderDate, RequiredDate, Freight, Version 
                   FROM [Order]
                  WHERE CustomerId = @CustomerId
               ORDER BY OrderDate ASC";

            object[] parms = { "@CustomerId", customerId };
            return Db.ReadList(sql, Make, parms);
        }

        /// <summary>
        /// Gets a list of orders placed within a date range.
        /// </summary>
        /// <param name="dateFrom">Date range begin date.</param>
        /// <param name="dateThru">Date range end date.</param>
        /// <returns>List of orders.</returns>
        public List<Order> GetOrdersByDate(DateTime dateFrom, DateTime dateThru)
        {
            string sql =
            @" SELECT OrderId, OrderDate, RequiredDate, Freight, Version
                 FROM [Order]
                WHERE OrderDate >= @DateFrom
                  AND OrderDate <= @DateThru
                ORDER BY OrderDate ASC ";

            object[] parms = { "@DateFrom", dateFrom, "@DateThru", dateThru };
            return Db.ReadList(sql, Make, parms);
        }

        /// <summary>
        /// Creates an Order object based on DataReader.
        /// </summary>
        private static Func<IDataReader, Order> Make = reader =>
           new Order
           {
               OrderId = reader["OrderId"].AsId(),
               OrderDate = reader["OrderDate"].AsDateTime(),
               RequiredDate = reader["RequiredDate"].AsDateTime(),
               Freight = reader["Freight"].AsDouble(),
               Version = reader["Version"].AsBase64String()
           };

        /// <summary>
        /// Creates query parameters list from Order object.
        /// </summary>
        /// <param name="order">Order.</param>
        /// <returns>Name value parameter list.</returns>
        private object[] Take(Order order)
        {
            return new object[]  
            {
                "@OrderId", order.OrderId,
                "@OrderDate", order.OrderDate,
                "@RequiredDate", order.RequiredDate,
                "@Freight", order.Freight,
			    "@Version", order.Version.AsByteArray()
            };
        }
    }
}
