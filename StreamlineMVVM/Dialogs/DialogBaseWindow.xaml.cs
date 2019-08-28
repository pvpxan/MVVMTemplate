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

namespace StreamlineMVVM
{
    /// <summary>
    /// Interaction logic for DialogBaseWindow.xaml
    /// </summary>
    public partial class DialogBaseWindow : Window
    {
        private DialogBaseWindowViewModel dialogBaseWindowViewModel = null;

        public DialogBaseWindow(DialogData dialogData)
        {
            if (Application.Current == null)
            {
                MessageBox.Show(this, "Window load error: " + Environment.NewLine + "The Application object has been shutdown or is null.", "Error...", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                InitializeComponent();

                try
                {
                    if (dialogData.WindowIconURI.Length > 0)
                    {
                        Icon = BitmapFrame.Create(new Uri(dialogData.WindowIconURI, UriKind.RelativeOrAbsolute));
                    }
                }
                catch
                {
                    // TODO (DB):  Find a way to extract the application icon and assign it.
                }

                // General Window properties. Not bound since some are not technically content and one is not able to be bound.
                WindowStartupLocation = dialogData.DialogStartupLocation; // Cannot be bound since it is a DependencyProperty
                WindowStyle = dialogData.DialogWindowStyle;
                Topmost = dialogData.Topmost;
                Title = dialogData.WindowTitle;
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
                if (dialogBaseWindowViewModel.UserDialogResult == WindowMessageResult.Undefined)
                {
                    e.Cancel = true;
                }
            }
        }
    }

    public class DialogBaseWindowViewModel : ViewModelBase
    {
        public DialogData dialogData { get; private set; }

        public bool RequireResult { get; private set; }
        public bool CancelAsync { get; private set; }

        public WindowMessageResult UserDialogResult = WindowMessageResult.Undefined;

        public Window DialogWindow = null;
        public ControlContentRendered WindowRenderedEvent = new ControlContentRendered();

        public DialogBaseWindowViewModel(DialogData data)
        {
            dialogData = data;

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
