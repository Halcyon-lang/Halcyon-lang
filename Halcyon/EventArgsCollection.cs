using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon.CustomEventArgs
{
    public class LineEventArgs : HandledEventArgs
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
    public class LoadFileArgs : HandledEventArgs
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
    public class ReadFileArgs : HandledEventArgs
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
    public class PreprocessEventArgs : HandledEventArgs
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

    public class SaveEventArgs : EventArgs
    {
        private StreamWriter sw;

        public SaveEventArgs(StreamWriter writer)
        {
            sw = writer;
        }

        public StreamWriter Writer
        {
            get { return sw; }
        }
    }
}
