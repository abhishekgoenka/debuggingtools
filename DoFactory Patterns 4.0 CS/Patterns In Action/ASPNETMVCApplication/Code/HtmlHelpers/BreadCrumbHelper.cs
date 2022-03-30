using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace ASPNETMVCApplication.Code.HtmlHelpers
{
    /// <summary>
    /// Static breadcrumb helper class.
    /// </summary>
    public static class BreadCrumbHelper
    {
        /// <summary>
        /// Extension method. Builds complete breadcrumbs strings.
        /// </summary>
        /// <param name="html">The HtmlHelper.</param>
        /// <returns>Breadcrumbs string.</returns>
        public static MvcHtmlString BreadCrumbs(this HtmlHelper html)
        {
            // First check for custom breadcrumbs
            var breadCrumbs = html.ViewContext.ViewData.Eval("BreadCrumbs") as List<BreadCrumb>;

            // If there are none specified, get crumbs from sitemap.
            if (breadCrumbs == null) breadCrumbs = GetCrumbsFromSiteMap();

            var crumbs = new StringBuilder();
            for (int i = 0; i < breadCrumbs.Count; i++)
            {
                var crumb = breadCrumbs[i];

                if (string.IsNullOrEmpty(crumb.Url))
                    crumbs.Append(" " + crumb.Title);
                else
                    crumbs.AppendFormat("<a href='{0}'>{1}</a>", crumb.Url, crumb.Title);
               

                if (i < breadCrumbs.Count - 1) crumbs.Append(" > ");
            }

            return MvcHtmlString.Create(crumbs.ToString());
        }


        /// <summary>
        /// Helper method. If no custom breadcrumbs are present for a page, 
        /// then crumbs are generated from the sitemap. 
        /// </summary>
        /// <returns></returns>
        private static List<BreadCrumb> GetCrumbsFromSiteMap()
        {
            var crumbs = new List<BreadCrumb>();

            // Current node, with title only
            crumbs.Add(new BreadCrumb { Title = SiteMap.CurrentNode.Title }); 

            // Recurse through remainder of parent nodes
            var node = SiteMap.CurrentNode.ParentNode;
            while (node != null)
            {
                crumbs.Insert(0, new BreadCrumb { Url = node.Url, Title = node.Title });
                node = node.ParentNode;
            }

            return crumbs;
        }
    }
}