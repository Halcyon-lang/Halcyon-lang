using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Reflection;

namespace Halcyon
{
    class Utils
    {
        [DllImport("Utils.dll")]
        public static extern void printHelp();
        [DllImport("Utils.dll")]
        public static extern void printInfoHelp();

        public static void giveInfo(string arg)
        {
            switch (arg)
            {
                case "elements":
                    Console.WriteLine();
                    foreach (Type type in Assembly.GetCallingAssembly().GetExportedTypes())
                    {
                        if (type.IsSubclassOf(typeof(Element)))
                        {
                            Console.WriteLine(type.Name);
                        }
                    }
                    Console.WriteLine();
                    break;
                case "classes":
                    Console.WriteLine();
                    foreach (Type type in Assembly.GetCallingAssembly().GetExportedTypes())
                    {
                        Console.WriteLine(type.Name);
                    }
                    Console.WriteLine();
                    break;
                case "version":
                    Console.WriteLine();
                    Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Version);
                    Console.WriteLine();
                    break;
                case "help":
                    Utils.printInfoHelp();
                    break;
                default:
                    Errors.Exceptions.Exception(0);
                    break;
            }
        }
    }
}
