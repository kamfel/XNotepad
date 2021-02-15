using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using XNotepad.Core.Enums;

namespace XNotepad.Core.Search
{
    public interface ISearchEngine
    {
        Task FindInDocument(TextReader source, string phrase, SearchModeEnum mode, CancellationToken token, Action<int> onOffsetFound);
    }
}
