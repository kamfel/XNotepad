using AutoFixture;
using AutoFixture.Kernel;
using System;

namespace XNotepad.TestingUtilities
{
    public static class FixtureExtensions
    {
        public static object Create(this IFixture fixture, Type type)
        {
            var context = new SpecimenContext(fixture);
            return context.Resolve(type);
        }

    }
}
