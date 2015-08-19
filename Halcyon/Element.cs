using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    public abstract class Element
    {
        public abstract string delimiter { get; }
        public abstract bool isBlock { get; }
        public abstract bool nameHasParentheses { get; }
        public abstract bool canHaveKeywords { get; }
        public abstract bool canHaveModifier { get; }
        public abstract string[] allowedModifiers { get; }
        public abstract bool endsWithsemicolon { get; }
        public abstract string[] allowedKeywords { get; }
        public abstract Content Content();
    }
}
