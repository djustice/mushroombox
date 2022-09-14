/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.HierarchyTools
{
    [InitializeOnLoad]
    public static class Header
    {
        static Header()
        {
            HierarchyItemDrawer.Register("Header", OnHierarchyItem, -10000);
        }

        [MenuItem("GameObject/Create Header", priority = 1)]
        public static GameObject Create()
        {
            GameObject go = new GameObject(Prefs.hierarchyHeaderPrefix + "Header");
            go.tag = "EditorOnly";
            GameObject active = Selection.activeGameObject;
            if (active != null)
            {
                go.transform.SetParent(active.transform.parent);
                go.transform.SetSiblingIndex(active.transform.GetSiblingIndex());
            }
            Undo.RegisterCreatedObjectUndo(go, go.name);
            Selection.activeGameObject = go;
            return go;
        }

        private static void OnHierarchyItem(HierarchyItem item)
        {
            if (!Prefs.hierarchyHeaders) return;

            GameObject go = item.gameObject;
            if (go == null) return;

            List<HeaderRule> rules = ReferenceManager.headerRules;
            HeaderRule rule = null;

            for (int i = 0; i < rules.Count; i++)
            {
                if (rules[i].Validate(go))
                {
                    rule = rules[i];
                    break;
                }
            }

            if (rule == null) return;
            rule.Draw(item);

            HierarchyItemDrawer.StopCurrentRowGUI();
        }
    }
}