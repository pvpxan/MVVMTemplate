using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace StreamlineMVVM
{
    /// <summary>
    /// Interaction logic for WindowsMessage.xaml
    /// </summary>
    public partial class WindowsMessage : UserControl
    {
        public WindowsMessage()
        {
            try
            {
                InitializeComponent();
            }
            catch //(Exception Ex)
            {
                // TODO (DB): Create some sort of error log in a default windows directory.
            }
        }

        private void hyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            using (Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri)))
            {
                e.Handled = true;
            }
        }
    }

    public class WindowsMessageViewModel : DialogBaseWindowViewModel
    {
        // ViewModel Only Vars
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        // TODO (DB): There might be some functionality added that requires this section.
        // ---------------------------------------------------------------------------------------------------------------------------------------------

        // Bound Variables
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------
        private System.Windows.Media.Brush _Background = System.Windows.Media.Brushes.White;
        public System.Windows.Media.Brush Background
        {
            get
            {
                return _Background;
            }
            set
            {
                _Background = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Background"));
            }
        }

        // -----------------------------------------------------
        private BitmapSource _MessageIcon;
        public BitmapSource MessageIcon
        {
            get
            {
                return _MessageIcon;
            }
            set
            {
                _MessageIcon = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MessageIcon"));
            }
        }

        // -----------------------------------------------------
        private System.Windows.Media.Brush _ContentHeaderColor = System.Windows.Media.Brushes.DarkBlue;
        public System.Windows.Media.Brush ContentHeaderColor
        {
            get
            {
                return _ContentHeaderColor;
            }
            set
            {
                _ContentHeaderColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContentHeaderColor"));
            }
        }

        // -----------------------------------------------------
        private string _ContentHeader = "";
        public string ContentHeader
        {
            get
            {
                return _ContentHeader;
            }
            set
            {
                _ContentHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContentHeader"));
            }
        }

        // -----------------------------------------------------
        private Visibility _ContentBodyVisibility = System.Windows.Visibility.Collapsed;
        public Visibility ContentBodyVisibility
        {
            get
            {
                return _ContentBodyVisibility;
            }
            set
            {
                _ContentBodyVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContentBodyVisibility"));
            }
        }

        // -----------------------------------------------------
        private System.Windows.Media.Brush _ContentBodyColor = System.Windows.Media.Brushes.Black;
        public System.Windows.Media.Brush ContentBodyColor
        {
            get
            {
                return _ContentBodyColor;
            }
            set
            {
                _ContentBodyColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContentBodyColor"));
            }
        }

        // -----------------------------------------------------
        private string _ContentBody = "";
        public string ContentBody
        {
            get
            {
                return _ContentBody;
            }
            set
            {
                _ContentBody = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContentBody"));
            }
        }

        // -----------------------------------------------------
        private bool _HyperLinkIsEnabled = false;
        public bool HyperLinkIsEnabled
        {
            get
            {
                return _HyperLinkIsEnabled;
            }
            set
            {
                _HyperLinkIsEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HyperLinkIsEnabled"));
            }
        }

        // -----------------------------------------------------
        private Visibility _HyperLinkVisibility = System.Windows.Visibility.Collapsed;
        public Visibility HyperLinkVisibility
        {
            get
            {
                return _HyperLinkVisibility;
            }
            set
            {
                _HyperLinkVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HyperLinkVisibility"));
            }
        }

        // -----------------------------------------------------
        private System.Windows.Media.Brush _HyperLinkColor = System.Windows.Media.Brushes.Blue;
        public System.Windows.Media.Brush HyperLinkColor
        {
            get
            {
                return _HyperLinkColor;
            }
            set
            {
                _HyperLinkColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HyperLinkColor"));
            }
        }

        // -----------------------------------------------------
        private System.Windows.Media.Brush _HyperLinkMouseOverColor = System.Windows.Media.Brushes.Red;
        public System.Windows.Media.Brush HyperLinkMouseOverColor
        {
            get
            {
                return _HyperLinkMouseOverColor;
            }
            set
            {
                _HyperLinkMouseOverColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HyperLinkMouseOverColor"));
            }
        }

        // -----------------------------------------------------
        private System.Windows.Media.Brush _HyperLinkMouseDisabledColor = System.Windows.Media.Brushes.Gray;
        public System.Windows.Media.Brush HyperLinkMouseDisabledColor
        {
            get
            {
                return _HyperLinkMouseDisabledColor;
            }
            set
            {
                _HyperLinkMouseDisabledColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HyperLinkMouseDisabledColor"));
            }
        }

        // -----------------------------------------------------
        private string _HyperLinkUri = "";
        public string HyperLinkUri
        {
            get
            {
                return _HyperLinkUri;
            }
            set
            {
                _HyperLinkUri = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HyperLinkUri"));
            }
        }

        // -----------------------------------------------------
        private string _HyperLinkText = "";
        public string HyperLinkText
        {
            get
            {
                return _HyperLinkText;
            }
            set
            {
                _HyperLinkText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HyperLinkText"));
            }
        }

        // -----------------------------------------------------
        private string _LeftContent = "";
        public string LeftContent
        {
            get
            {
                return _LeftContent;
            }
            set
            {
                _LeftContent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LeftContent"));
            }
        }

        // -----------------------------------------------------
        private bool _LeftIsEnabled = false;
        public bool LeftIsEnabled
        {
            get
            {
                return _LeftIsEnabled;
            }
            set
            {
                _LeftIsEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LeftIsEnabled"));
            }
        }

        // -----------------------------------------------------
        private Visibility _LeftVisibility = System.Windows.Visibility.Hidden;
        public Visibility LeftVisibility
        {
            get
            {
                return _LeftVisibility;
            }
            set
            {
                _LeftVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LeftVisibility"));
            }
        }

        // -----------------------------------------------------
        private string _CenterContent = "";
        public string CenterContent
        {
            get
            {
                return _CenterContent;
            }
            set
            {
                _CenterContent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CenterContent"));
            }
        }

        // -----------------------------------------------------
        private bool _CenterIsEnabled = false;
        public bool CenterIsEnabled
        {
            get
            {
                return _CenterIsEnabled;
            }
            set
            {
                _CenterIsEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CenterIsEnabled"));
            }
        }

        // -----------------------------------------------------
        private Visibility _CenterVisibility = System.Windows.Visibility.Hidden;
        public Visibility CenterVisibility
        {
            get
            {
                return _CenterVisibility;
            }
            set
            {
                _CenterVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CenterVisibility"));
            }
        }

        // -----------------------------------------------------
        private string _RightContent = "";
        public string RightContent
        {
            get
            {
                return _RightContent;
            }
            set
            {
                _RightContent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RightContent"));
            }
        }

        // -----------------------------------------------------
        private bool _RightIsEnabled = false;
        public bool RightIsEnabled
        {
            get
            {
                return _RightIsEnabled;
            }
            set
            {
                _RightIsEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RightIsEnabled"));
            }
        }

        // -----------------------------------------------------
        private Visibility _RightVisibility = System.Windows.Visibility.Hidden;
        public Visibility RightVisibility
        {
            get
            {
                return _RightVisibility;
            }
            set
            {
                _RightVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RightVisibility"));
            }
        }

        // Bound Commands
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------
        public ICommand Loaded { get; private set; }
        private void loadedCommand(object parameter)
        {
            //TODO (DB): Possible add some loaded events.
        }

        // -----------------------------------------------------
        public ICommand Rendered { get; private set; }
        private void renderedCommand(object parameter)
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => { })).Wait();
            }
        }

        // -----------------------------------------------------
        public ICommand LeftButton { get; private set; }
        public ICommand CenterButton { get; private set; }
        public ICommand RightButton { get; private set; }

        // Special Command Handling
        // ------------------------------------------------------
        private void okCommand(object parameter)
        {
            CloseDialogWithResult(parameter as Window, WindowMessageResult.Ok);
        }

        private void cancelCommand(object parameter)
        {
            CloseDialogWithResult(parameter as Window, WindowMessageResult.Cancel);
        }

        private void yesCommand(object parameter)
        {
            CloseDialogWithResult(parameter as Window, WindowMessageResult.Yes);
        }

        private void noCommand(object parameter)
        {
            CloseDialogWithResult(parameter as Window, WindowMessageResult.No);
        }

        private void exitCommand(object parameter)
        {
            CloseDialogWithResult(parameter as Window, WindowMessageResult.Exit);
        }

        private void continueCommand(object parameter)
        {
            CloseDialogWithResult(parameter as Window, WindowMessageResult.Continue);
        }

        private void acceptCommand(object parameter)
        {
            CloseDialogWithResult(parameter as Window, WindowMessageResult.Accept);
        }

        private void declineCommand(object parameter)
        {
            CloseDialogWithResult(parameter as Window, WindowMessageResult.Decline);
        }

        private void custom1Command(object parameter)
        {
            CloseDialogWithResult(parameter as Window, WindowMessageResult.Custom1);
        }

        private void custom2Command(object parameter)
        {
            CloseDialogWithResult(parameter as Window, WindowMessageResult.Custom2);
        }

        private void custom3Command(object parameter)
        {
            CloseDialogWithResult(parameter as Window, WindowMessageResult.Custom3);
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------

        // Constructor
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        public WindowsMessageViewModel(DialogData data) : base(data)
        {
            Background = data.Background;
            MessageIcon = GetIcon(data.MessageIcon);
            ContentHeader = data.ContentHeader;

            if (data.ContentBody.Length > 0)
            {
                ContentBodyVisibility = Visibility.Visible;
                ContentBody = data.ContentBody;
            }

            if (data.HyperLinkUri.Length > 0)
            {
                HyperLinkIsEnabled = true;
                HyperLinkVisibility = Visibility.Visible;

                HyperLinkUri = data.HyperLinkUri;

                if (data.HyperLinkText.Length > 0)
                {
                    HyperLinkText = data.HyperLinkText;
                }
                else
                {
                    HyperLinkText = data.HyperLinkUri;
                }
            }

            Loaded = new RelayCommand(loadedCommand);
            Rendered = new RelayCommand(renderedCommand);

            // Subscribes to the Rendered Event of the DialogBaseWindow that sets this control as its datacontext.
            WindowRenderedEvent.OnContentRendered += delegate { renderedCommand(DialogWindow); };

            switch (data.MessageButtons)
            {
                case WindowMessageButtons.Default:

                    break;

                case WindowMessageButtons.Ok:

                    CenterContent = "Ok";
                    CenterIsEnabled = true;
                    CenterVisibility = Visibility.Visible;

                    CenterButton = new RelayCommand(okCommand);

                    break;

                case WindowMessageButtons.OkCancel:

                    LeftContent = "Ok";
                    LeftIsEnabled = true;
                    LeftVisibility = Visibility.Visible;

                    LeftButton = new RelayCommand(okCommand);

                    RightContent = "Cancel";
                    RightIsEnabled = true;
                    RightVisibility = Visibility.Visible;

                    RightButton = new RelayCommand(cancelCommand);

                    break;

                case WindowMessageButtons.YesNo:

                    LeftContent = "Yes";
                    LeftIsEnabled = true;
                    LeftVisibility = Visibility.Visible;

                    LeftButton = new RelayCommand(yesCommand);

                    RightContent = "No";
                    RightIsEnabled = true;
                    RightVisibility = Visibility.Visible;

                    RightButton = new RelayCommand(noCommand);

                    break;

                case WindowMessageButtons.YesNoCancel:

                    LeftContent = "Yes";
                    LeftIsEnabled = true;
                    LeftVisibility = Visibility.Visible;

                    LeftButton = new RelayCommand(yesCommand);

                    CenterContent = "No";
                    CenterIsEnabled = true;
                    CenterVisibility = Visibility.Visible;

                    CenterButton = new RelayCommand(noCommand);

                    RightContent = "Cancel";
                    RightIsEnabled = true;
                    RightVisibility = Visibility.Visible;

                    RightButton = new RelayCommand(cancelCommand);

                    break;

                case WindowMessageButtons.Exit:

                    CenterContent = "Exit";
                    CenterIsEnabled = true;
                    CenterVisibility = Visibility.Visible;

                    CenterButton = new RelayCommand(exitCommand);

                    break;

                case WindowMessageButtons.ContinueCancel:

                    LeftContent = "Continue";
                    LeftIsEnabled = true;
                    LeftVisibility = Visibility.Visible;

                    LeftButton = new RelayCommand(continueCommand);

                    RightContent = "Cancel";
                    RightIsEnabled = true;
                    RightVisibility = Visibility.Visible;

                    RightButton = new RelayCommand(cancelCommand);

                    break;

                case WindowMessageButtons.AcceptDecline:

                    LeftContent = "Accept";
                    LeftIsEnabled = true;
                    LeftVisibility = Visibility.Visible;

                    LeftButton = new RelayCommand(acceptCommand);

                    RightContent = "Decline";
                    RightIsEnabled = true;
                    RightVisibility = Visibility.Visible;

                    RightButton = new RelayCommand(declineCommand);

                    break;

                case WindowMessageButtons.Custom:

                    if (data.CustomButtoms.Custom1.Length > 0)
                    {
                        LeftContent = data.CustomButtoms.Custom1;
                        LeftIsEnabled = true;
                        LeftVisibility = Visibility.Visible;

                        LeftButton = new RelayCommand(custom1Command);
                    }

                    if (data.CustomButtoms.Custom2.Length > 0)
                    {
                        CenterContent = data.CustomButtoms.Custom2;
                        CenterIsEnabled = true;
                        CenterVisibility = Visibility.Visible;

                        CenterButton = new RelayCommand(custom2Command);
                    }

                    if (data.CustomButtoms.Custom3.Length > 0)
                    {
                        RightContent = data.CustomButtoms.Custom3;
                        RightIsEnabled = true;
                        RightVisibility = Visibility.Visible;

                        RightButton = new RelayCommand(custom3Command);
                    }

                    break;
            }
        }

        private BitmapSource GetIcon(WindowMessageIcon icontype)
        {
            Icon icon = (Icon)typeof(SystemIcons).GetProperty(Convert.ToString(icontype), BindingFlags.Public | BindingFlags.Static).GetValue(null, null);

            return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------
    }
}
