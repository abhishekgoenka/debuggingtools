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
    /// Represents a collection of viewstate providers.
    /// </summary>
    /// <remarks>
    /// Design Pattern: Microsoft's Provider Design Pattern. This patterns is
    /// really is a blending of 3 GoF Design Patterns: Strategy, Factory, Singleton.
    /// 
    /// Strategy Pattern because the client has the option which viewstate provider Strategy to choose.
    /// Singleton Pattern because GlobalViewStateSingleton is a Singleton that holds a list of all available viewstate providers.
    /// Factory Pattern because viewstate providers are manufactured by a Factory class according to settings in a configuration file (web config).
    /// </remarks>
    public class ViewStateProviderCollection : ProviderCollection
    {
        /// <summary>
        /// Gets a viewState provider from a list given its name.
        /// </summary>
        /// <param name="name">Provider name.</param>
        /// <returns>Viewstate provider.</returns>
        public new ViewStateProviderBase this[string name]
        {
            get { return base[name] as ViewStateProviderBase; }
        }

        /// <summary>
        /// Adds a viewstate provider to a collection of providers.
        /// </summary>
        /// <param name="provider">Viewstate provider.</param>
        public override void Add(ProviderBase provider)
        {
            if (provider != null && provider is ViewStateProviderBase)
            {
                base.Add(provider);
            }
        }
    }
}
