using AutoFixture;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using XNotepad.UI.ViewModel;
using AutoFixture.AutoMoq;
using XNotepad.TestingUtilities;
using ICSharpCode.AvalonEdit.Document;

namespace XNotepad.UI.Tests.ViewModel
{
    [TestFixture]
    public class ViewModelTests
    {
        [Test]
        public void Every_created_viewmodel_has_all_commands_defined()
        {
            var fixture = new Fixture()
                //.Customize(new AutoMoqCustomization())
                .Customize(new SupportMutableValueTypesCustomization());

            var viewModelTypes = Assembly.GetAssembly(typeof(BaseViewModel)).GetTypes()
                .Where(x => x.IsSubclassOf(typeof(BaseViewModel)))
                .Where(x => x.IsClass)
                .Where(x => !x.IsAbstract)
                .ToList();

            Assert.Multiple(() => viewModelTypes.ForEach(type =>
            {
                var commandProperties = type.GetProperties()
                .Where(x => typeof(ICommand).IsAssignableFrom(x.PropertyType))
                .ToList();
                var instance = Activator.CreateInstance(type, null);
                var commands = commandProperties.Select(x => x.GetValue(instance));

                CollectionAssert.AllItemsAreNotNull(commands);
            }));
        }
    }
}
