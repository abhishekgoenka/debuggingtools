using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObjects;
using System.Data;

namespace DataObjects.AdoNet.Access
{
    /// <summary>
    /// Microsoft Access specific data access object that handles data access of product categories.
    /// </summary>
    public class AccessCategoryDao : ICategoryDao
    {
        /// <summary>
        /// Gets a list of categories.
        /// </summary>
        /// <returns>Category list.</returns>
        public List<Category> GetCategories()
        {
            string sql =
            @"SELECT CategoryId, CategoryName, Description 
                FROM [Category]";

            return Db.ReadList(sql, Make);
        }

        /// <summary>
        /// Gets a category for a given product.
        /// </summary>
        /// <param name="productId">Unique product identifier.</param>
        /// <returns>Category.</returns>
        public Category GetCategoryByProduct(int productId)
        {
            string sql =
            @"SELECT C.CategoryId, CategoryName, Description 
                FROM [Category] C INNER JOIN [Product] P ON P.CategoryId = C.CategoryId 
               WHERE ProductId = @ProductId ";

            object[] parms = { "@ProductId", productId };
            return Db.Read(sql, Make, parms);
        }

        /// <summary>
        /// Creates a Category object based on DataReader.
        /// </summary>
        private static Func<IDataReader, Category> Make = reader =>
           new Category
           {
               CategoryId = reader["CategoryId"].AsId(),
               Name = reader["CategoryName"].AsString(),
               Description = reader["Description"].AsString()
           };


        /// <summary>
        /// Creates query parameter list from Category object.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>Parameter name value list.</returns>
        private object[] Take(Category category)
        {
            return new object[]  
            {
                "@CategoryId", category.CategoryId,
                "@CategoryName", category.Name,
                "@Description", category.Description
            };
        }
        
    }
}
