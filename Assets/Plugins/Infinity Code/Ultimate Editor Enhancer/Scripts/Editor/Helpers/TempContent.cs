/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static class TempContent
    {
        private static GUIContent _text = new GUIContent();
        private static GUIContent _image = new GUIContent();

        public static GUIContent Get(string text)
        {
            _text.text = text;
            _text.tooltip = null;
            return _text;
        }

        public static GUIContent Get(string text, string tooltip)
        {
            _text.text = text;
            _text.tooltip = tooltip;
            return _text;
        }

        public static GUIContent Get(Texture texture)
        {
            _image.image = texture;
            _image.tooltip = null;
            return _image;
        }

        public static GUIContent Get(Texture texture, string tooltip)
        {
            _image.image = texture;
            _image.tooltip = tooltip;
            return _image;
        }
    }
}