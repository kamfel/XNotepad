using System;
using System.Windows.Input;

namespace XNotepad.UI
{
    public class Command<T> : ICommand
    {
        #region Constant Fields
        readonly Action<T> _execute;
        readonly Func<object, bool> _canExecute;
        readonly Action<Exception> _onException;
        #endregion

        #region Constructors

        public Command(Action<T> execute,
                            Func<object, bool> canExecute = null,
                            Action<Exception> onException = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute), $"{nameof(execute)} cannot be null");
            _canExecute = canExecute ?? (_ => true);
            _onException = onException;
        }

        #endregion

        public event EventHandler CanExecuteChanged;

        #region Methods

        public bool CanExecute(object parameter) => _canExecute(parameter);

        public void Execute(object parameter)
        {
            if (parameter is T validParameter)
                ExecuteInternal(validParameter);
            else if (parameter is null && !typeof(T).IsValueType)
                ExecuteInternal((T)parameter);
            else
                throw new ArgumentException("Invalid command parameter type.", nameof(T));
        }

        private void ExecuteInternal(T parameter)
        {
            try
            {
                _execute.Invoke(parameter);
            }
            catch (Exception ex) when (_onException != null)
            {
                _onException?.Invoke(ex);
            }
        }
        #endregion
    }

    public class Command : ICommand
    {
        #region Constant Fields
        readonly Action _execute;
        readonly Func<object, bool> _canExecute;
        readonly Action<Exception> _onException;
        #endregion

        #region Constructors

        public Command(Action execute,
                            Func<object, bool> canExecute = null,
                            Action<Exception> onException = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute), $"{nameof(execute)} cannot be null");
            _canExecute = canExecute ?? (_ => true);
            _onException = onException;
        }

        #endregion

        public event EventHandler CanExecuteChanged;

        #region Methods

        public bool CanExecute(object parameter) => _canExecute(parameter);

        public void Execute(object parameter)
        {
            try
            {
                _execute.Invoke();
            }
            catch (Exception ex) when (_onException != null)
            {
                NLog.LogManager.GetLogger("Command").Error(ex);
                _onException?.Invoke(ex);
            }
        }
        #endregion
    }
}
