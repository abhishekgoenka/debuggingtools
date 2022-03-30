using System.Web.Mvc;

namespace ASPNETMVCApplication.Areas.Shop
{
    /// <summary>
    /// Shopping area routing registration class.
    /// </summary>
    public class ShopAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// The area name: Shop.
        /// </summary>
        public override string AreaName
        {
            get { return "Shop"; }
        }

        /// <summary>
        /// Registers Shopping specific routes.
        /// Note: the order in which routes are registered is important!
        /// </summary>
        /// <param name="context">The registration context.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(null, "shop", new { controller = "Shop", action = "Index" });
            context.MapRoute(null, "shop/products", new { controller = "Shop", action = "Products" });
            context.MapRoute(null, "shop/products/{productid}", new { controller = "Shop", action = "Product" });
            context.MapRoute(null, "shop/search", new { controller = "Shop", action = "Search" });
            context.MapRoute(null, "shop/cart/checkout", new { controller = "Shop", action = "Checkout" });
            context.MapRoute(null, "shop/cart/recalculate", new { controller = "Shop", action = "Recalculate" });
            context.MapRoute(null, "shop/cart/shipping", new { controller = "Shop", action = "Shipping" });
            context.MapRoute(null, "shop/cart", new { controller = "Shop", action = "Cart" });

            context.MapRoute(
                "Shop_default",
                "Shop/{controller}/{action}/{id}",
                new { action = "Index", id = "" }
            );
        }
    }
}
