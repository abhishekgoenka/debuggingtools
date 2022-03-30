using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETMVCApplication.Areas.Shop
{
    /// <summary>
    /// Represents shipping option.
    /// </summary>
    public class ShippingMethodItem
    {
        /// <summary>
        /// Unique shipping identifier.
        /// </summary>
        public int ShippingId { get; set; }

        /// <summary>
        /// Name of shipping method.
        /// </summary>
        public string ShippingName { get; set; }
    }
}