using System.Runtime.Serialization;

namespace ActionService.Criteria
{
    /// <summary>
    /// Holds criteria for order queries.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class OrderCriteria : Criteria
    {
        /// <summary>
        /// Unique order identifier.
        /// </summary>
        [DataMember]
        public int OrderId { get; set; }

        /// <summary>
        /// Unique customer identifier.
        /// </summary>
        [DataMember]
        public int CustomerId { get; set; }
    }
}