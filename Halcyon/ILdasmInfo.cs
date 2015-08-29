using Halcyon.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    public class ILdasmInfo
    {
        public static bool Initialized = false;
        public static bool Built = false;
        public static Dictionary<Halcyon.ILdasm, string> Options = new Dictionary<ILdasm, string>();
        public static List<string> PreOptions = new List<string>();
        public static List<string> PostOptions = new List<string>();
        public static event EventHandler OnLoad = delegate { };
        public static event EventHandler OnBuild = delegate { };
        private static string _CommandLine;
        public static bool BenevolentOptions = false;
        public static char EquationChar = '=';
        public static string name;
        public static string CommandLine
        {
            get { return _CommandLine; }
            private set { _CommandLine = value; }
        }
        public static ILdasmAddResult Add(ILdasm option, string optionText)
        {
            try
            {
                if (!Options.ContainsKey(option))
                {
                    Options.Add(option, optionText);
                    return ILdasmAddResult.SUCCESS;
                }
                else
                {
                    return ILdasmAddResult.EXISTS;
                }
            }
            catch
            {
                return ILdasmAddResult.FAIL;
            }
        }
        public static ILdasmAddResult Add(ILdasm option, string optionText, bool overwriteExisting)
        {
            try
            {
                if (!Options.ContainsKey(option))
                {
                    Options.Add(option, optionText);
                    return ILdasmAddResult.SUCCESS;
                }
                else
                {
                    if (overwriteExisting)
                    {
                        return ILdasmAddResult.OVERWRITE;
                    }
                    return ILdasmAddResult.EXISTS;
                }
            }
            catch
            {
                return ILdasmAddResult.FAIL;
            }
        }
        public static void Initialize()
        {
            if (!Initialized)
            {
                Add(ILdasm.OUT, "/OUT");
            }
            OnLoad(null, EventArgs.Empty);
        }
        /// <summary>
        /// Laziest SelectOption
        /// </summary>
        /// <param name="option"> Option you want to add </param>
        public static void SelectOption(ILdasm option)
        {
            SelectOption(option, null);
        }
        /// <summary>
        /// Moderately lazy SelectOption
        /// </summary>
        /// <param name="option"> Option you want to add </param>
        /// <param name="extraInfo"> Extra information needed to be provided too ILasmExtra Options </param>
        public static void SelectOption(ILdasm option, string extraInfo)
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
        public static void SelectOption(ILdasm option, string extraInfo, Position pos)
        {
            if (Enum.IsDefined(typeof(ILdasmExtra), option) && String.IsNullOrEmpty(extraInfo))
            {
                Exceptions.Exception(14);
                return;
            }

            if (Enum.IsDefined(typeof(ILdasmExtra), option))
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
