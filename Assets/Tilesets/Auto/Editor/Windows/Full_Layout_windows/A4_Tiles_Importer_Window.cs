#define DEBUG   
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace IGL_Tech.RPGM.Auto_Tile_Importer
{
    public class A4_Tiles_Importer_Window : EditorWindow
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
        /// List of the sub pieces from the tile set
        /// </summary>
        protected static List<Texture2D> sub_blocks_top;

        /// <summary>
        /// List of the sub pieces from the tile set
        /// </summary>
        protected static List<Texture2D> sub_blocks_wall;

        /// <summary>
        /// List of boolean to select the block to import
        /// </summary>
        protected static List<bool> sub_blocks_top_to_import;

        /// <summary>
        /// List of boolean to select the block to import
        /// </summary>
        protected static List<bool> sub_blocks_wall_to_import;

        protected static List<string> sub_blocks_top_names;
        protected static List<string> sub_blocks_wall_names;

        protected static string path;

        protected static int wBlock = 32, hBlock = 32;
        protected static int mini_tile_w = 16, mini_tile_h = 16;

        ///// Set to generate a single sprite sheet file or multiple single image
        ///// </summary>
        //bool generate_sprite_sheet_image = true;

        //[MenuItem("Tools/RPGM Importer/A4/A4 Full Layout")]
        //public static void ShowWindow()
        //{
        //    EditorWindow.GetWindow<A4_Tiles_Importer_Window>(false, "A4 Full Layout Impoter");
        //}

        public static void Cut_Layout(string path_to_slice)
        {
            path = path_to_slice;
            img = new Texture2D(1, 1);
            byte[] bytedata = File.ReadAllBytes(path);
            img.LoadImage(bytedata);

            //get the sliced part
            Tiles_A4_Utility.A4_Tile_Slice_File(img, out wBlock, out hBlock, out mini_tile_w, out mini_tile_h, out sub_blocks_top, out sub_blocks_wall);
            sub_blocks_top_to_import = new List<bool>();
            sub_blocks_top_names = new List<string>();
            for (int i = 0; i < sub_blocks_top.Count; i++)
            {
                sub_blocks_top_to_import.Add(false);
                sub_blocks_top_names.Add(string.Format("_C_{0}_{1}", Path.GetFileNameWithoutExtension(path), i));
            }

            sub_blocks_wall_to_import = new List<bool>();
            sub_blocks_wall_names = new List<string>();
            for (int i = 0; i < sub_blocks_wall.Count; i++)
            {
                sub_blocks_wall_to_import.Add(false);
                sub_blocks_wall_names.Add(string.Format("_W_{0}_{1}", Path.GetFileNameWithoutExtension(path), i));
            }
        }

        //protected virtual void Select_Image()
        //{
        //    if (GUILayout.Button("Load Image"))
        //    {
        //        //path = EditorUtility.OpenFilePanel("Load Tile Set", Tiles_Utility.Last_Opened_Dir, "");
        //        path = EditorUtility.OpenFilePanel("Load Tile Set", "", "");
        //        if (path != null && path != "" && File.Exists(path) && File.Exists(path) && path.Contains("A4"))
        //        {
        //            //Tiles_Utility.Last_Opened_Dir = System.IO.Path.GetDirectoryName(path);
        //            CutLayout();
        //        }
        //        else
        //        {
        //            if (path != null && path != "")
        //                EditorUtility.DisplayDialog("Selection error!", "You have to select a file or an A4 file compatibile with RPG MAKER tile set", "OK");
        //        }
        //    }
        //}

        public static void OnGUI()
        {
            //generate_sprite_sheet_image = GUILayout.Toggle(generate_sprite_sheet_image, "Generate Sprite Sheet Image");
            //Select_Image();

            if (img == null) return;

            GUILayout.Label("Select the tile you want to import, then click the 'Generate Tiles' Button");
            //can select or deselect all
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Select All"))
            {
                if (sub_blocks_wall_to_import != null)
                    for (int i = 0; i < sub_blocks_wall_to_import.Count; i++)
                        sub_blocks_wall_to_import[i] = true;

                if (sub_blocks_top_to_import != null)
                    for (int i = 0; i < sub_blocks_top_to_import.Count; i++)
                        sub_blocks_top_to_import[i] = true;
            }
            if (GUILayout.Button("Select None"))
            {
                if (sub_blocks_wall_to_import != null)
                    for (int i = 0; i < sub_blocks_wall_to_import.Count; i++)
                        sub_blocks_wall_to_import[i] = false;

                if (sub_blocks_top_to_import != null)
                    for (int i = 0; i < sub_blocks_top_to_import.Count; i++)
                        sub_blocks_top_to_import[i] = false;
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Generate Tiles", "Create the selected auto tiles saving the asset in the default folders")))
            {
                //generate the top tile. They are A2 style tile
                Generate_Tiles(path, sub_blocks_top, sub_blocks_top_to_import, sub_blocks_top_names,
                    mini_tile_w, mini_tile_h, wBlock, hBlock);

                A3_Tiles_Importer_Window.Generate_Wall_Tiles(path, sub_blocks_wall, sub_blocks_wall_to_import, sub_blocks_wall_names,
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

                        Generate_Tiles(path, sub_blocks_top, sub_blocks_top_to_import, sub_blocks_top_names,
                            mini_tile_w, mini_tile_h, wBlock, hBlock, sprite_sheet_path, auto_tile_path);

                        A3_Tiles_Importer_Window.Generate_Wall_Tiles(path, sub_blocks_wall, sub_blocks_wall_to_import, sub_blocks_wall_names,
                            mini_tile_w, mini_tile_h, wBlock, hBlock, sprite_sheet_path, auto_tile_path);
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
            int topCount = sub_blocks_top != null ? sub_blocks_top.Count : -1;
            int wallCount = sub_blocks_wall != null ? sub_blocks_wall.Count : -1;
            for (int i = 0; i < Math.Max(topCount, wallCount); i++)
            {
                if (i != 0 && i % 8 == 0)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
                GUILayout.BeginVertical();
                if (sub_blocks_top != null && sub_blocks_top.Count != 0)
                {
                    GUILayout.BeginVertical();
                    Texture2D sub_top = sub_blocks_top[i];
                    sub_blocks_top_to_import[i] = GUILayout.Toggle(sub_blocks_top_to_import[i],
                        new GUIContent(sub_top, "Click on the image to (un)toggle it"));
                    sub_blocks_top_names[i] = GUILayout.TextArea(sub_blocks_top_names[i], GUILayout.Width(84));
                    GUILayout.EndVertical();
                }

                if (sub_blocks_wall != null && sub_blocks_wall.Count != 0)
                {
                    GUILayout.BeginVertical();
                    Texture2D sub_wall = sub_blocks_wall[i];
                    sub_blocks_wall_to_import[i] = GUILayout.Toggle(sub_blocks_wall_to_import[i],
                        new GUIContent(sub_wall, "Click on the image to (un)toggle it"));
                    sub_blocks_wall_names[i] = GUILayout.TextArea(sub_blocks_wall_names[i], GUILayout.Width(84));
                    GUILayout.EndVertical();
                }

                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        public static void Generate_Tiles(string path, List<Texture2D> sub_blocks, List<bool> sub_blocks_to_import, List<string> names,
        int mini_tile_w, int mini_tile_h, int wBlock, int hBlock,
        string dest_sprite_sheet = "", string dest_auto_tile = "")
        {
            if (sub_blocks == null) return;
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

            //List<string> images_path = new List<string>();//list of the path of the impoted tiles
            string image_path = "";

            Dictionary<byte, int> rule_tiles = new Dictionary<byte, int>(); //dictionary for the tile rules

            //foreach sub pieces in the image
            for (int sub_block_count = 0; sub_block_count < sub_blocks.Count; sub_block_count++)
            {
                if (!sub_blocks_to_import[sub_block_count]) continue; //If the current sub is not selected to process than skip it

                int tiles_counter = 0; // counter to enumerate the sprite

                Texture2D sub_piece = sub_blocks[sub_block_count]; //get the texture            

                //temp array to store the sub mini tiles
                Texture2D[] bottom_left_mini_tiles, bottom_right_mini_tiles, top_left_mini_tiles, top_right_mini_tiles;

                //generate the mini tiles to the following computation
                Tiles_A2_Utility.Generate_Mini_Tile_A2(sub_piece, mini_tile_w, mini_tile_h, out bottom_left_mini_tiles, out bottom_right_mini_tiles, out top_left_mini_tiles, out top_right_mini_tiles);

                //if (generate_sprite_sheet_image)
                {
                    Texture2D sprite_tiles = new Texture2D(wBlock * 8, hBlock * 6);
                    int sprite_tile_width = wBlock * 8;
                    //string sprite_sheet_path = string.Format(@"{0}/_{1}_{2}.png", loaded_file_image_path, Path.GetFileNameWithoutExtension(path), sub_block_count);
                    //string sprite_sheet_path = Path.Combine(loaded_file_image_path,
                    string sprite_sheet_path = Path.Combine(path_for_sprite_sheet,
                        string.Format(@"{0}.png", names[sub_block_count]));

                    //generate and iterate the final tile for the subs pieces
                    foreach (KeyValuePair<byte, Texture2D> kvp in Tiles_A2_Utility.Generate_Final_Tiles_A2_Terrain(mini_tile_w, mini_tile_h,
                        bottom_left_mini_tiles, bottom_right_mini_tiles, top_left_mini_tiles, top_right_mini_tiles, rule_tiles))
                    {
                        int xx = tiles_counter % 8 * wBlock;
                        int yy = tiles_counter / 8 * hBlock;
                        sprite_tiles.SetPixels(xx, sprite_tiles.height - yy - hBlock, wBlock, hBlock, kvp.Value.GetPixels());
                        tiles_counter++;
                    }

                    //images_path.Add(sprite_sheet_path);
                    image_path = sprite_sheet_path;
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
                        for (int i = 0; i < 48; i++)
                        {
                            int xx = i % 8 * wBlock;
                            int yy = (i / 8 + 1) * hBlock;
                            SpriteMetaData smd = new SpriteMetaData();
                            smd = new SpriteMetaData
                            {
                                alignment = 0,
                                border = new Vector4(0, 0, 0, 0),
                                name = string.Format("{0}_{1:00}", names[sub_block_count], i),
                                pivot = new Vector2(.5f, .5f),
                                rect = new Rect(xx, sprite_tiles.height - yy, wBlock, hBlock)
                            };
                            tmps[i] = smd;
                        }
                        importer.spritesheet = tmps;
                        importer.SaveAndReimport();
                    }
                    AssetDatabase.Refresh();
                    Tiles_A4_Utility.Generate_A4_Tile_SS(path, image_path, path_for_auto_tile, rule_tiles, names[sub_block_count]);
                }
                //else //single tile image
                //{
                //    //create the directory for the final images
                //    string tile_folder_path = string.Format(@"{0}/_{1}_{2}", loaded_file_image_path, Path.GetFileNameWithoutExtension(path), sub_block_count);
                //    //add the path of the this that will contains alla the sub block final tiles
                //    images_path.Add(tile_folder_path);
                //    if (!Directory.Exists(tile_folder_path))
                //        Directory.CreateDirectory(tile_folder_path);
                //    //generate and iterate the final tile for the subs pieces
                //    foreach (KeyValuePair<byte, Texture2D> kvp in Tiles_A2_Utility.Generate_Final_Tiles_A2_Terrain(mini_tile_w, mini_tile_h,
                //        bottom_left_mini_tiles, bottom_right_mini_tiles, top_left_mini_tiles, top_right_mini_tiles, rule_tiles))
                //    {
                //        //save each final tile to its own image
                //        var tile_bytes = kvp.Value.EncodeToPNG();
                //        string tile_file_path = string.Format(@"{0}/_{1}_{2}_{3:000}.png", tile_folder_path,
                //            Path.GetFileNameWithoutExtension(path), sub_block_count, tiles_counter);

                //        File.WriteAllBytes(tile_file_path, tile_bytes);
                //        tiles_counter++;
                //    }
                //}
            }
            AssetDatabase.Refresh(); //refresh asset database

            //generate the A2_tile Auto tiles
            //for (int i = 0; i < images_path.Count; i += 1)
            //{
            //    string str = images_path[i];
            //    if (generate_sprite_sheet_image)
            //        Tiles_A4_Utility.Generate_A4_Tile_SS(path, str, rule_tiles);
            //    else
            //        Tiles_A4_Utility.Generate_A4_Tile(path, str, rule_tiles, wBlock);
            //}
        }
    }
}