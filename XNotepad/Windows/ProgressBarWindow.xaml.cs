using System.Windows;
using XNotepad.Core.Interfaces;

namespace XNotepad.UI.Windows
{
    /// <summary>
    /// Interaction logic for ProgressBarWindow.xaml
    /// </summary>
    public partial class ProgressBarWindow : Window, ICloseable
    {
        public ProgressBarWindow(string message = null)
        {
            InitializeComponent();
            this.loadingMessageTextBlock.Text = message ?? "Loading...";
        }
    }
}
