using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASPNETMVCApplication.ActionServiceReference;

namespace ASPNETMVCApplication.Repositories
{
    /// <summary>
    /// Customer Repository interface.
    /// Derives from standard IRepository. Adds two customer specific members..
    /// </summary>
    public interface ICustomerRepository : IRepository<Customer>
    {
        List<Customer> GetCustomerListWithOrderStatistics(Criterion criterion);
        Customer GetCustomerWithOrders(Criterion criterion);
    }
}
