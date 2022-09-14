using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace IGL_Tech.RPGM.Auto_Tile_Importer
{
    [CustomEditor(typeof(A1_Water_Tile))]
    public class A1_Water_Custom_Editor : Auto_Tile_Base_Editor
    {
        private A1_Water_Tile tile { get { return (target as A1_Water_Tile); } }

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
