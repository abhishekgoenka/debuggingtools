using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ASPNETMVCApplication.Code.HtmlHelpers
{
    /// <summary>
    /// Sorter helper class. Holds extension methods.
    /// </summary>
    public static class SorterHelper
    {
        /// <summary>
        /// Extension method. Returns anchor element (a) that contains the virtual path for sort action.
        /// This is where column sort headers are created.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="list"></param>
        /// <param name="linkText"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString Sorter<T>(this HtmlHelper html, SortedList<T> list, string linkText, string sort, string order, object htmlAttributes = null)
        {
            if (list == null) return null;

            var tag = new TagBuilder("a");
            tag.InnerHtml = linkText;

            // Set Css class to selected if indeed selected
            if (list.Sort.Equals(sort, StringComparison.InvariantCultureIgnoreCase))
                tag.AddCssClass("selected-" + list.Order);

            // Onclick: submit back and sort by same column but in reverse order. Uses jQuery.
            tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tag.MergeAttribute("onclick", "$('#sort').val('" + sort + "');$('#order').val('" + list.Order.ReverseOrder() + "');$('form').submit();return false;");

            // Set the correct url to anchor tag. 
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var dictionary = html.ViewContext.RequestContext.RouteData.Values;
            string url = urlHelper.RouteUrl(dictionary);
            tag.MergeAttribute("href", url);

            return MvcHtmlString.Create(tag.ToString());
        }

        // Reverses sort order.
        private static string ReverseOrder(this string order)
        {
            return order.ToLower() == "asc" ? "desc" : "asc";
        }
    }
}