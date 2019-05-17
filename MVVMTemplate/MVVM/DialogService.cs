using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MVVMTemplate
{
    public enum WindowMessageResult
    {
        Undefined,
        Yes,
        No,
        Ok,
        Continue,
        Cancel,
        Exit,
        Accept,
        Decline,
        Error,
        Custom1,
        Custom2,
        Custom3,
    }

    public enum DialogButtons
    {
        Default,
        Ok,
        OkCancel,
        YesNo,
        YesNoCancel,
        Exit,
        ContinueCancel,
        AcceptDecline,
        Custom,
    }

    public enum DialogIcon
    {
        Application,
        Asterisk,
        Error,
        Exclamation,
        Hand,
        Information,
        Question,
        Shield,
        Warning,
        WinLogo
    }

    public class CustomDialogButtons
    {
        public string Custom1 = "";
        public string Custom2 = "";
        public string Custom3 = "";
    }

    public class DialogData
    {
        public string Title = "";
        public Brush Background = Brushes.White;
        public bool Topmost = true;

        public WindowStyle DialogWindowStyle = WindowStyle.ToolWindow;
        public string WindowIconURI = "";

        public bool RequireResult = false; // Tells the dialog if the X in the corner can be used or not to close.
        public bool CancelAsync = false; // Janky: When running async operations on the dialog, you can use this bool as a way to communicate to threads that the window is closing.

        public string Content = "";
        public string Caption = "";

        public string HyperLinkText = "";
        public string HyperLinkUri = "";

        public DialogIcon Icon = DialogIcon.WinLogo;

        public DialogButtons Buttons = DialogButtons.Default;
        public CustomDialogButtons CustomButtoms = new CustomDialogButtons();

        // Program specific options can also be added.
    }

    public static class DialogService
    {
        public static WindowMessageResult OpenDialog(DialogBaseWindowViewModel viewmodel, Window owner)
        {
            DialogBaseWindow dialog_window = new DialogBaseWindow();
            if (owner != null)
            {
                dialog_window.Owner = owner;
            }

            dialog_window.DataContext = viewmodel;
            dialog_window.ShowDialog();
            WindowMessageResult result = (dialog_window.DataContext as DialogBaseWindowViewModel).UserDialogResult;
            return result;
        }
    }
}
