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

namespace ASPNETWebApplication.WebAdmin
{
    public partial class Admin : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Meta data: helpful for SEO (search engine optimization)
            Page.Title = "Administration";
            Page.MetaKeywords = "Administration, Customers, Orders";
            Page.MetaDescription = "Administer Customers and Orders"; 

            if (!IsPostBack)
            {
                // Set the selected menu item in the Master page.
                SelectedMenu = "administration";
            }
        }
    }
}
