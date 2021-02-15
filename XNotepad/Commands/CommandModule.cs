using Ninject.Modules;
using XNotepad.UI.Commands.File;

namespace XNotepad.UI.Commands
{
    public class CommandModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ICommandFactory>().To<CommandFactory>();

            this.Bind<OpenFileCommand>().ToSelf();
            this.Bind<SaveFileAsCommand>().ToSelf();
            this.Bind<SaveFileCommand>().ToSelf();
        }
    }
}
