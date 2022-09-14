/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinityCode.UltimateEditorEnhancer.Windows;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.EditorMenus.Layouts
{
    public class BreadcrumbsLayout : MainLayoutItem
    {
        private bool _isActive = true;
        private GUIContent labelContent;
        private GUIContent upButtonContent;
        private GUIContent rightButtonContent;
        private GUIContent downButtonContent;
        private Vector2 labelSize;
        private bool hasParents;
        private bool hasChilds;
        private bool hasNeighbors;
        private bool isMultiple;
        private string newName;
        private GameObject[] targets;

        public override bool isActive
        {
            get { return _isActive; }
        }

        private void CalculateRect(ref Vector2 position, ref Vector2 offset, ref bool flipHorizontal, ref bool flipVertical)
        {
            GUIStyle style = EditorStyles.whiteLabel;
            Vector2 size = labelSize = style.CalcSize(labelContent) + new Vector2(style.margin.horizontal, style.margin.vertical);

            int countButtons = 0;
            if (hasParents) countButtons++;
            if (hasNeighbors) countButtons++;
            if (hasChilds) countButtons++;

            Vector2 buttonSize = Styles.transparentButton.CalcSize(upButtonContent);
            size.x += (buttonSize.x + Styles.transparentButton.margin.right) * countButtons;

            Vector2 toggleSize = GUI.skin.toggle.CalcSize(GUIContent.none);
            size.x += toggleSize.x;

            rect = new Rect(position, size);

            if (position.y < 60) offset.y = 60;
        }

        private bool CheckPrefs()
        {
            return true;
        }

        private void DrawChildrenButton()
        {
            if (!hasChilds) return;
            if (!GUILayout.Button(downButtonContent, Styles.transparentButton, GUILayout.Width(12))) return;

            GameObject g = Selection.activeGameObject;
            Transform t = g.transform;

            GenericMenuEx menu = GenericMenuEx.Start();
            GetChildren(t, menu, "");

            menu.Show();
        }

        private void DrawNeighborsButton()
        {
            if (!hasNeighbors) return;
            if (!GUILayout.Button(rightButtonContent, Styles.transparentButton, GUILayout.Width(12))) return;

            List<GameObject> items = new List<GameObject>();
            GameObject g = Selection.activeGameObject;
            Transform parent = g.transform.parent;
            if (parent != null)
            {
                for (int i = 0; i < parent.childCount; i++) items.Add(parent.GetChild(i).gameObject);
            }
            else
            {
                items = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().ToList();
            }

            GenericMenuEx menu = GenericMenuEx.Start();

            foreach (GameObject item in items)
            {
                GameObject go = item;
                menu.Add(go.name, Selection.activeGameObject == go, () =>
                {
                    Selection.activeGameObject = go;
                    SceneViewManager.OnNextGUI += EditorMenu.ShowInLastPosition;
                });
            }
            menu.Show();
        }

        private void DrawParentsButton()
        {
            if (!hasParents) return;
            if (!GUILayout.Button(upButtonContent, Styles.transparentButton, GUILayout.Width(12))) return;

            if (Prefs.breadcrumbsParentUp && Event.current.modifiers == Prefs.breadcrumbsParentUpModifiers)
            {
                Selection.activeGameObject = Selection.activeGameObject.transform.parent.gameObject;
                SceneViewManager.OnNextGUI += EditorMenu.ShowInLastPosition;
                return;
            }

            if (!Prefs.breadcrumbsParentShowAll || Event.current.modifiers != Prefs.breadcrumbsParentShowAllModifiers) return;

            List<GameObject> items = new List<GameObject>();
            GameObject g = Selection.activeGameObject;
            while (g.transform.parent != null)
            {
                g = g.transform.parent.gameObject;
                items.Insert(0, g);
            }

            GenericMenuEx menu = GenericMenuEx.Start();

            foreach (GameObject item in items)
            {
                GameObject go = item;
                menu.Add(go.name, false, () =>
                {
                    Selection.activeGameObject = go;
                    SceneViewManager.OnNextGUI += EditorMenu.ShowInLastPosition;
                });
            }
            menu.Show();
        }

        private void GetChildren(Transform t, GenericMenuEx menu, string prefix)
        {
            for (int i = 0; i < t.childCount; i++)
            {
                Transform ct = t.GetChild(i);
                GameObject go = ct.gameObject;
                string title = prefix + go.name;
                if (ct.childCount > 0)
                {
                    menu.Add(title + "/" + "Select", false, () =>
                    {
                        Selection.activeGameObject = go;
                        SceneViewManager.OnNextGUI += EditorMenu.ShowInLastPosition;
                    });
                    menu.AddSeparator(title + "/");
                    GetChildren(ct, menu, title + "/");
                }
                else
                {
                    menu.Add(title, () =>
                    {
                        Selection.activeGameObject = go;
                        SceneViewManager.OnNextGUI += EditorMenu.ShowInLastPosition;
                    });
                }
            }
        }

        public override void OnGUI()
        {
            Event e = Event.current;

            EditorGUILayout.BeginHorizontal();

            DrawParentsButton();
            EditorGUI.BeginChangeCheck();

            bool enabled = targets.All(t => t.activeSelf); 
            enabled = EditorGUILayout.Toggle(enabled);
            if (EditorGUI.EndChangeCheck())
            {
                foreach (GameObject target in targets)
                {
                    target.SetActive(enabled);
                    EditorUtility.SetDirty(target);
                }
            }
            EditorGUILayout.LabelField(labelContent, EditorStyles.whiteLabel, GUILayout.Width(labelSize.x));
            if (GUILayoutUtility.GetLastRect().Contains(e.mousePosition))
            {
                if (e.type == EventType.MouseDrag)
                {
                    DragAndDrop.PrepareStartDrag();
                    DragAndDrop.objectReferences = targets;
                    DragAndDrop.StartDrag("Drag GameObjects");
                    e.Use();
                    GUI.changed = true;
                }
                else if (e.type == EventType.MouseDown)
                {
                    if (e.button == 0 && !isMultiple)
                    {
                        EditorGUIUtility.PingObject(targets[0]);
                    }
                    else if (e.button == 1)
                    {
                        Rect lastRect = GUILayoutUtility.GetLastRect();
                        if (lastRect.Contains(e.mousePosition)) GameObjectUtils.ShowContextMenu(true, targets);
                    }
                }
            }

            DrawNeighborsButton();
            DrawChildrenButton();

            EditorGUILayout.EndHorizontal();
        }

        public override void Prepare(GameObject[] targets, Vector2 position, ref Vector2 offset, ref bool flipHorizontal, ref bool flipVertical)
        {
            _isActive = false;
            if (wnd != null) wnd.Close();

            if (!CheckPrefs()) return;
            this.targets = targets;
            if (targets == null || targets.Length == 0) return;

            _isActive = true;

            isMultiple = targets.Length > 1;

            hasParents = Prefs.breadcrumbsParent && !isMultiple && targets[0].transform.parent != null;
            hasChilds = Prefs.breadcrumbsChilds && !isMultiple && targets[0].transform.childCount > 0;
            hasNeighbors = false;
            if (Prefs.breadcrumbsNeighbors && !isMultiple)
            {
                if (hasParents)
                {
                    if (targets[0].transform.parent.childCount > 1) hasNeighbors = true;
                }
                else
                {
                    GameObject[] rootGameObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
                    hasNeighbors = rootGameObjects.Length > 1;
                }
            }

            if (!isMultiple) labelContent = new GUIContent(targets[0].name, GameObjectUtils.GetTransformPath(targets[0].transform).ToString());
            else
            {
                StringBuilder tooltipBuilder = new StringBuilder();
                for (int i = 0; i < Mathf.Min(targets.Length, 10); i++)
                {
                    GameObject go = targets[i];
                    if (i > 0) tooltipBuilder.Append("\n");
                    tooltipBuilder.Append(GameObjectUtils.GetTransformPath(go.transform));
                }

                labelContent = new GUIContent("Multiple (" + targets.Length + ")", tooltipBuilder.ToString());
            }

            upButtonContent = new GUIContent(Icons.arrowUp, "Parents");
            rightButtonContent = new GUIContent(Icons.arrowRight, "Neighbors");
            downButtonContent = new GUIContent(Icons.arrowDown, "Childs");

            CalculateRect(ref position, ref offset, ref flipHorizontal, ref flipVertical);
        }

        public override void SetPosition(Vector2 position, Vector2 offset, bool flipHorizontal, bool flipVertical)
        {
            int ox = 0;
            int oy = -20;
            rect.position = position - new Vector2(rect.width / 2, rect.height) + offset + new Vector2(ox, oy);
            if (rect.x < 1) rect.x = 1;
#if !UNITY_EDITOR_OSX
            if (rect.xMax > Screen.currentResolution.width - 1) rect.x = Screen.currentResolution.width - 1 - rect.width;
#endif

            if (flipVertical) rect.y += rect.height - oy * 2;
        }

        public override void Show()
        {
            wnd = LayoutWindow.Show(this, rect);
        }
    }
}