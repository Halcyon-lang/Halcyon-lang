using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon.Logging
{
    /// <summary>
    /// Halcyon logger. Handles logging and saving logs to a file.
    /// </summary>
    public static class Logger
    {
        static StringBuilder LogContent = new StringBuilder("Halcyon Log: \n");

        /// <summary>
        /// Logs input values into log file, which is saved upon end of the program. 
        /// </summary>
        /// <param name="args"> Anything convertible to string </param>
        public static void Log(params object[] args)
        {
            foreach(object arg in args) 
            {
                Console.Write(arg);
                LogContent.Append(arg);
            }
            Console.WriteLine();
            LogContent.Append("\n");
        }
        public static void TalkyLog(params object[] args)
        {
            foreach (object arg in args)
            {
                if (Program.Talkative)
                {
                    Console.Write(arg);
                }
                LogContent.Append(arg);
            }
            if (Program.Talkative)
            {
                Console.WriteLine();
            }
            LogContent.Append("\n");
        }

        /// <summary>
        /// Logs input values into log file, but does not create a new line.
        /// </summary>
        /// <param name="args"> Anything convertible to string </param>
        public static void LogNoNl(params object[] args)
        {
            foreach (object arg in args)
            {
                Console.Write(arg);
                LogContent.Append(arg);
            }
        }
        public static void Init()
        {
            Log("Logger initialized");
            Program.OnExit += ProgramExit;
        }

        public static void ProgramExit(object sender, System.ComponentModel.HandledEventArgs e)
        {
            SaveLog();
        }

        public static void SaveLog()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Halcyon.log");
            string existingContents = "";
            if(File.Exists(path)) 
            {
                existingContents = File.ReadAllText(path);
            }
            StreamWriter output = new StreamWriter(path);
            output.Write(existingContents);
            output.Write("\n" + DateTime.Now.ToLongTimeString() + ":\n");
            output.Write(LogContent);
            output.Close();
        }
    }
}
