using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Silverlight_Patterns_in_Action.ViewModels;
using System.ComponentModel.Composition;

namespace Silverlight_Patterns_in_Action.Views
{
    /// <summary>
    /// Search page.  
    /// MEF discovers and imports SearchViewModel.
    /// </summary>
    public partial class Search : Page
    {
        [Import(typeof(SearchViewModel))]
        public SearchViewModel MefSearchViewModel { get; set; }

        public Search()
        {
            InitializeComponent();

            CompositionInitializer.SatisfyImports(this);

            // Set data context of page to imported viewmodel.
            DataContext = MefSearchViewModel;

            MefSearchViewModel.AddingToCart += viewModel_AddingToCart;
            MefSearchViewModel.AddedToCartFailed += viewModel_AddedToCartFailed;
            MefSearchViewModel.AddedToCart += viewModel_AddedToCart;

            // Set focus on textbox product name
            Dispatcher.BeginInvoke(() => { textBoxProductName.Focus(); });

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
