using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XNotepad.Core.Extensions;
using XNotepad.Core.Jobs;
using XNotepad.Core.Model;
using XNotepad.Core.Threading;
using XNotepad.Core.Utilities;

namespace XNotepad.Core.File
{
    public class FileManager : IFileManager
    {
        private object documentCollectionLock = new object();

        private Logger logger;
        private List<DocumentInfo> openedDocuments;

        public event EventHandler<string> DocumentOpened;
        public event EventHandler<string> DocumentSaving;
        public event EventHandler<string> DocumentSaved;

        public FileManager()
        {
            this.logger = LogManager.GetLogger("FileManager");
            this.openedDocuments = new List<DocumentInfo>();
        }

        public bool TryGetDocumentInfo(string docId, out DocumentInfo document)
        {
            lock (documentCollectionLock)
            {
                document = this.openedDocuments.SingleOrDefault(x => x.Id == docId);

                return document != null;
            }
        }

        public async Task<string> OpenDocument(string filepath)
        {
            using var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.None);
            var hash = await HashingUtility.ComputeHashFromStreamAsync(stream);

            using var reader = FileReader.OpenStream(stream, Encoding.Default);
            var text = await reader.ReadToEndAsync();

            var info = new DocumentInfo()
            {
                Id = Guid.NewGuid().ToString(),
                Encoding = reader.CurrentEncoding,
                Document = new TextDocument(text),
                Hash = hash,
                Filepath = filepath
            };

            lock (documentCollectionLock)
            {
                this.openedDocuments.Add(info);
            }

            this.DocumentOpened?.Invoke(this, info.Id);

            return info.Id;
        }

        public string CreateEmptyDocument()
        {
            var info = new DocumentInfo()
            {
                Id = Guid.NewGuid().ToString(),
                Encoding = Encoding.Default,
                Document = new TextDocument(),
                Filepath = null
            };

            lock (documentCollectionLock)
            {
                openedDocuments.Add(info);
            }

            this.DocumentOpened?.Invoke(this, info.Id);

            return info.Id;
        }

        public async Task SaveDocument(string fileId, string filepath = null)
        {
            if (!this.TryGetDocumentInfo(fileId, out var docInfo))
            {
                this.logger.Info("Attempted save on closed document");
                return;
            }

            if (!docInfo.Semaphore.Wait(0))
            {
                this.logger.Info($"Couldn't enter monitor. DocId: {docInfo.Id}");
                return; // File is currently saved or removed by another thread. Do nothing.
            }

            try
            {
                // Verify document wasn't removed just before lock acquiring
                lock (documentCollectionLock)
                {
                    if (!this.openedDocuments.Contains(docInfo))
                    {
                        docInfo.Semaphore.Release();
                        return;
                    }
                }

                if (filepath == null)
                {
                    var isOutOfSync = await docInfo.IsOutOfSyncWithFileAsync().ConfigureAwait(true);

                    if (!isOutOfSync)
                    {
                        this.logger.Info($"File sychronised. docid = {docInfo.Id}");
                        this.DocumentSaved?.Invoke(this, docInfo.Id);
                        docInfo.Semaphore.Release();
                        return;
                    }
                }

                this.DocumentSaving?.Invoke(this, docInfo.Id);

                await this.SaveDocumentInternal(docInfo, filepath).ConfigureAwait(true);

                this.DocumentSaved?.Invoke(this, docInfo.Id);
            }
            finally
            {
                docInfo.Semaphore.Release();
            }
        }

        public Task CloseDocument(string fileId)
        {
            if (this.TryGetDocumentInfo(fileId, out var docInfo))
            {
                return Task.CompletedTask;
            }

            return Task.Run(async () =>
            {
                await docInfo.Semaphore.WaitAsync(); // Wait until save is finished.

                lock (documentCollectionLock)
                {
                    this.openedDocuments.TryRemove(docInfo);
                }

                docInfo.Semaphore.Release();
            });
        }

        public Task SaveAllDocuments()
        {
            lock (documentCollectionLock)
            {
                var tasks = new List<Task>(this.openedDocuments.Count);
                foreach (var docInfo in this.openedDocuments)
                {
                    if (docInfo.Filepath == null)
                    {
                        continue;
                    }

                    var task = Task.Run(() => this.SaveDocument(docInfo.Id));
                }

                return Task.WhenAll(tasks);
            }
        }

        private async Task SaveDocumentInternal(DocumentInfo docInfo, string filepath)
        {
            if (docInfo.Filepath == null && filepath == null)
            {
                throw new InvalidOperationException($"No filepath specified. DocId: {docInfo.Id}");
            }

            using (var reader = docInfo.Document.CreateReader())
            {
                var targetFilePath = filepath ?? docInfo.Filepath;
                var job = new FileSaveJob(targetFilePath, reader, docInfo.Encoding);
                var token = new CancellationToken();
                await JobRunner.Run(job, token);

                docInfo.Hash = job.SavedFileHash;
            }

            if (filepath != null && docInfo.Filepath != filepath)
            {
                docInfo.Filepath = filepath;
            }
        }
    }
}
