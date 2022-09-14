/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace InfinityCode.UltimateEditorEnhancer.InspectorTools
{
    public class EmptyInspector: InspectorInjector
    {
        private const string ELEMENT_NAME = "EmptyInspector";
        private const string SEARCHFIELD_NAME = "UEEEmptyInspectorSearchField";

        private static EmptyInspector instance;
        private static VisualElement visualElement;
        private static string filterText;

        public EmptyInspector()
        {
            EditorApplication.delayCall += InitInspector;
            WindowManager.OnMaximizedChanged += OnMaximizedChanged;
            Selection.selectionChanged += InitInspector;
        }

        private static void CreateButton(VisualElement parent, string submenu, string text)
        {
            ToolbarButton button = new ToolbarButton(() => EditorApplication.ExecuteMenuItem(submenu));
            button.text = text;
            button.style.unityTextAlign = TextAnchor.MiddleCenter;
            button.style.left = 0;
            button.style.borderLeftWidth = button.style.borderRightWidth = 0;
            parent.Add(button);
        }

        private VisualElement CreateContainer(VisualElement parent)
        {
            VisualElement el = new VisualElement();
            el.style.borderBottomWidth = el.style.borderTopWidth = el.style.borderLeftWidth = el.style.borderRightWidth = 1;
            el.style.borderBottomColor = el.style.borderTopColor = el.style.borderLeftColor = el.style.borderRightColor = Color.gray;
            el.style.marginLeft = 3;
            el.style.marginRight = 5;
            parent.Add(el);
            return el;
        }

        private static void CreateLabel(VisualElement parent, string text)
        {
            Label label = new Label(text);
            label.style.marginTop = 10;
            label.style.marginLeft = label.style.marginRight = 3;
            label.style.paddingLeft = 5;
            parent.Add(label);
        }

        private void DrawFilterTextField()
        {
            GUILayout.BeginHorizontal();
            GUI.SetNextControlName(SEARCHFIELD_NAME);
            EditorGUI.BeginChangeCheck();
            filterText = GUILayoutUtils.ToolbarSearchField(filterText);
            if (EditorGUI.EndChangeCheck()) UpdateFilteredItems();

            if (GUILayout.Button(TempContent.Get("?", "Help"), EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
            {
                Links.OpenDocumentation("empty-inspector");
            }

            GUILayout.EndHorizontal();
        }

        [InitializeOnLoadMethod]
        private static void Init()
        {
            instance = new EmptyInspector();
        }

        private VisualElement InitItems()
        {
            VisualElement visualElement = new VisualElement();
            visualElement.name = ELEMENT_NAME;

            Label helpbox = new Label("Nothing selected");
            helpbox.style.backgroundColor = Color.gray;
            helpbox.style.height = 30;
            helpbox.style.unityTextAlign = TextAnchor.MiddleCenter;

            visualElement.Add(helpbox);

            IMGUIContainer search = new IMGUIContainer(DrawFilterTextField);
            search.style.marginTop = 5;
            search.style.marginLeft = 5;
            search.style.marginRight = 5;
            visualElement.Add(search);

            InitUEEItems(visualElement);
            InitSettings(visualElement);
            InitWindows(visualElement);

            return visualElement;
        }

        private void InitSettings(VisualElement parent)
        {
            CreateLabel(parent, "Settings");
            VisualElement container = CreateContainer(parent);
            CreateButton(container, "Edit/Project Settings...", "Project Settings");
            CreateButton(container, "Edit/Preferences...", "Preferences");
            CreateButton(container, "Edit/Shortcuts...", "Shortcuts");
        }

        private void InitUEEItems(VisualElement parent)
        {
            CreateLabel(parent, "Ultimate Editor Enhancer");
            VisualElement container = CreateContainer(parent);
            CreateButton(container, WindowsHelper.MenuPath + "Bookmarks", "Bookmarks");
            CreateButton(container, WindowsHelper.MenuPath + "Distance Tool", "Distance Tool");
            CreateButton(container, WindowsHelper.MenuPath + "View Gallery", "View Gallery");
            CreateButton(container, WindowsHelper.MenuPath + "Documentation", "Documentation");
            CreateButton(container, WindowsHelper.MenuPath + "Settings", "Settings");
        }

        private void InitWindows(VisualElement parent)
        {
            CreateLabel(parent, "Packages");
            VisualElement container = CreateContainer(parent);

            CreateButton(container, "Assets/Import Package/Custom Package...", "Import Custom Package");

            bool skip = true;
            string groupName = "";

            foreach (string submenu in Unsupported.GetSubmenus("Window"))
            {
                string upper = Culture.textInfo.ToUpper(submenu);
                if (skip)
                {
                    if (upper == "WINDOW/PACKAGE MANAGER")
                    {
                        skip = false;
                    }
                    else continue;
                }

                string[] parts = submenu.Split('/');
                string firstPart = parts[1];

                if (parts.Length == 2)
                {
                    CreateButton(container, submenu, firstPart);
                }
                else if (parts.Length == 3)
                {
                    if (groupName != firstPart)
                    {
                        CreateLabel(parent, firstPart);
                        container = CreateContainer(parent);
                        groupName = firstPart;
                    }

                    CreateButton(container, submenu, parts[2]);
                }
            }
        }

        protected override bool OnInject(EditorWindow wnd, VisualElement mainContainer, VisualElement editorsList)
        {
            if (editorsList.parent[0].name == ELEMENT_NAME) editorsList.parent.RemoveAt(0);
            if (!Prefs.emptyInspector) return false;
            if (editorsList.childCount != 0 || float.IsNaN(editorsList.layout.width)) return false;

            if (visualElement == null) visualElement = InitItems();
            editorsList.parent.Insert(0, visualElement);
            filterText = "";
            UpdateFilteredItems();

            return true;
        }

        private void UpdateFilteredItems()
        {
            string t = filterText.Trim();
            if (string.IsNullOrEmpty(t))
            {
                for (int i = 2; i < visualElement.childCount; i += 2)
                {
                    visualElement[i].style.display = DisplayStyle.Flex;
                    VisualElement container = visualElement[i + 1];
                    container.style.display = DisplayStyle.Flex;
                    for (int j = 0; j < container.childCount; j++)
                    {
                        container[j].style.display = DisplayStyle.Flex;
                    }

                }
                return;
            }

            string pattern = SearchableItem.GetPattern(t);

            for (int i = 3; i < visualElement.childCount; i += 2)
            {
                VisualElement el = visualElement[i];

                bool hasVisible = false;
                VisualElement container = el as VisualElement;
                for (int j = 0; j < container.childCount; j++)
                {
                    ToolbarButton b = container[j] as ToolbarButton;
                    bool visible = SearchableItem.GetAccuracy(pattern, b.text) > 0;
                    if (visible)
                    {
                        b.style.display = DisplayStyle.Flex;
                        hasVisible = true;
                    }
                    else b.style.display = DisplayStyle.None;
                }

                if (hasVisible)
                {
                    visualElement[i - 1].style.display = DisplayStyle.Flex;
                    el.style.display = DisplayStyle.Flex;
                }

                else
                {
                    visualElement[i - 1].style.display = DisplayStyle.None;
                    el.style.display = DisplayStyle.None;
                }
            }
        }
    }
}