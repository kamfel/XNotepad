using System.Windows.Input;

namespace XNotepad.UI.Commands
{
    public interface ICommandFactory
    {
        T Get<T>() where T : ICommand;
    }
}
