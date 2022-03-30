using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ASPNETMVCApplication.Code.HtmlHelpers
{
    /// <summary>
    /// Static result helper class. Holds extension method.
    /// </summary>
    public static class ResultHelper
    {
        // css class name
        private static readonly string ValidationInputCssClassName = "result-summary-valid";

        /// <summary>
        /// Extension method. Builds result summary string (of results in action methods).
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="result"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString ResultSummary(this HtmlHelper htmlHelper, string result = null, IDictionary<string, object> htmlAttributes = null)
        {
            string innerResult = result != null ? result : (string)htmlHelper.ViewData["Result"];
            if (innerResult == null) return null;

            var spanTag = new TagBuilder("span");
            spanTag.SetInnerText(innerResult);
            string resultSpan = spanTag.ToString(TagRenderMode.Normal) + Environment.NewLine;

            var divTag = new TagBuilder("div");
            divTag.MergeAttributes(htmlAttributes);
            divTag.AddCssClass(ValidationInputCssClassName);
            divTag.InnerHtml = resultSpan;

            return MvcHtmlString.Create(divTag.ToString(TagRenderMode.Normal));
        }
    }
}