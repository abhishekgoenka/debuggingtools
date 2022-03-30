using System;
using System.Collections.Generic;
using System.Text;

namespace DataObjects.AdoNet.Oracle
{
    /// <summary>
    /// Oracle specific factory that creates Oracle specific data access objects.
    /// 
    /// GoF Design Pattern: Factory.
    /// </summary>
    public class OracleDaoFactory : IDaoFactory
    {
        /// <summary>
        /// Gets an Oracle specific customer data access object.
        /// </summary>
        public ICustomerDao CustomerDao
        {
            get { return new OracleCustomerDao(); }
        }

        /// <summary>
        /// Gets an Oracle specific order data access object.
        /// </summary>
        public IOrderDao OrderDao
        {
            get { return new OracleOrderDao(); }
        }

        /// <summary>
        /// Gets an Oracle specific order details data access object.
        /// </summary>
        public IOrderDetailDao OrderDetailDao
        {
            get { return new OracleOrderDetailDao(); }
        }

        /// <summary>
        /// Gets an Oracle specific product data access object.
        /// </summary>
        public IProductDao ProductDao
        {
            get { return new OracleProductDao(); }
        }

        /// <summary>
        /// Gets an Oracle specific category data access object.
        /// </summary>
        public ICategoryDao CategoryDao
        {
            get { return new OracleCategoryDao(); }
        }
    }
}
