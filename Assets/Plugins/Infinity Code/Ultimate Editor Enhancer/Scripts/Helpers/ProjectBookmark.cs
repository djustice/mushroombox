/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;

namespace InfinityCode.UltimateEditorEnhancer
{
    [Serializable]
    public class ProjectBookmark : BookmarkItem
    {
        public override bool isProjectItem
        {
            get { return true; }
        }

        public string path
        {
            get;
            set;
        }

        public ProjectBookmark()
        {

        }

        public ProjectBookmark(UnityEngine.Object obj):base(obj)
        {

        }
    }
}