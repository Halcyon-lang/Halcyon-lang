using Halcyon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon.Errors
{
    public static class Exceptions
    {
        private static int _Exceptions = 0;
        public static int ExceptionsSoFar
        {
            get { return _Exceptions; }
            private set { _Exceptions = value; }
        }
        public static Dictionary<int, string> List = new Dictionary<int, string>();
        public static void Add(int id, string text)
        {
            List.Add(id, text);
        }
        public static void Exception(int id)
        {
            ExceptionsSoFar++;
            if (ExceptionsSoFar <= 50)
            {
                if (List.ContainsKey(id))
                {
                    Logger.Log(string.Format("Exception {0}: {1}", id.ToString(), List[id]));
                }
                else
                {
                    Exception(1);
                }
                if (ExceptionsSoFar == 49) Exception(69);
            }
        }
        public static void Exception(int id, Exception e)
        {
            ExceptionsSoFar++;
            if (ExceptionsSoFar <= 50)
            {
                if (List.ContainsKey(id))
                {
                    Logger.Log(string.Format("Exception {0}: {1}", id.ToString(), List[id]));
                    Logger.Log("in " + e.Source);
                    Logger.Log(e.Message);
                    Logger.Log(e.StackTrace);
                }
                else
                {
                    Exception(1);
                }
                if (ExceptionsSoFar == 49) Exception(69);
            }
        }
        static Exceptions()
        {
                Add(0, "Invalid number of command-line arguments");
                Add(1, "Exception does not exist");
                Add(2, "Target file not found");
                Add(3, "Argument not found");
                Add(4, "Command does not exist");
                Add(5, "Unknown file type");
                Add(6, "Unable to read file");
                Add(7, "Include file not found");
                Add(8, "Invalid directive syntax");
                Add(9, "Unsupported header");
                Add(10, "Each #define key has to be unique");
                Add(11, "Log name cannot be empty or whitespace only.");
                Add(12, "Duplicit declarations in config.");
                Add(13, "Getting key failed");
                Add(14, "ILasm: Target option requires extra information provided");
                Add(15, "ILasm: Target option does not exist");
                Add(16, "ILasm: Invalid extra info provided");
                Add(17, "ILasm: Compilation has failed");
                Add(18, "Key in config not found, please delete your config and let it generate again, or add the following key manually");
                Add(19, "Extension for resource file has to be .res");
                Add(20, "Expected string convertible to integer");
                Add(21, "Failed to fetch publickeytoken of a referenced assembly:");
                Add(22, "Path needs to be either absolute or start with -\\");
                Add(23, "AL executable not found. Please pay attention to INSTALL.md. Also if you changed filenames in config, make sure they at least match.");
                Add(24, "ILasm executable not found. Please pay attention to INSTALL.md. Also if you changed filenames in config, make sure they at least match.");
                Add(25, "File does not start with assembly-block, if you intend to preprocess something else then code, use -preprocess compiler command.");
                Add(26, "Could not parse bool in config. Presuming false");
                Add(27, "LC executable not found. Make sure it is in the same folder as Halcyon");
                Add(28, "NGEN executable not found. Make sure it is in the same folder as Halcyon");
                Add(69, "Too many exceptions, are you fucking kidding me??");
        }
    }
}
