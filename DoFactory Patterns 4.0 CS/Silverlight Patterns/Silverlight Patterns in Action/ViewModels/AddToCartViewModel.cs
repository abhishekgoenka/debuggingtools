using System;
using System.Windows;
using System.Windows.Input;
using Silverlight_Patterns_in_Action.Web;
using System.ComponentModel;
using System.ServiceModel.DomainServices.Client;
using Silverlight_Patterns_in_Action.Code;
using Silverlight_Patterns_in_Action.Web.Services.Web;

namespace Silverlight_Patterns_in_Action.ViewModels
{
    /// <summary>
    /// Abstact ViewModel that provides AddtoCart functionality. 
    /// Inherited by two page-specific ViewModels (Search and Shopping).
    /// </summary>
    public abstract class AddToCartViewModel : ViewModelBase 
    {
        protected ActionDomainContext Context;

        /// <summary>
        /// Fires when product is added to cart.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> AddingToCart;

        /// <summary>
        /// Fires when product has been added successfully to cart.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> AddedToCart;

        /// <summary>
        /// Fires when adding to cart failed.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> AddedToCartFailed;

        private readonly RelayCommand _addtocartCommand;
        /// <summary>
        /// Add to Cart command.
        /// </summary>
        public ICommand AddToCartCommand { get { return _addtocartCommand; } }
       

        /// <summary>
        /// Constructor of AddToCart ViewModel.
        /// </summary>
        public AddToCartViewModel()
        {
            if (DesignerProperties.IsInDesignTool) return;

            _addtocartCommand = new RelayCommand(OnAddToCart);
            _addtocartCommand.IsEnabled = true;

            Context = new ActionDomainContext();
            Products = Context.Products;
        }

        private void OnAddToCart(object parameter)
        {
            Status = "";

            RaiseEvent(AddingToCart);

            // Validate data type and quantity limits.
            int quantity;
            if (!int.TryParse(Quantity, out quantity))
            {
                Status = "Quantity must be numeric";
                RaiseEvent(AddedToCartFailed);
                return;
            }

            if (quantity < 1 || quantity > 99)
            {
                Status = "Quantity must be between 1 and 99";
                RaiseEvent(AddedToCartFailed);
                return;
            }

            if (CurrentProduct == null)
            {
                Status = "Please select a product";
                RaiseEvent(AddedToCartFailed);
                return;
            }

            var newItem = new CartItem { Id = CurrentProduct.ProductId, Name = CurrentProduct.ProductName, Quantity = quantity, UnitPrice = (double)CurrentProduct.UnitPrice };
            var cartViewModel = (CartViewModel)Application.Current.Resources["MyCartViewModel"];
            cartViewModel.AddItem(newItem);

            RaiseEvent(AddedToCart);
        }

        private EntitySet<Product> _products;

        /// <summary>
        /// List of products that can be viewed.
        /// </summary>
        public EntitySet<Product> Products
        {
            get { return _products; }
            set
            {
                if (_products != value)
                {
                    _products = value;
                    OnPropertyChanged("Products");
                }
            }
        }

        private Product _currentProduct;

        /// <summary>
        /// Currently selected product.
        /// </summary>
        public Product CurrentProduct
        {
            get { return _currentProduct; }
            set
            {
                if (_currentProduct != value)
                {
                    _currentProduct = value;
                    OnPropertyChanged("CurrentProduct");
                }
            }
        }

        private string _quantity = "1";

        /// <summary>
        /// Quantity to be added to shopping cart.
        /// </summary>
        public string Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }

        private string _status;

        /// <summary>
        /// Textual shopping cart status that can be displayed on View.
        /// </summary>
        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        private string _rowsFound;

        /// <summary>
        /// Number of rows in current product list.
        /// </summary>
        public string RowsFound
        {
            get { return _rowsFound; }
            set
            {
                if (_rowsFound != value)
                {
                    _rowsFound = value;
                    OnPropertyChanged("RowsFound");
                }
            }
        }
    }
}
