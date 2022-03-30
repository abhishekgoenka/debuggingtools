using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPNETMVCApplication.ActionServiceReference;
using ASPNETMVCApplication.Repositories.Core;

namespace ASPNETMVCApplication.Repositories
{
    /// <summary>
    /// Order Repository.
    /// </summary>
    public class OrderRepository : RepositoryBase, IOrderRepository
    {
        /// <summary>
        /// Returns an order by orderId.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Order Get(int orderId)
        {
            var request = new OrderRequest().Prepare();

            request.LoadOptions = new string[] { "Order", "Customer", "OrderDetails" };
            request.Criteria = new OrderCriteria { OrderId = orderId };

            var response = Client.GetOrders(request);

            Correlate(request, response);

            if (request.RequestId != response.CorrelationId)
                throw new ApplicationException("GetOrder: RequestId and CorrelationId do not match.");

            return response.Order;
        }

        #region Not implemented members

        public List<ActionServiceReference.Order> GetList(Criterion criterion = null)
        {
            throw new NotImplementedException();
        }

        public ActionServiceReference.Order Get(Criterion criterion = null)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Criterion criterion = null)
        {
            throw new NotImplementedException();
        }

        public void Insert(ActionServiceReference.Order t)
        {
            throw new NotImplementedException();
        }

        public void Update(ActionServiceReference.Order t)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}