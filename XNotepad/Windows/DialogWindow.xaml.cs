using System.Windows;
using System.Windows.Media;
using XNotepad.Core.Interfaces;

namespace XNotepad.UI.Windows
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window, ICloseable
    {
        public DialogWindow()
        {
            InitializeComponent();
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.5;
            //this.Background = Brushes.Transparent;
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            this.Opacity = 1;
            this.Background = Brushes.White;
        }
    }
}
