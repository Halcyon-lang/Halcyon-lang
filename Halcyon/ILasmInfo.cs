using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Halcyon.Errors;

namespace Halcyon
{
    /// <summary>
    /// Contains information used by ILasm
    /// Allows to programatically select commandline options from an enum
    /// </summary>
    public static class ILasmInfo
    {
        /// <summary>
        /// Whether was ILasmInfo initialized or not
        /// </summary>
        public static bool Initialized = false;
        /// <summary>
        /// If options are "built"
        /// </summary>
        public static bool Built = false;
        /// <summary>
        /// Options in unbuilt form
        /// </summary>
        public static Dictionary<Halcyon.ILasm, string> Options = new Dictionary<ILasm,string>();
        /// <summary>
        /// Options before filename
        /// </summary>
        public static List<string> PreOptions = new List<string>();
        /// <summary>
        /// Options after filename
        /// </summary>
        public static List<string> PostOptions = new List<string>();
        /// <summary>
        /// Filename
        /// </summary>
        public static string name;
        /// <summary>
        /// Fired during load of ILasmInfo
        /// </summary>
        public static event EventHandler OnLoad = delegate { };
        /// <summary>
        /// Fired when Options are being built
        /// </summary>
        public static event EventHandler OnBuild = delegate { };
        private static string _CommandLine;
        /// <summary>
        /// Char used by wrapped executable for equations between option name and value
        /// </summary>
        public static char EquationChar = '=';
        /// <summary>
        /// Whether ignore type of options or not
        /// </summary>
        public static bool BenevolentOptions = false;
        /// <summary>
        /// Built options
        /// </summary>
        public static string CommandLine
        {
            get { return _CommandLine; }
            private set { _CommandLine = value; }
        }
        /// <summary>
        /// Used for "registering" options with their strings
        /// </summary>
        /// <param name="option"></param>
        /// <param name="optionText"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Used for "registering" options with their strings
        /// </summary>
        /// <param name="option"></param>
        /// <param name="optionText"></param>
        /// <returns></returns>
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
            if (Enum.IsDefined(typeof(ILasmExtra), option) && String.IsNullOrEmpty(extraInfo))
            {
                Exceptions.Exception(14);
                return;
            }

            if (Enum.IsDefined(typeof(ILasmExtra), option))
            {
                int HackyInt = 0;
                if (!BenevolentOptions)
                {
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
                }
                switch (pos)
                {
                    case Position.PRESTART:
                        PreOptions.Insert(0, Options[option] + EquationChar + extraInfo);
                        break;
                    case Position.PRE:
                        PreOptions.Add(Options[option] + EquationChar + extraInfo);
                        break;
                    case Position.POSTSTART:
                        PostOptions.Insert(0, Options[option] + EquationChar + extraInfo);
                        break;
                    case Position.POST:
                        PostOptions.Add(Options[option] + EquationChar + extraInfo);
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
        /// <summary>
        /// Builds option string
        /// </summary>
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
