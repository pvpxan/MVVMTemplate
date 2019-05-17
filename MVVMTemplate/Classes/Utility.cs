using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMTemplate
{
    // START DeleteFile Class -----------------------------------------------------------------------------------------------------------
    public static class DeleteFile
    {
        // Simple file deletion tool.
        public static bool Target(string file)
        {
            bool deleted = false;

            if (System.IO.File.Exists(file))
            {
                try
                {
                    System.IO.File.Delete(file);

                    deleted = true;
                }
                catch (Exception Ex)
                {
                    LogWriter.Exception("Error deleting file: " + file, Ex);
                }
            }
            else
            {
                deleted = true;
            }

            return deleted;
        }
    }
    // END DeleteFile Class -------------------------------------------------------------------------------------------------------------

    // START ConfigWriter Class ---------------------------------------------------------------------------------------------------------
    public static class ConfigManager
    {
        public static string Read(string Key)
        {
            string output = "";

            try
            {
                if (Key != null)
                {
                    ConfigurationManager.RefreshSection("appSettings");
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                    output = ConfigurationManager.AppSettings[Key];
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error reading config. Key: " + Key, Ex);
            }

            if (output == null)
            {
                output = "";
            }

            return output;
        }

        public static void Update(string Key, string Value)
        {
            try
            {
                if (Key != null)
                {
                    ConfigurationManager.RefreshSection("appSettings");
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                    if (Value == null)
                    {
                        Value = "";
                    }

                    string key_check = ConfigurationManager.AppSettings[Key];

                    if (key_check != null)
                    {
                        config.AppSettings.Settings[Key].Value = Value;
                    }
                    else
                    {
                        config.AppSettings.Settings.Add(Key, Value);
                    }

                    config.AppSettings.SectionInformation.ForceSave = true;
                    config.Save(ConfigurationSaveMode.Modified);

                    ConfigurationManager.RefreshSection("appSettings");
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error writing data to config. Key: " + Key + " | Value: " + Value, Ex);
            }
        }
    }
    // END ConfigWriter Class -----------------------------------------------------------------------------------------------------------

    // START LogWriter_Class ------------------------------------------------------------------------------------------------------------
    public static class LogWriter
    {
        // This class is a simple thread safe log writer/displayer.

        private static ReaderWriterLockSlim log_locker = new ReaderWriterLockSlim();

        private static string log_file { get; set; }

        // Creates a new log file if appropriate
        // -----------------------------------------------------------------------
        public static bool GenerateLogFile()
        {
            // Log file name is defined.
            string log_name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            log_file = (Statics.ProgramPath + @"\log\" + DateTime.Now.ToString("yyyy-MM-dd") + "-" + log_name + ".log");

            // Log file is created if none exists.
            if (System.IO.File.Exists(log_file) == false)
            {
                try
                {
                    System.IO.Directory.CreateDirectory(Statics.ProgramPath + @"\log"); // Generates the folder for the log files if non exists.
                }
                catch
                {
                    return false;
                }

                try
                {
                    System.IO.File.Create(log_file).Dispose(); // Generates a log file if non exists.
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
            // These might be used for timeouts later. Needed at this time for proper task creation.
            var source = new CancellationTokenSource();
            var token = source.Token;

            // Creates a thread that will write to a log file.
            Task.Factory.StartNew(() =>
            {
                LogEntry_Thread_Call(log);
            }, 
            token, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }

        private static void LogEntry_Thread_Call(string log)
        {
            log_locker.EnterWriteLock();

            if (GenerateLogFile())
            {
                try
                {
                    System.IO.File.AppendAllText(log_file, (DateTime.Now.ToString("yyyy-MM-dd - HH:mm:ss") + " - " + Statics.CurrentUser + " - " + log + Environment.NewLine));
                }
                catch (Exception Ex)
                {
                    Show_Message("Error updating logfile. " + Environment.NewLine + Convert.ToString(Ex), "Error...", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            log_locker.ExitWriteLock();
        }

        // -----------------------------------------------------------------------
        public static void DisplayAndLog(string log, MessageBoxImage type, Window window = null)
        {
            // These might be used for timeouts later. Needed at this time for proper task creation.
            var source = new CancellationTokenSource();
            var token = source.Token;

            // Creates a thread that will write to a log file.
            Task.Factory.StartNew(() =>
            {
                DisplayAndLog_Thread_Call(log, type, window);
            },
            token, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }

        private static void DisplayAndLog_Thread_Call(string log, MessageBoxImage type, Window window = null)
        {
            log_locker.EnterWriteLock();

            if (GenerateLogFile())
            {
                try
                {
                    System.IO.File.AppendAllText(log_file, DateTime.Now.ToString("yyyy-MM-dd - HH:mm:ss") + " - " + Statics.CurrentUser + " - " + log + Environment.NewLine);

                    switch (type)
                    {
                        default:

                            break;

                        case MessageBoxImage.Error:

                            Show_Message(log, "Error...", MessageBoxButton.OK, type, window);

                            break;

                        case MessageBoxImage.Exclamation:

                            Show_Message(log, "Important...", MessageBoxButton.OK, type, window);

                            break;

                        case MessageBoxImage.Information:

                            Show_Message(log, "Information...", MessageBoxButton.OK, type, window);

                            break;

                        case MessageBoxImage.None:

                            Show_Message(log, "Message...", MessageBoxButton.OK, type, window);

                            break;

                        case MessageBoxImage.Question:

                            Show_Message(log, "Question...", MessageBoxButton.OK, type, window);

                            break;
                    }
                }
                catch (Exception Ex)
                {
                    Show_Message("Error updating logfile. " + Environment.NewLine + Convert.ToString(Ex), "Error...", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            log_locker.ExitWriteLock();
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
                Exception_Thread_Call(log, Ex, show, exception, window);
            },
            token, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }

        private static void Exception_Thread_Call(string log, Exception Ex, bool show = false, bool exception = false, Window window = null)
        {
            LogEntry(log + Environment.NewLine + Convert.ToString(Ex));

            if (show)
            {
                if (exception)
                {
                    Show_Message(log + Environment.NewLine + Convert.ToString(Ex), "Error...", MessageBoxButton.OK, MessageBoxImage.Error, window);
                }
                else
                {
                    Show_Message(log, "Error...", MessageBoxButton.OK, MessageBoxImage.Error, window);
                }
            }
        }

        private static void Show_Message(string log, string title, MessageBoxButton button, MessageBoxImage type, Window window = null)
        {
            if (window == null && Statics.MainWindow != null)
            {
                window = Statics.MainWindow;
            }

            App.Current.Dispatcher.Invoke((Action)delegate
            {
                if (window == null)
                {
                    MessageBox.Show(log, title, button, type);
                }
                else
                {
                    MessageBox.Show(window, log, title, button, type);
                }
            });
        }

        // More for this later.
        // -----------------------------------------------------------------------
        //public static void ErrorLog()
        //{

        //}
    }
    // END LogWriter_Class --------------------------------------------------------------------------------------------------------------
}
