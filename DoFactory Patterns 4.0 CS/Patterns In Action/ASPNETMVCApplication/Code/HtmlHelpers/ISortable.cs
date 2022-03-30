using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace ASPNETMVCApplication.Code.HtmlHelpers
{
    /// <summary>
    /// Sortable interface. Defines column and order.
    /// </summary>
    public interface ISortable : IEnumerable
    {
        /// <summary>
        /// The sort column.
        /// </summary>
        string Sort { get; }

        /// <summary>
        /// The sort order.
        /// </summary>
        string Order { get; }
    }

    /// <summary>
    /// Generic form of ISortable interface.
    /// </summary>
    /// <typeparam name="T">Type of object being sorted.</typeparam>
    public interface ISortable<T> : ISortable, IEnumerable<T>
    {
        // No members..
    }
}