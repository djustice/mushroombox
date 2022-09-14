/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Collections.Generic;
using System.Linq;
using InfinityCode.UltimateEditorEnhancer.UnityTypes;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.Behaviors
{
    [InitializeOnLoad]
    public static class SelectCustomTool
    {
        private static Type lastCustomTool;

        static SelectCustomTool()
        {
            KeyManager.KeyBinding binding = KeyManager.AddBinding();
            binding.OnValidate += OnValidateShortcut;
            binding.OnInvoke += Select;

#if UNITY_2020_2_OR_NEWER
            ToolManager.activeToolChanged += OnActiveToolChanged;
#else
            EditorTools.activeToolChanged += OnActiveToolChanged;
#endif
        }

        private static Type GetActiveToolType()
        {
#if UNITY_2020_2_OR_NEWER
            return ToolManager.activeToolType;
#else
            return EditorTools.activeToolType;
#endif
        }

        private static void OnActiveToolChanged()
        {
            List<Type> tools = EditorToolUtilityRef.GetCustomEditorToolsForType(null);
            Type activeToolType = GetActiveToolType();
            if (tools != null && tools.Any(t => t == activeToolType)) lastCustomTool = activeToolType;
        }

        private static bool OnValidateShortcut()
        {
            Event e = Event.current;
            if (e.keyCode != Prefs.switchCustomToolKeyCode || e.modifiers != Prefs.switchCustomToolModifiers) return false;
            return !EditorGUIRef.IsEditingTextField();
        }

        private static void Select()
        {
            List<Type> tools = EditorToolUtilityRef.GetCustomEditorToolsForType(null);
            
            if (tools.Any(t => t == GetActiveToolType()))
            {
                int index = tools.IndexOf(lastCustomTool) + 1;
                SetTool(tools[index < tools.Count? index: 0]);
            }
            else
            {
                if (lastCustomTool == null) SetTool(tools[0]);
                else SetTool(lastCustomTool);
            }
        }

        private static void SetTool(Type type)
        {
#if UNITY_2020_2_OR_NEWER
            ToolManager.SetActiveTool(type);
#else
            EditorTools.SetActiveTool(type);
#endif
            string label;
            object[] attributes = type.GetCustomAttributes(typeof(EditorToolAttribute), true);
            if (attributes.Length > 0)
            {
                label = (attributes[0] as EditorToolAttribute).displayName;
            }
            else label = type.Name;

            foreach (SceneView view in SceneView.sceneViews)
            {
                view.ShowNotification(TempContent.Get(label), 1);
            }
        }
    }
}