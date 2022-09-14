/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Reflection;

namespace InfinityCode.UltimateEditorEnhancer.UnityTypes
{
    public static class TreeViewControllerRef
    {
        private static PropertyInfo _guiProp;
        private static Type _type;

        private static PropertyInfo guiProp
        {
            get
            {
                if (_guiProp == null) _guiProp = type.GetProperty("gui", Reflection.InstanceLookup);
                return _guiProp;
            }
        }

        public static Type type
        {
            get
            {
                if (_type == null) _type = Reflection.GetEditorType("IMGUI.Controls.TreeViewController");
                return _type;
            }
        }

        public static object GetGUI(object instance)
        {
            return guiProp.GetValue(instance);
        }
    }
}