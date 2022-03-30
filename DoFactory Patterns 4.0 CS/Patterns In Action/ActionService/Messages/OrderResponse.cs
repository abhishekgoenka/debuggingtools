using System.Runtime.Serialization;

using ActionService.MessageBase;
using ActionService.DataTransferObjects;

namespace ActionService.Messages
{
    /// <summary>
    /// Respresents a response message with a list of orders for a given customer.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class OrderResponse : ResponseBase
    {
        /// <summary>
        /// Default Constructor for OrderResponse.
        /// </summary>
        public OrderResponse() { }

        /// <summary>
        /// Overloaded Constructor for OrderResponse. Sets CorrelationId.
        /// </summary>
        /// <param name="correlationId"></param>
        public OrderResponse(string correlationId) : base(correlationId) { }

        /// <summary>
        /// List of orders for a given customer.
        /// </summary>
        [DataMember]
        public OrderDto[] Orders;

        /// <summary>
        /// Single order.
        /// </summary>
        [DataMember]
        public OrderDto Order;
    }
}
