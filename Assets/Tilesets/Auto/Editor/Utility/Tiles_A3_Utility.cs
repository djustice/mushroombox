using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace IGL_Tech.RPGM.Auto_Tile_Importer
{
    public class Tiles_A3_Utility
    {
        /// <summary>
        /// Slice the input file to extrapolate the sub block for the A3 style file
        /// </summary>
        /// <param name="img"></param>
        /// <param name="wBlock"></param>
        /// <param name="hBlock"></param>
        /// <param name="mini_tile_w"></param>
        /// <param name="mini_tile_h"></param>
        /// <returns></returns>
        public static List<Texture2D> A3_Tile_Slice_File(Texture2D img, out int wBlock, out int hBlock, out int mini_tile_w, out int mini_tile_h)
        {
            List<Texture2D> sub_blocks = new List<Texture2D>();
            //sub_blocks_to_import = new List<bool>();
            Vector2Int sub_size = new Vector2Int(img.width / 8, img.height / 4); //that is a fixed number of blocks
                                                                                 //divide in sub blocks
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Texture2D sub = new Texture2D(sub_size.x, sub_size.y);
                    sub.SetPixels(img.GetPixels(x * sub_size.x, (3 - y) * sub_size.y, sub_size.x, sub_size.y));
                    sub.Apply();
                    sub_blocks.Add(sub);
                }
            }
            mini_tile_w = sub_size.x / 4;
            wBlock = mini_tile_w * 2;
            mini_tile_h = sub_size.y / 4;
            hBlock = mini_tile_h * 2;
            return sub_blocks;
        }


        /// <summary>
        /// Generate the mini tiles for the WALL tile
        /// </summary>
        /// <param name="img"></param>
        /// <param name="wBlock"></param>
        /// <param name="hBlock"></param>
        /// <param name="bottom_left_mini_tiles"></param>
        /// <param name="bottom_right_mini_tiles"></param>
        /// <param name="top_left_mini_tiles"></param>
        /// <param name="top_right_mini_tiles"></param>
        public static void Generate_Mini_Tile_A3_Wall(Texture2D img, int wBlock, int hBlock,
        out Texture2D[] bottom_left_mini_tiles, out Texture2D[] bottom_right_mini_tiles,
        out Texture2D[] top_left_mini_tiles, out Texture2D[] top_right_mini_tiles, out Texture2D[,] rawPieces)
        {
            //Create the mini tile array
            bottom_left_mini_tiles = new Texture2D[9];
            bottom_right_mini_tiles = new Texture2D[9];
            top_left_mini_tiles = new Texture2D[9];
            top_right_mini_tiles = new Texture2D[9];

            rawPieces = new Texture2D[4, 4];
            int cc = 0;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    Texture2D tmp = new Texture2D(wBlock, hBlock);

                    Color[] pixels = img.GetPixels(x * wBlock, (3 - y) * hBlock, wBlock, hBlock);
                    tmp.SetPixels(0, 0, wBlock, hBlock, pixels);
                    tmp.Apply();
                    rawPieces[y, x] = tmp;
                    cc++;
                }
            }

            //bottom_left_mini_tiles
            bottom_left_mini_tiles[0] = rawPieces[3, 0];
            bottom_left_mini_tiles[1] = rawPieces[3, 1];
            bottom_left_mini_tiles[2] = rawPieces[3, 2];
            bottom_left_mini_tiles[3] = rawPieces[2, 0];
            bottom_left_mini_tiles[4] = rawPieces[2, 1];
            bottom_left_mini_tiles[5] = rawPieces[2, 2];
            bottom_left_mini_tiles[6] = rawPieces[1, 0];
            bottom_left_mini_tiles[7] = rawPieces[1, 1];
            bottom_left_mini_tiles[8] = rawPieces[1, 2];

            //bottom_right_mini_tiles
            bottom_right_mini_tiles[0] = rawPieces[3, 3];
            bottom_right_mini_tiles[1] = rawPieces[2, 3];
            bottom_right_mini_tiles[2] = rawPieces[1, 3];
            bottom_right_mini_tiles[3] = rawPieces[3, 2];
            bottom_right_mini_tiles[4] = rawPieces[2, 2];
            bottom_right_mini_tiles[5] = rawPieces[1, 2];
            bottom_right_mini_tiles[6] = rawPieces[3, 1];
            bottom_right_mini_tiles[7] = rawPieces[2, 1];
            bottom_right_mini_tiles[8] = rawPieces[1, 1];

            //top_right_mini_tiles
            top_right_mini_tiles[0] = rawPieces[0, 3];
            top_right_mini_tiles[1] = rawPieces[0, 2];
            top_right_mini_tiles[2] = rawPieces[0, 1];
            top_right_mini_tiles[3] = rawPieces[1, 3];
            top_right_mini_tiles[4] = rawPieces[1, 2];
            top_right_mini_tiles[5] = rawPieces[1, 1];
            top_right_mini_tiles[6] = rawPieces[2, 3];
            top_right_mini_tiles[7] = rawPieces[2, 2];
            top_right_mini_tiles[8] = rawPieces[2, 1];

            //top_left_mini_tiles
            top_left_mini_tiles[0] = rawPieces[0, 0];
            top_left_mini_tiles[1] = rawPieces[1, 0];
            top_left_mini_tiles[2] = rawPieces[2, 0];
            top_left_mini_tiles[3] = rawPieces[0, 1];
            top_left_mini_tiles[4] = rawPieces[1, 1];
            top_left_mini_tiles[5] = rawPieces[2, 1];
            top_left_mini_tiles[6] = rawPieces[0, 2];
            top_left_mini_tiles[7] = rawPieces[1, 2];
            top_left_mini_tiles[8] = rawPieces[2, 2];
        }

        public static int Select_Mini_Tile_A3_Wall(byte value, int b1, int b2, int b3, int b4)
        {
            bool bit1 = Tiles_Utility.GetBit(value, b1);
            bool bit2 = Tiles_Utility.GetBit(value, b2);
            bool bit3 = Tiles_Utility.GetBit(value, b3);
            //bool bitX = GetBit(value, bx);
            //bool bitY = GetBit(value, by);

            //corner
            if (!bit1 && !bit3)
                return 0; //mini_tiles[0];//corner

            //center
            if (bit1 && bit2 && bit3)
                return 8;

            if (!bit1 && bit3)
                return 6;

            //up side
            if (bit1 && !bit3)
                return 2;

            return 8; //mini_tiles[4];//mini_tiles[9];//tip
        }

        /// <summary>
        /// Generate the final tile for the A3 Wall tile
        /// </summary>
        /// <param name="mini_tile_w"></param>
        /// <param name="mini_tile_h"></param>
        /// <param name="bottom_left_mini_tiles"></param>
        /// <param name="bottom_right_mini_tiles"></param>
        /// <param name="top_left_mini_tiles"></param>
        /// <param name="top_right_mini_tiles"></param>
        /// <param name="rule_tiles"></param>
        /// <returns></returns>
        public static Dictionary<byte, Texture2D> Generate_Final_Tiles_A3_Wall(int mini_tile_w, int mini_tile_h, Texture2D[] bottom_left_mini_tiles, Texture2D[] bottom_right_mini_tiles,
            Texture2D[] top_left_mini_tiles, Texture2D[] top_right_mini_tiles, Dictionary<byte, int> rule_tiles)
        {
            Dictionary<byte, Texture2D> final_pieces = new Dictionary<byte, Texture2D>(); //pezzi finali da considerare per la creazione dell'immagine

            rule_tiles.Clear(); //lista delle regole di assegnamento

            List<string> used_Combination = new List<string>(); //dictionary of used combinations
            foreach (var comb in Tiles_Utility.ByteCombination())
            {
                Texture2D tmp = new Texture2D(mini_tile_w * 2, mini_tile_h * 2); //to make parametric 
                #region DO NOT TOUCH! This is the magic
                int bl = Select_Mini_Tile_A3_Wall(comb, 6, 5, 4, 0);
                int br = Select_Mini_Tile_A3_Wall(comb, 4, 3, 2, 6);
                int tr = Select_Mini_Tile_A3_Wall(comb, 2, 1, 0, 4);
                int tl = Select_Mini_Tile_A3_Wall(comb, 0, 7, 6, 2);

                tmp.SetPixels(0, 0, mini_tile_w, mini_tile_h, bottom_left_mini_tiles[bl].GetPixels());
                tmp.SetPixels(mini_tile_w, 0, mini_tile_w, mini_tile_h, bottom_right_mini_tiles[br].GetPixels());
                tmp.SetPixels(0, mini_tile_h, mini_tile_w, mini_tile_h, top_left_mini_tiles[tl].GetPixels());
                tmp.SetPixels(mini_tile_w, mini_tile_h, mini_tile_w, mini_tile_h, top_right_mini_tiles[tr].GetPixels());
                tmp.Apply();
                #endregion

                string key = string.Format("{0}{1}{2}{3}", bl, br, tl, tr);
                if (!used_Combination.Contains(key))
                {
                    used_Combination.Add(key); //salvo le key degli sprite usate
                    final_pieces.Add(comb, tmp);
                }
                int index = used_Combination.IndexOf(key); //retrive the index
                rule_tiles.Add(comb, index);
            }
            return final_pieces;
        }

        /// <summary>
        /// Generate the auto tile for the wall using a sprite sheet
        /// </summary>
        /// <param name="source_File_Path"></param>
        /// <param name="tile_path"></param>
        /// <param name="rule_tiles"></param>
        /// <param name="asset_name"></param>
        public static void Generate_A3_Wall_tile_SS(string source_File_Path, string tile_path, string destination_dir_for_auto_tile,
            Dictionary<byte, int> rule_tiles, string asset_name = "")
        {
            //create the auto tile
            //string dir = Path.Combine(Tiles_Utility.Auto_Tile_Folder_Path,
            //    string.Format(@"_{0}", Path.GetFileNameWithoutExtension(source_File_Path)));
            string dir = destination_dir_for_auto_tile;
            //string atile_path = string.Format(@"{0}/_{1}/{2}.asset", Tiles_Utility.Auto_Tile_Folder_Path, Path.GetFileNameWithoutExtension(source_File_Path),
            string atile_path = Path.Combine(dir, string.Format(@"{0}.asset", asset_name != "" ? asset_name : Path.GetFileNameWithoutExtension(tile_path)));
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            //A2_Tile atile = new A2_Tile();
            A3_Tile atile = ScriptableObject.CreateInstance<A3_Tile>();
            if (File.Exists(atile_path))
            {
                atile = AssetDatabase.LoadAssetAtPath<A3_Tile>(atile_path);

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
        /// </summary>
        /// <param name="source_File_Path"></param>
        /// <param name="tile_path"></param>
        /// <param name="rule_tiles"></param>
        /// <param name="wBlock"></param>
        public static void Generate_A3_Wall_Tile(string source_File_Path, string tile_path, Dictionary<byte, int> rule_tiles, int wBlock)
        {
            //create the auto tile
            string dir = Path.Combine(Tiles_Utility.Auto_Tile_Folder_Path,
                string.Format(@"_{0}", Path.GetFileNameWithoutExtension(source_File_Path)));
            //string atile_path = string.Format(@"{0}/_{1}/{2}.asset", Tiles_Utility.Auto_Tile_Folder_Path, Path.GetFileNameWithoutExtension(source_File_Path),
            string atile_path = Path.Combine(dir, string.Format(@"{0}.asset", Path.GetFileNameWithoutExtension(tile_path)));
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            A3_Tile atile = ScriptableObject.CreateInstance<A3_Tile>();
            if (File.Exists(atile_path))
            {
                atile = AssetDatabase.LoadAssetAtPath<A3_Tile>(atile_path);

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