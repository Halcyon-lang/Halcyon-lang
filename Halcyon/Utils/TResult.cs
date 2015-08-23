using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon.Utils
{
    public delegate TResult MethodCall<T, TResult>(T target, params object[] args);
}
