/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Reflection;
using UnityEditor;

namespace InfinityCode.UltimateEditorEnhancer.UnityTypes
{
    public static class TerrainInspectorRef
    {
        private static Type _type;
        private static FieldInfo _activeTerrainInspectorField;
        private static FieldInfo _activeTerrainInspectorInstanceField;
        private static PropertyInfo _selectedToolProp;

        public static Type type
        {
            get
            {
                if (_type == null) _type = Reflection.GetEditorType("TerrainInspector");
                return _type;
            }
        }

        private static FieldInfo activeTerrainInspectorInstanceField
        {
            get
            {
                if (_activeTerrainInspectorInstanceField == null) _activeTerrainInspectorInstanceField = type.GetField("s_activeTerrainInspectorInstance", Reflection.StaticLookup);
                return _activeTerrainInspectorInstanceField;
            }
        }

        private static FieldInfo activeTerrainInspectorField
        {
            get
            {
                if (_activeTerrainInspectorField == null) _activeTerrainInspectorField = type.GetField("s_activeTerrainInspector", Reflection.StaticLookup);
                return _activeTerrainInspectorField;
            }
        }

#if UNITY_2020_2_OR_NEWER
        private static PropertyInfo _brushSizeProp;
        

        private static PropertyInfo brushSizeProp
        {
            get
            {
                if (_brushSizeProp == null) _brushSizeProp = type.GetProperty("brushSize", Reflection.InstanceLookup);
                return _brushSizeProp;
            }
        }
#else        
        private static FieldInfo _sizeField;

        private static FieldInfo sizeField
        {
            get
            {
                if (_sizeField == null) _sizeField = type.GetField("m_Size", Reflection.InstanceLookup);
                return _sizeField;
            }
        }
#endif

        private static PropertyInfo selectedToolProp
        {
            get
            {
                if (_selectedToolProp == null) _selectedToolProp = type.GetProperty("selectedTool", Reflection.InstanceLookup);
                return _selectedToolProp;
            }
        }

        public static Editor GetActiveTerrainInspectorInstance()
        {
            return activeTerrainInspectorInstanceField.GetValue(null) as Editor;
        }


        public static float GetBrushSize(Editor editor)
        {
#if UNITY_2020_2_OR_NEWER
            return (float)brushSizeProp.GetValue(editor);
#else
            return (float) sizeField.GetValue(editor);
#endif
        }

        public static int GetSelectedTool(Editor editor)
        {
            return (int)selectedToolProp.GetValue(editor);
        }

        public static void SetBrushSize(Editor editor, float size)
        {
#if UNITY_2020_2_OR_NEWER
            brushSizeProp.SetValue(editor, size);
#else
            sizeField.SetValue(editor, size);
#endif
        }

        public static void SetActiveTerrainInspectorInstance(Editor editor)
        {
            activeTerrainInspectorInstanceField.SetValue(null, editor);
        }

        public static void SetActiveTerrainInspector(int id)
        {
            activeTerrainInspectorField.SetValue(null, id);
        }
    }
}