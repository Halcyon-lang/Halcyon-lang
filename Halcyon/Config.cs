﻿using Halcyon;
using Halcyon.Errors;
using Halcyon.Logging;
using Halcyon.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    /// <summary>
    /// Halcyon's config. If you want to have own config, simply create a child class of this class and override what you need to override. 
    /// Alternatively you can add own values to this config by subscribing to OnSave and adding own keys. Then you can just do for example
    /// public static string MyProperty = valueStash.ContainsKey("MyProperty") ? (string)valueStash["MyProperty"] : "default value";
    /// 
    /// This config solution only supports reading/writing of these types so far:
    ///     short
    ///     int
    ///     long
    ///     string
    ///     bool
    /// You can cast whatever type you want though. I take no responsibility for random shit it can cause.
    /// </summary>
    public class Config
    {
        public static Dictionary<string, object> valueStash = new Dictionary<string,object>();
        /// <summary>
        /// Fired at the end of Load()
        /// </summary>
        public static event EventHandler OnLoad = delegate { };
        /// <summary>
        /// Fired at the end of Save()
        /// </summary>
        public static event EventHandler OnSave = delegate { };
        public static bool defaultTalkative { get { return (bool)GetValue("defaultTalkative", (object)false); } set { TryUpdateKey("defaultTalkative", (object)value); } }
        public static string logName { get { return (string)GetValue("logName", (object)"Halcyon.log"); } set { TryUpdateKey("logName", value); } }
        private static string SavePath { get { return Path.Combine(Environment.CurrentDirectory, @"\Halcyon.cfg"); } }
        private static bool Initialized = false;
       
        /// <summary>
        /// If key is found, updates its TKeyValuePair, if not adds a new key with given value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TryUpdateKeyResult TryUpdateKey(string key, object value)
        {
            try
            {
                if (valueStash.ContainsKey(key))
                {
                    if (valueStash[key] == value) return TryUpdateKeyResult.Unchanged;
                    else
                    {
                        valueStash.Remove(key);
                        valueStash.Add(key, value);
                        return TryUpdateKeyResult.Success;
                    }
                }
                else
                {
                    valueStash.Add(key, value);
                    return TryUpdateKeyResult.NewKey;
                }
            }
            catch(Exception ex)
            {
                Logger.LogNoTrace(ex.Message, ex.Source, ex.HelpLink, ex.StackTrace);
                return TryUpdateKeyResult.Failed;
            }
        }
        public static TryGetValueResult TryGetValue(string key)
        {
            try
            {
                if (valueStash.ContainsKey(key))
                {
                    return TryGetValueResult.Success;
                }
                else
                {
                    return TryGetValueResult.Default;
                }
            }
            catch(Exception ex)
            {
                Logger.LogNoTrace(ex.Message, ex.Source, ex.HelpLink, ex.StackTrace);
                return TryGetValueResult.Failed;
            }
        }
        /// <summary>
        /// Returns value if key is found in valueStash
        /// Returns default value if key is not found in value stash
        /// Returns null if something gone wrong.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="Default"></param>
        /// <returns></returns>
        public static object GetValue(string key, object Default)
        {
            switch (TryGetValue(key))
            {
                case TryGetValueResult.Default:
                    return Default;
                case TryGetValueResult.Success:
                    return valueStash[key];
                case TryGetValueResult.Failed:
                    Exceptions.Exception(13);
                    return null; 
            }
            return null;
        }
        /// <summary>
        /// Loads the config
        /// </summary>
        public static void Load()
        {
            StreamReader sr = new StreamReader(File.Open(Path.Combine(Environment.CurrentDirectory, @"\Halcyon.cfg"), FileMode.Open));
            string content = sr.ReadToEnd();
            foreach (string line in content.Split('\n'))
            {
                string key = line.Trim().Split('=').First();
                string unprocessedValue = line.Replace(key, "").Replace(" = ", "");
                object value = new object();
                int holder = new int();
                short shortholder = new short();
                long longholder = new long();
                switch (unprocessedValue)
                {
                    case "true":
                    case "false":
                        value = ConvertUtils.Convert(unprocessedValue, new System.Globalization.CultureInfo(1), typeof(bool));
                        break;
                    default:
                        if (ConvertUtils.Int16TryParse(unprocessedValue.ToCharArray(), 0, unprocessedValue.Count(), out shortholder) == ParseResult.Success)
                        {
                            value = (object)shortholder;
                        }
                        else if (ConvertUtils.Int32TryParse(unprocessedValue.ToCharArray(), 0, unprocessedValue.Count(), out holder) == ParseResult.Success)
                        {
                            value = (object)holder;
                        }
                        else if (ConvertUtils.Int64TryParse(unprocessedValue.ToCharArray(), 0, unprocessedValue.Count(), out longholder) == ParseResult.Success)
                        {
                            value = (object)longholder;
                        }
                        else if (unprocessedValue.StartsWith("\""))
                        {
                            int check = MathUtils.Random.Next();
                            unprocessedValue = unprocessedValue.Replace("\\\"", "STRING_PLACEHOLDER" + check.ToString());
                            unprocessedValue = unprocessedValue.Replace("\"", "");
                            unprocessedValue = unprocessedValue.Replace("STRING_PLACEHOLDER" + check.ToString(), "\\\"");
                            value = (object)unprocessedValue;
                        }
                        //This the least safe way
                        else
                        {
                            value = (object)unprocessedValue;
                        }
                        break;
                }
                if (!valueStash.ContainsKey(key))
                {
                    valueStash.Add(key, value);
                }
                else
                {
                    Exceptions.Exception(11);
                    Logger.TalkyLog("at " + line);
                }
            }
            OnLoad(null, EventArgs.Empty);
        }
        /// <summary>
        /// Saves this config.
        /// </summary>
        public static void Save()
        {
            using (StreamWriter sw = new StreamWriter(File.Open(Path.Combine(Environment.CurrentDirectory, @"\Halcyon.cfg"), FileMode.Create)))
            {
                sw.WriteLine("defaultTalkative = " + (string)GetValue("defaultTalkative", (object)"false"));
                sw.WriteLine("logName = " + (string)GetValue("logName", (object)"Halcyon.log"));
                OnSave(null, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Initializes the main config.
        /// </summary>
        public static void Initialize() 
        {
            if (!Initialized)
            {
                if (File.Exists(Path.Combine(Environment.CurrentDirectory, @"\Halcyon.cfg")))
                {
                    Load();
                }
                else
                {
                    Save();
                    Load();
                }
            }
        }
    }
}