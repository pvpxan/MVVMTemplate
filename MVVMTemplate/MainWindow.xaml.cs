using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MVVMTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();

                Statics.MainWindow = this;

                Loaded += contentLoaded;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, "Window load error: " + Environment.NewLine + Convert.ToString(Ex), "Error...", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void contentLoaded(object sender, RoutedEventArgs e)
        {
            // Get the viewmodel from the DataContext
            MainWindowViewModel viewmodel = DataContext as MainWindowViewModel;

            //Call command from viewmodel     
            if ((viewmodel != null) && (viewmodel.Loaded.CanExecute(this)))
            {
                viewmodel.Loaded.Execute(this);
            }
        }
    }

    public class MainWindowViewModel : ViewModelBase
    {
        // ViewModel Only Vars
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        private Window window = null;
        // ---------------------------------------------------------------------------------------------------------------------------------------------

        // Constructor
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        public MainWindowViewModel()
        {
            Loaded = new RelayCommand(Loaded_Command);
            OpenDialog = new RelayCommand(OpenDialogCommand);
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
        private ICommand _OpenDialog;
        public ICommand OpenDialog
        {
            get
            {
                return _OpenDialog;
            }
            set
            {
                _OpenDialog = value;
            }
        }
        private void OpenDialogCommand(object parameter)
        {
            ExampleOne(parameter);
            ExampleTwo(parameter);
        }

        // Usage examples:
        // Windows like dialog:
        // NOTE: See the DialogData class above for more options. The method used below and even direct calling to open a dialog requires a window where the dialog will open.
        public static void ExampleOne(object parameter)
        {
            DialogData data = new DialogData()
            {
                Title = "Example...",
                Caption = "This is a sample message...",
                Content = "The is the body of the dialog.",
                DialogWindowStyle = WindowStyle.SingleBorderWindow,
                WindowIconURI = "pack://application:,,,/Resources/gear_icon2.ico",
                Buttons = DialogButtons.Ok,
            };

            Statics.OpenWindowsDialog(data, parameter as Window);
        }

        // Custom dialog:
        public static void ExampleTwo(object parameter)
        {
            DialogData data = new DialogData()
            {
                Title = "Example...",
                DialogWindowStyle = WindowStyle.SingleBorderWindow,
                WindowIconURI = "pack://application:,,,/Resources/gear_icon2.ico",
                Topmost = true,
            };

            DialogBaseWindowViewModel viewmodel = new CustomDialogViewModel(data);
            WindowMessageResult result = DialogService.OpenDialog(viewmodel, parameter as Window);
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------
    }
}
