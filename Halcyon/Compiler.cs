﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    public static class Compiler
    {
        public static void checkArgs(string[] args)
        {
            switch (args[0])
            {
                case "-compile":
                    if (Program.Talkative) 
                    Console.WriteLine("And here would I place normal compiler, IF I HAD ONE");
                    if (Program.Talkative) 
                    Console.WriteLine("Wait, I atleast do have Preprocessor!");
                    Preprocessor.LoadFile(args[1]);
                    Console.Write("Halcyon:");
                    Console.Title = "Halcyon Compiler";
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case "-help":
                    Utils.printHelp();
                    Console.Write("Halcyon:");
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case "-preprocess":
                    if (Program.Talkative) 
                    Console.WriteLine("I only preprocess");
                    Preprocessor.onlySaveAfterPreprocess = true;
                    Preprocessor.LoadFile(args[1]);
                    Console.Write("Halcyon:");
                    Console.Title = "Halcyon Compiler";
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case "-talkative":
                    Utils.switchTalkative();
                    Console.Write("Halcyon:");
                    Console.Title = "Halcyon Compiler";
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case "-convert":
                    if (Program.Talkative) 
                    Console.WriteLine("And here would I place conversion-only compiler, IF I HAD ONE");
                    if (Program.Talkative) 
                    Console.WriteLine("Wait, I atleast do have Preprocessor!");
                    Preprocessor.LoadFile(args[1]);
                    Console.Write("Halcyon:");
                    Console.Title = "Halcyon Compiler";
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case "-result":
                    if (Program.Talkative) 
                    Console.WriteLine("And here would I place line result, IF I HAD ONE");
                    Console.Write("Halcyon:");
                    Console.Title = "Halcyon Compiler";
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case "-info":
                    Utils.giveInfo(args[1]);
                    Console.Write("Halcyon:");
                    Console.Title = "Halcyon Compiler";
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                default:
                    Errors.Exceptions.Exception(3);
                    Console.Write("Halcyon:");
                    Console.Title = "Halcyon Compiler";
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
            }
        }
    }
}
