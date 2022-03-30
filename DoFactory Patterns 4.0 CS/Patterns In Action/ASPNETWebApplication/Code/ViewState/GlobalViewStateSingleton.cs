using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace ASPNETWebApplication.ViewState
{
    /// <summary>
    /// GlobalViewStateSingleton maintains a list of viewstates in a 
    /// globally accessible hashtable. This is a Singleton helper class to the 
    /// ViewStateProviderGlobal class.
    /// </summary>
    /// <remarks>
    /// Gof Design Pattern: Singleton.
    /// 
    /// The Singleton Design Pattern ensures that just one instance (the Singleton) 
    /// holds a reference to a list of all viewstate providers declared in the 
    /// configuration file (web.config).
    /// </remarks>
    public class GlobalViewStateSingleton
    {
        #region The Singleton definition

        // This is the single instance of this class
        private static readonly GlobalViewStateSingleton _instance = new GlobalViewStateSingleton();

        /// <summary>
        /// Private constructor for GlobalViewStateSingleton.
        /// Prevents others from instantiating additional instances.
        /// </summary>
        private GlobalViewStateSingleton()
        {
            ViewStates = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets the one instance of the GlobalViewStateSingleton class
        /// </summary>
        public static GlobalViewStateSingleton Instance
        {
            get { return _instance; }
        }

        #endregion

        /// <summary>
        /// Gets a list of ViewStates.
        /// </summary>
        public Dictionary<string, object> ViewStates { get; private set; }
    }
}
