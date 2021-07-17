using StreamlineMVVM;
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
            // Use these if you are loading embedded resources.
            DLLEmbeddingHandler.LoadAssemblies(); 
            DLLEmbeddingHandler.LoadResourceDictionary("StreamlineMVVM", "Templates/MergedResources.xaml"); // Loads the library MergedResources of StreamlineMVVM.
            DLLEmbeddingHandler.LoadResourceDictionary("MVVMTemplate", "Resources/DataTemplates.xaml"); // Loads application DataTemplates AFTER libraries loaded so things work.

            Statics.Initialization();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }

    // START DLLEmbeddingHandler_Class --------------------------------------------------------------------------------------------------
    public static class DLLEmbeddingHandler
    {
        // NECESSARY for loading embedded resources.
        // ----------------------------------------------------------------------------------------------
        public static void LoadAssemblies()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                return resolve(sender, args);
            };
        }

        private static Assembly resolve(object sender, ResolveEventArgs args)
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
                Shutdown("DLL", Ex);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }

            return loadedAssembly;
        }

        // This is for WPF applications only that reference XAML files built into an assembly.
        public static void LoadResourceDictionary(string assembly, string path)
        {
            // Uri path of assembly resource.
            string uri = @"pack://application:,,,/" + assembly + ";component/" + path;

            // Add Uri to App ResourceDictionary.
            try
            {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri(uri) });
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error loading embedded XAML resource. Application will now close." + Environment.NewLine + Convert.ToString(Ex));
                Shutdown("XAML", Ex);
            }
        }

        public static void Shutdown(string resource, Exception Ex)
        {
            string message = "Error loading embedded " + resource + " resource. Application will now close." + Environment.NewLine + Convert.ToString(Ex);

            MessageBox.Show(message);
            if (Application.Current != null)
            {
                Application.Current.Shutdown();
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
    // END DLLEmbeddingHandler_Class ----------------------------------------------------------------------------------------------------

    // START Statics_Class --------------------------------------------------------------------------------------------------------------
    public static class Statics
    {
        public static Version CurrentFileVersion { get; } = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
        public static string CurrentUser { get; } = Environment.UserName.ToLower();
        public static string[] Args { get; } = Environment.GetCommandLineArgs();
        public static string ProgramPath { get; } = getProgramPath();

        public static Window MainWindow = null;
        public static string INIFilePath = "";

        // Method to load start up global vars.
        public static void Initialization()
        {
            LogWriter.SetPath(ProgramPath, CurrentUser, Assembly.GetExecutingAssembly().GetName().Name);
        }

        private static string getProgramPath()
        {
            try
            {
                if (AppDomain.CurrentDomain.BaseDirectory[AppDomain.CurrentDomain.BaseDirectory.Length - 1] == '\\')
                {
                    return AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
                }
                else
                {
                    return Environment.CurrentDirectory;
                }
            }
            catch
            {
                // TODO: Add some sort of default logging.
                return "";
            }
        }

        public static void Shutdown()
        {
            if (Application.Current == null)
            {
                Environment.Exit(0);
            }
            else
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    Application.Current.Shutdown();
                });
            }
        }

        // Opens Custom Windows style Dialog Window and setting the owner of that window to the passed in paramater. Optinal use since a dialog can be directly opened.
        public static WindowMessageResult OpenWindowsDialog(DialogData data, Window window)
        {
            WindowMessageResult result = WindowMessageResult.Undefined;

            if (Application.Current == null)
            {
                return result;
            }

            // This will allow for use of this method from threads outside the UI thread.
            window.Dispatcher.Invoke((Action)delegate
            {
                result = MessageBoxEnhanced.OpenWindowMessage(data, window);
            });

            return result;
        }
    }
    // END Statics_Class ----------------------------------------------------------------------------------------------------------------
}
