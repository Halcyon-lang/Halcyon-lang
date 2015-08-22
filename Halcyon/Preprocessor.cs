﻿using Halcyon.CustomEventArgs;
using Halcyon.Errors;
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
    class Preprocessor
    {
        #region declarations
        //Events
        public static event EventHandler<LoadFileArgs> OnLoadFile = delegate { };
        public static event EventHandler<ReadFileArgs> OnReadFile = delegate { };
        public static event EventHandler<HandledEventArgs> OnStart = delegate { };
        public static event EventHandler<HandledEventArgs> OnReset = delegate { };
        public static event EventHandler<HandledEventArgs> OnPreprocessCompleted = delegate { };
        public static event EventHandler<HandledEventArgs> OnInitDirectives = delegate { };
        public static event EventHandler<LineEventArgs> OnNextLine = delegate { };
        public static event EventHandler<PreprocessEventArgs> OnPreprocess = delegate { };
        //fields
        public static string[] InputFile;
        public static StringBuilder PreprocessedFile = new StringBuilder();
        public static string FilePath = "";
        public static List<Directive> DirectiveList = new List<Directive>();
        public static bool onlySaveAfterPreprocess = false;
        public static bool firstrun = true;
        #endregion
        //Functions
        public static void LoadFile(string path) 
        {
            if (Program.Talkative) Console.WriteLine("Loadfile started");
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
            if (Program.Talkative)
                Console.WriteLine("Firing OnReset");
            OnReset(null, new HandledEventArgs());
            Console.WriteLine("Task completed. \n");
        }

        public static void initDirectives()
        {
            OnInitDirectives(null, new HandledEventArgs());
        }

        public static void initCommonPreprocessorEvents()
        {
            OnPreprocessCompleted += PreprocessorEvents.Preprocessor_DefineOnPreprocessCompleted;
            OnPreprocessCompleted += PreprocessorEvents.Preprocessor_OnPreprocessCompleted;
            PreprocessorEvents.OnNextLine += PreprocessorEvents.Preprocessor_OnNextLine;
            OnReset += PreprocessorEvents.Preprocessor_OnReset;
            OnInitDirectives += PreprocessorEvents.InitDirectives;
            OnPreprocess += PreprocessorEvents.Preprocess;
            OnLoadFile += PreprocessorEvents.LoadFile;
            OnReadFile += PreprocessorEvents.ReadFile;
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
            string temp = line.Replace("#define ", "");
            if (Program.Talkative)
            {
                foreach (string str in temp.Split(' '))
                {
                    Console.WriteLine(str);
                }
            }
            string key = temp.Split(Convert.ToChar(" ")).First();
            if (Program.Talkative) Console.WriteLine(temp.Replace(" " + key + " ", ""));
            string value = temp.Replace(key + " ", "");
            DefineList.Add(key, value);
        }
    }

    public static class PreprocessorEvents 
    {
        public static event EventHandler<LineEventArgs> OnNextLine;
        public static event EventHandler<HandledEventArgs> OnLoadFileFailed;
        public static event EventHandler<HandledEventArgs> OnReadFileFailed;
        public static void Preprocessor_DefineOnPreprocessCompleted(object sender, EventArgs e)
        {
            foreach (string key in Callbacks.DefineList.Keys)
            {
                if (Program.Talkative) Console.WriteLine("Key: " + key);
                if (Program.Talkative) Console.WriteLine("Replacing " + key + " with " + Callbacks.DefineList[key]);
                Preprocessor.PreprocessedFile = Preprocessor.PreprocessedFile.Replace(key, Callbacks.DefineList[key]);
            }
            string temp = Preprocessor.PreprocessedFile.ToString();
            if (Preprocessor.firstrun)
            {
                if (Program.Talkative)
                    Console.WriteLine("Going second run in preprocessor.");
                Preprocessor.PreprocessedFile.Clear();
                Preprocessor.Preproccess(temp.Split(Convert.ToChar("\n")));
            }
        }
        public static void Preprocessor_OnNextLine(object sender, LineEventArgs e)
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
        public static void Preprocessor_OnReset(object sender, EventArgs e)
        {
            Preprocessor.PreprocessedFile.Clear();
            Callbacks.DefineList.Clear();
        }
        public static void InitDirectives(object sender, EventArgs e)
        {
            Preprocessor.Add("include", Callbacks.Include);
            Preprocessor.Add("define", Callbacks.Define);
        }
        public static void Preprocess(object sender, PreprocessEventArgs e)
        {
            if (Program.Talkative)
                Console.WriteLine("Preproccess started");
            foreach (string line in e.file)
            {
                if (Program.Talkative)
                    Console.WriteLine("Looping through lines.");
                OnNextLine(null, new LineEventArgs(line));
            }
        }
        public static void Preprocessor_OnPreprocessCompleted(object sender, EventArgs e)
        {
            if (Program.Talkative)
                Console.WriteLine("If started");
            if (Preprocessor.onlySaveAfterPreprocess)
            {
                if (Program.Talkative)
                    Console.WriteLine("Writing file finished");
                System.IO.StreamWriter output = new System.IO.StreamWriter(Path.Combine(Environment.CurrentDirectory, Path.GetFileNameWithoutExtension(Preprocessor.FilePath) + ".halp"));
                output.Write(Preprocessor.PreprocessedFile + "\n");
                output.Close();
            }
        }
        public static void LoadFile(object sender, LoadFileArgs e)
        {
            string realPath;
            try
            {
                if (Program.Talkative)
                    Console.WriteLine("try catch started");
                if (e.path.StartsWith(@"-\"))
                {
                    if (Program.Talkative)
                        Console.WriteLine("Combine path started");
                    realPath = Path.Combine(Environment.CurrentDirectory, e.path.Replace(@"-\", ""));
                }
                else
                {
                    if (Program.Talkative)
                        Console.WriteLine("Didn't need to combine path");
                    realPath = e.path;
                }
                if (Program.Talkative)
                    Console.WriteLine("Setting path");
                Preprocessor.FilePath = realPath;
                Console.WriteLine("Path set to " + realPath);
                if (System.IO.File.Exists(realPath))
                {
                    if (Program.Talkative)
                        Console.WriteLine("Readfile started");
                    Preprocessor.ReadFile(realPath);
                }
                else
                {
                    Exceptions.Exception(2);
                }
            }
            catch
            {
                OnLoadFileFailed(null, new HandledEventArgs());
            }
        }
        public static void ReadFile(object sender, ReadFileArgs e)
        {
            if (Program.Talkative)
                Console.WriteLine("If started");
            if (!e.path.Contains(".halcyon"))
            {
                Exceptions.Exception(5);
            }
            else
            {
                if (Program.Talkative)
                    Console.WriteLine(e.path);
                try
                {
                    if (Program.Talkative)
                        Console.WriteLine("Assigning Input File...");
                    Preprocessor.InputFile = System.IO.File.ReadAllLines(e.path);

                    Preprocessor.Preproccess(Preprocessor.InputFile);
                }
                catch (Exception ex)
                {
                    if (Program.Talkative)
                        Console.WriteLine("Assigning apparently failed");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.Source);
                    Console.WriteLine(ex.InnerException);
                    Console.WriteLine(ex.StackTrace);
                    Exceptions.Exception(6);
                    OnReadFileFailed(null, new HandledEventArgs());
                }
            }
        }
    }
}
