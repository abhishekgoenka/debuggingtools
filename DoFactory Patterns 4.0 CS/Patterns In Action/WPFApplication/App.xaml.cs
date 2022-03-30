using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using WPFViewModel;

namespace WPFApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Gets customerview model from MainWindow.
        /// </summary>
        public CustomerViewModel CustomerViewModel
        {
            get
            {
                var window = MainWindow as WindowMain;
                return window.ViewModel;
            }
        }
    }
}
