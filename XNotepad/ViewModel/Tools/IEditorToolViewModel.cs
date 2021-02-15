using ICSharpCode.AvalonEdit.Document;
using System;
using XNotepad.Core.Args;

namespace XNotepad.UI.ViewModel
{
    public interface IEditorToolViewModel
    {
        event EventHandler<PositionChangedEventArgs> OnCaretPositionChanged;

        void ChangeDocument(TextDocument document);
    }
}
