using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using XNotepad.Core.Utilities;

namespace XNotepad.Core.Tests.Utilities
{
    [TestFixture()]
    public class FileUtilitiesTests
    {
        [TestCase(@"C:\folder\file.txt", @"file.txt")]
        [TestCase(@"C:\folder\file", @"file")]
        [TestCase(@"abc.txt", @"abc.txt")]
        [TestCase(@"file", @"file")]
        public void ExtractFileNameFromFullPath_ValidFilePath_ReturnsFileName(string filepath, string expected)
        {
            var result = FileUtilities.ExtractFileNameFromFullPath(filepath);

            Assert.AreEqual(expected, result);
        }

        [TestCase(null)]
        [TestCase(@"C]]]:\[][]231folder\abc.txt")]
        [TestCase(@":\folder\abc.txt")]
        [TestCase(@"???Se``der\abc.txt")]
        public void ExtractFileNameFromFullPath_InvalidFilePath_ReturnsEmpty(string filepath)
        {
            var result = FileUtilities.ExtractFileNameFromFullPath(filepath);

            Assert.IsEmpty(result);
        }

        [TestCase(@"C:\folder\file.txt")]
        [TestCase(@"D:\file.txt")]
        public void ValidateFilePath_ValidFilePath_ReturnTrue(string filepath)
        {
            var result = FileUtilities.ValidateFilePath(filepath);

            Assert.IsTrue(result);
        }

        [TestCase(@"C:\folder\file")]
        [TestCase(@"C:\")]
        public void ValidateFilePath_ValidDirectory_ReturnTrue(string directory)
        {
            var result = FileUtilities.ValidateFilePath(directory);

            Assert.IsTrue(result);
        }

        [TestCase(@":\:\folder\file.txt")]
        [TestCase(@"fo:lder\file.txt")]
        [TestCase(@"a:*.xts")]
        [TestCase(null)]
        public void ValidateFilePath_InvalidFilePath_ReturnsFalse(string filepath)
        {
            var result = FileUtilities.ValidateFilePath(filepath);

            Assert.IsFalse(result);
        }

        [TestCase(@":\folder\file")]
        [TestCase(@"fol???der\file")]
        [TestCase(@"ab:c")]
        [TestCase(null)]
        public void ValidateFilePath_InvalidDirectory_ReturnFalse(string directory)
        {
            var result = FileUtilities.ValidateFilePath(directory);

            Assert.IsFalse(result);
        }
    }
}
