using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETMVCApplication.Code.HtmlHelpers
{
    /// <summary>
    /// Sortable list. Supports sorting of lists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SortedList<T> : ISortable<T>
    {
        /// <summary>
        /// The sorted list.
        /// </summary>
        public List<T> List { get; private set; }

        /// <summary>
        /// The sort column of the list.
        /// </summary>
        public string Sort { get; private set; }

        /// <summary>
        /// The sort order of the list.
        /// </summary>
        public string Order { get; private set; } 

        /// <summary>
        /// Constructor of SortedList.
        /// </summary>
        /// <param name="list">The sorted list.</param>
        /// <param name="sort">The sort column.</param>
        /// <param name="order">The sort order.</param>
        public SortedList(List<T> list, string sort = null, string order = null)
        {
            List = list;
            Sort = sort;
            Order = order;
        }


        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable<T> Members

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}