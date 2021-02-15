using System.Windows.Input;

namespace XNotepad.UI.ViewModel
{
    public abstract class DialogBaseViewModel : BaseViewModel
    {
        public abstract string Title { get; }

        public ICommand AcceptCommand { get; protected set; }

        public ICommand CancelCommand { get; protected set; }
    }
}
