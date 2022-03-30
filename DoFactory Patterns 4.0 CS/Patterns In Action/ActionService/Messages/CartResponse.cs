using System.Runtime.Serialization;
using ActionService.MessageBase;
using ActionService.DataTransferObjects;

namespace ActionService.Messages
{
    /// <summary>
    /// Represents a shopping cart message response to client.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class CartResponse : ResponseBase
    {
        /// <summary>
        /// Default Constructor for CartResponse.
        /// </summary>
        public CartResponse() { }

        /// <summary>
        /// Overloaded Constructor for CartResponse. Sets CorrelationId.
        /// </summary>
        /// <param name="correlationId"></param>
        public CartResponse(string correlationId) : base(correlationId) { }

        /// <summary>
        /// The shopping cart.
        /// </summary>
        [DataMember]
        public ShoppingCartDto Cart;
    }
}