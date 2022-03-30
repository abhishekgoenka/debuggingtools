using System;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Web.Routing;
using Log;

namespace ASPNETWebApplication
{
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// Event handler for application start event. 
        /// Initializes routing, sitemap, and logging.
        /// </summary>
        protected void Application_Start(Object sender, EventArgs e)
        {
            // Initialize routing system
            RegisterRoutes(RouteTable.Routes);

            // Initialize sitemap facility
            InitializeSiteMapResolver();

            // Initialize logging facility
            InitializeLogger();
        }

        
        // Register routes with routing system.
        private static void RegisterRoutes(RouteCollection routes)
        {
            // Note: the order in which routes are registered is important.
            routes.Ignore("{resource}.axd/{*pathInfo}");

            routes.MapPageRoute("Error", "error", "~/Error.aspx");

            routes.MapPageRoute("Login", "login", "~/WebAuth/Login.aspx");
            routes.MapPageRoute("Logout", "logout", "~/WebAuth/Logout.aspx");

            routes.MapPageRoute("Shop", "shop", "~/WebShop/Shopping.aspx");
            routes.MapPageRoute("Products", "shop/products", "~/WebShop/Products.aspx");
            routes.MapPageRoute("Product", "shop/products/{productid}", "~/WebShop/Product.aspx");
            routes.MapPageRoute("Search", "shop/search", "~/WebShop/Search.aspx");
            routes.MapPageRoute("Cart", "shop/cart", "~/WebShop/Cart.aspx");
            routes.MapPageRoute("Checkout", "shop/checkout", "~/WebShop/Checkout.aspx");

            // Note: Order of these entries below is important
            routes.MapPageRoute("Admin", "admin", "~/WebAdmin/Admin.aspx");
            routes.MapPageRoute("Orders", "admin/customers/orders", "~/WebAdmin/Orders.aspx");
            routes.MapPageRoute("Order", "admin/customers/{customerid}/orders", "~/WebAdmin/Order.aspx");
            routes.MapPageRoute("Details", "admin/customers/{customerid}/orders/{orderid}/details", "~/WebAdmin/OrderDetails.aspx");
            routes.MapPageRoute("Customer", "admin/customers/{customerid}", "~/WebAdmin/Customer.aspx");
            routes.MapPageRoute("Customers", "admin/customers", "~/WebAdmin/Customers.aspx");

            routes.MapPageRoute("Default", "", "~/Default.aspx");
        }

        // Initialize sitemap event facility. 
        private void InitializeSiteMapResolver()
        {
            SiteMap.SiteMapResolve += SiteMapResolveHandler;
        }

        // The Sitemap resolve event is handed over to the current page being processed. 
        private SiteMapNode SiteMapResolveHandler(object sender, SiteMapResolveEventArgs e)
        {
            var pageBase = e.Context.CurrentHandler as PageBase;
            if (pageBase != null)
                return pageBase.SiteMapResolve(sender, e);
            else
                return null;
        }

        /// <summary>
        /// Initializes logging facility with severity level and observer(s).
        /// Private helper method.
        /// </summary>
        private void InitializeLogger()
        {
            // Read and assign application wide logging severity
            string severity = ConfigurationManager.AppSettings.Get("LogSeverity");
            SingletonLogger.Instance.Severity = (LogSeverity)Enum.Parse(typeof(LogSeverity), severity, true);

            // Send log messages to debugger console (output window). 
            // Btw: the attach operation is the Observer pattern.
            ILog log = new ObserverLogToConsole();
            SingletonLogger.Instance.Attach(log);

            // Send log messages to email (observer pattern)
            string from = "notification@yourcompany.com";
            string to = "webmaster@yourcompany.com";
            string subject = "Webmaster: please review";
            string body = "email text";
            var smtpClient = new SmtpClient("mail.yourcompany.com");
            log = new ObserverLogToEmail(from, to, subject, body, smtpClient);
            SingletonLogger.Instance.Attach(log);

            // Other log output options

            //// Send log messages to a file
            //log = new ObserverLogToFile(@"C:\Temp\DoFactory.log");
            //SingletonLogger.Instance.Attach(log);

            //// Send log message to event log
            //log = new ObserverLogToEventlog();
            //SingletonLogger.Instance.Attach(log);

            //// Send log messages to database (observer pattern)
            //log = new ObserverLogToDatabase();
            //SingletonLogger.Instance.Attach(log);
        }

        /// <summary>
        /// This is the last-resort exception handler.
        /// It uses te logging infrastructure to log the error details.
        /// The application will then be redirected according to the 
        /// customErrors configuration in web.config.
        /// </summary>
        /// <remarks>
        /// Logging is commented out. Be sure the application is authorized to write
        /// to a file, send email, or add to the event log. Only then turn logging on.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError().GetBaseException();

            // NOTE: commented out because the site needs authorization to logging resources.
            // SingletonLogger.Instance.Error(ex.Message);

            // <customErrors ..> in web config will now redirect.
        }
    }
}