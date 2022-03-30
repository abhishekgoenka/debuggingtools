using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataObjects
{
    /// <summary>
    /// Abstract factory interface. Creates data access objects.
    /// </summary>
    /// <remarks>
    /// GoF Design Pattern: Factory.
    /// </remarks>
    public interface IDaoFactory
    {
        /// <summary>
        /// Gets a customer data access object.
        /// </summary>
        ICustomerDao CustomerDao { get; }

        /// <summary>
        /// Gets an order data access object.
        /// </summary>
        IOrderDao OrderDao { get; }

        /// <summary>
        /// Gets an order detail data access object.
        /// </summary>
        IOrderDetailDao OrderDetailDao { get; }

        /// <summary>
        /// Gets a product data access object.
        /// </summary>
        IProductDao ProductDao { get; }

        /// <summary>
        /// Gets a category data access object.
        /// </summary>
        ICategoryDao CategoryDao { get; }
    }
}
