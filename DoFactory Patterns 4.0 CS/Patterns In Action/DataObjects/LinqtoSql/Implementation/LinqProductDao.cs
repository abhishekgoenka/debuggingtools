using System.Collections.Generic;
using System.Linq;

using DataObjects.LinqToSql.ModelMapper;
using BusinessObjects;
using System.Linq.Dynamic;

namespace DataObjects.LinqToSql.Implementation
{
    /// <summary>
    /// Linq-to-Sql implementation of the IProductDao interface.
    /// </summary>
    public class LinqProductDao : IProductDao
    {
        /// <summary>
        /// Gets a product given a product identifier.
        /// </summary>
        /// <param name="productId">Product identifier.</param>
        /// <returns>The product.</returns>
        public Product GetProduct(int productId)
        {
            using (var context = DataContextFactory.CreateContext())
            {
                return Mapper.Map(context.ProductEntities.SingleOrDefault(p => p.ProductId == productId));
            }
        }

        /// <summary>
        /// Gets list of product categories for a given category.
        /// </summary>
        /// <param name="categoryId">The category for which products are requested.</param>
        /// <param name="sortExpression">Sort order.</param>
        /// <returns>List of products.</returns>
        public List<Product> GetProductsByCategory(int categoryId, string sortExpression)
        {
            using (var context = DataContextFactory.CreateContext())
            {
                // build query tree
                return context.ProductEntities.Where(p => p.CategoryId == categoryId)
                    .OrderBy(sortExpression).Select(p => Mapper.Map(p)).ToList();
            }
        }

        /// <summary>
        /// Searches for products given a set of criteria.
        /// </summary>
        /// <param name="productName">Product Name criterium. Could be partial.</param>
        /// <param name="priceFrom">Minimumn price criterium.</param>
        /// <param name="priceThru">Maximumn price criterium.</param>
        /// <param name="sortExpression">Sort order in which to return product list.</param>
        /// <returns>List of found products.</returns>
        public List<Product> SearchProducts(string productName, double priceFrom, double priceThru, string sortExpression)
        {
            using (var context = DataContextFactory.CreateContext())
            {
                var query = context.ProductEntities.AsQueryable();
                if (!string.IsNullOrEmpty(productName))
                    query = query.Where(p => p.ProductName.StartsWith(productName));

                if (priceFrom != -1 && priceThru != -1)
                    query = query.Where(p => p.UnitPrice >= (decimal)priceFrom && p.UnitPrice <= (decimal)priceThru);

                return query.OrderBy(sortExpression,null).Select(p => Mapper.Map(p)).ToList();
            }
        }
    }
}
