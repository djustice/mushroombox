/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.SceneTools
{
    [InitializeOnLoad]
    public static class HighJumpToPoint
    {
        private static double lastShiftPressed;

#if UNITY_EDITOR_OSX
        private const EventModifiers MODIFIERS = EventModifiers.Command | EventModifiers.Shift;
#else
        private const EventModifiers MODIFIERS = EventModifiers.Control | EventModifiers.Shift;
#endif

        static HighJumpToPoint()
        {
            SceneViewManager.AddListener(OnSceneGUI);
        }

        private static void OnSceneGUI(SceneView view)
        {
            if (!Prefs.highJumpToPoint || view.orthographic) return;

            Event e = Event.current;
            bool isJump = e.type == EventType.MouseUp && e.button == 2 && e.modifiers == MODIFIERS;

            if (!isJump && Prefs.alternativeJumpShortcut && EditorWindow.mouseOverWindow is SceneView)
            {
                bool isAlternativeShortcut = e.type == EventType.KeyUp && (e.control || e.command) && (e.keyCode == KeyCode.LeftShift || e.keyCode == KeyCode.RightShift);
                if (isAlternativeShortcut)
                {
                    double timeDelta = EditorApplication.timeSinceStartup - lastShiftPressed;
                    isJump = timeDelta < JumpToPoint.MAX_SHIFT_DELAY;
                    lastShiftPressed = isJump ? 0 : EditorApplication.timeSinceStartup;
                }
            }

            if (!isJump) return;

            if (!JumpToPoint.GetTargetPoint(e, out Vector3 targetPosition)) return;

            view.LookAt(targetPosition + new Vector3(0, 20, 0), view.rotation, 20);

            UnityEditor.Tools.viewTool = ViewTool.None;
            GUIUtility.hotControl = 0;

            e.Use();
        }
    }
}