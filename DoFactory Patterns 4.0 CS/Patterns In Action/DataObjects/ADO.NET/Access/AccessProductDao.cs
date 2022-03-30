using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BusinessObjects;

namespace DataObjects.AdoNet.Access
{
    /// <summary>
    /// Microsoft Access specific data access object that handles data access
    /// of categories and products.
    /// </summary>
    public class AccessProductDao : IProductDao
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
            @"SELECT ProductId, ProductName, Weight, UnitPrice, UnitsInStock
                FROM [Product] AS P INNER JOIN [Category] AS C ON P.CategoryId = C.CategoryId
               WHERE C.CategoryId = @CategoryId ".OrderBy(sortExpression);

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
            @"SELECT ProductId, ProductName, Weight, UnitPrice, UnitsInStock
                FROM [Product] ";

            var where = new StringBuilder();
            List<object> parms1 = new List<object>();
            List<object> parms2 = new List<object>();

            if (!string.IsNullOrEmpty(productName))
            {
                where.Append("  WHERE ProductName LIKE @ProductName ");
                parms1.AddRange(new object[] { "@ProductName", productName + "%" });
            }

            if (priceFrom != -1 && priceThru != -1)
            {
                where.Append(where.Length == 0 ? " WHERE " : " AND ");
                where.Append("UnitPrice >= @PriceFrom AND ");
                where.Append("UnitPrice <= @PriceThru ");
                parms2.AddRange(new object[] { "@PriceFrom", priceFrom, "@PriceThru", priceThru });
            }

            sql += where.ToString().OrderBy(sortExpression);

            // In MS Access, only include parameters referenced in sql
            parms1.AddRange(parms2);
            object[] parms = parms1.ToArray();
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
             @"SELECT ProductId, ProductName, Weight, UnitPrice, UnitsInStock,
                      C.CategoryId, CategoryName, Description 
                 FROM [Product] AS P INNER JOIN [Category] AS C ON P.CategoryId = C.CategoryId
                WHERE P.ProductId = @ProductId ";

            object[] parms = { "@ProductId", productId };
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
              UnitsInStock = reader["UnitsInStock"].AsInt()
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
                "@UnitsInStock", product.UnitsInStock
            };
        }
    }
}
