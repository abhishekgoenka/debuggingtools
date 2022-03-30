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
    public partial class Order : PageBase
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
            var customer = new SiteMapNode(e.Provider, "Orders", null, "customer orders");

            admin.ParentNode = home;
            orders.ParentNode = admin;
            customer.ParentNode = orders;

            return customer;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Meta data: helpful for SEO (search engine optimization)
            Page.Title = "Customer Orders";
            Page.MetaKeywords = "Customer Orders, Electronic Products";
            Page.MetaDescription = "Customer Orders at Patterns in Action"; 

            if (!IsPostBack)
            {
                // Set the selected menu item in the Master page.
                SelectedMenu = "orders";

                // Save off customerId 
                CustomerId = int.Parse(Page.RouteData.Values["customerid"].ToString());

                Bind();
            }
        }

        /// <summary>
        /// Sets datasources and bind data to controls.
        /// </summary>
        private void Bind()
        {
            // Get customer via Customer Repository.
            var repository = new CustomerRepository();
            ActionServiceReference.Customer customer = repository.GetCustomerWithOrders(CustomerId);

            // Set company name
            LabelHeader.Text = "<font color='black'>Orders for:</font> "
                + customer.Company + " (" + customer.Country + ")";

            GridViewOrders.DataSource = customer.Orders;
            GridViewOrders.DataBind();
        }

        /// <summary>
        /// Gets or sets customerId for the page in Session.
        /// </summary>
        private int CustomerId
        {
            get { return int.Parse(Session["customerId"].ToString()); }
            set { Session["customerId"] = value; }
        }

        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var link = e.Row.Cells[4].Controls[0] as HyperLink;
                link.NavigateUrl = link.NavigateUrl.Replace("customerid", CustomerId.ToString());
            }
        }
    }
}
