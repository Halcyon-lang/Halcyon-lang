using System;
using Halcyon.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Halcyon.Utils;

namespace Halcyon
{
    public static class Compiler
    {
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
                    Logger.TalkyLog("I only preprocess");
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
                case "-info":
                    GeneralUtils.giveInfo(args[1]);
                    Console.Write("Halcyon:");
                    Console.Title = "Halcyon Compiler";
                    HalcyonConsole.Command(Console.ReadLine());
                    break;
                case "-ilasm":
                    Logger.Log(string.Format("ILasm wrapper v{0}.{1}", ApiVersion.ILasmMinor, ApiVersion.ILasmMajor));
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
