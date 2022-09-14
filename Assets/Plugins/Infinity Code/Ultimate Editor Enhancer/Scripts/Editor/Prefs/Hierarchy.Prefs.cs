/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using InfinityCode.UltimateEditorEnhancer.HierarchyTools;
using InfinityCode.UltimateEditorEnhancer.Interceptors;
using InfinityCode.UltimateEditorEnhancer.UnityTypes;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static partial class Prefs
    {
        public static bool hierarchyBookmarks = true;
        public static bool hierarchyRowBackground = true;
        public static HierarchyRowBackgroundStyle hierarchyRowBackgroundStyle = HierarchyRowBackgroundStyle.gradient;
        public static bool hierarchyEnableGameObject = true;
        public static bool hierarchyErrorIcons = true;
        public static bool hierarchyIcons = true;
        public static HierarchyIconsDisplayRule hierarchyIconsDisplayRule = HierarchyIconsDisplayRule.onHoverWithModifiers;
        public static bool hierarchyOverrideMainIcon = true;
        public static bool hierarchySoloVisibility = true;

        public static bool hierarchyTree = true;

#if !UNITY_EDITOR_OSX
        public static EventModifiers hierarchyIconsModifiers = EventModifiers.Control;
#else
        public static EventModifiers hierarchyIconsModifiers = EventModifiers.Command;
#endif
        public static int hierarchyIconsMaxItems = 6;

        public class HierarchyManager : StandalonePrefManager<HierarchyManager>, IHasShortcutPref
        {
            private const string sectionLabel = "Show Components In Hierarchy";

            public override IEnumerable<string> keywords
            {
                get { return new[] { "Hierarchy Icons", "Max Items", "Show error icon if GameObject has an error or exception" }; }
            }

            public override float order
            {
                get { return -46; }
            }

            public override void Draw()
            {
                hierarchyEnableGameObject = EditorGUILayout.ToggleLeft("Enable / Disable GameObject", hierarchyEnableGameObject);
                EditorGUI.BeginDisabledGroup(!unsafeFeatures);
                EditorGUI.BeginChangeCheck();
                _hierarchyTypeFilter = EditorGUILayout.ToggleLeft("Filter By Type", _hierarchyTypeFilter);
                if (EditorGUI.EndChangeCheck()) HierarchyToolbarInterceptor.Refresh();
                EditorGUI.EndDisabledGroup();

                DrawRowBackground();
                DrawBestComponents();
                DrawHierarchyIcons();

                hierarchyBookmarks = EditorGUILayout.ToggleLeft("Show Bookmark Button", hierarchyBookmarks);
                hierarchyErrorIcons = EditorGUILayout.ToggleLeft("Show Error Icon When GameObject Has an Error or Exception", hierarchyErrorIcons);
                hierarchySoloVisibility = EditorGUILayout.ToggleLeft("Solo Visibility", hierarchySoloVisibility);
                hierarchyTree = EditorGUILayout.ToggleLeft("Tree", hierarchyTree);
            }

            private static void DrawBestComponents()
            {
                EditorGUI.BeginChangeCheck();
                hierarchyOverrideMainIcon = EditorGUILayout.ToggleLeft("Show Best Component Icon Before Name", hierarchyOverrideMainIcon);
                if (!EditorGUI.EndChangeCheck()) return;

                Object[] windows = UnityEngine.Resources.FindObjectsOfTypeAll(SceneHierarchyWindowRef.type);
                foreach (Object wnd in windows)
                {
                    EditorWindow window = wnd as EditorWindow;
                    HierarchyHelper.SetDefaultIconsSize(window, hierarchyOverrideMainIcon ? 0 : 18);
                    window.Repaint();
                }
            }

            private static void DrawHierarchyIcons()
            {
                hierarchyIcons = EditorGUILayout.ToggleLeft(sectionLabel, hierarchyIcons, EditorStyles.label);

                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Modifiers", GUILayout.Width(labelWidth - 17));
                hierarchyIconsModifiers = DrawModifiers(hierarchyIconsModifiers);
                EditorGUILayout.EndHorizontal();

                hierarchyIconsMaxItems = EditorGUILayout.IntField("Max Items", hierarchyIconsMaxItems);
                if (hierarchyIconsMaxItems < 1) hierarchyIconsMaxItems = 1;

                hierarchyIconsDisplayRule = (HierarchyIconsDisplayRule)EditorGUILayout.EnumPopup("Display Rule", hierarchyIconsDisplayRule);

                EditorGUI.indentLevel--;
            }

            private static void DrawRowBackground()
            {
                hierarchyRowBackground = EditorGUILayout.ToggleLeft("Row Background", hierarchyRowBackground);

                EditorGUI.indentLevel++;
                EditorGUI.BeginDisabledGroup(!hierarchyRowBackground);

                EditorGUI.BeginChangeCheck();
                hierarchyRowBackgroundStyle = (HierarchyRowBackgroundStyle)EditorGUILayout.EnumPopup("Style", hierarchyRowBackgroundStyle);
                if (EditorGUI.EndChangeCheck())
                {
                    BackgroundDrawer.backgroundTexture = null;
                    EditorApplication.RepaintHierarchyWindow();
                }

                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel--;
            }

            public IEnumerable<Shortcut> GetShortcuts()
            {
                if (!hierarchyIcons) return new Shortcut[0];

                return new[]
                {
                    new Shortcut("Show Component Icons", "Hierarchy", hierarchyIconsModifiers)
                };
            }
        }

        public enum HierarchyRowBackgroundStyle
        {
            gradient,
            solid
        }
    }
}