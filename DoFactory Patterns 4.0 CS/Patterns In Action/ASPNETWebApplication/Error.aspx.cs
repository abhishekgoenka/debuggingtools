using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPNETWebApplication
{
    public partial class Error : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // No additional meta data (no need for SEO here).
            Page.Title = "Error Page";

            if (!IsPostBack)
            {
                // Set the selected menu item in Master page.
                SelectedMenu = "home";

                // Register javascript that opens popup windows.
                RegisterOpenWindowJavaScript();
            }
        }
    }
}