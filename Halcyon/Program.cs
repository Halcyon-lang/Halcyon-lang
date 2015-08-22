using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    class Program
    {
        public static string[] consoleArgs;
        public static bool Talkative = false;
        static void Main(string[] args)
        {
            Console.Title = "Halcyon Compiler";
            ExtensionLoader.Initialize();
            Errors.Exceptions.initExceptions();
            Preprocessor.initCommonPreprocessorEvents();
            Preprocessor.initDirectives();
            if (args != null)
            {
                consoleArgs = args;
            }
            switch (args.Count())
            {
                case 0:
                    Utils.printHelp();
                    Console.Write("Halcyon:");
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case 1:
                    Utils.printHelp();
                    Console.Write("Halcyon:");
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case 2:
                    Compiler.checkArgs(args);
                    break;
                default:
                    break;
            }
        }
    }
}
