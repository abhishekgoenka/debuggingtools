using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BusinessObjects;

namespace DataObjects.AdoNet.SqlServer
{
    /// <summary>
    /// Sql Server specific data access object that handles data access
    /// of categories and products.
    /// </summary>
    public class SqlServerProductDao : IProductDao
    {
        /// <summary>
        /// Gets a list of products for a given category.
        /// </summary>
        /// <param name="categoryId">Unique category identifier.</param>
        /// <param name="sortExpression">Sort order.</param>
        /// <returns>Sorted list of products.</returns>
        public List<Product> GetProductsByCategory(int categoryId, string sortExpression)
        {
            string sql = 
            @"SELECT ProductId, ProductName, Weight, UnitPrice, UnitsInStock, P.Version 
                FROM [Product] P JOIN [Category] C ON P.CategoryId = C.CategoryId
               WHERE C.CategoryId = @CategoryId".OrderBy(sortExpression);

            object[] parms = { "@CategoryId", categoryId };
            return Db.ReadList(sql, Make, parms);
        }

        /// <summary>
        /// Performs a search for products given several criteria.
        /// </summary>
        /// <param name="productName">Product name criterium.</param>
        /// <param name="priceFrom">Low end of price range.</param>
        /// <param name="priceThru">High end of price range.</param>
        /// <param name="sortExpression">Sort order.</param>
        /// <returns>Sorted list of products.</returns>
        public List<Product> SearchProducts(string productName, double priceFrom, double priceThru, string sortExpression)
        {
            string sql = 
                @"SELECT ProductId, ProductName, Weight, UnitPrice, UnitsInStock, Version
                    FROM [Product] ";

            var where = new StringBuilder();
            if (!string.IsNullOrEmpty(productName))
                where.Append("  WHERE ProductName LIKE @ProductName ");

            if (priceFrom != -1 && priceThru != -1)
            {
                where.Append( where.Length == 0 ? " WHERE " : " AND ");
                where.Append("UnitPrice >= @PriceFrom AND ");
                where.Append("UnitPrice <= @PriceThru ");
            }

            sql += where.ToString().OrderBy(sortExpression);

            object[] parms = { "@ProductName", "%" + productName + "%", "@PriceFrom", priceFrom, "@PriceThru", priceThru };
            return Db.ReadList(sql, Make, parms);
        }

        /// <summary>
        /// Gets a product.
        /// </summary>
        /// <param name="id">Unique product identifier.</param>
        /// <returns>Product.</returns>
        public Product GetProduct(int productId)
        {
            string sql = 
             @"SELECT ProductId, ProductName, Weight, UnitPrice, UnitsInStock, P.Version, 
                      C.CategoryId, CategoryName, Description 
                 FROM [Product] P JOIN [Category] C ON P.CategoryId = C.CategoryId
                WHERE P.ProductId = @ProductId";

            object[] parms =  {"@ProductId", productId};
            return Db.Read(sql, Make, parms);
        }

        /// <summary>
        /// Creates Product object from IDataReader.
        /// </summary>
        private static Func<IDataReader, Product> Make = reader =>
          new Product
          {
              ProductId = reader["ProductId"].AsId(),
              ProductName = reader["ProductName"].AsString(),
              Weight = reader["Weight"].AsString(),
              UnitPrice = reader["UnitPrice"].AsDouble(),
              UnitsInStock = reader["UnitsInStock"].AsInt(),
              Version = reader["Version"].AsBase64String()
          };
        

        /// <summary>
        /// Creates query parameter list from Product object.
        /// </summary>
        /// <param name="product">The product</param>
        /// <returns>Name value parameter list</returns>
        private object[] Take(Product product)
        {
            return new object[]  
            {
                "@ProductId", product.ProductId,
                "@ProductName", product.ProductName,
                "@Weight", product.Weight,
                "@UnitPrice", product.UnitPrice,
                "@UnitsInStock", product.UnitsInStock,
			    "@Version", product.Version.AsByteArray()
            };
        }
    }
}
