#define DEBUG   
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace IGL_Tech.RPGM.Auto_Tile_Importer
{
    public class A2_WOLF_Tiles_Importer_Window : EditorWindow
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
        /// The path of the loaded image
        /// </summary>
        private string path;

        int wBlock = 32, hBlock = 32;
        int mini_tile_w = 16, mini_tile_h = 16;


        [MenuItem("Tools/WOLF Importer/Import A2 Tiles")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<A2_WOLF_Tiles_Importer_Window>(false, "A2 WOLF Tiles");
        }

        void OnGUI()
        {
            if (GUILayout.Button("Load Image")) //open gile dialog to load the image
            {
                path = EditorUtility.OpenFilePanel("Load Tile Set", "", "");
                if (path != null && path != "" && File.Exists(path))
                {
                    img = new Texture2D(1, 1);
                    byte[] bytedata = File.ReadAllBytes(path);
                    img.LoadImage(bytedata);
                    mini_tile_w = img.width / 2;
                    mini_tile_h = img.height / 10;
                    wBlock = mini_tile_w * 2;
                    hBlock = mini_tile_h * 2;
                }
            }

            if (img == null) return;
            GUILayout.Label(img);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Generate Tiles", "Create the selected auto tiles saving the asset in the default folders")))
            {
                //generate the final tiles for the tile palette
                Generate_Tiles(path, img, mini_tile_w, mini_tile_h, wBlock, hBlock);
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
                        Generate_Tiles(path, img, mini_tile_w, mini_tile_h, wBlock, hBlock, sprite_sheet_path, auto_tile_path);
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// generate the tile of tipe A2 based on the parameter passed to the method
        /// </summary>
        /// <param name="path">The path of the input file</param>
        /// <param name="sub_blocks">collection of sliced block</param>
        /// <param name="sub_blocks_to_import">list of boolean to know which block we need to elaborate</param>
        /// <param name="mini_tile_w">size of the mini tile</param>
        /// <param name="mini_tile_h">size of the mini tile</param>
        /// <param name="wBlock">size of the final tile</param>
        /// <param name="hBlock">size of the final tile</param>
        /// <param name="generate_sprite_sheet_image"></param>
        public static void Generate_Tiles(string path, Texture2D image,
            int mini_tile_w, int mini_tile_h, int wBlock, int hBlock, string dest_sprite_sheet = "", string dest_auto_tile = "")
        {
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
            //string loaded_file_image_path = Path.Combine(Tiles_Utility.Final_image_folder_path, string.Format(@"_{0}", fileName));
            //if (!Directory.Exists(loaded_file_image_path))
            //    Directory.CreateDirectory(loaded_file_image_path);

            List<string> images_path = new List<string>();//list of the path of the imported tiles

            Dictionary<byte, int> rule_tiles = new Dictionary<byte, int>(); //dictionary for the tile rules

            int tiles_counter = 0; // counter to enumerate the sprite        

            //temp array to store the sub mini tiles
            Texture2D[] bottom_left_mini_tiles, bottom_right_mini_tiles, top_left_mini_tiles, top_right_mini_tiles;

            //generate the mini tiles to the following computation
            Tiles_A2_WOLF_Utility.Generate_Mini_Tile_A2(image, mini_tile_w, mini_tile_h, out bottom_left_mini_tiles, out bottom_right_mini_tiles, out top_left_mini_tiles, out top_right_mini_tiles);

            Texture2D sprite_tiles = new Texture2D(wBlock * 8, hBlock * 6);
            int sprite_tile_width = wBlock * 8;
            //string sprite_sheet_path = Path.Combine(loaded_file_image_path, 
            string sprite_sheet_path = Path.Combine(path_for_sprite_sheet,
                string.Format(@"_{0}.png", Path.GetFileNameWithoutExtension(path)));

            //generate and iterate the final tile for the subs pieces
            foreach (KeyValuePair<byte, Texture2D> kvp in Tiles_A2_Utility.Generate_Final_Tiles_A2_Terrain(mini_tile_w, mini_tile_h,
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
                importer.maxTextureSize = sprite_tile_width;
                SpriteMetaData[] tmps = new SpriteMetaData[8 * 6];
                string tmpName = Path.GetFileNameWithoutExtension(sprite_sheet_path);
                for (int i = 0; i < 48; i++)
                {
                    int xx = i % 8 * wBlock;
                    int yy = (i / 8 + 1) * hBlock;
                    SpriteMetaData smd = new SpriteMetaData();
                    smd = new SpriteMetaData();
                    smd.alignment = 0;
                    smd.border = new Vector4(0, 0, 0, 0);
                    smd.name = string.Format("{0}_{1:00}", tmpName, i);
                    smd.pivot = new Vector2(.5f, .5f);
                    smd.rect = new Rect(xx, sprite_tiles.height - yy, wBlock, hBlock);
                    tmps[i] = smd;
                }
                importer.spritesheet = tmps;
                importer.SaveAndReimport();
            }
            AssetDatabase.Refresh(); //refresh asset database

            //generate the A2_tile Auto tiles
            for (int i = 0; i < images_path.Count; i += 1)
            {
                string str = images_path[i];
                Tiles_A2_Utility.Generate_A2_Tile_SS(path, str, path_for_auto_tile, rule_tiles);
            }
        }
    }
}