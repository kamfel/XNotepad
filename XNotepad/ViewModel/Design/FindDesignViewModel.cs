using XNotepad.Core.Search;

namespace XNotepad.UI.ViewModel.Design
{
    public class FindDesignViewModel : FindViewModel
    {
        public FindDesignViewModel(ISearchEngine searchEngine) : base(searchEngine)
        {
        }

        public static FindDesignViewModel Instance => new FindDesignViewModel(new SearchEngine());
    }
}
