using System.Runtime.Serialization;

namespace ActionService.Criteria
{
    /// <summary>
    /// Holds criteria for customer queries.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class CustomerCriteria : Criteria
    {
        /// <summary>
        /// Unique customer identifier.
        /// </summary>
        [DataMember]
        public int CustomerId { get; set; }

        /// <summary>
        /// Flag as to whether to include order statistics.
        /// </summary>
        [DataMember]
        public bool IncludeOrderStatistics { get; set; }
    }
}
