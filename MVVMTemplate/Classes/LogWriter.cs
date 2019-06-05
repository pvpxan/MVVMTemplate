using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMTemplate
{
    // START LogWriter_Class ------------------------------------------------------------------------------------------------------------
    public static class LogWriter
    {
        // This class is a simple thread safe log writer/displayer.

        private static ReaderWriterLockSlim logLocker = new ReaderWriterLockSlim();

        private static string logApplication = "";
        private static string logPath = "";
        private static string logUser = "";
        private static string logFile = "";

        public static bool SetPath(string path, string user, string application)
        {
            if (System.IO.Directory.Exists(path) == false)
            {
                return false;
            }

            logApplication = application;
            logPath = path;
            logUser = user;

            return true;
        }

        // Creates a new log file if appropriate
        // -----------------------------------------------------------------------
        private static bool generateLogFile()
        {
            // Get the path of the application. Failure indicates that the user specific temp folder can be used IF access is available.
            if (logPath.Length <= 0)
            {
                try
                {
                    logPath = System.IO.Path.GetTempPath();
                }
                catch
                {
                    return false;
                }
            }

            // Log file name is defined.
            logFile = string.Format(@"{0}\log\{1}_{2}.log", logPath, logApplication, DateTime.Now.ToString("yyyy-MM-dd"));

            try
            {
                System.IO.Directory.CreateDirectory(logPath + @"\log"); // Generates the folder for the log files if non exists.
            }
            catch
            {
                return false;
            }

            // Log file is created if none exists.
            if (System.IO.File.Exists(logFile) == false)
            {
                try
                {
                    System.IO.File.Create(logFile).Dispose(); // Generates a log file if non exists.
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }
        // -----------------------------------------------------------------------

        // Logs an exception message in a specific format for a log file.
        // -----------------------------------------------------------------------
        public static void Exception(string log, Exception Ex)
        {
            string message = log + Environment.NewLine + Convert.ToString(Ex);
            LogEntry(message);
        }

        // Logs string to log file.
        // -----------------------------------------------------------------------
        public static void LogEntry(string log)
        {
            // TODO: These might be used for timeouts later. Needed at this time for proper task creation.
            var source = new CancellationTokenSource();
            var token = source.Token;

            // Creates a thread that will write to a log file.
            Task.Factory.StartNew(() =>
            {
                logEntryThreadCall(log);
            },
            token, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }

        private static void logEntryThreadCall(string log)
        {
            logLocker.EnterWriteLock();

            if (generateLogFile())
            {
                try
                {
                    System.IO.File.AppendAllText(logFile, (DateTime.Now.ToString("yyyy-MM-dd - HH:mm:ss") + " - " + logUser + " - " + log + Environment.NewLine));
                }
                catch
                {
                    // TODO (DB): Add a way to track errors writing to the log file.
                }
            }

            logLocker.ExitWriteLock();
        }

        // TODO: More for this later.
        // -----------------------------------------------------------------------
        //public static void ErrorLog()
        //{

        //}
    }
    // END LogWriter_Class --------------------------------------------------------------------------------------------------------------

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
                currentWindow = null;
            }
        }
        // -----------------------------------------------------------------------

        // -----------------------------------------------------------------------
        public static void LogDisplay(string log, MessageBoxImage messageType, Window window = null)
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
        public static void ExceptionDisplay(string log, Exception Ex, bool showFull, Window window = null)
        {
            string message = log + Environment.NewLine + Convert.ToString(Ex);
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

