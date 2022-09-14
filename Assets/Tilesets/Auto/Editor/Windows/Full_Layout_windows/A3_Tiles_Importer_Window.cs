#define DEBUG   
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace IGL_Tech.RPGM.Auto_Tile_Importer
{
    public class A3_Tiles_Importer_Window : EditorWindow
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
        protected static List<Texture2D> sub_blocks_wall;

        /// <summary>
        /// List of boolean to select the block to import
        /// </summary>
        protected static List<bool> sub_blocks_wall_to_import;

        protected static List<string> sub_blocks_wall_names;

        protected static string path;

        protected static int wBlock = 32, hBlock = 32;
        protected static int mini_tile_w = 16, mini_tile_h = 16;


        //[MenuItem("Tools/RPGM Importer/A3/A3 Full Layout")]
        //public static void ShowWindow()
        //{
        //    EditorWindow.GetWindow<A3_Tiles_Importer_Window>(false, "A3 Full Layout Impoter");
        //}

        public static void Cut_Layout(string path_to_slice)
        {
            path = path_to_slice;
            img = new Texture2D(1, 1);
            byte[] bytedata = File.ReadAllBytes(path);
            img.LoadImage(bytedata);

            //get the sliced part
            sub_blocks_wall = Tiles_A3_Utility.A3_Tile_Slice_File(img, out wBlock, out hBlock, out mini_tile_w, out mini_tile_h);
            sub_blocks_wall_to_import = new List<bool>();
            sub_blocks_wall_names = new List<string>();
            for (int i = 0; i < sub_blocks_wall.Count; i++)
            {
                sub_blocks_wall_to_import.Add(false);
                sub_blocks_wall_names.Add(string.Format("_{2}_{0}_{1}", Path.GetFileNameWithoutExtension(path), i, ((i / 8)) % 2 == 0 ? "R" : "W"));
            }
        }

        //protected virtual void Select_Image()
        //{
        //    if (GUILayout.Button("Load Image"))
        //    {
        //        //path = EditorUtility.OpenFilePanel("Load Tile Set", Tiles_Utility.Last_Opened_Dir, "");
        //        path = EditorUtility.OpenFilePanel("Load Tile Set", "", "");
        //        if (path != null && path != "" && File.Exists(path) && File.Exists(path) && path.Contains("A3"))
        //        {
        //            //Tiles_Utility.Last_Opened_Dir = System.IO.Path.GetDirectoryName(path);
        //            CutLayout();
        //        }
        //        else
        //        {
        //            if (path != null && path != "")
        //                EditorUtility.DisplayDialog("Selection error!", "You have to select a file or an A3 file compatibile with RPG MAKER tile set", "OK");
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
                for (int i = 0; i < sub_blocks_wall_to_import.Count; i++)
                    sub_blocks_wall_to_import[i] = true;
            }
            if (GUILayout.Button("Select None"))
            {
                for (int i = 0; i < sub_blocks_wall_to_import.Count; i++)
                    sub_blocks_wall_to_import[i] = false;
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Generate Tiles", "Create the selected auto tiles saving the asset in the default folders")))
            {
                ////generate waterfall tile style
                Generate_Wall_Tiles(path, sub_blocks_wall, sub_blocks_wall_to_import, sub_blocks_wall_names,
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
                        Generate_Wall_Tiles(path, sub_blocks_wall, sub_blocks_wall_to_import, sub_blocks_wall_names,
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
            for (int i = 0; i < sub_blocks_wall.Count; i++)
            {
                if (i != 0 && i % 8 == 0)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
                Texture2D sub = sub_blocks_wall[i];
                GUILayout.BeginVertical();
                sub_blocks_wall_to_import[i] = GUILayout.Toggle(sub_blocks_wall_to_import[i],
                    new GUIContent(sub, "Click on the image to (un)toggle it"));
                sub_blocks_wall_names[i] = GUILayout.TextArea(sub_blocks_wall_names[i], GUILayout.Width(84));
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.EndScrollView();
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
        public static void Generate_Wall_Tiles(string path, List<Texture2D> sub_blocks_wall, List<bool> sub_blocks_wall_to_import, List<string> names,
            int mini_tile_w, int mini_tile_h, int wBlock, int hBlock, string dest_sprite_sheet = "", string dest_auto_tile = "")
        {
            if (sub_blocks_wall == null) return;
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

            //List<string> images_path = new List<string>();//list of the folder of the imported tiles
            string image_path = "";

            Dictionary<byte, int> rule_tiles = new Dictionary<byte, int>();

            //foreach sub pieces in the image. If it's an animated auto tile 3 consecutive sub blocks are 3 frame of the animation
            for (int i = 0; i < sub_blocks_wall_to_import.Count; i++)
            {
                //If the current sub is not selected to process than skip it
                if (!sub_blocks_wall_to_import[i]) continue;

                int tiles_counter = 0; //set zero to che final tile counter
                Texture2D sub_piece = sub_blocks_wall[i];

                //temp array to store the sub mini tiles
                Texture2D[] bottom_left_mini_tiles, bottom_right_mini_tiles, top_left_mini_tiles, top_right_mini_tiles;
                Texture2D[,] raw_mini_tile;

                //generate the mini tiles to the following computation
                Tiles_A3_Utility.Generate_Mini_Tile_A3_Wall(sub_piece, mini_tile_w, mini_tile_h, out bottom_left_mini_tiles, out bottom_right_mini_tiles, out top_left_mini_tiles, out top_right_mini_tiles,
                    out raw_mini_tile);

                //if (generate_sprite_sheet_image)
                {
                    Texture2D sprite_tiles = new Texture2D(wBlock * 8, hBlock * 2);
                    //string sprite_sheet_path = Path.Combine(loaded_file_image_path,
                    string sprite_sheet_path = Path.Combine(path_for_sprite_sheet,
                        string.Format(@"{0}.png", names[i]));


                    //generate and iterate the final tile for the subs pieces
                    foreach (KeyValuePair<byte, Texture2D> kvp in Tiles_A3_Utility.Generate_Final_Tiles_A3_Wall(mini_tile_w, mini_tile_h,
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
                        importer.maxTextureSize = sprite_tiles.width;
                        SpriteMetaData[] tmps = new SpriteMetaData[8 * 2];
                        for (int j = 0; j < 16; j++)
                        {
                            int xx = j % 8 * wBlock;
                            int yy = (j / 8 + 1) * hBlock;
                            SpriteMetaData smd = new SpriteMetaData();
                            smd = new SpriteMetaData
                            {
                                alignment = 0,
                                border = new Vector4(0, 0, 0, 0),
                                name = string.Format("{0}_{1:00}", names[i], j),
                                pivot = new Vector2(.5f, .5f),
                                rect = new Rect(xx, sprite_tiles.height - yy, wBlock, hBlock)
                            };
                            tmps[j] = smd;
                        }
                        importer.spritesheet = tmps;
                        importer.SaveAndReimport();
                    }
                    AssetDatabase.Refresh();
                    Tiles_A3_Utility.Generate_A3_Wall_tile_SS(path, image_path, path_for_auto_tile, rule_tiles, names[i]);
                }
                //else {
                //    //create the directory for the final images
                //    string tile_folder_path = string.Format(@"{0}/_Wall_{1}_{2}", loaded_file_image_path, Path.GetFileNameWithoutExtension(path), i);
                //    //add the path of the this that will contains alla the sub block final tiles
                //    images_path.Add(tile_folder_path);
                //    if (!Directory.Exists(tile_folder_path))
                //        Directory.CreateDirectory(tile_folder_path);

                //    //generate and iterate the final tile for the subs pieces
                //    foreach (KeyValuePair<byte, Texture2D> kvp in Tiles_A3_Utility.Generate_Final_Tiles_A3_Wall(mini_tile_w, mini_tile_h,
                //        bottom_left_mini_tiles, bottom_right_mini_tiles, top_left_mini_tiles, top_right_mini_tiles, rule_tiles))
                //    {
                //        //save each final tile to its own image
                //        var tile_bytes = kvp.Value.EncodeToPNG();
                //        string tile_file_path = string.Format(@"{0}/_Water_{1}_{2}_{3:000}.png", tile_folder_path,
                //            Path.GetFileNameWithoutExtension(path), i, tiles_counter);

                //        File.WriteAllBytes(tile_file_path, tile_bytes);
                //        tiles_counter++;
                //    }
                //}

            }
            AssetDatabase.Refresh(); //refresh asset database

            //generate the fixed Auto tiles
            //for (int i = 0; i < images_path.Count; i ++)
            //{
            //    if (generate_sprite_sheet_image)
            //    {
            //        Tiles_A3_Utility.Generate_A3_Wall_tile_SS(path, images_path[i], rule_tiles);
            //    }
            //    //else
            //    //    Tiles_A3_Utility.Generate_A3_Wall_Tile(path, images_path[i], rule_tiles, wBlock);
            //}
        }
    }
}