using ICSharpCode.AvalonEdit.Document;
using System;

namespace XNotepad.Core.Args
{
    public class PositionChangedEventArgs : EventArgs
    {
        public ITextAnchor NewPosition { get; set; }
        public int Length { get; set; }
    }
}
