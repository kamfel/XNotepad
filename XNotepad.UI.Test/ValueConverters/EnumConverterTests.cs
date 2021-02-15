using NUnit.Framework;
using XNotepad.UI.ValueConverters;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using System.Globalization;

namespace XNotepad.UI.ValueConverters.Tests
{
    [TestFixture()]
    public class EnumConverterTests
    {
        private EnumConverter enumConverter;

        [SetUp]
        public void SetUp()
        {
            enumConverter = new EnumConverter();
        }

        [Test()]
        public void ShouldBeValidMarkupExtension()
        {
            var markupObject = enumConverter.ProvideValue(Mock.Of<IServiceProvider>());

            Assert.IsNotNull(markupObject);
        }
    }
}