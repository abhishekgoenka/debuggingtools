using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BusinessObjects;

namespace DataObjects
{
    /// <summary>
    /// Defines methods to access categories.
    /// </summary>
    /// <remarks>
    /// This is a database-independent interface. Implementations are database specific.
    /// </remarks>
    public interface ICategoryDao
    {
        /// <summary>
        /// Gets a list of product categories.
        /// </summary>
        /// <returns>List of product categories.</returns>
        List<Category> GetCategories();

        /// <summary>
        /// Gets a product category for a given product.
        /// </summary>
        /// <param name="productId">Unique product identifier.</param>
        /// <returns>Product category.</returns>
        Category GetCategoryByProduct(int productId);
    }
}
