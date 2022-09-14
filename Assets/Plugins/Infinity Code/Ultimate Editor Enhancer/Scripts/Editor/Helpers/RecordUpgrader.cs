/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using System.Reflection;
using InfinityCode.UltimateEditorEnhancer.Attributes;
using InfinityCode.UltimateEditorEnhancer.HierarchyTools;
using InfinityCode.UltimateEditorEnhancer.SceneTools;
using InfinityCode.UltimateEditorEnhancer.SceneTools.QuickAccessActions;
using InfinityCode.UltimateEditorEnhancer.UnityTypes;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer
{
    [InitializeOnLoad]
    public static class RecordUpgrader
    {
        private const int CurrentUpgradeID = 3;
        private const string BookmarkItemSeparator = "|";

        static RecordUpgrader()
        {
            int upgradeID = LocalSettings.upgradeID;
            if (upgradeID < 1) InitDefaultQuickAccessItems();
            if (upgradeID < 2)
            {
                ReplaceSaveQuickAccessItem();
                TryInsertOpenAction();
            }

            if (upgradeID < 3)
            {
                TryAddHeaderRules();
            }

            ReferenceManager.Save();
            LocalSettings.upgradeID = CurrentUpgradeID;
        }

        private static void TryAddHeaderRules()
        {
            if (ReferenceManager.headerRules.Count != 0) return;

            ReferenceManager.headerRules.Add(new HeaderRule
            {
                condition = HeaderCondition.nameStarts,
                value = "--",
                trimChars = "-=",
                backgroundColor = Color.gray,
                textColor = Color.white,
                textAlign = TextAlignment.Center,
                textStyle = FontStyle.Bold
            });
        }

        public static void InitDefaultQuickAccessItems()
        {
            List<QuickAccessItem> items = ReferenceManager.quickAccessItems;
            if (items.Count > 0) return;

            QuickAccessItem open = new QuickAccessItem(QuickAccessItemType.action)
            {
                settings = new[] { typeof(OpenAction).FullName },
                tooltip = "Open Scene",
                expanded = false
            };

            QuickAccessItem save = new QuickAccessItem(QuickAccessItemType.action)
            {
                settings = new[] { typeof(SaveAction).FullName },
                tooltip = "Save Scenes",
                expanded = false
            };

            QuickAccessItem hierarchy = new QuickAccessItem(QuickAccessItemType.window)
            {
                settings = new[] { SceneHierarchyWindowRef.type.AssemblyQualifiedName },
                icon = QuickAccessItemIcon.texture,
                iconSettings = Resources.iconsFolder + "Hierarchy2.png",
                tooltip = "Hierarchy",
                visibleRules = SceneViewVisibleRules.onMaximized,
                expanded = false
            };

            QuickAccessItem project = new QuickAccessItem(QuickAccessItemType.window)
            {
                settings = new[] { ProjectBrowserRef.type.AssemblyQualifiedName },
                icon = QuickAccessItemIcon.texture,
                iconSettings = Resources.iconsFolder + "Project.png",
                tooltip = "Project",
                visibleRules = SceneViewVisibleRules.onMaximized,
                expanded = false
            };

            QuickAccessItem inspector = new QuickAccessItem(QuickAccessItemType.window)
            {
                settings = new[] { InspectorWindowRef.type.AssemblyQualifiedName },
                icon = QuickAccessItemIcon.texture,
                iconSettings = Resources.iconsFolder + "Inspector.png",
                tooltip = "Inspector",
                visibleRules = SceneViewVisibleRules.onMaximized,
                expanded = false
            };

            QuickAccessItem bookmarks = new QuickAccessItem(QuickAccessItemType.window)
            {
                settings = new[] { "InfinityCode.UltimateEditorEnhancer.Windows.Bookmarks, UltimateEditorEnhancer-Editor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" },
                icon = QuickAccessItemIcon.texture,
                iconSettings = Resources.iconsFolder + "Star-White.png",
                tooltip = "Bookmarks",
                expanded = false
            };

            QuickAccessItem viewGallery = new QuickAccessItem(QuickAccessItemType.window)
            {
                settings = new[] { "InfinityCode.UltimateEditorEnhancer.Windows.ViewGallery, UltimateEditorEnhancer-Editor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" },
                icon = QuickAccessItemIcon.editorIconContent,
                iconSettings = "d_ViewToolOrbit",
                tooltip = "View Gallery",
                expanded = false
            };

            QuickAccessItem distanceTool = new QuickAccessItem(QuickAccessItemType.window)
            {
                settings = new[] { "InfinityCode.UltimateEditorEnhancer.Windows.DistanceTool, UltimateEditorEnhancer-Editor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" },
                icon = QuickAccessItemIcon.texture,
                iconSettings = Resources.iconsFolder + "Rule.png",
                tooltip = "Distance Tool",
                expanded = false
            };

            QuickAccessItem quickAccessSettings = new QuickAccessItem(QuickAccessItemType.settings)
            {
                settings = new[] { "Project/Ultimate Editor Enhancer/Scene View/Quick Access Bar" },
                icon = QuickAccessItemIcon.editorIconContent,
                iconSettings = "d_Settings",
                tooltip = "Edit Items",
                expanded = false
            };

            QuickAccessItem info = new QuickAccessItem(QuickAccessItemType.window)
            {
                settings = new[] { "InfinityCode.UltimateEditorEnhancer.Windows.Welcome, UltimateEditorEnhancer-Editor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" },
                icon = QuickAccessItemIcon.editorIconContent,
                iconSettings = "_Help",
                tooltip = "Info",
                expanded = false
            };

            items.Add(open);
            items.Add(save);
            items.Add(hierarchy);
            items.Add(project);
            items.Add(inspector);
            items.Add(bookmarks);
            items.Add(viewGallery);
            items.Add(distanceTool);
            items.Add(new QuickAccessItem(QuickAccessItemType.flexibleSpace));
            items.Add(quickAccessSettings);
            items.Add(info);
        }

        private static void ReplaceSaveQuickAccessItem()
        {
            List<QuickAccessItem> items = ReferenceManager.quickAccessItems;
            if (items.Count == 0) return;

            foreach (QuickAccessItem item in items)
            {
                if (item.type == QuickAccessItemType.menuItem && item.settings[0] == "File/Save")
                {
                    item.type = QuickAccessItemType.action;
                    item.settings[0] = typeof(SaveAction).FullName;
                    TitleAttribute titleAttribute = typeof(SaveAction).GetCustomAttribute<TitleAttribute>();
                    item.tooltip = titleAttribute != null ? titleAttribute.displayName : "Save Scenes";
                    item.icon = QuickAccessItemIcon.editorIconContent;
                }
            }
        }

        private static void TryInsertOpenAction()
        {
            List<QuickAccessItem> items = ReferenceManager.quickAccessItems;
            if (items.Count < 1) return;

            QuickAccessItem item = items[0];
            if (item.type != QuickAccessItemType.action || item.settings[0] != typeof(SaveAction).FullName) return;

            QuickAccessItem open = new QuickAccessItem(QuickAccessItemType.action)
            {
                settings = new[] { typeof(OpenAction).FullName },
                tooltip = "Open Scene",
                expanded = false
            };

            items.Insert(0, open);
        }
    }
}