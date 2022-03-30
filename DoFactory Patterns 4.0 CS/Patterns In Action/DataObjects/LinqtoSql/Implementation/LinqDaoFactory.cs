using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataObjects.LinqToSql.Implementation
{
    /// <summary>
    /// Linq-to-Sql specific factory that creates data access objects.
    /// </summary>
    /// <remarks>
    /// GoF Design Patterns: Factory.
    /// </remarks>
    public class LinqDaoFactory : IDaoFactory
    {
        /// <summary>
        /// Gets a Linq specific customer data access object.
        /// </summary>
        public ICustomerDao CustomerDao
        {
            get { return new LinqCustomerDao(); }
        }

        /// <summary>
        /// Gets a Linq specific order data access object.
        /// </summary>
        public IOrderDao OrderDao
        {
            get { return new LinqOrderDao(); }
        }

        /// <summary>
        /// Gets a Linq specific order detail data access object.
        /// </summary>
        public IOrderDetailDao OrderDetailDao
        {
            get { return new LinqOrderDetailDao(); }
        }


        /// <summary>
        /// Gets a Linq specific product data access object.
        /// </summary>
        public IProductDao ProductDao
        {
            get { return new LinqProductDao(); }
        }

        /// <summary>
        /// Gets a Linq specific category data access object.
        /// </summary>
        public ICategoryDao CategoryDao
        {
            get { return new LinqCategoryDao(); }
        }
    }
}

