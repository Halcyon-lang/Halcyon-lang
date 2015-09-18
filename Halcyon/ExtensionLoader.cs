﻿/*
This class is modification of ServerApi.cs from TerrariaServerApi
Copyright (C) 2011-2015 Nyx Studios (fka. The TShock Team)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.
This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Linq;
using System.Runtime.InteropServices;
using Halcyon;
using Halcyon.Logging;

namespace Halcyon
{
    public static class ExtensionLoader
    {
        public const string ExtensionsPath = "extensions";
        private static readonly Dictionary<string, Assembly> loadedAssemblies = new Dictionary<string, Assembly>();
        private static readonly List<ExtensionContainer> extensions = new List<ExtensionContainer>();
        public static string ExtensionsDirectoryPath
        {
            get;
            private set;
        }
        public static ReadOnlyCollection<ExtensionContainer> Extensions
        {
            get { return new ReadOnlyCollection<ExtensionContainer>(extensions); }
        }

        static ExtensionLoader()
        {
        }

        internal static void Initialize()
        {
            Logger.Log(
                string.Format("Halcyon v{0} started.", ApiVersion.Major.ToString() + "." + ApiVersion.Minor.ToString()));
            ExtensionsDirectoryPath = Path.Combine(Environment.CurrentDirectory, ExtensionsPath);
            if (!Directory.Exists(ExtensionsDirectoryPath))
            {
                string lcDirectoryPath =
                    Path.Combine(Path.GetDirectoryName(ExtensionsDirectoryPath), ExtensionsPath.ToLower());

                if (Directory.Exists(lcDirectoryPath))
                {
                    Directory.Move(lcDirectoryPath, ExtensionsDirectoryPath);
                    Logger.Log("Case sensitive filesystem detected, extensions directory has been renamed.");
                }
                else
                {
                    Directory.CreateDirectory(ExtensionsDirectoryPath);
                    Logger.Log(string.Format(
                    "Folder extensions does not exist. Creating now."));
                }
            }
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            LoadExtensions();
        }

        internal static void DeInitialize()
        {
            UnloadExtensions();
        }

        internal static void LoadExtensions()
        {
            List<FileInfo> fileInfos = new DirectoryInfo(ExtensionsDirectoryPath).GetFiles("*.dll").ToList();
            fileInfos.AddRange(new DirectoryInfo(ExtensionsDirectoryPath).GetFiles("*.extension"));
            foreach (FileInfo fileInfo in fileInfos)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
                try
                {
                    Assembly assembly;
                    if (!loadedAssemblies.TryGetValue(fileNameWithoutExtension, out assembly))
                    {
                        try
                        {
                            assembly = Assembly.Load(File.ReadAllBytes(fileInfo.FullName));
                        }
                        catch (BadImageFormatException)
                        {
                            continue;
                        }
                        loadedAssemblies.Add(fileNameWithoutExtension, assembly);
                    }
                    foreach (Type type in assembly.GetExportedTypes())
                    {
                        if (!type.IsSubclassOf(typeof(HalcyonExtension)) || !type.IsPublic || type.IsAbstract)
                            continue;
                        object[] customAttributes = type.GetCustomAttributes(typeof(ApiVersionAttribute), false);
                        if (customAttributes.Length == 0)
                            continue;
                        var apiVersionAttribute = (ApiVersionAttribute)customAttributes[0];
                        Version apiVersion = apiVersionAttribute.ApiVersion;
                        if (apiVersion.Major != ApiVersion.Major || apiVersion.Minor != ApiVersion.Minor)
                        {
                            Logger.Log(
                                string.Format("Extension \"{0}\" is designed for a different Halcyon API version ({1}) and was ignored.",
                                type.FullName, apiVersion.ToString(2)));
                            continue;
                        }
                        HalcyonExtension extensionInstance;
                        extensionInstance = (HalcyonExtension)Activator.CreateInstance(type);
                        try
                        {
                            extensionInstance = (HalcyonExtension)Activator.CreateInstance(type);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(String.Format("Could not create an instance of extension class \"{0}\""), type.FullName + "\n" + ex);
                        }
                        extensions.Add(new ExtensionContainer(extensionInstance));
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(string.Format("Failed to load assembly \"{0}\".", fileInfo.Name) + ex);
                }
            }
            IOrderedEnumerable<ExtensionContainer> orderedExtensionSelector =
                from x in Extensions
                orderby x.Extension.Order, x.Extension.Name
                select x;
            try
            {
                int count = 0;
                foreach (ExtensionContainer current in orderedExtensionSelector)
                {
                    count++;
                }
                foreach (ExtensionContainer current in orderedExtensionSelector)
                {
                    try
                    {
                        current.Initialize();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogNoTrace(ex.Message);
                        Logger.LogNoTrace(ex.Source);
                        Logger.LogNoTrace(ex.HelpLink);
                        Logger.LogNoTrace(ex.StackTrace);
                        // Broken extensions better stop the entire server init.
                        break;
                    }
                    Logger.Log(string.Format(
                        "Extension {0} v{1} (by {2}) initiated.", current.Extension.Name, current.Extension.Version, current.Extension.Author));
                }
            }
            catch
            {
            }
        }

        internal static void UnloadExtensions()
        {
            foreach (ExtensionContainer extensionContainer in extensions)
            {
                try
                {
                    extensionContainer.DeInitialize();
                    Logger.Log(string.Format(
                        "Extension \"{0}\" was deinitialized", extensionContainer.Extension.Name));
                }
                catch (Exception ex)
                {
                    Logger.Log(string.Format(
                        "Extension \"{0}\" has thrown an exception while being deinitialized:\n{1}", extensionContainer.Extension.Name, ex));
                }
            }

            foreach (ExtensionContainer extensionContainer in extensions)
            {


                try
                {
                    extensionContainer.Dispose();
                }
                catch (Exception ex)
                {
                    Logger.Log(string.Format(
                        "Extension \"{0}\" has thrown an exception while being disposed:\n{1}", extensionContainer.Extension.Name, ex));
                }


            }
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string fileName = args.Name.Split(',')[0];
            string path = Path.Combine(ExtensionsDirectoryPath, fileName + ".dll");
            try
            {
                if (File.Exists(path))
                {
                    Assembly assembly;
                    if (!loadedAssemblies.TryGetValue(fileName, out assembly))
                    {
                        assembly = Assembly.Load(File.ReadAllBytes(path));
                        loadedAssemblies.Add(fileName, assembly);
                    }
                    return assembly;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    string.Format("Error on resolving assembly \"{0}.dll\":\n{1}", fileName, ex));
            }
            return null;
        }
    }
}

