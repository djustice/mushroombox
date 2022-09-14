/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using InfinityCode.UltimateEditorEnhancer.UnityTypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static class HierarchyHelper
    {
        public static void ExpandHierarchy(EditorWindow window, GameObject selection)
        {
            if (selection != null)
            {
                SceneHierarchyWindowRef.SetExpandedRecursive(window, selection.GetInstanceID(), true);
            }
            else
            {
                object sceneHierarchy = SceneHierarchyWindowRef.GetSceneHierarchy(window);
                SceneHierarchyRef.SetScenesExpanded(sceneHierarchy, new List<string> { SceneManager.GetActiveScene().name });
            }
        }

        public static void SetDefaultIconsSize(EditorWindow hierarchyWindow, int size = 0)
        {
            object sceneHierarchy = SceneHierarchyWindowRef.GetSceneHierarchy(hierarchyWindow);
            if (sceneHierarchy == null) return;

            object treeView = SceneHierarchyRef.GetTreeView(sceneHierarchy);
            if (treeView == null) return;

            object gui = TreeViewControllerRef.GetGUI(treeView);
            if (gui == null) return;

            TreeViewGUIRef.SetIconWidth(gui, size);
            TreeViewGUIRef.SetSpaceBetweenIconAndText(gui, 18 - size);
        }
    }
}