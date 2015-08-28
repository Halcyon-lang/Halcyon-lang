using Halcyon.Logging;
using Halcyon.Utils;
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
    /// Handles passing the file to ILasm, and using ILasm
    /// Utilizes ILasmInfo and TargetAssembly classes.
    /// </summary>
    public static class ILasmCompiler
    {
        public static string FinalFile = "";
        
        public static StreamReader output;
        public static string Path = System.IO.Path.Combine(Environment.CurrentDirectory, "halc.tmp");

        /// <summary>
        /// This, in fact, is what executes ILasm.
        /// </summary>
        /// <param name="CommandLine"></param>
        /// <param name="FinalFile"></param>
        public static void ILasm(string FinalFile)
        {
            CreateTemporary(FinalFile);
            if (!ILasmInfo.Built)
            {
                ILasmInfo.name = "halc.tmp";
                ILasmInfo.BuildOptions();
            }
            Process ILasmProc = new Process();
            ILasmProc.StartInfo.Arguments = ILasmInfo.CommandLine;
            ILasmProc.StartInfo.FileName = Config.ILasmExecutableName;
            ILasmProc.StartInfo.UseShellExecute = false;
            ILasmProc.StartInfo.RedirectStandardOutput = true;
            ILasmProc.StartInfo.CreateNoWindow = true;
            ILasmProc.Start();
            output = ILasmProc.StandardOutput;
            Logger.Log(output.ReadToEnd());
            DeleteTemporary();
            Logger.Log("\nIlasm job done.");
            Logger.SaveLog();
        }
        public static void ILasmCommand(string CommandLine)
        {
            Process ILasmProc = new Process();
            ILasmProc.StartInfo.Arguments = CommandLine;
            ILasmProc.StartInfo.FileName = Config.ILasmExecutableName;
            ILasmProc.StartInfo.UseShellExecute = false;
            ILasmProc.StartInfo.RedirectStandardOutput = true;
            ILasmProc.StartInfo.CreateNoWindow = true;
            ILasmProc.Start();
            output = ILasmProc.StandardOutput;
            Logger.Log(output.ReadToEnd());
            Logger.Log("\nIlasm job done.");
            Logger.SaveLog();
        }
        /// <summary>
        /// Creates temporary file requiered for execution of ILasm.
        /// </summary>
        /// <param name="file"></param>
        public static void CreateTemporary(string file)
        {
            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
            StreamWriter tempfile = new StreamWriter(Path);
            tempfile.Write(file);
            tempfile.Close();
        }
        /// <summary>
        /// Deletes temporary file used for execution of ILasm
        /// </summary>
        public static void DeleteTemporary()
        {
            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
        }
    }
}
