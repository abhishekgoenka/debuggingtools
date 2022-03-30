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

namespace ASPNETWebApplication.WebShop
{
    /// <summary>
    /// Checkout page.
    /// </summary>
    public partial class Checkout : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Meta data: helpful for SEO (search engine optimization)
            Page.Title = "Checkout";
            Page.MetaKeywords = "Checkout, Pay, Shopping Cart, Electronic Products";
            Page.MetaDescription = "Checkout Shopping Cart for Patterns in Action Application"; 

            if (!IsPostBack)
            {
                // Set the selected menu item in Master page.
                SelectedMenu = "cart";
            }
        }
    }
}
