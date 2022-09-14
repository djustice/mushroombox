/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static class TemporaryObjectManager
    {
        [MenuItem(WindowsHelper.MenuPath + "Temporary Objects/Create Container", false, 102)]
        public static void CreateTemporaryContainer()
        {
            GameObject go = TemporaryContainer.GetContainer();
            Undo.RegisterCreatedObjectUndo(go, "Create Temporary Container");
        }

        [MenuItem(WindowsHelper.MenuPath + "Temporary Objects/Destroy Container", false, 102)]
        public static void DestroyTemporaryContainer()
        {
            TemporaryContainer temporaryContainer = Object.FindObjectOfType<TemporaryContainer>();
            if (temporaryContainer == null) return;

            Undo.DestroyObjectImmediate(temporaryContainer.gameObject);
        }
    }
}