using System;
using Halcyon.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Halcyon.Utils;

namespace Halcyon
{
    /// <summary>
    /// This class was originally suppossed to direct the compilation of Halcyon (SlashButter back then)
    /// Now it handles commands for advanced tasks (not like showing a stoopid help, ok, it does that too)
    /// </summary>
    public static class Compiler
    {
        /// <summary>
        /// Checks args, be it either command args or command-line args
        /// </summary>
        /// <param name="args"></param>
        public static void checkArgs(string[] args)
        {
            Logger.TalkyLog("Halcyon: " + String.Join(" ", args));
            switch (args[0])
            {
                case "-compile":
                    Logger.TalkyLog("And here would I place normal compiler, IF I HAD ONE");
                    Logger.TalkyLog("Wait, I atleast do have Preprocessor!");
                    Program.Mode = HalcyonMode.Compile;
                    Preprocessor.LoadFile(args[1]);
                    Console.Write("Halcyon:");
                    Console.Title = "Halcyon Compiler";
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case "-help":
                    GeneralUtils.printHelp();
                    Console.Write("Halcyon:");
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case "-preprocess":
                    Logger.TalkyLog("Preprocessor Started");
                    Program.Mode = HalcyonMode.Preprocess;
                    Preprocessor.LoadFile(args[1]);
                    Console.Write("Halcyon:");
                    Console.Title = "Halcyon Compiler";
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case "-talkative":
                    GeneralUtils.switchTalkative();
                    Console.Write("Halcyon:");
                    Console.Title = "Halcyon Compiler";
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case "-convert":
                    Logger.TalkyLog("And here would I place conversion-only compiler, IF I HAD ONE");
                    Logger.TalkyLog("Wait, I atleast do have Preprocessor!");
                    Program.Mode = HalcyonMode.Convert;
                    Preprocessor.LoadFile(args[1]);
                    Console.Write("Halcyon:");
                    Console.Title = "Halcyon Compiler";
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case "-result":
                    Logger.TalkyLog("And here would I place line result, IF I HAD ONE");
                    Program.Mode = HalcyonMode.Result;
                    Console.Write("Halcyon:");
                    Console.Title = "Halcyon Compiler";
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case "-exec":
                    Logger.Log(string.Format("Halcyon WrapperSelector v{0}.{1} started", ApiVersion.WrapperMajor, ApiVersion.WrapperMinor));
                    WrapperSelector.Select(args.Skip(1).ToArray());
                    Console.Write("Halcyon:");
                    Console.Title = "Halcyon Compiler";
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case "-info":
                    GeneralUtils.giveInfo(args[1]);
                    Console.Write("Halcyon:");
                    Console.Title = "Halcyon Compiler";
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case "-ilasm":
                    Logger.Log(string.Format("ILasm wrapper v{0}.{1}", ApiVersion.ILasmMajor, ApiVersion.ILasmMinor));
                    ILasmCompiler.ILasmCommand(args.Skip(1).ToArray().JoinToString(" "));
                    Console.Write("Halcyon:");
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case "-ilasmhelp":
                    ILasmCompiler.ILasmCommand(" ");
                    Console.Write("Halcyon:");
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
