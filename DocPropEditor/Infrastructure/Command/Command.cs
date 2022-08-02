using System;
using System.Windows.Input;

namespace DocPropEditor.Infrastructure.Command
{
    internal class Command:ICommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Action<object> _execute;

        public Command(Action<object> execute , Func<bool> canExecute = null)
        {
            _canExecute = canExecute;
            _execute = execute;
        }



        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

    

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute();
            }

            return true;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
