using System.Collections.Generic;
using System.ComponentModel;

using ASPNETWebApplication.ActionServiceReference;

namespace ASPNETWebApplication.Repositories
{
    /// <summary>
    /// Repository for products.
    /// </summary>
    /// <remarks>
    /// Repository Pattern.
    /// </remarks>
    [DataObject(true)]
    public class ProductRepository : RepositoryBase
    {
        /// <summary>
        /// Gets a list of product categories.
        /// </summary>
        /// <returns>List of categories.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IList<Category> GetCategories()
        {
            var request = new ProductRequest().Prepare();
            request.LoadOptions = new string[] { "Categories" };

            var response = Client.GetProducts(request);

            Correlate(request, response);

            return response.Categories;
        }

        /// <summary>
        /// Gets a list of products.
        /// </summary>
        /// <param name="categoryId">The category for which products are requested.</param>
        /// <param name="sortExpression">Sort order in which products are returned.</param>
        /// <returns>List of products.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IList<Product> GetProducts(int categoryId, string sortExpression)
        {
            var request = new ProductRequest().Prepare();
            request.LoadOptions = new string[] { "Products" };
            request.Criteria = new ProductCriteria
            {
                CategoryId = categoryId,
                SortExpression = sortExpression
            };
       
            var response = Client.GetProducts(request);

            Correlate(request, response);

            return response.Products;

        }

        /// <summary>
        /// Gets a specific product.
        /// </summary>
        /// <param name="productId">Unique product identifier.</param>
        /// <returns>The requested product.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public Product GetProduct(int productId)
        {
            var request = new ProductRequest().Prepare();
            request.LoadOptions = new string[] { "Product" };
            request.Criteria = new ProductCriteria { ProductId = productId };

            var response = Client.GetProducts(request);

            Correlate(request, response);

            return response.Product;

        }

        /// <summary>
        /// Searches for products.
        /// </summary>
        /// <param name="productName">Product name.</param>
        /// <param name="priceRangeId">Price range identifier.</param>
        /// <param name="sortExpression">Sort order in which products are returned.</param>
        /// <returns>List of products that meet the search criteria.</returns>
        public IList<Product> SearchProducts(string productName, int priceRangeId, string sortExpression)
        {
            var request = new ProductRequest().Prepare();
            request.LoadOptions = new string[] {  "Search" };

            double priceFrom = -1;
            double priceThru = -1;
            if (priceRangeId > 0)
            {
                PriceRangeItem pri = PriceRange.List[priceRangeId];
                priceFrom = pri.RangeFrom;
                priceThru = pri.RangeThru;
            }

            request.Criteria = new ProductCriteria
            {
                ProductName = productName,
                PriceFrom = priceFrom,
                PriceThru = priceThru,
                SortExpression = sortExpression
            };


            var response = Client.GetProducts(request);

            Correlate(request, response);

            return response.Products;
        }

        /// <summary>
        /// Gets a list of price ranges.
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IList<PriceRangeItem> GetProductPriceRange()
        {
            return PriceRange.List;
        }
    }
}
