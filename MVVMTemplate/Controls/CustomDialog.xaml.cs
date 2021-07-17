using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for CustomDialog.xaml
    /// </summary>
    public partial class CustomDialog : UserControl
    {
        private Window window = null;

        public CustomDialog(DialogData dialogData)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            try
            {
                this.DataContext = new CustomDialogViewModel(dialogData);
                InitializeComponent();

                Loaded += contentLoaded;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(mainWindow, "Control load error: " + Environment.NewLine + Convert.ToString(Ex), "Error...", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void contentLoaded(object sender, RoutedEventArgs e)
        {
            // Gets the Window object reference encapsulating this control.
            window = Window.GetWindow(this);

            // Subscribes to the Rendered Event of the DialogBaseWindow that sets this control as its datacontext.
            window.ContentRendered += contentRendered;

            // Get the viewmodel from the DataContext
            CustomDialogViewModel viewmodel = DataContext as CustomDialogViewModel;

            // Call command from viewmodel     
            if ((viewmodel != null) && (viewmodel.Loaded.CanExecute(window)))
            {
                viewmodel.Loaded.Execute(window);
            }
        }

        private void contentRendered(object sender, EventArgs e)
        {
            // Get the viewmodel from the DataContext
            CustomDialogViewModel viewmodel = DataContext as CustomDialogViewModel;

            // Call command from viewmodel     
            if ((viewmodel != null) && (viewmodel.Rendered.CanExecute(null)))
            {
                viewmodel.Rendered.Execute(null);
            }
        }
    }

    public class CustomDialogViewModel : DialogViewModel
    {
        // ViewModel Only Vars
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        private Window window = null;
        // ---------------------------------------------------------------------------------------------------------------------------------------------

        // Constructor
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        public CustomDialogViewModel(DialogData data) : base(data)
        {
            DataMessaging dataMessaging = new DataMessaging();
            dataMessaging.OnDataTransmittedEvent += onDialogDataReceived;
            Loaded = new RelayCommand(Loaded_Command);
            Rendered = new RelayCommand(Rendered_Command);
        }

        private void onDialogDataReceived(DialogData data)
        {
            
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------

        // Bound Variables
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------
        private string _UIMessage = "UI Message...";
        public string UIMessage
        {
            get
            {
                return _UIMessage;
            }
            set
            {
                _UIMessage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UIMessage"));
            }
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------

        // Bound ICommands
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

        // -----------------------------------------------------
        private ICommand _Rendered;
        public ICommand Rendered
        {
            get
            {
                return _Rendered;
            }
            set
            {
                _Rendered = value;
            }
        }
        private void Rendered_Command(object parameter)
        {

        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------
    }
}
