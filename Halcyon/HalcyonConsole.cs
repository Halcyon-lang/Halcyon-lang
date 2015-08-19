﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    class HalcyonConsole
    {
        public static void Command(string str) 
        {
            string[] args = str.Split(Convert.ToChar(" "));
            switch (args.Count())
            {
                case 0:
                    //Utils.printHelp();
                    Console.Write("Halcyon:");
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case 1:
                    if (args[0] != "-compile" && args[0] != "-result" && args[0] != "-convert" && args[0] != "-info" && args[0] != "-help" && !string.IsNullOrEmpty(args[0])) Errors.Exceptions.Exception(4);
                    else if (string.IsNullOrEmpty(args[0]))
                    {
                        Console.Write("SlashButter:");
                        HalcyonConsole.Command(Console.ReadLine());
                    }
                    else if (args[0] == "-info") Utils.printInfoHelp();
                    else if (args[0] == "-help") Utils.printHelp();
                    else
                    {
                        Errors.Exceptions.Exception(0);
                        Console.WriteLine();
                        Utils.printHelp();
                    }
                    Console.Write("SlashButter:");
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
