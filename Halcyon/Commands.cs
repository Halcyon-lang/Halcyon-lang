using Halcyon.Logging;
using Halcyon.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    public class Commands
    {
        internal static List<Command> HalcyonCommands = new List<Command>();
        public static List<Command> ExtensionCommands = new List<Command>();

        static Commands()
        {
            HalcyonCommands.Add(new Command("compile", "- Compiles input file into a managed assembly", Compile));
            HalcyonCommands.Add(new Command("convert", "- Converts Halcyon file to IL, saves file to wanted position", Convert));
            HalcyonCommands.Add(new Command("result", "- Prints result of the input to the console", Result));
            HalcyonCommands.Add(new Command("preprocess", "- Preprocesses a file", Preprocess));
            HalcyonCommands.Add(new Command("talkative", "- Switches verbose mode on and off", Talkative));
            HalcyonCommands.Add(new Command("help", "- Provides a list of all commands", Help));
            HalcyonCommands.Add(new Command("info", "<classes|elements|version> - Prints a certain piece of info or -info help", Info));
            HalcyonCommands.Add(new Command("exec", "<ilasm|al|ildasm> - Allows you to run these through managed code (Halcyon)", Exec));
            HalcyonCommands.Add(new Command("exit", "- shuts the program down", Exit));
        }

        public static void Exit(string[] args)
        {
            Program.End();
        }

        public static void Compile(string[] args)
        {
            if (args.Count() > 1)
            {
                Logger.TalkyLog("And here would I place normal compiler, IF I HAD ONE");
                Logger.TalkyLog("Wait, I atleast do have Preprocessor!");
                Program.Mode = HalcyonMode.Compile;
                Preprocessor.LoadFile(args[1]);
            }
            else
            {
                GeneralUtils.printHelp();
            }
        }

        public static void Info(string[] args)
        {
            if (args.Count() > 1)
            {
                GeneralUtils.giveInfo(args[1]);
            }
            else
            {
                GeneralUtils.printInfoHelp();
            }
        }

        public static void Exec(string[] args)
        {
            if (args.Count() > 1)
            {
                Logger.Log(string.Format("Halcyon WrapperSelector v{0}.{1} started", ApiVersion.WrapperMajor, ApiVersion.WrapperMinor));
                WrapperSelector.Select(args.Skip(1).ToArray());
            }
            else
            {
                GeneralUtils.printExecHelp();
            }
        }

        private static void Result(string[] args)
        {
            Logger.TalkyLog("And here would I place line result, IF I HAD ONE");
            Program.Mode = HalcyonMode.Result;
        }

        public static void Convert(string[] args)
        {
            if (args.Count() > 1)
            {
                Logger.TalkyLog("And here would I place conversion-only compiler, IF I HAD ONE");
                Logger.TalkyLog("Wait, I atleast do have Preprocessor!");
                Program.Mode = HalcyonMode.Convert;
                Preprocessor.LoadFile(args[1]);
            }
            else
            {
                GeneralUtils.printHelp();
            }
        }

        public static void Talkative(string[] args)
        {
            GeneralUtils.switchTalkative();
        }

        public static void Preprocess(string[] args)
        {
            if (args.Count() > 1)
            {
                Logger.TalkyLog("Preprocessor Started");
                Program.Mode = HalcyonMode.Preprocess;
                Preprocessor.LoadFile(args[1]);
            }
            else
            {
                GeneralUtils.printHelp();
            }
        }

        public static void Help(string[] args)
        {
            GeneralUtils.printHelp();
        }
    }
}
