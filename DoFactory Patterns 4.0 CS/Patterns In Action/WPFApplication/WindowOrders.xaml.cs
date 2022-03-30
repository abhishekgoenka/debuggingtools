using System.Windows;
using WPFViewModel;

namespace WPFApplication
{
    /// <summary>
    /// Interaction logic for Orders window.
    /// </summary>
    public partial class WindowOrders : Window
    {
        /// <summary>
        /// WindowsOrders page's constructor.
        /// </summary>
        public WindowOrders()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Helper. Makes it easy to get to customer ViewModel.
        /// </summary>
        private CustomerViewModel CustomerViewModel
        {
            get { return (Application.Current as App).CustomerViewModel; }
        }

        // Called when window has been loaded.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Gets customer model and set data context.
            var customerModel = CustomerViewModel.CurrentCustomer;
            DataContext = customerModel;

            Title = "Orders for: " + customerModel.Company;
  
            listViewOrders.SelectedIndex = 0;
        }
    }
}
