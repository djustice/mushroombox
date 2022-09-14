/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Collections.Generic;
using System.Linq;
using InfinityCode.UltimateEditorEnhancer.SceneTools;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.Tools
{
    [InitializeOnLoad]
    [EditorTool("Duplicate Tool")]
    public class DuplicateTool : EditorTool
    {
        public static int phase = 0;
        private static Vector3 position;

        private static GUIContent labelContent;
        private static GUIContent passiveContent;
        private static GUIContent activeContent;
        private static GUIStyle style;

        private static List<GameObject> tempItems;
        private Vector3 startPosition;
        private GameObject[] sourceGameObjects;
        private Bounds sourceBounds;
        private Vector3Int count = Vector3Int.zero;
        private Vector3 sourceSize;
        private static int lastPositionIDX;
        private Transform parent;

        public override GUIContent toolbarIcon
        {
            get
            {
#if UNITY_2020_2_OR_NEWER
                if (ToolManager.IsActiveTool(this))
#else
                if (EditorTools.IsActiveTool(this))
#endif
                {
                    if (activeContent == null) activeContent = new GUIContent(Icons.duplicateToolActive, "Duplicate Tool");
                    return activeContent;
                }

                if (passiveContent == null) passiveContent = new GUIContent(Icons.duplicateTool, "Duplicate Tool");
                return passiveContent;
            }
        }

        static DuplicateTool()
        {
            SceneViewManager.AddListener(OnSceneGUI, SceneViewOrder.late, true);
            tempItems = new List<GameObject>();
        }

        private void Finish()
        {
            if (tempItems.Count > 0)
            {
                Undo.SetCurrentGroupName("Duplicate GameObjects");
                int group = Undo.GetCurrentGroup();

                List<GameObject> gameObjects = Selection.gameObjects.ToList();

                foreach (GameObject item in tempItems)
                {
                    Transform t = item.transform;
                    while (t.childCount > 0)
                    {
                        Transform child = t.GetChild(0);
                        Vector3 pos = child.position;
                        child.SetParent(parent != null? parent: t.parent, false);
                        child.position = pos;
                        child.hideFlags = HideFlags.None;
                        gameObjects.Add(child.gameObject);
                        Undo.RegisterCreatedObjectUndo(child.gameObject, "Duplicate GameObject");
                    }

                    DestroyImmediate(item);
                }

                tempItems.Clear();

                Selection.objects = gameObjects.ToArray();
                Undo.CollapseUndoOperations(group);
            }

            Reset();
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            if (labelContent == null) return;

            Handles.BeginGUI();

            if (style == null)
            {
                style = new GUIStyle(ToolValues.StyleID)
                {
                    fontSize = 10,
                    alignment = TextAnchor.MiddleLeft,
                    wordWrap = false,
                    fixedHeight = 0,
                    border = new RectOffset(8, 8, 8, 8)
                };
            }

            Vector3 screenPoint = sceneView.camera.WorldToScreenPoint(position);
            Vector2 size = style.CalcSize(labelContent);
            if (screenPoint.y > size.y + 150) screenPoint.y -= size.y + 50;
            else screenPoint.y += size.y + 150;

            Rect rect = new Rect(screenPoint.x - size.x / 2, Screen.height - screenPoint.y - size.y / 2, size.x, size.y);

            GUI.Label(rect, labelContent, style);
            Handles.EndGUI();
        }

        public override void OnToolGUI(EditorWindow window)
        {
            if (!SelectionBoundsManager.hasBounds)
            {
                Reset();
                return;
            }

            if (phase == 0)
            {
                EditorGUI.BeginChangeCheck();

                position = UnityEditor.Tools.handlePosition;
                PositionHandle.Ids ids = PositionHandle.Ids.@default;
                Vector3 newPosition = PositionHandle.DoPositionHandle(ids, position, Quaternion.identity);

                if (EditorGUI.EndChangeCheck())
                {
                    Waila.Close();
                    Vector3 delta = newPosition - position;
                    if (Math.Abs(delta.sqrMagnitude) < float.Epsilon) return;

                    sourceGameObjects = Selection.gameObjects;
                    Transform firstTransform = sourceGameObjects[0].transform;
                    sourceBounds = new Bounds(firstTransform.position, Vector3.zero);
                    sourceSize = SelectionBoundsManager.bounds.size;

                    parent = firstTransform.parent;

                    for (int i = 1; i < sourceGameObjects.Length; i++)
                    {
                        Transform transform = sourceGameObjects[i].transform;
                        sourceBounds.Encapsulate(transform.position);
                        if (transform.parent != parent) parent = null;
                    }

                    startPosition = newPosition;
                    phase = 1;
                }

                position = newPosition;
            }
            else if (phase == 1)
            {
                EditorGUI.BeginChangeCheck();

                PositionHandle.Ids ids = PositionHandle.Ids.@default;
                if (lastPositionIDX == 0)
                {
                    lastPositionIDX = ids.x;
                }
                else if (ids.x != lastPositionIDX)
                {
                    int delta = ids.x - lastPositionIDX;
                    lastPositionIDX = ids.x;
                    GUIUtility.hotControl += delta;
                }
                position = PositionHandle.DoPositionHandle(ids, position, Quaternion.identity);

                Vector3 center = (startPosition + position) / 2;
                Vector3 size = startPosition - position;

                if (EditorGUI.EndChangeCheck() && GUIUtility.hotControl != 0) UpdatePreviews(size);

                if (Event.current.rawType == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
                {
                    Reset();
                    return;
                }

                if (GUIUtility.hotControl == 0)
                {
                    Finish();
                }
                else
                {
                    Handles.color = Color.red;
                    Handles.DrawWireCube(center, size);
                }
            }

            Vector3 handlePos = position + UnityEditor.Tools.handleRotation * new Vector3(-.1f, -.1f, -.1f) * HandleUtility.GetHandleSize(position);
            Handles.Label(handlePos, Icons.duplicate);
        }

        private void Reset()
        {
            foreach (GameObject item in tempItems) DestroyImmediate(item);
            tempItems.Clear();
            sourceGameObjects = null;
            labelContent = null;
            phase = 0;
            lastPositionIDX = 0;
        }

        private void UpdatePreviews(Vector3 size)
        {
            sourceSize = SelectionBoundsManager.bounds.size;

            bool snapEnabled = SnapHelper.enabled;
            if (snapEnabled)
            {
                float snapValue = SnapHelper.value;

                if (sourceSize.x < snapValue) sourceSize.x = snapValue;
                else sourceSize.x = Mathf.RoundToInt(sourceSize.x / snapValue) * snapValue;

                if (sourceSize.y < snapValue) sourceSize.y = snapValue;
                else sourceSize.y = Mathf.RoundToInt(sourceSize.y / snapValue) * snapValue;

                if (sourceSize.z < snapValue) sourceSize.z = snapValue;
                else sourceSize.z = Mathf.RoundToInt(sourceSize.z / snapValue) * snapValue;
            }

            bool isRectTransform = SelectionBoundsManager.isRectTransform;

            count.x = Mathf.RoundToInt(Mathf.Abs(size.x / sourceSize.x));
            count.y = Mathf.RoundToInt(Mathf.Abs(size.y / sourceSize.y));
            count.z = !isRectTransform? Mathf.RoundToInt(Mathf.Abs(size.z / sourceSize.z)): 0;

            int countItems = (count.x + 1) * (count.y + 1) * (count.z + 1) - 1;
            if (tempItems.Count == countItems) return;

            StaticStringBuilder.Clear();
            StaticStringBuilder.Append(count.x + 1).Append("x").Append(count.y + 1).Append("x").Append(count.z + 1);
            if (labelContent == null) labelContent = new GUIContent(StaticStringBuilder.GetString(true));
            else labelContent.text = StaticStringBuilder.GetString(true);

            int i = 0;

            float dx = Mathf.Sign(-size.x);
            float dy = Mathf.Sign(-size.y);
            float dz = Mathf.Sign(-size.z);

            while (tempItems.Count > countItems)
            {
                int removeIndex = tempItems.Count - 1;
                GameObject item = tempItems[removeIndex];
                DestroyImmediate(item);
                tempItems.RemoveAt(removeIndex);
            }

            Vector3 sourcePos = sourceBounds.min;
            Vector3 axis = ((Vector3)count).normalized;

            for (int x = 0; x <= count.x; x++)
            {
                for (int y = 0; y <= count.y; y++)
                {
                    for (int z = 0; z <= count.z; z++)
                    {
                        if (x == 0 && y == 0 && z == 0) continue;

                        GameObject item;
                        if (i < tempItems.Count) item = tempItems[i];
                        else
                        {
                            item = new GameObject("temp");
                            if (Prefs.hideDuplicateToolTemp) item.hideFlags = HideFlags.HideAndDontSave;
                            if (isRectTransform)
                            {
                                item.transform.SetParent(SelectionBoundsManager.firstGameObject.transform.parent, false);
                                item.AddComponent<RectTransform>();
                            }
                            
                            item.transform.position = sourcePos;

                            foreach (GameObject so in sourceGameObjects)
                            {
                                GameObject dup;
                                if (PrefabUtility.IsPartOfAnyPrefab(so))
                                {
                                    string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(so);
                                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                                    dup = PrefabUtility.InstantiatePrefab(prefab, item.transform) as GameObject;
                                }
                                else
                                {
                                    dup = Instantiate(so, item.transform, false);
                                }
                                dup.name = so.name;
                                dup.transform.position = so.transform.position;
                                dup.transform.rotation = so.transform.rotation;
                                dup.transform.localScale = so.transform.localScale;
                            }
                            tempItems.Add(item);
                        }

                        item.transform.position = sourcePos + new Vector3(sourceSize.x * x * dx, sourceSize.y * y * dy, sourceSize.z * z * dz);
                        if (snapEnabled)
                        {
                            Vector3 pos = item.transform.position;
                            SnapHelper.Snap(item.transform);
                            Vector3 offset = item.transform.position - pos;
                            offset.Scale(axis);
                            item.transform.position = pos + offset;
                        }
                        
                        i++;
                    }
                }
            }
        }
    }
}