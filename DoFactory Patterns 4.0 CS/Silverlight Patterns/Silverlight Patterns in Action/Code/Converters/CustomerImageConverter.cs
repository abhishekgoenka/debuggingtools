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
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Browser;

namespace Silverlight_Patterns_in_Action.Converters
{
    /// <summary>
    /// Converts a customerId into the appropriate customers's bitmap image.
    /// </summary>
    public class CustomerImageConverter : IValueConverter
    {
        /// <summary>
        /// Takes a customerId and converts it to a customer bitmap.
        /// </summary>
        /// <param name="value">The customer identifier.</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">Size of image: small or large.</param>
        /// <param name="culture"></param>
        /// <returns>Customer bitmap image.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string size = (string)parameter;
            
            int id = int.Parse(value.ToString());
            if (id > 91) id = 0; // New customers are getting the default silhouette icon.

            var uri = new Uri(HtmlPage.Document.DocumentUri, "Assets/Images/Customers/" + size + "/" + id + ".jpg");
            return new BitmapImage(uri);
        }
        
        // Not implemented.
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
