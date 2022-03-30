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
using Silverlight_Patterns_in_Action.ViewModels;

namespace Silverlight_Patterns_in_Action.Views
{
    /// <summary>
    /// Shopping Cart page.
    /// </summary>
    public partial class Cart : Page
    {
        public Cart()
        {
            InitializeComponent();
        }

        // This mimics UpdateSourceTrigger=PropertyChanged,
        // which is available in WPF but not in Silverlight. 
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                this.Focus();
                textBox.Focus();
            }

            // Simple validation. Forces range of 1-99.
            try
            {
                int quantity = int.Parse(textBox.Text);
                
                // Clamp values down if necessary
                if (quantity < 1 ) textBox.Text = "1";
                if (quantity > 99) textBox.Text = "99";
            }
            catch
            {
                // Not an integer, so set back to 1
                textBox.Text = "1";
            }
        }
    }
}
