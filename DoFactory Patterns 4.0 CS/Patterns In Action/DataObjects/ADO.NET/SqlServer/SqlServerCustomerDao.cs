using System;
using System.Collections.Generic;
using System.Data;
using BusinessObjects;

namespace DataObjects.AdoNet.SqlServer
{
    /// <summary>
    /// Sql Server specific data access object that handles data access of customers.
    /// </summary>
    public class SqlServerCustomerDao : ICustomerDao
    {
        /// <summary>
        /// Gets a sorted list of all customers.
        /// </summary>
        /// <param name="sortExpression">Sort order.</param>
        /// <returns>Sorted list of customers.</returns>
        public List<Customer> GetCustomers(string sortExpression)
        {
            string sql =
            @"SELECT CustomerId, CompanyName, City, Country, Version
                FROM [Customer] ".OrderBy(sortExpression);

            return Db.ReadList(sql, Make);
        }

        /// <summary>
        /// Gets a customer.
        /// </summary>
        /// <param name="customerId">Unique customer identifier.</param>
        /// <returns>Customer.</returns>
        public Customer GetCustomer(int customerId)
        {
            string sql =
            @" SELECT CustomerId, CompanyName, City, Country, Version
                 FROM [Customer]
                WHERE CustomerId = @CustomerId";

            object[] parms = { "@CustomerId", customerId };
            return Db.Read(sql, Make, parms);
        }

        /// <summary>
        /// Gets customer given an order.
        /// </summary>
        /// <param name="orderId">Unique order identifier.</param>
        /// <returns>Customer.</returns>
        public Customer GetCustomerByOrder(int orderId)
        {
            string sql =
            @" SELECT C.CustomerId, CompanyName, City, Country, C.Version
                 FROM [Order] O JOIN [Customer] C ON O.CustomerId = C.CustomerId
                WHERE OrderId = @OrderId";

            object[] parms = { "@OrderId", orderId };
            return Db.Read(sql, Make, parms);
        }


        /// <summary>
        /// Gets customers with order statistics in given sort order.
        /// </summary>
        /// /// <param name="sortExpression">Sort order.</param>
        /// <returns>Sorted list of customers with order statistics.</returns>
        public List<Customer> GetCustomersWithOrderStatistics(string sortExpression)
        {
            string sql =
            @"SELECT C.CustomerId, CompanyName, City, Country, C.Version,
                     MAX(OrderDate) AS LastOrderDate, COUNT(OrderId) AS NumOrders 
                FROM [Order] O JOIN [Customer] C ON O.CustomerId = C.CustomerId
               GROUP BY C.CustomerId, CompanyName, City, Country, C.Version "
                    .OrderBy(sortExpression);

            return Db.ReadList(sql, MakeWithStats);
        }

        /// <summary>
        /// Inserts a new customer. 
        /// </summary>
        /// <remarks>
        /// Following insert, customer object will contain new identifier.
        /// </remarks>
        /// <param name="customer">Customer.</param>
        public void InsertCustomer(Customer customer)
        {
            string sql =
            @"INSERT INTO [Customer] (CompanyName, City, Country) 			
              VALUES (@CompanyName, @City, @Country)";

            customer.CustomerId = Db.Insert(sql, Take(customer));
            customer.Version = GetCustomer(customer.CustomerId).Version;
        }
        /// <summary>
        /// Updates a customer.
        /// </summary>
        /// <param name="customer">Customer.</param>
        /// <returns>Number of customer records updated.</returns>
        public void UpdateCustomer(Customer customer)
        {
            string sql =
            @"UPDATE [Customer]
                 SET CompanyName = @CompanyName,
                     City = @City,
                     Country = @Country
               WHERE CustomerId = @CustomerId
                 AND Version = @Version";

            Db.Update(sql, Take(customer));
        }

        /// <summary>
        /// Deletes a customer.
        /// </summary>
        /// <param name="customer">Customer.</param>
        /// <returns>Number of customer records deleted.</returns>
        public void DeleteCustomer(Customer customer)
        {
            string sql =
            @"DELETE FROM [Customer]
               WHERE CustomerId = @CustomerId 
                AND Version = @Version";

            Db.Update(sql, Take(customer));
        }

        /// <summary>
        /// Creates a Customer object based on DataReader.
        /// </summary>
        private static Func<IDataReader, Customer> Make = reader =>
           new Customer
           {
               CustomerId = reader["CustomerId"].AsId(),
               Company = reader["CompanyName"].AsString(),
               City = reader["City"].AsString(),
               Country = reader["Country"].AsString(),
               Version = reader["Version"].AsBase64String()
           };


        /// <summary>
        /// Creates a Customers object with order statistics based on DataReader.
        /// </summary>
        private static Func<IDataReader, Customer> MakeWithStats = reader =>
            {
                Customer customer = Make(reader);
                customer.NumOrders = reader["NumOrders"].AsInt();
                customer.LastOrderDate = reader["LastOrderDate"].AsDateTime();

                return customer;
            };

        /// <summary>
        /// Creates query parameters list from Customer object
        /// </summary>
        /// <param name="customer">Customer.</param>
        /// <returns>Name value parameter list.</returns>
        private object[] Take(Customer customer)
        {
            return new object[]  
            {
                "@CustomerId", customer.CustomerId,
                "@CompanyName", customer.Company,
                "@City", customer.City,
                "@Country", customer.Country,
			    "@Version", customer.Version.AsByteArray()
            };
        }
    }
}
