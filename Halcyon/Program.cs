using Halcyon.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    class Program
    {
        public static string[] consoleArgs;
        public static bool Talkative = false;
        public static event EventHandler<HandledEventArgs> OnExit = delegate { };
        static void Main(string[] args)
        {
            Console.Title = "Halcyon Compiler";
            ExtensionLoader.Initialize();
            Config.Initialize();
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
                    GeneralUtils.printHelp();
                    Console.Write("Halcyon:");
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case 1:
                    GeneralUtils.printHelp();
                    Console.Write("Halcyon:");
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case 2:
                    Compiler.checkArgs(args);
                    break;
                default:
                    GeneralUtils.printHelp();
                    Console.Write("Halcyon:");
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
            }
            OnExit(null, new HandledEventArgs());
        }

        public static void Halt()
        {
            Environment.Exit(1);
        }
    }
}
