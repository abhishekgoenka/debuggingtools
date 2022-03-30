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

namespace Silverlight_Patterns_in_Action.ViewModels
{
    /// <summary>
    /// Attached property class. Holds VisualState Attached Property.
    /// </summary>
    public  class ShoppingCartStates : DependencyObject
    {
        /// <summary>
        /// Visual State Attached Property definition
        /// </summary>
        public static readonly DependencyProperty VisualStateProperty = DependencyProperty.RegisterAttached(
            "VisualState",
            typeof(string),
            typeof(ShoppingCartStates),
            new PropertyMetadata(OnVisualStateChanged));

        /// <summary>
        /// Callback. Sets state according to value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnVisualStateChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as Control; // the view
            if (control != null)
                VisualStateManager.GoToState(control, (string)e.NewValue, true);
            else
                throw new InvalidOperationException("VisualState is only supported on type Control and its descendents");
        }

        /// <summary>
        /// Gets VisualState attached property.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string GetVisualState(DependencyObject d)
        {
            return (string)d.GetValue(VisualStateProperty);
        }

        /// <summary>
        /// Sets VisualState attached property.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static void SetVisualState(DependencyObject d, string value)
        {
            d.SetValue(VisualStateProperty, value);
        }
    }
}
