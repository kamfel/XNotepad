using System.Threading.Tasks;
using XNotepad.Core.File.AutoSave;

namespace XNotepad.Core.File
{
    public interface IAutoSaveManager
    {
        AutoSaveConfiguration Configuration { get; }
        bool IsEnabled { get; }
        bool IsPaused { get; set; }
        void DocumentHasChanged(string prevFileId);
        void TextHasChanged(string fileId, int changeLength);
        Task UpdateConfigurationAsync(AutoSaveConfiguration config);
        void Enable();
        Task Disable();
    }
}
