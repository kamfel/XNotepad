using ICSharpCode.AvalonEdit.Document;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XNotepad.Core.Jobs;

namespace XNotepad.Core.Tests.Jobs
{
    public class SearchJobTests
    {
        [TestCase("abc sse aseonw vehr ddef", "abc", 0)]
        [TestCase("abc sse aseonw vehr ddef", "sse", 4)]
        [TestCase("abceonw vehr ddef", "ceonw", 2)]
        [TestCase("abc sse aseonw vehr ddef", "f", 23)]
        public async Task Text_is_found_at_correct_offset(string text, string searched, int offset)
        {
            var offsets = new List<int>();
            var document = new TextDocument(text);
            var token = new CancellationToken();
            Action<int> onFound = (int offset) => offsets.Add(offset);
            var sut = new SearchJob(document.CreateReader(), searched, onFound);

            await sut.RunAsync(token);

            Assert.AreEqual(1, offsets.Count);
            Assert.AreEqual(offset, offsets[0]);
        }

        [TestCase("abc sse aseonw vehr ddef", "abcebar")]
        [TestCase("abc sse aseonw vehr ddef", "sseesfsfs")]
        [TestCase("abceonw vehr ddef", "ceovvvewnw")]
        [TestCase("abc sse aseonw vehr ddef", "fwcw")]
        public async Task Text_is_not_found_if_no_match_found(string text, string searched)
        {
            var offsets = new List<int>();
            var document = new TextDocument(text);
            var token = new CancellationToken();
            Action<int> onFound = (int offset) => offsets.Add(offset);
            var sut = new SearchJob(document.CreateReader(), searched, onFound);

            await sut.RunAsync(token);

            Assert.AreEqual(0, offsets.Count);
        }

        [Test]
        public async Task Multiple_matches_are_found()
        {
            var searched = "Lorem";

            var text =
@"Lorem ipsum dolor sit amet, consectetur adipiscing elit. 
Etiam egestas leo urna, vel mollis urna sollicitudin non. 
Nam in orci ante. Praesent dapibus id purus sit amet molestie.
Maecenas erat ex, tempus vitae consectetur et, ullamcorper eget nibh. Nulla facilisi. 
Phasellus nec bibendum lorem. ALoremliquam ac est vitae neque placerat sollicitudin vitae nec odio. 
Aliquam cursus arcu quis magna feugiat, eget consectetur lorem accumsan.
Nam nibh est, aliquam eget ante ac, lacinia sceleLoremrisque eros. Phasellus commodo massa lacus. 
Ut aliquet fermentum tortor at egestas. Aliquam erat leo, auctor ut luctus sed,
aliquam id ligula. Cras eleifend sapien et tellus maximus, in fermentum leo
consectetur.Sed volutpat augue porttitor odLoremio dictum, et semper nisi tempor.
Quisque at aliquam turpis. Duis molestie lacus vitae eros tempor vehicula. ";

            var offsets = new List<int>() { 0, 302, 496, 748 };
            var offsetsResult = new List<int>() { };
            var document = new TextDocument(text);
            var token = new CancellationToken();
            Action<int> onFound = (int offset) => offsetsResult.Add(offset);
            var sut = new SearchJob(document.CreateReader(), searched, onFound);

            await sut.RunAsync(token);

            CollectionAssert.AreEquivalent(offsets, offsetsResult);
        }
    }
}
