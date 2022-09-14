/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using UnityEditor;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static class Settings
    {
        // First level
        private const string UEE_SETTINGS_PATH = "Project/Ultimate Editor Enhancer";
        private const string CONTEXT_MENU_SETTINGS_PATH = UEE_SETTINGS_PATH + "/Context Menu";
        private const string CREATE_BROWSER_SETTINGS_PATH = UEE_SETTINGS_PATH + "/Create Browser";
        private const string FAVORITE_WINDOWS_SETTINGS_PATH = UEE_SETTINGS_PATH + "/Favorite Windows";
        private const string GAMEOBJECT_SETTINGS_PATH = UEE_SETTINGS_PATH + "/GameObject";
        private const string HIERARCHY_SETTINGS_PATH = UEE_SETTINGS_PATH + "/Hierarchy";
        private const string INSPECTOR_SETTINGS_PATH = UEE_SETTINGS_PATH + "/Inspector";
        private const string SCENEVIEW_SETTINGS_PATH = UEE_SETTINGS_PATH + "/Scene View";
        private const string SEARCH_WINDOWS_SETTINGS_PATH = UEE_SETTINGS_PATH + "/Search";
        private const string TOOLBAR_SETTINGS_PATH = UEE_SETTINGS_PATH + "/Toolbar";
        private const string UNSAFE_SETTINGS_PATH = UEE_SETTINGS_PATH + "/Unsafe";
        private const string VIEWS_SETTINGS_PATH = UEE_SETTINGS_PATH + "/Views";

        // Second level
        private const string HEADERS_SETTINGS_PATH = HIERARCHY_SETTINGS_PATH + "/Headers";
        private const string HIGHLIGHT_SETTINGS_PATH = SCENEVIEW_SETTINGS_PATH + "/Highlight";
        private const string NAVIGATION_SETTINGS_PATH = SCENEVIEW_SETTINGS_PATH + "/Navigation";
        private const string QUICK_ACCESS_SETTINGS_PATH = SCENEVIEW_SETTINGS_PATH + "/Quick Access Bar";
        private const string WAILA_SETTINGS_PATH = SCENEVIEW_SETTINGS_PATH + "/WAILA";

        [SettingsProvider]
        public static SettingsProvider GetContextMenuSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider(CONTEXT_MENU_SETTINGS_PATH, SettingsScope.Project)
            {
                label = "Context Menu",
                guiHandler = Prefs.ContextMenuManager.DrawWithToolbar,
                keywords = Prefs.ContextMenuManager.GetKeywords()
            };
            return provider;
        }

        [SettingsProvider]
        public static SettingsProvider GetCreateBrowserSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider(CREATE_BROWSER_SETTINGS_PATH, SettingsScope.Project)
            {
                label = "Create Browser",
                guiHandler = Prefs.CreateBrowserManager.DrawWithToolbar,
                keywords = Prefs.CreateBrowserManager.GetKeywords()
            };
            return provider;
        }

        [SettingsProvider]
        public static SettingsProvider GetFavoriteWindowsSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider(FAVORITE_WINDOWS_SETTINGS_PATH, SettingsScope.Project)
            {
                label = "Favorite Windows",
                guiHandler = Prefs.FavoriteWindowsManager.DrawWithToolbar,
                keywords = Prefs.FavoriteWindowsManager.GetKeywords()
            };
            return provider;
        }

        [SettingsProvider]
        public static SettingsProvider GetGameObjectSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider(GAMEOBJECT_SETTINGS_PATH, SettingsScope.Project)
            {
                label = "GameObjects",
                guiHandler = Prefs.GameObjectManager.DrawWithToolbar,
                keywords = Prefs.GameObjectManager.GetKeywords()
            };
            return provider;
        }

        [SettingsProvider]
        public static SettingsProvider GetHeadersSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider(HEADERS_SETTINGS_PATH, SettingsScope.Project)
            {
                label = "Headers",
                guiHandler = Prefs.HeadersManager.DrawWithToolbar,
                keywords = Prefs.HeadersManager.GetKeywords()
            };
            return provider;
        }

        [SettingsProvider]
        public static SettingsProvider GetHierarchySettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider(HIERARCHY_SETTINGS_PATH, SettingsScope.Project)
            {
                label = "Hierarchy",
                guiHandler = Prefs.HierarchyManager.DrawWithToolbar,
                keywords = Prefs.HierarchyManager.GetKeywords()
            };
            return provider;
        }

        [SettingsProvider]
        public static SettingsProvider GetHighlightSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider(HIGHLIGHT_SETTINGS_PATH, SettingsScope.Project)
            {
                label = "Highlighter",
                guiHandler = Prefs.HighlightManager.DrawWithToolbar,
                keywords = Prefs.HighlightManager.GetKeywords()
            };
            return provider;
        }

        [SettingsProvider]
        public static SettingsProvider GetInspectorSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider(INSPECTOR_SETTINGS_PATH, SettingsScope.Project)
            {
                label = "Inspector",
                guiHandler = Prefs.InspectorManager.DrawWithToolbar,
                keywords = Prefs.InspectorManager.GetKeywords()
            };
            return provider;
        }

        [SettingsProvider]
        public static SettingsProvider GetNavigationSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider(NAVIGATION_SETTINGS_PATH, SettingsScope.Project)
            {
                label = "Navigation",
                guiHandler = Prefs.NavigationManager.DrawWithToolbar,
                keywords = Prefs.NavigationManager.GetKeywords()
            };
            return provider;
        }

        [SettingsProvider]
        public static SettingsProvider GetQuickAccessSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider(QUICK_ACCESS_SETTINGS_PATH, SettingsScope.Project)
            {
                label = "Quick Access Bar",
                guiHandler = Prefs.QuickAccessBarManager.DrawWithToolbar,
                keywords = Prefs.QuickAccessBarManager.GetKeywords()
            };
            return provider;
        }

        [SettingsProvider]
        public static SettingsProvider GetSearchSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider(SEARCH_WINDOWS_SETTINGS_PATH, SettingsScope.Project)
            {
                label = "Search",
                guiHandler = Prefs.SearchManager.DrawWithToolbar,
                keywords = Prefs.SearchManager.GetKeywords()
            };
            return provider;
        }

        [SettingsProvider]
        public static SettingsProvider GetSceneViewSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider(SCENEVIEW_SETTINGS_PATH, SettingsScope.Project)
            {
                label = "Scene View",
                guiHandler = Prefs.SceneViewManager.DrawWithToolbar,
                keywords = Prefs.SceneViewManager.GetKeywords()
            };
            return provider;
        }

        [SettingsProvider]
        public static SettingsProvider GetSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider(UEE_SETTINGS_PATH, SettingsScope.Project)
            {
                label = "Ultimate Editor Enhancer",
                guiHandler = Prefs.OnGUI,
                keywords = Prefs.GetKeywords()
            };
            return provider;
        }

        [SettingsProvider]
        public static SettingsProvider GetToolbarSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider(TOOLBAR_SETTINGS_PATH, SettingsScope.Project)
            {
                label = "Toolbar",
                guiHandler = Prefs.ToolbarManager.DrawWithToolbar,
                keywords = Prefs.ToolbarManager.GetKeywords()
            };
            return provider;
        }

        [SettingsProvider]
        public static SettingsProvider GetViewsSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider(VIEWS_SETTINGS_PATH, SettingsScope.Project)
            {
                label = "Views",
                guiHandler = Prefs.ViewGalleryManager.DrawWithToolbar,
                keywords = Prefs.ViewGalleryManager.GetKeywords()
            };
            return provider;
        }

        [SettingsProvider]
        public static SettingsProvider GetWailaSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider(WAILA_SETTINGS_PATH, SettingsScope.Project)
            {
                label = "WAILA",
                guiHandler = Prefs.WailaManager.DrawWithToolbar,
                keywords = Prefs.WailaManager.GetKeywords()
            };
            return provider;
        }

        [SettingsProvider]
        public static SettingsProvider GetUnsafeSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider(UNSAFE_SETTINGS_PATH, SettingsScope.Project)
            {
                label = "Unsafe",
                guiHandler = Prefs.UnsafeManager.DrawWithToolbar,
                keywords = Prefs.UnsafeManager.GetKeywords()
            };
            return provider;
        }

        public static void OpenFavoriteWindowsSettings()
        {
            SettingsService.OpenProjectSettings(FAVORITE_WINDOWS_SETTINGS_PATH);
        }

        public static void OpenQuickAccessSettings()
        {
            SettingsService.OpenProjectSettings(QUICK_ACCESS_SETTINGS_PATH);
        }

        [MenuItem(WindowsHelper.MenuPath + "Settings", false, 122)]
        public static void OpenSettings()
        {
            SettingsService.OpenProjectSettings(UEE_SETTINGS_PATH);
        }

        public static void OpenToolbarSettings()
        {
            SettingsService.OpenProjectSettings(TOOLBAR_SETTINGS_PATH);
        }
    }
}