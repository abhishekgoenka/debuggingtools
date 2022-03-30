using System.ComponentModel;

using ASPNETWebApplication.ActionServiceReference;

namespace ASPNETWebApplication.Repositories
{
    /// <summary>
    /// Repository for the shopping cart.
    /// </summary>
    /// <remarks>
    /// Repository Pattern.
    /// </remarks>
    [DataObject(true)]
    public class CartRepository : RepositoryBase
    {
        /// <summary>
        /// Gets the user's cart.
        /// </summary>
        /// <returns>Shopping cart.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public ShoppingCart GetCart()
        {
            var request = new CartRequest().Prepare();
            request.Action = "Read";

            var response = Client.GetCart(request);

            Correlate(request, response);

            return response.Cart;
        }

        /// <summary>
        /// Adds an item to the shopping cart.
        /// </summary>
        /// <param name="productId">Unique product identifier or item.</param>
        /// <param name="name">Item name.</param>
        /// <param name="quantity">Quantity of items.</param>
        /// <param name="unitPrice">Unit price for each item.</param>
        /// <returns>Updated shopping cart.</returns>
        public ShoppingCart AddItem(int productId, string name, int quantity, double unitPrice)
        {
            var request = new CartRequest().Prepare();
            request.Action = "Create";
            request.CartItem = new ShoppingCartItem { Id = productId, Name = name, Quantity = quantity, UnitPrice = unitPrice };

            var response = Client.SetCart(request);

            Correlate(request, response);

            return response.Cart;
        }

        /// <summary>
        /// Removes a line item from the shopping cart.
        /// </summary>
        /// <param name="productId">The item to be removed.</param>
        /// <returns>Updated shopping cart.</returns>
        public ShoppingCart RemoveItem(int productId)
        {
            var request = new CartRequest().Prepare();
            request.Action = "Delete";
            request.CartItem = new ShoppingCartItem { Id = productId };

            var response = Client.SetCart(request);

            Correlate(request, response);

            return response.Cart;
        }

        /// <summary>
        /// Updates a line item in the shopping cart with a new quantity.
        /// </summary>
        /// <param name="productId">Unique product line item.</param>
        /// <param name="quantity">New quantity.</param>
        /// <returns>Updated shopping cart.</returns>
        public ShoppingCart UpdateQuantity(int productId, int quantity)
        {
            var request = new CartRequest().Prepare();
            request.Action = "Update";
            request.CartItem = new ShoppingCartItem { Id = productId, Quantity = quantity };

            var response = Client.SetCart(request);

            Correlate(request, response);

            return response.Cart;
        }

        /// <summary>
        /// Sets shipping method used to compute shipping charges.
        /// </summary>
        /// <param name="shippingMethod">The name of the shipper.</param>
        /// <returns>Updated shopping cart.</returns>
        public ShoppingCart SetShippingMethod(string shippingMethod)
        {
            var request = new CartRequest().Prepare();
            request.Action = "Update";
            request.ShippingMethod = shippingMethod;

            var response = Client.SetCart(request);

            Correlate(request, response);

            return response.Cart;
        }
    }
}
