using Halcyon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon.Utils
{
    public static class GeneralUtils
    {
        /// <summary>
        /// Prints help
        /// </summary>
        public static void printHelp()
        {
            Logger.Log("Halcyon compiler help:");
            Logger.Log("   -compile [File] - Compiles file to executable");
            Logger.Log("   -convert [File] - Converts file to IL. File will be written down here");
            Logger.Log("   -result [Text] - how does given line look in IL");
            Logger.Log("   -preprocess [File] - preprocesses .halcyon file");
            Logger.Log("   -talkative - turns more commandline info viewed mode ON/OFF");
            Logger.Log("   -info <classes|elements|version> - prints a certain piece of info or -info help");
            Logger.Log("   -ilasm - provides direct access to ILasm. Just type your args behind and let ILasm do its stuff. Still encased in Halcyon");
            Logger.Log("   -ilasmhelp - provides you with that nifty help string it gives you everytime you forget to type");
            Logger.Log("   -exit exits the program safely");
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
            bool found = false;
            foreach (char ch in item)
            {
                if (!found)
                {
                    if (ch == ' ')
                    {
                        item = item.Substring(1);
                    }
                    else
                    {
                        found = true;
                    }
                }
            }
            return item;
        }
    }
}
