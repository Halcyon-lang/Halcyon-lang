using Halcyon.Errors;
using Halcyon.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    /// <summary>
    /// This is the Assembly Linker wrapper
    /// </summary>
    public class AssemblyLinkerProgram
    {
        /// <summary>
        /// What AssemblyLinker sends to Standard output.
        /// </summary>
        public static StreamReader output;
        public static bool silent = false;
        /// <summary>
        /// This executes AL with args found in AssemblyLinkerInfo
        /// </summary>
        public static void AssemblyLinker()
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, Config.ALExecutableName)))
            {
                if (!AssemblyLinkerInfo.Built)
                {
                    AssemblyLinkerInfo.BuildOptions();
                }
                Process ALProc = new Process();
                ALProc.StartInfo.Arguments = AssemblyLinkerInfo.CommandLine;
                ALProc.StartInfo.FileName = Config.ALExecutableName;
                ALProc.StartInfo.UseShellExecute = false;
                ALProc.StartInfo.RedirectStandardOutput = true;
                ALProc.StartInfo.CreateNoWindow = true;
                ALProc.Start();
                output = ALProc.StandardOutput;
                if (!silent)
                {
                    Logger.Log(output.ReadToEnd());
                    Logger.Log("\nAssemblyLinker job done.");
                    Logger.SaveLog();
                }
            }
            else
            {
                Exceptions.Exception(23);
            }
        }
        /// <summary>
        /// This executes AL with args found in AssemblyLinkerInfo
        /// </summary>
        public static void AssemblyLinker(SpeechMode mode)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, Config.ALExecutableName)))
            {
                if (!AssemblyLinkerInfo.Built)
                {
                    AssemblyLinkerInfo.BuildOptions();
                }
                Process ALProc = new Process();
                ALProc.StartInfo.Arguments = AssemblyLinkerInfo.CommandLine;
                ALProc.StartInfo.FileName = Config.ALExecutableName;
                ALProc.StartInfo.UseShellExecute = false;
                ALProc.StartInfo.RedirectStandardOutput = true;
                ALProc.StartInfo.CreateNoWindow = true;
                ALProc.Start();
                output = ALProc.StandardOutput;
                if (mode == SpeechMode.Normal)
                {
                    Logger.Log(output.ReadToEnd());
                    Logger.Log("\nAssemblyLinker job done.");
                    Logger.SaveLog();
                }
            }
            else
            {
                Exceptions.Exception(23);
            }
        }
        /// <summary>
        /// This executes AssemblyLinker with specified options. No interaction with AssemblyLinkerInfo here.
        /// </summary>
        /// <param name="CommandLine"></param>
        public static void AssemblyLinkerCommand(string CommandLine)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, Config.ALExecutableName)))
            {
                Process ALProc = new Process();
                ALProc.StartInfo.Arguments = CommandLine;
                ALProc.StartInfo.FileName = Config.ALExecutableName;
                ALProc.StartInfo.UseShellExecute = false;
                ALProc.StartInfo.RedirectStandardOutput = true;
                ALProc.StartInfo.CreateNoWindow = true;
                ALProc.Start();
                output = ALProc.StandardOutput;
                if (!silent)
                {
                    Logger.Log(output.ReadToEnd());
                    Logger.Log("\nAssemblyLinker job done.");
                    Logger.SaveLog();
                }
            }
            else
            {
                Exceptions.Exception(23);
            }
        }
        /// <summary>
        /// This executes AssemblyLinker with specified options. No interaction with AssemblyLinkerInfo here. Use SpeechMode.Silent to mute all tracing
        /// </summary>
        /// <param name="CommandLine"></param>
        public static void AssemblyLinkerCommand(string CommandLine, SpeechMode mode)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, Config.ALExecutableName)))
            {
                Process ALProc = new Process();
                ALProc.StartInfo.Arguments = CommandLine;
                ALProc.StartInfo.FileName = Config.ALExecutableName;
                ALProc.StartInfo.UseShellExecute = false;
                ALProc.StartInfo.RedirectStandardOutput = true;
                ALProc.StartInfo.CreateNoWindow = true;
                ALProc.Start();
                output = ALProc.StandardOutput;
                if (mode == SpeechMode.Normal)
                {
                    Logger.Log(output.ReadToEnd());
                    Logger.Log("\nAssemblyLinker job done.");
                    Logger.SaveLog();
                }
            }
            else
            {
                Exceptions.Exception(23);
            }
        }
    }
}
