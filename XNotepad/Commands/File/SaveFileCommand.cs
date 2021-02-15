using System.Threading.Tasks;
using XNotepad.Core.Enums;
using XNotepad.Core.File;
using XNotepad.Core.UI;

namespace XNotepad.UI.Commands.File
{
    public class SaveFileCommand : BaseAsyncCommand<string>
    {
        private IFileManager fileManager;
        private IWindowManager windowManager;

        public SaveFileCommand(
            IFileManager fileManager,
            IWindowManager windowManager)
        {
            this.fileManager = fileManager;
            this.windowManager = windowManager;
        }

        protected override async Task ExecuteAsync(string fileId)
        {
            if (!this.fileManager.TryGetDocumentInfo(fileId, out var documentInfo))
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"Attempted save non-existent file. docId = {fileId}");
                return; // Shouldn't happen
            }

            if (documentInfo.Filepath != null)
            {
                await fileManager.SaveDocument(fileId);
            }
            else
            {
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
}
