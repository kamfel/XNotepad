using System;
using System.Windows.Media;

namespace XNotepad.Core.Args
{
    public class FontChangedArgs : EventArgs
    {
        public FontFamily FontFamily { get; set; }
        public int FontSize { get; set; }
    }
}
