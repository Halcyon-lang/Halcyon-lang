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
        /// <summary>
        /// Args given to the console on launch of Halcyon
        /// </summary>
        public static string[] consoleArgs;
        /// <summary>
        /// Verbose tracing
        /// </summary>
        public static bool Talkative = false;
        /// <summary>
        /// Path of the selected file
        /// </summary>
        public static string Path;
        /// <summary>
        /// Compilation mode
        /// </summary>
        public static HalcyonMode Mode;
        /// <summary>
        /// Triggered when Halcyon exits through -exit command
        /// </summary>
        public static event EventHandler<HandledEventArgs> OnExit = delegate { };
        /// <summary>
        /// Triggered during initialization of Halcyon
        /// </summary>
        public static event EventHandler OnStart = delegate { };

        /// <summary>
        /// Entrypoint of Halcyon
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.Title = "Halcyon Compiler";
            //Errors.Exceptions.initExceptions();
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
