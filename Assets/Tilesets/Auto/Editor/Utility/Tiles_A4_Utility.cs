using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace IGL_Tech.RPGM.Auto_Tile_Importer
{
    public class Tiles_A4_Utility
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
        public static void A4_Tile_Slice_File(Texture2D img, out int wBlock, out int hBlock, out int mini_tile_w, out int mini_tile_h, out List<Texture2D> sub_blocks_top, out List<Texture2D> sub_blocks_wall)
        {
            sub_blocks_top = new List<Texture2D>();
            sub_blocks_wall = new List<Texture2D>();

            wBlock = img.width / 16;
            mini_tile_w = wBlock / 2;

            hBlock = img.height / 15;
            mini_tile_h = hBlock / 2;

            int top_height = hBlock * 3;
            int wall_height = hBlock * 2;
            //sub_blocks_to_import = new List<bool>();
            Vector2Int top_sub_size = new Vector2Int(wBlock * 2, hBlock * 3);
            Vector2Int wall_sub_size = new Vector2Int(wBlock * 2, hBlock * 2);
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Texture2D subT = new Texture2D(top_sub_size.x, top_sub_size.y);
                    subT.SetPixels(img.GetPixels(x * top_sub_size.x, (2 - y) * (top_height + wall_height) + wall_height, top_sub_size.x, top_sub_size.y));
                    subT.Apply();
                    sub_blocks_top.Add(subT);

                    subT = new Texture2D(wall_sub_size.x, wall_sub_size.y);
                    subT.SetPixels(img.GetPixels(x * wall_sub_size.x, (2 - y) * (top_height + wall_height), wall_sub_size.x, wall_sub_size.y));
                    subT.Apply();
                    sub_blocks_wall.Add(subT);
                }
            }
        }

        /// <summary>
        /// Genetate the A2_Tile or the A4Top tile from a sprite sheet image that contains al the tile
        /// </summary>
        /// <param name="tile_path">Path of the sprite sheet image</param>
        /// <param name="rule_tiles">Rule to crate the auto tile</param>
        public static void Generate_A4_Tile_SS(string source_File_Path, string tile_path, string destination_dir_for_auto_tile,
            Dictionary<byte, int> rule_tiles, string asset_name = "")
        {
            //create the auto tile
            //string dir = Path.Combine(Tiles_Utility.Auto_Tile_Folder_Path,
            //    string.Format(@"_{0}", Path.GetFileNameWithoutExtension(source_File_Path)));
            string dir = destination_dir_for_auto_tile;
            //string atile_path = string.Format(@"{0}/_{1}/{2}.asset", Tiles_Utility.Auto_Tile_Folder_Path, Path.GetFileNameWithoutExtension(source_File_Path),
            string atile_path = Path.Combine(dir, string.Format(@"{0}.asset", asset_name != "" ? asset_name : Path.GetFileNameWithoutExtension(tile_path)));
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            A4_Top_Tile atile = ScriptableObject.CreateInstance<A4_Top_Tile>();
            if (File.Exists(atile_path))
            {
                atile = AssetDatabase.LoadAssetAtPath<A4_Top_Tile>(atile_path);

            }
            EditorUtility.SetDirty(atile);

            if (atile != null)
            {
                atile.tile_Variants = new Sprite[rule_tiles.Count];
                var vars = Tiles_Utility.LoadSpriteSheet(tile_path);

                int cc = 0;
                foreach (var kvp in rule_tiles)
                {
                    Sprite tmp = vars[kvp.Value + 1] as Sprite;
                    if (cc == 0) //setting the sprite for the tile palette
                    {
                        cc++;
                        atile.sprite = tmp;
                        atile.preview = tmp;
                    }
                    atile.tile_Variants[kvp.Key] = tmp;
                }
            }
            if (File.Exists(atile_path))
                AssetDatabase.SaveAssets();
            else
                AssetDatabase.CreateAsset(atile, atile_path);
        }

        /// <summary>
        /// NOT USED ANYMORE
        /// Generate the A2_Tile or the A4Top tiles from the single image tile
        /// </summary>
        /// <param name="tile_path">Path of the dir where the single image are located</param>
        /// <param name="rule_tiles">Rule to create the tile</param>
        /// <param name="wBlock">width of the block</param>
        public static void Generate_A4_Tile(string source_File_Path, string tile_path, Dictionary<byte, int> rule_tiles, int wBlock)
        {
            //create the auto tile
            string dir = Path.Combine(Tiles_Utility.Auto_Tile_Folder_Path,
                string.Format(@"_{0}", Path.GetFileNameWithoutExtension(source_File_Path)));
            //string atile_path = string.Format(@"{0}/_{1}/{2}.asset", Tiles_Utility.Auto_Tile_Folder_Path, Path.GetFileNameWithoutExtension(source_File_Path),
            string atile_path = Path.Combine(dir, string.Format(@"{0}.asset", Path.GetFileNameWithoutExtension(tile_path)));
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            A4_Top_Tile atile = ScriptableObject.CreateInstance<A4_Top_Tile>();
            if (File.Exists(atile_path))
            {
                atile = AssetDatabase.LoadAssetAtPath<A4_Top_Tile>(atile_path);

            }
            EditorUtility.SetDirty(atile);

            if (atile != null)
            {
                atile.tile_Variants = new Sprite[rule_tiles.Count];
                string[] tiles = Directory.GetFiles(tile_path, "*.png");
                foreach (var fTile in tiles)
                {
                    //set the image importer setting
                    Tiles_Utility.Set_Impoter_Settings(fTile, wBlock);
                }

                int cc = 0;
                //StreamWriter myFile = new StreamWriter(@"C:\tmp\file.txt");
                foreach (var kvp in rule_tiles)
                {
                    Sprite tmp = AssetDatabase.LoadAssetAtPath<Sprite>(tiles[kvp.Value]);
                    if (cc == 0) //setting the sprite for the tile palette
                    {
                        cc++;
                        atile.sprite = tmp;
                        atile.preview = tmp;
                    }
                    atile.tile_Variants[kvp.Key] = tmp;
                    //myFile.WriteLine(string.Format("{0},{1}", kvp.Key, kvp.Value));
                }
                //myFile.Close();
            }
            if (File.Exists(atile_path))
                AssetDatabase.SaveAssets();
            else
                AssetDatabase.CreateAsset(atile, atile_path);
        }
    }
}