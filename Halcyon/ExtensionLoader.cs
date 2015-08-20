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

namespace Halcyon
{
    // TODO: Maybe re-implement a reload functionality for plugins, but you'll have to load all assemblies into their own
    // AppDomain in order to unload them again later. Beware that having them in their own AppDomain might cause threading 
    // problems as usual locks will only work in their own AppDomains.
    public static class PluginLoader
    {
        public const string PluginsPath = "extensions";
        private static readonly Dictionary<string, Assembly> loadedAssemblies = new Dictionary<string, Assembly>();
        private static readonly List<PluginContainer> plugins = new List<PluginContainer>();
        public static string ServerPluginsDirectoryPath
        {
            get;
            private set;
        }
        public static ReadOnlyCollection<PluginContainer> Plugins
        {
            get { return new ReadOnlyCollection<PluginContainer>(plugins); }
        }

        static PluginLoader()
        {
        }

        internal static void Initialize()
        {
            if (!Directory.Exists(ServerPluginsDirectoryPath))
            {
                string lcDirectoryPath =
                    Path.Combine(Path.GetDirectoryName(ServerPluginsDirectoryPath), PluginsPath.ToLower());

                if (Directory.Exists(lcDirectoryPath))
                {
                    Directory.Move(lcDirectoryPath, ServerPluginsDirectoryPath);
                    Console.WriteLine("Case sensitive filesystem detected, extensions directory has been renamed.", TraceLevel.Warning);
                }
                else
                {
                    Directory.CreateDirectory(ServerPluginsDirectoryPath);
                    Console.WriteLine(string.Format(
                    "Folder extensions does not exist. Creating now."),
                    TraceLevel.Info);
                }
            }

            // Add assembly resolver instructing it to use the server plugins directory as a search path.
            // TODO: Either adding the server plugins directory to PATH or as a privatePath node in the assembly config should do too.
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            LoadPlugins();
        }

        internal static void DeInitialize()
        {
            UnloadPlugins();
        }

        internal static void LoadPlugins()
        {
            string ignoredPluginsFilePath = Path.Combine(ServerPluginsDirectoryPath, "ignoredplugins.txt");

            List<string> ignoredFiles = new List<string>();
            if (File.Exists(ignoredPluginsFilePath))
                ignoredFiles.AddRange(File.ReadAllLines(ignoredPluginsFilePath));

            List<FileInfo> fileInfos = new DirectoryInfo(ServerPluginsDirectoryPath).GetFiles("*.dll").ToList();
            fileInfos.AddRange(new DirectoryInfo(ServerPluginsDirectoryPath).GetFiles("*.dll-plugin"));
            foreach (FileInfo fileInfo in fileInfos)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
                try
                {
                    Assembly assembly;
                    // The plugin assembly might have been resolved by another plugin assembly already, so no use to
                    // load it again, but we do still have to verify it and create plugin instances.
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
                            Console.WriteLine(
                                string.Format("Extension \"{0}\" is designed for a different Server API version ({1}) and was ignored.",
                                type.FullName, apiVersion.ToString(2)), TraceLevel.Warning);
                            continue;
                        }
                        HalcyonExtension pluginInstance;
                        pluginInstance = (HalcyonExtension)Activator.CreateInstance(type);
                        try
                        {
                            pluginInstance = (HalcyonExtension)Activator.CreateInstance(type);
                        }
                        catch (Exception ex)
                        {
                            // Broken plugins better stop the entire server init.
                            Console.WriteLine(String.Format("Could not create an instance of plugin class \"{0}\""), type.FullName + "\n" + ex);
                        }
                        plugins.Add(new PluginContainer(pluginInstance));
                    }
                }
                catch (Exception ex)
                {
                    // Broken assemblies / plugins better stop the entire server init.
                    Console.WriteLine(string.Format("Failed to load assembly \"{0}\".", fileInfo.Name) + ex);
                }
            }
            IOrderedEnumerable<PluginContainer> orderedPluginSelector =
                from x in Plugins
                orderby x.Plugin.Order, x.Plugin.Name
                select x;
            try
            {
                int count = 0;
                foreach (PluginContainer current in orderedPluginSelector)
                {
                    count++;
                }
                Console.WriteLine(count);
                foreach (PluginContainer current in orderedPluginSelector)
                {
                    try
                    {
                        current.Initialize();
                    }
                    catch (Exception ex)
                    {
                        // Broken plugins better stop the entire server init.
                        break;
                    }
                    Console.WriteLine(string.Format(
                        "Plugin {0} v{1} (by {2}) initiated.", current.Plugin.Name, current.Plugin.Version, current.Plugin.Author),
                        TraceLevel.Info);
                }
            }
            catch
            {
            }
        }

        internal static void UnloadPlugins()
        {
            foreach (PluginContainer pluginContainer in plugins)
            {
                try
                {
                    pluginContainer.DeInitialize();
                    Console.WriteLine(string.Format(
                        "Plugin \"{0}\" was deinitialized", pluginContainer.Plugin.Name),
                        TraceLevel.Error);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format(
                        "Plugin \"{0}\" has thrown an exception while being deinitialized:\n{1}", pluginContainer.Plugin.Name, ex),
                        TraceLevel.Error);
                }
            }

            foreach (PluginContainer pluginContainer in plugins)
            {


                try
                {
                    pluginContainer.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format(
                        "Plugin \"{0}\" has thrown an exception while being disposed:\n{1}", pluginContainer.Plugin.Name, ex),
                        TraceLevel.Error);
                }


            }
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string fileName = args.Name.Split(',')[0];
            string path = Path.Combine(ServerPluginsDirectoryPath, fileName + ".dll");
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
                Console.WriteLine(
                    string.Format("Error on resolving assembly \"{0}.dll\":\n{1}", fileName, ex),
                    TraceLevel.Error);
            }
            return null;
        }
    }
}

