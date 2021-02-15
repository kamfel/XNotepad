using System.Diagnostics;
using System.Windows.Input;
using XNotepad.Core.File;
using XNotepad.Core.UI;
using XNotepad.UI.Commands;
using XNotepad.UI.Commands.File;
using XNotepad.UI.ViewModel.Factory;

namespace XNotepad.UI.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Injected

        private IWindowManager windowManager;
        private IViewModelFactory viewModelFactory;
        private IFileManager fileManager;

        #endregion Injected

        #region Properties

        public string Title { get; set; }

        public EditorViewModel ViewDataContext { get; private set; }

        #endregion Properties

        #region Commands

        public ICommand NewFileCommand { get; private set; }

        public ICommand OpenFileCommand { get; private set; }

        public ICommand SaveFileCommand { get; private set; }

        public ICommand SaveFileAsCommand { get; private set; }

        public ICommand OpenFontOptionsCommand { get; private set; }

        public ICommand OpenFindDialogCommand { get; private set; }

        public ICommand OpenAutoSaveOptionsCommand { get; private set; }

        #endregion Commands

        #region Ctor

        public MainWindowViewModel(
            IWindowManager windowManager,
            IFileManager fileManager,
            IViewModelFactory viewModelFactory,
            ICommandFactory commandFactory)
        {
            this.fileManager = fileManager;
            this.windowManager = windowManager;
            this.viewModelFactory = viewModelFactory;
            this.ViewDataContext = this.viewModelFactory.Create<EditorViewModel>();

            OpenFileCommand = commandFactory.Get<OpenFileCommand>();
            NewFileCommand = new Command(CreateFile);
            SaveFileCommand = commandFactory.Get<SaveFileCommand>();
            SaveFileAsCommand = commandFactory.Get<SaveFileAsCommand>();
            OpenFontOptionsCommand = new Command(OpenFontOptions, x => true, ex => Trace.WriteLine(ex));
            OpenFindDialogCommand = new Command(OpenFindDialog, x => true, ex => Trace.WriteLine(ex));
            OpenAutoSaveOptionsCommand = new Command(OpenAutoSaveOptions, x => true, ex => Trace.WriteLine(ex));
        }

        #endregion Ctor

        #region Command handlers

        public void CreateFile()
        {
            this.fileManager.CreateEmptyDocument();
        }

        public void OpenFontOptions()
        {
            var vm = this.viewModelFactory.Create<FontOptionsViewModel>();
            windowManager.ShowModal(vm);
        }

        public void OpenAutoSaveOptions()
        {
            var vm = this.viewModelFactory.Create<AutoSaveOptionsViewModel>();
            windowManager.ShowModal(vm);
        }

        public void OpenFindDialog()
        {
            var vm = this.viewModelFactory.Create<FindViewModel>();
            ViewDataContext.ToolViewModel = vm;
        }

        #endregion Command handlers
    }
}
