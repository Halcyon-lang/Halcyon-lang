using Halcyon;
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

                switch (unprocessedValue)
                {
                    case "true":
                    case "false":
                        //ConvertUtils.
                        break;
                }
            }
        }

        public void Save()
        {
            using (StreamWriter sw = new StreamWriter(File.Open(Path.Combine(Environment.CurrentDirectory, @"\Halcyon.cfg"), FileMode.Create)))
            {
                sw.Write(Json.Serialize(this));
            }
        }
    }
}
