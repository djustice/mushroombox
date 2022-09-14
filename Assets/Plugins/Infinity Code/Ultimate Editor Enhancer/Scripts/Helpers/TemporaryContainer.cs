/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer
{
    public class TemporaryContainer : MonoBehaviour
    {
        private void Awake()
        {
            Destroy(gameObject);
        }

        public static GameObject GetContainer()
        {
            TemporaryContainer temporaryContainer = FindObjectOfType<TemporaryContainer>();
            if (temporaryContainer == null)
            {
                GameObject go = new GameObject("Temporary Container");
                go.tag = "EditorOnly";
                temporaryContainer = go.AddComponent<TemporaryContainer>();
#if UNITY_EDITOR
                UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Temporary Container");
#endif
            }
            return temporaryContainer.gameObject;
        }
    }
}