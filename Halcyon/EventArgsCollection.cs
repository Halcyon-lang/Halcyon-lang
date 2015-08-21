using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon.CustomEventArgs
{
    public class LineEventArgs : EventArgs
    {
        private string ln;

        public LineEventArgs(string line)
        {
            ln = line;
        }
        public string Line
        {
            get { return ln; }
        }
    }
    public class LoadFileArgs : EventArgs
    {
        private string pth;
        public LoadFileArgs(string Path)
        {
            pth = Path;
        }
        public string path
        {
            get { return pth; }
        }
    }
    public class ReadFileArgs : EventArgs
    {
        private string pth;
        public ReadFileArgs(string Path)
        {
            pth = Path;
        }
        public string path
        {
            get { return pth; }
        }
    }
    public class PreprocessEventArgs : EventArgs
    {
        private string[] fl;

        public PreprocessEventArgs(string[] File)
        {
            fl = File;
        }

        public string[] file
        {
            get { return fl; }
        }
    }
}
