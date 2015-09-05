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
    /// This is the Native Image Generator wrapper
    /// Fun fact: I don't know what's the purpose of NGEN
    /// </summary>
    public class NGENProgram
    {
        /// <summary>
        /// What NGEN sends to Standard output.
        /// </summary>
        public static StreamReader output;
        public static bool silent = false;
        /// <summary>
        /// This executes AL with args found in NGENInfo
        /// </summary>
        public static void NGEN()
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, Config.ALExecutableName)))
            {
                if (!NGENInfo.Built)
                {
                    NGENInfo.BuildOptions();
                }
                Process NGENProc = new Process();
                NGENProc.StartInfo.Arguments = NGENInfo.CommandLine;
                NGENProc.StartInfo.FileName = Config.ALExecutableName;
                NGENProc.StartInfo.UseShellExecute = false;
                NGENProc.StartInfo.RedirectStandardOutput = true;
                NGENProc.StartInfo.CreateNoWindow = true;
                NGENProc.Start();
                output = NGENProc.StandardOutput;
                if (!silent)
                {
                    Logger.Log(output.ReadToEnd());
                    Logger.Log("\nNGEN job done.");
                    Logger.SaveLog();
                }
            }
            else
            {
                Exceptions.Exception(23);
            }
        }
        /// <summary>
        /// This executes AL with args found in NGENInfo
        /// </summary>
        public static void NGEN(SpeechMode mode)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, Config.ALExecutableName)))
            {
                if (!NGENInfo.Built)
                {
                    NGENInfo.BuildOptions();
                }
                Process NGENProc = new Process();
                NGENProc.StartInfo.Arguments = NGENInfo.CommandLine;
                NGENProc.StartInfo.FileName = Config.ALExecutableName;
                NGENProc.StartInfo.UseShellExecute = false;
                NGENProc.StartInfo.RedirectStandardOutput = true;
                NGENProc.StartInfo.CreateNoWindow = true;
                NGENProc.Start();
                output = NGENProc.StandardOutput;
                if (mode == SpeechMode.Normal)
                {
                    Logger.Log(output.ReadToEnd());
                    Logger.Log("\nNGEN job done.");
                    Logger.SaveLog();
                }
            }
            else
            {
                Exceptions.Exception(23);
            }
        }
        /// <summary>
        /// This executes NGEN with specified options. No interaction with NGENInfo here.
        /// </summary>
        /// <param name="CommandLine"></param>
        public static void NGENCommand(string CommandLine)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, Config.ALExecutableName)))
            {
                Process NGENProc = new Process();
                NGENProc.StartInfo.Arguments = CommandLine;
                NGENProc.StartInfo.FileName = Config.ALExecutableName;
                NGENProc.StartInfo.UseShellExecute = false;
                NGENProc.StartInfo.RedirectStandardOutput = true;
                NGENProc.StartInfo.CreateNoWindow = true;
                NGENProc.Start();
                output = NGENProc.StandardOutput;
                if (!silent)
                {
                    Logger.Log(output.ReadToEnd());
                    Logger.Log("\nNGEN job done.");
                    Logger.SaveLog();
                }
            }
            else
            {
                Exceptions.Exception(23);
            }
        }
        /// <summary>
        /// This executes NGEN with specified options. No interaction with NGENInfo here. Use SpeechMode.Silent to mute all tracing
        /// </summary>
        /// <param name="CommandLine"></param>
        public static void NGENCommand(string CommandLine, SpeechMode mode)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, Config.ALExecutableName)))
            {
                Process NGENProc = new Process();
                NGENProc.StartInfo.Arguments = CommandLine;
                NGENProc.StartInfo.FileName = Config.ALExecutableName;
                NGENProc.StartInfo.UseShellExecute = false;
                NGENProc.StartInfo.RedirectStandardOutput = true;
                NGENProc.StartInfo.CreateNoWindow = true;
                NGENProc.Start();
                output = NGENProc.StandardOutput;
                if (mode == SpeechMode.Normal)
                {
                    Logger.Log(output.ReadToEnd());
                    Logger.Log("\nNGEN job done.");
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

