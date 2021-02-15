using NLog;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using XNotepad.Core.Extensions;

namespace XNotepad.UI
{
    public abstract class BaseAsyncCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (parameter is T validParameter)
                ExecuteAsync(validParameter).SafeFireAndForget(true, OnException);
            else if (parameter is null && !typeof(T).IsValueType)
                ExecuteAsync((T)parameter).SafeFireAndForget(true, OnException);
            else
                throw new ArgumentException("Invalid command parameter type.", nameof(T));
        }

        protected abstract Task ExecuteAsync(T parameter);

        protected virtual void OnException(Exception e)
        {
            LogManager.GetLogger("Command").Error(e);
        }
    }

    public abstract class BaseAsyncCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => ExecuteAsync().SafeFireAndForget(true, OnException);

        protected abstract Task ExecuteAsync();

        protected virtual void OnException(Exception e)
        {
            LogManager.GetLogger("Command").Error(e);
        }
    }
}