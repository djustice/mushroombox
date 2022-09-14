/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using System.Linq;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static partial class Prefs
    {
        public class GameObjectManager : StandalonePrefManager<GameObjectManager>
        {
            public override IEnumerable<string> keywords
            {
                get
                {
                    return SelectionBoundsManager.GetKeywords()
                        .Concat(DropToFloorManager.GetKeywords())
                        .Concat(GroupManager.GetKeywords())
                        .Concat(UngroupManager.GetKeywords())
                        .Concat(RenameManager.GetKeywords())
                        .Concat(ReplaceManager.GetKeywords())
                        .Concat(RotateByShortcutManager.GetKeywords());
                }
            }

            public override void Draw()
            {
                SelectionBoundsManager.Draw(null);
                DropToFloorManager.Draw(null);
                GroupManager.Draw(null);
                UngroupManager.Draw(null);
                RenameManager.Draw(null);
                ReplaceManager.Draw(null);
                RotateByShortcutManager.Draw(null);
            }
        }
    }
}