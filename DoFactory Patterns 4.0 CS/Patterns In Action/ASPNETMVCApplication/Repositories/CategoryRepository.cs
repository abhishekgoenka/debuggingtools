using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPNETMVCApplication.ActionServiceReference;
using ASPNETMVCApplication.Repositories.Core;

namespace ASPNETMVCApplication.Repositories
{
    /// <summary>
    /// Product Category Repository. Implements just one method.
    /// </summary>
    public class CategoryRepository : RepositoryBase, ICategoryRepository
    {
        /// <summary>
        /// Gets list of categories.
        /// </summary>
        /// <param name="criterion"></param>
        /// <returns></returns>
        public List<Category> GetList(Criterion criterion = null)
        {
            var request = new ProductRequest().Prepare();
            request.LoadOptions = new string[] { "Categories" };

            ProductResponse response = Client.GetProducts(request);

            Correlate(request, response);

            return response.Categories == null ? null : response.Categories.ToList();
        }

        #region Not implemented members

        public Category Get(int categoryId)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Criterion criterion = null)
        {
            throw new NotImplementedException();
        }

        public void Insert(Category t)
        {
            throw new NotImplementedException();
        }

        public void Update(Category t)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}