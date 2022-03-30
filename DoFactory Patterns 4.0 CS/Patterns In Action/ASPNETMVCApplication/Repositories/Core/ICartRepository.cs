using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPNETMVCApplication.ActionServiceReference;

namespace ASPNETMVCApplication.Repositories
{
    /// <summary>
    /// Cart Repository interface.
    /// Derives from standard IRepository. Adds two cart specific members..
    /// </summary>
    public interface ICartRepository : IRepository<ShoppingCartItem>
    {
        ShoppingCart GetCart();
        void UpdateShippingMethod(string method);
    }
}