﻿using Halcyon.Logging;
using Halcyon.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    class HalcyonConsole
    {
        /// <summary>
        /// Handles user input.
        /// </summary>
        /// <param name="str"></param>
        public static void Command(string str) 
        {
            string[] args = str.Split(Convert.ToChar(" "));
            Config.Save();
            switch (args.Count())
            {
                case 0:
                    //Utils.printHelp();
                    Console.Write("Halcyon:");
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case 1:
                    if (args[0] != "-compile" && args[0] != "-result" && args[0] != "-convert" && args[0] != "-info" && args[0] != "-help" && args[0] != "-talkative" && args[0] != "-test" && args[0] != "-exit" && args[0] != "-ilasm" && !string.IsNullOrEmpty(args[0])) Errors.Exceptions.Exception(4);
                    else if (string.IsNullOrEmpty(args[0]))
                    {
                        Console.Write("Halcyon:");
                        HalcyonConsole.Command(Console.ReadLine());
                    }
                    else if (args[0] == "-info") GeneralUtils.printInfoHelp();
                    else if (args[0] == "-help") GeneralUtils.printHelp();
                    else if (args[0] == "-talkative") GeneralUtils.switchTalkative();
                    else if (args[0] == "-ilasm")
                    {
                        Logger.Log(string.Format("ILasm wrapper v{0}.{1}", ApiVersion.ILasmMinor, ApiVersion.ILasmMajor));
                        ILasmCompiler.ILasm(" ");
                        Console.Write("Halcyon:");
                        HalcyonConsole.Command(Console.ReadLine());
                    }
                    else if (args[0] == "-test")
                    {
                        Config.logName = "aowfdloabfspagfvjn";
                        Config.Save();
                    }
                    else if (args[0] == "-exit") return;
                    else
                    {
                        Errors.Exceptions.Exception(0);
                        Console.WriteLine();
                        GeneralUtils.printHelp();
                    }
                    Console.Write("Halcyon:");
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case 2:
                    Compiler.checkArgs(args);
                    break;
                default:
                    break;
            }
            Console.ReadLine();
        }
    }
}
