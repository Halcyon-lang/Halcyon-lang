using Halcyon.CustomEventArgs;
using Halcyon.Errors;
using Halcyon.Logging;
using Halcyon.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    public delegate void DirectiveCallback(string line);
    public class Preprocessor
    {
        #region declarations
        //Events
        public static event EventHandler<LoadFileArgs> OnLoadFile = delegate { };
        public static event EventHandler<ReadFileArgs> OnReadFile = delegate { };
        public static event EventHandler<HandledEventArgs> OnStart = delegate { };
        public static event EventHandler<HandledEventArgs> OnReset = delegate { };
        public static event EventHandler<HandledEventArgs> OnPreprocessCompleted = delegate { };
        public static event EventHandler<HandledEventArgs> OnInitDirectives = delegate { };
        public static event EventHandler<PreprocessEventArgs> OnPreprocess = delegate { };
        public static event EventHandler<LineEventArgs> OnNextLine;
        public static event EventHandler<HandledEventArgs> OnLoadFileFailed;
        public static event EventHandler<HandledEventArgs> OnReadFileFailed;
        //Even trigger wrappers
        public static void NextLine(object sender, LineEventArgs args) { OnNextLine(sender, args); }
        public static void LoadFileFailed(object sender, HandledEventArgs args) { OnLoadFileFailed(sender, args); }
        public static void ReadFileFailed(object sender, HandledEventArgs args) { OnReadFileFailed(sender, args); }
        //fields
        public static string[] InputFile;
        public static StringBuilder PreprocessedFile = new StringBuilder();
        public static string FilePath = "";
        public static List<Directive> DirectiveList = new List<Directive>();
        public static bool onlySaveAfterPreprocess = false;
        private static bool EventsInitialized = false;
        public static bool firstrun = true;
        #endregion
        //Functions
        public static void LoadFile(string path) 
        {
            Logger.TalkyLog("Loadfile started");
            OnStart(null, new HandledEventArgs());
            OnLoadFile(null, new LoadFileArgs(path));
        }
        public static void ReadFile(string path)
        {
            OnReadFile(null, new ReadFileArgs(path));
        }
        public static void Preproccess(string[] file)
        {
            OnPreprocess(null, new PreprocessEventArgs(file));
            firstrun = false;
            OnPreprocessCompleted(null, new HandledEventArgs());
            Logger.TalkyLog("Firing OnReset");
            OnReset(null, new HandledEventArgs());
            Console.WriteLine("Task completed. \n");
        }
        public static void initDirectives()
        {
            OnInitDirectives(null, new HandledEventArgs());
        }
        /// <summary>
        /// Basically turns on the preprocessor.
        /// </summary>
        public static void initCommonPreprocessorEvents()
        {
            if (!EventsInitialized)
            {
                OnPreprocessCompleted += PreprocessorEvents.Preprocessor_DefineOnPreprocessCompleted;
                OnPreprocessCompleted += PreprocessorEvents.Preprocessor_OnPreprocessCompleted;
                Preprocessor.OnNextLine += PreprocessorEvents.Preprocessor_OnNextLine;
                OnReset += PreprocessorEvents.Preprocessor_OnReset;
                OnInitDirectives += PreprocessorEvents.InitDirectives;
                OnPreprocess += PreprocessorEvents.Preprocess;
                OnLoadFile += PreprocessorEvents.LoadFile;
                OnReadFile += PreprocessorEvents.ReadFile;
                EventsInitialized = true;
            }
        }
        /// <summary>
        /// For adding Directives
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        public static void Add(string name, DirectiveCallback callback)
        {
            DirectiveList.Add(new Directive(name, callback));
        }
    }

    public class Directive
    {
        public string Name;
        public DirectiveCallback callback;

        public Directive(string name, DirectiveCallback Callback) 
        {
            Name = name;
            callback = Callback;
        }
    }

    public static class Callbacks
    {
        public static Dictionary<string, string> DefineList = new Dictionary<string, string>();
        internal static void Include(string line)
        {
            string temp = line;
            string file = "";
            string defaultIncludePath = Path.Combine(Environment.CurrentDirectory, @"include\");
            temp = temp.Replace("#include", "");
            if (temp.Trim().StartsWith("\""))
            {
                temp = temp.Trim().Replace("\"", "");
                if (File.Exists(Path.Combine(Path.GetDirectoryName(Preprocessor.FilePath), temp)))
                    file = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Preprocessor.FilePath), temp));
                else Exceptions.Exception(7);
            }
            else if (temp.Trim().StartsWith("<"))
            {
                temp = temp.Trim().Replace("<", "");
                temp = temp.Trim().Replace(@">", "");
                Logger.TalkyLog("Include " + temp);
                Logger.TalkyLog("Environment.CurrentDirectory: " + Environment.CurrentDirectory);
                Logger.TalkyLog("Default Include Path: " + defaultIncludePath);
                Logger.TalkyLog("Include file path:" + Path.Combine(defaultIncludePath, temp.Trim()));
                try 
                {
                    file = File.ReadAllText(Path.Combine(defaultIncludePath, temp.Trim()));
                }
                catch
                {
                    Exceptions.Exception(7);
                    return;
                }
            }
            else
            {
                Exceptions.Exception(8);
                return;
            }

            if (!file.StartsWith("#Halcyon"))
            {
                Exceptions.Exception(9);
            }
            else
            {
                Preprocessor.PreprocessedFile.Append(file.Replace("#Halcyon", ""));
            }
        }
        public static void Define(string line)
        {
            string temp = line.Replace("#define ", "");
            if (Program.Talkative)
            {
                foreach (string str in temp.Split(' '))
                {
                    Console.WriteLine(str);
                }
            }
            string key = temp.Split(Convert.ToChar(" ")).First();
            Logger.TalkyLog(temp.Replace(" " + key + " ", ""));
            string value = temp.Replace(key + " ", "");
            if (!DefineList.ContainsKey(key))
            {
                DefineList.Add(key, value);
            }
            else if (DefineList.ContainsKey(key) && DefineList[key] == value)
            {
                return;
            }
            else
            {
                Exceptions.Exception(10);
            }
        }
        public static void Define(string line, object secondCall)
        {
            Preprocessor.PreprocessedFile = Preprocessor.PreprocessedFile.Replace(line, "");
            string temp = line.Replace("#define ", "");
            if (Program.Talkative)
            {
                temp = temp.RemoveWhiteSpace();
                foreach (string str in temp.Split(' '))
                {
                    Console.WriteLine(str);
                }
            }
            string key = temp.Split(Convert.ToChar(" ")).First();
            Logger.TalkyLog(temp.Replace(" " + key + " ", ""));
            string value = temp.Replace(key + " ", "");
            if (!DefineList.ContainsKey(key))
            {
                DefineList.Add(key, value);
            }
            else if (DefineList.ContainsKey(key) && DefineList[key] == value)
            {
                return;
            }
            else
            {
                Exceptions.Exception(10);
            }
        }
    }

    public static class PreprocessorEvents 
    {
        #region Listeners
        public static void Preprocessor_DefineOnPreprocessCompleted(object sender, EventArgs e)
        {
            foreach (string line in Preprocessor.PreprocessedFile.ToString().Split('\n'))
            {
                if (line.Trim().StartsWith("#define"))
                {
                    Callbacks.Define(line, null);
                }
            }
            foreach (string key in Callbacks.DefineList.Keys)
            {
                if (!String.IsNullOrEmpty(key))
                {
                    Logger.TalkyLog("Key: " + key);
                    Logger.TalkyLog("Replacing " + key + " with " + Callbacks.DefineList[key]);
                    Preprocessor.PreprocessedFile = Preprocessor.PreprocessedFile.Replace(key, Callbacks.DefineList[key]);
                }
            }
        }
        public static void Preprocessor_OnNextLine(object sender, LineEventArgs e)
        {
            if (!e.Handled)
            {
                if (e.Line.Trim().StartsWith("#"))
                {
                    foreach (Directive dir in Preprocessor.DirectiveList)
                    {
                        if (dir.Name == e.Line.Split(Convert.ToChar(" ")).First().Replace("#", ""))
                        {
                            dir.callback(e.Line);
                        }
                    }
                }
                else
                {
                    Preprocessor.PreprocessedFile.AppendLine(e.Line);
                }
            }
        }
        public static void Preprocessor_OnReset(object sender, HandledEventArgs e)
        {
            if (!e.Handled)
            {
                Preprocessor.PreprocessedFile.Clear();
                Callbacks.DefineList.Clear();
            }
        }
        public static void InitDirectives(object sender, HandledEventArgs e)
        {
            if (!e.Handled)
            {
                Preprocessor.Add("include", Callbacks.Include);
                Preprocessor.Add("define", Callbacks.Define);
            }
        }
        public static void Preprocess(object sender, PreprocessEventArgs e)
        {
            if (!e.Handled)
            {
                Logger.TalkyLog("Preproccess started");
                foreach (string line in e.file)
                {
                    Logger.TalkyLog("Looping through lines.");
                    Preprocessor.NextLine(null, new LineEventArgs(line));
                }
            }
        }
        public static void Preprocessor_OnPreprocessCompleted(object sender, HandledEventArgs e)
        {
            if (!e.Handled)
            {
                Logger.TalkyLog("Post-preprocessor actions started");
                if (Program.Mode == HalcyonMode.Preprocess)
                {
                    Logger.TalkyLog("Writing file " + Path.GetFileNameWithoutExtension(Preprocessor.FilePath) + ".halp");
                    System.IO.StreamWriter output = new System.IO.StreamWriter(Path.Combine(Environment.CurrentDirectory, Path.GetFileNameWithoutExtension(Preprocessor.FilePath) + ".halp"));
                    output.Write(Preprocessor.PreprocessedFile + "\n");
                    output.Close();
                    Logger.SaveLog();
                }
                else
                {
                    Logger.TalkyLog("Preprocessing completed. Procceeding to Assembly information parsing");
                    Referencer.Initialize();
                    //ParserSwitch.Start(Preprocessor.PreprocessedFile.ToString());
                }
            }
        }
        public static void LoadFile(object sender, LoadFileArgs e)
        {
            if (!e.Handled)
            {
                string realPath;
                try
                {
                    Logger.TalkyLog("try catch started");
                    if (e.path.StartsWith(@"-\"))
                    {
                        Logger.TalkyLog("Combine path started");
                        realPath = Path.Combine(Environment.CurrentDirectory, e.path.Replace(@"-\", ""));
                    }
                    else
                    {
                        Logger.TalkyLog("Didn't need to combine path");
                        realPath = e.path;
                    }
                    Logger.TalkyLog("Setting path");
                    Preprocessor.FilePath = realPath;
                    Console.WriteLine("Path set to " + realPath);
                    if (System.IO.File.Exists(realPath))
                    {
                        Logger.TalkyLog("Readfile started");
                        Program.Path = realPath;
                        Preprocessor.ReadFile(realPath);
                    }
                    else
                    {
                        Exceptions.Exception(2);
                    }
                }
                catch
                {
                    Preprocessor.LoadFileFailed(null, new HandledEventArgs());
                }
            }
        }
        public static void ReadFile(object sender, ReadFileArgs e)
        {
            if (!e.Handled)
            {
                Logger.TalkyLog("Reading input file and checking contents....");
                if (!e.path.Contains(".halcyon"))
                {
                    Exceptions.Exception(5);
                }
                else
                {
                    Logger.TalkyLog(e.path);
                    try
                    {
                        Logger.TalkyLog("Assigning Input File to a variable...");
                        Preprocessor.InputFile = System.IO.File.ReadAllLines(e.path);

                        Preprocessor.Preproccess(Preprocessor.InputFile);
                    }
                    catch (Exception ex)
                    {
                        Logger.TalkyLog("Well, there is an error");
                        if (ex.GetType() != typeof(System.ArgumentException))
                        {
                            Logger.Log(ex.ToString());
                            Logger.Log(ex.Message);
                            Logger.Log(ex.Source);
                            Logger.Log(ex.InnerException);
                            Logger.Log(ex.StackTrace);
                        }
                        else
                        {
                            Exceptions.Exception(22);
                            Logger.LogNoTrace(ex.ToString());
                            Logger.LogNoTrace(ex.Message);
                            Logger.LogNoTrace(ex.Source);
                            Logger.LogNoTrace(ex.InnerException);
                            Logger.LogNoTrace(ex.StackTrace);
                        }
                        Exceptions.Exception(6);
                    }
                }
            }
        }
        #endregion
    }
}
