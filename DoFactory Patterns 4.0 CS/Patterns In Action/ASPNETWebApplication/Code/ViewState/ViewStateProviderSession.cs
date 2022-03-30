using System.Web;
using System.Web.SessionState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASPNETWebApplication.ViewState
{
    /// <summary>
    /// Viewstate provider that is implemented using session objects.
    /// </summary>
    /// <remarks>
    /// Gof Design Pattern: Strategy.
    /// 
    /// The Strategy Design Pattern ensures that this class is 'pluggable' and 
    /// can fully function as a viewstate provider.
    /// </remarks>
    public class ViewStateProviderSession : ViewStateProviderBase
    {
        /// <summary>
        /// Saves view state information for the web page in a session object.
        /// </summary>
        /// <param name="name">Name of the viewstate.</param>
        /// <param name="viewState">Viewstate.</param>
        public override void SavePageState(string name, object viewState)
        {
            var session = HttpContext.Current.Session;
            session[name] = viewState;
        }

        /// <summary>
        /// Retrieves viewstate information for the web page from session.
        /// </summary>
        /// <param name="name">Name of the viewstate.</param>
        /// <returns>Viewstate.</returns>
        public override object LoadPageState(string name)
        {
            var session = HttpContext.Current.Session;
            return session[name];
        }
    }
}
