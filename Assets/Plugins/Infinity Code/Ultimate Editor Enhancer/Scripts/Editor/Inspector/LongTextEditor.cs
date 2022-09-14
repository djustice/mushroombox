/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using InfinityCode.UltimateEditorEnhancer.Interceptors;
using InfinityCode.UltimateEditorEnhancer.Windows;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.InspectorTools
{
    [InitializeOnLoad]
    public static class LongTextEditor
    {
        private const int buttonWidth = 20;

        private static int targetID = int.MinValue;
        private static string restoreText;
        private static bool drawButton;
        private static Rect buttonRect;

        static LongTextEditor()
        {
            EditorGUIDoTextFieldInterceptor.OnPrefix += OnPrefix;
            EditorGUIDoTextFieldInterceptor.OnPostfix += OnPostfix;
        }

        private static void OnPrefix(TextEditor editor, int id, ref Rect position, ref string text, GUIStyle style, string allowedletters, ref bool changed, bool reset, bool multiline, bool passwordfield)
        {
            if (!Prefs.expandLongTextFields) return;

            drawButton = false;
            if (multiline || passwordfield || allowedletters != null) return;
            if (EditorStyles.textField.CalcSize(TempContent.Get(text)).x < position.width - 10) return;

            drawButton = true;
            buttonRect = new Rect(position.xMax - buttonWidth, position.y, buttonWidth, position.height);
            position.width -= buttonRect.width + 2;
        }

        private static void OnPostfix(TextEditor editor, int id, ref Rect position, ref string text, GUIStyle style, string allowedletters, ref bool changed, bool reset, bool multiline, bool passwordfield)
        {
            if (!Prefs.expandLongTextFields) return;

            if (targetID == id && restoreText != null)
            {
                text = restoreText;
                editor.text = text;
                changed = true;
                targetID = int.MinValue;
                restoreText = null;
                GUI.changed = true;
            }

            if (!drawButton) return;
            if (GUI.Button(buttonRect, TempContent.Get(Icons.upDown, "Expand Text Field")))
            {
                targetID = id;
                LongTextEditorWindow.OpenWindow(text, new Rect(position.x - 3, position.y - 2, position.width + buttonWidth + 5, position.height + 2)).OnClose += s => { restoreText = s; };
            }
        }
    }
}