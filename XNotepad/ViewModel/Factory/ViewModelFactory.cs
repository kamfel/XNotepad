using Ninject;

namespace XNotepad.UI.ViewModel.Factory
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IKernel kernel;

        public ViewModelFactory(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public T Create<T>()
            where T : BaseViewModel
        {
            return kernel.Get<T>();
        }
    }
}
