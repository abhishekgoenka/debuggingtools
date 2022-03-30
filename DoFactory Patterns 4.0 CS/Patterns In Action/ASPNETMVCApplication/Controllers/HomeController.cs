using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPNETMVCApplication.Code.Filters;

namespace ASPNETMVCApplication.Controllers
{
    /// <summary>
    /// Controller for home page and error page.
    /// </summary>
    [HandleError]
    public class HomeController : BaseController
    {
        /// <summary>
        /// Action method for home page (index).
        /// </summary>
        /// <returns></returns>
        [Menu(MenuItem.Home)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Action method for when error occurs. Renders shared error page.
        /// </summary>
        /// <returns></returns>
        [Menu(MenuItem.Home)]
        public ActionResult Error()
        {
            return View();
        }
    }
}
