using PropertyChanged;
using System.ComponentModel;

namespace XNotepad.UI.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
