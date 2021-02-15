using Ninject.Modules;
using XNotepad.Core.File;
using XNotepad.Core.Search;

namespace XNotepad.Core
{
    public class CoreModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ISearchEngine>().To<SearchEngine>();
            this.Bind<IFileManager>().To<FileManager>().InSingletonScope();
            this.Bind<IAutoSaveManager>().To<AutoSaveManager>().InSingletonScope();
        }
    }
}
