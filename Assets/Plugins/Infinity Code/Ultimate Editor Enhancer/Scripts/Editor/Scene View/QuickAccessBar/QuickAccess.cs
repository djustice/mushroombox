/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Linq;
using InfinityCode.UltimateEditorEnhancer.EditorMenus;
using InfinityCode.UltimateEditorEnhancer.Integration;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.SceneTools
{
    [InitializeOnLoad]
    public static class QuickAccess
    {
        public static Action OnCollapseChanged;
        public static Action OnVisibleChanged;

        public static EditorWindow activeWindow;
        public static int activeWindowIndex = -1;
        public static int invokeItemIndex;
        public static Rect invokeItemRect;
        public static int invokeItemVisibleIndex;

        private static GUIStyle _activeContentStyle;
        private static Texture2D _collapseLineTexture;
        private static GUIStyle _contentStyle;
        private static bool _visible;
        private static Texture2D background;
        private static Action invokeItemAction;
        private static Rect rect;

        public static GUIStyle activeContentStyle
        {
            get
            {
                if (_activeContentStyle == null)
                {
                    _activeContentStyle = new GUIStyle(contentStyle);
                    _activeContentStyle.normal.background = _activeContentStyle.active.background;
                }

                return _activeContentStyle;
            }
        }

        public static bool collapsed
        {
            get { return LocalSettings.collapseQuickAccessBar; }
            set
            {
                bool changed = LocalSettings.collapseQuickAccessBar != value;
                LocalSettings.collapseQuickAccessBar = value;
                if (changed && OnCollapseChanged != null) OnCollapseChanged();
            }
        }

        public static GUIStyle contentStyle
        {
            get
            {
                if (_contentStyle == null || _contentStyle.normal.background == null)
                {
                    _contentStyle = new GUIStyle(EditorStyles.toolbarButton)
                    {
                        fontSize = 8,
                        fixedHeight = 32,
                        normal = { background = Resources.CreateSinglePixelTexture(0, 0.1f), textColor = Color.white },
                        hover = { background = Resources.CreateSinglePixelTexture(0, 0.2f) },
                        active = { background = Resources.CreateSinglePixelTexture(0, 0.3f) },
                        padding = new RectOffset()
                    };
                }

                return _contentStyle;
            }
        }

        public static bool visible
        {
            get
            {
                if (!Prefs.quickAccessBar) return false;

                SceneView sceneView = SceneView.lastActiveSceneView;
                if (sceneView == null) return false;

                bool maximized = WindowsHelper.IsMaximized(sceneView);
                return ReferenceManager.quickAccessItems.Any(i => i.Visible(maximized));
            }
        }

        public static float width
        {
            get { return 32; }
        }

        private static Texture2D collapseLineTexture
        {
            get
            {
                if (_collapseLineTexture == null) _collapseLineTexture = Resources.CreateSinglePixelTexture(0.3f, 0.8f, 0.3f, 0.6f);
                return _collapseLineTexture;
            }
        }

        static QuickAccess()
        {
            SceneViewManager.OnValidateOpenContextMenu += OnValidateOpenContextMenu;
            SceneViewManager.AddListener(OnSceneGUI, SceneViewOrder.quickAccessBar, true);
        }

        public static void CheckActiveWindow()
        {
            if (activeWindow == null) activeWindowIndex = -1;
        }

        public static void CloseActiveWindow()
        {
            if (activeWindow == null) return;

            if (EditorMenu.allowCloseWindow) activeWindow.Close(); 
            activeWindow = null;
            activeWindowIndex = -1;
        }

        private static void DrawBackground()
        {
            if (background == null)
            {
                background = new Texture2D(1, 1);
                background.SetPixel(0, 0, new Color(0, 0, 0, 0.6f));
                background.Apply();
            }

            float minIntend = GetMinIntend();
            float maxIntend = Prefs.quickAccessBarIndentMax;

            GUI.DrawTexture(new Rect(0, minIntend, rect.width, rect.height - minIntend - maxIntend), background, ScaleMode.StretchToFill);
        }

        private static void DrawCollapseArea(float minIntend, float maxIntend)
        {
            if (!(EditorWindow.mouseOverWindow is SceneView)) return;

            Event e = Event.current;
            Vector2 p = e.mousePosition;
            if (p.y < minIntend || p.y > rect.height - maxIntend) return;
            if (p.x < rect.width || p.x > rect.width + 3) return;

            if (e.type == EventType.Repaint)
            {
                GUI.DrawTexture(new Rect(rect.width, minIntend, 3, rect.height - minIntend - maxIntend), collapseLineTexture, ScaleMode.StretchToFill);
            }
            else if (e.type == EventType.MouseDown)
            {
                collapsed = true;
                e.Use();
                SceneViewManager.BlockMouseUp();
            }
        }

        private static void DrawContent(SceneView sceneView)
        {
            CheckActiveWindow();

            Event e = Event.current;
            bool maximized = WindowsHelper.IsMaximized(sceneView);

            int index = -1;
            int visibleIndex = -1;

            float y = GetMinIntend();

            foreach (QuickAccessItem item in ReferenceManager.quickAccessItems)
            {
                index++;
                if (item.type == QuickAccessItemType.flexibleSpace)
                {
                    GUILayout.FlexibleSpace();
                    continue;
                }

                if (item.type == QuickAccessItemType.space)
                {
                    GUILayout.Space(item.intSettings[0]);
                    continue;
                }

                if (item.type == QuickAccessItemType.action)
                {
                    item.DrawAction();
                    continue;
                }

                if (item.content == null) continue;
                if (!item.Visible(maximized)) continue;
                visibleIndex++;

                GUIStyle style = activeWindowIndex == index ? activeContentStyle : contentStyle;
                int padding = Mathf.RoundToInt(rect.width / 2 * (1 - item.iconScale));
                style.padding = new RectOffset(padding, padding, padding, padding);
                ButtonEvent buttonEvent = GUILayoutUtils.Button(item.content, style, GUILayout.Width(width), GUILayout.Height(width));
                if (buttonEvent == ButtonEvent.press)
                {
                    if (e.button == 0)
                    {
                        invokeItemRect = GUILayoutUtils.lastRect;
                        invokeItemRect.y += y;
                        invokeItemIndex = index;
                        invokeItemVisibleIndex = visibleIndex;
                        invokeItemAction = item.Invoke;
                        EditorApplication.update += InvokeItemAction;
                        e.Use();
                    }
                    else if (e.button == 1) ShowContextMenu();
                }
            }

            if (e.type == EventType.MouseDown && e.mousePosition.x < width && e.button == 1) ShowContextMenu();
        }

        private static float GetMinIntend()
        {
            float v = 0;

            if (ProGrids.isEnabled)
            {
                if (ProGrids.isMenuOpen) v = 205;
                else v = 25;
            }

            return Mathf.Max(v, Prefs.quickAccessBarIndentMin);
        }

        public static Rect GetFreeRect(Rect sceneRect)
        {
            if (visible && !collapsed) sceneRect.xMin = width;
            return sceneRect;
        }

        private static void InvokeItemAction()
        {
            EditorApplication.update -= InvokeItemAction;
            if (invokeItemAction == null) return;

            try
            {
                invokeItemAction();
            }
            catch
            {
                
            }

            invokeItemAction = null;
            invokeItemVisibleIndex = invokeItemIndex = -1;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            Event e = Event.current;
            if (e.type == EventType.Layout)
            {
                bool v = visible;
                if (v != _visible)
                {
                    if (OnVisibleChanged != null) OnVisibleChanged();
                    _visible = v;
                }
            }
            if (!_visible) return;

            Rect viewRect = SceneViewManager.GetRect(sceneView);

            rect = new Rect(0, 0, width, viewRect.height);

            try
            {
                Handles.BeginGUI();

                float minIntend = GetMinIntend();
                float maxIntend = Mathf.Max(Prefs.quickAccessBarIndentMax, 0);

                if (collapsed)
                {
                    float mx = e.mousePosition.x;
                    if (mx >= 0 && mx < 5)
                    {
                        if (e.type == EventType.Repaint)
                        {
                            GUI.DrawTexture(new Rect(0, minIntend, 3, rect.height - minIntend - maxIntend), collapseLineTexture, ScaleMode.StretchToFill);
                        }
                        else if (e.type == EventType.MouseDown)
                        {
                            collapsed = false;
                            e.Use();
                            SceneViewManager.BlockMouseUp();
                        }
                    }

                    Handles.EndGUI();

                    return;
                }

                DrawBackground();

                GUILayout.BeginArea(new Rect(0, minIntend, rect.width, rect.height - minIntend - maxIntend));
                try
                {
                    DrawContent(sceneView);
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);
                }
                GUILayout.EndArea();

                DrawCollapseArea(minIntend, maxIntend);

                Handles.EndGUI();
            }
            catch
            {
                
            }
        }

        private static bool OnValidateOpenContextMenu()
        {
            return !visible || Event.current.mousePosition.x > width;
        }

        private static void ShowContextMenu()
        {
            GenericMenuEx menu = GenericMenuEx.Start();
            menu.Add("Edit", Settings.OpenQuickAccessSettings);
            menu.Add("Collapse", () => collapsed = true);
            menu.Show();
            Event.current.Use();
        }
    }
}