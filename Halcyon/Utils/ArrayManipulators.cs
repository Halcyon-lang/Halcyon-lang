using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon.Utils
{
    /// <summary>
    /// For array utils that require a certain type.
    /// </summary>
    /// <typeparam name="ArrayType">Type of the Array</typeparam>
    public class ArrayManipulators<ArrayType>
    {
        public ArrayType[] RemoveEmptyEntries(ArrayType[] arr)
        {
            List<ArrayType> temp = new List<ArrayType>();
            foreach (ArrayType item in arr)
            {
                PrimitiveTypeCode type = ConvertUtils.GetTypeCode(GetType());
                switch (type)
                {
                    case PrimitiveTypeCode.Object:
                        if (item != null)
                        {
                            temp.Add(item);
                        }
                        break;
                    case PrimitiveTypeCode.String:
                        if (!String.IsNullOrEmpty((string)(object)item))
                        {
                            temp.Add(item);
                        }
                        break;
                    default:
                        if (item != null)
                        {
                            temp.Add(item);
                        }
                        break;
                }
            }
            return temp.ToArray();
        }
    }
}
