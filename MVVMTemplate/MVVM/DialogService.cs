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
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        OnContentRendered();
                    });
                }
                else
                {
                    OnContentRendered();
                }
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
        public WindowStartupLocation DialogStartupLocation = WindowStartupLocation.CenterOwner;
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
        // Takes a ViewModel based user control and opens a window with that control as content. Provides the ability to set Application object ShutdownMode.
        public static WindowMessageResult OpenDialog(DialogBaseWindowViewModel viewmodel, Window parentWindow, ShutdownMode shutdownMode)
        {
            // This is used to prevent the Application.Current from going null. It can be turned off by setting ApplicationExplicitShutdown.
            // If this returns in this manner, you did something wrong and used this libary in correctly.
            if (Application.Current == null)
            {
                return WindowMessageResult.Undefined;
            }

            Application.Current.ShutdownMode = shutdownMode;
            DialogBaseWindow dialogBaseWindow = new DialogBaseWindow(viewmodel.dialogData);
            if (parentWindow != null)
            {
                dialogBaseWindow.Owner = parentWindow;
            }

            dialogBaseWindow.DataContext = viewmodel;
            dialogBaseWindow.ShowDialog();

            WindowMessageResult result = WindowMessageResult.Undefined;
            try
            {
                // This will allow for use of this method from threads outside the UI thread.
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    result = (dialogBaseWindow.DataContext as DialogBaseWindowViewModel).UserDialogResult;
                });
            }
            catch
            {
                // TODO (DB): This probably does not need to have anything here.
            }

            return result;
        }

        // Same as above but also provides but with a default Application object ShutdownMode set to ShutdownMode.OnLastWindowClose.
        public static WindowMessageResult OpenDialog(DialogBaseWindowViewModel viewmodel, Window parentWindow)
        {
            return OpenDialog(viewmodel, parentWindow, ShutdownMode.OnLastWindowClose);
        }

        // Opens Window Message based on DialogData and sets the owner of that window to the passed in paramater.
        public static WindowMessageResult OpenWindowMessage(DialogData data, Window parentWindow)
        {
            DialogBaseWindowViewModel viewmodel = new WindowsMessageViewModel(data);
            return OpenDialog(viewmodel, parentWindow, ShutdownMode.OnLastWindowClose);
        }

        // Opens Window Message based on DialogData and sets the owner of that window to the passed in paramater.
        public static WindowMessageResult OpenWindowMessage(DialogData data, Window parentWindow, ShutdownMode shutdownMode)
        {
            DialogBaseWindowViewModel viewmodel = new WindowsMessageViewModel(data);
            return OpenDialog(viewmodel, parentWindow, shutdownMode);
        }
    }
}
