using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMTemplate
{
    // START FileIOHandler Class --------------------------------------------------------------------------------------------------------
    public class OutputResult
    {
        public string filepath = "";
        public bool success = true;
    }

    public static class FileIOHandler
    {
        // Simple file deletion tool.
        public static bool Delete(string file)
        {
            if (File.Exists(file))
            {
                try
                {
                    File.Delete(file);
                    return true;
                }
                catch (Exception Ex)
                {
                    LogWriter.Exception("Error deleting file: " + file, Ex);
                    return false;
                }
            }

            return false;
        }

        public static List<OutputResult> CopyDirectory(string sourceDirectory, string targetDirectory)
        {
            List<OutputResult> output = new List<OutputResult>();

            try
            {
                DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
                DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

                foreach (OutputResult result in copyDirecoryWork(diSource, diTarget))
                {
                    output.Add(result);
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error processing Copy method.", Ex);
            }

            return output;
        }

        private static List<OutputResult> copyDirecoryWork(DirectoryInfo source, DirectoryInfo target)
        {
            List<OutputResult> output = new List<OutputResult>();

            try
            {
                Directory.CreateDirectory(target.FullName);

                // Copy each file into the new directory.
                foreach (FileInfo fi in source.GetFiles())
                {
                    OutputResult file_result = new OutputResult();

                    try
                    {
                        file_result.filepath = fi.FullName;

                        fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
                    }
                    catch (Exception Ex)
                    {
                        file_result.success = false;

                        LogWriter.Exception("Failed to copy file: " + fi.FullName, Ex);
                    }
                }

                // Copy each subdirectory using recursion.
                foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                {
                    try
                    {
                        DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);

                        foreach (OutputResult result in copyDirecoryWork(diSourceSubDir, nextTargetSubDir))
                        {
                            output.Add(result);
                        }
                    }
                    catch (Exception Ex)
                    {
                        LogWriter.Exception("Error with creating subdirectory: " + diSourceSubDir.Name, Ex);
                    }
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error processing Copy method.", Ex);
            }

            return output;
        }
    }
    // END FileIOHandler Class ----------------------------------------------------------------------------------------------------------
}
