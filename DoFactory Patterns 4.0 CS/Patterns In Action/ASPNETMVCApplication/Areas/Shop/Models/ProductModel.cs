using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace ASPNETMVCApplication.Areas.Shop.Models
{
    /// <summary>
    /// Class product model. Represents product.
    /// </summary>
    public class ProductModel
    {
        /// <summary>
        ///  Category name of product.
        /// </summary>
        [DisplayName("Category")]
        public string CategoryName { get; set; }

        /// <summary>
        /// Product identifier.
        /// </summary>
        [DisplayName("Id")]
        public int ProductId { get; set; }

        /// <summary>
        /// Product name.
        /// </summary>
        [DisplayName("Product Name")]
        public string Name { get; set; }

        /// <summary>
        /// Weight of product.
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// Price of product.
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// Number of units in stock of product.
        /// </summary>
        [DisplayName ("# in Stock")]
        public int UnitsInStock { get; set; }
    }
}