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
using System.ComponentModel;
using System.Collections.Generic;
using Silverlight_Contracts;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Collections.ObjectModel;

namespace Silverlight_Charts.ViewModels
{
    /// <summary>
    /// ViewModel for Pie Chart Control. 
    /// Note: Implements MEFs IPartImportSatisfiedNotification.
    /// </summary>
    public class PieChartViewModel : IPartImportsSatisfiedNotification
    {
        [Import(typeof(IContext))]
        public IContext Context { get; set; }

        public PieChartViewModel()
        {
            // In design mode, simply return.
            if (DesignerProperties.IsInDesignTool) return;

            ChartData = new ObservableCollection<OrderStatistics>();

            // MEF: Compose this control
            CompositionInitializer.SatisfyImports(this);
        }

        /// <summary>
        /// Data for pie chart
        /// </summary>
        public ObservableCollection<OrderStatistics> ChartData { get; set; }

        /// <summary>
        /// Is called when MEF part resolution is satisfied. 
        /// </summary>
        public void OnImportsSatisfied()
        {
            // Asynchronous call. Get statistics for 2004.
            int year = 2004;
            Context.GetOrderStatistics(ImportStatisticsCallback, year);
        }

        /// <summary>
        /// Callback. Called when statistics data is retrieved.
        /// </summary>
        /// <param name="list"></param>
        public void ImportStatisticsCallback(List<OrderStatistics> list)
        {
            foreach (var item in list)
                ChartData.Add(item);
        }
    } 
}
