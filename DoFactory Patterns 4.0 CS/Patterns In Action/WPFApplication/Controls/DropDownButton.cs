using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace WPFApplication.Controls
{
    /// <summary>
    /// Drop down button control.
    /// </summary>
    public class DropDownButton : ToggleButton
    {
        /// <summary>
        /// Dropdown dependency property
        /// </summary>
        public static readonly DependencyProperty DropDownProperty =
            DependencyProperty.Register("DropDown", typeof(ContextMenu),
            typeof(DropDownButton), new UIPropertyMetadata(null));

        /// <summary>
        /// Constructor.
        /// </summary>
        public DropDownButton()
        {
            // Bind the ToggleButton.IsChecked property to the drop-down's IsOpen property 
            Binding binding = new Binding("DropDown.IsOpen");
            binding.Source = this;
            this.SetBinding(IsCheckedProperty, binding);
        }


        /// <summary>
        /// Gets and sets the context menu (the dropdown).
        /// </summary>
        public ContextMenu DropDown
        {
            get { return (ContextMenu)GetValue(DropDownProperty); }
            set { SetValue(DropDownProperty, value); }
        }


        /// <summary>
        /// Overridden OnClick. Opens the dropdown.
        /// </summary>
        protected override void OnClick()
        {
            if (DropDown == null) return;

            // Position and display dropdown
            DropDown.PlacementTarget = this;
            DropDown.Placement = PlacementMode.Bottom;

            DropDown.IsOpen = true;
        }
    }
}
