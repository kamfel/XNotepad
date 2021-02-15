using System.Threading.Tasks;
using XNotepad.Core.Enums;
using XNotepad.Core.File;
using XNotepad.Core.UI;

namespace XNotepad.UI.Commands.File
{
    public class SaveFileAsCommand : BaseAsyncCommand<string>
    {
        private IFileManager fileManager;
        private IWindowManager windowManager;

        public SaveFileAsCommand(
            IFileManager fileManager,
            IWindowManager windowManager)
        {
            this.fileManager = fileManager;
            this.windowManager = windowManager;
        }

        protected override async Task ExecuteAsync(string fileId)
        {
            if (!this.fileManager.TryGetDocumentInfo(fileId, out var docInfo))
            {
                return;
            }

            var filepath = windowManager.ShowFileDialog(new DialogParameters()
            {
                FileExtensions = { FileExtensionEnum.Txt, FileExtensionEnum.All },
                FileMustExist = false
            });

            if (filepath == null)
            {
                return;
            }

            await this.fileManager.SaveDocument(fileId, filepath);
        }
    }
}
