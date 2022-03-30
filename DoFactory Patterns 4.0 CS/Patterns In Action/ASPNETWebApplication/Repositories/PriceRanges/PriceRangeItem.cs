
namespace ASPNETWebApplication.Repositories
{
    /// <summary>
    /// A PriceRange item used in the PriceRange list.  PriceRanges are used for 
    /// searching the product catalog. 
    /// </summary>
    public class PriceRangeItem
    {
        /// <summary>
        /// Constructor for PriceRangeItem.
        /// </summary>
        /// <param name="rangeId">Unique identifier for the price range.</param>
        /// <param name="rangeFrom">Lower end of the price range.</param>
        /// <param name="rangeThru">Higher end of the price range.</param>
        /// <param name="rangeText">Easy-to-read form of the price range.</param>
        public PriceRangeItem(int rangeId, double rangeFrom, double rangeThru, string rangeText)
        {
            RangeId = rangeId;
            RangeFrom = rangeFrom;
            RangeThru = rangeThru;
            RangeText = rangeText;
        }

        /// <summary>
        /// Gets the unique PriceRange identifier.
        /// </summary>
        public int RangeId { get; private set; }

        /// <summary>
        /// Gets the low end of the PriceRange item.
        /// </summary>
        public double RangeFrom { get; private set; }

        /// <summary>
        /// Gets the high end of the PriceRange item.
        /// </summary>
        public double RangeThru { get; private set; }

        /// <summary>
        /// Gets an easy-to-read form of the PriceRange item.
        /// </summary>
        public string RangeText { get; private set; }
    }
}