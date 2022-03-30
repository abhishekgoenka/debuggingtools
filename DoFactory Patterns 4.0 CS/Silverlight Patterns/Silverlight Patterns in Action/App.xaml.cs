using System.Windows;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System;
using System.ComponentModel;
using System.Diagnostics;
using Silverlight_Patterns_in_Action.ViewModels;

namespace Silverlight_Patterns_in_Action
{
    /// <summary>
    /// Main Application class for Silverlight Patterns in Action.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Constructor for App class.
        /// </summary>
        public App()
        {
            InitializeComponent();

            // Create a WebContext and add it to the ApplicationLifetimeObjects collection.
            // This will then be available as 'WebContext.Current'.
            var webContext = new WebContext();
            webContext.Authentication = new FormsAuthentication();
            ApplicationLifetimeObjects.Add(webContext);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // This enables control binding XAML files to WebContext.Current properties. Not currently used.
            this.Resources.Add("WebContext", WebContext.Current);

            // Init MEF container.
            InitializeContainer();

            this.RootVisual = new MainPage();

            // Get database loaded (to speed up initial shopping page).
            BootstrapDb();
        }

        /// <summary>
        /// Initialize MEF Container and load specified plugin xap file.
        /// </summary>
        private void InitializeContainer()
        {
            var catalog = new AggregateCatalog();

            // Add this assembly to list of catalogs
            catalog.Catalogs.Add(new DeploymentCatalog());  

            // Add charting assembly to list of catalogs
            var uri = new Uri("SilverlightCharts.xap", UriKind.Relative);
            var chartCatalog = new DeploymentCatalog(uri);
            chartCatalog.DownloadCompleted += catalog_DownloadCompleted;
            catalog.Catalogs.Add(chartCatalog);

            // Perform part composition.
            CompositionHost.Initialize(catalog);

            // Asynchronously download charts imports
            chartCatalog.DownloadAsync();
        }

        void catalog_DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
                ErrorWindow.CreateNew(e.Error);
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If app is running outside of the debugger then report the exception using the error window.
            // Note: in production you should log the error and handle the exception appropriatedly.
            if (!Debugger.IsAttached)
            {
                e.Handled = true;
                ErrorWindow.CreateNew(e.ExceptionObject);
            }
        }

        private void BootstrapDb()
        {
            // Initial call to get the database loaded.
            // This will render initial shopping pages much faster.
            new ShoppingViewModel().LoadCategories();
        }
    }
}