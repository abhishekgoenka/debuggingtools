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
    public partial class Customers : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Meta data: helpful for SEO (search engine optimization)
            Page.Title = "Customer List";
            Page.MetaKeywords = "Customer List, Patterns in Action";
            Page.MetaDescription = "Customer List at Patterns in Action"; 

            if (!IsPostBack)
            {
                // Set the selected menu item in the Master page.
                SelectedMenu = "customers";

                // Set default sort settings.
                SortColumn = "CustomerId";
                SortDirection = "ASC";

                Bind();
            }
        }

        /// <summary>
        /// Sets datasources and bind data. 
        /// </summary>
        private void Bind()
        {
            var repository = new CustomerRepository();
            GridViewCustomers.DataSource = repository.GetCustomers(SortExpression);
            GridViewCustomers.DataBind();
        }

        #region Sorting

        /// <summary>
        /// Sets sort order and re-binds page.
        /// </summary>
        protected void GridViewCustomers_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection = (SortDirection == "ASC") ? "DESC" : "ASC";
            SortColumn = e.SortExpression;

            Bind();
        }

        /// <summary>
        /// Adds glyphs to gridview header according to sort order.
        /// </summary>
        protected void GridViewCustomers_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                AddGlyph(this.GridViewCustomers, e.Row);
            }
        }

        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton linkButton = e.Row.Cells[5].Controls[0] as LinkButton;
                // Escape single quotes in Javascript. 
                string company = DataBinder.Eval(e.Row.DataItem, "Company").ToString().Replace("'", "\\'");
                linkButton.Attributes.Add("onclick", "javascript:return " +
                "confirm('OK to delete \"" + company + "\"?')");
            }
        }

        #endregion

        /// <summary>
        /// Deletes selected customer from database.
        /// </summary>
        protected void GridViewCustomers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var row = GridViewCustomers.Rows[e.RowIndex];
            int customerId = int.Parse(row.Cells[0].Text);

            var repository = new CustomerRepository();

            var customer = repository.GetCustomerWithOrders(customerId);
            if (customer.Orders.Length > 0)
            {
                string customerName = row.Cells[1].Text;
                LabelError.Text = "Cannot delete " + customerName + " because they have existing orders!";
            }
            else
            {
                repository.DeleteCustomer(customerId);
                Bind();
            }
        }
    }
}
