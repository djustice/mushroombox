#define DEBUG   
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace IGL_Tech.RPGM.Auto_Tile_Importer
{
    public class RPGMXP_Water_Impoter_Windows : A1_Single_Water_Impoter
    {
        //[MenuItem("Tools/RPGM XP/Water Auto Tile")]
        //public new static void ShowWindow()
        //{
        //    EditorWindow.GetWindow<RPGMXP_Water_Impoter_Windows>(false, "Water Importer");
        //}

        //protected override void Select_Image()
        //{
        //    if (GUILayout.Button("Load Image"))
        //    {
        //        path = EditorUtility.OpenFilePanel("Load Tile Set", "", "");
        //        if (path != null && path != "" && File.Exists(path))
        //        {
        //            //CutLayout();
        //        }
        //        else
        //        {
        //            if (path != null && path != "")
        //                EditorUtility.DisplayDialog("Selection error!", "You have to select a file or an A1 file compatibile with RPG MAKER tile set", "OK");
        //        }
        //    }
        //}

        public static new void Cut_Layout(string path_to_slice)
        {
            path = path_to_slice;
            img = new Texture2D(1, 1);
            byte[] bytedata = File.ReadAllBytes(path);
            img.LoadImage(bytedata);

            RPGM_XP_Utility.RPGM_XP_Water_Slice(img, out wBlock, out hBlock, out mini_tile_w, out mini_tile_h, out sub_blocks_water);
            sub_blocks_water_to_import = new List<bool>();
            sub_blocks_water_names = new List<string>();
            //for (int i = 0; i < sub_blocks_water.Count / 3; i++)
            {
                sub_blocks_water_to_import.Add(false);
                sub_blocks_water_names.Add(string.Format("_XPW_{0}", Path.GetFileNameWithoutExtension(path)));
            }

            sub_blocks_floating = new List<Texture2D>();
            sub_blocks_twister = new List<Texture2D>();
        }

        public static new void OnGui()
        {
            animFrame = 5;
            //Select_Image();

            if (img == null) return;

            GUILayout.Label("Select the tile you want to import, then click the 'Generate Tiles' Button");

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Generate Tiles", "Create the selected auto tiles saving the asset in the default folders")))
            {
                //generate the animated A1 style tile
                Generate_Water_Tiles(path, sub_blocks_water, sub_blocks_water_to_import, sub_blocks_water_names,
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
                        Generate_Water_Tiles(path, sub_blocks_water, sub_blocks_water_to_import, sub_blocks_water_names,
                            mini_tile_w, mini_tile_h, wBlock, hBlock, sprite_sheet_path, auto_tile_path);
                    }
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginVertical(GUILayout.Height(Screen.height / 3 * 2));
            if (sub_blocks_water != null && sub_blocks_water.Count != 0)
            {
                int frame = (frameCount / 100) % animFrame;
                frame = (animFrame == 4) ? (frame < 3 ? frame : 1) : ((frameCount / 100) % 4);
                GUILayout.Space(10);
                GUILayout.Label("Animated Water Tile");
                GUILayout.BeginHorizontal();
                for (int i = 0; i < sub_blocks_water.Count; i += (animFrame - 1))
                {
                    GUILayout.BeginVertical();
                    Texture2D sub_water = sub_blocks_water[i + frame];
                    sub_blocks_water_to_import[i / (animFrame - 1)] = GUILayout.Toggle(sub_blocks_water_to_import[i / (animFrame - 1)],
                        new GUIContent(sub_water, "Click on the image to (un)toggle it"));
                    sub_blocks_water_names[i / (animFrame - 1)] = GUILayout.TextArea(sub_blocks_water_names[i / (animFrame - 1)], GUILayout.Width(84));
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
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
        public static new void Generate_Water_Tiles(string path, List<Texture2D> sub_blocks_water, List<bool> sub_blocks_water_to_import, List<string> names,
            int mini_tile_w, int mini_tile_h, int wBlock, int hBlock,
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
            string fileName = Path.GetFileNameWithoutExtension(path);
            //string loaded_file_image_path = string.Format(@"{0}/_{1}", Tiles_Utility.Final_image_folder_path, fileName); //ex rtp_import\Outside_A2\single_block_folder\final_tile\Image
            //string loaded_file_image_path = Path.Combine(Tiles_Utility.Final_image_folder_path, string.Format(@"_{0}", fileName));
            //if (!Directory.Exists(loaded_file_image_path))
            //    Directory.CreateDirectory(loaded_file_image_path);

            List<string> images_path = new List<string>();//list of the folder of the imported tiles

            Dictionary<byte, int> rule_tiles = new Dictionary<byte, int>();

            //foreach sub pieces in the image. If it's an animated auto tile 3 consecutive sub blocks are 3 frame of the animation
            for (int sub_block_count = 0; sub_block_count < sub_blocks_water_to_import.Count; sub_block_count++)
            {
                //If the current sub is not selected to process than skip it
                if (!sub_blocks_water_to_import[sub_block_count]) continue;

                for (int i = sub_block_count * 4; i < sub_block_count * 4 + 4; i++)
                {
                    int tiles_counter = 0; //set zero to che final tile counter
                    Texture2D sub_piece = sub_blocks_water[i];

                    //temp array to store the sub mini tiles
                    Texture2D[] bottom_left_mini_tiles, bottom_right_mini_tiles, top_left_mini_tiles, top_right_mini_tiles;

                    //generate the mini tiles to the following computation
                    RPGM_XP_Utility.Generate_Mini_Tile_RPGMXP(sub_piece, mini_tile_w, mini_tile_h, out bottom_left_mini_tiles, out bottom_right_mini_tiles, out top_left_mini_tiles, out top_right_mini_tiles);

                    int wb = 8, hb = 7;
                    Texture2D sprite_tiles = new Texture2D(wBlock * wb, hBlock * hb);
                    int sprite_tile_width = Mathf.Max(wBlock * wb, wBlock * hb);
                    //string sprite_sheet_path = string.Format(@"{0}/_Water_{1}_{2}.png", loaded_file_image_path, Path.GetFileNameWithoutExtension(path), i);
                    //string sprite_sheet_path = Path.Combine(loaded_file_image_path,
                    string sprite_sheet_path = Path.Combine(path_for_sprite_sheet,
                        string.Format(@"{0}_{1}.png", names[sub_block_count], i - sub_block_count * 4));

                    //generate and iterate the final tile for the subs pieces
                    foreach (KeyValuePair<byte, Texture2D> kvp in RPGM_XP_Utility.Generate_Final_Tiles_RPGMXP(mini_tile_w, mini_tile_h,
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
                        SpriteMetaData[] tmps = new SpriteMetaData[wb * hb];
                        for (int j = 0; j < hb * wb; j++)
                        {
                            int xx = j % 8 * wBlock;
                            int yy = (j / 8 + 1) * hBlock;
                            SpriteMetaData smd = new SpriteMetaData();
                            smd = new SpriteMetaData
                            {
                                alignment = 0,
                                border = new Vector4(0, 0, 0, 0),
                                name = string.Format("{0}_{2}_{1:00}", names[sub_block_count], j, i),
                                pivot = new Vector2(.5f, .5f),
                                rect = new Rect(xx, sprite_tiles.height - yy, wBlock, hBlock)
                            };
                            tmps[j] = smd;
                        }
                        importer.spritesheet = tmps;
                        importer.SaveAndReimport();
                    }
                }
                AssetDatabase.Refresh();
                RPGM_XP_Utility.Generate_Water_Tile_SS(path, path_for_auto_tile,
                    images_path[0], images_path[1], images_path[2], images_path[3], rule_tiles, wBlock,
                    names[sub_block_count]);
                images_path.Clear();
            }
            AssetDatabase.Refresh(); //refresh asset database

            ////generate the fixed Auto tiles
            //for (int i = 0; i < images_path.Count; i += 4)
            //{
            //    RPGM_XP_Utility.Generate_Water_Tile_SS(path, images_path[i], images_path[i + 1], images_path[i + 2], images_path[i + 3], rule_tiles, wBlock);
            //}
        }
    }
}