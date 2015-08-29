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
    public class ILdasmProgram
    {
        /// <summary>
        /// What ILdasm sends to Standard output.
        /// </summary>
        public static StreamReader output;
        public static bool silent = false;
        /// <summary>
        /// This executes ILdasm with args found in ILdasmInfo
        /// </summary>
        public static void ILdasm()
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, Config.ILdasmExecutableName)))
            {
                if (!ILdasmInfo.Built)
                {
                    ILdasmInfo.BuildOptions();
                }
                Process ILdasmProc = new Process();
                ILdasmProc.StartInfo.Arguments = ILdasmInfo.CommandLine;
                ILdasmProc.StartInfo.FileName = Config.ILdasmExecutableName;
                ILdasmProc.StartInfo.UseShellExecute = false;
                ILdasmProc.StartInfo.RedirectStandardOutput = true;
                ILdasmProc.StartInfo.CreateNoWindow = true;
                ILdasmProc.Start();
                output = ILdasmProc.StandardOutput;
                if (!silent)
                {
                    Logger.Log(output.ReadToEnd());
                    Logger.Log("\nILdasm job done.");
                    Logger.SaveLog();
                }
            }
            else
            {
                Exceptions.Exception(23);
            }
        }
        /// <summary>
        /// This executes ILdasm with args found in ILdasmInfo
        /// </summary>
        public static void ILdasm(SpeechMode mode)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, Config.ILdasmExecutableName)))
            {
                if (!ILdasmInfo.Built)
                {
                    ILdasmInfo.BuildOptions();
                }
                Process ILdasmProc = new Process();
                ILdasmProc.StartInfo.Arguments = ILdasmInfo.CommandLine;
                ILdasmProc.StartInfo.FileName = Config.ILdasmExecutableName;
                ILdasmProc.StartInfo.UseShellExecute = false;
                ILdasmProc.StartInfo.RedirectStandardOutput = true;
                ILdasmProc.StartInfo.CreateNoWindow = true;
                ILdasmProc.Start();
                output = ILdasmProc.StandardOutput;
                if (mode == SpeechMode.Normal)
                {
                    Logger.Log(output.ReadToEnd());
                    Logger.Log("\nILdasm job done.");
                    Logger.SaveLog();
                }
            }
            else
            {
                Exceptions.Exception(23);
            }
        }
        /// <summary>
        /// This executes ILdasm with specified options. No interaction with ILdasmInfo here.
        /// </summary>
        /// <param name="CommandLine"></param>
        public static void ILdasmCommand(string CommandLine)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, Config.ILdasmExecutableName)))
            {
                Process ILdasmProc = new Process();
                ILdasmProc.StartInfo.Arguments = CommandLine;
                ILdasmProc.StartInfo.FileName = Config.ILdasmExecutableName;
                ILdasmProc.StartInfo.UseShellExecute = false;
                ILdasmProc.StartInfo.RedirectStandardOutput = true;
                ILdasmProc.StartInfo.CreateNoWindow = true;
                ILdasmProc.Start();
                output = ILdasmProc.StandardOutput;
                if (!silent)
                {
                    Logger.Log(output.ReadToEnd());
                    Logger.Log("\nILdasm job done.");
                    Logger.SaveLog();
                }
            }
            else
            {
                Exceptions.Exception(23);
            }
        }
        /// <summary>
        /// This executes ILdasm with specified options. No interaction with ILdasmInfo here. Use SpeechMode.Silent to mute all tracing
        /// </summary>
        /// <param name="CommandLine"></param>
        public static void ILdasmCommand(string CommandLine, SpeechMode mode)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, Config.ILdasmExecutableName)))
            {
                Process ILdasmProc = new Process();
                ILdasmProc.StartInfo.Arguments = CommandLine;
                ILdasmProc.StartInfo.FileName = Config.ILdasmExecutableName;
                ILdasmProc.StartInfo.UseShellExecute = false;
                ILdasmProc.StartInfo.RedirectStandardOutput = true;
                ILdasmProc.StartInfo.CreateNoWindow = true;
                ILdasmProc.Start();
                output = ILdasmProc.StandardOutput;
                if (mode == SpeechMode.Normal)
                {
                    Logger.Log(output.ReadToEnd());
                    Logger.Log("\nILdasm job done.");
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
