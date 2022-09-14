/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.HierarchyTools
{
    [InitializeOnLoad]
    public static class BackgroundDrawer
    {
        public static Texture2D backgroundTexture;

        static BackgroundDrawer()
        {
            HierarchyItemDrawer.Register("BackgroundDrawer", OnDrawItem, -1);
        }

        private static void InitTexture()
        {
            if (Prefs.hierarchyRowBackgroundStyle == Prefs.HierarchyRowBackgroundStyle.gradient)
            {
                backgroundTexture = Resources.Load<Texture2D>("Textures/Other/HierarchyBackground.png");
                if (backgroundTexture == null) backgroundTexture = Resources.CreateSinglePixelTexture(1, 0.3f);
            }
            else backgroundTexture = Resources.CreateSinglePixelTexture(1, 0.3f);
        }

        private static void OnDrawItem(HierarchyItem item)
        {
            if (!Prefs.hierarchyRowBackground) return;
            if (Event.current.type != EventType.Repaint) return;

            GameObject target = item.gameObject;
            if (target == null) return;

            SceneReferences r = SceneReferences.Get(item.gameObject.scene, false);
            if (r == null) return;

            SceneReferences.HierarchyBackground background = r.GetBackground(item.gameObject);
            if (background == null) return;

            if (backgroundTexture == null) InitTexture();

            Color guiColor = GUI.color;
            GUI.color = background.color;
            Rect rect = item.rect;
            rect.xMin += 16;
            GUI.DrawTexture(rect, backgroundTexture, ScaleMode.StretchToFill);
            GUI.color = guiColor;
        }
    }
}