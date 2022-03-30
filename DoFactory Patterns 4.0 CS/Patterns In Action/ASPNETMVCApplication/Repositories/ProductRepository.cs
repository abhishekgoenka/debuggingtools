using System;
using System.Collections.Generic;
using System.Linq;

using ASPNETMVCApplication.ActionServiceReference;
using ASPNETMVCApplication.Areas.Shop;
using ASPNETMVCApplication.Repositories.Core;

namespace ASPNETMVCApplication.Repositories
{
    /// <summary>
    /// Product Repository.
    /// </summary>
    public class ProductRepository : RepositoryBase, IProductRepository
    {
        /// <summary>
        /// Gets list of products.
        /// </summary>
        /// <param name="criterion"></param>
        /// <returns></returns>
        public List<Product> GetList(Criterion criterion = null)
        {
            var request = new ProductRequest().Prepare();
            
            request.LoadOptions = new string[] { "Products" };
            request.Criteria = new ProductCriteria
            {
                CategoryId = int.Parse(criterion.Filters.Single(f => f.Attribute == "CategoryId").Operand.ToString()),
                SortExpression = criterion.OrderByExpression
            };

            var response = Client.GetProducts(request);

            Correlate(request, response);

            return response.Products == null ? null : response.Products.ToList();
        }

        /// <summary>
        /// Searches for products according to several search criteria.
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="priceRangeId"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public List<Product> Search(string productName, int priceRangeId, string sort, string order)
        {
            var request = new ProductRequest().Prepare();

            request.LoadOptions = new string[] { "Search" };

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
                SortExpression = sort + " " + order
            };

            var response = Client.GetProducts(request);

            Correlate(request, response);

            return response.Products.ToList();
        }

        /// <summary>
        /// Gets a product by productId.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Product Get(int productId)
        {
            var request = new ProductRequest().Prepare();
            request.LoadOptions = new string[] { "Product" };
            request.Criteria = new ProductCriteria { ProductId = productId };

            var response = Client.GetProducts(request);

            Correlate(request, response);

            return response.Product;
        }

        #region Not implemented members

        public int GetCount(Criterion criterion = null)
        {
            throw new NotImplementedException();
        }

        public void Insert(Product t)
        {
            throw new NotImplementedException();
        }

        public void Update(Product t)
        {
            throw new NotImplementedException();
        }

        public void Delete(int productId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}