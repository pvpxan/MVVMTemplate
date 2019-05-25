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

        private static string logFile { get; set; }

        // Creates a new log file if appropriate
        // -----------------------------------------------------------------------
        private static bool generateLogFile()
        {
            // Log file name is defined.
            string applicationName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            logFile = string.Format(@"{0}\log\{1}_{2}.log", Statics.ProgramPath, applicationName, DateTime.Now.ToString("yyyy-MM-dd"));

            try
            {
                System.IO.Directory.CreateDirectory(Statics.ProgramPath + @"\log"); // Generates the folder for the log files if non exists.
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
                    System.IO.File.AppendAllText(logFile, (DateTime.Now.ToString("yyyy-MM-dd - HH:mm:ss") + " - " + Statics.CurrentUser + " - " + log + Environment.NewLine));
                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Error updating logfile. " + Environment.NewLine + Convert.ToString(Ex), "Error...", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            logLocker.ExitWriteLock();
        }

        // -----------------------------------------------------------------------
        public static void LogDisplay(string log, MessageBoxImage messageType, Window window = null)
        {
            // These might be used for timeouts later. Needed at this time for proper task creation.
            var source = new CancellationTokenSource();
            var token = source.Token;

            // Creates a thread that will write to a log file.
            Task.Factory.StartNew(() =>
            {
                logDisplayThreadCall(log, messageType, window);
            },
            token, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }

        private static void logDisplayThreadCall(string log, MessageBoxImage messageType, Window window = null)
        {
            logLocker.EnterWriteLock();

            if (generateLogFile())
            {
                if (window == null && Statics.MainWindow != null)
                {
                    window = Statics.MainWindow;
                }

                string message = log;
                string caption = "Error...";
                MessageBoxImage messageBoxImage = messageType;

                try
                {
                    System.IO.File.AppendAllText(logFile, DateTime.Now.ToString("yyyy-MM-dd - HH:mm:ss") + " - " + Statics.CurrentUser + " - " + log + Environment.NewLine);
                }
                catch (Exception Ex)
                {
                    message =
                        "Error updating logfile with the following log entry:" + Environment.NewLine +
                        log + Environment.NewLine +
                        "LogWriter failure details:" + Environment.NewLine +
                        Convert.ToString(Ex);
                    messageBoxImage = MessageBoxImage.Error;
                }

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

                App.Current.Dispatcher.Invoke((Action)delegate
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

            logLocker.ExitWriteLock();
        }

        // -----------------------------------------------------------------------
        public static void Exception(string log, Exception Ex, bool show = false, bool exception = false, Window window = null)
        {
            // These might be used for timeouts later. Needed at this time for proper task creation.
            var source = new CancellationTokenSource();
            var token = source.Token;

            // Creates a thread that will write to a log file.
            Task.Factory.StartNew(() =>
            {
                exceptionThreadCall(log, Ex, show, exception, window);
            },
            token, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }

        private static void exceptionThreadCall(string log, Exception Ex, bool show = false, bool exception = false, Window window = null)
        {
            string message = log + Environment.NewLine + Convert.ToString(Ex);
            LogEntry(message);

            if (show == false)
            {
                return;
            }

            if (window == null && Statics.MainWindow != null)
            {
                window = Statics.MainWindow;
            }

            if (exception)
            {
                message = log;
            }

            App.Current.Dispatcher.Invoke((Action)delegate
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
        //public static void ErrorLog()
        //{

        //}
    }
    // END LogWriter_Class --------------------------------------------------------------------------------------------------------------
}

