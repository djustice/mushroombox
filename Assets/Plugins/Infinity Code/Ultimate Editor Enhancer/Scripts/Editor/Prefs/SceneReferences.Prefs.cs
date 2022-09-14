﻿/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using UnityEditor;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static partial class Prefs
    {
        public static bool hideSceneReferences = true;

        public class SceneReferencesManager : PrefManager
        {
            public override IEnumerable<string> keywords
            {
                get
                {
                    return new[]
                    {
                        "Hide Scene References",
                    };
                }
            }

            public override float order
            {
                get { return Order.sceneReferences; }
            }

            public override void Draw()
            {
                EditorGUI.BeginChangeCheck();
                hideSceneReferences = EditorGUILayout.ToggleLeft("Hide Scene References", hideSceneReferences);
                if (EditorGUI.EndChangeCheck())
                {
                    SceneManagerHelper.UpdateInstances();
                    EditorApplication.RepaintHierarchyWindow();
                }
            }
        }
    }
}