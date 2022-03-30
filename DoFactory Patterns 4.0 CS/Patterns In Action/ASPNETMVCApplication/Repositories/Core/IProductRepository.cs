using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ASPNETMVCApplication.ActionServiceReference;

namespace ASPNETMVCApplication.Repositories
{
    /// <summary>
    /// Product Repository interface.
    /// Derives from standard IRepository. Adds one product specific member.
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
        List<Product> Search(string productName, int priceRangeId, string sort, string order);
    }
}