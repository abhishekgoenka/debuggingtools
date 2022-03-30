using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETMVCApplication.Areas.Admin.Models
{
    /// <summary>
    /// Order Model class.
    /// </summary>
    public class OrderModel
    {
        /// <summary>
        /// Unique customer identifier.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// The order identifier.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// The order date.
        /// </summary>
        public string OrderDate { get; set; }

        /// <summary>
        /// The date the order is required.
        /// </summary>
        public string RequiredDate { get; set; }

        /// <summary>
        /// The order shipping method.
        /// </summary>
        public string Shipping { get; set; }

        /// <summary>
        /// The order record version number.
        /// </summary>
        public string Version { get; set; }
    }
}