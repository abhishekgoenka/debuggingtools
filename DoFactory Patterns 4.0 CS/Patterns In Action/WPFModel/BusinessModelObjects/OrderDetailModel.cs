using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFModel.BusinessModelObjects
{
    /// <summary>
    /// Model of order details. 
    /// Not inherited from BaseModel because no orders are placed in this app.
    /// </summary>
    public class OrderDetailModel
    {
        /// <summary>
        /// Gets or sets product name.
        /// Not inherited from BaseModel because no changes are made.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets quantity of products ordered.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or set unit price of product.
        /// </summary>
        public float UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets discount applied to unit price in this order.
        /// </summary>
        public float Discount { get; set; }

        /// <summary>
        /// Gets or sets order of which this order detail is a part.
        /// </summary>
        public OrderModel Order { get; set; }

        /// <summary>
        /// Gets or sets record version.
        /// </summary>
        public string Version { get; set; }
    }
}
