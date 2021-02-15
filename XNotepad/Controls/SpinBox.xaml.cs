using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace XNotepad.UI.Controls
{
    /// <summary>
    /// Interaction logic for SpinBox.xaml
    /// </summary>
    public partial class SpinBox : UserControl
    {
        public Geometry UpPath { get; set; } = Geometry.Parse("M177 159.7l136 136c9.4 9.4 9.4 24.6 0 33.9l-22.6 22.6c-9.4 9.4-24.6 9.4-33.9 0L160 255.9l-96.4 96.4c-9.4 9.4-24.6 9.4-33.9 0L7 329.7c-9.4-9.4-9.4-24.6 0-33.9l136-136c9.4-9.5 24.6-9.5 34-.1z");
        public Geometry DownPath { get; set; } = Geometry.Parse("M143 352.3L7 216.3c-9.4-9.4-9.4-24.6 0-33.9l22.6-22.6c9.4-9.4 24.6-9.4 33.9 0l96.4 96.4 96.4-96.4c9.4-9.4 24.6-9.4 33.9 0l22.6 22.6c9.4 9.4 9.4 24.6 0 33.9l-136 136c-9.2 9.4-24.4 9.4-33.8 0z");

        public string StringValue
        {
            get
            {
                return GetValue(ValueProperty).ToString();
            }
            set
            {
                if (int.TryParse(value, out var val))
                {
                    SetValue(ValueProperty, val);
                }
            }
        }

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(SpinBox), new PropertyMetadata(0, ValueChanged));

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue > 999 || (int)e.NewValue < 0)
                d.SetValue(ValueProperty, e.OldValue);
        }

        public SpinBox()
        {
            InitializeComponent();

            this.UpPath = Geometry.Parse("M177 159.7l136 136c9.4 9.4 9.4 24.6 0 33.9l-22.6 22.6c-9.4 9.4-24.6 9.4-33.9 0L160 255.9l-96.4 96.4c-9.4 9.4-24.6 9.4-33.9 0L7 329.7c-9.4-9.4-9.4-24.6 0-33.9l136-136c9.4-9.5 24.6-9.5 34-.1z");

            this.DownPath = Geometry.Parse("M143 352.3L7 216.3c-9.4-9.4-9.4-24.6 0-33.9l22.6-22.6c9.4-9.4 24.6-9.4 33.9 0l96.4 96.4 96.4-96.4c9.4-9.4 24.6-9.4 33.9 0l22.6 22.6c9.4 9.4 9.4 24.6 0 33.9l-136 136c-9.2 9.4-24.4 9.4-33.8 0z");
        }

        #region Handlers

        private void ButtonUp_Click(object sender, RoutedEventArgs e) => Value++;
        private void ButtonDown_Click(object sender, RoutedEventArgs e) => Value--;

        #endregion
    }
}
