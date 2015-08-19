using System;
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
                    Console.WriteLine("And here would I place normal compiler, IF I HAD ONE");
                    Console.Write("SlashButter:");
                    Console.Title = "SlashButter Compiler";
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case "-help":
                    Utils.printHelp();
                    Console.Write("SlashButter:");
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case "-convert":
                    Console.WriteLine("And here would I place conversion-only compiler, IF I HAD ONE");
                    Console.Write("SlashButter:");
                    Console.Title = "SlashButter Compiler";
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case "-result":
                    Console.WriteLine("And here would I place line result, IF I HAD ONE");
                    Console.Write("SlashButter:");
                    Console.Title = "SlashButter Compiler";
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case "-info":
                    Utils.giveInfo(args[1]);
                    Console.Write("SlashButter:");
                    Console.Title = "SlashButter Compiler";
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                default:
                    Errors.Exceptions.Exception(3);
                    Console.Write("SlashButter:");
                    Console.Title = "SlashButter Compiler";
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
            }

        }
    }
}
