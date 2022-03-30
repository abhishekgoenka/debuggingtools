using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ASPNETWebApplication.Repositories;
using System.Web.Security;

namespace ASPNETWebApplication.WebAuth
{
    public partial class Login : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Meta data: helpful for SEO (search engine optimization)
            Page.Title = "Login";
            Page.MetaKeywords = "Login, Administration";
            Page.MetaDescription = "Login to Administrative Area in Patterns in Action"; 

            if (!IsPostBack)
            {
                // Set the selected menu item in Master page.
                SelectedMenu = "login";

                Tries = 0;

                // Put cursor in first field
                TextboxUserName.Focus();
            }
        }

        protected void ButtonSubmit_Click(object sender, System.EventArgs e)
        {
            string username = TextboxUserName.Text.Trim();
            string password = TextboxPassword.Text.Trim();

            var repository = new AuthRepository();

            if (repository.Login(username, password))
            {
                FormsAuthentication.SetAuthCookie(username, false);

                string redirectUrl = FormsAuthentication.GetRedirectUrl(username, false);
                if (redirectUrl != null && redirectUrl.IndexOf("admin") >= 0)
                    FormsAuthentication.RedirectFromLoginPage(username, false);
                else
                    Response.Redirect(UrlMaker.ToAdmin()); 
            }
            else
            {
                if (Tries >= 2)
                    Response.Redirect(UrlMaker.ToDefault()); 
                else
                {
                    Tries += 1;
                    this.LiteralError.Text = "Invalid Username or Password. Please try again.";
                }
            }
        }

        // Counter for number of login attempts.
        private int Tries
        {
            get { return int.Parse(ViewState["Tries"].ToString()); }
            set { ViewState["Tries"] = value; }
        }
    }
}
