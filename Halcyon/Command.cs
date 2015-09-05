using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Halcyon
{
    public delegate void CommandCallback(string[] args);
    public class Command
    {
        public string Name;
        public string Description;
        public CommandCallback Callback;

        public Command(string name, string description, CommandCallback callback)
        {
            Name = name;
            Description = description;
            Callback = callback;
        }
    }
}
