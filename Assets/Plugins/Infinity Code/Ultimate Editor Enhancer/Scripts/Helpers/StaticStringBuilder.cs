/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Text;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static class StaticStringBuilder
    {
        private static StringBuilder builder = new StringBuilder();

        public static int Length
        {
            get { return builder.Length; }
            set
            {
                if (value >= 0 && value < builder.Length) builder.Length = value;
            }
        }

        public static StringBuilder Append(object value)
        {
            return builder.Append(value);
        }

        public static StringBuilder Append(string value)
        {
            return builder.Append(value);
        }

        public static StringBuilder AppendEscaped(string value, string[] escapeChars)
        {
            if (escapeChars == null || escapeChars.Length % 2 != 0) throw new Exception("Length of escapeChars must be N * 2");
            for (int i = 0; i < value.Length; i++)
            {
                bool escaped = false;
                for (int j = 0; j < escapeChars.Length; j += 2)
                {
                    string s = escapeChars[j];
                    if (value[i] == s[0])
                    {
                        builder.Append(escapeChars[j + 1]);
                        escaped = true;
                        break;
                    }
                }

                if (!escaped) builder.Append(value[i]);
            }

            return builder;
        }

        public static void Clear()
        {
            builder.Length = 0;
        }

        public static string GetString(bool clear = false)
        {
            string s = builder.ToString();
            if (clear) Clear();
            return s;
        }

        public static StringBuilder GetBuilder()
        {
            return builder;
        }

        public static StringBuilder Insert(int index, char value)
        {
            return builder.Insert(index, value);
        }

        public static StringBuilder Insert(int index, string value)
        {
            return builder.Insert(index, value);
        }
    }
}