using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETMVCApplication.Code.HtmlHelpers
{
    /// <summary>
    /// Breadcrumb class. Representing a single crumb.
    /// </summary>
    public class BreadCrumb
    {
        /// <summary>
        /// Url of crumb.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Title (name) of crumb.
        /// </summary>
        public string Title { get; set; }
    }
}