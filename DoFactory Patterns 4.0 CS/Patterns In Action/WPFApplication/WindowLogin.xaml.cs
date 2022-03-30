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

namespace WPFApplication
{
    /// <summary>
    /// Login Window.
    /// </summary>
    public partial class WindowLogin : Window
    {
        /// <summary>
        /// Constructor for login window.
        /// </summary>
        public WindowLogin()
        {
            InitializeComponent();
        }

        // Link was clicked for credential info.
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("For demonstration purposes please use\nUserName: debbie\nPassword: secret123", "Login credentials");
        }

        // OK buttons was clicked.
        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            string username = usernameBox.Text.Trim();
            string password = passwordBox.Password.Trim();

            try
            {
                var provider = new Provider();
                provider.Login(username, password);

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Arrow;
                MessageBox.Show(ex.Message + " Please try again.", "Login failed");
            }
        }
    }
}
