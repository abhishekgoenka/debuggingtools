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
using System.Globalization;

namespace Silverlight_Contracts
{
    /// <summary>
    /// Order statistics data package.
    /// </summary>
    public class OrderStatistics
    {
        public int Month { get; set; }
        public double Freight { get; set; }
        public double OrderCount { get; set; }

        // Helper. Returns month name from month number.
        public string MonthName
        {
            get { return CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.GetValue(Month - 1).ToString().Substring(0, 3); }
        }

    }
}
