using ICSharpCode.AvalonEdit.Document;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using XNotepad.Core.File;
using XNotepad.Core.UI;
using XNotepad.TestingUtilities;
using XNotepad.UI.ViewModel;

namespace XNotepad.UI.Tests.ViewModel
{
    [TestFixture]
    public class EditorViewModelTests : TestBase
    {
        [Test, AutoMoqData]
        public void Should_have_at_least_one_document_opened_after_construction(EditorViewModel sut)
        {
            Assert.That(sut.Documents.Count > 0);
            Assert.NotNull(sut.CurrentDocument);
        }

        [Test, AutoMoqData]
        public void Caret_location_of_current_document_is_valid_after_construction(EditorViewModel sut)
        {
            Assert.Multiple(() =>
            {
                Assert.That(sut.CurrentLine > 0);
                Assert.That(sut.CurrentColumn > 0);
                Assert.That(sut.CurrentCaretOffset >= 0);
            });
        }

        [Test, AutoMoqData]
        public void All_commands_are_defined_after_contruction(EditorViewModel sut)
        {
            var commandProperties = typeof(EditorViewModel)
                .GetProperties()
                .Where(x => x.PropertyType is ICommand)
                .Select(x => x.GetValue(sut));

            Assert.That(commandProperties, Is.All.Not.Null);
        }
    }
}
