using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToDoApplication.Util;

namespace ToDoApplication.Command
{
    internal class AsyncCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
                
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        public void Execute(object parameter)
        {
            AsyncVoidHelper.TryThrowOnDispatcher(_execute);
        }

        public void RaiseCanExecuteChanged()
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
    }
    internal class AsyncCommand<T> : ICommand where T : class
    {
        private readonly Func<T, Task> _execute;    
        private readonly Func<T, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter as T);
        }

        public void Execute(object parameter)
        {

           AsyncVoidHelper.TryThrowOnDispatcher(() => _execute(parameter as T));
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

}
