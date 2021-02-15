using System;
using System.Threading.Tasks;
using System.Windows.Input;
using XNotepad.Core.Extensions;

namespace XNotepad.UI
{
    public sealed class AsyncCommand<T> : ICommand
    {
        #region Constant Fields
        readonly Func<T, Task> _execute;
        readonly Func<object, bool> _canExecute;
        readonly Action<Exception> _onException;
        readonly bool _continueOnCapturedContext;
        #endregion

        #region Constructors

        public AsyncCommand(Func<T, Task> execute,
                            Func<object, bool> canExecute = null,
                            Action<Exception> onException = null,
                            bool continueOnCapturedContext = true)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute), $"{nameof(execute)} cannot be null");
            _canExecute = canExecute ?? (_ => true);
            _onException = onException;
            _continueOnCapturedContext = continueOnCapturedContext;
        }

        #endregion

        public event EventHandler CanExecuteChanged;

        #region Methods

        public bool CanExecute(object parameter) => _canExecute(parameter);

        public Task ExecuteAsync(T parameter) => _execute(parameter);

        void ICommand.Execute(object parameter)
        {
            if (parameter is T validParameter)
                ExecuteAsync(validParameter).SafeFireAndForget(_continueOnCapturedContext, _onException);
            else if (parameter is null && !typeof(T).IsValueType)
                ExecuteAsync((T)parameter).SafeFireAndForget(_continueOnCapturedContext, _onException);
            else
                throw new ArgumentException("Invalid command parameter type.", nameof(T));
        }
        #endregion
    }

    public sealed class AsyncCommand : ICommand
    {
        #region Constant Fields
        readonly Func<Task> _execute;
        readonly Func<object, bool> _canExecute;
        readonly Action<Exception> _onException;
        readonly bool _continueOnCapturedContext;
        #endregion

        #region Constructors

        public AsyncCommand(Func<Task> execute,
                            Func<object, bool> canExecute = null,
                            Action<Exception> onException = null,
                            bool continueOnCapturedContext = true)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute), $"{nameof(execute)} cannot be null");
            _canExecute = canExecute ?? (_ => true);
            _onException = onException;
            _continueOnCapturedContext = continueOnCapturedContext;
        }

        #endregion

        public event EventHandler CanExecuteChanged;

        #region Methods

        public bool CanExecute(object parameter) => _canExecute(parameter);

        public Task ExecuteAsync() => _execute();

        void ICommand.Execute(object parameter) => _execute().SafeFireAndForget(_continueOnCapturedContext, _onException);

        #endregion
    }
}