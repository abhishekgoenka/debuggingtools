using System.ComponentModel;

namespace Silverlight_Patterns_in_Action.ViewModels
{
    /// <summary>
    /// Common ViewModelEventArgs (shared by all ViewModel events). 
    /// Derives from CancelEventArgs (used to cancel delete events).
    /// Can be extended with additional ViewModel specific properties.
    /// </summary>
    public class ViewModelEventArgs : CancelEventArgs
    {
        // Empty
    }
}
