using Microsoft.Win32;
using NLog;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFCustomMessageBox;
using XNotepad.Core.Enums;
using XNotepad.Core.UI;
using XNotepad.UI.Resources;
using XNotepad.UI.Windows;

namespace XNotepad.UI
{
    public class WindowManager : IWindowManager
    {
        public string ShowFileDialog(DialogParameters parameters)
        {
            var extensions = FileExtension.Dictionary[FileExtensionEnum.All];

            if (parameters.FileExtensions.Count != 0)
            {
                var ext = parameters.FileExtensions.Select(x => FileExtension.Dictionary[x]);
                extensions = string.Join("|", ext);
            }

            var dialog = new OpenFileDialog()
            {
                CheckPathExists = true,
                CheckFileExists = parameters.FileMustExist,
                ValidateNames = true,
                Filter = extensions,
            };

            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            else
            {
                return null;
            }
        }

        public bool? ShowModal(object dataContext)
        {
            var window = new DialogWindow();
            window.DataContext = dataContext;
            window.Owner = Application.Current.MainWindow;
            return window.ShowDialog();
        }

        public void ShowModeless(object dataContext)
        {
            var window = new DialogWindow();
            window.DataContext = dataContext;
            window.Show();
        }

        public MessageBoxResult ShowMessage(MessageViewModel model)
        {
            switch (model.Buttons)
            {
                case MessageBoxButton.OK:
                    return CustomMessageBox.ShowOK(
                        model.Message,
                        model.Title,
                        model.OkText ?? MsgBoxStrings.Ok,
                        model.Icon);
                case MessageBoxButton.OKCancel:
                    return CustomMessageBox.ShowOKCancel(
                        model.Message,
                        model.Title,
                        model.OkText ?? MsgBoxStrings.Ok,
                        model.CancelText ?? MsgBoxStrings.Cancel,
                        model.Icon);
                case MessageBoxButton.YesNoCancel:
                    return CustomMessageBox.ShowYesNoCancel(
                        model.Message,
                        model.Title,
                        model.OkText ?? MsgBoxStrings.Ok,
                        model.NoText ?? MsgBoxStrings.No,
                        model.CancelText ?? MsgBoxStrings.Cancel,
                        model.Icon);
                case MessageBoxButton.YesNo:
                    return CustomMessageBox.ShowYesNo(
                        model.Message,
                        model.Title,
                        model.OkText ?? MsgBoxStrings.Ok,
                        model.NoText ?? MsgBoxStrings.No,
                        model.Icon);
                default:
                    throw new ArgumentException("Unknown enum value.", nameof(model.Buttons));
            }
        }

        public void DoWorkWithProgress(string message, Func<Task> work)
        {
            var progress = new ProgressBarWindow(message);
            progress.Owner = Application.Current.MainWindow;

            progress.Loaded += async (s, e) =>
            {
                try
                {
                    await work();
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger("ProgressWork").Error(ex);
                    this.ShowMessage(new MessageViewModel()
                    {
                        Buttons = MessageBoxButton.OK,
                        Title = "Error",
                        Message = "An unexpected error occured."
                    });
                }
                finally
                {
                    (s as Window).Close();
                }
            };

            progress.ShowDialog();
        }
    }
}
