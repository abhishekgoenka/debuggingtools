using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using ASPNETMVCApplication.Code.Filters;
using ASPNETMVCApplication.Repositories;
using ASPNETMVCApplication.ActionServiceReference;
using ASPNETMVCApplication.Areas.Shop.Models;
using ASPNETMVCApplication.Code.HtmlHelpers;
using ASPNETMVCApplication.Controllers;
using ASPNETWebApplication;

namespace ASPNETMVCApplication.Areas.Shop.Controllers
{
    /// <summary>
    /// Controller class for the Shopping area.
    /// </summary>
    public class ShopController : BaseController
    {
        private ICategoryRepository _categoryRepository;
        private IProductRepository _productRepository;
        private ICartRepository _cartRepository;

        /// <summary>
        /// Default Constructor for ShopController.
        /// </summary>
        public ShopController()
            : this(new CategoryRepository(), 
                   new ProductRepository(), 
                   new CartRepository())
        {
        }

        /// <summary>
        /// Overloaded 'injectable' Constructor for ShopController.
        ///
        /// Pattern: Constructor Dependency Injection (DI).
        /// </summary>
        /// <param name="categoryRepository"></param>
        /// <param name="productRepository"></param>
        /// <param name="cartRepository"></param>
        public ShopController(ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            ICartRepository cartRepository)
        {
           _categoryRepository = categoryRepository;
           _productRepository = productRepository;
           _cartRepository = cartRepository;
        }

        /// <summary>
        /// Action method. 
        /// Prepares shopping home page view (Index) 
        /// </summary>
        /// <returns></returns>
        [Menu(MenuItem.Shopping)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///  Action method. 
        ///  Gets category and product data.
        ///  Product data is filtered by category and sorted.
        /// </summary>
        /// <param name="categoryId">Category for which to filter products. Default is ProductId 1.</param>
        /// <param name="sort">Sort of product list. Default is product Id.</param>
        /// <param name="order">Sord order of product list. Default = 'asc'.</param>
        /// <returns></returns>
        [Menu(MenuItem.Products)]
        public ActionResult Products(int categoryId = 1, string sort = "productid", string order = "asc")
        {
            // Selectlist of categories dropdown
            ViewData["Categories"] = new SelectList(_categoryRepository.GetList(), "CategoryId", "Name", categoryId);

            var criterion = new Criterion(sort, order, "CategoryId", Operator.Equals, categoryId);
            return View(new SortedList<ProductModel>(_productRepository.GetList(criterion).ToModel(), sort, order));
        }

        /// <summary>
        /// Action Method.
        /// Gets product details given a product id.
        /// </summary>
        /// <param name="productId">Product identifier.</param>
        /// <param name="message">Optional message coming from prior page and need to be displayed.</param>
        /// <returns></returns>
        [Menu(MenuItem.Products)]
        public ActionResult Product(int productId, string message = null)
        {
            ViewData["BreadCrumbs"] = new List<BreadCrumb> { 
                new BreadCrumb { Url = UrlMaker.ToDefault(), Title = "home" }, 
                new BreadCrumb { Url = UrlMaker.ToShopping(), Title = "shopping" }, 
                new BreadCrumb { Url = UrlMaker.ToProducts(), Title = "product catalog" }, 
                new BreadCrumb { Title = "product details" } };

            ViewData["ProductImage"] = imageService + "GetProductImage/" + productId;

            if (message != null) ModelState.AddModelError("Message", message);

            return View(_productRepository.Get(productId).ToModel());
        }

        /// <summary>
        /// Action method. HTTP Post only.
        /// Adds a product to the shopping cart.
        /// </summary>
        /// <param name="productId">The product to be added.</param>
        /// <param name="quantity">The quantite of product.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Product(int productId, int quantity)
        {
            if (quantity > 0 && quantity < 100)
            {
                var product = _productRepository.Get(productId);
                var cartItem = new ShoppingCartItem { Id = productId, Name = product.ProductName, Quantity = quantity, UnitPrice = product.UnitPrice };

                _cartRepository.Insert(cartItem);

                return RedirectToAction("Cart");
            }

            return RedirectToAction("Product", new { productId = productId, message = "Quantity must be between 1 and 99." });
        }

        /// <summary>
        /// Action method.
        /// Searches for products given a name and/or price range.
        /// </summary>
        /// <param name="productName">The product name.</param>
        /// <param name="ranges">The price range identifier.</param>
        /// <param name="sort">Sort column for product list.</param>
        /// <param name="order">Sort order for product list.</param>
        /// <returns></returns>
        [Menu(MenuItem.Search)]
        public ActionResult Search(string productName = "", string ranges = "0", string sort = "productid", string order = "asc")
        {
            ViewData["ProductName"] = productName;
            ViewData["Ranges"] = new SelectList(PriceRange.List, "RangeId", "RangeText", ranges); 

            var productModels = new List<ProductModel>();
            if (Request.HttpMethod == "POST")
            {
                var model = _productRepository.Search(productName, int.Parse(ranges), sort, order).ToModel();
                return View(new SortedList<ProductModel>(model, sort, order));
            }

            return View(new SortedList<ProductModel>(new List<ProductModel>(), sort, order));
        }

        /// <summary>
        /// Action method. HTTP GET only.
        /// Gets shopping cart data and shipping options.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Menu(MenuItem.Cart)]
        public ActionResult Cart()
        {
            var cart = _cartRepository.GetCart();

            // Populate and select shipping dropdown
            ViewData["Shipping"] = new SelectList(ShippingMethod.List, "ShippingId", "ShippingName", GetShippingId(cart.ShippingMethod));

            return View(cart.ToModel());
        }

        /// <summary>
        /// Action method. HTTP POST only.
        /// Deletes a shopping cart item.
        /// </summary>
        /// <param name="delete">Product id of item do delete.</param>
        /// <returns></returns>
        [HttpPost, Menu(MenuItem.Cart)]
        public ActionResult Cart(string delete = null)
        {
            if (!string.IsNullOrEmpty(delete))
            {
                int productId = int.Parse(delete);
                _cartRepository.Delete(productId);
            }

            return RedirectToAction("Cart");
        }

        /// <summary>
        /// Action method.
        /// Checkout shopping cart (not implemented).
        /// </summary>
        /// <returns></returns>
        [Menu(MenuItem.Cart)]
        public ActionResult Checkout()
        {
            return View();
        }

        /// <summary>
        /// Action method. HTTP POST only.
        /// Recalculate shopping cart. Evaluate each item's quantity.
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost, Menu(MenuItem.Cart)]
        public ActionResult Recalculate(FormCollection formCollection)
        {
            foreach (var key in formCollection.AllKeys)
            {
                if (key.StartsWith("prodid-"))
                {
                    // Extract productId for cart line item.
                    int productId = int.Parse(key.Substring(7));

                    int quantity;
                    if (!int.TryParse(formCollection[key], out quantity))
                        quantity = 0;

                    if (quantity > 0 && quantity < 100)
                        _cartRepository.Update(new ShoppingCartItem { Id = productId, Quantity = quantity }); 
                    else
                        _cartRepository.Delete(productId);
                }
            }

            return RedirectToAction("Cart");
        }

        /// <summary>
        /// Action method. HTTP POST only.
        /// Updates shipping method.  Cart page will display updated total, subtotals, etc.
        /// </summary>
        /// <param name="shippingId"></param>
        /// <returns></returns>
        [HttpPost, Menu(MenuItem.Cart)]
        public ActionResult Shipping(string shippingId)
        {
            if (!string.IsNullOrEmpty(shippingId))
            {
                string method = GetShippingMethod(int.Parse(shippingId));
                _cartRepository.UpdateShippingMethod(method);
            }

            return RedirectToAction("Cart");
        }

        // Helper method. Returns shipping id based on shipping method provided.
        private int GetShippingId(string shippingMethod)
        {
            return ShippingMethod.List.Single(m => m.ShippingName == shippingMethod).ShippingId;
        }

        // Helper method. Returns shipping method given a shipping identifier.
        private string GetShippingMethod(int shippingId)
        {
            return ShippingMethod.List.Single(m => m.ShippingId == shippingId).ShippingName;
        }
    }
}


