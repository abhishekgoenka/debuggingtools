using System.Data;
using System.Linq;
using System.ServiceModel.DomainServices.EntityFramework;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;

namespace Silverlight_Patterns_in_Action.Web.Services.Web
{
    /// <summary>
    /// Implements application logic using the ActionEntities context.
    /// </summary>
    [EnableClientAccess()]
    public class ActionDomainService : LinqToEntitiesDomainService<ActionEntities>
    {
        /// <summary>
        /// Gets all product categories.
        /// </summary>
        /// <returns>Queryable collection of categories.</returns>
        public IQueryable<Category> GetCategories()
        {
            return this.ObjectContext.Categories;
        }

     
        /// <summary>
        /// Gets a list of customers.
        /// </summary>
        /// <returns>Queryable collection of customers.</returns>
        [RequiresAuthentication]
        public IQueryable<Customer> GetCustomers()
        {
            return this.ObjectContext.Customers;
        }

        /// <summary>
        /// Finds a list of customers by company name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Queryable collection of customers.</returns>
        [RequiresAuthentication]
        public IQueryable<Customer> FindCustomers(string name)
        {
            return this.ObjectContext.Customers.Where(c => c.CompanyName.Contains(name));
        }

        /// <summary>
        /// Inserts a new Customer in the database.
        /// </summary>
        /// <param name="customer">The customer.</param>
        [RequiresAuthentication]
        public void InsertCustomer(Customer customer)
        {
            if ((customer.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(customer, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Customers.AddObject(customer);
            }
        }

        /// <summary>
        /// Updates a customer in the database.
        /// </summary>
        /// <param name="currentCustomer">The customer.</param>
        [RequiresAuthentication]
        public void UpdateCustomer(Customer currentCustomer)
        {
            this.ObjectContext.Customers.AttachAsModified(currentCustomer, this.ChangeSet.GetOriginal(currentCustomer));
        }

        /// <summary>
        /// Deletes a customer from the database.
        /// </summary>
        /// <param name="customer">The customer.</param>
        [RequiresAuthentication]
        public void DeleteCustomer(Customer customer)
        {
            if ((customer.EntityState == EntityState.Detached))
            {
                this.ObjectContext.Customers.Attach(customer);
            }
            this.ObjectContext.Customers.DeleteObject(customer);
        }

        /// <summary>
        /// Gets a list of orders with associated customer information.
        /// </summary>
        /// <returns>Queryable collection of orders.</returns>
        [RequiresAuthentication]
        public IQueryable<Order> GetOrders()
        {
            return this.ObjectContext.Orders.Include("Customer");
        }

        /// <summary>
        /// Gets a list of orders by customer.
        /// </summary>
        /// <returns>Queryable collection of orders.</returns>
        [RequiresAuthentication]
        public IQueryable<Order> GetOrdersByCustomer(int customerId)
        {
            return this.ObjectContext.Orders.Where(o => o.CustomerId == customerId);
        }

        /// <summary>
        /// Gets the order for a given year.
        /// </summary>
        /// <param name="year">The order year.</param>
        /// <returns>A queryable collection of orders.</returns>
        [RequiresAuthentication]
        public IQueryable<Order> GetOrdersByYear(int year)
        {
            return this.ObjectContext.Orders.Where(o => o.OrderDate.Year == year).OrderBy(o => o.OrderDate.Month);
        }

        /// <summary>
        /// Gets the number of orders placed for a specific customer.
        /// </summary>
        /// <param name="customerId">The customer's identifier.</param>
        /// <returns>Number of orders.</returns>
        [Invoke]    
        [RequiresAuthentication]
        public int GetOrderCountByCustomer(int customerId)
        {
            return this.ObjectContext.Orders.Where(o => o.CustomerId == customerId).Count();
        }
       
        /// <summary>
        /// Gets the order details (line items) for a given order.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns>A queryable collection of order details.</returns>
        [RequiresAuthentication]
        public IQueryable<OrderDetail> GetOrderDetailsByOrder(int orderId)
        {
            return this.ObjectContext.OrderDetails.Include("Product").Where(o => o.OrderId == orderId);
        }

        /// <summary>
        /// Gets products by category.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns>Queryable collection of products.</returns>
        public IQueryable<Product> GetProductsByCategory(int categoryId)
        {
            return this.ObjectContext.Products.Where(p => p.CategoryId == categoryId);
        }

        /// <summary>
        /// Get products given some search criteria (name and price range).
        /// </summary>
        /// <param name="productName">Product Name</param>
        /// <param name="fromPrice">Price range starting price.</param>
        /// <param name="thruPrice">Price range ending price</param>
        /// <returns>Queryable colelction of products.</returns>
        public IQueryable<Product> FindProducts(string productName, double? fromPrice, double? thruPrice)
        {
            var query = this.ObjectContext.Products;
            if (!string.IsNullOrEmpty(productName) && fromPrice != null && thruPrice != null)
                return this.ObjectContext.Products.Where(p => p.ProductName.Contains(productName) && p.UnitPrice > (decimal)fromPrice && p.UnitPrice < (decimal)thruPrice);
            else if (!string.IsNullOrEmpty(productName))
                return this.ObjectContext.Products.Where(p => p.ProductName.Contains(productName));
            else if (fromPrice != null && thruPrice != null)
                return this.ObjectContext.Products.Where(p => p.UnitPrice > (decimal)fromPrice && p.UnitPrice < (decimal)thruPrice);

            return query;
        }
     

        /// <summary>
        /// Important Error Reporting method.  
        /// Add logging information here.
        /// </summary>
        /// <param name="errorInfo"></param>
        protected override void OnError(DomainServiceErrorInfo errorInfo)
        {
            //errorInfo logging here..
            base.OnError(errorInfo);
        }
    }
}


