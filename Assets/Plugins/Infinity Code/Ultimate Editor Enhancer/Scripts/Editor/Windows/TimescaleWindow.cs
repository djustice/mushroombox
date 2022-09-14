/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Linq;
using InfinityCode.UltimateEditorEnhancer.Tools;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.Windows
{
    [InitializeOnLoad]
    public class TimescaleWindow : EditorWindow
    {
        private bool closeOnLossFocus;

        static TimescaleWindow()
        {
            Timer.OnLeftClick += OnTimerClick;
        }

        private void OnGUI()
        {
            if (closeOnLossFocus && focusedWindow != this && focusedWindow != this)
            {
                Close();
                return;
            }

            Event e = Event.current;
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape)
            {
                e.Use();
                Close();
                return;
            }

            Time.timeScale = EditorGUILayout.Slider("Timescale", Time.timeScale, 0, 100);
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("0.1")) Time.timeScale = 0.1f;
            if (GUILayout.Button("0.25")) Time.timeScale = 0.25f;
            if (GUILayout.Button("0.5")) Time.timeScale = 0.5f;
            if (GUILayout.Button("1")) Time.timeScale = 1f;
            if (GUILayout.Button("2")) Time.timeScale = 2f;
            if (GUILayout.Button("5")) Time.timeScale = 5f;
            if (GUILayout.Button("..."))
            {
                InputDialog.Show("Enter Timescale (0-100)", Time.timeScale.ToString(), s =>
                {
                    float v;
                    if (float.TryParse(s, out v))
                    {
                        if (v < 0) EditorUtility.DisplayDialog("Error", "TimeScale is out of range. The value cannot be less than 0.0.", "OK");
                        else if (v > 100) EditorUtility.DisplayDialog("Error", "TimeScale is out of range. When running in the editor this value needs to be less than or equal to 100.0", "OK");
                        else Time.timeScale = v;
                    }
                });
            }


            EditorGUILayout.EndHorizontal();
        }

        private static void OnTimerClick()
        {
            TimescaleWindow[] windows = UnityEngine.Resources.FindObjectsOfTypeAll<TimescaleWindow>();
            if (windows.Any(w => w.closeOnLossFocus))
            {
                foreach (TimescaleWindow window in windows)
                {
                    if (window.closeOnLossFocus) window.Close();
                }
            }
            else
            {
                Rect rect = GUILayoutUtils.lastRect;
                ShowPopupWindow(rect.position + new Vector2(0, rect.height + 5));
            }
        }

        public static EditorWindow ShowPopupWindow(Vector2 position)
        {
            TimescaleWindow wnd = CreateInstance<TimescaleWindow>();
            wnd.titleContent = new GUIContent("Bookmarks");
            position = GUIUtility.GUIToScreenPoint(position);
            Vector2 size = new Vector2(300, 44);
            Rect rect = new Rect(position, size);

            wnd.minSize = rect.size;
            wnd.position = rect;
            wnd.closeOnLossFocus = true;
            wnd.ShowPopup();
            wnd.Focus();

            return wnd;
        }

        [MenuItem(WindowsHelper.MenuPath + "Timescale", false, 102)]
        public static void ShowWindow()
        {
            TimescaleWindow wnd = GetWindow<TimescaleWindow>("Timescale");
            Rect rect = wnd.position;
            rect.size = new Vector2(300, 40);
            wnd.minSize = rect.size;
            wnd.position = rect;
        }
    }
}