/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using System.Linq;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static partial class Prefs
    {
        public class SceneViewManager : StandalonePrefManager<SceneViewManager>
        {
            public override IEnumerable<string> keywords
            {
                get
                {
                    return ObjectToolbarManager.GetKeywords()
                        .Concat(SwitchCustomToolManager.GetKeywords())
                        .Concat(DistanceToolManager.GetKeywords())
                        .Concat(TerrainBrushSizeManager.GetKeywords())
                        .Concat(ToolValuesManager.GetKeywords());
                }
            }

            public override void Draw()
            {
                ObjectToolbarManager.Draw(null);
                SwitchCustomToolManager.Draw(null);
                TerrainBrushSizeManager.Draw(null);
                DistanceToolManager.Draw(null);
                ToolValuesManager.Draw(null);
            }
        }
    }
}