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

using ASPNETWebApplication.Repositories;
using ASPNETWebApplication.ActionServiceReference;

namespace ASPNETWebApplication.WebAdmin
{
    public partial class Customer : PageBase
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
            var customers = new SiteMapNode(e.Provider, "Customers", "~/admin/customers", "customers");
            var customer = new SiteMapNode(e.Provider, "Customer", null, "customer details");

            admin.ParentNode = home;
            customers.ParentNode = admin;
            customer.ParentNode = customers;

            return customer;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Meta data: helpful for SEO (search engine optimization)
            Page.Title = "Customer Details";
            Page.MetaKeywords = "Customer Details, Patterns in Action";
            Page.MetaDescription = "Customer Details at Patterns in Action"; 

            if (!IsPostBack)
            {
                // Set the selected menu item in the Master page.
                SelectedMenu = "customers";

                CustomerId = int.Parse(Page.RouteData.Values["customerid"].ToString());

                // Set DetailsView control in Add or Edit mode.
                if (CustomerId == 0)
                    DetailsViewCustomer.ChangeMode(DetailsViewMode.Insert);
                else
                    DetailsViewCustomer.ChangeMode(DetailsViewMode.Edit);

                // Set image
                ImageCustomer.ImageUrl = imageService + "GetCustomerImageLarge/" + CustomerId;
            }
        }

        /// <summary>
        /// Gets or sets the customerId for this page.
        /// </summary>
        private int CustomerId
        {
            get { return int.Parse(Session["CustomerId"].ToString()); }
            set { Session["CustomerId"] = value; }
        }

        /// <summary>
        /// Saves data for new or edited customer to database.
        /// </summary>
        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            var repository = new CustomerRepository();

            ActionServiceReference.Customer customer;
            if (CustomerId == 0)
                customer = new ActionServiceReference.Customer();
            else
                customer = repository.GetCustomer(CustomerId);

            // Get Company name from page.
            var row = DetailsViewCustomer.Rows[1];
            var textBox = row.Cells[1].Controls[0] as TextBox;
            customer.Company = textBox.Text.Trim();

            // Get City from page.
            row = DetailsViewCustomer.Rows[2];
            textBox = row.Cells[1].Controls[0] as TextBox;
            customer.City = textBox.Text.Trim();

            // Get Country from page.
            row = DetailsViewCustomer.Rows[3];
            textBox = row.Cells[1].Controls[0] as TextBox;
            customer.Country = textBox.Text.Trim();

            try
            {
                if (CustomerId == 0)
                    repository.AddCustomer(customer);
                else
                    repository.UpdateCustomer(customer);
            }
            catch (ApplicationException ex)
            {
                LabelError.Text = ex.Message.Replace(Environment.NewLine, "<br />");
                PanelError.Visible = true;
                return;
            }

            // Return to list of customers.
            Response.Redirect(UrlMaker.ToCustomers()); 

        }

        /// <summary>
        /// Cancel the page and redirect user to page with list of customers.
        /// </summary>
        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlMaker.ToCustomers()); 
        }

        /// <summary>
        /// Executed only once. Used to place cursor in first editable field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DetailsView_OnDataBound(object sender, EventArgs e)
        {
            if (DetailsViewCustomer.Rows.Count < 1) return;

            var row = DetailsViewCustomer.Rows[1];
            var textBox = row.Cells[1].Controls[0] as TextBox;
            textBox.Focus();
        }
    }
}
