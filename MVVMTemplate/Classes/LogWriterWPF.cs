using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMTemplate
{
    // START LogWriterWPF_Class ---------------------------------------------------------------------------------------------------------
    public static class LogWriterWPF
    {
        // This class allows for displaying of messages before they are logged with LogWriterConsole

        private static Window currentWindow = null;

        // Define the active current window.
        // -----------------------------------------------------------------------
        private static void setCurrentWindow()
        {
            try
            {
                currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
            }
            catch
            {
                if (Application.Current.Windows.Count > 0)
                {
                    currentWindow = Application.Current.Windows[0];
                }
            }
        }
        // -----------------------------------------------------------------------

        // -----------------------------------------------------------------------
        public static void LogDisplay(string log, MessageBoxImage messageType)
        {
            logDisplay(log, messageType);
        }

        public static void LogDisplay(string log, MessageBoxImage messageType, Window window)
        {
            logDisplay(log, messageType, window);
        }

        private static void logDisplay(string log, MessageBoxImage messageType, Window window = null)
        {
            if (window == null)
            {
                setCurrentWindow();
                if (currentWindow != null)
                {
                    window = currentWindow;
                }
            }

            string message = log;
            string caption = "Error...";
            MessageBoxImage messageBoxImage = messageType;

            LogWriter.LogEntry(log);

            switch (messageBoxImage)
            {
                case MessageBoxImage.Error:
                    caption = "Error...";
                    break;

                case MessageBoxImage.Exclamation:
                    caption = "Important...";
                    break;

                case MessageBoxImage.Information:
                    caption = "Information...";
                    break;

                case MessageBoxImage.None:
                    caption = "Message...";
                    break;

                case MessageBoxImage.Question:
                    caption = "Question...";
                    break;

                default:
                    break;
            }

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                if (window != null)
                {
                    MessageBox.Show(window, message, caption, MessageBoxButton.OK, messageBoxImage);
                }
                else
                {
                    MessageBox.Show(message, caption, MessageBoxButton.OK, messageBoxImage);
                }
            });
        }

        // -----------------------------------------------------------------------
        public static void ExceptionDisplay(string log, Exception ex, bool showFull)
        {
            exceptionDisplay(log, ex, showFull);
        }

        public static void ExceptionDisplay(string log, Exception ex, bool showFull, Window window)
        {
            exceptionDisplay(log, ex, showFull, window);
        }

        private static void exceptionDisplay(string log, Exception ex, bool showFull, Window window = null)
        {
            string message = log + Environment.NewLine + Convert.ToString(ex);
            LogWriter.LogEntry(message);

            if (window == null)
            {
                setCurrentWindow();
                if (currentWindow != null)
                {
                    window = currentWindow;
                }
            }

            if (showFull == false)
            {
                message = log;
            }

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                if (window != null)
                {
                    MessageBox.Show(window, message, "Error...", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(message, "Error...", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        // TODO: More for this later.
        // -----------------------------------------------------------------------
        //public static void LogFailure()
        //{

        //}
    }
    // END LogWriterWPF_Class -----------------------------------------------------------------------------------------------------------
}
