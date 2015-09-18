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
        /// <summary>
        /// Halcyon's base commands
        /// </summary>
        internal static List<Command> HalcyonCommands = new List<Command>();
        /// <summary>
        /// Place all dem commands of yours here. NOTE: naming them exactly the same as any of Halcyon's base commands renders them as unusable
        /// </summary>
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

        /// <summary>
        /// Allows you to change Halcyon's core commands
        /// </summary>
        /// <param name="Name">Name of the command you wish to change</param>
        /// <param name="NewCallback">New callback for the command you wish to change. NewCallback.Name must match Name parameter of this method</param>
        /// <returns>
        ///     ChangeResult.Success if successfully changed command
        ///     ChangeResult.NotFound if there is no such command in Halcyon's core commands
        ///     ChangeResult.Fail if the replacement failed
        ///     ChangeResult.SameNameExpected if Name != NewCallback.Name
        /// </returns>
        public static ChangeResult Change(string Name, Command NewCallback)
        {
            if (NewCallback.Name != Name) return ChangeResult.SameNameExpected;
            try
            {
                int index = -1;
                foreach (Command cmd in HalcyonCommands)
                {
                    if (cmd.Name == Name)
                    {
                        index = HalcyonCommands.IndexOf(cmd);
                        break;
                    }
                }
                if (index != -1)
                {
                    HalcyonCommands.Remove(HalcyonCommands[index]);
                    HalcyonCommands.Add(NewCallback);
                    return ChangeResult.Success;
                }
                else
                {
                    return ChangeResult.NotFound;
                }
            }
            catch
            {
                return ChangeResult.Fail;
            }
        }
        /// <summary>
        /// Allows you to run an either Halcyon's core or any extension Command 
        /// with ease
        /// </summary>
        /// <param name="Name">Name of the commands you wish to run</param>
        /// <returns></returns>
        public static bool Run(string Name) 
        {
            return Run(Name, new string[] {});
        }
        /// <summary>
        /// Allows you to run an either Halcyon's core or any extension Command
        /// with ease
        /// </summary>
        /// <param name="Name">Name of the command</param>
        /// <param name="Arguments">Arguments you wish to provide to the command</param>
        /// <returns></returns>
        public static bool Run(string Name, string[] Arguments)
        {
            bool flag = false;
            foreach (Command cmd in HalcyonCommands)
            {
                if (!string.IsNullOrEmpty(Name) 
                    && !string.IsNullOrWhiteSpace(Name) 
                    && cmd.Name == Name)
                {
                    cmd.Callback(Arguments);
                    flag = true;
                    return flag;
                }
            }
            foreach (Command cmd in ExtensionCommands)
            {
                if (!string.IsNullOrEmpty(Name)
                    && !string.IsNullOrWhiteSpace(Name)
                    && cmd.Name == Name)
                {
                    cmd.Callback(Arguments);
                    flag = true;
                    return flag;
                }
            }
            return flag;
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
