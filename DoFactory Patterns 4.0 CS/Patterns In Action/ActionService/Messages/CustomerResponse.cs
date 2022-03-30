using System.Collections.Generic;
using System.Runtime.Serialization;

using ActionService.MessageBase;
using ActionService.DataTransferObjects;

namespace ActionService.Messages
{
    /// <summary>
    /// Represents a customer response message to client
    /// </summary>    
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class CustomerResponse : ResponseBase
    {
        /// <summary>
        /// Default Constructor for CustomerResponse.
        /// </summary>
        public CustomerResponse() { }

        /// <summary>
        /// Overloaded Constructor for CustomerResponse. Sets CorrelationId.
        /// </summary>
        /// <param name="correlationId"></param>
        public CustomerResponse(string correlationId) : base(correlationId) { }

        /// <summary>
        /// List of customers. 
        /// </summary>
        [DataMember]
        public IList<CustomerDto> Customers;

        /// <summary>
        /// Single customer
        /// </summary>
        [DataMember]
        public CustomerDto Customer;
    }
}
