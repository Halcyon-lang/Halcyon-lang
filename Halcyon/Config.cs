using Halcyon;
using Halcyon.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    public static class Config
    {
        private static Dictionary<string, object> valueStash = new Dictionary<string,object>();
        public static bool defaultTalkative = valueStash.ContainsKey("defaultTalkative") ? (bool)valueStash["defaultTalkative"] : false;
        public static string logName = valueStash.ContainsKey("logName") ? (string)valueStash["logName"] : "Halcyon.log";
        private static string SavePath { get { return Path.Combine(Environment.CurrentDirectory, @"\Halcyon.cfg"); } }
        public static void Load()
        {
            StreamReader sr = new StreamReader(File.Open(Path.Combine(Environment.CurrentDirectory, @"\Halcyon.cfg"), FileMode.Open));
            string content = sr.ReadToEnd();
            foreach (string line in content.Split('\n'))
            {
                string key = line.Trim().Split('=').First();
                string unprocessedValue = line.Trim().Split('=')[1].Substring(1);
                object value = new object();
                int holder = new int();
                short shortholder = new short();
                switch (unprocessedValue)
                {
                    case "true":
                    case "false":
                        value = ConvertUtils.Convert(unprocessedValue, new System.Globalization.CultureInfo(1), typeof(bool));
                        break;
                    default:
                        if (ConvertUtils.Int16TryParse(unprocessedValue.ToCharArray(), 0, unprocessedValue.Count(), out shortholder) == ParseResult.Success)
                        {

                        }
                        if(ConvertUtils.Int32TryParse(unprocessedValue.ToCharArray(), 0, unprocessedValue.Count(), out holder) == ParseResult.Success) 
                        {
                            value = (object)holder;
                        }
                        break;
                }
            }
        }

        public static void Save()
        {
            using (StreamWriter sw = new StreamWriter(File.Open(Path.Combine(Environment.CurrentDirectory, @"\Halcyon.cfg"), FileMode.Create)))
            {
            
            }
        }
    }
}
