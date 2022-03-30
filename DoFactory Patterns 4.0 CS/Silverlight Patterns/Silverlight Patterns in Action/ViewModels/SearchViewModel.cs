using System.Windows.Input;
using Silverlight_Patterns_in_Action.Code.Pricing;
using System.ServiceModel.DomainServices.Client;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace Silverlight_Patterns_in_Action.ViewModels
{
    /// <summary>
    /// ViewModel for the Search page.
    /// </summary>
    /// <remarks>
    /// Try changing CreationPolicy.NonShared to Shared and see this difference. 
    /// </remarks>
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(SearchViewModel))]
    public class SearchViewModel : AddToCartViewModel 
    {
        private readonly RelayCommand _searchCommand;
        private readonly RelayCommand _resetCommand;

        /// <summary>
        /// The Search command.
        /// </summary>
        public ICommand SearchCommand { get { return _searchCommand; } }

        /// <summary>
        /// The Reset command (resets search).
        /// </summary>
        public ICommand ResetCommand { get { return _resetCommand; } }

        /// <summary>
        /// Constructor for SearchViewModel.
        /// </summary>
        public SearchViewModel()
            : base()
        {
            // In design mode, just return
            if (DesignerProperties.IsInDesignTool) return;

            _searchCommand = new RelayCommand(OnSearch);
            _searchCommand.IsEnabled = true;

            _resetCommand = new RelayCommand(OnReset);
            _resetCommand.IsEnabled = false;

            // Init price range dropdown options.
            PriceRanges = PriceRange.List;
            CurrentPriceRange = PriceRanges[0];
        }

        private void OnSearch(object parameter)
        {
            Status = "";
            RowsFound = "";

            Products.Clear();

            string productName = ProductName == "" ? null : ProductName;

            double? priceFrom = null;
            double? priceThru = null;

            if (_currentPriceRange.RangeId > 0)
            {
                priceFrom = CurrentPriceRange.RangeFrom;
                priceThru = CurrentPriceRange.RangeThru;
            }

            // Searches for products, given criteria
            Context.Load(Context.FindProductsQuery(productName, priceFrom, priceThru), FindProductsQueryCallback, true);
        }

        private void FindProductsQueryCallback(LoadOperation loadOperation)
        {
            if (loadOperation.HasError)
            {
                Status = loadOperation.Error.Message;
                loadOperation.MarkErrorAsHandled();
                return;
            }

            if (loadOperation.TotalEntityCount > 0)
            {
                foreach (var product in Products)
                {
                    CurrentProduct = product;
                    break;
                }
            }

            RowsFound = loadOperation.TotalEntityCount + " records";

            _resetCommand.IsEnabled = true;
        }

        private void OnReset(object parameter)
        {
            // Reset page (search criteria & statuses)
            Status = "";
            RowsFound = "";
            Quantity = "1";

            ProductName = "";
            CurrentPriceRange = PriceRanges[0];

            Products.Clear();
            CurrentProduct = null;

            _resetCommand.IsEnabled = false;
        }

        private string _productName;
        
        /// <summary>
        /// The searched productname.
        /// </summary>
        public string ProductName
        {
            get { return _productName; }
            set
            {
                if (_productName != value)
                {
                    _productName = value;
                    OnPropertyChanged("ProductName");
                }
            }
        }

        private List<PriceRangeItem> _priceRanges;

        /// <summary>
        /// List of price ranges.
        /// </summary>
        public List<PriceRangeItem> PriceRanges
        {
            get { return _priceRanges; }
            set
            {
                if (_priceRanges != value)
                {
                    _priceRanges = value;
                    OnPropertyChanged("PriceRanges");
                }
            }
        }


        private PriceRangeItem _currentPriceRange;

        /// <summary>
        /// Currently selected pricerange
        /// </summary>
        public PriceRangeItem CurrentPriceRange
        {
            get { return _currentPriceRange; }
            set
            {
                if (_currentPriceRange != value)
                {
                    _currentPriceRange = value;
                    OnPropertyChanged("CurrentPriceRange");
                }
            }
        }
    }
}
