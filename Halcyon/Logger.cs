﻿using System;
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
        private static bool _Initiated = false;
        public static bool Initiated 
        {
            get { return _Initiated; }
            private set { _Initiated = value; }
        }
        public static StringBuilder LogContent = new StringBuilder();
        public static string LogName = "Halcyon.log";

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
            LogContent.AppendLine();
        }
        /// <summary>
        /// Log showing output to console only when Talkative mode is enabled.
        /// </summary>
        /// <param name="args"></param>
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
            LogContent.AppendLine();
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
        /// <summary>
        /// Logs without telling anyone anything.
        /// </summary>
        /// <param name="args"></param>
        public static void LogNoTrace(params object[] args)
        {
            foreach (object arg in args)
            {
                LogContent.Append(arg);
            }
            LogContent.AppendLine();
        }
        /// <summary>
        /// Inits the logger
        /// </summary>
        public static void Init()
        {
            if (!Initiated)
            {
                Log("Logger initialized.");
                Program.OnExit += ProgramExit;
            }
        }
        
        public static void ProgramExit(object sender, System.ComponentModel.HandledEventArgs e)
        {
            SaveLog();
        }
        /// <summary>
        /// Saves what log got so far.
        /// </summary>
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
            output.Write("[" + DateTime.Now.ToLongTimeString() + "] Halcyon says hi!:");
            output.WriteLine();
            output.Write(LogContent);
            output.Close();
        }
        /// <summary>
        /// Saves log to a file with custom filename.
        /// </summary>
        /// <param name="filename"></param>
        public static void SaveLog(string filename)
        {
            bool SeparateAdditions = false;
            if (!String.IsNullOrEmpty(filename) && !String.IsNullOrWhiteSpace(filename))
            {
                string path = Path.Combine(Environment.CurrentDirectory, filename);
                string existingContents = "";
                if (File.Exists(path))
                {
                    existingContents = File.ReadAllText(path);
                    SeparateAdditions = true;
                }
                StreamWriter output = new StreamWriter(path);
                output.Write(existingContents);
                if (SeparateAdditions)
                {
                    output.WriteLine("\n");
                    output.WriteLine("\n");
                }
                output.Write("\n" + DateTime.Now.ToLongTimeString() + "\n");
                output.Write(LogContent);
                output.Close();
            }
        }
    }
}
