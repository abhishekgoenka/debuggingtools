using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ASPNETWebApplication.ViewState
{
    /// <summary>
    /// Viewstate provider that is implemented using a global singleton hashtable.
    /// </summary>
    /// <remarks>
    /// Gof Design Pattern: Strategy.
    ///
    /// The Strategy Design Pattern ensures that this class is 'pluggable' and 
    /// can fully function as a viewstate provider.
    /// </remarks>
    public class ViewStateProviderGlobal : ViewStateProviderBase
    {
        /// <summary>
        /// Saves view state information for the web page in a global variable.
        /// </summary>
        /// <param name="name">Name of the viewstate.</param>
        /// <param name="viewState">Viewstate.</param>
        public override void SavePageState(string name, object viewState)
        {
            GlobalViewStateSingleton.Instance.ViewStates.Add(name,viewState);
        }

        /// <summary>
        /// Retrieves viewstate information for the web page from global variable.
        /// </summary>
        /// <param name="name">Name of the viewstate.</param>
        /// <returns>Viewstate.</returns>
        public override object LoadPageState(string name)
        {
            return GlobalViewStateSingleton.Instance.ViewStates[name];
        }
    }
}
