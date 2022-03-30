using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASPNETMVCApplication.ActionServiceReference;

namespace ASPNETMVCApplication.Repositories
{
    /// <summary>
    /// Order Repository Interface.
    /// Derives from standard IRepository. No order specific members are added.  
    /// </summary>
    public interface IOrderRepository : IRepository<Order>
    {
        // No additiional members...
    }
}
