/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Collections.Generic;
using System.IO;
using InfinityCode.UltimateEditorEnhancer.Interceptors;
using UnityEditor;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static partial class Prefs
    {
        public static bool _changeNumberFieldValueByArrow = true;
        public static bool _expandLongTextFields = true;
        public static bool _hierarchyTypeFilter = true;
        public static bool _improveCurveEditor = true;
        public static bool _searchInEnumFields = true;
        public static bool _unsafeFeatures = true;
        public static int searchInEnumFieldsMinValues = 6;

        private static int hasUnsafeBlock = -1; // -1 - unknown, 0 - no block, 1 - has block

        public static bool changeNumberFieldValueByArrow
        {
            get => _changeNumberFieldValueByArrow && unsafeFeatures;
        }

        public static bool expandLongTextFields
        {
            get => _expandLongTextFields && unsafeFeatures;
        }

        public static bool hierarchyTypeFilter
        {
            get => _hierarchyTypeFilter && unsafeFeatures;
        }

        public static bool improveCurveEditor
        {
            get => _improveCurveEditor && unsafeFeatures;
        }

        public static bool searchInEnumFields
        {
            get => _searchInEnumFields && unsafeFeatures;
        }

        public static bool unsafeFeatures
        {
            get
            {
                if (hasUnsafeBlock == -1)
                {
                    hasUnsafeBlock = File.Exists("UEENoUnsafe.txt") ? 1 : 0;
                }
                return _unsafeFeatures && hasUnsafeBlock == 0;
            }
        }

        public class UnsafeManager: StandalonePrefManager<UnsafeManager>
        {
            public override IEnumerable<string> keywords
            {
                get
                {
                    return new[]
                    {
                        "Unsafe",
                        "Change Number Fields Value By Arrows",
                        "Hierarchy Type Filter",
                        "Improve Curve Editor",
                        "Search In Enum Fields"
                    };
                }
            }

            private void DrawToggleField(string label, ref bool value, Action OnChange)
            {
                EditorGUI.BeginChangeCheck();
                value = EditorGUILayout.ToggleLeft(label, value);
                if (EditorGUI.EndChangeCheck() && OnChange != null) OnChange();
            }

            public override void Draw()
            {
                EditorGUI.BeginChangeCheck();

                _unsafeFeatures = EditorGUILayout.ToggleLeft("Unsafe Features", _unsafeFeatures);

                if (EditorGUI.EndChangeCheck())
                {
                    EnumPopupInterceptor.Refresh();
                    HierarchyToolbarInterceptor.Refresh();
                    NumberFieldInterceptor.Refresh();
                }

                EditorGUI.BeginDisabledGroup(!_unsafeFeatures);

                DrawToggleField("Change Number Fields Value By Arrows", ref _changeNumberFieldValueByArrow, NumberFieldInterceptor.Refresh);
                _expandLongTextFields = EditorGUILayout.ToggleLeft("Expand Long Text Fields", _expandLongTextFields);
                DrawToggleField("Hierarchy Type Filter", ref _hierarchyTypeFilter, HierarchyToolbarInterceptor.Refresh);
                _improveCurveEditor = EditorGUILayout.ToggleLeft("Improve Curve Editor", _improveCurveEditor);
                DrawToggleField("Search In Enum Fields", ref _searchInEnumFields, EnumPopupInterceptor.Refresh);

                if (_searchInEnumFields)
                {
                    EditorGUI.indentLevel++;
                    searchInEnumFieldsMinValues = EditorGUILayout.IntField("Min Values", searchInEnumFieldsMinValues);
                    EditorGUI.indentLevel--;
                }

                EditorGUI.EndDisabledGroup();
            }
        }
    }
}