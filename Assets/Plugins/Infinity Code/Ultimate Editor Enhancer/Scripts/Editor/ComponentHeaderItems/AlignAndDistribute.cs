/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Linq;
using InfinityCode.UltimateEditorEnhancer.Attributes;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.ComponentHeader
{
    public static class AlignAndDistribute
    {
        [ComponentHeaderButton]
        public static bool Draw(Rect rectangle, Object[] targets)
        {
            if (!Validate(targets)) return false;

            if (GUI.Button(rectangle, TempContent.Get(Styles.isProSkin? Icons.align: Icons.alignDark, "Align & Distribute"), Styles.iconButton))
            {
                GenericMenuEx menu = GenericMenuEx.Start();
                GameObject[] gameObjects = targets.Select(t => (t as Transform).gameObject).ToArray();

                menu.Add("Align/X/Min", () =>    GameObjectUtils.Align(gameObjects, 0, 1, 0, 0));
                menu.Add("Align/X/Center", () => GameObjectUtils.Align(gameObjects, 1, 1, 0, 0));
                menu.Add("Align/X/Max", () =>    GameObjectUtils.Align(gameObjects, 2, 1, 0, 0));
                
                menu.Add("Align/Y/Min", () =>    GameObjectUtils.Align(gameObjects, 0, 0, 1, 0));
                menu.Add("Align/Y/Center", () => GameObjectUtils.Align(gameObjects, 1, 0, 1, 0));
                menu.Add("Align/Y/Max", () =>    GameObjectUtils.Align(gameObjects, 2, 0, 1, 0));
                
                menu.Add("Align/Z/Min", () =>    GameObjectUtils.Align(gameObjects, 0, 0, 0, 1));
                menu.Add("Align/Z/Center", () => GameObjectUtils.Align(gameObjects, 1, 0, 0, 1));
                menu.Add("Align/Z/Max", () =>    GameObjectUtils.Align(gameObjects, 2, 0, 0, 1));

                menu.Add("Distribute/X", () => GameObjectUtils.Distribute(gameObjects, 1, 0, 0));
                menu.Add("Distribute/Y", () => GameObjectUtils.Distribute(gameObjects, 0, 1, 0));
                menu.Add("Distribute/Z", () => GameObjectUtils.Distribute(gameObjects, 0, 0, 1));

                menu.Show();
            }

            return true;
        }

        private static bool Validate(Object[] targets)
        {
            if (!Prefs.componentExtraHeaderButtons) return false;
            if (targets.Length < 2) return false;

            Object target = targets[0];
            if (!(target is Transform)) return false;
            if (target is RectTransform) return false;
            return true;
        }
    }
}