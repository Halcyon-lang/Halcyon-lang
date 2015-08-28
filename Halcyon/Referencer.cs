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
    /// <summary>
    /// Used for retrieving ReferenceStrings of assemblies, which, is then required to reference any assembly in Halcyon
    /// </summary>
    public static class Referencer
    {
        /// <summary>
        /// The folder of th
        /// </summary>
        public static string Folder;
        /// <summary>
        /// Contains information required for building ReferenceStrings of all assemblies that are : 
        ///     loaded, 
        ///     or in the same folder as the .halcyon file (or where Initialize specifies),
        ///     mscorlib and other that are loaded automatically or so
        /// </summary>
        public static Dictionary<string, ValuePair<string, string>> AssemblyInfos = new Dictionary<string,ValuePair<string,string>>();
        /// <summary>
        /// All assemblies loaded
        /// </summary>
        public static readonly Dictionary<string, Assembly> LoadedAssemblies = new Dictionary<string, Assembly>();
        /// <summary>
        /// Used to init Referencer. Call this method always before you want to use Referencer. DeInitialize after your job is done;
        /// <param name="path">Everything on where Path.GetDirectoryName() works</param>
        /// </summary>
        public static void Initialize(string path)
        {
            Logger.TalkyLog("Initiating Referencer.");
            Folder = Path.GetDirectoryName(path);
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            LoadAssemblies();
        }
        /// <summary>
        /// After finishing the compilation/conversion job, always DeInitialize referencer. 
        /// It basically resets it to a state in which all them assemblies can be
        /// </summary>
        public static void DeInitialize()
        {
            Folder = "";
            AssemblyInfos.Clear();
            LoadedAssemblies.Clear();
            Logger.TalkyLog("Referencer restarted.");
        }
        /// <summary>
        /// Loads all assemblies from folder where target .halcyon file is
        /// </summary>
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
                    // Broken assemblies better stop the entire referencer init.
                    Logger.Log(string.Format("Failed to load assembly \"{0}\".", fileInfo.Name) + ex);
                }
            }
            RegisterAssemblies();
        }
        /// <summary>
        /// Hooking up onto AssemblyResolveEvent to be able to register even already resolved assemblies.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Registers the names, publickeytokens in the form of blobs and versions of all resolved assemblies to AssemblyInfos so they can be later retrieved in ReferenceString
        /// or whatever needs them.
        /// </summary>
        public static void RegisterAssemblies()
        {
            foreach (Assembly assembly in LoadedAssemblies.Values)
            {
                if (!AssemblyInfos.ContainsKey(assembly.GetName().Name))
                {
                    AssemblyInfos.Add(assembly.GetName().Name, new ValuePair<string, string>(CodeUtils.ToBlob(assembly.GetName().GetPublicKeyToken()), string.Format(".ver {0}:{1}:{2}:{3}",
                    assembly.GetName().Version.Major.ToString(),
                    assembly.GetName().Version.Minor.ToString(),
                    assembly.GetName().Version.Build.ToString(),
                    assembly.GetName().Version.Revision.ToString())));
                    if(assembly.GetName().Name != "Halcyon") 
                    {
                        Logger.TalkyLog("registering ", assembly.GetName().Name, CodeUtils.ToBlob(assembly.GetName().GetPublicKeyToken()), string.Format(".ver {0}:{1}:{2}:{3}",
                        assembly.GetName().Version.Major.ToString(),
                        assembly.GetName().Version.Minor.ToString(),
                        assembly.GetName().Version.Build.ToString(),
                        assembly.GetName().Version.Revision.ToString()));
                    }
                }
            }
            //Better loop through CurrentDomain as well so we can get all the good stuff like mscorlib
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!AssemblyInfos.ContainsKey(assembly.GetName().Name))
                {
                    AssemblyInfos.Add(assembly.GetName().Name, new ValuePair<string, string>(CodeUtils.ToBlob(assembly.GetName().GetPublicKeyToken()), string.Format(".ver {0}:{1}:{2}:{3}",
                    assembly.GetName().Version.Major.ToString(),
                    assembly.GetName().Version.Minor.ToString(),
                    assembly.GetName().Version.Build.ToString(),
                    assembly.GetName().Version.Revision.ToString())));
                    if(assembly.GetName().Name != "Halcyon") 
                    {
                        Logger.TalkyLog("registering ", assembly.GetName().Name, CodeUtils.ToBlob(assembly.GetName().GetPublicKeyToken()), string.Format(".ver {0}:{1}:{2}:{3}",
                        assembly.GetName().Version.Major.ToString(),
                        assembly.GetName().Version.Minor.ToString(),
                        assembly.GetName().Version.Build.ToString(),
                        assembly.GetName().Version.Revision.ToString()));
                    }
                }
            }

            //Halcyon's publickeytoken does not load properly in the way above. Therefore we need to get it manually.
            AssemblyInfos.Remove("Halcyon");
            AssemblyInfos.Add("Halcyon", new ValuePair<string, string>(CodeUtils.ToBlob(Assembly.GetAssembly(typeof(Halcyon.Referencer)).GetName().GetPublicKeyToken()), string.Format(".ver {0}:{1}:{2}:{3}",
                        Assembly.GetAssembly(typeof(Halcyon.Referencer)).GetName().Version.Major.ToString(),
                        Assembly.GetAssembly(typeof(Halcyon.Referencer)).GetName().Version.Minor.ToString(),
                        Assembly.GetAssembly(typeof(Halcyon.Referencer)).GetName().Version.Build.ToString(),
                        Assembly.GetAssembly(typeof(Halcyon.Referencer)).GetName().Version.Revision.ToString())));
            Logger.TalkyLog("registering ", Assembly.GetAssembly(typeof(Halcyon.Referencer)).GetName().Name, CodeUtils.ToBlob(Assembly.GetAssembly(typeof(Halcyon.Referencer)).GetName().GetPublicKeyToken()), string.Format(".ver {0}:{1}:{2}:{3}",
                        Assembly.GetAssembly(typeof(Halcyon.Referencer)).GetName().Version.Major.ToString(),
                        Assembly.GetAssembly(typeof(Halcyon.Referencer)).GetName().Version.Minor.ToString(),
                        Assembly.GetAssembly(typeof(Halcyon.Referencer)).GetName().Version.Build.ToString(),
                        Assembly.GetAssembly(typeof(Halcyon.Referencer)).GetName().Version.Revision.ToString()));
        }
        /// <summary>
        /// Generates whole .assembly extern block for referencing another assembly. 
        /// The referenced assembly must be in the same folder as .halcyon file which is being converted/compiled
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ReferenceString(string name)
        {
            StringBuilder temp = new StringBuilder("");
            if (AssemblyInfos.ContainsKey(name))
            {
                temp.AppendLine(".assembly extern " + name);
                temp.AppendLine("{");
                temp.AppendLine(".publickeytoken = " + AssemblyInfos[name].LeftValue);
                temp.AppendLine(AssemblyInfos[name].RightValue);
                temp.AppendLine("}");
            }
            return temp.ToString();
        }
    }
}
