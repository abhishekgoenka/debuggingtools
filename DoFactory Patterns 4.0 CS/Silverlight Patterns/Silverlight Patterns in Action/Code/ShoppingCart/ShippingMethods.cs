using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Silverlight_Patterns_in_Action.Code.ShoppingCart
{
    /// <summary>
    /// Represents teh shipping methods offered in the application.
    /// </summary>
    public class ShippingMethods
     {
        /// <summary>
        /// List of shipping methods.
        /// </summary>
        public List<ShippingMethodItem> List { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ShippingMethods()
        {
            List = new List<ShippingMethodItem>();
            List.Add(new ShippingMethodItem { ShippingId = 1, ShippingName = "Fedex" });
            List.Add(new ShippingMethodItem { ShippingId = 2, ShippingName = "UPS" });
            List.Add(new ShippingMethodItem { ShippingId = 3, ShippingName = "USPS" });
        }

        
    }
}
