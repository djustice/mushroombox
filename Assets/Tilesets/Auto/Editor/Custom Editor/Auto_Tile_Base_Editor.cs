using UnityEngine;
using UnityEditor;

namespace IGL_Tech.RPGM.Auto_Tile_Importer
{
    [CustomEditor(typeof(Auto_Tile_Base))]
    public class Auto_Tile_Base_Editor : Editor
    {

        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            Auto_Tile_Base myTile = target as Auto_Tile_Base;

            if (myTile == null) return null;

            Texture2D preview;
#if !UNITY_2021
            do
            {
                System.Threading.Thread.Sleep(100);
                preview = AssetPreview.GetAssetPreview(myTile.preview);
            }
            while (preview == null);
#else
            preview = AssetPreview.GetAssetPreview(myTile.preview);
#endif
            if (preview != null)
            {
                Texture2D cache = new Texture2D(width, height);
                EditorUtility.CopySerialized(preview, cache);
                return cache;
            }

            return null;
        }
    }
}
