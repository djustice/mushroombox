#define DEBUG   
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace IGL_Tech.RPGM.Auto_Tile_Importer
{
    public class A5_Tiles_Importer_Window : EditorWindow
    {
        /// <summary>
        /// Scroll position
        /// </summary>
        static Vector2 scrollPosition = Vector2.zero;

        /// <summary>
        /// Loaded Image from file
        /// </summary>
        protected static Texture2D img = null;

        /// <summary>
        /// List of boolean to select the block to import
        /// </summary>
        protected static List<bool> sub_blocks_to_import;

        /// <summary>
        /// List of the sub pieces from the tile set
        /// </summary>
        protected static List<Texture2D> sub_blocks;

        protected static string path;

        protected static int wBlock = 32, hBlock = 32;
        protected static int mini_tile_w = 16, mini_tile_h = 16;

        //[MenuItem("Tools/RPGM Importer/A5/A5 Full Layout")]
        //public static void ShowWindow()
        //{
        //    EditorWindow.GetWindow<A5_Tiles_Importer_Window>(false, "A5 Full Layout Impoter");
        //}

        public static void Cut_Layout(string path_to_slice)
        {
            path = path_to_slice;
            img = new Texture2D(1, 1);
            byte[] bytedata = File.ReadAllBytes(path);
            img.LoadImage(bytedata);

            //get the sliced part
            Tiles_A5_Utility.A5_Tile_Slice_File(img, out wBlock, out hBlock, out sub_blocks);
            sub_blocks_to_import = new List<bool>();
            for (int i = 0; i < sub_blocks.Count; i++)
                sub_blocks_to_import.Add(false);
        }

        //protected virtual void Select_Image()
        //{
        //    if (GUILayout.Button("Load Image"))
        //    {
        //        //path = EditorUtility.OpenFilePanel("Load Tile Set", Tiles_Utility.Last_Opened_Dir, "");
        //        path = EditorUtility.OpenFilePanel("Load Tile Set", "", "");
        //        if (path != null && path != "" && File.Exists(path))
        //        {
        //            //Tiles_Utility.Last_Opened_Dir = System.IO.Path.GetDirectoryName(path);
        //            CutLayout();
        //        }
        //        else
        //        {
        //            if (path != null && path != "")
        //                EditorUtility.DisplayDialog("Selection error!", "You have to select a file or an A5 file compatibile with RPG MAKER tile set", "OK");
        //        }
        //    }
        //}

        public static void OnGUI()
        {
            //Select_Image();

            if (img == null) return;

            GUILayout.Label("Select the tile you want to import, then click the 'Generate Tiles' Button");
            //can select or deselect all
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Select All"))
            {
                if (sub_blocks_to_import != null)
                    for (int i = 0; i < sub_blocks_to_import.Count; i++)
                        sub_blocks_to_import[i] = true;
            }
            if (GUILayout.Button("Select None"))
            {
                if (sub_blocks_to_import != null)
                    for (int i = 0; i < sub_blocks_to_import.Count; i++)
                        sub_blocks_to_import[i] = false;
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Generate Tiles", "Create the selected auto tiles saving the asset in the default folders")))
            {
                //generate the top tile. They are A2 style tile
                Generate_Tiles(path, sub_blocks, sub_blocks_to_import, wBlock, hBlock);
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
                        Generate_Tiles(path, sub_blocks, sub_blocks_to_import, wBlock, hBlock,
                            sprite_sheet_path, auto_tile_path);
                    }
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar,
                GUILayout.Height(Screen.height / 3 * 2));
            GUILayout.BeginVertical();
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            for (int i = 0; i < 8 * 16; i++)
            {
                if (i != 0 && i % 8 == 0)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
                GUILayout.BeginVertical();
                if (sub_blocks != null)
                {
                    Texture2D sub_top = sub_blocks[i];
                    sub_blocks_to_import[i] = GUILayout.Toggle(sub_blocks_to_import[i], sub_top);
                }

                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        /// <summary>
        /// Generate the selected tiles
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sub_blocks"></param>
        /// <param name="sub_blocksto_import"></param>
        /// <param name="wBlock"></param>
        /// <param name="hBlock"></param>
        public static void Generate_Tiles(string path, List<Texture2D> sub_blocks, List<bool> sub_blocksto_import, int wBlock, int hBlock,
            string dest_sprite_sheet = "", string dest_auto_tile = "")
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
            //string fileName = Path.GetFileNameWithoutExtension(path);

            List<string> images_path = new List<string>();//list of the folder of the imported tiles

            //foreach sub pieces in the image. If it's an animated auto tile 3 consecutive sub blocks are 3 frame of the animation
            for (int sub_block_count = 0; sub_block_count < sub_blocks_to_import.Count; sub_block_count++)
            {
                //If the current sub is not selected to process than skip it
                if (!sub_blocks_to_import[sub_block_count]) continue;

                //int tiles_counter = 0; //set zero to che final tile counter
                //Texture2D sub_piece = sub_blocks[sub_block_count];

                //save each final tile to its own image
                var tile_bytes = sub_blocks[sub_block_count].EncodeToPNG();
                //string tile_file_path = Path.Combine(loaded_file_image_path, 
                string tile_file_path = Path.Combine(path_for_sprite_sheet,
                    string.Format(@"_{0}_{1:000}.png", Path.GetFileNameWithoutExtension(path), sub_block_count));


                images_path.Add(tile_file_path);

                File.WriteAllBytes(tile_file_path, tile_bytes);
                AssetDatabase.Refresh();
                TextureImporter importer = AssetImporter.GetAtPath(tile_file_path) as TextureImporter;

                if (importer != null)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spriteImportMode = SpriteImportMode.Single;
                    importer.filterMode = FilterMode.Point;
                    importer.spritePixelsPerUnit = hBlock;
                    importer.compressionQuality = 0;
                    importer.textureCompression = TextureImporterCompression.Uncompressed;
                    importer.maxTextureSize = wBlock;
                    importer.SaveAndReimport();
                }

                //tiles_counter++;
            }
            AssetDatabase.Refresh(); //refresh asset database

            //generate the fixed Auto tiles
            for (int i = 0; i < images_path.Count; i++)
            {
                Tiles_A5_Utility.Generate_A5_Tile(path, path_for_auto_tile, images_path[i]);
            }
        }
    }
}