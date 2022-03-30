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
    /// Represents the custom viewstate provider section in web.config.
    /// </summary>
    /// <remarks>
    /// GoF Design Patterns: Factory.
    /// 
    /// The Factory Design Pattern implementation is hidden from us, but is implemented
    /// by the ConfigurationSection base class. This class reads the configuration 
    /// file (web.config) and manufactures the viewstate providers accordingly.
    /// </remarks>
    public class ViewStateProviderServiceSection : ConfigurationSection
    {
        /// <summary>
        /// Gets a collection of viewstate providers from web.config.
        /// </summary>
        [ConfigurationProperty("providers")]
        public ProviderSettingsCollection Providers
        {
            get { return (ProviderSettingsCollection)base["providers"]; }
        }

        /// <summary>
        /// Gets or sets the default viewstate provider.
        /// </summary>
        [StringValidator(MinLength = 1)]
        [ConfigurationProperty("defaultProvider", DefaultValue = "ViewStateProviderCache")]
        public string DefaultProvider
        {
            get { return (string)base["defaultProvider"]; }
            set { base["defaultProvider"] = value; }
        }
    }
}
