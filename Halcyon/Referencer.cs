using Halcyon.Errors;
using Halcyon.Logging;
using Halcyon.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    public static class Referencer
    {
        public static string Folder;
        public static string AssemblyBlock;
        public static Dictionary<string, ValuePair<string, string>> AssemblyInfos;
        public static readonly Dictionary<string, Assembly> LoadedAssemblies = new Dictionary<string, Assembly>();

        public static void Initialize()
        {
            Folder = Path.GetDirectoryName(Program.Path);
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            LoadAssemblies();
        }

        public static void DeInitialize()
        {
            Folder = "";
            AssemblyBlock = "";
            AssemblyInfos.Clear();
            LoadedAssemblies.Clear();
        }
        public static void LoadAssemblies()
        {
            string ignoredFilesPath = Path.Combine(Folder, "ignore");

            List<string> ignoredFiles = new List<string>();
            if (File.Exists(ignoredFilesPath))
                ignoredFiles.AddRange(File.ReadAllLines(ignoredFilesPath));

            List<FileInfo> fileInfos = new DirectoryInfo(Folder).GetFiles("*.dll").ToList();
            fileInfos.AddRange(new DirectoryInfo(Folder).GetFiles("*.exe"));

            foreach (FileInfo fileInfo in fileInfos)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
                try
                {
                    Assembly assembly;
                    // The plugin assembly might have been resolved by another plugin assembly already, so checking is required
                    if (!LoadedAssemblies.TryGetValue(fileNameWithoutExtension, out assembly))
                    {
                        try
                        {
                            assembly = Assembly.Load(File.ReadAllBytes(fileInfo.FullName));
                        }
                        catch (BadImageFormatException)
                        {
                            continue;
                        }
                        LoadedAssemblies.Add(fileNameWithoutExtension, assembly);
                    }
                }
                catch (Exception ex)
                {
                    // Broken assemblies / plugins better stop the entire server init.
                    Logger.Log(string.Format("Failed to load assembly \"{0}\".", fileInfo.Name) + ex);
                }
            }
            RegisterAssemblies();
        }

        public static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string fileName = args.Name.Split(',')[0];
            string path = Path.Combine(Folder, fileName + ".dll");
            try
            {
                if (File.Exists(path))
                {
                    Assembly assembly;
                    if (!LoadedAssemblies.TryGetValue(fileName, out assembly))
                    {
                        assembly = Assembly.Load(File.ReadAllBytes(path));
                        LoadedAssemblies.Add(fileName, assembly);
                    }
                    return assembly;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    string.Format("Error on resolving assembly \"{0}.dll\":\n{1}", fileName, ex),
                    TraceLevel.Error);
            }
            return null;
        }
        public static void RegisterAssemblies()
        {
            foreach (Assembly assembly in LoadedAssemblies.Values)
            {
                try
                {
                    AssemblyInfos.Add(assembly.GetName().Name, new ValuePair<string, string>(CodeUtils.ToBlob(assembly.GetName().GetPublicKeyToken()), string.Format(".ver {0}:{1}:{2}:{3}", 
                        assembly.GetName().Version.Major, 
                        assembly.GetName().Version.Minor, 
                        assembly.GetName().Version.Build, 
                        assembly.GetName().Version.Revision)));
                }
                catch
                {
                    Exceptions.Exception(21);
                    Logger.Log(" " + assembly.GetName().Name);
                }
            }
        }
    }
}
