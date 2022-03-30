using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace ASPNETMVCApplication.Controllers
{
    /// <summary>
    /// Base class of all controllers.
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Reference to image service.
        /// </summary>
        protected static readonly string imageService = ConfigurationManager.AppSettings.Get("ImageService");

        /// <summary>
        /// View Data entry named Results sent to View.  
        /// Used by numerous pages.
        /// </summary>
        public string Result { set { ViewData["Result"] = value; } }
    }
}
