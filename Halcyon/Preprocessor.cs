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
        public static event EventHandler OnPreprocessCompleted = delegate { };
        public static event EventHandler OnInitDirectives = delegate { };
        public static event EventHandler<LineEventArgs> OnNextLine = delegate { };
        //fields
        public static string[] InputFile;
        public static StringBuilder PreprocessedFile = new StringBuilder();
        public static string FilePath;
        public static List<Directive> DirectiveList = new List<Directive>();
        public static bool onlySaveAfterPreprocess = false;
        //Functions
        public static void LoadFile(string path) 
        {
            OnStart(null, EventArgs.Empty);
            string realPath;
            try
            {
                if (path.StartsWith("-\\"))
                {
                    realPath = Path.Combine(Environment.CurrentDirectory, path.Substring(2));
                }
                else
                {
                    realPath = path;
                }
                FilePath = realPath;
                Console.WriteLine("Path set to " + realPath);
                if (System.IO.File.Exists(realPath))
                {
                    OnLoadFile(null, EventArgs.Empty);
                    ReadFile(realPath);
                }
            }
            catch
            {
                OnLoadFileFailed(null, EventArgs.Empty);
            }
        }
        public static void ReadFile(string path)
        {
            if (!path.Contains(".halcyon"))
            {
                Exceptions.Exception(5);
            }
            else
            {
                try
                {
                    InputFile = System.IO.File.ReadAllLines(path);
                    OnReadFile(null, EventArgs.Empty);
                    Preproccess(InputFile);
                }
                catch
                {
                    Exceptions.Exception(6);
                    OnReadFileFailed(null, EventArgs.Empty);
                }
            }
        }

        public static void Preproccess(string[] file)
        {
            foreach (string line in file)
            {
                OnNextLine(null, new LineEventArgs(line));
            }
            OnPreprocessCompleted(null, EventArgs.Empty);
            if (onlySaveAfterPreprocess)
            {
                System.IO.StreamWriter output = new System.IO.StreamWriter(Environment.CurrentDirectory + Path.GetFileNameWithoutExtension(FilePath) + ".halp");
                output.Write(PreprocessedFile + "\n");
                output.Close();
            }
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
            temp = temp.Replace("#include", "");
            if (temp.StartsWith("\""))
            {
                temp.Replace("\"", "");
                if (File.Exists(Path.Combine(Path.GetDirectoryName(Preprocessor.FilePath), temp)))
                    file = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Preprocessor.FilePath), temp));
                else Exceptions.Exception(7);
            }
            else if (temp.StartsWith("<"))
            {
                temp.Replace("<", "");
                temp.Replace(">", "");
                if (File.Exists(Path.Combine(Path.Combine(Environment.CurrentDirectory, "\\include"), temp)))
                    file = File.ReadAllText(Path.Combine(Path.Combine(Environment.CurrentDirectory, "\\include"), temp));
                else Exceptions.Exception(7);
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
                StringBuilder sb = new StringBuilder();
                sb.Append(Preprocessor.PreprocessedFile.ToString());
                sb.Append(file.Replace("#Halcyon", ""));
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
