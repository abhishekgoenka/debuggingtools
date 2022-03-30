using System.Windows.Input;

namespace WPFViewModel
{
    /// <summary>
    /// Abstract class that encapsulates a routed UI command.
    /// </summary>
    public abstract class CommandModel 
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CommandModel()
        {
            Command = new RoutedUICommand();
        }

        /// <summary>
        /// Gets the routed command.
        /// </summary>
        public RoutedUICommand Command{ private set; get; }

        /// <summary>
        /// Abstract method to execute the command. Needs implementation.
        /// </summary>
        public abstract void OnExecute(object sender, ExecutedRoutedEventArgs e);

        /// <summary>
        /// Determines if a command is enabled. Override to provide custom behavior. 
        /// Do not call the base version when overriding.
        /// </summary>
        public virtual void OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
    }
}
