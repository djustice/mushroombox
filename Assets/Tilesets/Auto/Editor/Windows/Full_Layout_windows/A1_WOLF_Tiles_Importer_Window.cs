#define DEBUG   
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace IGL_Tech.RPGM.Auto_Tile_Importer
{
    public class A1_WOLF_Tiles_Importer_Window : EditorWindow
    {
        /// <summary>
        /// Scroll position
        /// </summary>
        //Vector2 scrollPosition = Vector2.zero;

        /// <summary>
        /// Loaded Image from file
        /// </summary>
        Texture2D img = null;

        /// <summary>
        /// List of the sub pieces from the tile set
        /// </summary>
        List<Texture2D> sub_blocks_water;

        private string path;

        int wBlock = 32, hBlock = 32;
        int mini_tile_w = 16, mini_tile_h = 16;

        [MenuItem("Tools/WOLF Importer/Import A1 Tiles")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<A1_WOLF_Tiles_Importer_Window>(false, "A1 WOLF Tiles");
        }

        void OnGUI()
        {
            if (GUILayout.Button("Load Image"))
            {
                path = EditorUtility.OpenFilePanel("Load Tile Set", "", "");
                if (path != null && path != "" && File.Exists(path))
                {
                    img = new Texture2D(1, 1);
                    byte[] bytedata = File.ReadAllBytes(path);
                    img.LoadImage(bytedata);

                    //get the sliced part
                    Tiles_A1_WOLF_Utility.A1_Tile_Slice_File(img, out wBlock, out hBlock, out mini_tile_w, out mini_tile_h, out sub_blocks_water);
                }
            }

            if (img == null) return;
            GUILayout.BeginHorizontal();
            foreach (var txt in sub_blocks_water)
                GUILayout.Label(txt);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Generate Tiles", "Create the selected auto tiles saving the asset in the default folders")))
            {
                //generate the animated A1 style tile
                Generate_Water_Tiles(path, sub_blocks_water,
                    mini_tile_w, mini_tile_h, wBlock, hBlock);
            }
            GUILayout.Space(10);
            if (GUILayout.Button(new GUIContent("Generate Tiles as", "Create the selected auto tiles saving the asset in the specified folders")))
            {
                string sprite_sheet_path = EditorUtility.OpenFolderPanel("Select the directory for the sprite sheets", "Assets", "");
                if (sprite_sheet_path != null && sprite_sheet_path != "")
                {
                    string auto_tile_path = EditorUtility.OpenFolderPanel("Select the directory for the auto tiles", "Assets", "");
                    if (auto_tile_path != null && auto_tile_path != "")
                    {
                        sprite_sheet_path = sprite_sheet_path.Substring(sprite_sheet_path.IndexOf("Assets"));
                        auto_tile_path = auto_tile_path.Substring(auto_tile_path.IndexOf("Assets"));
                        //generate the animated A1 style tile
                        Generate_Water_Tiles(path, sub_blocks_water,
                            mini_tile_w, mini_tile_h, wBlock, hBlock, sprite_sheet_path, auto_tile_path);
                    }
                }
            }
        }

        /// <summary>
        /// Generate the selected water tile
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sub_blocks_water"></param>
        /// <param name="sub_blocks_water_to_import"></param>
        /// <param name="mini_tile_w"></param>
        /// <param name="mini_tile_h"></param>
        /// <param name="wBlock"></param>
        /// <param name="hBlock"></param>
        /// <param name="generate_sprite_sheet_image"></param>
        public static void Generate_Water_Tiles(string path, List<Texture2D> sub_blocks_water,
            int mini_tile_w, int mini_tile_h, int wBlock, int hBlock, string dest_sprite_sheet = "", string dest_auto_tile = "")
        {
            //create the final directory for the auto tile
            string path_for_sprite_sheet = dest_sprite_sheet != "" ? dest_sprite_sheet : Tiles_Utility.Final_image_folder_path;
            string path_for_auto_tile = dest_auto_tile != "" ? dest_auto_tile : Tiles_Utility.Auto_Tile_Folder_Path;
            //create the final directory for the auto tile
            if (!Directory.Exists(path_for_auto_tile))
                Directory.CreateDirectory(path_for_auto_tile);

            //create the final directory for the generated Images
            if (!Directory.Exists(path_for_sprite_sheet))
                Directory.CreateDirectory(path_for_sprite_sheet);

            //create the folder for that specific file image
            string fileName = Path.GetFileNameWithoutExtension(path);
            //string loaded_file_image_path = string.Format(@"{0}/_{1}", Tiles_Utility.Final_image_folder_path, fileName); //ex rtp_import\Outside_A2\single_block_folder\final_tile\Image
            //string loaded_file_image_path = Path.Combine(Tiles_Utility.Final_image_folder_path, string.Format(@"_{0}",fileName)); //ex rtp_import\Outside_A2\single_block_folder\final_tile\Image
            //if (!Directory.Exists(loaded_file_image_path))
            //    Directory.CreateDirectory(loaded_file_image_path);

            List<string> images_path = new List<string>();//list of the folder of the imported tiles

            Dictionary<byte, int> rule_tiles = new Dictionary<byte, int>();

            for (int i = 0; i < 3; i++)
            {
                int tiles_counter = 0; //set zero to che final tile counter
                Texture2D sub_piece = sub_blocks_water[i];

                //temp array to store the sub mini tiles
                Texture2D[] bottom_left_mini_tiles, bottom_right_mini_tiles, top_left_mini_tiles, top_right_mini_tiles;

                //generate the mini tiles to the following computation
                Tiles_A1_WOLF_Utility.Generate_Mini_Tile_A1Water(sub_piece, mini_tile_w, mini_tile_h, out bottom_left_mini_tiles, out bottom_right_mini_tiles, out top_left_mini_tiles, out top_right_mini_tiles);

                Texture2D sprite_tiles = new Texture2D(wBlock * 8, hBlock * 6);
                //string sprite_sheet_path = Path.Combine(loaded_file_image_path,  
                string sprite_sheet_path = Path.Combine(path_for_sprite_sheet,
                    string.Format(@"_Water_{0}_{1}.png", Path.GetFileNameWithoutExtension(path), i));

                //generate and iterate the final tile for the subs pieces
                foreach (KeyValuePair<byte, Texture2D> kvp in Tiles_A1_Utility.Generate_Final_Tiles_A1_Water(mini_tile_w, mini_tile_h,
                    bottom_left_mini_tiles, bottom_right_mini_tiles, top_left_mini_tiles, top_right_mini_tiles, rule_tiles))
                {
                    int xx = tiles_counter % 8 * wBlock;
                    int yy = tiles_counter / 8 * hBlock;
                    sprite_tiles.SetPixels(xx, sprite_tiles.height - yy - hBlock, wBlock, hBlock, kvp.Value.GetPixels());
                    tiles_counter++;
                }

                images_path.Add(sprite_sheet_path);
                File.WriteAllBytes(sprite_sheet_path, sprite_tiles.EncodeToPNG());
                AssetDatabase.Refresh();
                TextureImporter importer = AssetImporter.GetAtPath(sprite_sheet_path) as TextureImporter;
                if (importer != null)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spriteImportMode = SpriteImportMode.Multiple;
                    importer.filterMode = FilterMode.Point;
                    importer.spritePixelsPerUnit = hBlock;
                    importer.compressionQuality = 0;
                    importer.textureCompression = TextureImporterCompression.Uncompressed;
                    importer.maxTextureSize = sprite_tiles.width;
                    SpriteMetaData[] tmps = new SpriteMetaData[8 * 6];
                    string tmpName = Path.GetFileNameWithoutExtension(sprite_sheet_path);
                    for (int j = 0; j < 48; j++)
                    {
                        int xx = j % 8 * wBlock;
                        int yy = (j / 8 + 1) * hBlock;
                        SpriteMetaData smd = new SpriteMetaData();
                        smd = new SpriteMetaData();
                        smd.alignment = 0;
                        smd.border = new Vector4(0, 0, 0, 0);
                        smd.name = string.Format("{0}_{1:00}", tmpName, j);
                        smd.pivot = new Vector2(.5f, .5f);
                        smd.rect = new Rect(xx, sprite_tiles.height - yy, wBlock, hBlock);
                        tmps[j] = smd;
                    }
                    importer.spritesheet = tmps;
                    importer.SaveAndReimport();
                }
            }

            AssetDatabase.Refresh(); //refresh asset database

            //generate the fixed Auto tiles
            for (int i = 0; i < images_path.Count; i += 3)
            {
                Tiles_A1_Utility.Generate_A1_Water_Tile_SS(path, path_for_auto_tile,
                    images_path[i], images_path[i + 1], images_path[i + 2], rule_tiles, wBlock);
            }
        }
    }
}