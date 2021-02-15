using ICSharpCode.AvalonEdit.Document;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using XNotepad.Core.Args;
using XNotepad.Core.Enums;
using XNotepad.Core.File;
using XNotepad.Core.UI;
using XNotepad.Core.Utilities;

namespace XNotepad.UI.ViewModel
{
    public class EditorViewModel : BaseViewModel
    {
        private readonly object documentCollectionLock = new object();

        private IWindowManager windowManager;
        private IFileManager fileManager;
        private IAutoSaveManager autoSaveManager;
        private IFontManager fontManager;

        private TextLocation currentCaretLocation;
        private int currentCaretOffset;
        private DocumentViewModel currentDocument;
        private IEditorToolViewModel editorToolViewModel;

        public ObservableCollection<DocumentViewModel> Documents { get; set; }
        public DocumentViewModel CurrentDocument
        {
            get => currentDocument;
            set
            {
                if (this.currentDocument != null)
                {
                    autoSaveManager.DocumentHasChanged(this.currentDocument.Id);
                }

                if (this.CurrentDocument != value)
                {
                    this.currentDocument = value;
                    this.ToolViewModel?.ChangeDocument(value?.Document);
                }
            }
        }

        public FontFamily CurrentFontFamily { get; set; }
        public int CurrentFontSize { get; set; }

        public IEditorToolViewModel ToolViewModel
        {
            get => editorToolViewModel;
            set
            {
                this.editorToolViewModel = value;

                if (this.editorToolViewModel != null)
                {
                    this.editorToolViewModel.ChangeDocument(this.CurrentDocument?.Document);
                    this.editorToolViewModel.OnCaretPositionChanged += this.OnCaretPositionChanged;
                }
            }
        }

        public int SelectionOffset { get; set; }
        public int SelectionLength { get; set; }

        #region CaretLocation

        public int CurrentCaretOffset
        {
            get => currentCaretOffset;
            set
            {
                if (this.CurrentDocument != null)
                {
                    this.currentCaretLocation = this.CurrentDocument.Document.GetLocation(value);
                }

                this.currentCaretOffset = value;
            }
        }

        [DependsOn(nameof(CurrentCaretOffset))]
        public int CurrentLine => this.currentCaretLocation.Line;

        [DependsOn(nameof(CurrentCaretOffset))]
        public int CurrentColumn => this.currentCaretLocation.Column;

        #endregion CaretLocation

        public ICommand CloseFileCommand { get; private set; }

        public EditorViewModel(
            IWindowManager windowManager,
            IFileManager fileManager,
            IAutoSaveManager autoSaveManager,
            IFontManager fontManager)
        {
            this.windowManager = windowManager;
            this.fileManager = fileManager;
            this.autoSaveManager = autoSaveManager;
            this.fontManager = fontManager;

            this.fontManager.GetCurrentFont(out var fontFamily, out var fontSize);

            if (fontFamily != null)
            {
                this.CurrentFontFamily = fontFamily;
            }

            this.CurrentFontSize = fontSize;
            this.CurrentCaretOffset = 0;
            this.Documents = new ObservableCollection<DocumentViewModel>();
            this.currentCaretLocation = new TextLocation(1, 1);

            BindingOperations.EnableCollectionSynchronization(this.Documents, documentCollectionLock);

            this.fileManager.DocumentOpened += FileManager_DocumentOpened;
            this.fileManager.DocumentSaving += FileManager_DocumentSaving;
            this.fileManager.DocumentSaved += FileManager_DocumentSaved;

            this.fontManager.FontChanged += FontManager_FontChanged;

            this.CloseFileCommand = new AsyncCommand<string>(CloseFile);
        }


        public async Task CloseFile(string id)
        {
            this.autoSaveManager.IsPaused = true;

            var document = this.Documents.SingleOrDefault(x => x.Id == id);

            if (document == null)
            {
                this.autoSaveManager.IsPaused = false;
                return;
            }

            switch (document.DocumentState)
            {
                case DocumentStateEnum.New:
                case DocumentStateEnum.Modified:
                    var result = this.windowManager.ShowMessage(new MessageViewModel()
                    {
                        Title = "",
                        Message = "This file has unsaved changes. Do you want to continue?",
                        Buttons = MessageBoxButton.OKCancel,
                        Icon = MessageBoxImage.Question
                    });

                    if (result == MessageBoxResult.OK)
                    {
                        await this.CloseDocument(document);
                    }

                    break;
                case DocumentStateEnum.Saving:

                    this.windowManager.ShowMessage(new MessageViewModel()
                    {
                        Title = "",
                        Message = "File is currently saved.",
                        Buttons = MessageBoxButton.OK,
                        Icon = MessageBoxImage.Exclamation
                    });

                    break;
                case DocumentStateEnum.UpToDate:
                    await this.CloseDocument(document);
                    break;
                default:
                    break;
            }

            this.autoSaveManager.IsPaused = false;
        }

        private void FileManager_DocumentOpened(object sender, string docId)
        {
            lock (documentCollectionLock)
            {
                if (!this.fileManager.TryGetDocumentInfo(docId, out var info))
                {
                    return;
                }

                var isNewDocument = info.Hash == null; //New document doesn't have hash computed
                var newDocumentCount = this.Documents.Count(x => x.DocumentState == DocumentStateEnum.New);

                var filename = isNewDocument ? $"new {newDocumentCount + 1}" : FileUtilities.ExtractFileNameFromFullPath(info.Filepath);
                var filepath = isNewDocument ? null : info.Filepath;

                var document = new DocumentViewModel(docId)
                {
                    Document = info.Document,
                    FileName = filename,
                    FilePath = filepath,
                    IsVisible = true,
                    DocumentState = isNewDocument ? DocumentStateEnum.New : DocumentStateEnum.UpToDate,
                    Encoding = info.Encoding,
                };

                document.Document.Changed += this.OnTextChanged;

                this.Documents.Add(document);
                this.CurrentDocument = document;
            }
        }

        private void FileManager_DocumentSaving(object sender, string docId)
        {
            lock (documentCollectionLock)
            {
                var document = this.Documents.SingleOrDefault(x => x.Id == docId);

                if (document == null)
                {
                    return;
                }

                document.DocumentState = DocumentStateEnum.Saving;
            }
        }

        private void FileManager_DocumentSaved(object sender, string docId)
        {
            lock (documentCollectionLock)
            {
                var document = this.Documents.SingleOrDefault(x => x.Id == docId);

                if (document == null)
                {
                    return;
                }

                if (!this.fileManager.TryGetDocumentInfo(docId, out var docInfo))
                {
                    return;
                }

                if (docInfo.Filepath != null)
                {
                    document.FileName = FileUtilities.ExtractFileNameFromFullPath(docInfo.Filepath);
                    document.FilePath = docInfo.Filepath;
                    document.DocumentState = DocumentStateEnum.UpToDate;
                }
            }
        }

        private void FontManager_FontChanged(object sender, FontChangedArgs e)
        {
            if (e.FontFamily != null)
            {
                this.CurrentFontFamily = e.FontFamily;
            }

            this.CurrentFontSize = e.FontSize;
        }

        private void OnTextChanged(object sender, DocumentChangeEventArgs args)
        {
            var document = sender as TextDocument;

            if (document == null)
            {
                return;
            }

            var changeCount = args.InsertionLength + args.RemovalLength;

            if (changeCount != 0)
            {
                lock (this.documentCollectionLock)
                {
                    var docInfo = this.Documents.SingleOrDefault(x => x.Document == document);

                    if (docInfo != null)
                    {
                        docInfo.DocumentState = DocumentStateEnum.Modified;
                        this.autoSaveManager.TextHasChanged(docInfo.Id, changeCount);
                    }
                }
            }

        }

        private void OnCaretPositionChanged(object sender, PositionChangedEventArgs args)
        {
            this.CurrentCaretOffset = args.NewPosition.Offset;
            this.SelectionLength = args.Length;
            this.SelectionOffset = args.NewPosition.Offset;
        }

        private async Task CloseDocument(DocumentViewModel document)
        {
            await this.fileManager.CloseDocument(document.Id);

            lock (documentCollectionLock)
            {
                document.Document.Changed -= this.OnTextChanged;
                this.Documents.Remove(document);
            }

        }
    }
}
