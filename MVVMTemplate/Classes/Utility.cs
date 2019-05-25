using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMTemplate
{
    // START DeleteFile Class -----------------------------------------------------------------------------------------------------------
    public static class DeleteFile
    {
        // Simple file deletion tool.
        public static bool Target(string file)
        {
            if (System.IO.File.Exists(file))
            {
                try
                {
                    System.IO.File.Delete(file);
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
    }
    // END DeleteFile Class -------------------------------------------------------------------------------------------------------------
}
