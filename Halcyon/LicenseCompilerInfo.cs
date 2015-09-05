using Halcyon.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    public class LicenseCompilerInfo
    {
        public static bool Initialized = false;
        public static bool Built = false;
        public static Dictionary<Halcyon.LicenseCompiler, string> Options = new Dictionary<LicenseCompiler, string>();
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
        public static LicenseCompilerAddResult Add(LicenseCompiler option, string optionText)
        {
            try
            {
                if (!Options.ContainsKey(option))
                {
                    Options.Add(option, optionText);
                    return LicenseCompilerAddResult.SUCCESS;
                }
                else
                {
                    return LicenseCompilerAddResult.EXISTS;
                }
            }
            catch
            {
                return LicenseCompilerAddResult.FAIL;
            }
        }
        public static LicenseCompilerAddResult Add(LicenseCompiler option, string optionText, bool overwriteExisting)
        {
            try
            {
                if (!Options.ContainsKey(option))
                {
                    Options.Add(option, optionText);
                    return LicenseCompilerAddResult.SUCCESS;
                }
                else
                {
                    if (overwriteExisting)
                    {
                        return LicenseCompilerAddResult.OVERWRITE;
                    }
                    return LicenseCompilerAddResult.EXISTS;
                }
            }
            catch
            {
                return LicenseCompilerAddResult.FAIL;
            }
        }
        public static void Initialize()
        {
            if (!Initialized)
            {
                Add(LicenseCompiler.NOLOGO, "/nologo");
                
            }
            OnLoad(null, EventArgs.Empty);
        }
        /// <summary>
        /// Laziest SelectOption
        /// </summary>
        /// <param name="option"> Option you want to add </param>
        public static void SelectOption(LicenseCompiler option)
        {
            SelectOption(option, null);
        }
        /// <summary>
        /// Moderately lazy SelectOption
        /// </summary>
        /// <param name="option"> Option you want to add </param>
        /// <param name="extraInfo"> Extra information needed to be provided too ILasmExtra Options </param>
        public static void SelectOption(LicenseCompiler option, string extraInfo)
        {
            SelectOption(option, extraInfo, Position.PRE);
        }
        /// <summary>
        /// Least lazy SelectOption
        /// Provides the very best way to add options you want to use for LicenseCompiler. If option needs to have extra info provided, put it into extraInfo. 
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
        public static void SelectOption(LicenseCompiler option, string extraInfo, Position pos)
        {
            if (Enum.IsDefined(typeof(LicenseCompilerExtra), option) && String.IsNullOrEmpty(extraInfo))
            {
                Exceptions.Exception(14);
                return;
            }

            if (Enum.IsDefined(typeof(LicenseCompilerExtra), option))
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
