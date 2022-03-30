using System;
using ASPNETWebApplication.Controls;
using System.Security.Policy;

namespace ASPNETWebApplication
{
    public partial class SiteMasterPage : System.Web.UI.MasterPage
    {
        /// <summary>
        /// Establishes the composite menu hierarchy which is present on all pages.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Build the composite menu tree
                // This tree implements the Composite Design Pattern.
                var root = new MenuCompositeItem("root", null);
                var home = new MenuCompositeItem("home", UrlMaker.ToDefault()); 
                var shop = new MenuCompositeItem("shopping", UrlMaker.ToShopping()); 
                var prod = new MenuCompositeItem("products", UrlMaker.ToProducts()); 
                var srch = new MenuCompositeItem("search", UrlMaker.ToSearch());
                var cart = new MenuCompositeItem("cart", UrlMaker.ToCart());
                var admn = new MenuCompositeItem("administration", UrlMaker.ToAdmin());
                var cust = new MenuCompositeItem("customers", UrlMaker.ToCustomers());
                var ordr = new MenuCompositeItem("orders", UrlMaker.ToOrders()); 

                MenuCompositeItem auth;
                if (Request.IsAuthenticated)
                    auth = new MenuCompositeItem("logout", UrlMaker.ToLogout()); 
                else
                    auth = new MenuCompositeItem("login", UrlMaker.ToLogin()); 

                shop.Children.Add(prod);
                shop.Children.Add(srch);
                shop.Children.Add(cart);
                admn.Children.Add(cust);
                admn.Children.Add(ordr);
                root.Children.Add(home);
                root.Children.Add(shop);
                root.Children.Add(admn);
                root.Children.Add(auth);
                

                TheMenuComposite.MenuItems = root;
            }
        }

        /// <summary>
        /// Gets the menu from the master page. This property makes the menu 
        /// accessible from contentplaceholders. This allows the individual pages 
        /// to set the selected menu item.
        /// </summary>
        public MenuComposite TheMenuInMasterPage
        {
            get { return this.TheMenuComposite; }
        }

        /// <summary>
        /// Gets the page render time.
        /// </summary>
        protected string PageRenderTime
        {
            get
            {
                // Be sure that all ContentPlaceHolder pages are derived from PageBase.
                // BTW: this is how you access content pages from a master page --
                // most developers ask about access the other way around, that is, access 
                // the master page from the content pages which is also demonstrated here 
                // with the above TheMenuInMasterPage property.
                try
                {
                    var pageBase = this.ContentPlaceHolder1.Page as PageBase; 
                    return pageBase.PageRenderTime;
                }
                catch { /* do nothing */ }

                return "";
            }
        }
    }
}
