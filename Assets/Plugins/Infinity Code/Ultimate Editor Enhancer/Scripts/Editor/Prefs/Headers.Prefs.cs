/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static partial class Prefs
    {
        public static bool hierarchyHeaders = true;
        public static string hierarchyHeaderPrefix = "--";

        public class HeadersManager : StandalonePrefManager<HeadersManager>
        {
            private static SerializedObject so;

            public override void Draw()
            {
                hierarchyHeaders = EditorGUILayout.ToggleLeft("Headers", hierarchyHeaders);
                EditorGUI.BeginDisabledGroup(!hierarchyHeaders);

                if (so == null) so = new SerializedObject(ReferenceManager.instance);
                SerializedProperty prop = so.FindProperty("_headerRules");
                EditorGUILayout.PropertyField(prop, true);
                so.ApplyModifiedProperties();

                EditorGUI.EndDisabledGroup();
            }
        }
    }
}