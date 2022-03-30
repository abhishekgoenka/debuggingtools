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

namespace Silverlight_Patterns_in_Action.Code
{
    /// <summary>
    /// Shipping method enumeration.
    /// </summary>
    public enum ShippingMethod
    {
        /// <summary>
        /// Shipping method not determined.
        /// </summary>
        None,

        /// <summary>
        /// Shipping via Federal Express.
        /// </summary>
        Fedex,

        /// <summary>
        /// Shipping via United Postal Services.
        /// </summary>
        UPS,

        /// <summary>
        /// Shipping via United States Postal Services.
        /// </summary>
        USPS
    }
}
