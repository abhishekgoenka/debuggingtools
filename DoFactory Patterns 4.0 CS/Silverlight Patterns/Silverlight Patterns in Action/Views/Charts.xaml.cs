using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel;

namespace Silverlight_Patterns_in_Action.Views
{
    /// <summary>
    /// Holds plugin chart controls in a tab control.
    /// Access requires authentication.
    /// </summary>
    public partial class Charts : Page, IPartImportsSatisfiedNotification
    {
        /// <summary>
        /// The plugged in charts (discovered and imported by MEF).
        /// </summary>
        [ImportMany]
        public UserControl[] PluginCharts { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Charts()
        {
            InitializeComponent();

            // Resolves extensions for this page.
            CompositionInitializer.SatisfyImports(this);
        }

        /// <summary>
        /// Once importing is done, we add chart user controls to tab.
        /// </summary>
        public void OnImportsSatisfied()
        {
            tabCharts.Items.Clear();

            foreach (var chart in PluginCharts)
            {
                chart.Margin = new Thickness(20);

                var item = new TabItem();
                item.Header = chart.Tag;
                item.Content = chart;

                tabCharts.Items.Add(item);
            }
        }
    }
}
