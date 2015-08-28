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
        public static bool And(bool left, bool right)
        {
            if (right && left)
            {
                return true;
            }
            else { return false; } 
        }
        public static bool Or(bool left, bool right)
        {
            if (right || left)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool XOr(bool left, bool right)
        {
            if (left && !right)
            {
                return true;
            }
            else if (!left && right)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        public static bool Toggle(this bool me)
        {
            if (me)
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
