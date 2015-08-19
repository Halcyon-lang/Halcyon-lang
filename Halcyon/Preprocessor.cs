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
        public static string[] InputFile;
        public static string PreprocessedFile = "";
        public static string FilePath;
        public static List<Directive> DirectiveList = new List<Directive>();
        public static void LoadFile(string path) 
        {
            string realPath;
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
                ReadFile(realPath);
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
                    Preproccess();
                }
                catch
                {
                    Exceptions.Exception(6);
                }
            }
        }

        public static void Preproccess()
        {
            throw new NotImplementedException();
        }

        public static void initDirectives()
        {
            Add("include", Callbacks.Include);
            Add("define", Callbacks.Define);
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
                sb.Append(Preprocessor.PreprocessedFile);
                sb.Append(file.Replace("#Halcyon", ""));
            }
        }

        internal static void Define(string line)
        {
            string temp = line.Replace("#define", "");
            string key = temp.Split(Convert.ToChar(" ")).First();
            string value = String.Join(" ", temp.Replace(key, "").Split(Convert.ToChar(" ")));
            Preprocessor.PreprocessedFile = Preprocessor.PreprocessedFile.Replace(key, value); 
        }
    }
}
