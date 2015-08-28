using Halcyon.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    public static class ParserSwitch
    {
        public static string File = "";
        public static void Start(string file)
        {
            File = file;
            if (CodeUtils.RemoveEmptyLines(File).StartsWith("assembly", StringComparison.CurrentCultureIgnoreCase))
            {

            }
        }
    }
}
 