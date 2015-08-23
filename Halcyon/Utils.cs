using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Globalization;
using Halcyon;
using System.Xml;
using System.Numerics;
using System.ComponentModel;
using System.Data.SqlTypes;

namespace Halcyon
{
    public delegate TResult MethodCall<T, TResult>(T target, params object[] args);
    public class Utils
    {   
        /// <summary>
        /// Prints help
        /// </summary>
        public static void printHelp()
        {
            Console.WriteLine("Halcyon compiler help:");
            Console.WriteLine("   -compile [File] - Compiles file to executable");
            Console.WriteLine("   -convert [File] - Converts file to IL. File will be written down here");
            Console.WriteLine("   -result [Text] - how does given line look in IL");
            Console.WriteLine("   -preprocess [File] - preprocesses .halcyon file");
            Console.WriteLine("   -talkative - turns more commandline info viewed mode ON/OFF");
            Console.WriteLine("   -info <classes|elements|version> - prints a certain piece of info or -info help");
            Console.WriteLine("   -exit exits the program safely");
        }
        /// <summary>
        /// Prints help for giveInfo when user does not provide -info with any arguments
        /// </summary>
        public static void printInfoHelp()
        {
            printf("-info\n");
            printf("   classes - Prints all classes\n");
            printf("   elements - Prints currently loaded elements\n");
            printf("   version - Prints current version of Halcyon\n");
            return;
        }
        //I am ashamed for this workaround of complete laziness, but I need it to start at least once
        /// <summary>
        /// Just like in C... I am so lazy
        /// </summary>
        /// <param name="str">Any string</param>
        public static void printf(string str)
        {
            Console.Write(str);
        }
        /// <summary>
        /// Provides user with useful info & stats about Halcyon
        /// </summary>
        /// <param name="arg">Any known command</param>
        public static void giveInfo(string arg)
        {
            switch (arg)
            {
                case "elements":
                    Console.WriteLine();
                    foreach (Type type in Assembly.GetCallingAssembly().GetExportedTypes())
                    {
                        if (type.IsSubclassOf(typeof(Element)))
                        {
                            Console.WriteLine(type.Name);
                        }
                    }
                    Console.WriteLine();
                    break;
                case "classes":
                    Console.WriteLine();
                    foreach (Type type in Assembly.GetCallingAssembly().GetExportedTypes())
                    {
                        Console.WriteLine(type.Name);
                    }
                    Console.WriteLine();
                    break;
                case "version":
                    Console.WriteLine();
                    Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Version);
                    Console.WriteLine();
                    break;
                case "help":
                    Utils.printInfoHelp();
                    break;
                default:
                    Errors.Exceptions.Exception(0);
                    break;
            }
        }
        /// <summary>
        /// Switches talkative mode on and off.
        /// </summary>
        public static void switchTalkative()
        {
            if (Program.Talkative)
            {
                Console.WriteLine("Talkative mode disabled. \n");
                Program.Talkative = false;
            }
            else
            {
                Console.WriteLine("Talkative mode enabled. \n");
                Program.Talkative = true;
            }
        }
        /// <summary>
        /// Gets number of a line
        /// </summary>
        /// <param name="line">The line whose number you want</param>
        /// <param name="container">Either string or StringBuilder</param>
        /// <returns></returns>
        public static int[] getLineNumber(string line, object container)
        {
            int lineCount = 0;
            List<int> lst = new List<int>();
            if (container is StringBuilder)
            {
                StringBuilder sb = (StringBuilder)container;
                foreach (string ln in sb.ToString().Split('\n'))
                {
                    lineCount++;
                    if (ln == line)
                    {
                        lst.Add(lineCount);
                    }
                }
            }
            else if (container is string)
            {
                foreach (string ln in ((string)container).Split('\n'))
                {
                    lineCount++;
                    if (ln == line)
                    {
                        lst.Add(lineCount);
                    }
                }
            }
            else
            {
                lst.Add(-1);
            }
            return lst.ToArray();
        }
        /// <summary>
        /// Returns line based on the number given
        /// </summary>
        /// <param name="lineNumber">Number of the line you want to retrieve</param>
        /// <param name="container">Either StringBuilder or string</param>
        /// <returns></returns>
        public static string getLineByNumber(int lineNumber, object container)
        {
            string Line = "";
            int lineCount = 0;
            if (container is StringBuilder)
            {
                foreach (string line in ((StringBuilder)container).ToString().Split('\n'))
                {
                    lineCount++;
                    if (lineCount == lineNumber)
                    {
                        Line = line;
                    }
                }
            }
            else if (container is string)
            {
                foreach (string line in ((string)container).Split('\n'))
                {
                    lineCount++;
                    if (lineCount == lineNumber)
                    {
                        Line = line;
                    }
                }
            }
            else
            {
                Line = "NF";
            }
            return Line;
        }
        /// <summary>
        /// Removes whitespace at the START of the string, not anywhere else. Note: THe only whitespace I know is, in fact, space.
        /// </summary>
        /// <param name="item">string to be ridden of extra whitespace at start</param>
        /// <returns></returns>
        public static string removeWhiteSpace(ref string item)
        {
            bool found = false;
            foreach (char ch in item)
            {
                if (!found)
                {
                    if (ch == ' ')
                    {
                        item = item.Substring(1);
                    }
                    else
                    {
                        found = true;
                    }
                }
            }
            return item;
        }
    }
    public static class StringUtils
    {
        public const string CarriageReturnLineFeed = "\r\n";
        public const string Empty = "";
        public const char CarriageReturn = '\r';
        public const char LineFeed = '\n';
        public const char Tab = '\t';
        public static string FormatWith(this string format, IFormatProvider provider, object arg0)
        {
            return format.FormatWith(provider, new object[]
			{
				arg0
			});
        }
        public static string FormatWith(this string format, IFormatProvider provider, object arg0, object arg1)
        {
            return format.FormatWith(provider, new object[]
			{
				arg0,
				arg1
			});
        }
        public static string FormatWith(this string format, IFormatProvider provider, object arg0, object arg1, object arg2)
        {
            return format.FormatWith(provider, new object[]
			{
				arg0,
				arg1,
				arg2
			});
        }
        public static string FormatWith(this string format, IFormatProvider provider, object arg0, object arg1, object arg2, object arg3)
        {
            return format.FormatWith(provider, new object[]
			{
				arg0,
				arg1,
				arg2,
				arg3
			});
        }
        private static string FormatWith(this string format, IFormatProvider provider, params object[] args)
        {
            ValidationUtils.ArgumentNotNull(format, "format");
            return string.Format(provider, format, args);
        }
        public static bool IsWhiteSpace(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (s.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < s.Length; i++)
            {
                if (!char.IsWhiteSpace(s[i]))
                {
                    return false;
                }
            }
            return true;
        }
        public static string NullEmptyString(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                return s;
            }
            return null;
        }
        public static StringWriter CreateStringWriter(int capacity)
        {
            StringBuilder sb = new StringBuilder(capacity);
            return new StringWriter(sb, CultureInfo.InvariantCulture);
        }
        public static int? GetLength(string value)
        {
            if (value == null)
            {
                return null;
            }
            return new int?(value.Length);
        }
        public static void ToCharAsUnicode(char c, char[] buffer)
        {
            buffer[0] = '\\';
            buffer[1] = 'u';
            buffer[2] = MathUtils.IntToHex((int)(c >> 12 & '\u000f'));
            buffer[3] = MathUtils.IntToHex((int)(c >> 8 & '\u000f'));
            buffer[4] = MathUtils.IntToHex((int)(c >> 4 & '\u000f'));
            buffer[5] = MathUtils.IntToHex((int)(c & '\u000f'));
        }
        public static TSource ForgivingCaseSensitiveFind<TSource>(this IEnumerable<TSource> source, Func<TSource, string> valueSelector, string testValue)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (valueSelector == null)
            {
                throw new ArgumentNullException("valueSelector");
            }
            IEnumerable<TSource> source2 =
                from s in source
                where string.Equals(valueSelector(s), testValue, StringComparison.OrdinalIgnoreCase)
                select s;
            if (source2.Count<TSource>() <= 1)
            {
                return source2.SingleOrDefault<TSource>();
            }
            IEnumerable<TSource> source3 =
                from s in source
                where string.Equals(valueSelector(s), testValue, StringComparison.Ordinal)
                select s;
            return source3.SingleOrDefault<TSource>();
        }
        public static string ToCamelCase(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }
            if (!char.IsUpper(s[0]))
            {
                return s;
            }
            char[] array = s.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                bool flag = i + 1 < array.Length;
                if (i > 0 && flag && !char.IsUpper(array[i + 1]))
                {
                    break;
                }
                array[i] = char.ToLower(array[i], CultureInfo.InvariantCulture);
            }
            return new string(array);
        }
        public static bool IsHighSurrogate(char c)
        {
            return char.IsHighSurrogate(c);
        }
        public static bool IsLowSurrogate(char c)
        {
            return char.IsLowSurrogate(c);
        }
        public static bool StartsWith(this string source, char value)
        {
            return source.Length > 0 && source[0] == value;
        }
        public static bool EndsWith(this string source, char value)
        {
            return source.Length > 0 && source[source.Length - 1] == value;
        }
    }
    public static class MathUtils
    {
        public static int IntLength(ulong i)
        {
            if (i < 10000000000uL)
            {
                if (i < 10uL)
                {
                    return 1;
                }
                if (i < 100uL)
                {
                    return 2;
                }
                if (i < 1000uL)
                {
                    return 3;
                }
                if (i < 10000uL)
                {
                    return 4;
                }
                if (i < 100000uL)
                {
                    return 5;
                }
                if (i < 1000000uL)
                {
                    return 6;
                }
                if (i < 10000000uL)
                {
                    return 7;
                }
                if (i < 100000000uL)
                {
                    return 8;
                }
                if (i < 1000000000uL)
                {
                    return 9;
                }
                return 10;
            }
            else
            {
                if (i < 100000000000uL)
                {
                    return 11;
                }
                if (i < 1000000000000uL)
                {
                    return 12;
                }
                if (i < 10000000000000uL)
                {
                    return 13;
                }
                if (i < 100000000000000uL)
                {
                    return 14;
                }
                if (i < 1000000000000000uL)
                {
                    return 15;
                }
                if (i < 10000000000000000uL)
                {
                    return 16;
                }
                if (i < 100000000000000000uL)
                {
                    return 17;
                }
                if (i < 1000000000000000000uL)
                {
                    return 18;
                }
                if (i < 10000000000000000000uL)
                {
                    return 19;
                }
                return 20;
            }
        }
        public static char IntToHex(int n)
        {
            if (n <= 9)
            {
                return (char)(n + 48);
            }
            return (char)(n - 10 + 97);
        }
        public static int? Min(int? val1, int? val2)
        {
            if (!val1.HasValue)
            {
                return val2;
            }
            if (!val2.HasValue)
            {
                return val1;
            }
            return new int?(Math.Min(val1.Value, val2.Value));
        }
        public static int? Max(int? val1, int? val2)
        {
            if (!val1.HasValue)
            {
                return val2;
            }
            if (!val2.HasValue)
            {
                return val1;
            }
            return new int?(Math.Max(val1.Value, val2.Value));
        }
        public static double? Max(double? val1, double? val2)
        {
            if (!val1.HasValue)
            {
                return val2;
            }
            if (!val2.HasValue)
            {
                return val1;
            }
            return new double?(Math.Max(val1.Value, val2.Value));
        }
        public static bool ApproxEquals(double d1, double d2)
        {
            if (d1 == d2)
            {
                return true;
            }
            double num = (Math.Abs(d1) + Math.Abs(d2) + 10.0) * 2.2204460492503131E-16;
            double num2 = d1 - d2;
            return -num < num2 && num > num2;
        }
    }
    public static class ValidationUtils
    {
        public static void ArgumentNotNullOrEmpty(string value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            if (value.Length == 0)
            {
                throw new ArgumentException("'{0}' cannot be empty.".FormatWith(CultureInfo.InvariantCulture, parameterName), parameterName);
            }
        }
        public static void ArgumentTypeIsEnum(Type enumType, string parameterName)
        {
            ValidationUtils.ArgumentNotNull(enumType, "enumType");
            if (!enumType.IsEnum == true)
            {
                throw new ArgumentException("Type {0} is not an Enum.".FormatWith(CultureInfo.InvariantCulture, enumType), parameterName);
            }
        }
        public static void ArgumentNotNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
    public static class DateTimeUtils
    {
        public const int DaysPer100Years = 36524;
        public const int DaysPer400Years = 146097;
        public const int DaysPer4Years = 1461;
        public const int DaysPerYear = 365;
        public const long TicksPerDay = 864000000000L;
        public static readonly long InitialJavaScriptDateTicks;
        public static readonly int[] DaysToMonth365;
        public static readonly int[] DaysToMonth366;
        static DateTimeUtils()
        {
            DateTimeUtils.InitialJavaScriptDateTicks = 621355968000000000L;
            DateTimeUtils.DaysToMonth365 = new int[]
			{
				0,
				31,
				59,
				90,
				120,
				151,
				181,
				212,
				243,
				273,
				304,
				334,
				365
			};
            DateTimeUtils.DaysToMonth366 = new int[]
			{
				0,
				31,
				60,
				91,
				121,
				152,
				182,
				213,
				244,
				274,
				305,
				335,
				366
			};
        }
        public static TimeSpan GetUtcOffset(this DateTime d)
        {
            return TimeZoneInfo.Local.GetUtcOffset(d);
        }
        public static XmlDateTimeSerializationMode ToSerializationMode(DateTimeKind kind)
        {
            switch (kind)
            {
                case DateTimeKind.Unspecified:
                    return XmlDateTimeSerializationMode.Unspecified;
                case DateTimeKind.Utc:
                    return XmlDateTimeSerializationMode.Utc;
                case DateTimeKind.Local:
                    return XmlDateTimeSerializationMode.Local;
                default:
                    throw MiscellaneousUtils.CreateArgumentOutOfRangeException("kind", kind, "Unexpected DateTimeKind value.");
            }
        }
        public static DateTime EnsureDateTime(DateTime value, DateTimeZoneHandling timeZone)
        {
            switch (timeZone)
            {
                case DateTimeZoneHandling.Local:
                    value = DateTimeUtils.SwitchToLocalTime(value);
                    break;
                case DateTimeZoneHandling.Utc:
                    value = DateTimeUtils.SwitchToUtcTime(value);
                    break;
                case DateTimeZoneHandling.Unspecified:
                    value = new DateTime(value.Ticks, DateTimeKind.Unspecified);
                    break;
                case DateTimeZoneHandling.RoundtripKind:
                    break;
                default:
                    throw new ArgumentException("Invalid date time handling value.");
            }
            return value;
        }
        public static DateTime SwitchToLocalTime(DateTime value)
        {
            switch (value.Kind)
            {
                case DateTimeKind.Unspecified:
                    return new DateTime(value.Ticks, DateTimeKind.Local);
                case DateTimeKind.Utc:
                    return value.ToLocalTime();
                case DateTimeKind.Local:
                    return value;
                default:
                    return value;
            }
        }
        public static DateTime SwitchToUtcTime(DateTime value)
        {
            switch (value.Kind)
            {
                case DateTimeKind.Unspecified:
                    return new DateTime(value.Ticks, DateTimeKind.Utc);
                case DateTimeKind.Utc:
                    return value;
                case DateTimeKind.Local:
                    return value.ToUniversalTime();
                default:
                    return value;
            }
        }
        public static long ToUniversalTicks(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                return dateTime.Ticks;
            }
            return DateTimeUtils.ToUniversalTicks(dateTime, dateTime.GetUtcOffset());
        }
        public static long ToUniversalTicks(DateTime dateTime, TimeSpan offset)
        {
            if (dateTime.Kind == DateTimeKind.Utc || dateTime == DateTime.MaxValue || dateTime == DateTime.MinValue)
            {
                return dateTime.Ticks;
            }
            long num = dateTime.Ticks - offset.Ticks;
            if (num > 3155378975999999999L)
            {
                return 3155378975999999999L;
            }
            if (num < 0L)
            {
                return 0L;
            }
            return num;
        }
        public static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, TimeSpan offset)
        {
            long universialTicks = DateTimeUtils.ToUniversalTicks(dateTime, offset);
            return DateTimeUtils.UniversialTicksToJavaScriptTicks(universialTicks);
        }
        public static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime)
        {
            return DateTimeUtils.ConvertDateTimeToJavaScriptTicks(dateTime, true);
        }
        public static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, bool convertToUtc)
        {
            long universialTicks = convertToUtc ? DateTimeUtils.ToUniversalTicks(dateTime) : dateTime.Ticks;
            return DateTimeUtils.UniversialTicksToJavaScriptTicks(universialTicks);
        }
        public static long UniversialTicksToJavaScriptTicks(long universialTicks)
        {
            return (universialTicks - DateTimeUtils.InitialJavaScriptDateTicks) / 10000L;
        }
        public static DateTime ConvertJavaScriptTicksToDateTime(long javaScriptTicks)
        {
            DateTime result = new DateTime(javaScriptTicks * 10000L + DateTimeUtils.InitialJavaScriptDateTicks, DateTimeKind.Utc);
            return result;
        }
        public static bool TryParseDateIso(string text, DateParseHandling dateParseHandling, DateTimeZoneHandling dateTimeZoneHandling, out object dt)
        {
            DateTimeParser dateTimeParser = default(DateTimeParser);
            if (!dateTimeParser.Parse(text))
            {
                dt = null;
                return false;
            }
            DateTime dateTime = new DateTime(dateTimeParser.Year, dateTimeParser.Month, dateTimeParser.Day, dateTimeParser.Hour, dateTimeParser.Minute, dateTimeParser.Second);
            dateTime = dateTime.AddTicks((long)dateTimeParser.Fraction);
            if (dateParseHandling != DateParseHandling.DateTimeOffset)
            {
                switch (dateTimeParser.Zone)
                {
                    case ParserTimeZone.Utc:
                        dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Utc);
                        break;
                    case ParserTimeZone.LocalWestOfUtc:
                        {
                            TimeSpan timeSpan = new TimeSpan(dateTimeParser.ZoneHour, dateTimeParser.ZoneMinute, 0);
                            long num = dateTime.Ticks + timeSpan.Ticks;
                            if (num <= DateTime.MaxValue.Ticks)
                            {
                                dateTime = new DateTime(num, DateTimeKind.Utc).ToLocalTime();
                            }
                            else
                            {
                                num += dateTime.GetUtcOffset().Ticks;
                                if (num > DateTime.MaxValue.Ticks)
                                {
                                    num = DateTime.MaxValue.Ticks;
                                }
                                dateTime = new DateTime(num, DateTimeKind.Local);
                            }
                            break;
                        }
                    case ParserTimeZone.LocalEastOfUtc:
                        {
                            TimeSpan timeSpan2 = new TimeSpan(dateTimeParser.ZoneHour, dateTimeParser.ZoneMinute, 0);
                            long num = dateTime.Ticks - timeSpan2.Ticks;
                            if (num >= DateTime.MinValue.Ticks)
                            {
                                dateTime = new DateTime(num, DateTimeKind.Utc).ToLocalTime();
                            }
                            else
                            {
                                num += dateTime.GetUtcOffset().Ticks;
                                if (num < DateTime.MinValue.Ticks)
                                {
                                    num = DateTime.MinValue.Ticks;
                                }
                                dateTime = new DateTime(num, DateTimeKind.Local);
                            }
                            break;
                        }
                }
                dt = DateTimeUtils.EnsureDateTime(dateTime, dateTimeZoneHandling);
                return true;
            }
            TimeSpan utcOffset;
            switch (dateTimeParser.Zone)
            {
                case ParserTimeZone.Utc:
                    utcOffset = new TimeSpan(0L);
                    break;
                case ParserTimeZone.LocalWestOfUtc:
                    utcOffset = new TimeSpan(-dateTimeParser.ZoneHour, -dateTimeParser.ZoneMinute, 0);
                    break;
                case ParserTimeZone.LocalEastOfUtc:
                    utcOffset = new TimeSpan(dateTimeParser.ZoneHour, dateTimeParser.ZoneMinute, 0);
                    break;
                default:
                    utcOffset = TimeZoneInfo.Local.GetUtcOffset(dateTime);
                    break;
            }
            long num2 = dateTime.Ticks - utcOffset.Ticks;
            if (num2 < 0L || num2 > 3155378975999999999L)
            {
                dt = null;
                return false;
            }
            dt = new DateTimeOffset(dateTime, utcOffset);
            return true;
        }
        public static bool TryParseDateTime(string s, DateParseHandling dateParseHandling, DateTimeZoneHandling dateTimeZoneHandling, string dateFormatString, CultureInfo culture, out object dt)
        {
            if (s.Length > 0)
            {
                if (s[0] == '/')
                {
                    if (s.StartsWith("/Date(", StringComparison.Ordinal) && s.EndsWith(")/", StringComparison.Ordinal) && DateTimeUtils.TryParseDateMicrosoft(s, dateParseHandling, dateTimeZoneHandling, out dt))
                    {
                        return true;
                    }
                }
                else if (s.Length >= 19 && s.Length <= 40 && char.IsDigit(s[0]) && s[10] == 'T' && DateTimeUtils.TryParseDateIso(s, dateParseHandling, dateTimeZoneHandling, out dt))
                {
                    return true;
                }
                if (!string.IsNullOrEmpty(dateFormatString) && DateTimeUtils.TryParseDateExact(s, dateParseHandling, dateTimeZoneHandling, dateFormatString, culture, out dt))
                {
                    return true;
                }
            }
            dt = null;
            return false;
        }
        public static bool TryParseDateMicrosoft(string text, DateParseHandling dateParseHandling, DateTimeZoneHandling dateTimeZoneHandling, out object dt)
        {
            string text2 = text.Substring(6, text.Length - 8);
            DateTimeKind dateTimeKind = DateTimeKind.Utc;
            int num = text2.IndexOf('+', 1);
            if (num == -1)
            {
                num = text2.IndexOf('-', 1);
            }
            TimeSpan timeSpan = TimeSpan.Zero;
            if (num != -1)
            {
                dateTimeKind = DateTimeKind.Local;
                timeSpan = DateTimeUtils.ReadOffset(text2.Substring(num));
                text2 = text2.Substring(0, num);
            }
            long javaScriptTicks;
            if (!long.TryParse(text2, NumberStyles.Integer, CultureInfo.InvariantCulture, out javaScriptTicks))
            {
                dt = null;
                return false;
            }
            DateTime dateTime = DateTimeUtils.ConvertJavaScriptTicksToDateTime(javaScriptTicks);
            if (dateParseHandling == DateParseHandling.DateTimeOffset)
            {
                dt = new DateTimeOffset(dateTime.Add(timeSpan).Ticks, timeSpan);
                return true;
            }
            DateTime value;
            switch (dateTimeKind)
            {
                case DateTimeKind.Unspecified:
                    value = DateTime.SpecifyKind(dateTime.ToLocalTime(), DateTimeKind.Unspecified);
                    goto IL_C6;
                case DateTimeKind.Local:
                    value = dateTime.ToLocalTime();
                    goto IL_C6;
            }
            value = dateTime;
        IL_C6:
            dt = DateTimeUtils.EnsureDateTime(value, dateTimeZoneHandling);
            return true;
        }
        public static bool TryParseDateExact(string text, DateParseHandling dateParseHandling, DateTimeZoneHandling dateTimeZoneHandling, string dateFormatString, CultureInfo culture, out object dt)
        {
            DateTime dateTime;
            if (dateParseHandling == DateParseHandling.DateTimeOffset)
            {
                DateTimeOffset dateTimeOffset;
                if (DateTimeOffset.TryParseExact(text, dateFormatString, culture, DateTimeStyles.RoundtripKind, out dateTimeOffset))
                {
                    dt = dateTimeOffset;
                    return true;
                }
            }
            else if (DateTime.TryParseExact(text, dateFormatString, culture, DateTimeStyles.RoundtripKind, out dateTime))
            {
                dateTime = DateTimeUtils.EnsureDateTime(dateTime, dateTimeZoneHandling);
                dt = dateTime;
                return true;
            }
            dt = null;
            return false;
        }
        public static TimeSpan ReadOffset(string offsetText)
        {
            bool flag = offsetText[0] == '-';
            int num = int.Parse(offsetText.Substring(1, 2), NumberStyles.Integer, CultureInfo.InvariantCulture);
            int num2 = 0;
            if (offsetText.Length >= 5)
            {
                num2 = int.Parse(offsetText.Substring(3, 2), NumberStyles.Integer, CultureInfo.InvariantCulture);
            }
            TimeSpan result = TimeSpan.FromHours((double)num) + TimeSpan.FromMinutes((double)num2);
            if (flag)
            {
                result = result.Negate();
            }
            return result;
        }
        public static void WriteDateTimeString(TextWriter writer, DateTime value, DateFormatHandling format, string formatString, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(formatString))
            {
                char[] array = new char[64];
                int count = DateTimeUtils.WriteDateTimeString(array, 0, value, null, value.Kind, format);
                writer.Write(array, 0, count);
                return;
            }
            writer.Write(value.ToString(formatString, culture));
        }
        public static int WriteDateTimeString(char[] chars, int start, DateTime value, TimeSpan? offset, DateTimeKind kind, DateFormatHandling format)
        {
            int num2;
            if (format == DateFormatHandling.MicrosoftDateFormat)
            {
                TimeSpan offset2 = offset ?? value.GetUtcOffset();
                long num = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(value, offset2);
                "\\/Date(".CopyTo(0, chars, start, 7);
                num2 = start + 7;
                string text = num.ToString(CultureInfo.InvariantCulture);
                text.CopyTo(0, chars, num2, text.Length);
                num2 += text.Length;
                switch (kind)
                {
                    case DateTimeKind.Unspecified:
                        if (value != DateTime.MaxValue && value != DateTime.MinValue)
                        {
                            num2 = DateTimeUtils.WriteDateTimeOffset(chars, num2, offset2, format);
                        }
                        break;
                    case DateTimeKind.Local:
                        num2 = DateTimeUtils.WriteDateTimeOffset(chars, num2, offset2, format);
                        break;
                }
                ")\\/".CopyTo(0, chars, num2, 3);
                num2 += 3;
            }
            else
            {
                num2 = DateTimeUtils.WriteDefaultIsoDate(chars, start, value);
                switch (kind)
                {
                    case DateTimeKind.Utc:
                        chars[num2++] = 'Z';
                        break;
                    case DateTimeKind.Local:
                        num2 = DateTimeUtils.WriteDateTimeOffset(chars, num2, offset ?? value.GetUtcOffset(), format);
                        break;
                }
            }
            return num2;
        }
        public static int WriteDefaultIsoDate(char[] chars, int start, DateTime dt)
        {
            int num = 19;
            int value;
            int value2;
            int value3;
            DateTimeUtils.GetDateValues(dt, out value, out value2, out value3);
            DateTimeUtils.CopyIntToCharArray(chars, start, value, 4);
            chars[start + 4] = '-';
            DateTimeUtils.CopyIntToCharArray(chars, start + 5, value2, 2);
            chars[start + 7] = '-';
            DateTimeUtils.CopyIntToCharArray(chars, start + 8, value3, 2);
            chars[start + 10] = 'T';
            DateTimeUtils.CopyIntToCharArray(chars, start + 11, dt.Hour, 2);
            chars[start + 13] = ':';
            DateTimeUtils.CopyIntToCharArray(chars, start + 14, dt.Minute, 2);
            chars[start + 16] = ':';
            DateTimeUtils.CopyIntToCharArray(chars, start + 17, dt.Second, 2);
            int num2 = (int)(dt.Ticks % 10000000L);
            if (num2 != 0)
            {
                int num3 = 7;
                while (num2 % 10 == 0)
                {
                    num3--;
                    num2 /= 10;
                }
                chars[start + 19] = '.';
                DateTimeUtils.CopyIntToCharArray(chars, start + 20, num2, num3);
                num += num3 + 1;
            }
            return start + num;
        }
        public static void CopyIntToCharArray(char[] chars, int start, int value, int digits)
        {
            while (digits-- != 0)
            {
                chars[start + digits] = (char)(value % 10 + 48);
                value /= 10;
            }
        }
        public static int WriteDateTimeOffset(char[] chars, int start, TimeSpan offset, DateFormatHandling format)
        {
            chars[start++] = ((offset.Ticks >= 0L) ? '+' : '-');
            int value = Math.Abs(offset.Hours);
            DateTimeUtils.CopyIntToCharArray(chars, start, value, 2);
            start += 2;
            if (format == DateFormatHandling.IsoDateFormat)
            {
                chars[start++] = ':';
            }
            int value2 = Math.Abs(offset.Minutes);
            DateTimeUtils.CopyIntToCharArray(chars, start, value2, 2);
            start += 2;
            return start;
        }
        public static void WriteDateTimeOffsetString(TextWriter writer, DateTimeOffset value, DateFormatHandling format, string formatString, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(formatString))
            {
                char[] array = new char[64];
                int count = DateTimeUtils.WriteDateTimeString(array, 0, (format == DateFormatHandling.IsoDateFormat) ? value.DateTime : value.UtcDateTime, new TimeSpan?(value.Offset), DateTimeKind.Local, format);
                writer.Write(array, 0, count);
                return;
            }
            writer.Write(value.ToString(formatString, culture));
        }
        public static void GetDateValues(DateTime td, out int year, out int month, out int day)
        {
            long ticks = td.Ticks;
            int i = (int)(ticks / 864000000000L);
            int num = i / 146097;
            i -= num * 146097;
            int num2 = i / 36524;
            if (num2 == 4)
            {
                num2 = 3;
            }
            i -= num2 * 36524;
            int num3 = i / 1461;
            i -= num3 * 1461;
            int num4 = i / 365;
            if (num4 == 4)
            {
                num4 = 3;
            }
            year = num * 400 + num2 * 100 + num3 * 4 + num4 + 1;
            i -= num4 * 365;
            int[] array = (num4 == 3 && (num3 != 24 || num2 == 3)) ? DateTimeUtils.DaysToMonth366 : DateTimeUtils.DaysToMonth365;
            int num5 = i >> 6;
            while (i >= array[num5])
            {
                num5++;
            }
            month = num5;
            day = i - array[num5 - 1] + 1;
        }
    }
    public static class MiscellaneousUtils
    {
        public static bool ValueEquals(object objA, object objB)
        {
            if (objA == null && objB == null)
            {
                return true;
            }
            if (objA != null && objB == null)
            {
                return false;
            }
            if (objA == null && objB != null)
            {
                return false;
            }
            if (!(objA.GetType() != objB.GetType()))
            {
                return objA.Equals(objB);
            }
            if (ConvertUtils.IsInteger(objA) && ConvertUtils.IsInteger(objB))
            {
                return Convert.ToDecimal(objA, CultureInfo.CurrentCulture).Equals(Convert.ToDecimal(objB, CultureInfo.CurrentCulture));
            }
            return (objA is double || objA is float || objA is decimal) && (objB is double || objB is float || objB is decimal) && MathUtils.ApproxEquals(Convert.ToDouble(objA, CultureInfo.CurrentCulture), Convert.ToDouble(objB, CultureInfo.CurrentCulture));
        }
        public static ArgumentOutOfRangeException CreateArgumentOutOfRangeException(string paramName, object actualValue, string message)
        {
            string message2 = message + Environment.NewLine + "Actual value was {0}.".FormatWith(CultureInfo.InvariantCulture, actualValue);
            return new ArgumentOutOfRangeException(paramName, message2);
        }
        public static string ToString(object value)
        {
            if (value == null)
            {
                return "{null}";
            }
            if (!(value is string))
            {
                return value.ToString();
            }
            return "\"" + value.ToString() + "\"";
        }
        public static int ByteArrayCompare(byte[] a1, byte[] a2)
        {
            int num = a1.Length.CompareTo(a2.Length);
            if (num != 0)
            {
                return num;
            }
            for (int i = 0; i < a1.Length; i++)
            {
                int num2 = a1[i].CompareTo(a2[i]);
                if (num2 != 0)
                {
                    return num2;
                }
            }
            return 0;
        }
        public static string GetPrefix(string qualifiedName)
        {
            string result;
            string text;
            MiscellaneousUtils.GetQualifiedNameParts(qualifiedName, out result, out text);
            return result;
        }
        public static string GetLocalName(string qualifiedName)
        {
            string text;
            string result;
            MiscellaneousUtils.GetQualifiedNameParts(qualifiedName, out text, out result);
            return result;
        }
        public static void GetQualifiedNameParts(string qualifiedName, out string prefix, out string localName)
        {
            int num = qualifiedName.IndexOf(':');
            if (num == -1 || num == 0 || qualifiedName.Length - 1 == num)
            {
                prefix = null;
                localName = qualifiedName;
                return;
            }
            prefix = qualifiedName.Substring(0, num);
            localName = qualifiedName.Substring(num + 1);
        }
        internal static string FormatValueForPrint(object value)
        {
            if (value == null)
            {
                return "{null}";
            }
            if (value is string)
            {
                return "\"" + value + "\"";
            }
            return value.ToString();
        }
    }
    public static class ConvertUtils
    {
        internal struct TypeConvertKey : IEquatable<ConvertUtils.TypeConvertKey>
        {
            private readonly Type _initialType;
            private readonly Type _targetType;
            public Type InitialType
            {
                get
                {
                    return this._initialType;
                }
            }
            public Type TargetType
            {
                get
                {
                    return this._targetType;
                }
            }
            public TypeConvertKey(Type initialType, Type targetType)
            {
                this._initialType = initialType;
                this._targetType = targetType;
            }
            public override int GetHashCode()
            {
                return this._initialType.GetHashCode() ^ this._targetType.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                return obj is ConvertUtils.TypeConvertKey && this.Equals((ConvertUtils.TypeConvertKey)obj);
            }
            public bool Equals(ConvertUtils.TypeConvertKey other)
            {
                return this._initialType == other._initialType && this._targetType == other._targetType;
            }
        }
        #region TypeCodeMap & PrimitiveTypeCode & CastConverters
        private static readonly Dictionary<Type, PrimitiveTypeCode> TypeCodeMap = new Dictionary<Type, PrimitiveTypeCode>
		{
			
			{
				typeof(char),
				PrimitiveTypeCode.Char
			},
			
			{
				typeof(char?),
				PrimitiveTypeCode.CharNullable
			},
			
			{
				typeof(bool),
				PrimitiveTypeCode.Boolean
			},
			
			{
				typeof(bool?),
				PrimitiveTypeCode.BooleanNullable
			},
			
			{
				typeof(sbyte),
				PrimitiveTypeCode.SByte
			},
			
			{
				typeof(sbyte?),
				PrimitiveTypeCode.SByteNullable
			},
			
			{
				typeof(short),
				PrimitiveTypeCode.Int16
			},
			
			{
				typeof(short?),
				PrimitiveTypeCode.Int16Nullable
			},
			
			{
				typeof(ushort),
				PrimitiveTypeCode.UInt16
			},
			
			{
				typeof(ushort?),
				PrimitiveTypeCode.UInt16Nullable
			},
			
			{
				typeof(int),
				PrimitiveTypeCode.Int32
			},
			
			{
				typeof(int?),
				PrimitiveTypeCode.Int32Nullable
			},
			
			{
				typeof(byte),
				PrimitiveTypeCode.Byte
			},
			
			{
				typeof(byte?),
				PrimitiveTypeCode.ByteNullable
			},
			
			{
				typeof(uint),
				PrimitiveTypeCode.UInt32
			},
			
			{
				typeof(uint?),
				PrimitiveTypeCode.UInt32Nullable
			},
			
			{
				typeof(long),
				PrimitiveTypeCode.Int64
			},
			
			{
				typeof(long?),
				PrimitiveTypeCode.Int64Nullable
			},
			
			{
				typeof(ulong),
				PrimitiveTypeCode.UInt64
			},
			
			{
				typeof(ulong?),
				PrimitiveTypeCode.UInt64Nullable
			},
			
			{
				typeof(float),
				PrimitiveTypeCode.Single
			},
			
			{
				typeof(float?),
				PrimitiveTypeCode.SingleNullable
			},
			
			{
				typeof(double),
				PrimitiveTypeCode.Double
			},
			
			{
				typeof(double?),
				PrimitiveTypeCode.DoubleNullable
			},
			
			{
				typeof(DateTime),
				PrimitiveTypeCode.DateTime
			},
			
			{
				typeof(DateTime?),
				PrimitiveTypeCode.DateTimeNullable
			},
			
			{
				typeof(DateTimeOffset),
				PrimitiveTypeCode.DateTimeOffset
			},
			
			{
				typeof(DateTimeOffset?),
				PrimitiveTypeCode.DateTimeOffsetNullable
			},
			
			{
				typeof(decimal),
				PrimitiveTypeCode.Decimal
			},
			
			{
				typeof(decimal?),
				PrimitiveTypeCode.DecimalNullable
			},
			
			{
				typeof(Guid),
				PrimitiveTypeCode.Guid
			},
			
			{
				typeof(Guid?),
				PrimitiveTypeCode.GuidNullable
			},
			
			{
				typeof(TimeSpan),
				PrimitiveTypeCode.TimeSpan
			},
			
			{
				typeof(TimeSpan?),
				PrimitiveTypeCode.TimeSpanNullable
			},
			
			{
				typeof(BigInteger),
				PrimitiveTypeCode.BigInteger
			},
			
			{
				typeof(BigInteger?),
				PrimitiveTypeCode.BigIntegerNullable
			},
			
			{
				typeof(Uri),
				PrimitiveTypeCode.Uri
			},
			
			{
				typeof(string),
				PrimitiveTypeCode.String
			},
			
			{
				typeof(byte[]),
				PrimitiveTypeCode.Bytes
			},
			
			{
				typeof(DBNull),
				PrimitiveTypeCode.DBNull
			}
		};
        private static readonly ThreadSafeStore<ConvertUtils.TypeConvertKey, Func<object, object>> CastConverters = new ThreadSafeStore<ConvertUtils.TypeConvertKey, Func<object, object>>(new Func<ConvertUtils.TypeConvertKey, Func<object, object>>(ConvertUtils.CreateCastConverter));
        private static readonly TypeInformation[] PrimitiveTypeCodes = new TypeInformation[]
		{
			new TypeInformation
			{
				Type = typeof(object),
				TypeCode = PrimitiveTypeCode.Empty
			},
			new TypeInformation
			{
				Type = typeof(object),
				TypeCode = PrimitiveTypeCode.Object
			},
			new TypeInformation
			{
				Type = typeof(object),
				TypeCode = PrimitiveTypeCode.DBNull
			},
			new TypeInformation
			{
				Type = typeof(bool),
				TypeCode = PrimitiveTypeCode.Boolean
			},
			new TypeInformation
			{
				Type = typeof(char),
				TypeCode = PrimitiveTypeCode.Char
			},
			new TypeInformation
			{
				Type = typeof(sbyte),
				TypeCode = PrimitiveTypeCode.SByte
			},
			new TypeInformation
			{
				Type = typeof(byte),
				TypeCode = PrimitiveTypeCode.Byte
			},
			new TypeInformation
			{
				Type = typeof(short),
				TypeCode = PrimitiveTypeCode.Int16
			},
			new TypeInformation
			{
				Type = typeof(ushort),
				TypeCode = PrimitiveTypeCode.UInt16
			},
			new TypeInformation
			{
				Type = typeof(int),
				TypeCode = PrimitiveTypeCode.Int32
			},
			new TypeInformation
			{
				Type = typeof(uint),
				TypeCode = PrimitiveTypeCode.UInt32
			},
			new TypeInformation
			{
				Type = typeof(long),
				TypeCode = PrimitiveTypeCode.Int64
			},
			new TypeInformation
			{
				Type = typeof(ulong),
				TypeCode = PrimitiveTypeCode.UInt64
			},
			new TypeInformation
			{
				Type = typeof(float),
				TypeCode = PrimitiveTypeCode.Single
			},
			new TypeInformation
			{
				Type = typeof(double),
				TypeCode = PrimitiveTypeCode.Double
			},
			new TypeInformation
			{
				Type = typeof(decimal),
				TypeCode = PrimitiveTypeCode.Decimal
			},
			new TypeInformation
			{
				Type = typeof(DateTime),
				TypeCode = PrimitiveTypeCode.DateTime
			},
			new TypeInformation
			{
				Type = typeof(object),
				TypeCode = PrimitiveTypeCode.Empty
			},
			new TypeInformation
			{
				Type = typeof(string),
				TypeCode = PrimitiveTypeCode.String
			}
		};
        #endregion
        public static PrimitiveTypeCode GetTypeCode(Type t)
        {
            bool flag;
            return ConvertUtils.GetTypeCode(t, out flag);
        }
        public static PrimitiveTypeCode GetTypeCode(Type t, out bool isEnum)
        {
            PrimitiveTypeCode result;
            if (ConvertUtils.TypeCodeMap.TryGetValue(t, out result))
            {
                isEnum = false;
                return result;
            }
            if (t.IsEnum == true)
            {
                isEnum = true;
                return ConvertUtils.GetTypeCode(Enum.GetUnderlyingType(t));
            }
            if (ReflectionUtils.IsNullableType(t))
            {
                Type underlyingType = Nullable.GetUnderlyingType(t);
                if (underlyingType.IsEnum)
                {
                    Type t2 = typeof(Nullable<>).MakeGenericType(new Type[]
					{
						Enum.GetUnderlyingType(underlyingType)
					});
                    isEnum = true;
                    return ConvertUtils.GetTypeCode(t2);
                }
            }
            isEnum = false;
            return PrimitiveTypeCode.Object;
        }
        public static TypeInformation GetTypeInformation(IConvertible convertable)
        {
            return ConvertUtils.PrimitiveTypeCodes[(int)convertable.GetTypeCode()];
        }
        public static bool IsConvertible(Type t)
        {
            return typeof(IConvertible).IsAssignableFrom(t);
        }
        public static TimeSpan ParseTimeSpan(string input)
        {
            return TimeSpan.Parse(input, CultureInfo.InvariantCulture);
        }        
        internal static BigInteger ToBigInteger(object value)
        {
            if (value is BigInteger)
            {
                return (BigInteger)value;
            }
            if (value is string)
            {
                return BigInteger.Parse((string)value, CultureInfo.InvariantCulture);
            }
            if (value is float)
            {
                return new BigInteger((float)value);
            }
            if (value is double)
            {
                return new BigInteger((double)value);
            }
            if (value is decimal)
            {
                return new BigInteger((decimal)value);
            }
            if (value is int)
            {
                return new BigInteger((int)value);
            }
            if (value is long)
            {
                return new BigInteger((long)value);
            }
            if (value is uint)
            {
                return new BigInteger((uint)value);
            }
            if (value is ulong)
            {
                return new BigInteger((ulong)value);
            }
            if (value is byte[])
            {
                return new BigInteger((byte[])value);
            }
            throw new InvalidCastException("Cannot convert {0} to BigInteger.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
        }
        public static object FromBigInteger(BigInteger i, Type targetType)
        {
            if (targetType == typeof(decimal))
            {
                return (decimal)i;
            }
            if (targetType == typeof(double))
            {
                return (double)i;
            }
            if (targetType == typeof(float))
            {
                return (float)i;
            }
            if (targetType == typeof(ulong))
            {
                return (ulong)i;
            }
            object result;
            try
            {
                result = System.Convert.ChangeType((long)i, targetType, CultureInfo.InvariantCulture);
            }
            catch (Exception innerException)
            {
                throw new InvalidOperationException("Can not convert from BigInteger to {0}.".FormatWith(CultureInfo.InvariantCulture, targetType), innerException);
            }
            return result;
        }
        public static object Convert(object initialValue, CultureInfo culture, Type targetType)
        {
            object result;
            switch (ConvertUtils.TryConvertInternal(initialValue, culture, targetType, out result))
            {
                case ConvertResult.Success:
                    return result;
                case ConvertResult.CannotConvertNull:
                    throw new Exception("Can not convert null {0} into non-nullable {1}.".FormatWith(CultureInfo.InvariantCulture, initialValue.GetType(), targetType));
                case ConvertResult.NotInstantiableType:
                    throw new ArgumentException("Target type {0} is not a value type or a non-abstract class.".FormatWith(CultureInfo.InvariantCulture, targetType), "targetType");
                case ConvertResult.NoValidConversion:
                    throw new InvalidOperationException("Can not convert from {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, initialValue.GetType(), targetType));
                default:
                    throw new InvalidOperationException("Unexpected conversion result.");
            }
        }
        private static bool TryConvert(object initialValue, CultureInfo culture, Type targetType, out object value)
        {
            bool result;
            try
            {
                if (ConvertUtils.TryConvertInternal(initialValue, culture, targetType, out value) == ConvertResult.Success)
                {
                    result = true;
                }
                else
                {
                    value = null;
                    result = false;
                }
            }
            catch
            {
                value = null;
                result = false;
            }
            return result;
        }
        /// <summary>
        /// Not working.
        /// </summary>
        /// <param name="initialValue"></param>
        /// <param name="culture"></param>
        /// <param name="targetType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static ConvertResult TryConvertInternal(object initialValue, CultureInfo culture, Type targetType, out object value)
        {
            if (initialValue == null)
            {
                throw new ArgumentNullException("initialValue");
            }
            if (ReflectionUtils.IsNullableType(targetType))
            {
                targetType = Nullable.GetUnderlyingType(targetType);
            }
            Type type = initialValue.GetType();
            if (targetType == type)
            {
                value = initialValue;
                return ConvertResult.Success;
            }
            if (ConvertUtils.IsConvertible(initialValue.GetType()) && ConvertUtils.IsConvertible(targetType))
            {
                if (targetType.IsEnum)
                {
                    if (initialValue is string)
                    {
                        value = Enum.Parse(targetType, initialValue.ToString(), true);
                        return ConvertResult.Success;
                    }
                    if (ConvertUtils.IsInteger(initialValue))
                    {
                        value = Enum.ToObject(targetType, initialValue);
                        return ConvertResult.Success;
                    }
                }
                value = System.Convert.ChangeType(initialValue, targetType, culture);
                return ConvertResult.Success;
            }
            if (initialValue is DateTime && targetType == typeof(DateTimeOffset))
            {
                value = new DateTimeOffset((DateTime)initialValue);
                return ConvertResult.Success;
            }
            if (initialValue is byte[] && targetType == typeof(Guid))
            {
                value = new Guid((byte[])initialValue);
                return ConvertResult.Success;
            }
            if (initialValue is Guid && targetType == typeof(byte[]))
            {
                value = ((Guid)initialValue).ToByteArray();
                return ConvertResult.Success;
            }
            if (initialValue is string)
            {
                if (targetType == typeof(Guid))
                {
                    value = new Guid((string)initialValue);
                    return ConvertResult.Success;
                }
                if (targetType == typeof(Uri))
                {
                    value = new Uri((string)initialValue, UriKind.RelativeOrAbsolute);
                    return ConvertResult.Success;
                }
                if (targetType == typeof(TimeSpan))
                {
                    value = ConvertUtils.ParseTimeSpan((string)initialValue);
                    return ConvertResult.Success;
                }
                if (targetType == typeof(byte[]))
                {
                    value = System.Convert.FromBase64String((string)initialValue);
                    return ConvertResult.Success;
                }
                if (typeof(Type).IsAssignableFrom(targetType))
                {
                    value = Type.GetType((string)initialValue, true);
                    return ConvertResult.Success;
                }
            }
            if (targetType == typeof(BigInteger))
            {
                value = ConvertUtils.ToBigInteger(initialValue);
                return ConvertResult.Success;
            }
            if (initialValue is BigInteger)
            {
                value = ConvertUtils.FromBigInteger((BigInteger)initialValue, targetType);
                return ConvertResult.Success;
            }
            TypeConverter converter = ConvertUtils.GetConverter(type);
            if (converter != null && converter.CanConvertTo(targetType))
            {
                value = converter.ConvertTo(null, culture, initialValue, targetType);
                return ConvertResult.Success;
            }
            TypeConverter converter2 = ConvertUtils.GetConverter(targetType);
            if (converter2 != null && converter2.CanConvertFrom(type))
            {
                value = converter2.ConvertFrom(null, culture, initialValue);
                return ConvertResult.Success;
            }
            if (initialValue == DBNull.Value)
            {
                if (ReflectionUtils.IsNullable(targetType))
                {
                    value = ConvertUtils.EnsureTypeAssignable(null, type, targetType);
                    return ConvertResult.Success;
                }
                value = null;
                return ConvertResult.CannotConvertNull;
            }
            else
            {
                if (initialValue is INullable)
                {
                    value = ConvertUtils.EnsureTypeAssignable(ConvertUtils.ToValue((INullable)initialValue), type, targetType);
                    return ConvertResult.Success;
                }
                if (targetType.IsInterface || targetType.IsGenericTypeDefinition || targetType.IsAbstract)
                {
                    value = null;
                    return ConvertResult.NotInstantiableType;
                }
                value = null;
                return ConvertResult.NoValidConversion;
            }
        }
        public static object ConvertOrCast(object initialValue, CultureInfo culture, Type targetType)
        {
            if (targetType == typeof(object))
            {
                return initialValue;
            }
            if (initialValue == null && ReflectionUtils.IsNullable(targetType))
            {
                return null;
            }
            object result;
            if (ConvertUtils.TryConvert(initialValue, culture, targetType, out result))
            {
                return result;
            }
            return ConvertUtils.EnsureTypeAssignable(initialValue, ReflectionUtils.GetObjectType(initialValue), targetType);
        }
        private static object EnsureTypeAssignable(object value, Type initialType, Type targetType)
        {
            Type type = (value != null) ? value.GetType() : null;
            if (value != null)
            {
                if (targetType.IsAssignableFrom(type))
                {
                    return value;
                }
                Func<object, object> func = ConvertUtils.CastConverters.Get(new ConvertUtils.TypeConvertKey(type, targetType));
                if (func != null)
                {
                    return func(value);
                }
            }
            else if (ReflectionUtils.IsNullable(targetType))
            {
                return null;
            }
            throw new ArgumentException("Could not cast or convert from {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, (initialType != null) ? initialType.ToString() : "{null}", targetType));
        }
        public static object ToValue(INullable nullableValue)
        {
            if (nullableValue == null)
            {
                return null;
            }
            if (nullableValue is SqlInt32)
            {
                return ConvertUtils.ToValue((SqlInt32)nullableValue);
            }
            if (nullableValue is SqlInt64)
            {
                return ConvertUtils.ToValue((SqlInt64)nullableValue);
            }
            if (nullableValue is SqlBoolean)
            {
                return ConvertUtils.ToValue((SqlBoolean)nullableValue);
            }
            if (nullableValue is SqlString)
            {
                return ConvertUtils.ToValue((SqlString)nullableValue);
            }
            if (nullableValue is SqlDateTime)
            {
                return ConvertUtils.ToValue((SqlDateTime)nullableValue);
            }
            throw new ArgumentException("Unsupported INullable type: {0}".FormatWith(CultureInfo.InvariantCulture, nullableValue.GetType()));
        }
        /// <summary>
        /// Not working, don't use!
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        internal static TypeConverter GetConverter(Type t)
        {
            return null;
        }
        public static bool IsInteger(object value)
        {
            switch (ConvertUtils.GetTypeCode(value.GetType()))
            {
                case PrimitiveTypeCode.SByte:
                case PrimitiveTypeCode.Int16:
                case PrimitiveTypeCode.UInt16:
                case PrimitiveTypeCode.Int32:
                case PrimitiveTypeCode.Byte:
                case PrimitiveTypeCode.UInt32:
                case PrimitiveTypeCode.Int64:
                case PrimitiveTypeCode.UInt64:
                    return true;
            }
            return false;
        }
        public static ParseResult Int32TryParse(char[] chars, int start, int length, out int value)
        {
            value = 0;
            if (length == 0)
            {
                return ParseResult.Invalid;
            }
            bool flag = chars[start] == '-';
            if (flag)
            {
                if (length == 1)
                {
                    return ParseResult.Invalid;
                }
                start++;
                length--;
            }
            int num = start + length;
            for (int i = start; i < num; i++)
            {
                int num2 = (int)(chars[i] - '0');
                if (num2 < 0 || num2 > 9)
                {
                    return ParseResult.Invalid;
                }
                int num3 = 10 * value - num2;
                if (num3 > value)
                {
                    for (i++; i < num; i++)
                    {
                        num2 = (int)(chars[i] - '0');
                        if (num2 < 0 || num2 > 9)
                        {
                            return ParseResult.Invalid;
                        }
                    }
                    return ParseResult.Overflow;
                }
                value = num3;
            }
            if (!flag)
            {
                if (value == -2147483648)
                {
                    return ParseResult.Overflow;
                }
                value = -value;
            }
            return ParseResult.Success;
        }
        public static ParseResult Int64TryParse(char[] chars, int start, int length, out long value)
        {
            value = 0L;
            if (length == 0)
            {
                return ParseResult.Invalid;
            }
            bool flag = chars[start] == '-';
            if (flag)
            {
                if (length == 1)
                {
                    return ParseResult.Invalid;
                }
                start++;
                length--;
            }
            int num = start + length;
            for (int i = start; i < num; i++)
            {
                int num2 = (int)(chars[i] - '0');
                if (num2 < 0 || num2 > 9)
                {
                    return ParseResult.Invalid;
                }
                long num3 = 10L * value - (long)num2;
                if (num3 > value)
                {
                    for (i++; i < num; i++)
                    {
                        num2 = (int)(chars[i] - '0');
                        if (num2 < 0 || num2 > 9)
                        {
                            return ParseResult.Invalid;
                        }
                    }
                    return ParseResult.Overflow;
                }
                value = num3;
            }
            if (!flag)
            {
                if (value == -9223372036854775808L)
                {
                    return ParseResult.Overflow;
                }
                value = -value;
            }
            return ParseResult.Success;
        }
        public static bool TryConvertGuid(string s, out Guid g)
        {
            return Guid.TryParse(s, out g);
        }
        /// <summary>
        /// Not working, dont use!
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static Func<object, object> CreateCastConverter(ConvertUtils.TypeConvertKey t)
        {
            MethodInfo method = t.TargetType.GetMethod("op_Implicit", new Type[]
			{
				t.InitialType
			});
            if (method == null)
            {
                method = t.TargetType.GetMethod("op_Explicit", new Type[]
				{
					t.InitialType
				});
            }
            if (method == null)
            {
                return null;
            }
            return null;
        }
    }
}
