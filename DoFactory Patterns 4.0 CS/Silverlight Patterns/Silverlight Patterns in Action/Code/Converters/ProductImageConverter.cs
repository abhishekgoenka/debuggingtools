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
    public class ProductImageConverter : IValueConverter
    {
        /// <summary>
        /// Takes a productId and converts it to a product bitmap.
        /// </summary>
        /// <param name="value">The product identifier.</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>Product bitmap image.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var uri = new Uri(HtmlPage.Document.DocumentUri, "Assets/Images/Products/" + value.ToString() + ".jpg");
            return new BitmapImage(uri);
        }

        // Not implemented.
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
