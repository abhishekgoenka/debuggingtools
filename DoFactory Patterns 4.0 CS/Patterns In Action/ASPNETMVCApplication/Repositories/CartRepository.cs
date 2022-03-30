using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPNETMVCApplication.ActionServiceReference;
using ASPNETMVCApplication.Repositories.Core;

namespace ASPNETMVCApplication.Repositories
{
    /// <summary>
    /// Shopping cart repository class.
    /// Good example of a repository that implements CRUD operations.
    /// </summary>
    public class CartRepository : RepositoryBase,  ICartRepository
    {
        /// <summary>
        /// Inserts a new shopping cart item.
        /// </summary>
        /// <param name="cartItem"></param>
        public void Insert(ShoppingCartItem cartItem)
        {
            var request = new CartRequest().Prepare();
            request.Action = "Create";
            request.CartItem = cartItem;
            
            var response = Client.SetCart(request);

            Correlate(request, response);
        }

        /// <summary>
        /// Updates a shopping cart item.
        /// </summary>
        /// <param name="cartItem"></param>
        public void Update(ShoppingCartItem cartItem)
        {
            var request = new CartRequest().Prepare();
            request.Action = "Update";
            request.CartItem = cartItem;

            var response = Client.SetCart(request);

            Correlate(request, response);
        }

        /// <summary>
        /// Deletes a shopping cart item from cart.
        /// </summary>
        /// <param name="productId"></param>
        public void Delete(int productId)
        {
            var request = new CartRequest().Prepare();
            request.Action = "Delete";
            request.CartItem = new ShoppingCartItem { Id = productId };
            
            var response = Client.SetCart(request);

            Correlate(request, response);
        }

        /// <summary>
        /// Gets the shopping cart.
        /// </summary>
        /// <returns></returns>
        public ShoppingCart GetCart()
        {
            var request = new CartRequest().Prepare();
            request.Action = "Read";
            
            var response = Client.GetCart(request);

            Correlate(request, response);

            return response.Cart;
        }

        /// <summary>
        /// Update shipping method to shopping cart.
        /// </summary>
        /// <param name="shippingMethod"></param>
        public void UpdateShippingMethod(string shippingMethod)
        {
            var request = new CartRequest().Prepare();
            request.Action = "Update";
            request.ShippingMethod = shippingMethod;
            
            var response = Client.SetCart(request);

            Correlate(request, response);
        }

        #region Not implemented members

        public List<ShoppingCartItem> GetList(Criterion criterion = null)
        {
            throw new NotImplementedException();
        }

        public ShoppingCartItem Get(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Criterion criterion = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}