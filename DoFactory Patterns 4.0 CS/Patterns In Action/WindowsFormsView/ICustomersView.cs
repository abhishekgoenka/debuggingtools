using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WindowsFormsModel.BusinessObjects;

namespace WindowsFormsView
{
    /// <summary>
    /// Respresents view of a list of customers
    /// </summary>
    public interface ICustomersView : IView
    {
        IList<CustomerModel> Customers { set; }
    }
}
