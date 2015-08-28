using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon.Utils
{
    /// <summary>
    /// For array utils that don't require an exact type.
    /// </summary>
    public static class ArrayExtensions
    {
        public static string JoinToString(this Array arr, string delimiter)
        {
            int i = 0;
            StringBuilder temp = new StringBuilder();
            foreach (object obj in arr)
            {
                i++;
                if (obj.GetType() == typeof(string))
                {
                    
                    temp.Append((string)obj);
                    if (i != arr.Length)
                    temp.Append(delimiter);
                }
            }
            return temp.ToString();
        }
    }
}
