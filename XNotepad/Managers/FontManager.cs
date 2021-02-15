using System;
using System.Linq;
using System.Windows.Media;
using XNotepad.Core.Args;
using XNotepad.Core.UI;

namespace XNotepad.UI
{
    public class FontManager : IFontManager
    {
        private FontFamily fontFamily;
        private int fontSize;

        public event EventHandler<FontChangedArgs> FontChanged;

        public FontManager()
        {
            this.fontSize = 15;
            this.fontFamily = Fonts.SystemFontFamilies.SingleOrDefault(x => x.FamilyNames.Any(y => y.Value == "Arial"));
        }

        public void GetCurrentFont(out FontFamily fontFamily, out int fontSize)
        {
            fontFamily = this.fontFamily;
            fontSize = this.fontSize;
        }

        public void SetCurrentFont(FontFamily fontFamily, int fontSize)
        {
            this.fontFamily = fontFamily;
            this.fontSize = fontSize;

            if (this.FontChanged != null)
            {
                this.FontChanged(this, new FontChangedArgs()
                {
                    FontFamily = fontFamily,
                    FontSize = fontSize
                });
            }
        }
    }
}
