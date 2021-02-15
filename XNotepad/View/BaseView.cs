using System.Windows;
using System.Windows.Controls;

namespace XNotepad.UI.View
{
    public class BaseView : UserControl
    {
        public void Close(bool? dialogResult = null)
        {
            var window = Window.GetWindow(this);
            window.DialogResult = dialogResult;
            window.Close();
        }
    }
}
