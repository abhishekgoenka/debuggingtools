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

namespace Silverlight_Patterns_in_Action.Code.Pricing
{
    /// <summary>
    /// Holds a static list of price ranges.
    /// </summary>
    public static class PriceRange
    {
        public static List<PriceRangeItem> List { get; private set; }

        /// <summary>
        /// Static constructor for PriceRange.
        /// </summary>
        static PriceRange()
        {
            List = new List<PriceRangeItem>();

            List.Add(new PriceRangeItem(0, 0, 0, "price range"));
            List.Add(new PriceRangeItem(1, 0, 50, "$0 - $50"));
            List.Add(new PriceRangeItem(2, 51, 100, "$51 - $100"));
            List.Add(new PriceRangeItem(3, 101, 250, "$101 - $250"));
            List.Add(new PriceRangeItem(4, 251, 1000, "$251 - $1,000"));
            List.Add(new PriceRangeItem(5, 1001, 2000, "$1,001 - $2,000"));
            List.Add(new PriceRangeItem(6, 2001, 10000, "$2,001 - $10,000"));
        }
    }
}
