using System.Collections.Generic;
using System.Linq;
using BusinessObjects;
using DataObjects.LinqToSql.ModelMapper;

namespace DataObjects.LinqToSql.Implementation
{
    /// <summary>
    /// Linq-to-Sql implementation of the ICategoryDao interface.
    /// </summary>
    public class LinqCategoryDao : ICategoryDao
    {
        /// <summary>
        /// Gets list of product categories
        /// </summary>
        /// <returns>List of categories.</returns>
        public List<Category> GetCategories()
        {
            using (var context = DataContextFactory.CreateContext())
            {
                return context.CategoryEntities.Select(c => Mapper.Map(c)).ToList();
            }
        }

        /// <summary>
        /// Gets category for a given a product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The category.</returns>
        public Category GetCategoryByProduct(int productId)
        {
            using (var context = DataContextFactory.CreateContext())
            {
                return Mapper.Map(context.CategoryEntities.SelectMany(c => context.ProductEntities
                    .Where(p => c.CategoryId == p.CategoryId)
                    .Where(p => p.ProductId == productId),
                     (c, p) => c).SingleOrDefault(c => true));
            }
        }
    }
}
