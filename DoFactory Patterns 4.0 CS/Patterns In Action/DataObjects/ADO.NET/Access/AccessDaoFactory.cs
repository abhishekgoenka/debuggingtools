using System;
using System.Collections.Generic;
using System.Text;

namespace DataObjects.AdoNet.Access
{
    /// <summary>
    /// Microsoft Access specific factory that creates Microsoft Access 
    /// specific data access objects.
    /// </summary>
    /// <remarks>
    /// GoF Design Patterns: Factory.
    /// </remarks>
    public class AccessDaoFactory : IDaoFactory
    {
        /// <summary>
        /// Gets a Microsoft Access specific customer data access object.
        /// </summary>
        public ICustomerDao CustomerDao { get { return new AccessCustomerDao(); } }

        /// <summary>
        /// Gets a Microsoft Access specific order data access object.
        /// </summary>
        public IOrderDao OrderDao { get { return new AccessOrderDao(); } }

        /// <summary>
        /// Gets a Microsoft Access specific order detail data access object.
        /// </summary>
        public IOrderDetailDao OrderDetailDao { get { return new AccessOrderDetailDao(); } }

        /// <summary>
        /// Gets a Microsoft Access specific product data access object.
        /// </summary>
        public IProductDao ProductDao { get { return new AccessProductDao(); } }

        /// <summary>
        /// Gets a Microsoft Access specific category data access object.
        /// </summary>
        public ICategoryDao CategoryDao { get { return new AccessCategoryDao(); } }
    }
}
