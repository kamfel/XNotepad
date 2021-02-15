using System.Windows;
using System.Windows.Controls;

namespace XNotepad.UI.Controls
{
    /// <summary>
    /// Interaction logic for Footer.xaml
    /// </summary>
    public partial class Footer : UserControl
    {
        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string), typeof(Footer), new PropertyMetadata("<No filepath>"));

        public string FileState
        {
            get { return (string)GetValue(FileStateProperty); }
            set { SetValue(FileStateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileStateProperty =
            DependencyProperty.Register("FileState", typeof(string), typeof(Footer), new PropertyMetadata("<No state>"));

        public int LineNumber
        {
            get { return (int)GetValue(LineNumberProperty); }
            set { SetValue(LineNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LineNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineNumberProperty =
            DependencyProperty.Register("LineNumber", typeof(int), typeof(Footer), new PropertyMetadata(0));

        public int ColumnNumber
        {
            get { return (int)GetValue(ColumnNumberProperty); }
            set { SetValue(ColumnNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColumnNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnNumberProperty =
            DependencyProperty.Register("ColumnNumber", typeof(int), typeof(Footer), new PropertyMetadata(0));

        public bool IsChangingState
        {
            get { return (bool)GetValue(IsChangingStateProperty); }
            set { SetValue(IsChangingStateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsChangingState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsChangingStateProperty =
            DependencyProperty.Register("IsChangingState", typeof(bool), typeof(Footer), new PropertyMetadata(false));

        public string EncodingName
        {
            get { return (string)GetValue(EncodingNameProperty); }
            set { SetValue(EncodingNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EncodingName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EncodingNameProperty =
            DependencyProperty.Register("EncodingName", typeof(string), typeof(Footer), new PropertyMetadata("<No encoding>"));

        public Footer()
        {
            InitializeComponent();
        }
    }
}
