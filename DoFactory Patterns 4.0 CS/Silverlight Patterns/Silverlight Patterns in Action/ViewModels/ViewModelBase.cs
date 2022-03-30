using System;
using System.ComponentModel;

namespace Silverlight_Patterns_in_Action.ViewModels
{
    /// <summary>
    /// Abstract base class for all ViewModels.
    /// Provides INotifyPropertyChanged services and Event handler services.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// PropertyChanged event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Raises event when an events handler is available.
        protected void RaiseEvent(EventHandler<ViewModelEventArgs> handler)
        {
            if (handler != null)
                handler(this, new ViewModelEventArgs());
        }
    }
}
