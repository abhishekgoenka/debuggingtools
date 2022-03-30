using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObjects;
using DataObjects.EntityFramework.ModelMapper;

using System.Linq.Dynamic;

namespace DataObjects.EntityFramework.Implementation
{
    /// <summary>
    /// Entity Framework implementation of the IProductDao interface.
    /// </summary>
    public class EntityProductDao : IProductDao
    {
        /// <summary>
        /// Gets a product given a product identifier.
        /// </summary>
        /// <param name="productId">Product identifier.</param>
        /// <returns>The product.</returns>
        public Product GetProduct(int productId)
        {
            using (var context = DataObjectFactory.CreateContext())
            {
                return Mapper.Map(context.ProductEntities
                            .SingleOrDefault(p => p.ProductId == productId));
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
            using (var context = DataObjectFactory.CreateContext()) 
            {
                // Load single Category with all Product child records
                var category = context.CategoryEntities.Include("Products").FirstOrDefault(c => c.CategoryId == categoryId);

                // Order by (using dynamic linq) without going back to db
                var products = category.Products.AsQueryable().OrderBy(sortExpression, null); 
                
                // Return list of business objects
                return products.Select(p => Mapper.Map(p)).ToList();
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
            using (var context = DataObjectFactory.CreateContext())
            {
                var query = context.ProductEntities.AsQueryable();
                if (!string.IsNullOrEmpty(productName))
                    query = query.Where(p => p.ProductName.StartsWith(productName));

                if (priceFrom != -1 && priceThru != -1)
                    query = query.Where(p => p.UnitPrice >= (decimal)priceFrom && p.UnitPrice <= (decimal)priceThru);

                var products = query.OrderBy(sortExpression, null).ToList();

                return products.Select(p => Mapper.Map(p)).ToList();
            }
        }
    }
}
