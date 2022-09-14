/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Linq;
using InfinityCode.UltimateEditorEnhancer.UnityTypes;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.Tools
{
    [EditorTool("Pivot Tool")]
    public class PivotTool : EditorTool
    {
        private static GUIContent activeContent;
        private static Vector3 firstPoint;
        private static Texture handleIcon;
        private static Mode mode = Mode.move;
        private static PRS[] oldValues;

        private static GUIContent passiveContent;
        private static int pointIndex;

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
                    if (activeContent == null) activeContent = new GUIContent(Icons.pivotToolActive, "Pivot Tool");
                    return activeContent;
                }

                if (passiveContent == null) passiveContent = new GUIContent(Icons.pivotTool, "Pivot Tool");
                return passiveContent;
            }
        }

#if UNITY_2020_3_OR_NEWER
        public override void OnActivated()
#else
        private void OnEnable()
#endif
        {
            mode = 0;
            pointIndex = 0;
        }

        private void ChangePivot(Vector3 position, Quaternion rotation)
        {
            int childCount = Selection.gameObjects.Max(t => t.transform.childCount);
            if (oldValues == null)
            {
                oldValues = new PRS[Mathf.Max(8, Mathf.NextPowerOfTwo(childCount))];
                for (int i = 0; i < oldValues.Length; i++)
                {
                    oldValues[i] = new PRS();
                }
            }
            else if (oldValues.Length <= childCount)
            {
                int oldCount = oldValues.Length;
                oldValues = new PRS[Mathf.NextPowerOfTwo(childCount)];
                for (int i = oldCount; i < oldValues.Length; i++)
                {
                    oldValues[i] = new PRS();
                }
            }

            Undo.SetCurrentGroupName("Change Pivot");
            int group = Undo.GetCurrentGroup();

            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                GameObject go = Selection.gameObjects[i];
                Transform t = go.transform;

                for (int j = 0; j < t.childCount; j++)
                {
                    oldValues[j].Save(t.GetChild(j));
                }

                Undo.RecordObject(t, "Change Pivot");

                t.position = position;
                t.rotation = rotation;

                for (int j = 0; j < t.childCount; j++)
                {
                    Transform child = t.GetChild(j);
                    Undo.RecordObject(child, "Change Pivot");
                    oldValues[j].Restore(child);
                }
            }

            Undo.CollapseUndoOperations(@group);
        }

        public override void OnToolGUI(EditorWindow window)
        {
            if (Selection.gameObjects.Length == 0) return;
            if (handleIcon == null) handleIcon = EditorIconContents.avatarPivot.image;

            Vector3 position = UnityEditor.Tools.handlePosition;
            Quaternion rotation = Selection.activeGameObject.transform.rotation;

            Event e = Event.current;
            if (e.modifiers == EventModifiers.Alt)
            {
                Color clr = Handles.color;

                Handles.color = Color.blue;
                Handles.DrawLine(position - rotation * Vector3.forward * 1000, position + rotation * Vector3.forward * 1000);

                Handles.color = Color.red;
                Handles.DrawLine(position - rotation * Vector3.left * 1000, position + rotation * Vector3.left * 1000);

                Handles.color = Color.green;
                Handles.DrawLine(position - rotation * Vector3.up * 1000, position + rotation * Vector3.up * 1000);

                Handles.color = clr;
            }

            if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.V)
                {
                    mode = Mode.setPivot;
                    e.Use();
                }
                else if (e.keyCode == KeyCode.LeftShift && mode != Mode.setOrientation)
                {
                    mode = Mode.setOrientation;
                    pointIndex = 0;
                    e.Use();
                }
            }
            else if (e.type == EventType.KeyUp)
            {
                if (e.keyCode == KeyCode.V || e.keyCode == KeyCode.LeftShift)
                {
                    mode = Mode.move;
                    e.Use();
                }
            }

            bool changed = false;
            float handleSize;
            EventType eventType = e.type;

            EditorGUI.BeginChangeCheck();
            if (mode == Mode.move)
            {
                position = Handles.PositionHandle(position, rotation);
                rotation = Handles.RotationHandle(rotation, position);
                handleSize = HandleUtility.GetHandleSize(position);
                Handles.Label(position + new Vector3(1, -1, 0) * handleSize * 0.125f, handleIcon);
            }
            else if (mode == Mode.setPivot)
            {
                handleSize = HandleUtility.GetHandleSize(position);
                HandleUtilityRef.FindNearestVertex(e.mousePosition, out position);
                Handles.RectangleHandleCap(-1, position, (window as SceneView).camera.transform.rotation, handleSize * 0.125f, eventType);
                Handles.Label(position + new Vector3(1, -1, 0) * handleSize * 0.125f, handleIcon);
                if (eventType == EventType.MouseDown && e.button == 0)
                {
                    changed = true;
                    mode = Mode.move;
                    e.Use();
                }
            }
            else if (mode == Mode.setOrientation)
            {
                if (pointIndex == 1)
                {
                    handleSize = HandleUtility.GetHandleSize(firstPoint);
                    Handles.RectangleHandleCap(-1, firstPoint, (window as SceneView).camera.transform.rotation, handleSize * 0.125f, eventType);
                }

                handleSize = HandleUtility.GetHandleSize(position);
                HandleUtilityRef.FindNearestVertex(e.mousePosition, out position);
                Handles.RectangleHandleCap(-1, position, (window as SceneView).camera.transform.rotation, handleSize * 0.125f, eventType);
                Handles.Label(position + new Vector3(1, -1, 0) * handleSize, handleIcon);

                if (pointIndex == 1)
                {
                    Color color = Handles.color;
                    Handles.color = Color.green;
                    Handles.DrawLine(firstPoint, position);
                    Handles.color = color;
                }

                if (eventType == EventType.MouseDown && e.button == 0)
                {
                    if (pointIndex == 0)
                    {
                        firstPoint = position;
                        pointIndex = 1;
                        e.Use();
                    }
                    else
                    {
                        pointIndex = 0;
                        mode = Mode.move;
                        e.Use();

                        GenericMenuEx menu = GenericMenuEx.Start();
                        menu.Add("X", () => SetOrientation(firstPoint, position, Vector3.right));
                        menu.Add("Y", () => SetOrientation(firstPoint, position, Vector3.up));
                        menu.Add("Z", () => SetOrientation(firstPoint, position, Vector3.forward));
                        menu.Show();
                    }
                }
            }

            if (EditorGUI.EndChangeCheck() || changed) ChangePivot(position, rotation);
        }

        private void SetOrientation(Vector3 v1, Vector3 v2, Vector3 axis)
        {
            Quaternion rotation = Quaternion.FromToRotation(axis, v2 - v1);
            ChangePivot(UnityEditor.Tools.handlePosition, rotation);
        }

        private class PRS
        {
            public Vector3 position;
            public Quaternion rotation;

            public void Save(Transform transform)
            {
                position = transform.position;
                rotation = transform.rotation;
            }

            public void Restore(Transform transform)
            {
                transform.position = position;
                transform.rotation = rotation;
            }
        }

        public enum Mode
        {
            move,
            setPivot,
            setOrientation
        }
    }
}