using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WindowsFormsModel.BusinessObjects;

namespace WindowsFormsView
{
    /// <summary>
    /// Represents view of orders.
    /// </summary>
    public interface IOrdersView : IView
    {
        IList<OrderModel> Orders { set; }
    }
}
