using System;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Text;
using Silverlight_Patterns_in_Action.Code;


namespace Silverlight_Patterns_in_Action.ViewModels
{
    /// <summary>
    /// Viewmodel for (shopping) Cart page.
    /// </summary>
    public class CartViewModel : ViewModelBase
    {
        // Observable collection (extended version) of cart items
        public ObservableCollectionEx<CartItem> CartItems { get; private set; }

        // Commanding
        private readonly RelayCommand _removeCommand;
        public ICommand RemoveCommand { get { return _removeCommand; } }

        /// <summary>
        /// Constructor for CartViewModel.
        /// </summary>
        public CartViewModel()
        {
            // In design mode, simply return.
            if (DesignerProperties.IsInDesignTool) return;

            CartItems = new ObservableCollectionEx<CartItem>();
            CartItems.CollectionChanged += CartItems_CollectionChanged;
            ((INotifyPropertyChanged)CartItems).PropertyChanged += CartItems_PropertyChanged;

            _removeCommand = new RelayCommand(OnRemove);
            _removeCommand.IsEnabled = true;

            LoadCartFromIsolatedStorage();

            if (CartItems.Count > 0)
                CurrentItem = CartItems[0];
        }

        void CartItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Recalculate();
        }

        void CartItems_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Quantity")
                Recalculate();
        }

        private void OnRemove(object item)
        {
            CartItems.Remove(item as CartItem);
        }

        /// <summary>
        /// Add an product item to the shopping cart.
        /// </summary>
        /// <param name="newItem">The new cart item to be added.</param>
        public void AddItem(CartItem newItem)
        {
            // Check that item not already exists in shopping cart.
            foreach (var cartItem in CartItems)
            {
                if (cartItem.Id == newItem.Id)
                {
                    cartItem.Quantity += newItem.Quantity;
                    return;
                }
            }

            // A new item is added to shopping cart.
            CartItems.Add(newItem);

            CurrentItem = newItem;
        }

        // Recalculates total, subtotal, shipping for entire cart.
        private void Recalculate()
        {
            double subTotal = 0.0;
            double shipping = 0.0;

            foreach (var item in CartItems)
            {
                subTotal += item.UnitPrice * item.Quantity;
                shipping += _shippingStrategy.EstimateShipping(item.UnitPrice, item.Quantity);
            }

            // These settings fire the PropertyChange events
            SubTotal = subTotal;
            Shipping = shipping;
            Total = subTotal + shipping;

            // Immediately persist to iso storage.
            SaveCartToIsolatedStorage();
        }

        private double _total;

        /// <summary>
        /// The grand total of the shopping cart.
        /// </summary>
        public double Total
        {
            get { return _total; }
            set
            {
                if (_total != value)
                {
                    _total = value;
                    OnPropertyChanged("Total");

                    CurrentCartState = "Normal";
                    CurrentCartState = "Changed";
                }
            }
        }

        private double _subTotal;

        /// <summary>
        /// The subtotal of the shopping cart items.
        /// </summary>
        public double SubTotal
        {
            get { return _subTotal; }
            set
            {
                if (_subTotal != value)
                {
                    _subTotal = value;
                    OnPropertyChanged("SubTotal");
                }
            }
        }

        private double _shipping;

        /// <summary>
        /// Shipping costs of all items in the cart.
        /// </summary>
        public double Shipping
        {
            get { return _shipping; }
            set
            {
                if (_shipping != value)
                {
                    _shipping = value;
                    OnPropertyChanged("Shipping");
                }
            }
        }

        private IShippingStrategy _shippingStrategy = new ShippingStrategyFedex();
        private int _shippingId = 1;

        /// <summary>
        /// Gets or sets shipping 'strategy', i.e. how products are shipped.
        /// This is the Strategy Design Pattern in action.
        /// </summary>
        public int ShippingId
        {
            get { return _shippingId; }
            set
            {
                if (_shippingId != value)
                {
                    _shippingId = value;
                    switch (_shippingId)
                    {
                        case (int)ShippingMethod.Fedex: _shippingStrategy = new ShippingStrategyFedex(); break;
                        case (int)ShippingMethod.UPS: _shippingStrategy = new ShippingStrategyUPS(); break;
                        default: _shippingStrategy = new ShippingStrategyUSPS(); break;
                    }

                    Recalculate();
                }
            }
        }

        private CartItem _currentItem;

        /// <summary>
        /// Currently selected item in the shopping cart.
        /// </summary>
        public CartItem CurrentItem
        {
            get { return _currentItem; }
            set
            {
                if (_currentItem != value)
                {
                    _currentItem = value;
                    OnPropertyChanged("CurrentItem");
                }
            }
        }

        private string _currentCartState; 

        /// <summary>
        /// Current ViewState as used in ViewState Manager
        /// </summary>
        public string CurrentCartState
        {
            get { return _currentCartState; }
            set
            {
                if (value != _currentCartState)
                {
                    _currentCartState = value;
                    OnPropertyChanged("CurrentCartState");
                }
            }
        }

        #region Isolated Storage Persistance

        private static readonly string cartPath = "cart.txt";

        // Load cart from isolated storage.
        // Simple deserialization from disk.
        private void LoadCartFromIsolatedStorage()
        {
            var data = IsolatedStoreHelper.LoadData(cartPath);
            if (string.IsNullOrEmpty(data)) return;

            string[] tokens = data.Split(';');
            ShippingId = int.Parse(tokens[0]);
            int count = int.Parse(tokens[1]);

            for (int i = 0; i < count; i++)
            {
                int index = 2 + (i * 4);
                CartItems.Add(new CartItem
                {
                    Id = int.Parse(tokens[index + 0]),
                    Name = tokens[index + 1],
                    Quantity = int.Parse(tokens[index + 2]),
                    UnitPrice = double.Parse(tokens[index + 3])
                });
            }


        }

        // Saves shopping cart to isolated storage.
        // Basically serializes cart to disk.
        private void SaveCartToIsolatedStorage()
        {
            var data = new StringBuilder();
            data.Append(ShippingId + ";");
            data.Append(CartItems.Count + ";");

            foreach (var item in CartItems)
            {
                data.Append(item.Id + ";");
                data.Append(item.Name + ";");
                data.Append(item.Quantity + ";");
                data.Append(item.UnitPrice + ";");
            }

            IsolatedStoreHelper.SaveData(data.ToString(), cartPath);
        }

        #endregion
    }
}
