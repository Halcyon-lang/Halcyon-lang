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
        public static HalcyonMode Mode = HalcyonMode.None;
        /// <summary>
        /// Triggered when Halcyon exits through -exit command or End() method
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
            Config.Initialize();
            Preprocessor.initCommonPreprocessorEvents();
            Preprocessor.initDirectives();
            ExtensionLoader.Initialize();
            Config.PerformCheck();
            OnStart(null, EventArgs.Empty);
            if (args != null)
            {
                consoleArgs = args;
                HalcyonConsole.Command(consoleArgs.JoinToString(" "));
            }
            HalcyonConsole.Command(Console.ReadLine());
            OnExit(null, new HandledEventArgs());
        }
        /// <summary>
        /// Shut down the program when something goes really really wrong with Halt();
        /// </summary>
        public static void Halt()
        {
            Environment.Exit(1);
        }

        /// <summary>
        /// Shut down Halcyon
        /// </summary>
        public static void End()
        {
            OnExit(null, new HandledEventArgs());
            Environment.Exit(0);
        }
    }
}
