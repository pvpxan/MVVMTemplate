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
        public void AppStartup(object sender, StartupEventArgs e)
        {
            // LoadAssemblies(); // Load embedded resources.
            Statics.Initialization();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        // NECESSARY for loading embedded resources.
        // ----------------------------------------------------------------------------------------------
        public static void LoadAssemblies()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string dllName = new AssemblyName(args.Name).Name + ".dll";
                var assembly = Assembly.GetExecutingAssembly();

                string resourceName = assembly.GetManifestResourceNames().FirstOrDefault(rn => rn.EndsWith(dllName));
                if (resourceName == null)
                {
                    return null; // Not found, maybe another handler will find it
                }

                System.IO.Stream stream = null;
                Assembly loadedAssembly = null;
                try
                {
                    stream = assembly.GetManifestResourceStream(resourceName);
                    byte[] assemblyData = new byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    loadedAssembly = Assembly.Load(assemblyData);
                }
                catch (Exception Ex)
                {
                    loadedAssembly = null;
                    MessageBox.Show("Error loading embedded assembly resource. Application will now close." + Environment.NewLine + Convert.ToString(Ex));
                    Application.Current.Shutdown();
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Dispose();
                    }
                }

                return loadedAssembly;
            };
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
        public static void Initialization()
        {
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
            catch
            {
                // TODO: Add some sort of default logging.
            }

            LogWriter.SetPath(ProgramPath, CurrentUser, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
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
}
