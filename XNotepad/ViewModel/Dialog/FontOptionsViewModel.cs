using System.Collections.ObjectModel;
using System.Windows.Media;
using XNotepad.Core.Extensions;
using XNotepad.Core.Interfaces;
using XNotepad.Core.UI;

namespace XNotepad.UI.ViewModel
{
    public class FontOptionsViewModel : DialogBaseViewModel
    {
        private IFontManager fontManager;

        public override string Title => "Font options";

        public ObservableCollection<FontFamily> FontFamilies { get; set; }
        public FontFamily SelectedFontFamily { get; set; }
        public int FontSize { get; set; }

        public FontOptionsViewModel(IFontManager fontManager)
        {
            this.fontManager = fontManager;
            this.FontFamilies = Fonts.SystemFontFamilies.ToObservable();

            this.GetCurrentFont();

            this.AcceptCommand = new Command<ICloseable>(Accept);
            this.CancelCommand = new Command<ICloseable>(Cancel);
        }

        public void Accept(ICloseable closeable)
        {
            this.fontManager.SetCurrentFont(this.SelectedFontFamily, this.FontSize);
            closeable.Close();
        }

        public void Cancel(ICloseable closeable)
        {
            closeable.Close();
        }

        private void GetCurrentFont()
        {
            this.fontManager.GetCurrentFont(out var fontFamily, out var fontSize);

            if (fontFamily != null)
            {
                this.SelectedFontFamily = fontFamily;
            }

            this.FontSize = fontSize;
        }
    }
}
