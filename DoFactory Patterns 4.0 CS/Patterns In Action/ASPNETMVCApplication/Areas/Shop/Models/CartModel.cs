using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETMVCApplication.Areas.Shop.Models
{
    /// <summary>
    /// Shopping cart model. 
    /// Maintains information of the cart as a whole.
    /// </summary>
    public class CartModel
    {
        /// <summary>
        /// The shopping cart items.
        /// </summary>
        public List<CartItemModel> CartItems { get; set; }

        /// <summary>
        /// Shipping method selected.
        /// </summary>
        public string ShippingMethod { get; set; }

        /// <summary>
        /// Subtotal of all cart items.
        /// </summary>
        public string SubTotal { get; set; }

        /// <summary>
        /// Grand total of entire cart (Subtotal + Shipping)
        /// </summary>
        public string Total { get; set; }

        /// <summary>
        /// Shipping cost for entire cart.
        /// </summary>
        public string Shipping { get; set; }
    }
}