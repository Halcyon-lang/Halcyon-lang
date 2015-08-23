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
        private static bool Initiated = false;
        public static Dictionary<int, string> list = new Dictionary<int, string>();
        public static void Add(int id, string text)
        {
            list.Add(id, text);
        }
        public static void Exception(int id)
        {
            if (list.ContainsKey(id))
            {
                Logger.Log(string.Format("Exception {0}: {1}", id.ToString(), list[id]));
            }
            else
            {
                Exception(1);
            }
        }
        public static void initExceptions()
        {
            if (!Initiated)
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
                Initiated = true;
            }
        }
    }
}
