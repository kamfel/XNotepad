using System;
using System.Threading.Tasks;
using System.Windows;

namespace XNotepad.Core.UI
{
    public interface IWindowManager
    {
        string ShowFileDialog(DialogParameters parameters);
        bool? ShowModal(object dataContext);
        void ShowModeless(object dataContext);
        void DoWorkWithProgress(string message, Func<Task> work);
        MessageBoxResult ShowMessage(MessageViewModel model);
    }
}
