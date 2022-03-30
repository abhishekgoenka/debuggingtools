using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WPFApplication.Converters
{
    /// <summary>
    /// Customer Image converter utility. Converts a customer id to an image path.
    /// </summary>
    public class CustomerImageConverter : IValueConverter
    {
        /// <summary>
        /// Converts customerId into path of customer image.
        /// </summary>
        /// <returns>Image path.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string size = (string)parameter;

            int id = int.Parse(value.ToString());
            if (id > 91) id = 0; // New customers are getting the default silhouette icon.

            return "Images/Customers/" + size + "/" + id + ".jpg";
        }           

        /// <summary>
        /// ConvertBack is not implemented.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
