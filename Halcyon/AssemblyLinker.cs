﻿using Halcyon.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    public class AssemblyLinkerProgram
    {
        
        public static StreamReader output;
        public static Dictionary<string, ValuePair<string, string>> References = new Dictionary<string, ValuePair<string, string>>();

        /// <summary>
        /// This executes AL with args found it AssemblyLinkerInfo
        /// </summary>
        /// <param name="CommandLine"></param>
        /// <param name="FinalFile"></param>
        public static void AssemblyLinker()
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
            Logger.Log(output.ReadToEnd());
            Logger.Log("\nAssemblyLinker job done.");
            Logger.SaveLog();
        }
        public static void Assembly(string CommandLine)
        {
            Process ALProc = new Process();
            ALProc.StartInfo.Arguments = CommandLine;
            ALProc.StartInfo.FileName = Config.ILasmExecutableName;
            ALProc.StartInfo.UseShellExecute = false;
            ALProc.StartInfo.RedirectStandardOutput = true;
            ALProc.StartInfo.CreateNoWindow = true;
            ALProc.Start();
            output = ALProc.StandardOutput;
            Logger.Log(output.ReadToEnd());
            Logger.Log("\nAssemblyLinker job done.");
            Logger.SaveLog();
        }
    }
}
