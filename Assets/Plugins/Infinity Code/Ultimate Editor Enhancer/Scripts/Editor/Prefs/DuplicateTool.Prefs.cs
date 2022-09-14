/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using UnityEditor;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static partial class Prefs
    {
        public static bool hideDuplicateToolTemp = true;

        public class DuplicateToolManager : PrefManager
        {
            public override IEnumerable<string> keywords
            {
                get { return new[] { "Duplicate Tool" }; }
            }

            public override float order
            {
                get { return (Order.sceneReferences + Order.improveBehaviors) / 2f; }
            }

            public override void Draw()
            {
                hideDuplicateToolTemp = EditorGUILayout.ToggleLeft("Hide Temporary Objects of Duplicate Tool", hideDuplicateToolTemp);
            }
        }
    }
}