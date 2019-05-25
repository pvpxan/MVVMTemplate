using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MVVMTemplate
{
    // START INI_Class ------------------------------------------------------------------------------------------------------------------
    public static class INI
    {
        private static ReaderWriterLockSlim iniLocker = new ReaderWriterLockSlim();

        public static bool? ReadBool(string file, string key)
        {
            string value = Read(file, key);
            bool parsed;
            if (bool.TryParse(value, out parsed) == false)
            {
                return null;
            }

            return parsed;
        }

        public static int? ReadInt(string file, string key)
        {
            string value = Read(file, key);
            if (Regex.IsMatch(value, @"^\d+$") == false)
            {
                return null;
            }

            return Convert.ToInt32(value);
        }

        public static string Read(string file, string key)
        {
            iniLocker.EnterReadLock();
            string value = safeRead(file, key);
            iniLocker.ExitReadLock();
            return value;
        }

        private static string safeRead(string file, string key)
        {
            if (file == null || key == null)
            {
                return "";
            }

            if (System.IO.File.Exists(file) == false)
            {
                return "";
            }

            string[] iniFile = null;

            try
            {
                iniFile = System.IO.File.ReadAllLines(file); // Reads all the lines of the ini file to an array.
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error reading ini file.", Ex);
                return "";
            }

            if (iniFile.Length <= 0) // Checks if there is readable content in the ini file.
            {
                return "";
            }

            // Checks each line of the array to see if it matches ini format and if it contains the item we are searching for.
            for (int i = 0; i < iniFile.Length; i++)
            {
                string lineLower = iniFile[i].ToLower();

                if (lineLower.Contains(key.ToLower()) && lineLower.Contains("=")) // Checks format.
                {
                    string[] keyValuePair = iniFile[i].Split('=');

                    // Checks if line is using key value pair is formated properly. At this point, it should at least have a length of 2.
                    // If the length is greater than 2, that means multiple '=' which is no good.
                    if (keyValuePair.Length == 2 && keyValuePair[0].ToLower() == key.ToLower())
                    {
                        return keyValuePair[1];
                    }
                }
            }

            return "";
        }

        public static bool Write(string file, string key, string value, bool create, bool backup)
        {
            iniLocker.EnterWriteLock();
            bool success = safeWrite(file, key, value, create, backup);
            iniLocker.ExitWriteLock();
            return success;
        }

        private static bool safeWrite(string file, string key, string value, bool create, bool backup)
        {
            if (file == null || key == null || value == null)
            {
                return false;
            }

            if (System.IO.File.Exists(file) == false && create == false)
            {
                LogWriter.LogEntry("INI write failure. File does not exist: " + file);
                return false;
            }

            if (System.IO.File.Exists(file) == false && create)
            {
                if (writeAllLines(file, new string[] { key + "=" + value, }, backup))
                {
                    return true;
                }

                return false;
            }

            // File should exists at this point.
            List<string> iniFile = null;

            try
            {
                iniFile = System.IO.File.ReadAllLines(file).ToList(); // Read the file to a List<string>
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error reading INI file: " + file, Ex);
                return false;
            }

            if (iniFile.Count <= 0)
            {
                if (writeAllLines(file, new string[] { key + "=" + value, }, backup))
                {
                    return true;
                }

                return false;
            }

            bool updated = false;

            // Checks each line of the array to see if it matches ini format and if it contains the item we want to update.
            for (int i = 0; i < iniFile.Count; i++)
            {
                string line = iniFile[i].ToLower();

                if (line.Contains(key.ToLower()) && line.Contains("="))
                {
                    iniFile[i] = key + "=" + value;
                    updated = true;
                    break;
                }
            }

            if (updated == false)
            {
                iniFile.Add(key + "=" + value);
            }

            if (writeAllLines(file, iniFile.ToArray(), backup))
            {
                return true;
            }

            return false;
        }

        private static bool writeAllLines(string file, string[] lines, bool backup)
        {
            string tempFile = "";

            try
            {
                tempFile = string.Format(@"{0}\{1}_Temp.ini", System.IO.Path.GetDirectoryName(file), System.IO.Path.GetFileNameWithoutExtension(file));
                System.IO.File.WriteAllLines(tempFile, lines);
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error creating temp INI file. File: " + tempFile, Ex);
                return false;
            }

            if (backup)
            {
                string backFile = "";

                try
                {
                    backFile = string.Format(@"{0}\{1}_Back.ini", System.IO.Path.GetDirectoryName(file), System.IO.Path.GetFileNameWithoutExtension(file));
                    System.IO.File.Copy(file, backFile, true);
                }
                catch (Exception Ex)
                {
                    LogWriter.Exception("Error creating backup INI file. File: " + backFile, Ex);
                }
            }

            try
            {
                System.IO.File.Copy(tempFile, file, true);
                System.IO.File.Delete(tempFile);
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error overwriting original INI file. File: " + file, Ex);
                return false;
            }

            return true;
        }
    }
    // END INI_Class --------------------------------------------------------------------------------------------------------------------
}
