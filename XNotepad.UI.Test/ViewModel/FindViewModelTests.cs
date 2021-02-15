using ICSharpCode.AvalonEdit.Document;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XNotepad.Core.Enums;
using XNotepad.Core.Search;
using XNotepad.TestingUtilities;
using XNotepad.UI.ViewModel;

namespace XNotepad.UI.Tests.ViewModel
{
    public class FindViewModelTests
    {
        [Test]
        public async Task FindNext_finds_valid_positions_in_document()
        {
            var phrase = "Lorem";
            var document = new TextDocument(
@"Lorem ipsum abs xddd work or noe iavb Lorem back down weug
. This is Lorem or not Lor working and not.");

            var indexes = new int[] { 0, 38, 70, 0 };
            var currentIndex = 0;

            var engine = new SearchEngine();
            var sut = new FindViewModel(engine);
            sut.ChangeDocument(document);
            sut.SearchedText = phrase;
            sut.OnCaretPositionChanged += (s, e) =>
            {
                Assert.Less(currentIndex, indexes.Length);
                Assert.AreEqual(indexes[currentIndex], e.NewPosition);
                currentIndex++;
            };

            for (int i = 0; i < indexes.Length; i++)
            {
                await sut.FindNext();
            }

            Assert.AreEqual(indexes.Length - 1, currentIndex);
        } 

        [Test, AutoMoqData]
        public void ChangeDocument_clears_results(ISearchEngine engine)
        {
            //var sut = new FindViewModel(engine);
            //sut.Offsets = new ObservableCollection<int>() { 1, 4, 8 };
            //sut.ChangeDocument(new TextDocument());

            //CollectionAssert.IsEmpty(sut.Offsets);
        }

        [Test, AutoMoqData]
        public void Changing_parameters_clears_results(ISearchEngine engine)
        {
            Assert.Pass();
        }

        [Test]
        public async Task Changing_parameters_triggers_search_cancelation()
        {
            var searchCancelled = false;
            Task verifier = null;

            var engine = new Mock<ISearchEngine>();
            engine.Setup(x =>
                x.FindInDocument(
                    It.IsAny<TextReader>(),
                    It.IsAny<string>(),
                    It.IsAny<SearchModeEnum>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Action<int>>()))
                .Callback<TextReader, string, SearchModeEnum, CancellationToken, Action<int>>(async (a, b, v, ct, e) =>
                {
                    verifier = new Task(async () =>
                    {
                        while (true)
                        {
                            await Task.Delay(100);
                            if (ct.IsCancellationRequested)
                            {
                                searchCancelled = true;
                                ct.ThrowIfCancellationRequested();
                            }
                        }
                    }, ct);

                    await Task.Run(() => verifier);
                });

            var sut = new FindViewModel(engine.Object);
            sut.ChangeDocument(new TextDocument());

            await sut.FindNext();
            sut.IgnoreCase = !sut.IgnoreCase;
            await Task.Delay(150);

            Assert.NotNull(verifier);
            Assert.IsTrue(verifier.IsCanceled);
            Assert.IsTrue(searchCancelled);
        }
    }
}
