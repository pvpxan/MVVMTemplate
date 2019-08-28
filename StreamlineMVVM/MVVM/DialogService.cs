using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace StreamlineMVVM
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
        public Brush Background { get; set; } = Brushes.White;

        public Brush ContentHeaderColor { get; set; } = Brushes.DarkBlue;
        public Brush ContentBodyColor { get; set; } = Brushes.Black;

        public Brush HyperLinkColor { get; set; } = Brushes.Blue;
        public Brush HyperLinkMouseOverColor { get; set; } = Brushes.Red;
        public Brush HyperLinkMouseDisabledColor { get; set; } = Brushes.Gray;
    }

    public class CustomWindowsMessageButtons
    {
        public string Custom1 { get; set; } = "";
        public string Custom2 { get; set; } = "";
        public string Custom3 { get; set; } = "";
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

        public Action OnContentRendered { get; set; }
    }

    public class DialogData
    {
        public Window ParentWindow { get; set; }

        public string WindowTitle { get; set; } = "";
        public Brush Background { get; set; } = Brushes.White;
        public bool Topmost { get; set; } = true;
        public WindowStyle DialogWindowStyle { get; set; } = WindowStyle.ToolWindow;
        public WindowStartupLocation DialogStartupLocation { get; set; } = WindowStartupLocation.CenterOwner;
        public string WindowIconURI { get; set; } = "";

        public bool RequireResult { get; set; } = false;
        public bool CancelAsync { get; set; } = false;

        public Brush ContentHeaderColor { get; set; } = Brushes.DarkBlue;
        public string ContentHeader { get; set; } = "";

        public Brush ContentBodyColor { get; set; } = Brushes.Black;
        public string ContentBody { get; set; } = "";

        public Brush HyperLinkColor { get; set; } = Brushes.Blue;
        public Brush HyperLinkMouseOverColor { get; set; } = Brushes.Red;
        public Brush HyperLinkMouseDisabledColor { get; set; } = Brushes.Gray;
        public string HyperLinkText { get; set; } = "";
        public string HyperLinkUri { get; set; } = "";

        public WindowMessageIcon MessageIcon { get; set; } = WindowMessageIcon.Information;
        public WindowMessageButtons MessageButtons { get; set; } = WindowMessageButtons.Ok;
        public CustomWindowsMessageButtons CustomButtoms { get; set; } = new CustomWindowsMessageButtons();

        // Program Specific Options
        public object CustomData { get; set; }
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
