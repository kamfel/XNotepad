using ICSharpCode.AvalonEdit;
using NLog;
using System;
using System.Windows;

namespace XNotepad.UI.Controls
{
    public class MvvmTextEditor : TextEditor
    {
        public int CurrentCaretOffset
        {
            get { return (int)GetValue(CaretOffsetProperty); }
            set { SetValue(CaretOffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CaretOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaretOffsetProperty =
            DependencyProperty.Register("CurrentCaretOffset", typeof(int), typeof(MvvmTextEditor), new PropertyMetadata());


        public int SelectionOffset
        {
            get { return base.SelectionStart; }
            set { SetValue(SelectionOffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectionOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionOffsetProperty =
            DependencyProperty.Register("SelectionOffset", typeof(int), typeof(MvvmTextEditor), new PropertyMetadata(SelectionOffsetChanged));

        private static void SelectionOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as MvvmTextEditor;
            obj.SelectionStart = (int)e.NewValue;
        }

        public int CurrentSelectionLength
        {
            get { return base.SelectionLength; }
            set { SetValue(CurrentSelectionLengthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectionLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentSelectionLengthProperty =
            DependencyProperty.Register("CurrentSelectionLength", typeof(int), typeof(MvvmTextEditor), new PropertyMetadata(SelectionLengthChanged));

        private static void SelectionLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as MvvmTextEditor;
            obj.Select(obj.SelectionStart, (int)e.NewValue);
        }

        public MvvmTextEditor()
        {
            this.TextArea.Caret.PositionChanged += Caret_PositionChanged;
        }

        private void Caret_PositionChanged(object sender, EventArgs e)
        {
            SetCurrentValue(CaretOffsetProperty, base.CaretOffset);
        }
    }
}
