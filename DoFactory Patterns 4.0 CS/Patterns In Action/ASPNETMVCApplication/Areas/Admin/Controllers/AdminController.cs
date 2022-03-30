using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using ASPNETMVCApplication.Code.Filters;
using ASPNETMVCApplication.Repositories;
using ASPNETMVCApplication.Areas.Admin.Models;
using ASPNETMVCApplication.Code.HtmlHelpers;
using ASPNETMVCApplication.Controllers;
using ASPNETWebApplication;

namespace ASPNETMVCApplication.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller class for the Administration area.
    /// Authorization is required to access this class on all action methods.
    /// </summary>
    [Authorize]
    public class AdminController : BaseController
    {
        private ICustomerRepository _customerRepository;
        private IOrderRepository _orderRepository;

        /// <summary>
        /// Default Constructor for AdminController.
        /// </summary>
        public AdminController()
            : this(new CustomerRepository(), new OrderRepository())
        {
        }

        /// <summary>
        /// Overloaded 'injectable' Constructor for AdminController.
        /// 
        /// Pattern: Constructor Dependency Injection (DI).
        /// </summary>
        /// <param name="customerRepository">The customer repository.</param>
        /// <param name="orderRepository">The order repository.</param>
        public AdminController(ICustomerRepository customerRepository, IOrderRepository orderRepository)
        {
            this._customerRepository = customerRepository;
            this._orderRepository = orderRepository;
        }

        /// <summary>
        /// Action method. HTTP GET only.
        /// Prepares home page view (Index) 
        /// </summary>
        /// <returns>Home page view.</returns>
        [HttpGet]
        [Menu(MenuItem.Administration)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Action Method.
        /// Retrieves customers and optionally deletes customer record.
        /// </summary>
        /// <param name="sort">The sort variable. Default is customer id.</param>
        /// <param name="order">The sort order. Default is 'asc'.</param>
        /// <param name="delete">The customerId to be deleted. Default is null, no delete.</param>
        /// <returns></returns>
        [Menu(MenuItem.Customers)]
        public ActionResult Customers(string sort = "customerid", string order = "asc", string delete = null)
        {
            if (!string.IsNullOrEmpty(delete))
                ExecuteDelete(delete);

            var customerModels = _customerRepository.GetList(new Criterion(sort, order)).ToModel();
            return View(new SortedList<CustomerModel>(customerModels, sort, order));
        }

        // Private helper executing delete action
        private void ExecuteDelete(string delete)
        {
            int customerId = int.Parse(delete);

            var customer = _customerRepository.GetCustomerWithOrders(
                new Criterion("CustomerId", Operator.Equals, customerId));
            if (customer.Orders.Length > 0)
                Result = "Cannot delete customer because they have existing orders";
            else
            {
                _customerRepository.Delete(customerId);
                Result = "Customer has been deleted successfully";
            }
        }

        /// <summary>
        /// Action method. HTTP GET Only. 
        /// Retrieves a single customer record.
        /// </summary>
        /// <param name="customerId">The customer to be displayed.</param>
        /// <returns></returns>
        [HttpGet]
        [Menu(MenuItem.Customers)]
        public ActionResult Customer(int customerId)
        {
            SetCustomerViewData(customerId);

            var model = customerId == 0 ? new CustomerModel() : _customerRepository.Get(customerId).ToModel();
            return View(model);
        }


        /// <summary>
        /// Action method. HTTP POST only.
        /// Saves changed (new or updated) customer record.
        /// </summary>
        /// <param name="model">The customer model with changed data.</param>
        /// <returns></returns>
        [HttpPost]
        [Menu(MenuItem.Customers)]
        public ActionResult Customer(CustomerModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = model.FromModel();
                if (customer.CustomerId > 0)
                    _customerRepository.Update(customer);
                else
                    _customerRepository.Insert(customer);

                return RedirectToAction("Customers");
            }

            SetCustomerViewData(model.CustomerId);

            // Show with errors
            return View(model);
        }

        // private Helper
        private void SetCustomerViewData(int? customerId = 0)
        {
            ViewData["BreadCrumbs"] = new List<BreadCrumb> { 
                new BreadCrumb { Url = UrlMaker.ToDefault(), Title = "home" }, 
                new BreadCrumb { Url = UrlMaker.ToAdmin(), Title = "administration" }, 
                new BreadCrumb { Url = UrlMaker.ToCustomers(), Title = "customers" }, 
                new BreadCrumb { Title = "customer details" } };

            ViewData["CustomerImage"] = imageService + "GetCustomerImageLarge/" + customerId;
        }

        /// <summary>
        /// Orders Action method. Retrieves orders with a given sort order.
        /// </summary>
        /// <param name="sort">The sort variable. Default is 'customerId'</param>
        /// <param name="order">The sort order. Default is 'asc'.</param>
        /// <returns></returns>
        [Menu(MenuItem.Orders)]
        public ActionResult Orders(string sort = "customerid", string order = "asc")
        {
            var models = _customerRepository.GetCustomerListWithOrderStatistics(
                new Criterion(sort, order)).ToModel();

            return View(new SortedList<CustomerModel>(models, sort, order));
        }

        /// <summary>
        /// Action method. HTTP GET only.
        /// Retrieves a list of orders by customer.
        /// </summary>
        /// <param name="customerId">The customer Identifier</param>
        /// <returns></returns>
        [HttpGet]
        [Menu(MenuItem.Orders)]
        public ActionResult CustomerOrders(int customerId)
        {
            ViewData["CustomerId"] = customerId;

            var customer = _customerRepository.GetCustomerWithOrders(
                new Criterion("CustomerId", Operator.Equals, customerId));

            ViewData["Company"] = customer.Company;
            ViewData["BreadCrumbs"] = new List<BreadCrumb> { 
                new BreadCrumb { Url = UrlMaker.ToDefault(), Title = "home" }, 
                new BreadCrumb { Url = UrlMaker.ToAdmin(), Title = "administration" }, 
                new BreadCrumb { Url = UrlMaker.ToOrders(), Title = "orders" }, 
                new BreadCrumb { Title = "customer orders" } };

            return View(customer.Orders.ToList().ToModel());
        }


        /// <summary>
        /// Action method. HTTP GET only.
        /// Retrieves orderdetails for given customer and order id.
        /// </summary>
        /// <param name="customerId">The customer Identifier.</param>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Menu(MenuItem.Orders)]
        public ActionResult OrderDetails(int customerId, int orderId)
        {
            var order = _orderRepository.Get(orderId);

            ViewData["OrderDate"] = "Order Date: " + string.Format("{0:MM/dd/yyyy}", order.OrderDate);
            ViewData["BreadCrumbs"] = new List<BreadCrumb> { 
                new BreadCrumb { Url = UrlMaker.ToDefault(), Title = "home" }, 
                new BreadCrumb { Url = UrlMaker.ToAdmin(), Title = "administration" }, 
                new BreadCrumb { Url = UrlMaker.ToOrders(), Title = "orders" }, 
                new BreadCrumb { Url = UrlMaker.ToCustomerOrders(customerId), Title = "customer orders" }, 
                new BreadCrumb { Title = "line items" } };

            return View(order.OrderDetails.ToList().ToModel());
        }
    }
}
