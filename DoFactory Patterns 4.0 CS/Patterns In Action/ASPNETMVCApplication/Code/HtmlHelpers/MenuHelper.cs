using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ASPNETMVCApplication.Code.HtmlHelpers
{
    /// <summary>
    /// Static menu helper class. Holds Menu extension method.
    /// </summary>
    public static class MenuHelper
    {
        /// <summary>
        /// Extension method. Builds menu item. Used on Master page only.
        /// </summary>
        /// <param name="html">The Html Helper</param>
        /// <param name="linkText">Link text</param>
        /// <param name="actionName">Name of action method</param>
        /// <param name="controllerName">Name of controller</param>
        /// <param name="routeValues">Route values</param>
        /// <returns>Anchor tag string.</returns>
        public static MvcHtmlString Menu(this HtmlHelper html, string linkText, string actionName, string controllerName, object routeValues = null)
        {
            var selectedLink = (string)html.ViewContext.ViewData.Eval("SelectedMenu");
            object htmlAttributes = selectedLink == linkText ? new { @class = "selected" } : null;

            // returns anchor tag with text and url.
            return html.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        }
    }
}