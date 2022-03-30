using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETMVCApplication.Areas.Shop.Models
{
    /// <summary>
    /// Cart item (line item) model.
    /// </summary>
    public class CartItemModel
    {
        /// <summary>
        /// Unique identifier of cart item.
        /// </summary>
        public int CartItemId { get; set; }

        /// <summary>
        /// Product identifier.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Quantity of items.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Product name of item.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Price per item
        /// </summary>
        public string UnitPrice { get; set; }

        /// <summary>
        /// Total price of cart line item (unitprice * quantity).
        /// </summary>
        public string TotalPrice { get; set; }
    }
}