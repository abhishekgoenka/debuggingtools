using System.Runtime.Serialization;
using ActionService.MessageBase;
using ActionService.DataTransferObjects;

namespace ActionService.Messages
{
    /// <summary>
    /// Represents a shopping cart request message from client.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class CartRequest : RequestBase
    {
        /// <summary>
        /// Shopping cart item.
        /// </summary>
        [DataMember]
        public ShoppingCartItemDto CartItem; 

        /// <summary>
        /// Shipping method.
        /// </summary>
        [DataMember]
        public string ShippingMethod; 
    }
}