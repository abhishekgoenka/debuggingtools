using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASPNETMVCApplication.Areas.Admin.Models
{
    /// <summary>
    /// Customer Model class.
    /// Properties have data annotations used for Validation and Display.
    /// </summary>
    public class CustomerModel
    {
        /// <summary>
        /// The Customer Identifier.
        /// </summary>
        [DisplayName("Id")]
        public int CustomerId { get; set; }

        /// <summary>
        /// Customer Company name.
        /// </summary>
        [DisplayName("Company Name")]
        [Required(ErrorMessage = "Company Name is required.")]
        [StringLength(30, ErrorMessage = "Company Name can be at most 30 characters")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Customer City. 
        /// </summary>
        [Required(ErrorMessage = "City is required.")]
        [StringLength(30, ErrorMessage = "City can be at most 30 characters")]
        public string City { get; set; }

        /// <summary>
        /// Customer country.
        /// </summary>
        [Required(ErrorMessage = "Country is required.")]
        [StringLength(30, ErrorMessage = "Country can be at most 30 characters")]
        public string Country { get; set; }

        /// <summary>
        /// Record Version number.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Total orders placed by customer.
        /// </summary>
        public int NumOrders { get; set; }

        /// <summary>
        /// Customer's last order date.
        /// </summary>
        public string LastOrderDate { get; set; }
    }
}