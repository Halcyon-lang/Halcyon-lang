using Halcyon.Logging;
using Halcyon.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    public static class WrapperSelector
    {
        public static void Select(string[] args)
        {
            ArrayManipulators<string> arrman = new ArrayManipulators<string>();
            string[] selectargs = arrman.RemoveEmptyEntries(args);
            Logger.TalkyLog(args[0]);
            switch (selectargs[0])
            {
                case "ilasm":
                    Logger.Log(string.Format("ILasm wrapper v{0}.{1}", ApiVersion.ILasmMajor, ApiVersion.ILasmMinor));
                    Logger.TalkyLog(args.Skip(1).ToArray().JoinToString(" "));
                    ILasmCompiler.ILasmCommand(args.Skip(1).ToArray().JoinToString(" "));
                    break;
                case "al":
                    Logger.Log(string.Format("AssemblyLinker wrapper v{0}.{1}", ApiVersion.ALMajor, ApiVersion.ALMinor));
                    Logger.TalkyLog(args.Skip(1).ToArray().JoinToString(" "));
                    AssemblyLinkerProgram.AssemblyLinkerCommand(args.Skip(1).ToArray().JoinToString(" "));
                    break;
                case "help":
                    GeneralUtils.printExecHelp();
                    break;
            }
        }
    }
}
