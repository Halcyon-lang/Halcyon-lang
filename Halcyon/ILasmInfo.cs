﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Halcyon.Errors;

namespace Halcyon
{
    public static class ILasmInfo
    {
        public static bool Initialized = false;
        public static bool Built = false;
        public static Dictionary<Halcyon.ILasm, string> Options = new Dictionary<ILasm,string>();
        public static List<string> PreOptions = new List<string>();
        public static List<string> PostOptions = new List<string>();
        public static string name;
        public static event EventHandler OnLoad = delegate { };
        public static event EventHandler OnBuild = delegate { };
        private static string _CommandLine;
        public static string CommandLine
        {
            get { return _CommandLine; }
            private set { _CommandLine = value; }
        }
        public static ILasmAddResult Add(ILasm option, string optionText)
        {
            try
            {
                if (!Options.ContainsKey(option))
                {
                    Options.Add(option, optionText);
                    return ILasmAddResult.SUCCESS;
                }
                else
                {
                    return ILasmAddResult.EXISTS;
                }
            }
            catch
            {
                return ILasmAddResult.FAIL;
            }
        }
        public static ILasmAddResult Add(ILasm option, string optionText, bool overwriteExisting)
        {
            try
            {
                if (!Options.ContainsKey(option))
                {
                    Options.Add(option, optionText);
                    return ILasmAddResult.SUCCESS;
                }
                else
                {
                    if (overwriteExisting)
                    {
                        return ILasmAddResult.OVERWRITE;
                    }
                    return ILasmAddResult.EXISTS;
                }
            }
            catch
            {
                return ILasmAddResult.FAIL;
            }
        }
        public static void Initialize()
        {
            if (!Initialized)
            {
                Add(ILasm.NOLOGO, "/NOLOGO");
                Add(ILasm.QUIET, "/QUIET");
                Add(ILasm.NOAUTOINHERIT, "/NOAUTOINHERIT");
                Add(ILasm.DLL, "/DLL");
                Add(ILasm.EXE, "/EXE");
                Add(ILasm.PDB, "/PDB");
                Add(ILasm.APPCONTAINER, "/APPCONTAINER");
                Add(ILasm.DEBUG, "/DEBUG");
                Add(ILasm.DEBUGIMPL, "/DEBUG=IMPL");
                Add(ILasm.DEBUGOPT, "/DEBUG=OPT");
                Add(ILasm.OPTIMIZE, "/OPTIMIZE");
                Add(ILasm.FOLD, "/FOLD");
                Add(ILasm.CLOCK, "/CLOCK");
                Add(ILasm.RESOURCE, "/RESOURCE");
                Add(ILasm.OUTPUT, "/OUTPUT");
                Add(ILasm.KEY, "/KEY");
                Add(ILasm.INCLUDE, "/INCLUDE");
                Add(ILasm.SUBSYSTEM, "/SUBSYSTEM");
                Add(ILasm.SSVER, "/SSVER");
                Add(ILasm.FLAGS, "/FLAGS");
                Add(ILasm.ALIGNMENT, "/ALIGNMENT");
                Add(ILasm.BASE, "/BASE");
                Add(ILasm.STACK, "/STACK");
                Add(ILasm.MDV, "/MDV");
                Add(ILasm.MSV, "/MSV");
                Add(ILasm.PE64, "/PE64");
                Add(ILasm.HIGHENTROPYVA, "/HIGHENTROPYVA");
                Add(ILasm.NOCORSTUB, "/NOCORSTUB");
                Add(ILasm.STRIPRELOC, "/STRIPRELOC");
                Add(ILasm.ITANIUM, "/ITANIUM");
                Add(ILasm.X64, "/X64");
                Add(ILasm.ARM, "/ARM");
                Add(ILasm.PREFER32BIT, "/32BITPREFERRED");
                Add(ILasm.ENC, "/ENC");
            }
            OnLoad(null, EventArgs.Empty);
        }
        /// <summary>
        /// Laziest SelectOption
        /// </summary>
        /// <param name="option"> Option you want to add </param>
        public static void SelectOption(ILasm option) 
        {
            SelectOption(option, null);
        }
        /// <summary>
        /// Moderately lazy SelectOption
        /// </summary>
        /// <param name="option"> Option you want to add </param>
        /// <param name="extraInfo"> Extra information needed to be provided too ILasmExtra Options </param>
        public static void SelectOption(ILasm option, string extraInfo)
        {
            SelectOption(option, extraInfo, Position.PRE);
        }
        /// <summary>
        /// Least lazy SelectOption
        /// Provides the very best way to add options you want to use for ILasm. If option needs to have extra info provided, put it into extraInfo. 
        /// SelectOptions allows you to place options at four places:
        ///     Start of options before name of the compiled file (Position.PRESTART)
        ///     End of options before name of the compiled file (Position.PRE)
        ///     Start of options after name of the compiled file (Position.POSTSTART)
        ///     End of options after name of the compiled file (Position.POST)     
        /// NOTE: When adding options with extra info required, don't add the '='. SelectOption already adds it itself.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="extraInfo"></param>
        /// <param name="pos"></param>
        public static void SelectOption(ILasm option, string extraInfo, Position pos)
        {
            string temp = "";
            if (Enum.IsDefined(typeof(ILasmExtra), option) && String.IsNullOrEmpty(extraInfo))
            {
                Exceptions.Exception(14);
                return;
            }

            if (Enum.IsDefined(typeof(ILasmExtra), option))
            {
                int HackyInt = 0;
                switch (option)
                {
                    case ILasm.RESOURCE:
                        if (!extraInfo.EndsWith(".res") && !Config.benevolentOptions)
                        {
                            Exceptions.Exception(16);
                            Exceptions.Exception(19);
                            return;
                        }
                        break;
                    case ILasm.BASE:
                    case ILasm.STACK:
                    case ILasm.FLAGS:
                    case ILasm.SUBSYSTEM:
                    case ILasm.ALIGNMENT:
                        if (!Int32.TryParse(extraInfo, out HackyInt))
                        {
                            Exceptions.Exception(16);
                            Exceptions.Exception(20);
                        }
                        break;
                }
                switch (pos)
                {
                    case Position.PRESTART:
                        PreOptions.Insert(0, Options[option] + "=" + extraInfo);
                        break;
                    case Position.PRE:
                        PreOptions.Add(Options[option] + "=" + extraInfo);
                        break;
                    case Position.POSTSTART:
                        PostOptions.Insert(0, Options[option] + "=" + extraInfo);
                        break;
                    case Position.POST:
                        PostOptions.Add(Options[option] + "=" + extraInfo);
                        break;
                }
            }
            else
            {
                switch (pos)
                {
                    case Position.PRESTART:
                        PreOptions.Insert(0, Options[option]);
                        break;
                    case Position.PRE:
                        PreOptions.Add(Options[option]);
                        break;
                    case Position.POSTSTART:
                        PostOptions.Insert(0, Options[option]);
                        break;
                    case Position.POST:
                        PostOptions.Add(Options[option]);
                        break;
                }
            }
        }
        public static void BuildOptions()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string str in PreOptions)
            {
                sb.Append(str + " ");
            }
            sb.Append(name);
            foreach (string str2 in PostOptions) 
            {
                sb.Append(" " + str2);
            }
            CommandLine = sb.ToString();
            OnBuild(null, EventArgs.Empty);
            Built = true;
        }
    }
}
