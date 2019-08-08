using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MVVMTemplate
{
    // START INI_Class ------------------------------------------------------------------------------------------------------------------
    public class INIKeyValuePair
    {
        public string Key = "";
        public string Value = "";
    }

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
            int parsed;
            if (int.TryParse(value, out parsed) == false)
            {
                return null;
            }

            return parsed;
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

            if (File.Exists(file) == false)
            {
                return "";
            }

            string[] iniFile = null;

            try
            {
                iniFile = File.ReadAllLines(file); // Reads all the lines of the ini file to an array.
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
            if (string.IsNullOrEmpty(file) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                return false;
            }

            INIKeyValuePair[] iniKeyValueSettings = new INIKeyValuePair[]
            {
                new INIKeyValuePair(){ Key = key, Value = value},
            };

            iniLocker.EnterWriteLock();
            bool success = safeWrite(file, iniKeyValueSettings, create, backup);
            iniLocker.ExitWriteLock();
            return success;
        }

        public static bool MultiWrite(string file, INIKeyValuePair[] iniKeyValueSettings, bool create, bool backup)
        {
            if (string.IsNullOrEmpty(file) || iniKeyValueSettings == null || iniKeyValueSettings.Length <= 0)
            {
                return false;
            }

            iniLocker.EnterWriteLock();
            bool success = safeWrite(file, iniKeyValueSettings, create, backup);
            iniLocker.ExitWriteLock();
            return success;
        }

        private static bool safeWrite(string file, INIKeyValuePair[] iniKeyValueSettings, bool create, bool backup)
        {
            if (File.Exists(file) == false && create == false)
            {
                LogWriter.LogEntry("INI write failure. File does not exist: " + file);
                return false;
            }

            if (File.Exists(file) == false && create)
            {
                if (writeAllLines(file, getAllText(iniKeyValueSettings), backup))
                {
                    return true;
                }

                return false;
            }

            // File should already exists at this point.
            string[] iniFile = null;
            try
            {
                iniFile = File.ReadAllLines(file); // Read the file to a List<string>
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error reading INI file: " + file, Ex);
                return false;
            }

            // If the file is empty, just write directly to it.
            if (iniFile == null || iniFile.Length <= 0)
            {
                if (writeAllLines(file, getAllText(iniKeyValueSettings), backup))
                {
                    return true;
                }

                return false;
            }

            // Converts the lines to a List<INIKeyValuePair>.
            // This will check each line to make sure it is in the proper format for an INI file.
            List<INIKeyValuePair> iniFileKeyValueList = new List<INIKeyValuePair>();
            for (int i = 0; i < iniFile.Length; i++)
            {
                string line = iniFile[i].ToLower();
                if (line.Contains("=") == false)
                {
                    continue;
                }

                string[] parts = line.Split('=');
                if (parts.Length > 2)
                {
                    continue;
                }

                iniFileKeyValueList.Add(new INIKeyValuePair() { Key = parts[0], Value = parts[1] });
            }

            // Checks the parameters to see if there are any new Key/Value pairs that need to be added or updated.
            foreach (INIKeyValuePair iniKeyValue in iniKeyValueSettings)
            {
                int index = iniFileKeyValueList.FindIndex(k => k.Key.ToLower() == iniKeyValue.Key.ToLower());
                if (index > -1)
                {
                    iniFileKeyValueList[index].Value = iniKeyValue.Value;
                }
                else
                {
                    iniFileKeyValueList.Add(new INIKeyValuePair() { Key = iniKeyValue.Key, Value = iniKeyValue.Value });
                }
            }

            if (writeAllLines(file, getAllText(iniFileKeyValueList.ToArray()), backup))
            {
                return true;
            }

            return false;
        }

        private static string getAllText(INIKeyValuePair[] iniKeyValueList)
        {
            string allText = "";
            foreach (INIKeyValuePair iniKeyValuePair in iniKeyValueList)
            {
                if (string.IsNullOrEmpty(iniKeyValuePair.Key))
                {
                    continue;
                }

                if (iniKeyValuePair.Value == null)
                {
                    iniKeyValuePair.Value = "";
                }

                allText = allText + iniKeyValuePair.Key + "=" + iniKeyValuePair.Value + Environment.NewLine;
            }

            return allText;
        }

        private static bool writeAllLines(string file, string allText, bool backup)
        {
            string tempFile = "";

            try
            {
                tempFile = string.Format(@"{0}\{1}_Temp.ini", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
                File.WriteAllText(tempFile, allText);
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
                    backFile = string.Format(@"{0}\{1}_Back.ini", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));

                    if (File.Exists(file))
                    {
                        File.Copy(file, backFile, true);
                    }
                }
                catch (Exception Ex)
                {
                    LogWriter.Exception("Error creating backup INI file. File: " + backFile, Ex);
                }
            }

            try
            {
                File.Copy(tempFile, file, true);
                File.Delete(tempFile);
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
