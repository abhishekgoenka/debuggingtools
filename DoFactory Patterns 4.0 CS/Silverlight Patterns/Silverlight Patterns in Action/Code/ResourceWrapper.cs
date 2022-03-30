using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Browser;

namespace Silverlight_Patterns_in_Action
{
    /// <summary>
    /// Wraps the application string resources that are compiled into the assembly.
    /// </summary>
    public sealed class ResourceWrapper
    {
        private static ApplicationStrings applicationStrings = new ApplicationStrings();

        /// <summary>
        /// Gets the Application strings.
        /// </summary>
        public ApplicationStrings ApplicationStrings
        {
            get { return applicationStrings; }
        }
    }
}