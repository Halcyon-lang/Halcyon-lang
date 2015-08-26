using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon.Utils
{
    public static class BooleanUtils
    {
        public static bool ParseBool(string source)
        {
            if (source.ToLower().Trim() == "false")
            {
                return false;
            }
            else if (source.ToLower().Trim() == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
