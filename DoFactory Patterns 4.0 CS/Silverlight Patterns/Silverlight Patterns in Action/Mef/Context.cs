using System;
using System.ComponentModel.Composition;
using Silverlight_Patterns_in_Action.Web.Services.Web;
using Silverlight_Contracts;
using System.Collections.Generic;
using System.ServiceModel.DomainServices.Client;
using Silverlight_Patterns_in_Action.Web;
using System.Linq;

namespace Silverlight_Patterns_in_Action
{
    /// <summary>
    /// Implementation of the MEF IContext contract.
    /// </summary>
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(IContext))]
    public class Context : IContext
    {
        private Action<List<OrderStatistics>> _callback;
        private EntitySet<Order> _orders;

        /// <summary>
        /// Gets order statistics. Asynch call.
        /// </summary>
        /// <param name="callback">Callback function.</param>
        /// <param name="year"></param>
        public void GetOrderStatistics(Action<List<OrderStatistics>> callback, int year)
        {
            var context = new ActionDomainContext();
            _orders = context.Orders;
            _callback = callback;

            context.Load(context.GetOrdersByYearQuery(year), GetOrdersByYearCallback, true);
        }

        /// <summary>
        /// Callback. Gets raw statistics data (by year) and groups it by month. 
        /// </summary>
        /// <param name="loadOperation"></param>
        public void GetOrdersByYearCallback(LoadOperation loadOperation)
        {
            if (loadOperation.HasError)
            {
                throw loadOperation.Error;
            }

            var orderStatistics = 
                from o in _orders.ToList()
                group o by o.OrderDate.Month into g
                select new OrderStatistics{ Month = g.Key, OrderCount = g.Count(),  Freight = g.Sum( o => (double)o.Freight) };

            _callback(orderStatistics.ToList());
        }
    }
}
