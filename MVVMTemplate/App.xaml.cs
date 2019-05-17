using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMTemplate
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void App_Startup(object sender, StartupEventArgs e)
        {
            Statics.initialization();

            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
        }
    }

    // START Statics_Class --------------------------------------------------------------------------------------------------------------
    public static class Statics
    {
        public static Version CurrentFileVersion = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);

        public static Window MainWindow = null;

        public static readonly string[] Args = Environment.GetCommandLineArgs();

        public static readonly string CurrentUser = Environment.UserName.ToLower();

        public static string ProgramPath = "";
        public static string INIFilePath = "";

        // Method to load start up global vars.
        public static void initialization()
        {
            //LoadDLLs load_dlls = new LoadDLLs();
            //load_dlls.init();

            try
            {
                if (System.AppDomain.CurrentDomain.BaseDirectory[System.AppDomain.CurrentDomain.BaseDirectory.Length - 1] == '\\')
                {
                    ProgramPath = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
                }
                else
                {
                    ProgramPath = Environment.CurrentDirectory;
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error attempting to find program local path.", Ex);
            }
        }

        // Opens Custom Windows style Dialog Window and setting the owner of that window to the passed in paramater. Optinal use since a dialog can be directly opened.
        public static WindowMessageResult OpenWindowsDialog(DialogData data, Window window)
        {
            WindowMessageResult result = WindowMessageResult.Undefined;

            // This will allow for use of this method from threads outside the UI thread.
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                DialogBaseWindowViewModel viewmodel = new WindowsMessageViewModel(data);
                result = DialogService.OpenDialog(viewmodel, window);
            });

            return result;
        }
    }
    // END Statics_Class ----------------------------------------------------------------------------------------------------------------

    // START LoadDLLs_Class -------------------------------------------------------------------------------------------------------------
    public class LoadDLLs
    {
        public void init()
        {
            // NECESSARY for loading embedded resources. Cannot be static class.
            // -----------------------------------------------------------------------------------
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                {
                    string resourceName = new AssemblyName(args.Name).Name + ".dll";
                    string resource = Array.Find(this.GetType().Assembly.GetManifestResourceNames(), element => element.EndsWith(resourceName));

                    using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
                    {
                        Byte[] assemblyData = new Byte[stream.Length];
                        stream.Read(assemblyData, 0, assemblyData.Length);
                        return Assembly.Load(assemblyData);
                    }
                };
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error loading embedded assembly resource. Application is about to crash.", Ex, true);
            }
            // -----------------------------------------------------------------------------------
        }
    }
    // END LoadDLLs_Class ---------------------------------------------------------------------------------------------------------------
}
