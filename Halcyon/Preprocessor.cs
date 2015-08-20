using Halcyon.Errors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    public delegate void DirectiveCallback(string line);
    class Preprocessor
    {
        //Events
        public static event EventHandler OnLoadFile = delegate { };
        public static event EventHandler OnLoadFileFailed = delegate { };
        public static event EventHandler OnReadFile = delegate { };
        public static event EventHandler OnReadFileFailed = delegate { };
        public static event EventHandler OnStart = delegate { };
        public static event EventHandler OnReset = delegate { };
        public static event EventHandler OnPreprocessCompleted = delegate { };
        public static event EventHandler OnInitDirectives = delegate { };
        public static event EventHandler<LineEventArgs> OnNextLine = delegate { };
        //fields
        public static string[] InputFile;
        public static StringBuilder PreprocessedFile = new StringBuilder();
        public static string FilePath = "";
        public static List<Directive> DirectiveList = new List<Directive>();
        public static bool onlySaveAfterPreprocess = false;
        //Functions
        public static void LoadFile(string path) 
        {
            if(Program.Talkative) Console.WriteLine("Loadfile started");
            OnStart(null, EventArgs.Empty);
            string realPath;
            try
            {
                if(Program.Talkative) 
                Console.WriteLine("try catch started");
                if (path.StartsWith(@"-\"))
                {
                    if(Program.Talkative) 
                    Console.WriteLine("Combine path started");
                    realPath = Path.Combine(Environment.CurrentDirectory, path.Replace(@"-\",""));
                }
                else
                {
                    if(Program.Talkative) 
                    Console.WriteLine("Didn't need to combine path");
                    realPath = path;
                }
                if(Program.Talkative) 
                Console.WriteLine("Setting path");
                FilePath = realPath;
                Console.WriteLine("Path set to " + realPath);
                if (System.IO.File.Exists(realPath))
                {
                    OnLoadFile(null, EventArgs.Empty);
                    if(Program.Talkative) 
                    Console.WriteLine("Readfile started");
                    ReadFile(realPath);
                }
                else
                {
                    Exceptions.Exception(2);
                }
            }
            catch
            {
                OnLoadFileFailed(null, EventArgs.Empty);
            }
        }
        public static void ReadFile(string path)
        {
            if(Program.Talkative) 
            Console.WriteLine("If started");
            if (!path.Contains(".halcyon"))
            {
                Exceptions.Exception(5);
            }
            else
            {
                if (Program.Talkative)
                Console.WriteLine(path);
                try
                {
                    if(Program.Talkative) 
                    Console.WriteLine("Assigning Input File...");
                    InputFile = System.IO.File.ReadAllLines(path);
                    //OnReadFile(null, EventArgs.Empty);
                    Preproccess(InputFile);
                }
                catch (Exception e)
                {
                    if(Program.Talkative)
                    Console.WriteLine("Assigning apparently failed");
                    Exceptions.Exception(6);
                    OnReadFileFailed(null, EventArgs.Empty);
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.Source);
                    Console.WriteLine(e.InnerException);
                    Console.WriteLine(e.StackTrace);
                }
            }
        }

        public static void Preproccess(string[] file)
        {
            if(Program.Talkative) 
            Console.WriteLine("Preproccess started");
            foreach (string line in file)
            {
                if(Program.Talkative) 
                Console.WriteLine("Looping through lines.");
                OnNextLine(null, new LineEventArgs(line));
            }
            OnPreprocessCompleted(null, EventArgs.Empty);
            if(Program.Talkative) 
            Console.WriteLine("If started");
            if (onlySaveAfterPreprocess)
            {
                if(Program.Talkative) 
                Console.WriteLine("Writing file finished");
                System.IO.StreamWriter output = new System.IO.StreamWriter(Path.Combine(Environment.CurrentDirectory, Path.GetFileNameWithoutExtension(FilePath) + ".halp"));
                output.Write(PreprocessedFile + "\n");
                output.Close();
            }
            if(Program.Talkative) 
            Console.WriteLine("Firing OnReset");
            OnReset(null, EventArgs.Empty);
            Console.WriteLine("Task completed. \n");
        }

        public static void initDirectives()
        {
            Add("include", Callbacks.Include);
            Add("define", Callbacks.Define);
            OnInitDirectives(null, EventArgs.Empty);
        }

        public static void initCommonPreprocessorEvents()
        {
            OnPreprocessCompleted += PreprocessorEvents.Preprocessor_OnPreprocessCompleted;
            OnNextLine += PreprocessorEvents.Preprocessor_OnNextLine;
            OnReset += PreprocessorEvents.Preprocessor_OnReset;
        }

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
        internal static Dictionary<string, string> DefineList = new Dictionary<string, string>();
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
                if (Program.Talkative)
                    Console.WriteLine("Include " + temp);
                if (Program.Talkative)
                    Console.WriteLine("Environment.CurrentDirectory: " + Environment.CurrentDirectory);
                if (Program.Talkative)
                    Console.WriteLine("Default Include Path: " + defaultIncludePath);
                if (Program.Talkative)
                    Console.WriteLine("Include file path:" + Path.Combine(defaultIncludePath, temp.Trim()));
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

        internal static void Define(string line)
        {
            string temp = line.Replace("#define", "");
            string key = temp.Split(Convert.ToChar(" ")).First();
            string value = String.Join(" ", temp.Replace(key, "").Split(Convert.ToChar(" ")));
            DefineList.Add(key, value);
        }
    }

    public static class PreprocessorEvents 
    {
        public static void Preprocessor_OnPreprocessCompleted(object sender, EventArgs e)
        {
            //To be continued soon enough
        }

        public static void Preprocessor_OnNextLine(object sender, LineEventArgs e)
        {
            if (e.Line.StartsWith("#"))
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
        public static void Preprocessor_OnReset(object sender, EventArgs e)
        {
            Preprocessor.PreprocessedFile.Clear();
        }
    }

    public class LineEventArgs : EventArgs
    {
        private string ln;

        public LineEventArgs(string line)
        {
            ln = line;
        }
        public string Line
        {
            get { return ln; }
        }
    }
}
