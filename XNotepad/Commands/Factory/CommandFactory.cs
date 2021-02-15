using Ninject;
using System.Windows.Input;

namespace XNotepad.UI.Commands
{
    public class CommandFactory : ICommandFactory
    {
        private IKernel kernel;

        public CommandFactory(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public T Get<T>() where T : ICommand
        {
            return kernel.Get<T>();
        }
    }
}
