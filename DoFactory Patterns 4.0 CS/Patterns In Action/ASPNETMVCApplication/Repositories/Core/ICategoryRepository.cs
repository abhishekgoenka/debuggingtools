using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASPNETMVCApplication.ActionServiceReference;

namespace ASPNETMVCApplication.Repositories
{
    /// <summary>
    /// Category Repository Interface.
    /// Derives from standard IRepository. No category specific members are added.  
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        // No additional members...
    }
}
