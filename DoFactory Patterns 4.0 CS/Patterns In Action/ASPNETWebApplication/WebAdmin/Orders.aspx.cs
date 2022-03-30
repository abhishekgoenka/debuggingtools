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
    public partial class Orders : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Meta data: helpful for SEO (search engine optimization)
            Page.Title = "Customers and Orders";
            Page.MetaKeywords = "Customers, Order Count, Patterns In Action";
            Page.MetaDescription = "Customes and their Order Count at Patterns in Action"; 

            if (!IsPostBack)
            {
                // Set the selected menu item in Master page.
                SelectedMenu = "orders";

                // Set the default sort settings
                SortColumn = "CustomerId";
                SortDirection = "ASC";

                Bind();
            }
        }

        /// <summary>
        /// Sets datasources and bind data to controls.
        /// </summary>
        private void Bind()
        {
            // Retrieve orders via Customer Facade. 
            var repository = new CustomerRepository();
            GridViewOrders.DataSource = repository.GetCustomersWithOrderStatistics(SortExpression);
            GridViewOrders.DataBind();
        }

        #region Sorting

        /// <summary>
        /// Sets sort order and re-binds page.
        /// </summary>
        protected void GridViewOrders_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection = (SortDirection == "ASC") ? "DESC" : "ASC";
            SortColumn = e.SortExpression;

            Bind();
        }

        /// <summary>
        /// Adds glyph to header according to current sort settings.
        /// </summary>
        protected void GridViewOrders_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                AddGlyph(this.GridViewOrders, e.Row);
            }
        }

        #endregion
    }
}
