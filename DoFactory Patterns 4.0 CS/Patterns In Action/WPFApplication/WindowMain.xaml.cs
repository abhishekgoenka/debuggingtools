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

using WPFModel.Provider;
using WPFModel.BusinessModelObjects;
using WPFViewModel;
using System.Windows.Media.Animation;

namespace WPFApplication
{
    /// <summary>
    /// Main window for WPF application. Shows list of customers.
    /// </summary>
    public partial class WindowMain : Window
    {
        /// <summary>
        /// The customer viewmodel.
        /// </summary>
        public CustomerViewModel ViewModel { private set; get; }

        /// <summary>
        /// Constructor for main window.
        /// </summary>
        public WindowMain()
        {
            InitializeComponent();

            // Create viewmodel and set data context.
            ViewModel = new CustomerViewModel(new Provider());
            DataContext = ViewModel;
        }

        /// <summary>
        /// Double clicking on customer rectangle opens Orders dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewOrdersCommand_Executed(sender, null);
        }

        /// <summary>
        /// Hitting Enter key also opens Orders dialog. 
        /// Hitting Del key deletes item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewOrdersCommand_Executed(sender, null);
            }
            else if (e.Key == Key.Delete)
            {
                if (ViewModel.CurrentCustomer == null)
                    MessageBox.Show("Please select a customer first");
                else
                    DeleteCommand_Executed(null, null);
            }
        }

        #region Menu Command handlers

        /// <summary>
        /// Checks if login command can execute.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !ViewModel.IsLoaded;
        }

        /// <summary>
        /// Executes login command. Opens login dialog and loads customers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var window = new WindowLogin();
            window.Owner = this; // This will center dialog in owner window

            if (window.ShowDialog() == true)
            {
                TextBlockAnnouncement.Visibility = Visibility.Collapsed;

                Cursor = Cursors.Wait;
                ViewModel.LoadCustomers();
                Cursor = Cursors.Arrow;

                CommandManager.InvalidateRequerySuggested();
            }
        }

        /// <summary>
        /// Checks if logout command can execute.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogoutCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.IsLoaded;
        }

        /// <summary>
        /// Executes logout command. Unload customers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogoutCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.UnloadCustomers();
            TextBlockAnnouncement.Visibility = Visibility.Visible;

            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Executes exit command. Shutdown application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Checks if add-customer command can execute.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.CanAdd;
        }

        /// <summary>
        /// Executes add-customer command. Opens customer dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var window = new WindowCustomer();
            window.Owner = this;
            window.IsNewCustomer = true;
            
            if (window.ShowDialog() == true)
            {
                this.CustomerListBox.ScrollIntoView(ViewModel.CurrentCustomer);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        /// <summary>
        /// Checks if edit-customer command can execute.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.CanEdit;
        }

        /// <summary>
        /// Execute edit-customer command. Opens customer dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var window = new WindowCustomer();
            window.Owner = this;
            
            if (window.ShowDialog() == true)
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }

        /// <summary>
        /// Checks if delete-customer command can execute.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.CanDelete;
        }

        /// <summary>
        /// Executes delete-customer command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.DeleteCommandModel.OnExecute(this, e);

            if (!e.Handled)
            {
                string name = ViewModel.CurrentCustomer != null ? ViewModel.CurrentCustomer.Company : "customer";
                MessageBox.Show("Cannot delete " + name + " because they have existing orders.", "Delete Customer");
            }

            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Checks if view-orders command can execute.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewOrdersCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.CanViewOrders;
        }

        /// <summary>
        /// Execute view-orders command. Opens orders dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewOrdersCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var window = new WindowOrders();
            window.Owner = this;
            
            window.ShowDialog();

            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Executes How-do-I menu command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HowDoICommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("How do I help is not implemented", "How Do I");
        }

        /// <summary>
        /// Executes help index command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IndexCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Help index is not implemented", "Index");
        }

        /// <summary>
        /// Executes about command. Opens about box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var window = new WindowAbout();
            window.Owner = this;
            window.ShowDialog();
        }

        #endregion
    }
}
