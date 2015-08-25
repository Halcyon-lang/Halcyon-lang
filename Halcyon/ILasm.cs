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
        public static Process ILasmProc = new Process();
        public static StreamReader output;
        public static string Path = System.IO.Path.Combine(Environment.CurrentDirectory, "halc.tmp");
        public static Dictionary<string, ValuePair<string, string>> References = new Dictionary<string, ValuePair<string, string>>();

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
            
            ILasmProc.StartInfo.Arguments = ILasmInfo.CommandLine;
            ILasmProc.StartInfo.FileName = Config.ILasmExecutableName;
            ILasmProc.StartInfo.UseShellExecute = false;
            ILasmProc.OutputDataReceived += CatchILasmOutput;
            ILasmProc.StartInfo.RedirectStandardOutput = true;
            ILasmProc.StartInfo.CreateNoWindow = true;
            ILasmProc.Start();
            ILasmProc.WaitForExit();
            Logger.Log("\nIlasm job done.");
            DeleteTemporary();
        }
        public static void ILasmCommand(string CommandLine)
        {
            ILasmProc.StartInfo.Arguments = CommandLine;
            ILasmProc.StartInfo.FileName = Config.ILasmExecutableName;
            ILasmProc.StartInfo.UseShellExecute = false;
            ILasmProc.OutputDataReceived += CatchILasmOutput;
            ILasmProc.StartInfo.RedirectStandardOutput = true;
            output = ILasmProc.StandardOutput;
            ILasmProc.StartInfo.CreateNoWindow = true;
            ILasmProc.Start();
            ILasmProc.WaitForExit();
            Logger.Log("\nIlasm job done.");
            Logger.Log(output.ReadToEnd());
        }
        /// <summary>
        /// Handles transferring ILasm output to logs and console windowx 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void CatchILasmOutput(object sender, DataReceivedEventArgs e)
        {
            Logger.Log(e.Data);
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
