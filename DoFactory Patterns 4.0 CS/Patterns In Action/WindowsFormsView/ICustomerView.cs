using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsView
{
    /// <summary>
    /// Represents a single customer view
    /// </summary>
    public interface ICustomerView : IView
    {
        int CustomerId { get; set; }
        string Company { get; set; }
        string City { get; set; }
        string Country { get; set; }
        string Version { get; set; }
    }
}
