/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Collections.Generic;
using InfinityCode.UltimateEditorEnhancer.Tools;
using InfinityCode.UltimateEditorEnhancer.UnityTypes;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.SceneTools
{
    [InitializeOnLoad]
    public static class SmartSelection
    {
        private static GUIStyle _areaStyle;
        private static GameObject highlightGO;
        private static GameObject lastHighlightGO;
        private static Rect screenRect;

        private static GUIStyle areaStyle
        {
            get
            {
                if (_areaStyle == null)
                {
                    _areaStyle = new GUIStyle(Waila.StyleID);
                    _areaStyle.fontSize = 10;
                    _areaStyle.stretchHeight = true;
                    _areaStyle.fixedHeight = 0;
                    _areaStyle.border = new RectOffset(8, 8, 8, 8);
                    _areaStyle.margin = new RectOffset(4, 4, 4, 4);
                }

                return _areaStyle;
            }
        }

        static SmartSelection()
        {
            KeyManager.KeyBinding binding = KeyManager.AddBinding();
            binding.OnValidate += () => Prefs.waila && Prefs.wailaSmartSelection;
            binding.OnInvoke += OnInvoke;

            Waila.OnClose += OnClose;
            Waila.OnDrawModeExternal += OnDrawModeExternal;
            Waila.OnUpdateTooltipsExternal += OnUpdateTooltipsExternal;
        }

        private static void DrawButton(ref Rect r, Transform t, bool addSlash, ref bool state)
        {
            if (t.parent != null)
            {
                DrawButton(ref r, t.parent, true, ref state);
            }

            Rect r2 = new Rect(r);
            GUIContent content = new GUIContent(t.gameObject.name);
            GUIStyle style = Waila.labelStyle;
            r2.width = style.CalcSize(content).x + style.margin.horizontal;

            r.xMin += r2.width;

            ButtonEvent e = GUILayoutUtils.Button(r2, content, style);

            if (e == ButtonEvent.click)
            {
                if (Event.current.control || Event.current.shift) SelectionRef.Add(t.gameObject);
                else Selection.activeGameObject = t.gameObject;
                state = true;
            }
            else if (e == ButtonEvent.drag)
            {
                DragAndDrop.PrepareStartDrag();
                DragAndDrop.objectReferences = new[] { t.gameObject };
                DragAndDrop.StartDrag("Drag " + t.gameObject.name);
                Event.current.Use();
                state = true;
            }

            if (r2.Contains(Event.current.mousePosition))
            {
                highlightGO = t.gameObject;
            }

            if (addSlash)
            {
                r2.xMin = r2.xMax;
                content.text = "/";
                r2.width = style.CalcSize(content).x + style.margin.horizontal;
                GUI.Label(r2, content, style);
                r.xMin += r2.width;
            }
        }

        private static void DrawBubbleSmartSelection()
        {
            if (!UnityEditor.Tools.hidden) UnityEditor.Tools.hidden = true;

            Event e = Event.current;
            EventType type = e.type;

            highlightGO = null;

            try
            {
                Handles.BeginGUI();

                if (e.type == EventType.Repaint) areaStyle.Draw(screenRect, GUIContent.none, -1);

                GUIStyle style = Waila.labelStyle;
                RectOffset margin = style.margin;
                RectOffset padding = style.padding;

                Rect r = new Rect(screenRect.x + 5, screenRect.y + margin.top + padding.top, screenRect.width - 10, style.lineHeight + margin.vertical + padding.vertical);

                GUI.Label(r, "Select GameObject:", style);
                r.y += r.height + margin.bottom;
                r.height = 1;
                EditorGUI.DrawRect(r, new Color(0.5f, 0.5f, 0.5f, 1));

                r.y += 2;
                r.height = style.lineHeight + margin.vertical + padding.vertical;

                try
                {
                    bool state = false;

                    for (int i = 0; i < Waila.targets.Count; i++)
                    {
                        Rect r2 = new Rect(r);
                        r2.y += i * (style.lineHeight + margin.vertical + padding.vertical);
                        Transform t = Waila.targets[i].transform;
                        try
                        {
                            DrawButton(ref r2, t, false, ref state);
                        }
                        catch
                        {
                        }
                    }

                    if (state)
                    {
                        Waila.mode = 0;
                        UnityEditor.Tools.hidden = false;
                    }
                }
                catch (Exception ex)
                {
                    Log.Add(ex);
                }

                if (lastHighlightGO != highlightGO)
                {
                    lastHighlightGO = highlightGO;
                    Highlighter.Highlight(highlightGO);
                }

                if (type == EventType.MouseUp)
                {
                    Waila.mode = 0;
                    UnityEditor.Tools.hidden = false;
                }
                else if (type == EventType.KeyDown)
                {
                    if (e.keyCode != KeyCode.LeftShift && e.keyCode != KeyCode.RightShift && e.keyCode != KeyCode.LeftControl && e.keyCode != KeyCode.RightControl)
                    {
                        Waila.mode = 0;
                        UnityEditor.Tools.hidden = false;
                    }
                }

                Handles.EndGUI();
            }
            catch
            {
            }
        }

        private static void DrawSmartSelection()
        {
            if (Prefs.wailaSmartSelectionStyle == SmartSelectionStyle.bubble) DrawBubbleSmartSelection();
        }

        private static void OnClose()
        {
            Waila.Highlight(null);
        }

        private static void OnDrawModeExternal()
        {
            if (Waila.mode != 1) return;

            DrawSmartSelection();
        }

        private static void OnInvoke()
        {
            if (Waila.mode != 0) return;

            Event e = Event.current;

            if (e.type == EventType.KeyDown &&
                e.keyCode == Prefs.wailaSmartSelectionKeyCode &&
                e.modifiers == Prefs.wailaSmartSelectionModifiers)
            {
                ShowSmartSelection();
                e.Use();
            }
        }

        private static bool OnUpdateTooltipsExternal()
        {
            if (Prefs.wailaShowAllNamesUnderCursor && Event.current.modifiers == Prefs.wailaShowAllNamesUnderCursorModifiers)
            {
                UpdateAllTooltips();
                return true;
            }

            return false;
        }

        private static bool PrepareBubble(List<GameObject> targets, GUIStyle style, RectOffset margin, float height, RectOffset padding, Vector2 slashSize, int rightMargin, float width)
        {
            int count = 0;

            try
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    GameObject go = targets[i];
                    if (go == null) break;

                    float w = 0;
                    Transform t = go.transform;
                    Vector2 contentSize = style.CalcSize(new GUIContent(t.gameObject.name));
                    w += contentSize.x + margin.horizontal;
                    height += contentSize.y + margin.bottom + padding.bottom;

                    while (t.parent != null)
                    {
                        t = t.parent;
                        w += slashSize.x + rightMargin;
                        contentSize = style.CalcSize(new GUIContent(t.gameObject.name));
                        w += contentSize.x + rightMargin;
                    }

                    w += 5;
                    if (w > width) width = w;

                    count++;
                }
            }
            catch (Exception e)
            {
                Log.Add(e);
            }

            if (count == 0) return false;

            Vector2 size = new Vector2(width + 12, height + 32);
            Vector2 position = Event.current.mousePosition - new Vector2(size.x / 2, size.y * 1.5f);

            if (position.x < 5) position.x = 5;
            else if (position.x + size.x > EditorWindow.focusedWindow.position.width - 5) position.x = EditorWindow.focusedWindow.position.width - size.x - 5;

            if (position.y < 5) position.y = 5;
            else if (position.y + size.y > EditorWindow.focusedWindow.position.height - 5) position.y = EditorWindow.focusedWindow.position.height - size.y - 5;

            screenRect = new Rect(position, size);
            return true;
        }

        private static void ShowSmartSelection()
        {
            if (!(EditorWindow.mouseOverWindow is SceneView)) return;

            List<GameObject> targets = Waila.targets;

            if (!Prefs.wailaShowAllNamesUnderCursor) UpdateAllTooltips();

            if (targets == null || targets.Count == 0) return;

            GUIStyle style = Waila.labelStyle;
            RectOffset margin = style.margin;
            RectOffset padding = style.padding;

            float width = style.CalcSize(new GUIContent("Select GameObject")).x + margin.horizontal + 10;

            int rightMargin = margin.right;
            Vector2 slashSize = style.CalcSize(new GUIContent("/"));

            float height = margin.top;

            if (Prefs.wailaSmartSelectionStyle == SmartSelectionStyle.bubble)
            {
                if (!PrepareBubble(targets, style, margin, height, padding, slashSize, rightMargin, width))
                {
                    return;
                }
            }
            else FlatSmartSelectionWindow.Show();

            Waila.mode = 1;
            Waila.tooltip = null;
        }

        private static void UpdateAllTooltips()
        {
            Waila.tooltip = null;

            int count = 0;

            StaticStringBuilder.Clear();

            Waila.targets.Clear();

            while (count < 20)
            {
                GameObject go = HandleUtility.PickGameObject(Event.current.mousePosition, false, Waila.targets.ToArray());
                if (go == null) break;

                Waila.targets.Add(go);
                if (count > 0) StaticStringBuilder.Append("\n");
                int length = StaticStringBuilder.Length;
                Transform t = go.transform;
                StaticStringBuilder.Append(t.gameObject.name);
                while (t.parent != null)
                {
                    t = t.parent;
                    StaticStringBuilder.Insert(length, " / ");
                    StaticStringBuilder.Insert(length, t.gameObject.name);
                }

                count++;
            }

            if (Waila.targets.Count > 0) Waila.Highlight(Waila.targets[0]);
            else Waila.Highlight(null);

            if (count > 0) Waila.tooltip = new GUIContent(StaticStringBuilder.GetString(true));
        }
    }
}