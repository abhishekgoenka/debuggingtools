using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WindowsFormsModel;
using WindowsFormsView;

namespace WindowsFormsPresenter
{
    /// <summary>
    /// Orders Presenter class.
    /// </summary>
    /// <remarks>
    /// MV Patterns: MVP design pattern.
    /// </remarks>
    public class OrdersPresenter : Presenter<IOrdersView>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view">The view</param>
        public OrdersPresenter(IOrdersView view)
            : base(view)
        {
        }

        /// <summary>
        /// Displays list of orders.
        /// </summary>
        /// <param name="customerId">Customer id to display.</param>
        public void Display(int customerId)
        {
            View.Orders = Model.GetOrders(customerId); 
        }
    }
}
