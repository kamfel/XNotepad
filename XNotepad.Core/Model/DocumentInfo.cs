using ICSharpCode.AvalonEdit.Document;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XNotepad.Core.Utilities;

namespace XNotepad.Core.Model
{
    public class DocumentInfo
    {
        public SemaphoreSlim Semaphore { get; }
        public string Id { get; set; }
        public TextDocument Document { get; set; }
        public Encoding Encoding { get; set; }
        public byte[] Hash { get; set; }
        public string Filepath { get; set; }

        public DocumentInfo()
        {
            this.Semaphore = new SemaphoreSlim(1);
        }

        public async Task<bool> IsOutOfSyncWithFileAsync()
        {

            if (this.Filepath == null || this.Hash == null)
            {
                return false;
            }

            var fileInfo = new FileInfo(this.Filepath);

            if (!fileInfo.Exists)
            {
                return false;
            }

            using (var reader = this.Document.CreateReader())
            {
                var currentHash = await HashingUtility.ComputeHashFromReaderAsync(reader, this.Encoding);

                return !this.Hash.SequenceEqual(currentHash);
            }
        }
    }
}
