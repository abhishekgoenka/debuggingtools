using System;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Input;
using Silverlight_Patterns_in_Action.ViewModels;

namespace Silverlight_Patterns_in_Action
{
    /// <summary>
    /// Shopping page.
    /// </summary>
    public partial class Shopping : Page
    {
        public Shopping()
        {
            InitializeComponent();

            // Get viewmodel and attach eventhandlers to viewmodel events.
            var viewModel = Resources["MyShoppingViewModel"] as ShoppingViewModel;

            viewModel.AddingToCart += viewModel_AddingToCart;
            viewModel.AddedToCartFailed += viewModel_AddedToCartFailed;
            viewModel.AddedToCart += viewModel_AddedToCart;
        }

        // About to add product to cart.
        private void viewModel_AddingToCart(object sender, ViewModelEventArgs e)
        {
            this.Cursor = Cursors.Wait;
        }

        // Successfully added product to cart. Redirect to cart page.
        private void viewModel_AddedToCart(object sender, ViewModelEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            NavigationService.Navigate(new Uri("/Cart", UriKind.Relative));
        }

        // Adding product to cart was unsuccessful.
        private void viewModel_AddedToCartFailed(object sender, ViewModelEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }
    }
}