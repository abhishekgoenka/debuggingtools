using System.Web.Mvc;

namespace ASPNETMVCApplication.Areas.Auth
{
    /// <summary>
    /// Authentication area routing registration class.
    /// </summary>
    public class AuthAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// The area name: Auth.
        /// </summary>
        public override string AreaName
        {
            get { return "Auth"; }
        }
        /// <summary>
        /// Registers Authentication specific routes.
        /// </summary>
        /// <param name="context">The registration context.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(null, "login", new { controller = "Auth", action = "Login" });
            context.MapRoute(null, "logout", new { controller = "Auth", action = "Logout" });

            context.MapRoute(
                "Auth_default",
                "Auth/{controller}/{action}/{id}",
                new { action = "Index", id = "" }
            );
        }
    }
}
