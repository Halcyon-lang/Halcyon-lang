using Halcyon.Logging;
using Halcyon.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    /// <summary>
    /// Entry point of Halcyon
    /// </summary>
    public class Program
    {
        public static string[] consoleArgs;
        public static bool Talkative = false;
        /// <summary>
        /// Events
        /// </summary>
        public static event EventHandler<HandledEventArgs> OnExit = delegate { };
        public static event EventHandler OnStart = delegate { };
        static void Main(string[] args)
        {
            Console.Title = "Halcyon Compiler";
            Errors.Exceptions.initExceptions();
            Config.Initialize();
            Preprocessor.initCommonPreprocessorEvents();
            Preprocessor.initDirectives();
            ILasmInfo.Initialize();
            ExtensionLoader.Initialize();
            Config.PerformCheck();
            OnStart(null, EventArgs.Empty);
            if (args != null)
            {
                consoleArgs = args;
            }
            if (args.Count() >= 2 && args[0] == "-ilasm")
            {
                Logger.Log(string.Format("ILasm wrapper v{0}.{1}", ApiVersion.ILasmMinor, ApiVersion.ILasmMajor));
                ILasmCompiler.ILasmCommand(args.Skip(1).ToArray().JoinToString(" "));
                HalcyonConsole.Command(Console.ReadLine());
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
        /// <summary>
        /// Shut down the program when something goes really really wrong with Halt();
        /// </summary>
        public static void Halt()
        {
            Environment.Exit(1);
        }
    }
}
