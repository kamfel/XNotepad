using ICSharpCode.AvalonEdit.Document;
using System.Text;
using XNotepad.Core.Enums;

namespace XNotepad.UI.ViewModel
{
    public class DocumentViewModel : BaseViewModel
    {
        public string Id { get; }
        public TextDocument Document { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool IsVisible { get; set; }
        public DocumentStateEnum DocumentState { get; set; }
        public Encoding Encoding { get; set; }

        public DocumentViewModel(string id)
        {
            this.Id = id;
        }
    }
}
