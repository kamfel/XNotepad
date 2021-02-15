using Ninject;
using XNotepad.Core.File;
using XNotepad.UI.Commands;
using XNotepad.UI.ViewModel.Factory;

namespace XNotepad.UI.ViewModel.Design
{
    public class MainWindowDesignViewModel : MainWindowViewModel
    {
        public static MainWindowDesignViewModel Instance { get; } = new MainWindowDesignViewModel();

        public MainWindowDesignViewModel()
            : base(new WindowManager(), new FileManager(), new ViewModelFactory(new StandardKernel()), new CommandFactory(new StandardKernel()))
        {
            Title = "XNotepad";

            //OpenedFiles = new ObservableCollection<OpenedFileViewModel>
            //{
            //    new OpenedFileViewModel()
            //    {
            //        Guid = Guid.NewGuid().ToString(),
            //        OpenedFileState = DocumentStateEnum.New,
            //        FileName = "testfile.txt",
            //        FilePath = @"C:\blale\testfile.txt",
            //        Content = "TEXTAegngiuaiouwrebnseuianrbioe dmgopern oeng oeno eo eon enp egp peorgbmepsb,lfnbl klesdl; ",
            //        IsVisible = true,
            //    },
            //    new OpenedFileViewModel()
            //    {
            //        Guid = Guid.NewGuid().ToString(),
            //        OpenedFileState = DocumentStateEnum.UpToDate,
            //        Content = "dkns w o eo wo wo ksldfjao; wo nwo anaofghnie i klabn ibaw dmgopern oeng oeno eo eon enp egp peorgbmepsb,lfnbl klesdl; ",
            //        FileName = "testfile21431.txt",
            //        IsVisible = true
            //    }
            //};

            //SelectedOpenedFile = OpenedFiles[0];
        }
    }
}
