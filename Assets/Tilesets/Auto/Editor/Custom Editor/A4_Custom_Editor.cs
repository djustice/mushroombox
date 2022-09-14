using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace IGL_Tech.RPGM.Auto_Tile_Importer
{
    [CustomEditor(typeof(A4_Top_Tile))]
    public class A4_Custom_Editor : Auto_Tile_Base_Editor
    {
        private A4_Top_Tile tile { get { return (target as A4_Top_Tile); } }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            float oldLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 210;

            EditorGUI.BeginChangeCheck();
            tile.preview = (Sprite)EditorGUILayout.ObjectField("Preview", tile.preview, typeof(Sprite), false, null);

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(tile);

            EditorGUIUtility.labelWidth = oldLabelWidth;
        }
    }
}