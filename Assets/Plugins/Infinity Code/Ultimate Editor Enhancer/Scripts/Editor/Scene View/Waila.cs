/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.SceneTools
{
    [InitializeOnLoad]
    public static class Waila
    {
        public const string StyleID = "sv_label_4";

        public static Action OnClose;
        public static Action OnDrawModeExternal;
        public static Func<GameObject, string, string> OnPrepareTooltip;
        public static Func<bool> OnUpdateTooltipsExternal;

        public static List<GameObject> targets;
        public static int mode; // 0 - tooltip, 1 - smart selection

        public static GUIContent tooltip;
        private static GUIStyle _labelStyle;
        private static GUIStyle tooltipStyle;


        public static GUIStyle labelStyle
        {
            get
            {
                if (_labelStyle == null)
                {
                    _labelStyle = new GUIStyle(StyleID);
                    _labelStyle.fontSize = 10;
                    _labelStyle.normal.background = null;
                    _labelStyle.padding = new RectOffset(0, 4, 2, 0);
                    _labelStyle.margin = new RectOffset(0, 0, 0, 2);
                    _labelStyle.alignment = TextAnchor.MiddleLeft;
                    _labelStyle.wordWrap = false;
                }

                return _labelStyle;
            }
        }

        static Waila()
        {
            SceneViewManager.AddListener(OnSceneGUI, SceneViewOrder.waila, true);
            targets = new List<GameObject>();
        }

        public static void Close()
        {
            mode = 0;
            tooltip = null;

            if (OnClose != null) OnClose();
        }

        private static void DrawTooltip()
        {
            if (tooltipStyle == null)
            {
                tooltipStyle = new GUIStyle(StyleID);
                tooltipStyle.fontSize = 10;
                tooltipStyle.stretchHeight = true;
                tooltipStyle.fixedHeight = 0;
                tooltipStyle.border = new RectOffset(8, 8, 8, 8);
                tooltipStyle.margin = new RectOffset(0, 0, 0, 0);
                tooltipStyle.padding = new RectOffset(8, 8, 8, 8);
                tooltipStyle.alignment = TextAnchor.MiddleLeft;
            }
            Vector2 size = tooltipStyle.CalcSize(tooltip);
            Vector2 position = Event.current.mousePosition - new Vector2(size.x / 2, size.y + 10);
            Rect rect = new Rect(position, size + new Vector2(4, 0));

            Handles.BeginGUI();
            GUI.Label(rect, tooltip, tooltipStyle);
            Handles.EndGUI();
        }

        public static void Highlight(GameObject go)
        {
            if (!Prefs.highlight || !Prefs.highlightOnWaila) return;

            EditorWindow wnd = EditorWindow.mouseOverWindow;
            if (wnd == null || !(wnd is SceneView)) return;

            if (Highlighter.Highlight(go))
            {
                Highlighter.RepaintAllHierarchies();
            }
        }

        private static void InsertTerrainHeight(TerrainCollider collider, ref string tooltipText)
        {
            if (collider == null) return;

            RaycastHit hit;
            if (collider.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out hit, float.PositiveInfinity))
            {
                tooltipText += "\nWorld Position: " + hit.point.ToString("F2");
            }
        }

        private static void OnSceneGUI(SceneView sceneview)
        {
            if (EditorWindow.mouseOverWindow != sceneview)
            {
                if (mode == 0)
                {
                    tooltip = null;
                    Highlight(null);
                    return;
                }
            }
            if (!Prefs.waila) return;

            Event e = Event.current;

            if (Preview.isActive ||
                InputManager.GetAnyMouseButton() && tooltip == null)
            {
                if (mode == 0)
                {
                    tooltip = null;
                    targets.Clear();
                    Highlight(null);
                    return;
                }
            }

            if (mode == 0)
            {
                if (e.type == EventType.MouseMove || e.type == EventType.KeyUp || e.type == EventType.KeyDown)
                {
                    if (OnUpdateTooltipsExternal != null && OnUpdateTooltipsExternal()) { }
                    else if (Prefs.wailaShowNameUnderCursor && e.modifiers == Prefs.wailaShowNameUnderCursorModifiers) UpdateTooltip();
                    else
                    {
                        tooltip = null;
                        targets.Clear();
                        Highlight(null);
                    }
                }

                if (tooltip != null)
                {
                    DrawTooltip();

                    if (e.type == EventType.MouseDown && e.modifiers == Prefs.wailaShowNameUnderCursorModifiers && GUIUtility.hotControl != 0)
                    {
                        Selection.activeGameObject = targets[0];
                        SceneViewManager.AddListener(RestoreSelection);
                        e.Use();
                    }
                }
            }
            else if (OnDrawModeExternal != null) OnDrawModeExternal();
        }

        private static void RestoreSelection(SceneView view)
        {
            if (targets == null || targets.Count == 0)
            {
                SceneViewManager.RemoveListener(RestoreSelection);
                return;
            }

            if (Selection.gameObjects.Length != 1 || Selection.gameObjects[0] != targets[0])
            {
                SceneViewManager.RemoveListener(RestoreSelection);
                Selection.activeGameObject = targets[0];
            }
        }

        private static void UpdateTooltip()
        {
            GameObject go = HandleUtility.PickGameObject(Event.current.mousePosition, false, null);
            if (go == null)
            {
                Highlight(null);
                tooltip = null;
                targets.Clear();
                return;
            }

            if (PrefabUtility.IsPartOfAnyPrefab(go)) go = PrefabUtility.GetNearestPrefabInstanceRoot(go);

            Highlight(go);

            if (targets.Count != 1 || targets[0] != go)
            {
                targets.Clear();
                targets.Add(go);
            }

            string tooltipText = go.name;
            TerrainCollider terrainCollider = targets[0].GetComponent<TerrainCollider>();

            if (terrainCollider != null) InsertTerrainHeight(terrainCollider, ref tooltipText);

            if (OnPrepareTooltip != null)
            {
                tooltipText = OnPrepareTooltip(go, tooltipText);
            }

            tooltip = new GUIContent(tooltipText);
        }
    }
}