/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using InfinityCode.UltimateEditorEnhancer.PropertyDrawers;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InfinityCode.UltimateEditorEnhancer.Behaviors
{
    [InitializeOnLoad]
    public static class DragAndDropToEventField
    {
        static DragAndDropToEventField()
        {
            UnityEventBaseDrawer.OnGUIAfter += OnGUIAfter;
        }

        private static void OnDragPerformed(SerializedProperty property)
        {
            if (DragAndDrop.objectReferences.Length == 0) return;
            foreach (Object obj in DragAndDrop.objectReferences)
            {
                if (!(obj is GameObject || obj is Component)) return;
            }

            DragAndDrop.AcceptDrag();

            foreach (Object obj in DragAndDrop.objectReferences)
            {
                SerializedProperty callsProp = property.FindPropertyRelative("m_PersistentCalls.m_Calls");
                callsProp.arraySize++;

                GameObject target;
                if (obj is GameObject) target = obj as GameObject;
                else if (obj is Component) target = (obj as Component).gameObject;
                else continue;

                SerializedProperty last = callsProp.GetArrayElementAtIndex(callsProp.arraySize - 1);
                last.FindPropertyRelative("m_Target").objectReferenceValue = target;
                last.FindPropertyRelative("m_CallState").enumValueIndex = 2;
            }

            property.serializedObject.SetIsDifferentCacheDirty();
            Event.current.Use();
        }

        private static void OnDragUpdated()
        {
            if (DragAndDrop.objectReferences.Length == 0) return;
            foreach (Object obj in DragAndDrop.objectReferences)
            {
                if (!(obj is GameObject || obj is Component)) return;
            }

            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            Event.current.Use();
        }

        private static void OnGUIAfter(Rect position, SerializedProperty property, GUIContent label)
        {
            Event e = Event.current;
            if (position.Contains(e.mousePosition))
            {
                if (e.type == EventType.DragUpdated) OnDragUpdated();
                else if (e.type == EventType.DragPerform) OnDragPerformed(property);
            }
        }
    }
}