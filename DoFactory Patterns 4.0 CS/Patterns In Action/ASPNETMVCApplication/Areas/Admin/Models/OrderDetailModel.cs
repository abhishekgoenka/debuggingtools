using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETMVCApplication.Areas.Admin.Models
{
    /// <summary>
    /// Order Detail Model class (represents one line item).
    /// </summary>
    public class OrderDetailModel
    {
        /// <summary>
        /// The order detail identifier.
        /// </summary>
        public int OrderDetailId;

        /// <summary>
        /// The order identifier.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// The product identifier.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// The product name.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// The quantity of product items.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The price per unit of product.
        /// </summary>
        public string UnitPrice { get; set; }

        /// <summary>
        /// The total for the line item.
        /// </summary>
        public string Total { get; set; }

        /// <summary>
        /// Amount of discount applied to this line item.
        /// </summary>
        public string Discount { get; set; }

        /// <summary>
        /// The detail order record version number.
        /// </summary>
        public string Version { get; set; }
    }
}