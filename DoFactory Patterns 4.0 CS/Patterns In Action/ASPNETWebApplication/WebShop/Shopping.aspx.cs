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
    /// Central shopping page. Has links to other pages.
    /// </summary>
    public partial class Shopping : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Meta data: helpful for SEO (search engine optimization)
            Page.Title = "Shopping";
            Page.MetaKeywords = "Shopping, Electronic Products";
            Page.MetaDescription = "Start your Shopping for Electronic Products at Patterns in Action"; 

            if (!IsPostBack)
            {
                // Set the selected menu item in Master page.
                SelectedMenu = "shopping";
            }
        }
    }
}
