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
        public DialogBaseWindow()
        {
            try
            {
                InitializeComponent();

                Closing += OnWindowClosing;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, "Window load error: " + Environment.NewLine + Convert.ToString(Ex), "Error...", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            DialogBaseWindowViewModel dialog_base_window_viewmodel = DataContext as DialogBaseWindowViewModel;

            if (dialog_base_window_viewmodel.RequireResult)
            {
                if (dialog_base_window_viewmodel.UserDialogResult == dialog_base_window_viewmodel.DefaultDialogResult)
                {
                    e.Cancel = true;
                }
            }
        }
    }

    public abstract class DialogBaseWindowViewModel : ViewModelBase
    {
        public string Title { get; private set; }
        public Brush Background { get; private set; }
        public bool Topmost { get; private set; }

        public WindowStyle DialogWindowStyle { get; private set; }
        public ImageSource WindowIcon { get; private set; }

        public bool RequireResult { get; private set; }
        public bool CancelAsync { get; private set; }

        public WindowMessageResult UserDialogResult { get; private set; }
        public WindowMessageResult DefaultDialogResult = WindowMessageResult.Undefined;

        public DialogBaseWindowViewModel(DialogData data)
        {
            Title = data.Title;
            Topmost = data.Topmost;
            Background = data.Background;

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
