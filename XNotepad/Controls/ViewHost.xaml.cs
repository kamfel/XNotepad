using System;
using System.Windows.Controls;
using System.Windows.Input;
using XNotepad.Core.Interfaces;

namespace XNotepad.UI.Controls
{
    /// <summary>
    /// Interaction logic for ViewHost.xaml
    /// </summary>
    public partial class ViewHost : UserControl, ICloseable
    {
        private bool isFocused;
        private double notFocusedOpacity = 0.4;
        private double focusedOpacity = 1;

        public ViewHost()
        {
            InitializeComponent();
        }

        public void Close()
        {
            this.DataContext = null;
        }

        private void ContentGotFocus(object sender, EventArgs e)
        {
            this.Opacity = focusedOpacity;
            this.isFocused = true;
        }

        private void ContentLostFocus(object sender, EventArgs e)
        {
            this.Opacity = notFocusedOpacity;
            this.isFocused = false;
        }

        private void ContentMouseEnter(object sender, MouseEventArgs e)
        {
            this.Opacity = focusedOpacity;
        }

        private void ContentMouseLeave(object sender, MouseEventArgs e)
        {
            if (!this.isFocused)
            {
                this.Opacity = notFocusedOpacity;
            }
        }
    }
}
