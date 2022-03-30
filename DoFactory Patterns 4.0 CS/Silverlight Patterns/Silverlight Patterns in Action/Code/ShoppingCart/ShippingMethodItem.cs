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

namespace Silverlight_Patterns_in_Action.Code.ShoppingCart
{
    /// <summary>
    /// Represents a shipping method.
    /// </summary>
    public class ShippingMethodItem
    {
        /// <summary>
        /// Unique shipping identifier.
        /// </summary>
        public int ShippingId { get; set; }

        /// <summary>
        /// Shipping method name: eg. UPS, Fedex, etc.
        /// </summary>
        public string ShippingName { get; set; }
    }
}
