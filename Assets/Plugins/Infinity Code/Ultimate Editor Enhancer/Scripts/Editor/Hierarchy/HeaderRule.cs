/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.HierarchyTools
{
    [Serializable]
    public class HeaderRule
    {
        public HeaderCondition condition = HeaderCondition.nameStarts;
        public string value;
        public string trimChars = "-=";
        public Color backgroundColor = Color.gray;
        public Color textColor = Color.white;
        public TextAlignment textAlign = TextAlignment.Center;
        public FontStyle textStyle = FontStyle.Bold;

        private Color _backgroundColor;
        private Color _textColor;
        private TextAlignment _textAlign = TextAlignment.Center;
        private FontStyle _textStyle = FontStyle.Bold;

        private GUIStyle _headerStyle;

        private GUIStyle headerStyle
        {
            get
            {
                if (_headerStyle == null || CheckStyleChanges())
                {
                    _headerStyle = new GUIStyle();
                    Color color = backgroundColor;
                    color.a = 1;
                    _headerStyle.normal.background = Resources.CreateSinglePixelTexture(color);
                    _textColor = new Color(textColor.r, textColor.g, textColor.b, 1);
                    _headerStyle.normal.textColor = _textColor;

                    if (textAlign == TextAlignment.Center) _headerStyle.alignment = TextAnchor.MiddleCenter;
                    else if (textAlign == TextAlignment.Left) _headerStyle.alignment = TextAnchor.MiddleLeft;
                    else _headerStyle.alignment = TextAnchor.MiddleRight;

                    _headerStyle.fontStyle = textStyle;
                    _headerStyle.padding = new RectOffset(8, 8, 0, 0);

                    _backgroundColor = backgroundColor;
                    _textAlign = textAlign;
                    _textStyle = textStyle;
                }
                else if (_headerStyle.normal.background == null)
                {
                    Color color = backgroundColor;
                    color.a = 1;
                    _headerStyle.normal.background = Resources.CreateSinglePixelTexture(color);
                }

                return _headerStyle;
            }
        }

        private bool CheckStyleChanges()
        {
            return backgroundColor != _backgroundColor ||
                   textAlign != _textAlign ||
                   textStyle != _textStyle ||
                   MathHelper.ColorsEqualWithoutAlpha(textColor, _textColor);
        }

        public void Draw(HierarchyItem item)
        {
            if (Event.current.type != EventType.Repaint) return;

            string name = item.gameObject.name;

            Rect rect = item.rect;
            Rect r = new Rect(32, rect.y, rect.xMax - 16, rect.height);

            int start = 0;
            int end = name.Length;

            for (int i = start; i < name.Length; i++)
            {
                char c = name[i];
                int j;
                for (j = 0; j < trimChars.Length; j++)
                {
                    if (trimChars[j] == c)
                    {
                        start++;
                        break;
                    }
                }
                if (j == trimChars.Length) break;
            }

            for (int i = end - 1; i > start; i--)
            {
                char c = name[i];
                int j;
                for (j = 0; j < trimChars.Length; j++)
                {
                    if (trimChars[j] == c)
                    {
                        end--;
                        break;
                    }
                }
                if (j == trimChars.Length) break;
            }

            name = name.Substring(start, end - start);

            headerStyle.Draw(r, TempContent.Get(name), 0, false, false);
        }

        public bool Validate(GameObject go)
        {
            string name = go.name;
            if (condition == HeaderCondition.nameStarts) return name.StartsWith(value);
            if (condition == HeaderCondition.nameContains) return name.Contains(value);
            if (condition == HeaderCondition.nameEqual) return name == value;
            return false;
        }
    }
}