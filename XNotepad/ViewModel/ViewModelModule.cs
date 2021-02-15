using Ninject.Modules;
using XNotepad.UI.ViewModel.Factory;

namespace XNotepad.UI.ViewModel
{
    public class ViewModelModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IViewModelFactory>().To<ViewModelFactory>();

            this.Bind<MainWindowViewModel>().ToSelf();
            this.Bind<EditorViewModel>().ToSelf();
            this.Bind<AutoSaveOptionsViewModel>().ToSelf();
            this.Bind<FindViewModel>().ToSelf();
            this.Bind<FontOptionsViewModel>().ToSelf();
        }
    }
}
