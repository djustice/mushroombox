/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer
{
    public abstract class SearchableItem
    {
        private const int upFactor = -3;

        protected float _accuracy;

        public float accuracy
        {
            get { return _accuracy; }
            set { _accuracy = value; }
        }

        protected static bool Contains(string str1, string str2)
        {
            if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2) || str2.Length > str1.Length) return false;

            int i, j;

            TextInfo textInfo = Culture.textInfo;

            int l2 = str2.Length;

            for (i = 0; i < str1.Length - l2 + 1; i++)
            {
                for (j = 0; j < l2; j++)
                {
                    char c1 = textInfo.ToUpper(str1[i + j]);
                    char c2 = textInfo.ToUpper(str2[j]);
                    if (c1 != c2) break;
                }

                if (j == l2) return true;
            }

            return false;
        }

        public static float GetAccuracy(string pattern, params string[] values)
        {
            if (values == null || values.Length == 0) return 0;

            if (string.IsNullOrEmpty(pattern))
            {
                return 1;
            }

            float accuracy = 0;

            for (int i = 0; i < values.Length; i++)
            {
                string s = values[i];

                int r = Match(s, pattern);
                if (r == int.MinValue) continue;

                float v = 1 - r / (float)s.Length;
                if (r == pattern.Length * upFactor)
                {
                    accuracy = v;
                    return accuracy;
                }
                if (accuracy < v) accuracy = v;
            }

            return accuracy;
        }

        public static string GetPattern(string str)
        {
            string search = str;

            TextInfo textInfo = Culture.textInfo;
            StaticStringBuilder.Clear();

            bool lastWhite = false;

            for (int i = 0; i < search.Length; i++)
            {
                char c = search[i];
                if (c == ' ' || c == '\t' || c == '\n')
                {
                    if (!lastWhite && StaticStringBuilder.Length > 0)
                    {
                        StaticStringBuilder.Append(' ');
                        lastWhite = true;
                    }
                }
                else
                {
                    StaticStringBuilder.Append(textInfo.ToUpper(c));
                    lastWhite = false;
                }
            }

            if (lastWhite) StaticStringBuilder.Length -= 1;

            return StaticStringBuilder.GetString();
        }

        public static string GetPattern(string str, out string assetType)
        {
            assetType = string.Empty;
            string search = str;

            TextInfo textInfo = Culture.textInfo;

            Match match = Regex.Match(search, @"(:)(\w*)");
            if (match.Success)
            {
                assetType = textInfo.ToUpper(match.Groups[2].Value);
                if (assetType == "PREFAB") assetType = "GAMEOBJECT";
                search = Regex.Replace(search, @"(:)(\w*)", "");
            }

            StaticStringBuilder.Clear();

            bool lastWhite = false;

            for (int i = 0; i < search.Length; i++)
            {
                char c = search[i];
                if (c == ' ' || c == '\t' || c == '\n')
                {
                    if (!lastWhite && StaticStringBuilder.Length > 0)
                    {
                        StaticStringBuilder.Append(' ');
                        lastWhite = true;
                    }
                }
                else
                {
                    StaticStringBuilder.Append(textInfo.ToUpper(c));
                    lastWhite = false;
                }
            }

            if (lastWhite) StaticStringBuilder.Length -= 1;

            return StaticStringBuilder.GetString();
        }

        protected abstract int GetSearchCount();

        protected abstract string GetSearchString(int index);

        protected static int Match(string str1, string str2)
        {
            int bestExtra = int.MaxValue;

            int l1 = str1.Length;
            int l2 = str2.Length;

            TextInfo textInfo = Culture.textInfo;

            for (int i = 0; i < l1 - l2 + 1; i++)
            {
                bool success = true;
                int iOffset = 0;
                int extra = 0;
                bool prevNoSkip = true;

                for (int j = 0; j < l2; j++)
                {
                    char c = str2[j];

                    int j2 = i + j;
                    int i1 = j2 + iOffset;
                    if (i1 >= l1)
                    {
                        success = false;
                        break;
                    }

                    char c1 = str1[i1];

                    if (c1 >= 'A' && c1 <= 'Z')
                    {
                        if (prevNoSkip) extra += upFactor;
                    }
                    else if (c1 >= 'a' && c1 <= 'z') c1 = (char)(c1 - 32);
                    else
                    {
                        char c2 = textInfo.ToUpper(c1);
                        if (c1 == c2 && prevNoSkip) extra += upFactor;
                        c1 = c2;
                    }

                    if (c == c1)
                    {
                        prevNoSkip = true;
                        continue;
                    }

                    if (j == 0)
                    {
                        success = false;
                        break;
                    }

                    bool successSkip = false;
                    iOffset++;
                    int cOffset = 0;

                    while (j2 + iOffset < l1)
                    {
                        char oc = str1[j2 + iOffset];
                        char uc = textInfo.ToUpper(oc);
                        cOffset++;
                        if (uc != c)
                        {
                            iOffset++;
                            continue;
                        }

                        if (oc == uc) extra += upFactor;
                        else extra += cOffset;

                        successSkip = true;
                        break;
                    }

                    if (!successSkip)
                    {
                        success = false;
                        break;
                    }

                    prevNoSkip = false;
                }

                if (success)
                {
                    if (extra == l2 * upFactor) return extra;
                    bestExtra = Math.Min(extra, bestExtra);
                }
            }

            return bestExtra != int.MaxValue ? bestExtra : int.MinValue;
        }

        public virtual float UpdateAccuracy(string pattern)
        {
            _accuracy = 0;

            if (string.IsNullOrEmpty(pattern))
            {
                _accuracy = 1;
                return 1;
            }

            for (int i = 0; i < GetSearchCount(); i++)
            {
                string s = GetSearchString(i);

                int r = Match(s, pattern);
                if (r == int.MinValue) continue;

                float v = 1 - r / (float)s.Length;
                if (r == pattern.Length * upFactor)
                {
                    _accuracy = v;
                    return _accuracy;
                }
                if (_accuracy < v) _accuracy = v;
            }

            return _accuracy;
        }
    }
}