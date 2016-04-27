using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPSGame.Tools
{
    public class StringMod
    {

        /// <summary>
        /// 前后修剪
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Trim(string str)
        {
            if (str == null || "" == str)
            {
                return ("");
            }

            int startIndex = 0;
            while (isWhitespace(str[startIndex]))
            {
                startIndex++;
            };
            int endIndex = (str.Length - 1);
            while (isWhitespace(str[endIndex]))
            {
                endIndex--;
            };
            if (endIndex >= startIndex)
            {
                return (str.Substring(startIndex, (endIndex + 1)));
            };
            return ("");
        }


        public static bool isWhitespace(char character)
        {
            switch (character)
            {
                case ' ':
                case '\t':
                case '\r':
                case '\n':
                case '\f':
                    return (true);
                default:
                    return (false);
            };
        }

        public static bool isWhitespace(string character)
        {
            if (string.IsNullOrEmpty(character))
            {
                return false;
            }

            switch (character[0])
            {
                case ' ':
                case '\t':
                case '\r':
                case '\n':
                case '\f':
                    return (true);
                default:
                    return (false);
            };
        }


        /// <summary>
        /// 忽略大小写判断字符串是否相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool isEqualIgnoreCase(string a, string b)
        {
            return a.ToLower() == b.ToLower();
        }
    }
}
