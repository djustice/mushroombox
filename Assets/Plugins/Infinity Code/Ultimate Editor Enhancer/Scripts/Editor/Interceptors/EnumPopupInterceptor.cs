/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Reflection;
using InfinityCode.UltimateEditorEnhancer.UnityTypes;
using InfinityCode.UltimateEditorEnhancer.Windows;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.Interceptors
{
    public class EnumPopupInterceptor: StatedInterceptor<EnumPopupInterceptor>
    {
        protected override MethodInfo originalMethod
        {
            get { return EditorGUIRef.doPopupMethod; }
        }

        public override bool state
        {
            get { return Prefs.searchInEnumFields; }
        }

        protected override string prefixMethodName
        {
            get { return nameof(DoPopupPrefix); }
        }

        private static void DoPopupPrefix(
            Rect position,
            int controlID,
            int selected,
            GUIContent[] popupValues,
            Func<int, bool> checkEnabled,
            GUIStyle style)
        {
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0 && position.Contains(e.mousePosition))
                    {
                        if (Application.platform == RuntimePlatform.OSXEditor)
                        {
                            position.y = (float)(-19.0 + position.y - selected * 16);
                        }

                        if (popupValues.Length >= Prefs.searchInEnumFieldsMinValues)
                        {
                            object instance = Activator.CreateInstance(PopupCallbackInfoRef.type, controlID);
                            PopupCallbackInfoRef.SetInstance(instance);
                            FlatSelectorWindow.Show(position, popupValues, EditorGUI.showMixedValue ? -1 : selected).OnSelect += i => { PopupCallbackInfoRef.GetSetEnumValueDelegate(instance).Invoke(null, null, i); };
                            GUIUtility.keyboardControl = controlID;
                            e.Use();
                        }
                    }
                    break;
                case EventType.KeyDown:
                    if (MainActionKeyForControl(e, controlID))
                    {
                        if (Application.platform == RuntimePlatform.OSXEditor)
                        {
                            position.y = (float)(-19.0 + position.y - selected * 16);
                        }

                        if (popupValues.Length >= Prefs.searchInEnumFieldsMinValues)
                        {
                            object instance = Activator.CreateInstance(PopupCallbackInfoRef.type, controlID);
                            PopupCallbackInfoRef.SetInstance(instance);
                            FlatSelectorWindow.Show(position, popupValues, EditorGUI.showMixedValue ? -1 : selected).OnSelect += i => { PopupCallbackInfoRef.GetSetEnumValueDelegate(instance).Invoke(null, null, i); };
                            e.Use();
                        }
                    }
                    break;
            }
        }

        internal static bool MainActionKeyForControl(Event e, int controlId)
        {
            if (GUIUtility.keyboardControl != controlId) return false;
            bool flag = e.alt || e.shift || e.command || e.control;
            return e.type == EventType.KeyDown && (e.keyCode == KeyCode.Space || e.keyCode == KeyCode.Return || e.keyCode == KeyCode.KeypadEnter) && !flag;
        }
    }
}