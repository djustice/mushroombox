using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace IGL_Tech.RPGM.Auto_Tile_Importer
{
    public class Tiles_A5_Utility
    {

        /// <summary>
        /// Slice the input file to extrapolate the sub block for the A4 style file
        /// </summary>
        /// <param name="img"></param>
        /// <param name="wBlock"></param>
        /// <param name="hBlock"></param>
        /// <param name="mini_tile_w"></param>
        /// <param name="mini_tile_h"></param>
        /// <returns></returns>
        public static void A5_Tile_Slice_File(Texture2D img, out int wBlock, out int hBlock, out List<Texture2D> sub_blocks)
        {
            sub_blocks = new List<Texture2D>();

            wBlock = img.width / 8;

            hBlock = img.height / 16;

            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Texture2D subT = new Texture2D(wBlock, hBlock);
                    subT.SetPixels(img.GetPixels(x * wBlock, (15 - y) * hBlock, wBlock, hBlock));
                    subT.Apply();
                    sub_blocks.Add(subT);
                }
            }
        }

        /// <summary>
        /// Genetate the A2_Tile or the A4Top tile from a sprite sheet image that contains al the tile
        /// </summary>
        /// <param name="tile_path">Path of the sprite sheet image</param>
        /// <param name="rule_tiles">Rule to crate the auto tile</param>
        public static void Generate_A5_Tile(string source_File_Path, string destination_dir_for_auto_tile, string tile_path)
        {
            //create the auto tile
            string dir = destination_dir_for_auto_tile;
            //string dir = Path.Combine(destination_dir_for_auto_tile,
            //    string.Format(@"_{0}", Path.GetFileNameWithoutExtension(source_File_Path)));
            //string atile_path = string.Format(@"{0}/_{1}/{2}.asset", Tiles_Utility.Auto_Tile_Folder_Path, Path.GetFileNameWithoutExtension(source_File_Path),
            string atile_path = Path.Combine(dir, string.Format(@"{0}.asset", Path.GetFileNameWithoutExtension(tile_path)));

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            //A2_Tile atile = new A2_Tile();
            A5_Tile atile = ScriptableObject.CreateInstance<A5_Tile>();
            if (File.Exists(atile_path))
            {
                atile = AssetDatabase.LoadAssetAtPath<A5_Tile>(atile_path);
            }
            EditorUtility.SetDirty(atile);

            if (atile != null)
            {
                Sprite ss = AssetDatabase.LoadAssetAtPath<Sprite>(tile_path);

                atile.preview = ss;
                atile.sprite = ss;
            }
            if (File.Exists(atile_path))
                AssetDatabase.SaveAssets();
            else
                AssetDatabase.CreateAsset(atile, atile_path);
        }
    }
}