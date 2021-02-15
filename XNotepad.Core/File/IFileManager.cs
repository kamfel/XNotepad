using System;
using System.Threading.Tasks;
using XNotepad.Core.Model;

namespace XNotepad.Core.File
{
    public interface IFileManager
    {
        event EventHandler<string> DocumentOpened;

        event EventHandler<string> DocumentSaving;

        event EventHandler<string> DocumentSaved;

        bool TryGetDocumentInfo(string docId, out DocumentInfo document);

        Task<string> OpenDocument(string filepath);

        string CreateEmptyDocument();

        Task CloseDocument(string fileId);

        Task SaveAllDocuments();

        Task SaveDocument(string fileId, string filepath = null);
    }
}
