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
using System.Windows.Shapes;

namespace MVVMTemplate
{
    /// <summary>
    /// Interaction logic for DialogBaseWindow.xaml
    /// </summary>
    public partial class DialogBaseWindow : Window
    {
        private DialogBaseWindowViewModel dialogBaseWindowViewModel = null;

        public DialogBaseWindow()
        {
            if (Application.Current == null)
            {
                MessageBox.Show(this, "Window load error: " + Environment.NewLine + "The Application object has been shutdown or is null.", "Error...", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                InitializeComponent();
                Loaded += contentLoaded;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, "Window load error: " + Environment.NewLine + Convert.ToString(Ex), "Error...", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void contentLoaded(object sender, RoutedEventArgs e)
        {
            dialogBaseWindowViewModel = DataContext as DialogBaseWindowViewModel;
            dialogBaseWindowViewModel.DialogWindow = this;

            ContentRendered += contentRendered;
            Closing += onWindowClosing;
        }

        private void contentRendered(object sender, EventArgs e)
        {
            dialogBaseWindowViewModel.WindowRenderedEvent.ContentRendered();
        }

        private void onWindowClosing(object sender, CancelEventArgs e)
        {
            if (dialogBaseWindowViewModel.RequireResult)
            {
                if (dialogBaseWindowViewModel.UserDialogResult == dialogBaseWindowViewModel.DefaultDialogResult)
                {
                    e.Cancel = true;
                }
            }
        }
    }

    public class DialogBaseWindowViewModel : ViewModelBase
    {
        public string WindowTitle { get; private set; }
        public bool Topmost { get; private set; }
        public WindowStartupLocation DialogStartupLocation { get; set; }

        public WindowStyle DialogWindowStyle { get; private set; }
        public ImageSource WindowIcon { get; private set; }

        public bool RequireResult { get; private set; }
        public bool CancelAsync { get; private set; }

        public WindowMessageResult UserDialogResult { get; private set; }
        public WindowMessageResult DefaultDialogResult = WindowMessageResult.Undefined;

        public Window DialogWindow = null;
        public ControlContentRendered WindowRenderedEvent = new ControlContentRendered();

        public DialogBaseWindowViewModel(DialogData data)
        {
            WindowTitle = data.WindowTitle;
            Topmost = data.Topmost;
            DialogStartupLocation = data.DialogStartupLocation;
            DialogWindowStyle = data.DialogWindowStyle;

            try
            {
                if (data.WindowIconURI.Length > 0)
                {
                    WindowIcon = BitmapFrame.Create(new Uri(data.WindowIconURI, UriKind.RelativeOrAbsolute));
                }
                else
                {
                    WindowIcon = Application.Current.MainWindow.Icon;
                }
            }
            catch
            {
                WindowIcon = Application.Current.MainWindow.Icon;
            }

            RequireResult = data.RequireResult;
            CancelAsync = data.CancelAsync;
        }

        public void CloseDialogWithResult(Window dialog, WindowMessageResult result)
        {
            CancelAsync = true; // If method uses this to control running of async threads, this will force it to close when the window closes.

            UserDialogResult = result;
            if (dialog != null)
            {
                dialog.DialogResult = true;
            }
        }
    }
}
