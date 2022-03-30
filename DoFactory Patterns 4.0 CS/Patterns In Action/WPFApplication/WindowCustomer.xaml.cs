using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using WPFModel.BusinessModelObjects;
using WPFViewModel;
using WPFApplication.Converters;

namespace WPFApplication
{
    /// <summary>
    /// Customer details edit window.
    /// </summary>
    public partial class WindowCustomer : Window
    {
        /// <summary>
        /// Flag indicating new customer or not.
        /// </summary>
        public bool IsNewCustomer { get; set; }

        private string _originalCompany;
        private string _originalCity;
        private string _originalCountry;

        /// <summary>
        /// Customer window constructor.
        /// </summary>
        public WindowCustomer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Helper. Makes it easy to get to customer ViewModel.
        /// </summary>
        private CustomerViewModel CustomerViewModel
        {
            get { return (Application.Current as App).CustomerViewModel; }
        }

        /// <summary>
        /// Loads new or existing record.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CommandModel commandModel;

            // New customer.
            if (IsNewCustomer)
            {

                this.DataContext = CustomerViewModel.NewCustomerModel;
                Title = "Add new customer";

                commandModel = CustomerViewModel.AddCommandModel;

                // Display little hint message
                LabelNewMessage1.Visibility = Visibility.Visible;
                LabelNewMessage2.Visibility = Visibility.Visible;
            }
            else
            {
                this.DataContext = CustomerViewModel.CurrentCustomer;

                // Save off original values. Due to binding viewmodel is changed immediately when editing.
                // So, when canceling we have these values to restore original state.
                // Suggestion: could be implemented as Memento pattern.
                _originalCompany = CustomerViewModel.CurrentCustomer.Company;
                _originalCity = CustomerViewModel.CurrentCustomer.City;
                _originalCountry = CustomerViewModel.CurrentCustomer.Country;

                Title = "Edit customer";

                commandModel = CustomerViewModel.EditCommandModel;
            }

            textBoxCustomer.Focus();

            // The command helps determine whether save button is enabled or not
            buttonSave.Command = commandModel.Command;
            buttonSave.CommandParameter = this.DataContext;
            buttonSave.CommandBindings.Add(new CommandBinding(commandModel.Command, commandModel.OnExecute, commandModel.OnCanExecute));
        }

        // Save button was clicked
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }

        // Cancel button was clicked
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            // Restore viewmodel to original values
            if (!IsNewCustomer)
            {
                CustomerViewModel.CurrentCustomer.Company = _originalCompany;
                CustomerViewModel.CurrentCustomer.City = _originalCity;
                CustomerViewModel.CurrentCustomer.Country = _originalCountry;
            }
        }
    }
}
