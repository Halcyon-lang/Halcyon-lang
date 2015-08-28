using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon.Utils
{
    public static class CodeUtils
    {
        public static List<string> GetCodeBlocks(string source)
        {
            List<string> Blocks = new List<string>();
            string CurrentBlock = "";
            bool isInString = false;
            string Temp;
            int OpenedBrackets = 0;
            int ClosedBrackets = 0;
            int charIndex = 0;
            Temp = source;
            NEXTBLOCK:
            foreach (char ch in Temp) 
            {
                if (BooleanUtils.And(ch == '\"' || ch == '\'', source[charIndex - 1] != '\\'))
                {
                    if (isInString) isInString = false;
                    else isInString = true;
                }
                if (ch == '{' && !isInString)
                {
                    CurrentBlock += ch;
                    OpenedBrackets++;
                }
                else if (ch == '}' && !isInString)
                {
                    CurrentBlock += ch;
                    ClosedBrackets++;
                    if (BooleanUtils.And(OpenedBrackets != 0, ClosedBrackets != 0) && OpenedBrackets == ClosedBrackets)
                    {
                        break;
                    } 
                }
                else
                {
                    CurrentBlock += ch;
                }
                charIndex++;
            }
            Temp.Replace(CurrentBlock, "");
            Blocks.Add(CurrentBlock);
            CurrentBlock = "";
            if (!String.IsNullOrEmpty(Temp))
            {
                goto NEXTBLOCK;
            }
            return Blocks;
        }
        public static string RemoveEmptyLines(string source)
        {
            StringBuilder temp = new StringBuilder();
            foreach (string line in source.Split('\n'))
            {
                if (!String.IsNullOrWhiteSpace(line))
                {
                    temp.AppendLine(line);
                }
            }
            return temp.ToString();
        }
        public static bool IsStatement(string line, string nextline)
        {
            if (BooleanUtils.Or(line.Contains('{') || nextline.Trim().StartsWith('{'), line.Contains('}') || nextline.Trim().StartsWith('}')))
            {
                return false;
            }
            return true;
        }
        public static string ToBlob(byte[] blob)
        {
            StringBuilder temp = new StringBuilder();
            temp.Append("(");
            for (int i = 0; i < blob.Length; i++)
            {

                temp.Append(' ');
                temp.Append(blob[i].ToString("x2"));
            }
            temp.Append(" )");
            return temp.ToString();
        }
        public static string Escape(string identifier)
        {
            if (IsValidIdentifier(identifier) && ILInfo.ilKeywords.Contains(identifier))
            {
                return identifier;
            }
            return "'" + ConvertString(identifier).Replace("'", "\\'") + "'";
        }
        public static bool IsValidIdentifier(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                return false;
            }
            if (!char.IsLetter(identifier[0]) && !IsValidIdentifierCharacter(identifier[0]))
            {
                return identifier == ".ctor" || identifier == ".cctor";
            }
            for (int i = 1; i < identifier.Length; i++)
            {
                if (!char.IsLetterOrDigit(identifier[i]) && !IsValidIdentifierCharacter(identifier[i]) && identifier[i] != '.')
                {
                    return false;
                }
            }
            return true;
        }
        public static bool IsValidIdentifierCharacter(char c)
        {
            return c == '_' || c == '$' || c == '@' || c == '?' || c == '`';
        }
        public static string ConvertString(string str)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (c == '"')
                {
                    stringBuilder.Append("\\\"");
                }
                else
                {
                    stringBuilder.Append(ConvertChar(c));
                }
            }
            return stringBuilder.ToString();
        }
        public static string ConvertChar(char ch)
        {
            char c = ch;
            switch (c)
            {
                case '\0':
                    return "\\0";
                case '\u0001':
                case '\u0002':
                case '\u0003':
                case '\u0004':
                case '\u0005':
                case '\u0006':
                    break;
                case '\a':
                    return "\\a";
                case '\b':
                    return "\\b";
                case '\t':
                    return "\\t";
                case '\n':
                    return "\\n";
                case '\v':
                    return "\\v";
                case '\f':
                    return "\\f";
                case '\r':
                    return "\\r";
                default:
                    if (c == '\\')
                    {
                        return "\\\\";
                    }
                    break;
            }
            if (char.IsControl(ch) || char.IsSurrogate(ch) || (char.IsWhiteSpace(ch) && ch != ' '))
            {
                string arg_AB_0 = "\\u";
                int num = (int)ch;
                return arg_AB_0 + num.ToString("x4");
            }
            return ch.ToString();
        }
        /// <summary>
        /// Removes every occurence of the character if it is not in a string or char literal.
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static string RemoveIfNotString(this string source, char ch)
        {
            StringBuilder temp = new StringBuilder();
            bool isString = false;
            int index = 0;
            foreach (char c in source)
            {
                if (BooleanUtils.And(c == '\"' || c == '\'', source[index - 1] != '\\'))
                {
                    temp.Append(c);
                    isString.Toggle();
                }
                if (c == ch && !isString)
                {
                    index++;
                    continue;
                }
                else
                {
                    temp.Append(c);
                }
                index++;
            }
            return temp.ToString();
        }
    }
}
