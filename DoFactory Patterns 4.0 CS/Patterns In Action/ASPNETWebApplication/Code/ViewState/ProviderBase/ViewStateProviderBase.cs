using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;

namespace ASPNETWebApplication.ViewState
{
    /// <summary>
    /// Defines the methods that viewstate providers should implement.
    /// </summary>
    /// <remarks>
    /// Design Pattern: Microsoft's Provider Design Pattern. This pattern is
    /// really is a blending of 3 GoF Design Patterns: Strategy, Factory, Singleton.
    /// 
    /// Strategy Pattern because the client has the option which viewstate provider Strategy to choose.
    /// Singleton Pattern because GlobalViewStateSingleton is a Singleton that holds a list of all available viewstate providers.
    /// Factory Pattern because viewstate providers are manufactured according to settings in a configuration file (web config).
    /// </remarks>
    public abstract class ViewStateProviderBase : ProviderBase
    {
        /// <summary>
        /// Saves any view or control state information for the web page.
        /// </summary>
        /// <param name="name">Name of the viewstate.</param>
        /// <param name="viewState">Viewstate.</param>
        public abstract void SavePageState(string name, object viewState);

        /// <summary>
        /// Loads any view or control state information for the web page.
        /// </summary>
        /// <param name="name">Name of the viewstate.</param>
        /// <returns>Viewstate.</returns>
        public abstract object LoadPageState(string name);
    }
}
