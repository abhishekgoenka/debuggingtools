using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETMVCApplication.Areas.Shop
{
    /// <summary>
    /// Static class holding predefined price ranges.
    /// </summary>
    public static class PriceRange
    {
        /// <summary>
        /// List of price ranges.
        /// </summary>
        public static List<PriceRangeItem> List { get; private set; }

        /// <summary>
        /// Static constructor initializeing price range list.
        /// </summary>
        static PriceRange()
        {
            List = new List<PriceRangeItem>();

            List.Add(new PriceRangeItem(0, 0, 0, "select"));
            List.Add(new PriceRangeItem(1, 0, 50, "$0 - $50"));
            List.Add(new PriceRangeItem(2, 51, 100, "$51 - $100"));
            List.Add(new PriceRangeItem(3, 101, 250, "$101 - $250"));
            List.Add(new PriceRangeItem(4, 251, 1000, "$251 - $1,000"));
            List.Add(new PriceRangeItem(5, 1001, 2000, "$1,001 - $2,000"));
            List.Add(new PriceRangeItem(6, 2001, 10000, "$2,001 - $10,000"));
        }
    }
}