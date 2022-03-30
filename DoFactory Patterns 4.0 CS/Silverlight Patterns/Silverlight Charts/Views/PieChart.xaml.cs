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
using System.ComponentModel.Composition;
using Silverlight_Contracts;

namespace Silverlight_Charts.Views
{
    /// <summary>
    /// Pie chart is exportable. Not shared.
    /// </summary>
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(UserControl))]
    public partial class PieChart : UserControl
    {
        public PieChart()
        {
            InitializeComponent();
        }
    }
}
