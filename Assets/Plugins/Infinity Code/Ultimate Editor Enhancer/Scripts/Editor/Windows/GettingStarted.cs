/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using InfinityCode.UltimateEditorEnhancer.JSON;
using InfinityCode.UltimateEditorEnhancer.UnityTypes;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.Windows
{
    public class GettingStarted : EditorWindow
    {
        private static string folder;
        private static Slide[] slides;
        private static List<Slide> flatSlides;
        private int totalSlides;
        private static Slide activeSlide;
        private static Slide[] activeSlides;
        private static Slide first;
        private static Slide last;
        private static GUIContent[] contents;
        private static GSTreeView treeView;
        private static TreeViewState treeViewState;
        private static bool needReload = true;
        private static string filterText;
        private static bool resetSelection;

        private void DrawActiveSlide()
        {
            Event e = Event.current;

            Rect buttonsRect = new Rect(position.width - 35, 5, 30, 20);

            if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.Space || e.keyCode == KeyCode.RightArrow)
                {
                    SetSlide(activeSlide.next);
                }
                else if (e.keyCode == KeyCode.Backspace || e.keyCode == KeyCode.LeftArrow)
                {
                    SetSlide(activeSlide.prev);
                }

                UpdateTitle();
                Repaint();
            }
            else if (e.type == EventType.MouseUp && !buttonsRect.Contains(e.mousePosition))
            {
                if (e.button == 0)
                {
                    SetSlide(activeSlide.next);
                }
                else if (e.button == 1)
                {
                    SetSlide(activeSlide.prev);
                }

                UpdateTitle();

                Repaint();
            }

            if (activeSlide.texture != null) GUI.DrawTexture(new Rect(302, 2, position.width - 304, position.height - 4), activeSlide.texture);
            if (contents == null) contents = new[] { new GUIContent("?", "Open Documentation") };
            int ti = GUI.Toolbar(buttonsRect, -1, contents);
            if (ti != -1) Links.OpenDocumentation(activeSlide.help);
        }

        private static bool DrawFilterTextField()
        {
            GUILayout.BeginArea(new Rect(2, 2, 300, 16));
            GUI.SetNextControlName("UEEGettingStartedSearchTextField");
            EditorGUI.BeginChangeCheck();
            filterText = GUILayoutUtils.ToolbarSearchField(filterText);
            bool changed = EditorGUI.EndChangeCheck();

            if (resetSelection && Event.current.type == EventType.Repaint)
            {
                GUI.FocusControl("UEEGettingStartedSearchTextField");
                resetSelection = false;
            }

            GUILayout.EndArea();

            return changed;
        }

        private void DrawTableOfContent()
        {
            bool filterChanged = DrawFilterTextField();

            if (filterChanged || activeSlides == null)
            {
                if (string.IsNullOrEmpty(filterText)) activeSlides = slides.ToArray();
                else
                {
                    string pattern = SearchableItem.GetPattern(filterText);
                    activeSlides = flatSlides.Where(p =>
                    {
                        Slide parent = p.parent;
                        while (parent != null)
                        {
                            if (parent.accuracy > 0)
                            {
                                p.accuracy = 0;
                                return false;
                            }

                            parent = parent.parent;
                        }
                        return p.UpdateAccuracy(pattern) > 0;
                    }).OrderByDescending(p => p.accuracy).ToArray();
                }

                needReload = true;
            }

            if (treeView == null)
            {
                treeViewState = new TreeViewState();
                treeView = new GSTreeView(treeViewState);
                treeView.ExpandAll();
                needReload = false;
            }

            if (needReload) treeView.Reload();

            Rect rect = new Rect(0, 18, 300, position.height - 18);
            treeView.OnGUI(rect);
        }

        private void InitSlides(Slide[] slides, Slide parent, ref int index, ref Slide prev)
        {
            for (int i = 0; i < slides.Length; i++)
            {
                Slide slide = slides[i];
                slide.parent = parent;
                if (!string.IsNullOrEmpty(slide.image))
                {
                    slide.prev = prev;
                    slide.index = ++index;
                    if (prev == null) activeSlide = slide;
                    else prev.next = slide;
                    prev = slide;
                }

                slide.id = flatSlides.Count;
                flatSlides.Add(slide);

                if (slide.slides != null) InitSlides(slide.slides, slide, ref index, ref prev);
            }
        }

        private void OnDisable()
        {
            if (slides != null)
            {
                foreach (Slide slide in slides) slide.Dispose();
                slides = null;
            }

            activeSlide = null;
            activeSlides = null;
            first = null;
            flatSlides = null;
            last = null;
            slides = null;
            treeView = null;
            treeViewState = null;
        }

        private void OnEnable()
        {
            folder = Resources.assetFolder + "Textures/Getting Started/";
            string content = File.ReadAllText(folder + "_Content.json", Encoding.UTF8);

            slides = Json.Deserialize<Slide[]>(content);

            Slide prev = null;
            totalSlides = 0;
            flatSlides = new List<Slide>();
            InitSlides(slides, null, ref totalSlides, ref prev);
            
            last = prev;
            first = activeSlide;

            first.prev = last;
            last.next = first;

            minSize = new Vector2(904, 454);
            maxSize = new Vector2(904, 454);

            UpdateTitle();
            SetSlide(activeSlide);
        }

        public void OnGUI()
        {
            DrawTableOfContent();
            DrawActiveSlide();
        }

        [MenuItem(WindowsHelper.MenuPath + "Getting Started", false, 121)]
        public static void OpenWindow()
        {
            GettingStarted wnd = GetWindow<GettingStarted>(true, "Getting Started", true);
            SetSlide(activeSlide); 
            wnd.UpdateTitle();
        }

        private static void SetSlide(Slide slide)
        {
            if (string.IsNullOrEmpty(slide.image))
            {
                if (slide.slides == null) return;
                
                bool success = false;
                for (int i = 0; i < slide.slides.Length; i++)
                {
                    Slide s = slide.slides[i];
                    if (string.IsNullOrEmpty(s.image)) continue;

                    slide = s;
                    success = true;
                    break;
                }

                if (!success) return;
            }
            activeSlide = slide;
            if (treeView != null)
            {
                treeView.SetSelection(new List<int>{ slide.id });
                treeView.FrameItem(slide.id);
            }
        }

        private void UpdateTitle()
        {
            titleContent = new GUIContent("Getting Started. Frame " + activeSlide.index + " / " + totalSlides + " (click to continue)");
        }

        public class Slide : SearchableItem
        {
            public string title;
            public string image;
            public string help;
            public Slide[] slides;
            public int index;
            public float added;
            public float updated;

            public Slide next;
            public Slide parent;
            public Slide prev;
            private Texture2D _texture;
            public int id;

            public Texture2D texture
            {
                get
                {
                    if (_texture == null)
                    {
                        _texture = AssetDatabase.LoadAssetAtPath<Texture2D>(folder + image);
                    }

                    return _texture;
                }
            }

            public void Dispose()
            {
                if (slides != null)
                {
                    foreach (Slide slide in slides)
                    {
                        slide.Dispose();
                    }
                }

                slides = null;
                next = null;
                prev = null;
                _texture = null;
            }

            protected override int GetSearchCount()
            {
                return 1;
            }

            protected override string GetSearchString(int index)
            {
                return title;
            }
        }

        internal class GSTreeView : TreeView
        {
            private List<TreeViewItem> allItems;

            public GSTreeView(TreeViewState state) : base(state)
            {
                useScrollView = true;
                showBorder = true;
                showAlternatingRowBackgrounds = true;
                Reload();
            }

            protected override TreeViewItem BuildRoot()
            {
                var root = new TreeViewItem { id = -1, depth = -1, displayName = "Root" };
                allItems = new List<TreeViewItem>();

                BuildTree(activeSlides, 0);

                SetupParentsAndChildrenFromDepths(root, allItems);

                return root;
            }

            private void BuildTree(Slide[] slides, int depth)
            {
                for (int i = 0; i < slides.Length; i++)
                {
                    Slide slide = slides[i];
                    GSTreeViewItem item = new GSTreeViewItem
                    {
                        id = slide.id, 
                        depth = depth, 
                        displayName = slide.title,
                        slide = slide
                    };
                    allItems.Add(item);
                    if (slide.slides != null && slide.slides.Length > 0) BuildTree(slide.slides, depth + 1);
                }
            }

            protected override void SingleClickedItem(int id)
            {
                TreeViewItem item = treeView.FindItem(id, treeView.rootItem);
                SetSlide((item as GSTreeViewItem).slide);
            }
        }

        public class GSTreeViewItem : TreeViewItem
        {
            public Slide slide;
        }

        internal class GSItem : SearchableItem
        {
            public string name;
            public GameObject target;

            public GSItem(GameObject target)
            {
                this.target = target;
                name = target.name;
            }

            public void Dispose()
            {
                target = null;
            }

            protected override int GetSearchCount()
            {
                return 1;
            }

            protected override string GetSearchString(int index)
            {
                return name;
            }
        }
    }
}