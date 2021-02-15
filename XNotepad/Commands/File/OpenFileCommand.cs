using System.Threading.Tasks;
using XNotepad.Core.Enums;
using XNotepad.Core.File;
using XNotepad.Core.UI;

namespace XNotepad.UI.Commands.File
{
    public class OpenFileCommand : BaseAsyncCommand
    {
        private IWindowManager windowManager;
        private IFileManager fileManager;

        public OpenFileCommand(
            IWindowManager windowManager,
            IFileManager fileManager)
        {
            this.windowManager = windowManager;
            this.fileManager = fileManager;
        }

        protected override async Task ExecuteAsync()
        {
            var filepath = windowManager.ShowFileDialog(new DialogParameters()
            {
                FileExtensions = { FileExtensionEnum.Txt },
                FileMustExist = true
            });

            if (string.IsNullOrEmpty(filepath))
            {
                return;
            }

            await fileManager.OpenDocument(filepath);
        }
    }
}
