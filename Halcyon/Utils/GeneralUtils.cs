using Halcyon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon.Utils
{
    /// <summary>
    /// Provides various utilities, usually very old (oldest things of Halcyon, to be exact), which doesn't really fit anywhere
    /// or I am too lazy to move them.
    /// </summary>
    public static class GeneralUtils
    {
        /// <summary>
        /// Prints help
        /// </summary>
        public static void printHelp()
        {
            Logger.Log("Halcyon compiler help:");
            foreach (Command cmd in Commands.HalcyonCommands)
            {
                Logger.Log("   -", cmd.Name, " ", cmd.Description);
            }
            foreach (Command cmd in Commands.ExtensionCommands)
            {
                Logger.Log("   -", cmd.Name, " ", cmd.Description);
            }
        }
        /// <summary>
        /// Prints help for giveInfo when user does not provide -info with any arguments
        /// </summary>
        public static void printInfoHelp()
        {
            Logger.LogNoNl("-info\n");
            Logger.LogNoNl("   classes - Prints all classes\n");
            Logger.LogNoNl("   elements - Prints currently loaded elements\n");
            Logger.LogNoNl("   version - Prints current version of Halcyon\n");
            return;
        }
        /// <summary>
        /// Provides user with the info piece they requested with -info command;
        /// </summary>
        /// <param name="arg"></param>
        public static void giveInfo(string arg)
        {
            switch (arg)
            {
                case "elements":
                    Console.WriteLine();
                    foreach (Type type in Assembly.GetCallingAssembly().GetExportedTypes())
                    {
                        if (type.IsSubclassOf(typeof(Element)))
                        {
                            Console.WriteLine(type.Name);
                        }
                    }
                    Console.WriteLine();
                    break;
                case "classes":
                    Console.WriteLine();
                    foreach (Type type in Assembly.GetCallingAssembly().GetExportedTypes())
                    {
                        Console.WriteLine(type.Name);
                    }
                    Console.WriteLine();
                    break;
                case "version":
                    Console.WriteLine();
                    Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Version);
                    Console.WriteLine();
                    break;
                case "help":
                    GeneralUtils.printInfoHelp();
                    break;
                default:
                    Errors.Exceptions.Exception(0);
                    break;
            }
        }
        /// <summary>
        /// Switches talkative mode on and off.
        /// </summary>
        public static void switchTalkative()
        {
            if (Program.Talkative)
            {
                Console.WriteLine("Talkative mode disabled. \n");
                Program.Talkative = false;
            }
            else
            {
                Console.WriteLine("Talkative mode enabled. \n");
                Program.Talkative = true;
            }
        }
        /// <summary>
        /// Gets number of a line
        /// </summary>
        /// <param name="line">The line whose number you want</param>
        /// <param name="container">Either string or StringBuilder</param>
        /// <returns></returns>
        public static int[] getLineNumber(string line, object container)
        {
            int lineCount = 0;
            List<int> lst = new List<int>();
            if (container is StringBuilder)
            {
                StringBuilder sb = (StringBuilder)container;
                foreach (string ln in sb.ToString().Split('\n'))
                {
                    lineCount++;
                    if (ln == line)
                    {
                        lst.Add(lineCount);
                    }
                }
            }
            else if (container is string)
            {
                foreach (string ln in ((string)container).Split('\n'))
                {
                    lineCount++;
                    if (ln == line)
                    {
                        lst.Add(lineCount);
                    }
                }
            }
            else
            {
                lst.Add(-1);
            }
            return lst.ToArray();
        }
        /// <summary>
        /// Returns line based on the number given
        /// </summary>
        /// <param name="lineNumber">Number of the line you want to retrieve</param>
        /// <param name="container">Either StringBuilder or string</param>
        /// <returns></returns>
        public static string getLineByNumber(int lineNumber, object container)
        {
            string Line = "";
            int lineCount = 0;
            if (container is StringBuilder)
            {
                foreach (string line in ((StringBuilder)container).ToString().Split('\n'))
                {
                    lineCount++;
                    if (lineCount == lineNumber)
                    {
                        Line = line;
                    }
                }
            }
            else if (container is string)
            {
                foreach (string line in ((string)container).Split('\n'))
                {
                    lineCount++;
                    if (lineCount == lineNumber)
                    {
                        Line = line;
                    }
                }
            }
            else
            {
                Line = "NF";
            }
            return Line;
        }
        /// <summary>
        /// Removes whitespace at the START of the string, not anywhere else. Note: The only whitespace I know is, in fact, space.
        /// </summary>
        /// <param name="item">string to be ridden of extra whitespace at start</param>
        /// <returns></returns>
        public static string RemoveWhiteSpace(this string item)
        {
            foreach (char ch in item)
            {
                    if (ch == ' ' || ch == '\t' || ch == ' ')
                    {
                        item = item.Substring(1);
                    }
                    else
                    {
                        break;
                    }
            }
            return item;
        }
        /// <summary>
        /// Prints "help string" of WrapperSelector
        /// </summary>
        public static void printExecHelp()
        {
            Logger.Log("Halcyon Exec aka WrapperSelector is a Halcyon utility for executing several .NET tools from managed code.");
            Logger.Log("-exec command is an example of using WrapperSelector at command-line.");
            Logger.Log("Available Options:");
            Logger.Log("   ilasm <any options and file name> - runs ILasm with the specified args");
            Logger.Log("   al <any options and file names> - runs AssemblyLinker with the specified args");
            Logger.Log("   ildasm <any options and file name> runs ILdasm with the specified args");
            Logger.Log("   ngen <any options and file name> runs Native Image Generator (ngen) with the specified args");
            Logger.Log("   lc <any options and file name> runs LicenseCompiler with the specified args");
            Logger.Log("Note that everything is in early stages, so there are still some imperfections");
            Logger.Log("NOTE: using any -exec without <any options and file name> will most likely print it's help string. Only ILdasm doesn't have one so far, so I will have to write it by hand once I have enough docs");
        }
    }
}
