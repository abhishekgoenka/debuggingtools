using System.Runtime.Serialization;

namespace ActionService.DataTransferObjects
{
    /// <summary>
    /// Shopping Cart Data Transfer Object.
    /// 
    /// The purpose of the CustomerTransferObject is to facilitate transport of 
    /// customer business data in a serializable format. Business data is kept in 
    /// publicly accessible auto property members. This class has no methods. 
    /// </summary>
    /// <remarks>
    /// Pattern: Data Transfer Objects.
    /// 
    /// Data Transfer Objects are objects that transfer data between processes, but without behavior.
    /// </remarks>
    [DataContract(Name = "ShoppingCart", Namespace = "http://www.yourcompany.com/types/")]
    public class ShoppingCartDto
    {
        /// <summary>
        /// Cost of shipping.
        /// </summary>
        [DataMember]
        public double Shipping { get; set; }

        /// <summary>
        /// Shopping cart subtotal.
        /// </summary>
        [DataMember]
        public double SubTotal { get; set; }

        /// <summary>
        /// Shopping cart total.
        /// </summary>
        [DataMember]
        public double Total { get; set; }

        /// <summary>
        /// Shopping carts selected shipping method.
        /// </summary>
        [DataMember]
        public string ShippingMethod { get; set; }

        /// <summary>
        /// Shopping cart item.
        /// </summary>
        [DataMember]
        public ShoppingCartItemDto[] CartItems { get; set; }
    }
}

