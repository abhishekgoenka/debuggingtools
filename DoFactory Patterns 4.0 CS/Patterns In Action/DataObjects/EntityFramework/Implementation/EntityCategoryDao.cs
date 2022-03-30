using System.Linq;
using System.Collections.Generic;

using BusinessObjects;
using DataObjects.EntityFramework.ModelMapper;

namespace DataObjects.EntityFramework.Implementation
{
    /// <summary>
    /// Entity Framework implementation of the ICategoryDao interface.
    /// </summary>
    public class EntityCategoryDao : ICategoryDao
    {
        /// <summary>
        /// Gets list of product categories
        /// </summary>
        /// <returns>List of categories.</returns>
        public List<Category> GetCategories()
        {
            using (var context = DataObjectFactory.CreateContext()) 
            {
                var list = new List<Category>();

                var categories = context.CategoryEntities.ToList();
                foreach(var category in categories)
                    list.Add(Mapper.Map(category));

                return list;
            }
        }

        /// <summary>
        /// Gets category for a given a product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The category.</returns>
        public Category GetCategoryByProduct(int productId)
        {
            using (var context = DataObjectFactory.CreateContext())
            {
                 var category = ( context.ProductEntities.Include("Category")
                    .FirstOrDefault(p => p.ProductId == productId).Category );

                 return Mapper.Map(category);
            }
        }
    }
}
