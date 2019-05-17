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

namespace MVVMTemplate
{
    /// <summary>
    /// Interaction logic for WindowsMessage.xaml
    /// </summary>
    public partial class WindowsMessage : UserControl
    {
        private Window window = null;

        public WindowsMessage()
        {
            MainWindow main_window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            try
            {
                InitializeComponent();

                Loaded += Content_Loaded;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(main_window, "Window load error: " + Environment.NewLine + Convert.ToString(Ex), "Error...", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Content_Loaded(object sender, RoutedEventArgs e)
        {
            // Gets the Window object reference encapsulating this control.
            window = Window.GetWindow(this);

            // Get the viewmodel from the DataContext
            WindowsMessageViewModel viewmodel = DataContext as WindowsMessageViewModel;

            // Call command from viewmodel     
            if ((viewmodel != null) && (viewmodel.Loaded.CanExecute(window)))
            {
                viewmodel.Loaded.Execute(window);
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
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
        private Window window = null;
        // ---------------------------------------------------------------------------------------------------------------------------------------------

        // Constructor
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        public WindowsMessageViewModel(DialogData data) : base(data)
        {
            MessageImage = GetIcon(data.Icon);
            Caption = data.Caption;
            Content = data.Content;

            if (data.HyperLinkUri.Length > 0)
            {
                HyperLink_IsEnabled = true;
                HyperLink_Visibility = Visibility.Visible;

                HyperLink_Uri = data.HyperLinkUri;

                if (data.HyperLinkText.Length > 0)
                {
                    HyperLink_Text = data.HyperLinkText;
                }
                else
                {
                    HyperLink_Text = data.HyperLinkUri;
                }
            }

            Loaded = new RelayCommand(Loaded_Command);

            switch (data.Buttons)
            {
                case DialogButtons.Default:

                    break;

                case DialogButtons.Ok:

                    Center_Content = "Ok";
                    Center_IsEnabled = true;
                    Center_Visibility = Visibility.Visible;

                    Center_Button = new RelayCommand(Ok_Command);

                    break;

                case DialogButtons.OkCancel:

                    Left_Content = "Ok";
                    Left_IsEnabled = true;
                    Left_Visibility = Visibility.Visible;

                    Left_Button = new RelayCommand(Ok_Command);

                    Right_Content = "Cancel";
                    Right_IsEnabled = true;
                    Right_Visibility = Visibility.Visible;

                    Right_Button = new RelayCommand(Cancel_Command);

                    break;

                case DialogButtons.YesNo:

                    Left_Content = "Yes";
                    Left_IsEnabled = true;
                    Left_Visibility = Visibility.Visible;

                    Left_Button = new RelayCommand(Yes_Command);

                    Right_Content = "No";
                    Right_IsEnabled = true;
                    Right_Visibility = Visibility.Visible;

                    Right_Button = new RelayCommand(No_Command);

                    break;

                case DialogButtons.YesNoCancel:

                    Left_Content = "Yes";
                    Left_IsEnabled = true;
                    Left_Visibility = Visibility.Visible;

                    Left_Button = new RelayCommand(Yes_Command);

                    Center_Content = "No";
                    Center_IsEnabled = true;
                    Center_Visibility = Visibility.Visible;

                    Center_Button = new RelayCommand(No_Command);

                    Right_Content = "Cancel";
                    Right_IsEnabled = true;
                    Right_Visibility = Visibility.Visible;

                    Right_Button = new RelayCommand(Cancel_Command);

                    break;

                case DialogButtons.Exit:

                    Center_Content = "Exit";
                    Center_IsEnabled = true;
                    Center_Visibility = Visibility.Visible;

                    Center_Button = new RelayCommand(Exit_Command);

                    break;

                case DialogButtons.ContinueCancel:

                    Left_Content = "Continue";
                    Left_IsEnabled = true;
                    Left_Visibility = Visibility.Visible;

                    Left_Button = new RelayCommand(Continue_Command);

                    Right_Content = "Cancel";
                    Right_IsEnabled = true;
                    Right_Visibility = Visibility.Visible;

                    Right_Button = new RelayCommand(Cancel_Command);

                    break;

                case DialogButtons.AcceptDecline:

                    Left_Content = "Accept";
                    Left_IsEnabled = true;
                    Left_Visibility = Visibility.Visible;

                    Left_Button = new RelayCommand(Accept_Command);

                    Right_Content = "Decline";
                    Right_IsEnabled = true;
                    Right_Visibility = Visibility.Visible;

                    Right_Button = new RelayCommand(Decline_Command);

                    break;
            }
        }

        private BitmapSource GetIcon(DialogIcon icontype)
        {
            Icon icon = (Icon)typeof(SystemIcons).GetProperty(Convert.ToString(icontype), BindingFlags.Public | BindingFlags.Static).GetValue(null, null);

            return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------

        // Bound Variables
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------
        private BitmapSource _MessageImage;
        public BitmapSource MessageImage
        {
            get
            {
                return _MessageImage;
            }
            set
            {
                _MessageImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MessageImage"));
            }
        }

        // -----------------------------------------------------
        private string _Caption = "";
        public string Caption
        {
            get
            {
                return _Caption;
            }
            set
            {
                _Caption = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Caption"));
            }
        }

        // -----------------------------------------------------
        private string _Content = "";
        public string Content
        {
            get
            {
                return _Content;
            }
            set
            {
                _Content = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Content"));
            }
        }

        // -----------------------------------------------------
        private string _HyperLink_Uri = "";
        public string HyperLink_Uri
        {
            get
            {
                return _HyperLink_Uri;
            }
            set
            {
                _HyperLink_Uri = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HyperLink_Uri"));
            }
        }

        // -----------------------------------------------------
        private string _HyperLink_Text = "";
        public string HyperLink_Text
        {
            get
            {
                return _HyperLink_Text;
            }
            set
            {
                _HyperLink_Text = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HyperLink_Text"));
            }
        }

        // -----------------------------------------------------
        private string _Left_Content = "";
        public string Left_Content
        {
            get
            {
                return _Left_Content;
            }
            set
            {
                _Left_Content = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Left_Content"));
            }
        }

        // -----------------------------------------------------
        private bool _HyperLink_IsEnabled = false;
        public bool HyperLink_IsEnabled
        {
            get
            {
                return _HyperLink_IsEnabled;
            }
            set
            {
                _HyperLink_IsEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HyperLink_IsEnabled"));
            }
        }

        // -----------------------------------------------------
        private Visibility _HyperLink_Visibility = System.Windows.Visibility.Collapsed;
        public Visibility HyperLink_Visibility
        {
            get
            {
                return _HyperLink_Visibility;
            }
            set
            {
                _HyperLink_Visibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HyperLink_Visibility"));
            }
        }

        // -----------------------------------------------------
        private bool _Left_IsEnabled = false;
        public bool Left_IsEnabled
        {
            get
            {
                return _Left_IsEnabled;
            }
            set
            {
                _Left_IsEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Left_IsEnabled"));
            }
        }

        // -----------------------------------------------------
        private Visibility _Left_Visibility = System.Windows.Visibility.Hidden;
        public Visibility Left_Visibility
        {
            get
            {
                return _Left_Visibility;
            }
            set
            {
                _Left_Visibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Left_Visibility"));
            }
        }

        // -----------------------------------------------------
        private string _Center_Content = "";
        public string Center_Content
        {
            get
            {
                return _Center_Content;
            }
            set
            {
                _Center_Content = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Center_Content"));
            }
        }

        // -----------------------------------------------------
        private bool _Center_IsEnabled = false;
        public bool Center_IsEnabled
        {
            get
            {
                return _Center_IsEnabled;
            }
            set
            {
                _Center_IsEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Center_IsEnabled"));
            }
        }

        // -----------------------------------------------------
        private Visibility _Center_Visibility = System.Windows.Visibility.Hidden;
        public Visibility Center_Visibility
        {
            get
            {
                return _Center_Visibility;
            }
            set
            {
                _Center_Visibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Center_Visibility"));
            }
        }

        // -----------------------------------------------------
        private string _Right_Content = "";
        public string Right_Content
        {
            get
            {
                return _Right_Content;
            }
            set
            {
                _Right_Content = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Right_Content"));
            }
        }

        // -----------------------------------------------------
        private bool _Right_IsEnabled = false;
        public bool Right_IsEnabled
        {
            get
            {
                return _Right_IsEnabled;
            }
            set
            {
                _Right_IsEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Right_IsEnabled"));
            }
        }

        // -----------------------------------------------------
        private Visibility _Right_Visibility = System.Windows.Visibility.Hidden;
        public Visibility Right_Visibility
        {
            get
            {
                return _Right_Visibility;
            }
            set
            {
                _Right_Visibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Right_Visibility"));
            }
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------

        // Bound Commands
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------
        private ICommand _Loaded;
        public ICommand Loaded
        {
            get
            {
                return _Loaded;
            }
            set
            {
                _Loaded = value;
            }
        }
        private void Loaded_Command(object parameter)
        {
            window = parameter as Window;
        }

        // ------------------------------------------------------
        private ICommand _Left_Button;
        public ICommand Left_Button
        {
            get
            {
                return _Left_Button;
            }
            set
            {
                _Left_Button = value;
            }
        }

        // ------------------------------------------------------
        private ICommand _Center_Button;
        public ICommand Center_Button
        {
            get
            {
                return _Center_Button;
            }
            set
            {
                _Center_Button = value;
            }
        }

        // ------------------------------------------------------
        private ICommand _Right_Button;
        public ICommand Right_Button
        {
            get
            {
                return _Right_Button;
            }
            set
            {
                _Right_Button = value;
            }
        }

        // Special Command Handling
        // ------------------------------------------------------
        private void Ok_Command(object parameter)
        {
            CloseDialogWithResult(parameter as Window, WindowMessageResult.Ok);
        }

        private void Cancel_Command(object parameter)
        {
            CloseDialogWithResult(parameter as Window, WindowMessageResult.Cancel);
        }

        private void Yes_Command(object parameter)
        {
            CloseDialogWithResult(parameter as Window, WindowMessageResult.Yes);
        }

        private void No_Command(object parameter)
        {
            CloseDialogWithResult(parameter as Window, WindowMessageResult.No);
        }

        private void Exit_Command(object parameter)
        {
            CloseDialogWithResult(parameter as Window, WindowMessageResult.Exit);
        }

        private void Continue_Command(object parameter)
        {
            CloseDialogWithResult(parameter as Window, WindowMessageResult.Continue);
        }

        private void Accept_Command(object parameter)
        {
            CloseDialogWithResult(parameter as Window, WindowMessageResult.Accept);
        }

        private void Decline_Command(object parameter)
        {
            CloseDialogWithResult(parameter as Window, WindowMessageResult.Decline);
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------
    }
}
