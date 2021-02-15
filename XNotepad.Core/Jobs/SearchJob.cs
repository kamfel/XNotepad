using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using XNotepad.Core.Threading;

namespace XNotepad.Core.Jobs
{
    public class SearchJob : IJob
    {
        private TextReader reader;
        private Action<int> OnTextFound;
        private string searchedText;
        private bool ignoreCase;

        public SearchJob(
            TextReader reader,
            string searchedText,
            Action<int> onTextFound,
            bool ignoreCase = false)
        {
            this.reader = reader;
            this.searchedText = searchedText;
            this.OnTextFound = onTextFound;
            this.ignoreCase = ignoreCase;
        }

        public async Task RunAsync(CancellationToken ct)
        {
            var currentIndex = 0;
            var buffer = new char[100];
            var lastIndexMatched = 0;

            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(1000);
                var charsRead = await this.reader.ReadAsync(buffer, 0, buffer.Length);

                if (charsRead == 0)
                {
                    return;
                }

                for (int i = 0; i < charsRead; i++)
                {
                    if (IsMatch(searchedText[lastIndexMatched], buffer[i]))
                    {
                        lastIndexMatched++;

                        if (lastIndexMatched == searchedText.Length)
                        {
                            OnTextFound(currentIndex + i - lastIndexMatched + 1);
                            lastIndexMatched = 0;
                        }
                    }
                    else
                    {
                        lastIndexMatched = 0;
                    }
                }

                currentIndex += charsRead;
            }
        }

        public string GetJobDescription()
        {
            return "Search job";
        }

        private bool IsMatch(char char1, char char2)
        {
            if (ignoreCase)
            {
                return char.ToLower(char1) == char.ToLower(char2);
            }
            else
            {
                return char1 == char2;
            }
        }
    }
}
