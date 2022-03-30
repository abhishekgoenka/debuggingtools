using System.Collections.Generic;
using System.Runtime.Serialization;

using ActionService.MessageBase;
using ActionService.DataTransferObjects;

namespace ActionService.Messages
{
    /// <summary>
    /// Represents a product response message from web service to client.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class ProductResponse : ResponseBase
    {
        /// <summary>
        /// Default Constructor for ProductResponse.
        /// </summary>
        public ProductResponse() { }

        /// <summary>
        /// Overloaded Constructor for ProductResponse. Sets CorrelationId.
        /// </summary>
        /// <param name="correlationId"></param>
        public ProductResponse(string correlationId) : base(correlationId) { }

        /// <summary>
        /// List of categories.
        /// </summary>
        [DataMember]
        public IList<CategoryDto> Categories; 

        /// <summary>
        /// List of products.
        /// </summary>
        [DataMember]
        public IList<ProductDto> Products; 

        /// <summary>
        /// Single product.
        /// </summary>
        [DataMember]
        public ProductDto Product;
    }
}
