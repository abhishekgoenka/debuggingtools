using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETWebApplication
{
    /// <summary>
    /// Central location from where Url's can be retrieved. 
    /// This avoids the situation where URLs need to be hardcoded within pages or in code-behinds.
    ///</summary>
    ///<remarks>
    /// Used throughout the application. Typically used with 'Response.Redirect' calls.
    /// Also used in the menuing system (which is located in the Master page code behind).
    /// </remarks>
    public static class UrlMaker
    {
        // Home page
        public static string ToDefault() { return Path("~/"); }

        // Login / logout
        public static string ToLogin() { return Path("~/login"); }
        public static string ToLogout() { return Path("~/logout"); }

        // Shopping 
        public static string ToShopping() { return Path("~/shop"); }
        public static string ToProducts() { return Path("~/shop/products"); }
        public static string ToProduct(int productId) { return Path("~/shop/products/{0}", productId); }
        public static string ToSearch() { return Path("~/shop/search"); }
        public static string ToCart() { return Path("~/shop/cart"); }
        public static string ToCheckout() { return Path("~/shop/checkout"); }

        // Administration
        public static string ToAdmin() { return Path("~/admin"); }
        public static string ToOrders() { return Path("~/admin/customers/orders"); }
        public static string ToDetails(int customerId, int orderId) { return Path("~/admin/customers/{0}/orders/{1}/details", customerId, orderId); }
        public static string ToCustomer(int customerId) { return Path("~/admin/customers/{0}", customerId); }
        public static string ToCustomers() { return Path("~/admin/customers"); }

        // Error page
        public static string ToError() { return Path("~/error"); }

        // private url builder helper
        private static string Path(string virtualPath)
        {
            return VirtualPathUtility.ToAbsolute(virtualPath);
        }

        // private url builder helper
        private static string Path(string virtualPath, params object[] args)
        {
            return Path(string.Format(virtualPath, args));
        }
    }
}