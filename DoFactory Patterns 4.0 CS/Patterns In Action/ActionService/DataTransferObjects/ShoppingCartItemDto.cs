using System.Runtime.Serialization;

namespace ActionService.DataTransferObjects
{
    /// <summary>
    /// Shopping Cart Item Data Transfer Object.
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
    [DataContract(Name = "ShoppingCartItem", Namespace = "http://www.yourcompany.com/types/")]
    public class ShoppingCartItemDto
    {
        /// <summary>
        /// Unique shopping cart item identifier.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Shopping cart item product name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Quantity of items in shopping cart line item.
        /// </summary>
        [DataMember]
        public int Quantity { get; set; }

        /// <summary>
        /// Unit price of shopping cart line item.
        /// </summary>
        [DataMember]
        public double UnitPrice { get; set; }
    }
}