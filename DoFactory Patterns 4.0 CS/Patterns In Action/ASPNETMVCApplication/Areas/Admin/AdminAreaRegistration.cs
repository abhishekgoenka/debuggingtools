using System.Web.Mvc;

namespace ASPNETMVCApplication.Areas.Admin
{
    /// <summary>
    /// Admin area routing registration class.
    /// </summary>
    public class AdminAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// The area name: Admin.
        /// </summary>
        public override string AreaName
        {
            get { return "Admin"; }
        }

        /// <summary>
        /// Registers Admin specific routes.
        /// Note: the order in which routes are registered is important!
        /// </summary>
        /// <param name="context">The registration context.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(null, "admin", new { controller = "Admin", action = "Index" });
            context.MapRoute(null, "admin/customers", new { controller = "Admin", action = "Customers" });
            context.MapRoute(null, "admin/customers/orders", new { controller = "Admin", action = "Orders" });
            context.MapRoute(null, "admin/customers/{customerid}", new { controller = "Admin", action = "Customer" });
            context.MapRoute(null, "admin/customers/{customerid}/orders", new { controller = "Admin", action = "CustomerOrders" });
            context.MapRoute(null, "admin/customers/{customerid}/orders/{orderid}", new { controller = "Admin", action = "Order" });
            context.MapRoute(null, "admin/customers/{customerid}/orders/{orderid}/details", new { controller = "Admin", action = "OrderDetails" });

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = "" }
            );
        }
    }
}
