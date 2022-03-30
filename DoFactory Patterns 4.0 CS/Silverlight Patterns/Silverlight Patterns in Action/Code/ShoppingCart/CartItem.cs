using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Silverlight_Patterns_in_Action.Code
{
    /// <summary>
    /// Represents a shopping cart line item, with quantity, product, price, etc..
    /// Implemented INotifyPropertyChanged interface to facilitate databinding.
    /// </summary>
    public class CartItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Unique identifier of the product;
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the product.
        /// </summary>
        public string Name { get; set; }

        private int _quantity;

        /// <summary>
        /// Quantity of products.
        /// </summary>
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    if (PropertyChanged != null)
                    {
                        // Refresh both
                        OnPropertyChanged("Quantity");
                        OnPropertyChanged("Price");
                    }
                }
            }
        }

        /// <summary>
        /// Price per unit for product.
        /// </summary>
        public double UnitPrice { get; set; }

        /// <summary>
        /// Price for quantity of products. Total line item price.
        /// </summary>
        public double Price
        {
            get { return Quantity * UnitPrice; }
        }


        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
