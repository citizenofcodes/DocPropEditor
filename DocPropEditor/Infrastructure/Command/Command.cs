﻿using System;
using System.Windows.Input;

namespace DocPropEditor.Infrastructure.Command
{
    internal class Command:ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        public Command(Action<object> execute , Predicate<object> canExecute = null)
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
                return _canExecute(parameter);
            }

            return true;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
