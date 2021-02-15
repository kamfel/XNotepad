using System;
using System.Windows.Media;
using XNotepad.Core.Args;

namespace XNotepad.Core.UI
{
    public interface IFontManager
    {
        event EventHandler<FontChangedArgs> FontChanged;
        public void GetCurrentFont(out FontFamily fontFamily, out int fontSize);
        public void SetCurrentFont(FontFamily fontFamily, int fontSize);
    }
}
