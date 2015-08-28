using Halcyon.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    public class AssemblyLinkerInfo
    {
        public static bool Initialized = false;
        public static bool Built = false;
        public static Dictionary<Halcyon.AssemblyLinker, string> Options = new Dictionary<AssemblyLinker, string>();
        public static List<string> PreOptions = new List<string>();
        public static List<string> PostOptions = new List<string>();
        public static event EventHandler OnLoad = delegate { };
        public static event EventHandler OnBuild = delegate { };
        private static string _CommandLine;
        public static bool BenevolentOptions = false;
        public static char EquationChar = ':';
        public static string CommandLine
        {
            get { return _CommandLine; }
            private set { _CommandLine = value; }
        }
        public static AssemblyLinkerAddResult Add(AssemblyLinker option, string optionText)
        {
            try
            {
                if (!Options.ContainsKey(option))
                {
                    Options.Add(option, optionText);
                    return AssemblyLinkerAddResult.SUCCESS;
                }
                else
                {
                    return AssemblyLinkerAddResult.EXISTS;
                }
            }
            catch
            {
                return AssemblyLinkerAddResult.FAIL;
            }
        }
        public static AssemblyLinkerAddResult Add(AssemblyLinker option, string optionText, bool overwriteExisting)
        {
            try
            {
                if (!Options.ContainsKey(option))
                {
                    Options.Add(option, optionText);
                    return AssemblyLinkerAddResult.SUCCESS;
                }
                else
                {
                    if (overwriteExisting)
                    {
                        return AssemblyLinkerAddResult.OVERWRITE;
                    }
                    return AssemblyLinkerAddResult.EXISTS;
                }
            }
            catch
            {
                return AssemblyLinkerAddResult.FAIL;
            }
        }
        public static void Initialize()
        {
            if (!Initialized)
            {
                Add(AssemblyLinker.NOLOGO, "/nologo");
                Add(AssemblyLinker.HELP, "/?");
                Add(AssemblyLinker.ALGID, "/algid");
                Add(AssemblyLinker.BASE, "/base");
                Add(AssemblyLinker.BUGREPORT, "/bugreport");
                Add(AssemblyLinker.COMP, "/comp");
                Add(AssemblyLinker.CONFIG, "/config");
                Add(AssemblyLinker.COPY, "/copy");
                Add(AssemblyLinker.C, "/c");
                Add(AssemblyLinker.DELAY, "/delay");
                Add(AssemblyLinker.DESCR, "/descr");
                Add(AssemblyLinker.E, "/e");
                Add(AssemblyLinker.FILEVERSION, "/fileversion");
                Add(AssemblyLinker.FLAGS, "/flags");
                Add(AssemblyLinker.FULLPATHS, "/fullpaths");
                Add(AssemblyLinker.KEYF, "/keyf");
                Add(AssemblyLinker.KEYN, "/keyn");
                Add(AssemblyLinker.MAIN, "/main");
                Add(AssemblyLinker.OUT, "/out");
                Add(AssemblyLinker.PLATFORM, "/platform");
                Add(AssemblyLinker.PROD, "/prod");
                Add(AssemblyLinker.PRODUCTV, "/productv");
                Add(AssemblyLinker.SUBSYSTEMVERSION, "/subsystemversion");
                Add(AssemblyLinker.T, "/t");
                Add(AssemblyLinker.TEMPLATE, "/template");
                Add(AssemblyLinker.TITLE, "/title");
                Add(AssemblyLinker.TRADE, "/trade");
                Add(AssemblyLinker.V, "/v");
                Add(AssemblyLinker.WIN32ICON, "/win32icon");
                Add(AssemblyLinker.WIN32RES, "/win32res");
                Add(AssemblyLinker.LINK, "/linkresource");
                Add(AssemblyLinker.EMBED, "/embedresource");
            }
            OnLoad(null, EventArgs.Empty);
        }
        /// <summary>
        /// Laziest SelectOption
        /// </summary>
        /// <param name="option"> Option you want to add </param>
        public static void SelectOption(AssemblyLinker option)
        {
            SelectOption(option, null);
        }
        /// <summary>
        /// Moderately lazy SelectOption
        /// </summary>
        /// <param name="option"> Option you want to add </param>
        /// <param name="extraInfo"> Extra information needed to be provided too ILasmExtra Options </param>
        public static void SelectOption(AssemblyLinker option, string extraInfo)
        {
            SelectOption(option, extraInfo, Position.PRE);
        }
        /// <summary>
        /// Least lazy SelectOption
        /// Provides the very best way to add options you want to use for AssemblyLinker. If option needs to have extra info provided, put it into extraInfo. 
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
        public static void SelectOption(AssemblyLinker option, string extraInfo, Position pos)
        {
            if (Enum.IsDefined(typeof(AssemblyLinkerExtra), option) && String.IsNullOrEmpty(extraInfo))
            {
                Exceptions.Exception(14);
                return;
            }

            if (Enum.IsDefined(typeof(AssemblyLinkerExtra), option))
            {
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
        public static void BuildOptions()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string str in PreOptions)
            {
                sb.Append(str + " ");
            }
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
