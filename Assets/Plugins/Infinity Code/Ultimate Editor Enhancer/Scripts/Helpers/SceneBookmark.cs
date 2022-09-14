/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;

namespace InfinityCode.UltimateEditorEnhancer
{
    [Serializable]
    public class SceneBookmark : BookmarkItem
    {
        public override bool isProjectItem
        {
            get { return false; }
        }

        public SceneBookmark()
        {

        }

        public SceneBookmark(UnityEngine.Object obj) : base(obj)
        {

        }
    }
}