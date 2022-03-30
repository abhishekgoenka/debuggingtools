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

namespace Silverlight_Contracts
{
    /// <summary>
    /// The data access interface to be implemented and exported by the 'main Silverlight application'.
    /// The Charts assembly consumes this service.
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// Retrieves order statistics asynchronously.
        /// </summary>
        /// <param name="callback">Callback for when retrieval is complete.</param>
        /// <param name="year">Year for which statistics is required.</param>
        void GetOrderStatistics(Action<List<OrderStatistics>> callback, int year);
    }
}
