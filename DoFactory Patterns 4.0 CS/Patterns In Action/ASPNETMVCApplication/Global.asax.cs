using System;
using System.Web.Mvc;
using System.Web.Routing;
using ASPNETMVCApplication.Repositories;
using ASPNETMVCApplication.Repositories.Core;

namespace ASPNETMVCApplication
{
    /// <summary>
    /// MVC application class. This is where it all begins...
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Application start.
        /// </summary>
        protected void Application_Start()
        {
            // Register routes (in Areas and at the Root)
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
        }

        // Register couple of routes. Other routes are registered in their own Areas.
        private void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Home and error page
            routes.MapRoute("home", "", new { controller = "Home", action = "Index" });
            routes.MapRoute("error", "error", new { controller = "Home", action = "Error" });

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

        }

        /// <summary>
        /// Application error event. This is where last resort eror handling occurs. 
        /// This is a great place to log errors and then let the <customErrors > entry 
        /// in web.config determine which (error) page displays next.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError().GetBaseException();

            // Log error here...

            // <customErrors ..> in web config will now redirect.
        }
    }
}