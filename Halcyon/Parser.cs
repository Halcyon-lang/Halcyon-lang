using Halcyon.Errors;
using Halcyon.Logging;
using Halcyon.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    public static class ParserSwitch
    {
        public static string File = "";
        public static void Start(string file)
        {
            Logger.TalkyLog("Parsing started");
            File = file;
            if (CodeUtils.RemoveEmptyLines(File).StartsWith("assembly", StringComparison.CurrentCultureIgnoreCase))
            {
                string assemblyblock = CodeUtils.GetCodeBlocks(CodeUtils.RemoveEmptyLines(File))[0];
                Logger.TalkyLog("AssemblyBlock: ");
                Logger.TalkyLog(assemblyblock);
                AssemblyInfoParser.Parse(assemblyblock);
            }
            else if (!CodeUtils.RemoveEmptyLines(File).StartsWith("assembly", StringComparison.CurrentCultureIgnoreCase) && Program.Mode != HalcyonMode.Preprocess)
            {
                Exceptions.Exception(25);
            }
        }
    }
    public static class AssemblyInfoParser
    {
        public static string AssemblyBlock;
        /// <summary>
        /// Don't use. It changes and is eventually reduced to empty.
        /// </summary>
        public static string DisassembledBlock;

        public static void Parse(string block)
        {
            int curlyIndex = block.IndexOf('{');
            AssemblyBlock = block;

            SetName(block.Substring(0, curlyIndex));
            DisassembledBlock = block.Substring(curlyIndex + 1);
            DisassembledBlock = DisassembledBlock.RemoveIfNotString('}');
            ProcessInfo();

        }
        public static void ProcessInfo()
        {
            string[] rawstatements;
            rawstatements = DisassembledBlock.Split(';');
            List<string> statements = new List<string>();
            foreach (string rawstatement in rawstatements)
            {
                statements.Add(rawstatement.RemoveWhiteSpace());
                Logger.TalkyLog("statement: " + rawstatement);
            }
        }

        public static void SetName(string assemblypart)
        {
            TargetAssembly.Name = assemblypart.Replace("assembly", "").Trim();
            Logger.TalkyLog(TargetAssembly.Name);
        }
    }
}
 