using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MVVMTemplate
{
    public static class INI
    {
        private static ReaderWriterLockSlim ini_locker = new ReaderWriterLockSlim();

        public static bool? ReadBool(string file, string key)
        {
            bool parsed;
            if (bool.TryParse(Read(file, key), out parsed))
            {
                return parsed;
            }
            else
            {
                return null;
            }
        }

        public static int? ReadInt(string file, string key)
        {
            bool int_check = Regex.IsMatch(Read(file, key), @"^\d+$");

            if (int_check)
            {
                return Convert.ToInt32(Read(file, key));
            }
            else
            {
                return null;
            }
        }

        // This will block until the INI file is free for reading. Be careful.
        public static string Read(string file, string key)
        {
            ini_locker.EnterReadLock();

            if (System.IO.File.Exists(file))
            {
                try
                {
                    // Reads all the lines of the ini file to an array.
                    string[] ini_file = System.IO.File.ReadAllLines(file);

                    // Checks each line of the array to see if it matches ini format and if it contains the item we are searching for.
                    for (int i = 0; i < ini_file.Length; i++)
                    {
                        if (ini_file[i].ToLower().Contains(key) && ini_file[i].ToLower().Contains("=")) // Checks format.
                        {
                            string[] key_value_pair = ini_file[i].Split('=');

                            if (key_value_pair.Length == 2) // Checks if contains data.
                            {
                                if (key_value_pair[0].ToLower() == key.ToLower())
                                {
                                    return key_value_pair[1];
                                }
                            }
                        }
                    }
                }
                catch
                {

                }
            }

            ini_locker.ExitReadLock();

            return "";
        }

        public static void Write(string file, string key, string value)
        {
            ini_locker.EnterWriteLock();

            // These might be used for timeouts later. Needed at this time for proper task creation.
            var source = new CancellationTokenSource();
            var token = source.Token;

            // Creates a thread that will read from the INI file.
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (System.IO.File.Exists(file))
                    {
                        // Reads all the lines of the ini file to a list.
                        List<string> ini_file = System.IO.File.ReadAllLines(file).ToList();

                        // Checks each line of the array to see if it matches ini format and if it contains the item we want to update.
                        for (int i = 0; i < ini_file.Count; i++)
                        {
                            if (ini_file[i].ToLower().Contains(key) && ini_file[i].ToLower().Contains("="))
                            {
                                ini_file[i] = key + "=" + value;
                            }
                            else
                            {
                                ini_file.Add(key + "=" + value);
                            }
                        }

                        System.IO.File.WriteAllLines(file, ini_file.ToArray());
                    }
                    else
                    {
                        System.IO.File.WriteAllLines(file, new string[] { key + "=" + value, });
                    }
                }
                catch (Exception Ex)
                {
                    LogWriter.Exception("Error writing to ini file", Ex);
                }
            },
            token, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }
    }
    // END INI_Class --------------------------------------------------------------------------------------------------------------------
}
