using System;
using System.Windows.Input;

namespace MPData
{
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// The action to run.
        /// </summary>
        private Action _action;

        /// <summary>
        /// The event that fires when the <see cref="CanExecute(object)"/> value has changed.
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public RelayCommand(Action action)
        {
            _action = action;
        }

        /// <summary>
        /// A RelayCommand can always execute.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter) => true;
        
        /// <summary>
        /// Executes the command's Action.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _action();
        }
    }
}
