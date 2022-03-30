using System;
using ASPNETWebApplication.Repositories;
using System.Web.Security;

namespace ASPNETWebApplication.WebAuth
{
    public partial class Logout : PageBase
    {
        /// <summary>
        /// Performs logout operation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var repository = new AuthRepository();
            repository.Logout();

            FormsAuthentication.SignOut();
        }
    }
}