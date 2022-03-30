using System;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media.Imaging;
using System.Windows.Browser;

namespace Silverlight_Patterns_in_Action.Views
{
    /// <summary>
    /// Silverlight patterns home page.
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
        }

        // Set url to image
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var uri = new Uri(HtmlPage.Document.DocumentUri, "Assets/Images/App/clouds.jpg");
            ImageClouds.Source = new BitmapImage(uri);
        }
    }
}
