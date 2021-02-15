using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XNotepad.Core.Threading;

namespace XNotepad.Core.Jobs
{
    public class FileSaveJob : IJob
    {
        private const int BufferSize = 65536;

        private string filepath;
        private string tmpFilepath;
        private TextReader textReader;
        private Encoding encoding;

        public byte[] SavedFileHash { get; private set; }

        public FileSaveJob(
            string filepath,
            TextReader textReader,
            Encoding encoding)
        {
            this.filepath = filepath;
            this.textReader = textReader;
            this.encoding = encoding;
            this.tmpFilepath = filepath + ".tmp";
        }

        public string GetJobDescription()
        {
            return $"File save job";
        }

        public async Task RunAsync(CancellationToken ct)
        {
            using (var writer = new StreamWriter(tmpFilepath, false, encoding))
            {
                var buffer = new char[BufferSize];
                var currentSourceOffset = 0;

                while (textReader.Peek() > -1)
                {
                    var charsRead = await textReader.ReadBlockAsync(buffer, currentSourceOffset, buffer.Length).ConfigureAwait(false);

                    await writer.WriteAsync(buffer, 0, charsRead).ConfigureAwait(false);

                    currentSourceOffset += charsRead;
                }
            }

            try
            {
                System.IO.File.Move(tmpFilepath, filepath, true);

                using (var stream = System.IO.File.OpenRead(filepath))
                using (var hashAlgorithm = SHA1.Create())
                {
                    this.SavedFileHash = hashAlgorithm.ComputeHash(stream);
                }

            }
            finally
            {
                System.IO.File.Delete(tmpFilepath);
            }
        }
    }
}
