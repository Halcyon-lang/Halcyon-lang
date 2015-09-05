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
    /// This is the License Compiler wrapper
    /// </summary>
    public class LicenseCompilerProgram
    {
        /// <summary>
        /// What LicenseCompiler sends to Standard output.
        /// </summary>
        public static StreamReader output;
        public static bool silent = false;
        /// <summary>
        /// This executes AL with args found in LicenseCompilerInfo
        /// </summary>
        public static void LicenseCompiler()
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, Config.ALExecutableName)))
            {
                if (!LicenseCompilerInfo.Built)
                {
                    LicenseCompilerInfo.BuildOptions();
                }
                Process LCProc = new Process();
                LCProc.StartInfo.Arguments = LicenseCompilerInfo.CommandLine;
                LCProc.StartInfo.FileName = Config.ALExecutableName;
                LCProc.StartInfo.UseShellExecute = false;
                LCProc.StartInfo.RedirectStandardOutput = true;
                LCProc.StartInfo.CreateNoWindow = true;
                LCProc.Start();
                output = LCProc.StandardOutput;
                if (!silent)
                {
                    Logger.Log(output.ReadToEnd());
                    Logger.Log("\nLicenseCompiler job done.");
                    Logger.SaveLog();
                }
            }
            else
            {
                Exceptions.Exception(23);
            }
        }
        /// <summary>
        /// This executes AL with args found in LicenseCompilerInfo
        /// </summary>
        public static void LicenseCompiler(SpeechMode mode)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, Config.ALExecutableName)))
            {
                if (!LicenseCompilerInfo.Built)
                {
                    LicenseCompilerInfo.BuildOptions();
                }
                Process LCProc = new Process();
                LCProc.StartInfo.Arguments = LicenseCompilerInfo.CommandLine;
                LCProc.StartInfo.FileName = Config.ALExecutableName;
                LCProc.StartInfo.UseShellExecute = false;
                LCProc.StartInfo.RedirectStandardOutput = true;
                LCProc.StartInfo.CreateNoWindow = true;
                LCProc.Start();
                output = LCProc.StandardOutput;
                if (mode == SpeechMode.Normal)
                {
                    Logger.Log(output.ReadToEnd());
                    Logger.Log("\nLicenseCompiler job done.");
                    Logger.SaveLog();
                }
            }
            else
            {
                Exceptions.Exception(23);
            }
        }
        /// <summary>
        /// This executes LicenseCompiler with specified options. No interaction with LicenseCompilerInfo here.
        /// </summary>
        /// <param name="CommandLine"></param>
        public static void LicenseCompilerCommand(string CommandLine)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, Config.ALExecutableName)))
            {
                Process LCProc = new Process();
                LCProc.StartInfo.Arguments = CommandLine;
                LCProc.StartInfo.FileName = Config.ALExecutableName;
                LCProc.StartInfo.UseShellExecute = false;
                LCProc.StartInfo.RedirectStandardOutput = true;
                LCProc.StartInfo.CreateNoWindow = true;
                LCProc.Start();
                output = LCProc.StandardOutput;
                if (!silent)
                {
                    Logger.Log(output.ReadToEnd());
                    Logger.Log("\nLicenseCompiler job done.");
                    Logger.SaveLog();
                }
            }
            else
            {
                Exceptions.Exception(23);
            }
        }
        /// <summary>
        /// This executes LicenseCompiler with specified options. No interaction with LicenseCompilerInfo here. Use SpeechMode.Silent to mute all tracing
        /// </summary>
        /// <param name="CommandLine"></param>
        public static void LicenseCompilerCommand(string CommandLine, SpeechMode mode)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, Config.ALExecutableName)))
            {
                Process LCProc = new Process();
                LCProc.StartInfo.Arguments = CommandLine;
                LCProc.StartInfo.FileName = Config.ALExecutableName;
                LCProc.StartInfo.UseShellExecute = false;
                LCProc.StartInfo.RedirectStandardOutput = true;
                LCProc.StartInfo.CreateNoWindow = true;
                LCProc.Start();
                output = LCProc.StandardOutput;
                if (mode == SpeechMode.Normal)
                {
                    Logger.Log(output.ReadToEnd());
                    Logger.Log("\nLicenseCompiler job done.");
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

