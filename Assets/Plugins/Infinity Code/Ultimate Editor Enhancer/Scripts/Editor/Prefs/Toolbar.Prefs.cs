/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static partial class Prefs
    {
        public static TimerMode timerMode = TimerMode.icon;
        public static bool showViewStateToolbarIcon = true;

        public class ToolbarManager : StandalonePrefManager<ToolbarManager>
        {
            public override IEnumerable<string> keywords
            {
                get
                {
                    return ToolbarWindowsManager.GetKeywords().Concat(new []
                    {
                        "Show icon on toolbar if selection has View State",
                        "Timer"
                    });
                }
            }

            public override void Draw()
            {
                showViewStateToolbarIcon = EditorGUILayout.ToggleLeft("Show Icon If Selection Has View State", showViewStateToolbarIcon);
                timerMode = (TimerMode)EditorGUILayout.EnumPopup("Timer Mode", timerMode);
                ToolbarWindowsManager.Draw(null);
            }
        }
    }
}