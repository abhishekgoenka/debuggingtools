using System.Runtime.Serialization;

namespace ActionService.Criteria
{
    /// <summary>
    /// Holds criteria for product queries.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class ProductCriteria : Criteria
    {
        /// <summary>
        /// Unique category identifier.
        /// </summary>
        [DataMember]
        public int CategoryId { get; set; }

        /// <summary>
        /// Unique product identifier.
        /// </summary>
        [DataMember]
        public int ProductId { get; set; }

        /// <summary>
        /// The product name.
        /// </summary>
        [DataMember]
        public string ProductName { get; set; }

        /// <summary>
        /// Price range low end. 
        /// </summary>
        [DataMember]
        public double PriceFrom { get; set; }

        /// <summary>
        /// Price range high end.
        /// </summary>
        [DataMember]
        public double PriceThru { get; set; }
    }
}