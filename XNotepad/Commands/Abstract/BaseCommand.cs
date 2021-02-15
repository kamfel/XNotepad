using NLog;
using System;
using System.Windows.Input;

namespace XNotepad.UI.Commands
{
    public abstract class BaseCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (parameter is T validParameter)
                Execute(validParameter);
            else if (parameter is null && !typeof(T).IsValueType)
                Execute((T)parameter);
            else
                throw new ArgumentException("Invalid command parameter type.", nameof(T));
        }

        protected abstract void Execute(T parameter);

        protected virtual void OnException(Exception e)
        {
            LogManager.GetLogger("Command").Error(e);
        }

        private void ExecuteInternal(T parameter)
        {
            try
            {
                Execute(parameter);
            }
            catch (Exception e)
            {
                OnException(e);
            }
        }
    }

    public abstract class BaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            try
            {
                Execute();
            }
            catch (Exception e)
            {
                OnException(e);
            }
        }

        protected abstract void Execute();

        protected virtual void OnException(Exception e)
        {
            LogManager.GetLogger("Command").Error(e);
        }
    }
}
