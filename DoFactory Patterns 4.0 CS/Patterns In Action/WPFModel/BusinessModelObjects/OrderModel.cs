using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace WPFModel.BusinessModelObjects
{
    /// <summary>
    /// Model of the Order. 
    /// Not inherited from BaseModel because no orders are placed in this app.
    /// </summary>
    public class OrderModel
    {
        /// <summary>
        /// Gets or sets order identifier. This identifier is encrypted.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets order date.
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Gets or set required order delivery date.
        /// </summary>
        public DateTime RequiredDate { get; set; }

        /// <summary>
        /// Gets or sets freight (shipping) costs.
        /// </summary>
        public float Freight { get; set; }

        /// <summary>
        /// Gets or sets list of order details (line items) for order.
        /// </summary>
        public ObservableCollection<OrderDetailModel> OrderDetails { get; set; }

        /// <summary>
        /// Gets or sets customer for which order is placed.
        /// </summary>
        public CustomerModel Customer { get; set; }

        /// <summary>
        /// Gets or sets teh record version.
        /// </summary>
        public string Version { get; set; }
    }
}
