using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon.Code
{
    public class Entry
    {
        public List<Element> Elements = new List<Element>();
        public Element Main;
        public string ID;
        public string Parent;
        public List<Entry> Children = new List<Entry>();
    }
}
