using ICSharpCode.AvalonEdit.Document;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using XNotepad.Core.Args;
using XNotepad.Core.Enums;
using XNotepad.Core.Interfaces;
using XNotepad.Core.Search;

namespace XNotepad.UI.ViewModel
{
    public class FindViewModel : BaseViewModel, IEditorToolViewModel
    {
        private ISearchEngine searchEngine;

        private string searchedText;
        private bool ignoreCase;
        private SearchModeEnum searchMode;

        private CancellationTokenSource cancellationTokenSource;

        public event EventHandler<PositionChangedEventArgs> OnCaretPositionChanged;

        private TextDocument CurrentDocument { get; set; }
        public ObservableCollection<TextAnchor> Offsets { get; set; }
        public int FoundCount { get; set; }
        public int CurrentOffsetIndex { get; set; }
        public bool IsSearching { get; set; }
        public bool IsWaitingForNextMatch { get; set; }

        public string SearchedText
        {
            get => this.searchedText;
            set
            {
                if (this.searchedText != value)
                {
                    this.searchedText = value;
                    this.Offsets = null;
                    CancelSearch();
                }
            }
        }

        public SearchModeEnum SearchMode
        {
            get => this.searchMode;
            set
            {
                if (this.searchMode != value)
                {
                    this.searchMode = value;
                    this.Offsets = null;
                    CancelSearch();
                }
            }
        }

        public bool IgnoreCase
        {
            get => ignoreCase;
            set
            {
                if (ignoreCase != value)
                {
                    this.ignoreCase = value;
                    this.Offsets = null;
                    CancelSearch();
                }
            }
        }

        [DependsOn(nameof(SearchedText), nameof(CurrentDocument), nameof(OnCaretPositionChanged))]
        public bool CanFindNext => this.CurrentDocument != null
                && this.OnCaretPositionChanged != null
                && !string.IsNullOrWhiteSpace(this.SearchedText)
                && !IsWaitingForNextMatch;

        [DependsOn(nameof(Offsets))]
        public bool HasResults => this.Offsets != null;

        public ICommand FindNextCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public FindViewModel(ISearchEngine searchEngine)
        {
            this.searchEngine = searchEngine;
            this.cancellationTokenSource = new CancellationTokenSource();

            this.FindNextCommand = new AsyncCommand(FindNext, x => true, x => Trace.WriteLine(x));
            this.CloseCommand = new Command<ICloseable>(x => x.Close(), x => true, x => Trace.WriteLine(x));
        }

        public void ChangeDocument(TextDocument document)
        {
            this.CancelSearch();
            this.CurrentDocument = document;
        }

        public async Task FindNext()
        {
            if (!this.HasResults)
            {
                this.Offsets = new ObservableCollection<TextAnchor>();

                this.IsSearching = true;
                this.TryMoveCaretToNextPosition();
                using (var reader = this.CurrentDocument.CreateReader())
                {
                    await this.searchEngine.FindInDocument(
                        reader,
                        this.SearchedText,
                        this.SearchMode,
                        this.cancellationTokenSource.Token,
                        this.OnResultFoundCallback);
                }
                this.IsSearching = false;

                this.IsWaitingForNextMatch = false;
            }
            else
            {
                this.TryMoveCaretToNextPosition();
            }
        }

        private void OnResultFoundCallback(int offset)
        {
            if (this.Offsets != null)
            {
                var anchor = this.CurrentDocument.CreateAnchor(offset);
                this.Offsets.Add(anchor);
                this.FoundCount++;

                if (this.IsWaitingForNextMatch)
                {
                    this.IsWaitingForNextMatch = false;
                    this.TryMoveCaretToNextPosition();
                }
            }
        }

        private void CancelSearch()
        {
            if (this.cancellationTokenSource != null && this.IsSearching)
            {
                this.cancellationTokenSource.Cancel();
                this.cancellationTokenSource = new CancellationTokenSource();
            }

            this.Offsets = null;
            this.IsWaitingForNextMatch = false;
            this.FoundCount = 0;
            this.CurrentOffsetIndex = 0;
        }

        private void TryMoveCaretToNextPosition()
        {
            if (this.OnCaretPositionChanged == null || this.IsWaitingForNextMatch)
            {
                return;
            }


            if (this.CurrentOffsetIndex >= this.Offsets.Count)
            {
                if (this.IsSearching)
                {
                    this.IsWaitingForNextMatch = true;
                    return;
                }
                else
                {
                    this.CurrentOffsetIndex = 0;
                }
            }

            this.OnCaretPositionChanged.Invoke(
                this,
                new PositionChangedEventArgs()
                {
                    NewPosition = this.Offsets[this.CurrentOffsetIndex],
                    Length = this.SearchedText.Length
                });

            this.CurrentOffsetIndex++;
        }
    }
}
