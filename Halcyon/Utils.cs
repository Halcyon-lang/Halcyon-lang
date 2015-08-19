﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Reflection;

namespace Halcyon
{
    class Utils
    {
        //Why it no work, jeez
        /*[DllImport(@"Utils.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void printHelp();
        [DllImport(@"Utils.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void printInfoHelp();*/
        public static void printHelp()
        {
            Console.WriteLine("SlashButter compiler help:");
            Console.WriteLine("   -compile [File] - Compiles file to executable");
            Console.WriteLine("   -convert [File] - Converts file to IL. File's will be written down here");
            Console.WriteLine("   -result [Text] - how does given line look in IL");
            Console.WriteLine("   -preprocess [File] - preprocesses .halcyon file");
        }
        public static void printInfoHelp()
        {
            printf("-info\n");
            printf("   classes - Prints all classes\n");
            printf("   elements - Prints currently loaded elements\n");
            printf("   version - Prints current version of SlashButter\n");
            return;
        }

        //I am ashamed for this workaround of complete laziness, but I need it to start at least once
        public static void printf(string str)
        {
            Console.Write(str);
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
                    Utils.printInfoHelp();
                    break;
                default:
                    Errors.Exceptions.Exception(0);
                    break;
            }
        }
    }
}
