using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace XNotepad.Core.Utilities
{
    public static class HashingUtility
    {
        public static Task<byte[]> ComputeHashFromStreamAsync(Stream stream)
        {
            return Task.Run(() =>
            {
                using (var algo = SHA1.Create())
                {
                    var hash = algo.ComputeHash(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    return hash;
                }
            });
        }

        public static Task<byte[]> ComputeHashFromReaderAsync(TextReader reader, Encoding encoding)
        {
            return Task.Run(() =>
            {
                var text = reader.ReadToEnd();
                var bytes = encoding.GetBytes(text);

                using (var algo = SHA1.Create())
                {
                    return algo.ComputeHash(bytes);
                }
            });
        }
    }
}
