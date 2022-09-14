/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static class Links
    {
        public const string assetStore = "https://assetstore.unity.com/packages/tools/utilities/ultimate-editor-enhancer-141831";
        public const string changelog = "https://infinity-code.com/products_update/get-changelog.php?asset=Ultimate%20Editor%20Enhancer&from=1.0";
        public const string documentation = "https://infinity-code.com/documentation/ultimate-editor-enhancer.html";
        public const string forum = "https://forum.infinity-code.com";
        public const string homepage = "https://infinity-code.com/assets/ultimate-editor-enhancer";
        public const string reviews = assetStore + "/reviews";
        public const string support = "mailto:support@infinity-code.com?subject=Ultimate%20Editor%20Enhancer";
        public const string youtube = "https://www.youtube.com/playlist?list=PL2QU1uhBMew_mR83EYhex5q3uZaMTwg1S";
        private const string aid = "?aid=1100liByC";

        public static void Open(string url)
        {
            Application.OpenURL(url);
        }

        public static void OpenAssetStore()
        {
            Open(assetStore + aid);
        }

        public static void OpenChangelog()
        {
            Open(changelog);
        }

        [MenuItem(WindowsHelper.MenuPath + "Documentation", false, 120)]
        public static void OpenDocumentation()
        {
            OpenDocumentation(null);
        }

        public static void OpenDocumentation(string anchor)
        {
            string url = documentation;
            if (!string.IsNullOrEmpty(anchor)) url += "#" + anchor;
            Open(url);
        }

        public static void OpenForum()
        {
            Open(forum);
        }

        public static void OpenHomepage()
        {
            Open(homepage);
        }

        public static void OpenLocalDocumentation()
        {
            string url = Resources.assetFolder + "Documentation/Content/Documentation-Content.html";
            Application.OpenURL(url);
        }

        public static void OpenReviews()
        {
            Open(reviews + aid);
        }

        public static void OpenSupport()
        {
            Open(support);
        }

        public static void OpenYouTube()
        {
            Open(youtube);
        }
    }
}