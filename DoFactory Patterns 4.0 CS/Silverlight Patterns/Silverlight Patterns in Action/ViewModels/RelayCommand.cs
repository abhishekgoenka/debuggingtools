using System;
using System.Windows.Input;

namespace Silverlight_Patterns_in_Action.ViewModels
{
    /// <summary>
    /// Command helper. Command execution is 'relayed' to a delegate Action.
    /// Allows programmatic enable/disable of command.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private Action<object> _handler;
        private bool _isEnabled;

        /// <summary>
        /// RelayCommand constructor.
        /// </summary>
        /// <param name="handler"></param>
        public RelayCommand(Action<object> handler)
        {
            _handler = handler;
        }

        /// <summary>
        /// Gets and sets enable status of command.
        /// </summary>
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    if (CanExecuteChanged != null)
                    {
                        CanExecuteChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Check if command can execute.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _handler(parameter);
        }
    }
}
