using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using XNotepad.Core.Enums;
using XNotepad.Core.Jobs;
using XNotepad.Core.Threading;

namespace XNotepad.Core.Search
{
    public class SearchEngine : ISearchEngine
    {
        public async Task FindInDocument(TextReader source, string phrase, SearchModeEnum mode, CancellationToken token, Action<int> onOffsetFound)
        {
            var dispatcher = Dispatcher.CurrentDispatcher;

            Action<int> onOffsetFoundUI = (int offset) =>
            {
                dispatcher.Invoke(onOffsetFound, offset); // Invoked on UI thread
            };

            switch (mode)
            {
                case SearchModeEnum.Default:
                    var job = new SearchJob(source, phrase, onOffsetFoundUI);
                    await JobRunner.Run(job, token);
                    break;
                case SearchModeEnum.Regex:
                    break;
                default:
                    break;
            }
        }
    }
}
