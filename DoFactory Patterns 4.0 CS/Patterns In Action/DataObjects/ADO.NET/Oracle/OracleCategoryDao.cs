using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObjects;
using System.Data;

namespace DataObjects.AdoNet.Oracle
{
    /// <summary>
    /// Oracle specific data access object that handles data access of product categories.
    /// </summary>
    public class OracleCategoryDao : ICategoryDao
    {
        /// <summary>
        /// Gets a list of categories. Stubbed.
        /// </summary>
        /// <returns>Category list.</returns>
        public List<Category> GetCategories()
        {
            throw new NotImplementedException("Oracle data access not implemented.");
        }

        /// <summary>
        /// Gets a category for a given product. Stubbed.
        /// </summary>
        /// <param name="productId">Unique product identifier.</param>
        /// <returns>Category.</returns>
        public Category GetCategoryByProduct(int productId)
        {
            throw new NotImplementedException("Oracle data access not implemented.");
        }
    }
}
