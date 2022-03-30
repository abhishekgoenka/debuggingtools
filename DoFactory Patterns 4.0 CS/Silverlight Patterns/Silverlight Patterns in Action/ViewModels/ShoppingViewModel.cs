using System.ComponentModel;
using System.ServiceModel.DomainServices.Client;
using Silverlight_Patterns_in_Action.Web;

namespace Silverlight_Patterns_in_Action.ViewModels
{
    /// <summary>
    /// ViewModel for Shopping page.
    /// </summary>
    public class ShoppingViewModel : AddToCartViewModel
    {
        /// <summary>
        /// Constructor of ShoppingViewModel.
        /// </summary>
        public ShoppingViewModel()
            : base()
        {
            if (DesignerProperties.IsInDesignTool) return;

            Categories = Context.Categories;

            LoadCategories();
        }

        // Loads categories from database.
        public void LoadCategories()
        {
            Categories.Clear();
            Context.Load(Context.GetCategoriesQuery(), GetCategoriesQueryCallback, null);
        }

        /// <summary>
        /// GetCategoryQueries callback method.
        /// </summary>
        /// <param name="loadOperation"></param>
        public void GetCategoriesQueryCallback(LoadOperation loadOperation)
        {
            if (loadOperation.HasError)
            {
                Status = loadOperation.Error.Message;
                loadOperation.MarkErrorAsHandled();
                return;
            }

            if (loadOperation.TotalEntityCount > 0)
            {
                foreach (var category in Categories)
                {
                    CurrentCategory = category;
                    break;
                }
            }
        }

        // Loads products from database given a category.
        private void LoadProducts()
        {
            RowsFound = "";

            Products.Clear();
            int categoryId = CurrentCategory.CategoryId;
            Context.Load(Context.GetProductsByCategoryQuery(categoryId), GetProductsByCategoryQueryCallback, null);
        }

        /// <summary>
        /// GetProductsByCategory callback method.
        /// </summary>
        /// <param name="loadOperation"></param>
        public void GetProductsByCategoryQueryCallback(LoadOperation loadOperation)
        {
            if (loadOperation.HasError)
            {
                Status = loadOperation.Error.Message;
                loadOperation.MarkErrorAsHandled();
                return;
            }

            if (loadOperation.TotalEntityCount > 0)
            {
                foreach (var product in Products)
                {
                    CurrentProduct = product;
                    break;
                }
            }

            RowsFound = loadOperation.TotalEntityCount + " records";
        }


        private EntitySet<Category> _categories;

        /// <summary>
        /// Collection of Categories.
        /// </summary>
        public EntitySet<Category> Categories
        {
            get { return _categories; }
            set
            {
                if (_categories != value)
                {
                    _categories = value;
                    OnPropertyChanged("Categories");
                }
            }
        }

        private Category _currentCategory;

        /// <summary>
        /// Currently selected Category.
        /// </summary>
        public Category CurrentCategory
        {
            get { return _currentCategory; }
            set
            {
                if (_currentCategory != value)
                {
                    _currentCategory = value;
                    OnPropertyChanged("CurrentCategory");

                    // When category changes, new products are loaded.
                    LoadProducts();
                }
            }
        }
    }
}
