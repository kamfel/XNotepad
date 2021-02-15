using System;
using AutoFixture;
using AutoFixture.NUnit3;
using AutoFixture.AutoMoq;

namespace XNotepad.TestingUtilities
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(() => new Fixture()
                .Customize(new AutoMoqCustomization()))
        {
        }
    }
}
