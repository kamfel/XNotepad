using Ninject;
using NLog;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using XNotepad.Core;
using XNotepad.Core.File;
using XNotepad.Core.Threading;
using XNotepad.Core.UI;
using XNotepad.UI;
using XNotepad.UI.Commands;
using XNotepad.UI.ViewModel;

namespace XNotepad
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel kernel;

        public App()
        {
            this.EnsureEnglishCulture();
            this.ConfigureDI();
            this.StartJobRunner();
            this.ShowMainWindow();
        }

        private void ConfigureDI()
        {
            this.kernel = new StandardKernel(
                new ViewModelModule(),
                new CommandModule(),
                new CoreModule());

            kernel.Bind<IWindowManager>().To<WindowManager>().InSingletonScope();
            kernel.Bind<IFontManager>().To<FontManager>().InSingletonScope();
        }

        private void EnsureEnglishCulture()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                        XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        private void StartJobRunner()
        {
            JobRunner.Start(LogManager.GetLogger("JobRunner"));
        }

        private void ShowMainWindow()
        {
            var window = new MainWindow();
            window.DataContext = this.kernel.Get<MainWindowViewModel>();

            window.Closing += Window_Closing;

            window.ShowDialog();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var windowManager = kernel.Get<IWindowManager>();
            var autoSaveManager = kernel.Get<IAutoSaveManager>();

            windowManager.DoWorkWithProgress("Finishing background work...", async () =>
            {
                await autoSaveManager.Disable();

                await JobRunner.StopAsync();
            });
        }
    }
}
