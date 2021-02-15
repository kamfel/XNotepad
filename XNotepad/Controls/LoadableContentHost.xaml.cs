﻿using Bindables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace XNotepad.UI.Controls
{
    /// <summary>
    /// Interaction logic for LoadableContentHost.xaml
    /// </summary>
    [ContentProperty(nameof(UserContent))]
    public partial class LoadableContentHost : UserControl
    {
        [DependencyProperty]
        public bool Loading { get; set; }

        public Geometry Spinner { get; }

        [DependencyProperty]
        public FrameworkElement UserContent { get; set; }

        public LoadableContentHost()
        {
            this.Spinner = Geometry.Parse("M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z");

            InitializeComponent();
        }
    }
}
