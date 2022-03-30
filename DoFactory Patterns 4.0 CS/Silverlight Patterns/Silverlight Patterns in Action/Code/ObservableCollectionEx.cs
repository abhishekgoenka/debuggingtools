
using System.ComponentModel;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Silverlight_Patterns_in_Action.Code
{
    /// <summary>
    /// Extension to ObservableCollection. 
    /// 
    /// Fires the PropertyChanged event when any of its items raises the same event. 
    /// Maintains the proper eventhandlers when Collection changes occur.
    /// </summary>
    /// <typeparam name="T">The business object type being observed.</typeparam>
    public class ObservableCollectionEx<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            Detach(e.OldItems);
            Attach(e.NewItems);

            base.OnCollectionChanged(e);
        }

        private void Detach(IList list)
        {
            if (list != null)
            {
                foreach (T item in list)
                    item.PropertyChanged -= (x, y) => ItemPropertyChanged(y);
            }
        }

        private void Attach(IList list)
        {
            if (list != null)
            {
                foreach (T item in list)
                    item.PropertyChanged += (x, y) => ItemPropertyChanged(y);
            }
        }

        private void ItemPropertyChanged(PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);
        }
    }
}
