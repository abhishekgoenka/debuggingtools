using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ASPNETMVCApplication.Areas.Shop.Controllers;
using ASPNETMVCApplication.Areas.Shop.Models;
using ASPNETMVCApplication.Code.HtmlHelpers;
using ASPNETMVCApplication.ActionServiceReference;
using ASPNETMVCApplication.Repositories;

using ASPNETMVCApplication.Tests.Moq;
using Moq;


namespace ASPNETMVCApplication.Tests.Controllers
{
    /// <summary>
    /// ShopControllerTest. TestClass for ShopController.
    /// 
    /// Note: the Pattern in each TestMethod is: AAA (Arrange, Act, Assert).
    /// </summary>
    [TestClass]
    public class ShopControllerTest
    {
        private Mock<ICategoryRepository> _mockCategoryRepository;
        private Mock<IProductRepository> _mockProductRepository;
        private Mock<ICartRepository> _mockCartRepository;

        /// <summary>
        /// Initialize testing environment by 'setting up' Mocks.
        /// </summary>
        [TestInitialize]
        public void InitializeMocks()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockCartRepository = new Mock<ICartRepository>();

            // Setup getting a list of categories
            var categories = new List<Category> { new Category { CategoryId = 1, Name = "test-category" } };
            _mockCategoryRepository.Setup(c => c.GetList(null)).Returns(categories);

            // Setup getting a list of products
            var products = new List<Product> { new Product { ProductId = 1, ProductName = "test-product" } };
            _mockProductRepository.Setup(p => p.GetList(It.IsAny<Criterion>())).Returns(products); 

            // Setup getting a product
            var product = new Product { ProductId = 1, ProductName = "test-product" };
            _mockProductRepository.Setup(p => p.Get(1)).Returns(product);

            // Setup searching for products
            _mockProductRepository.Setup(p => p.Search("", 0, "productid", "asc")).Returns(products);

            // Setup getting the shopping cart
            var cartItems = new[] { new ShoppingCartItem {
                 Id = 1, Name = "test-product", Quantity = 1 } };
            var cart = new ShoppingCart { CartItems = cartItems, ShippingMethod = "UPS" };
            _mockCartRepository.Setup(c => c.GetCart()).Returns(cart);

            // Setup cart CRUD methods
            _mockCartRepository.Setup(c => c.Insert(It.IsAny<ShoppingCartItem>()));
            _mockCartRepository.Setup(c => c.Update(It.IsAny<ShoppingCartItem>()));
            _mockCartRepository.Setup(c => c.Delete(It.IsAny<int>()));

            // Setup cart shipping method change
            _mockCartRepository.Setup(c => c.UpdateShippingMethod(It.IsAny<string>()));
        }

        // Private helper. Creates shop controller.
        // This is a Factory Method.
        private ShopController CreateShopController()
        {
            // Note: This is where DI (Dependency Injection) takes place.
            // The repositories are injected (via the constructor) into the controller.
            return new ShopController(_mockCategoryRepository.Object,  
                                      _mockProductRepository.Object, 
                                      _mockCartRepository.Object);
        }

        /// <summary>
        /// Tests Index page.
        /// </summary>
        [TestMethod]
        public void IndexTest()
        {
            // Arrange
            var controller = CreateShopController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(result.ViewName, string.Empty);
        }

        /// <summary>
        /// Tests Products action method. Category and Products retrieval.
        /// </summary>
        [TestMethod]
        public void ProductsTest()
        {
            // Arrange 
            var controller = CreateShopController();

            // Act
            var result = controller.Products() as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(SortedList<ProductModel>));

            Assert.AreEqual(result.ViewName, string.Empty);

            var categories = result.ViewData["Categories"] as SelectList;
            Assert.AreEqual(1, categories.Count());

            var products = result.ViewData.Model as SortedList<ProductModel>;
            Assert.AreEqual(1, products.List.Count());
        }

        /// <summary>
        /// Test Product action method. Single product retrieval.
        /// </summary>
        [TestMethod]
        public void ProductTest()
        {
            // Arrange
            var controller = CreateShopController();

            // Act
            var result = controller.Product(1) as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(ProductModel));
            Assert.AreEqual((result.ViewData.Model as ProductModel).Name, "test-product");
        }

        /// <summary>
        /// Test Product action method with incoming error message.
        /// </summary>
        [TestMethod]
        public void ProductWithMessageTest()
        {
            // Arrange
            var controller = CreateShopController();

            // Act
            var result = controller.Product(1, "test-message") as ViewResult;

            // Assert
            Assert.AreEqual((result.ViewData.Model as ProductModel).Name, "test-product");
            Assert.AreEqual(result.ViewData.ModelState["Message"].Errors[0].ErrorMessage, "test-message");
        }

        /// <summary>
        /// Test Product Search Action method. Returns sorted product list.
        /// Note: this creates a Fake HttpContext which is necessary to access an [HttpPost] decorated action method.
        /// </summary>
        [TestMethod]
        public void SearchTest()
        {
            // Arrange
            var controller = CreateShopController();

            controller.SetFakeControllerContext();
            controller.Request.SetHttpMethodResult("POST");

            // Act
            var result = controller.Search() as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(SortedList<ProductModel>));

            var products = result.ViewData.Model as SortedList<ProductModel>;
            Assert.AreEqual(1, products.List.Count());
        }

        /// <summary>
        /// Tests Cart action method. Returns complete shopping cart.
        /// </summary>
        [TestMethod]
        public void ShowCartTest()
        {
            // Arrange
            var controller = CreateShopController();

            // Act
            var result = controller.Cart() as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(CartModel));
            
            var cart = result.ViewData.Model as CartModel;
            Assert.AreEqual(1, cart.CartItems.Count);
        }

        /// <summary>
        /// Test Product action method in which product is added to shopping cart.
        /// </summary>
        [TestMethod]
        public void AddProductToCartTest()
        {
            var controller = CreateShopController();

            // Act
            var result = controller.Product(1, 3) as ViewResult;

            // Assert
            _mockCartRepository.Verify(c => c.Insert(It.IsAny<ShoppingCartItem>()),Times.Exactly(1));

            // Redirect to Cart
            Assert.IsNull(result);
        }


        /// <summary>
        /// Test Cart action method in which a cart item is removed from shopping cart.
        /// </summary>
        [TestMethod]
        public void DeleteProductFromCartTest()
        {
            // Arrange
            var controller = CreateShopController();

            // Act
            string delete = "1";
            var result = controller.Cart(delete) as ViewResult;

            // Assert
            _mockCartRepository.Verify(c => c.Delete(It.IsAny<int>()), Times.Exactly(1));

            // Redirect to Cart 
            Assert.IsNull(result);
        }

        /// <summary>
        /// Test Shipping action method in which shipping method is changed.
        /// </summary>
        [TestMethod]
        public void ShippingChangeShippingTest()
        {
            // Arrange
            var controller = CreateShopController();

            // Act
            var result = controller.Shipping("1") as ViewResult;

            // Assert
            _mockCartRepository.Verify(c => c.UpdateShippingMethod(It.IsAny<string>()), Times.Exactly(1));

            // Redirect to Cart 
            Assert.IsNull(result);
        }

        /// <summary>
        /// Test Recalculate action method in which cart item quanties are changed and recalculated.
        /// </summary>
         [TestMethod]
        public void RecalculateTest()
        {
            // Arrange
            var controller = CreateShopController();

            // Act
            var formCollection = new FormCollection();
            formCollection.Add("prodid-1", "2");
            var result = controller.Recalculate(formCollection) as ViewResult;

            // Assert
            _mockCartRepository.Verify(c => c.Update(It.IsAny<ShoppingCartItem>()), Times.Exactly(1));
            Assert.IsNull(result);
        }

        /// <summary>
        /// Test Checkout action method. This simply redirects to Checkout page.
        /// </summary>
        [TestMethod]
        public void CheckoutTest()
        {
            // Arrange
            var controller = CreateShopController();

            // Act
            var result = controller.Checkout() as ViewResult;

            // Display checkout page
            Assert.AreEqual(result.ViewName, string.Empty);
        }
    }
}


