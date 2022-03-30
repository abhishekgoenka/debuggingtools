using System.ComponentModel;

using ASPNETWebApplication.ActionServiceReference;

namespace ASPNETWebApplication.Repositories
{
    /// <summary>
    /// Repository for customer orders.
    /// </summary>
    /// <remarks>
    /// Repository Pattern.
    /// </remarks>
    public class OrderRepository : RepositoryBase
    {
        /// <summary>
        /// Gets a specific order.
        /// </summary>
        /// <param name="orderId">Unique order identifier.</param>
        /// <returns>The requested Order.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public Order GetOrder(int orderId)
        {
            var request = new OrderRequest().Prepare();
            request.LoadOptions = new string[] { "Order", "Customer", "OrderDetails" };
            request.Criteria = new OrderCriteria { OrderId = orderId };

            var response = Client.GetOrders(request);

            Correlate(request, response);

            return response.Order;
        }
    }
}
