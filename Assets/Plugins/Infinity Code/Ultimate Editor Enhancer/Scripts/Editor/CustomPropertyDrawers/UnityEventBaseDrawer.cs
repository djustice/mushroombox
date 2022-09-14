/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace InfinityCode.UltimateEditorEnhancer.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(UnityEventBase), true)]
    public class UnityEventBaseDrawer : UnityEventDrawer
    {
        public static Action<Rect, SerializedProperty, GUIContent> OnGUIBefore;
        public static Action<Rect, SerializedProperty, GUIContent> OnGUIAfter;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (OnGUIBefore != null) OnGUIBefore(position, property, label);
            base.OnGUI(position, property, label);
            if (OnGUIAfter != null) OnGUIAfter(position, property, label);
        }
    }
}