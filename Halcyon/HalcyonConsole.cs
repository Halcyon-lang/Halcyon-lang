using Halcyon.Logging;
using Halcyon.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    public class HalcyonConsole
    {
        public static bool LoopConsole = true;
        /// <summary>
        /// Handles user input.
        /// </summary>
        /// <param name="str"></param>
        public static void Command(string str) 
        {
            ArrayManipulators<string> arrman = new ArrayManipulators<string>();
            string[] args = arrman.RemoveEmptyEntries(str.RemoveWhiteSpace().Split(Convert.ToChar(" ")));
            Config.Save();
            Logger.SaveLog();
            
            if (Config.ConsoleEnabled)
            {
                bool flag = false;
                if (args.Count() >= 1)
                {
                    foreach (Command cmd in Commands.HalcyonCommands)
                    {
                        if (cmd.Name.ToLower() == (args[0].Length == 1 || args[0].Length == 0 ? "" : args[0].Substring(1).ToLower()))
                        {
                            cmd.Callback(args);
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        foreach (Command cmd in Commands.ExtensionCommands)
                        {
                            if (cmd.Name.ToLower() == (args[0].Length == 1 || args[0].Length == 0 ? "" : args[0].Substring(1).ToLower()))
                            {
                                cmd.Callback(args);
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (!flag)
                    {
                        GeneralUtils.printHelp();
                    }
                }
            }
            if (LoopConsole)
            {
                if (Program.Mode != HalcyonMode.None)
                {
                    Referencer.DeInitialize();
                    Program.Mode = HalcyonMode.None;
                }
                Console.Write("Halcyon:");
                Console.Title = "Halcyon Compiler";
                HalcyonConsole.Command(Console.ReadLine());
            }
        }
    }
}
