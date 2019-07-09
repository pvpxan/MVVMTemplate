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

    public enum WindowMessageButtons
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

    public enum WindowMessageIcon
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

    public class WindowMessageColorSet
    {
        public Brush Background = Brushes.White;

        public Brush ContentHeaderColor = Brushes.DarkBlue;
        public Brush ContentBodyColor = Brushes.Black;

        public Brush HyperLinkColor = Brushes.Blue;
        public Brush HyperLinkMouseOverColor = Brushes.Red;
        public Brush HyperLinkMouseDisabledColor = Brushes.Gray;
    }

    public class CustomWindowsMessageButtons
    {
        public string Custom1 = "";
        public string Custom2 = "";
        public string Custom3 = "";
    }

    public class ControlContentRendered
    {
        public void ContentRendered()
        {
            if (OnContentRendered != null)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    OnContentRendered();
                });
            }
        }

        public Action OnContentRendered;
    }

    public class DialogData
    {
        public Window ParentWindow = null;

        public string WindowTitle = "";
        public Brush Background = Brushes.White;
        public bool Topmost = true;

        public WindowStyle DialogWindowStyle = WindowStyle.ToolWindow;
        public string WindowIconURI = "";

        public bool RequireResult = false;
        public bool CancelAsync = false;

        public Brush ContentHeaderColor = Brushes.DarkBlue;
        public string ContentHeader = "";

        public Brush ContentBodyColor = Brushes.Black;
        public string ContentBody = "";

        public Brush HyperLinkColor = Brushes.Blue;
        public Brush HyperLinkMouseOverColor = Brushes.Red;
        public Brush HyperLinkMouseDisabledColor = Brushes.Gray;
        public string HyperLinkText = "";
        public string HyperLinkUri = "";

        public WindowMessageIcon MessageIcon = WindowMessageIcon.Information;
        public WindowMessageButtons MessageButtons = WindowMessageButtons.Ok;
        public CustomWindowsMessageButtons CustomButtoms = new CustomWindowsMessageButtons();

        // Program Specific Options
        public object CustomData;
    }

    public static class DialogService
    {
        public static WindowMessageResult OpenDialog(DialogBaseWindowViewModel viewmodel, Window parentWindow)
        {
            DialogBaseWindow dialogBaseWindow = new DialogBaseWindow();
            if (parentWindow != null)
            {
                dialogBaseWindow.Owner = parentWindow;
            }

            dialogBaseWindow.DataContext = viewmodel;
            dialogBaseWindow.ShowDialog();
            WindowMessageResult result = (dialogBaseWindow.DataContext as DialogBaseWindowViewModel).UserDialogResult;
            return result;
        }
    }
}
