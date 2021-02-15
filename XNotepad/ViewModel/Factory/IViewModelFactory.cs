namespace XNotepad.UI.ViewModel.Factory
{
    public interface IViewModelFactory
    {
        T Create<T>()
            where T : BaseViewModel;
    }
}
