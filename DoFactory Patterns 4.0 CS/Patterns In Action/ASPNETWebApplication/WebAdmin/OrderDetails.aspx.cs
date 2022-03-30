using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

using ASPNETWebApplication.ActionServiceReference;
using ASPNETWebApplication.Repositories;

namespace ASPNETWebApplication.WebAdmin
{
    public partial class OrderDetails : PageBase
    {
        /// <summary>
        /// Override. Returns custom breadcrumb chain for this page. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public override SiteMapNode SiteMapResolve(object sender, SiteMapResolveEventArgs e)
        {
            var home = new SiteMapNode(e.Provider, "Home", "~/", "home");
            var admin = new SiteMapNode(e.Provider, "Admin", "~/admin", "administration");
            var orders = new SiteMapNode(e.Provider, "Orders", "~/admin/customers/orders", "orders");

            var customerId = Page.RouteData.Values["customerid"].ToString();
            var customer = new SiteMapNode(e.Provider, "Customer", "~/admin/customers/" + customerId + "/orders", "customer orders");
            var details = new SiteMapNode(e.Provider, "Details", null, "line items");

            admin.ParentNode = home;
            orders.ParentNode = admin;
            customer.ParentNode = orders;
            details.ParentNode = customer;

            return details;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Meta data: helpful for SEO (search engine optimization)
            Page.Title = "Order Details";
            Page.MetaKeywords = "Customers, Order Details, Patterns in Action";
            Page.MetaDescription = "Customer's order details at Patterns in Action"; 

            if (!IsPostBack)
            {
                // Set the selected menu item in Master page.
                SelectedMenu = "orders";

                // Save off OrderId for this page.
                OrderId = int.Parse(Page.RouteData.Values["orderid"].ToString());
                CustomerId = int.Parse(Page.RouteData.Values["customerid"].ToString());

                Bind();
            }
        }

        /// <summary>
        /// Sets datasources and bind data to controls.
        /// </summary>
        private void Bind()
        {
            var repository = new OrderRepository();
            ActionServiceReference.Order order = repository.GetOrder(OrderId);

            // Set the date
            LabelHeader.Text = "Order Line Items"; 
            LabelOrderDate.Text = "Order date: " + order.OrderDate.ToShortDateString();
            HyperLinkBack.Text = "< back to orders "; 

            GridViewOrderDetails.DataSource = order.OrderDetails;
            GridViewOrderDetails.DataBind();
        }

        /// <summary>
        /// Gets or sets orderId
        /// </summary>
        private int OrderId { get; set; }

        /// <summary>
        /// Gets or sets customerId
        /// </summary>
        private int CustomerId { get; set; }
    }
}
