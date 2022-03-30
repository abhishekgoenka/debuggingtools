using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BusinessObjects;

namespace DataObjects.AdoNet.Access
{
    /// <summary>
    /// Microsoft Access specific data access object that handles data access of customers.
    /// </summary>
    public class AccessCustomerDao : ICustomerDao
    {
        /// <summary>
        /// Gets a sorted list of all customers.
        /// </summary>
        /// <param name="sortExpression">Sort order.</param>
        /// <returns>Sorted list of customers.</returns>
        public List<Customer> GetCustomers(string sortExpression)
        {
            string sql =
            @"SELECT CustomerId, CompanyName, City, Country
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
            @" SELECT CustomerId, CompanyName, City, Country
                 FROM [Customer]
                WHERE CustomerId = @CustomerId ";

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
            @" SELECT C.CustomerId, CompanyName, City, Country
                 FROM [Order] AS O INNER JOIN [Customer] AS C ON O.CustomerId = C.CustomerId
                WHERE OrderId = @OrderId ";

            object[] parms = { "@OrderId", orderId };
            return Db.Read(sql, Make, parms);
        }


        /// <summary>
        /// Gets customers with order statistics in given sort order.
        /// </summary>
        /// <param name="customers">Customer list.</param>
        /// /// <param name="sortExpression">Sort order.</param>
        /// <returns>Sorted list of customers with order statistics.</returns>
        public List<Customer> GetCustomersWithOrderStatistics(string sortExpression)
        {
            // MS Access fixup: prefix ambiguous column name and replace aliased names
            if (sortExpression.ToLower().StartsWith("customerid")) sortExpression = "C." + sortExpression;
            if (sortExpression.StartsWith("NumOrders")) sortExpression = sortExpression.Replace("NumOrders", "COUNT(OrderId)");
            if (sortExpression.StartsWith("LastOrderDate")) sortExpression = sortExpression.Replace("LastOrderDate", "MAX(OrderDate)");
            
            string sql =
            @"SELECT C.CustomerId, CompanyName, City, Country, 
                     MAX(OrderDate) AS LastOrderDate, COUNT(OrderId) AS NumOrders 
                FROM [Order] AS O INNER JOIN [Customer] AS C ON O.CustomerId = C.CustomerId
               GROUP BY C.CustomerId, CompanyName, City, Country "
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
               WHERE CustomerId = @CustomerId";

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
               WHERE CustomerId = @CustomerId";

            Db.Update(sql, new object[] { "@CustomerId", customer.CustomerId });
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
               Country = reader["Country"].AsString()
           };

        /// <summary>
        /// Creates a Customer object with order statistics based on DataReader.
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
            // Note: Parameters in MS Access are positional. This means that the order of parameters 
            // should match the order in which parameters appear in the sql.
            return new object[]  
            {
                "@CompanyName", customer.Company,
                "@City", customer.City,
                "@Country", customer.Country,
                "@CustomerId", customer.CustomerId
            };
        }
    }
}
