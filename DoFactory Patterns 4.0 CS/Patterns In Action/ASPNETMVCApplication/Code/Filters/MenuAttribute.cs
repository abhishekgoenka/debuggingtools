using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPNETMVCApplication.Code.Filters
{
    /// <summary>
    /// Custom ActionFilterAttribute, used to set Selected Menu items.
    /// </summary>
    public class MenuAttribute : ActionFilterAttribute
    {
        private MenuItem _selectedMenu;

        /// <summary>
        /// Constructor of MenuAttribute.
        /// Requires selected menu item enumerated value.
        /// </summary>
        /// <param name="selectedMenu">Selected menu item.</param>
        public MenuAttribute(MenuItem selectedMenu)
        {
            this._selectedMenu = selectedMenu;
        }

        /// <summary>
        /// Sets selected menu in ViewData.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewData["SelectedMenu"] = _selectedMenu.ToString().ToLower();
        }
    }
}