using System.Windows;

namespace XNotepad.Core.UI
{
    public class MessageViewModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public MessageBoxButton Buttons { get; set; }
        public MessageBoxImage Icon { get; set; }
        public string OkText { get; set; }
        public string NoText { get; set; }
        public string CancelText { get; set; }
    }
}
