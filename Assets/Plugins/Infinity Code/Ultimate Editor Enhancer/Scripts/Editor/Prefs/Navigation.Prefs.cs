﻿/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using System.Linq;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static partial class Prefs
    {
        public class NavigationManager : StandalonePrefManager<NavigationManager>
        {
            public override IEnumerable<string> keywords
            {
                get 
                { 
                    return JumpToPointManager.GetKeywords()
                        .Concat(PreviewManager.GetKeywords())
                        .Concat(FrameSelectedBoundsManager.GetKeywords())
                        .Concat(ZoomShortcutManager.GetKeywords());
                }
            }

            public override void Draw()
            {
                JumpToPointManager.Draw(null);
                PreviewManager.Draw(null);
                FrameSelectedBoundsManager.Draw(null);
                ZoomShortcutManager.Draw(null);
            }
        }
    }
}