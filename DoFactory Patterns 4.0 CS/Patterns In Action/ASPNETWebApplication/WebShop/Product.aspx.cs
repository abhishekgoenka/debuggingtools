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

namespace ASPNETWebApplication.WebShop
{
    /// <summary>
    /// Product detail page.
    /// </summary>
    public partial class Product : PageBase
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
            var shopping = new SiteMapNode(e.Provider, "Shopping", "~/shop", "shopping");
            var products = new SiteMapNode(e.Provider, "Products", "~/shop/products", "product catalog");
            var product = new SiteMapNode(e.Provider, "Product", null, "product details");

            shopping.ParentNode = home;
            products.ParentNode = shopping;
            product.ParentNode = products;

            return product;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Meta data: helpful for SEO (search engine optimization)
            Page.Title = "Product Details";
            Page.MetaKeywords = "Product Details, Electronic Products";
            Page.MetaDescription = "Product Details for Electronic Products at Patterns in Action"; 

            if (!IsPostBack)
            {
                // Set the selected menu item in Master page.
                SelectedMenu = "products";

                // Save off ProductId for this page.
                ProductId = int.Parse(Page.RouteData.Values["productid"].ToString());

                // This page is also accessible from Cart page
                if (Request["HTTP_REFERER"].ToString().Contains("cart"))
                    HyperLinkBack.Text = "&lt; back to shopping cart";

                // Get product image from image service. This demo supplies just a single image.
                ImageProduct.ImageUrl = imageService + "GetProductImage/" + ProductId;
            }
        }

        /// <summary>
        /// Adds item to shopping cart and redirect to shopping cart page.
        /// </summary>
        protected void ButtonAddToCart_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid) return;

            // Retrieve product via Product Facade.
            var repository = new ProductRepository();
            ActionServiceReference.Product product = repository.GetProduct(ProductId);

            // Get product details and add information to cart.
            int productId = product.ProductId;
            string name = product.ProductName;
            double unitPrice = product.UnitPrice;

            int quantity;
            if (!int.TryParse(TextBoxQuantity.Text.Trim(), out quantity))
                quantity = 1;

            var cartRepository = new CartRepository();
            cartRepository.AddItem(productId, name, quantity, unitPrice);

            // Show shopping cart to user.
            Response.Redirect(UrlMaker.ToCart()); 
        }

        /// <summary>
        /// Gets and sets productId to Session.
        /// </summary>
        private int ProductId
        {
            set { Session["productId"] = value; }
            get { return int.Parse(Session["productId"].ToString()); }
        }
    }
}
