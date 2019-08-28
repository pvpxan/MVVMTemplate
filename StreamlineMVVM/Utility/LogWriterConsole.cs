using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StreamlineMVVM
{
    // START LogWriter_Class ------------------------------------------------------------------------------------------------------------
    public static class LogWriter
    {
        // This class is a simple thread safe log writer/displayer.
        private static ReaderWriterLockSlim logLocker = new ReaderWriterLockSlim();

        // Since this class spins up threads, there is a possiblity that a console app can close before a thread finishes.
        public static ConcurrentQueue<string> LogQueue { get; private set; } = new ConcurrentQueue<string>();

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
        public static void Exception(string log, Exception ex)
        {
            string message = log + Environment.NewLine + Convert.ToString(ex);
            LogEntry(message);
        }

        // Logs string to log file.
        // -----------------------------------------------------------------------
        public static void LogEntry(string log)
        {
            // TODO: These might be used for timeouts later. Needed at this time for proper task creation.
            var source = new CancellationTokenSource();
            var token = source.Token;

            LogQueue.Enqueue(log);

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

            LogQueue.TryDequeue(out string loggedMessage);
            logLocker.ExitWriteLock();
        }

        // TODO: More for this later.
        // -----------------------------------------------------------------------
        //public static void ErrorLog()
        //{

        //}
    }
    // END LogWriter_Class --------------------------------------------------------------------------------------------------------------
}
