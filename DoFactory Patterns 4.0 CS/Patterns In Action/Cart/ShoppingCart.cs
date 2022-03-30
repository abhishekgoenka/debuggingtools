using System.Collections.Generic;

namespace Cart
{
    /// <summary>
    /// A non-persistent shopping cart. It would be simple to create a
    /// shopping cart table in the database and make it persistent.
    /// </summary>
    /// <remarks>
    /// GoF Design Patterns: Strategy.
    /// Enterprise Design Patterns: Table Module, Data Table Gateway.
    /// The Strategy Pattern is the 'pluggability' of the different shipping methods. 
    /// Another patterns is the 'Lazy' computation of Total and SubTotal, depending on the IsDirty flag.
    /// (this limits the recomputation to when the actual values are needed and not when changes are made).
    /// </remarks>
    public class ShoppingCart
    {
        // The shopping cart line items
        public List<ShoppingCartItem> Items { get; private set; }

        // Totals
        private double _subTotal;
        private double _total;
        private double _shipping;

        // Pluggable shipping strategy
        private IShipping _shippingStrategy;
        private ShippingMethod _shippingMethod;

        // When cart is 'dirty' recalculations are required
        private bool _isDirty = false;

        /// <summary>
        /// Default constructor. Sets standard shipping method as Fedex.
        /// </summary>
        public ShoppingCart()
            : this(ShippingMethod.Fedex)
        {
        }

        /// <summary>
        /// Overloaded constructor for shopping cart. 
        /// </summary>
        public ShoppingCart(ShippingMethod shippingMethod)
        {
            Items = new List<ShoppingCartItem>();
            ShippingMethod = shippingMethod;
        }

        /// <summary>
        /// Adds a product item to the shopping cart. 
        /// Items may already exist; in that case just add to quantity.
        /// </summary>
        /// <param name="id">Unique product identifier.</param>
        /// <param name="name">Product name.</param>
        /// <param name="quantity">Quantity.</param>
        /// <param name="unitPrice">Unit price of product.</param>
        public void AddItem(int id, string name, int quantity, double unitPrice)
        {
            _isDirty = true;

            foreach (var item in Items)
            {
                if (item.Id == id)
                {
                    item.Quantity += quantity;
                    return;
                }
            }

            Items.Add(new ShoppingCartItem
            {
                Id = id,
                Name = name,
                Quantity = quantity,
                UnitPrice = unitPrice
            });
        }

        /// <summary>
        /// Removes a product item from the shopping cart.
        /// </summary>
        /// <param name="id">Unique product identifier.</param>
        public void RemoveItem(int id)
        {
            foreach (var item in Items)
            {
                if (item.Id == id)
                {
                    Items.Remove(item);
                    _isDirty = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Updates quantity for a given product in shopping cart.
        /// </summary>
        /// <param name="id">Unique product identifier.</param>
        /// <param name="quantity">New quantity.</param>
        public void UpdateQuantity(int id, int quantity)
        {
            foreach (var item in Items)
            {
                if (item.Id == id)
                {
                    item.Quantity = quantity;
                    _isDirty = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Lazily recalculates the total, subtotals, and shipping costs.
        /// </summary>
        public void ReCalculate()
        {
            // No need to calculate if nothing was changed
            if (!_isDirty) return;

            _subTotal = 0.0;
            _shipping = 0.0;

            foreach (var item in Items)
            {
                _subTotal += item.UnitPrice * item.Quantity;
                _shipping += _shippingStrategy.EstimateShipping(item.UnitPrice, item.Quantity);
            }

            // Add subtotal and shipping to get total
            _total = _subTotal + _shipping;

            _isDirty = false;
        }

        /// <summary>
        /// Gets subtotal for all items in the shopping cart. Recalculate if needed.
        /// </summary>
        public double SubTotal
        {
            get { ReCalculate(); return _subTotal; }
        }

        /// <summary>
        /// Gets total for all items in shopping cart. Recalculate if needed.
        /// </summary>
        public double Total
        {
            get { ReCalculate(); return _total; }
        }

        /// <summary>
        /// Gets shipping cost for all items in shopping cart. Recalculate if needed.
        /// </summary>
        public double Shipping
        {
            get { ReCalculate(); return _shipping; }
        }

        /// <summary>
        /// Gets or sets shipping method, which in turn sets the 'strategy', 
        /// i.e. the means at which products are shipped.
        /// This is the Strategy Design Pattern in action.
        /// </summary>
        public ShippingMethod ShippingMethod
        {
            get { return _shippingMethod; }

            set
            {
                _shippingMethod = value;

                switch (_shippingMethod)
                {
                    case ShippingMethod.Fedex: _shippingStrategy = new ShippingStrategyFedex(); break;
                    case ShippingMethod.UPS: _shippingStrategy = new ShippingStrategyUPS(); break;
                    default: _shippingStrategy = new ShippingStrategyUSPS(); break;
                }
                _isDirty = true;
            }
        }
    }
}

