using XNotepad.Core.File;
using XNotepad.Core.File.AutoSave;

namespace XNotepad.UI.ViewModel.Design
{
    public class EditorDesignViewModel : EditorViewModel
    {
        public static EditorDesignViewModel Instance { get; } = new EditorDesignViewModel();

        public EditorDesignViewModel()
            : base(new WindowManager(), new FileManager(), new AutoSaveManager(new FileManager(), new AutoSaveConfiguration()), new FontManager())
        {

        }
    }
}
