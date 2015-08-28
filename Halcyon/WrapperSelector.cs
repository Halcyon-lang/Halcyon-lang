using Halcyon.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    public static class WrapperSelector
    {
        public static void Select(string[] args)
        {
            ArrayManipulators<string> arrman = new ArrayManipulators<string>();
            string[] selectargs = arrman.RemoveEmptyEntries(args);
        }
    }
}
