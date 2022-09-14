/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using InfinityCode.UltimateEditorEnhancer.Interceptors;
using InfinityCode.UltimateEditorEnhancer.UnityTypes;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static partial class Prefs
    {
        public static bool animatorInspectorClips = true;
        public static bool boxColliderDetect = true;
        public static bool componentExtraHeaderButtons = true;
        public static bool dragObjectFields = true;
        public static bool emptyInspector = true;
        public static bool hideEmptyHelpButton = true;
        public static bool hidePresetButton = false;
        public static bool inspectorBar = true;
        public static bool inspectorBarShowMaterials = false;
        //public static bool inspectorBarRelatedComponents = true;
        public static bool nestedEditors = true;
        public static NestedEditorSide nestedEditorsSide = NestedEditorSide.right;
        public static bool nestedEditorInReorderableList = true;
        public static bool objectFieldSelector = true;
        public static bool saveComponentRuntime = true;
        public static bool transformInspectorGlobalValues = true;

        public class InspectorManager : StandalonePrefManager<InspectorManager>
        {
            public override IEnumerable<string> keywords
            {
                get
                {
                    return new[]
                    {
                        "Animator Inspector Clips",
                        "Drag Object Field",
                        "Expand Long Text Fields",
                        "Hide Empty Help Buttons",
                        "Hide Preset Button",
                        "Inspector Bar",
                        "Items On Empty Inspector",
                        "Nested Editor",
                        "Object Field Selector",
                    };
                }
            }

            public override void Draw()
            {
                EditorGUI.BeginChangeCheck();
                animatorInspectorClips = EditorGUILayout.ToggleLeft("Animator Inspector Clips", animatorInspectorClips);
                if (EditorGUI.EndChangeCheck()) AnimatorInspectorInterceptor.Refresh();

                DrawComponentHeader();

                dragObjectFields = EditorGUILayout.ToggleLeft("Drag Object Fields", dragObjectFields);
                _expandLongTextFields = EditorGUILayout.ToggleLeft("Expand Long Text Fields", _expandLongTextFields);

                DrawInspectorBar();
                
                emptyInspector = EditorGUILayout.ToggleLeft("Items On Empty Inspector", emptyInspector);
                
                DrawNestedEditor();

                objectFieldSelector = EditorGUILayout.ToggleLeft("Object Field Selector", objectFieldSelector);
            }

            private static void DrawComponentHeader()
            {
                EditorGUILayout.LabelField("Component Header");
                EditorGUI.indentLevel++;

                DrawExtraHeaderButtons();

                EditorGUI.BeginChangeCheck();
                hideEmptyHelpButton = EditorGUILayout.ToggleLeft("Hide Empty Help Button", hideEmptyHelpButton);
                if (EditorGUI.EndChangeCheck()) HelpIconButtonInterceptor.Refresh();

                EditorGUI.BeginChangeCheck();
                hidePresetButton = EditorGUILayout.ToggleLeft("Hide Preset Button", hidePresetButton);
                if (EditorGUI.EndChangeCheck()) DrawPresetButtonInterceptor.Refresh();

                EditorGUI.indentLevel--;

                EditorGUILayout.Space();
            }

            private static void DrawExtraHeaderButtons()
            {
                componentExtraHeaderButtons = EditorGUILayout.ToggleLeft("Extra Header Buttons", componentExtraHeaderButtons);
                EditorGUI.BeginDisabledGroup(!componentExtraHeaderButtons);
                EditorGUI.indentLevel++;
                boxColliderDetect = EditorGUILayout.ToggleLeft("Box Collider Detect Size", boxColliderDetect);
                saveComponentRuntime = EditorGUILayout.ToggleLeft("Save Component At Runtime", saveComponentRuntime);
                transformInspectorGlobalValues = EditorGUILayout.ToggleLeft("Transform Global Values", transformInspectorGlobalValues);
                EditorGUI.indentLevel--;
                EditorGUI.EndDisabledGroup();
            }

            private static void DrawInspectorBar()
            {
                inspectorBar = EditorGUILayout.ToggleLeft("Inspector Bar", inspectorBar);
                EditorGUI.indentLevel++;
                EditorGUI.BeginDisabledGroup(!inspectorBar);

                //inspectorBarRelatedComponents = EditorGUILayout.ToggleLeft("Related Components", inspectorBarRelatedComponents);
                inspectorBarShowMaterials = EditorGUILayout.ToggleLeft("Show Materials", inspectorBarShowMaterials);

                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel--;
            }

            private static void DrawNestedEditor()
            {
                nestedEditors = EditorGUILayout.ToggleLeft("Nested Editors", nestedEditors);
                EditorGUI.indentLevel++;
                EditorGUI.BeginDisabledGroup(!nestedEditors);

                nestedEditorsSide = (NestedEditorSide) EditorGUILayout.EnumPopup("Side", nestedEditorsSide);

                EditorGUI.BeginChangeCheck();
                nestedEditorInReorderableList = EditorGUILayout.ToggleLeft("Show In Reorderable List", nestedEditorInReorderableList);
                if (EditorGUI.EndChangeCheck())
                {
                    ReorderableListInterceptor.Refresh();
                    Object[] windows = UnityEngine.Resources.FindObjectsOfTypeAll(InspectorWindowRef.type);
                    foreach (EditorWindow wnd in windows) wnd.Repaint();
                }

                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel--;
            }
        }
    }
}