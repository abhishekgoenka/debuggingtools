using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETWebApplication
{
    /// <summary>
    /// Static helper class. Returns Urls. Used throughout application.
    /// </summary>
    /// <remarks>
    /// This class prevents you from having to use hard-coded strings (which are error prone).
    /// </remarks>
    public static class UrlMaker
    {
        // Home page
        public static string ToDefault() { return Path("~/"); }

        // Login / logut pages
        public static string ToLogin() { return Path("~/login"); }
        public static string ToLogout() { return Path("~/logout"); }

        // Shopping pages
        public static string ToShopping() { return Path("~/shop"); }
        public static string ToProducts() { return Path("~/shop/products"); }
        public static string ToProduct(int productId) { return Path("~/shop/products/{0}", productId); }
        public static string ToSearch() { return Path("~/shop/search"); }
        public static string ToCart() { return Path("~/shop/cart"); }
        public static string ToCheckout() { return Path("~/shop/checkout"); }

        // Admin pages
        public static string ToAdmin() { return Path("~/admin"); }
        public static string ToOrders() { return Path("~/admin/customers/orders"); }
        public static string ToCustomerOrders(int customerId) { return Path("~/admin/customers/{0}/orders", customerId); }
        public static string ToCustomer(int customerId) { return Path("~/admin/customers/{0}", customerId); }
        public static string ToCustomers() { return Path("~/admin/customers"); }

        // Error page
        public static string ToError() { return Path("~/error"); }

        // Private url builder helper method
        private static string Path(string virtualPath)
        {
            // When running unit tests, the HttpContext is not available
            if (HttpContext.Current == null)
                return "";
            
            return VirtualPathUtility.ToAbsolute(virtualPath);
        }

        // Private url builder helper method
        private static string Path(string virtualPath, params object[] args)
        {
            return Path(string.Format(virtualPath, args));
        }
    }
}