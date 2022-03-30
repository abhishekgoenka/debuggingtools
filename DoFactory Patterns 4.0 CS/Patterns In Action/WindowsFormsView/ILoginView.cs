using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsView
{
    /// <summary>
    /// Respresents login view with credentials.
    /// </summary>
    public interface ILoginView : IView
    {
        string UserName { get; }
        string Password { get; }
    }
}
