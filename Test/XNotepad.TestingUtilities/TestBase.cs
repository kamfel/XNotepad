using AutoFixture;
using AutoFixture.AutoMoq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNotepad.TestingUtilities
{
    public class TestBase
    {
        protected IFixture fixture;

        protected TestBase()
        {
            this.fixture = new Fixture();

            this.fixture.Customize(new AutoMoqCustomization());
        }

        protected T Any<T>()
        {
            return this.fixture.Create<T>();
        }
    }
}
