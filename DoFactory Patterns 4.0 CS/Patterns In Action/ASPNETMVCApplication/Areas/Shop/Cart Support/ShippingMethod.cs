using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETMVCApplication.Areas.Shop
{
    /// <summary>
    /// Static class holding shipping methods.
    /// </summary>
    public static class ShippingMethod
    {
        /// <summary>
        /// List of shipping methods.
        /// </summary>
        public static List<ShippingMethodItem> List { get; set; }

        /// <summary>
        /// Static constructor initializing shipping methods list.
        /// </summary>
        static ShippingMethod()
        {
            List = new List<ShippingMethodItem>();
            List.Add(new ShippingMethodItem { ShippingId = 1, ShippingName = "Fedex" });
            List.Add(new ShippingMethodItem { ShippingId = 2, ShippingName = "UPS" });
            List.Add(new ShippingMethodItem { ShippingId = 3, ShippingName = "USPS" });
        }
    }
}