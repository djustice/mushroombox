#define DEBUG   
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace IGL_Tech.RPGM.Auto_Tile_Importer
{
    public class RPGMXP_Terrain_Impoter_Windows : A2_Single_Terrain_Importer
    {

        //[MenuItem("Tools/RPGM XP/Terrain Auto Tile")]
        //public new static void ShowWindow()
        //{
        //    EditorWindow.GetWindow<RPGMXP_Terrain_Impoter_Windows>(false, "Terrain Importer");
        //}

        public static new void Cut_Layout(string path_to_slice)
        {
            path = path_to_slice;
            img = new Texture2D(1, 1);
            byte[] bytedata = File.ReadAllBytes(path);
            img.LoadImage(bytedata);

            //img = RPGM_XP_Utility.VXACE_2_XP(tmp);

            //sub block slicing
            //get the sliced part
            Vector2Int sub_size = new Vector2Int(img.width, img.height); //that is a fixed number of blocks
                                                                         //divide in sub blocks

            mini_tile_w = sub_size.x / 6;
            wBlock = mini_tile_w * 2;
            mini_tile_h = sub_size.y / 8;
            hBlock = mini_tile_h * 2;

            sub_blocks = new List<Texture2D>();
            sub_blocks.Add(img);

            sub_blocks_to_import = new List<bool>();
            sub_block_names = new List<string>();
            //for (int i = 0; i < sub_blocks.Count; i++)
            {
                sub_blocks_to_import.Add(false);
                sub_block_names.Add(string.Format("_XPT_{0}", Path.GetFileNameWithoutExtension(path)));
            }
        }

        public new static void OnGUI()
        {
            if (img == null) return;

            GUILayout.Label("Select the tile you want to import, then click the 'Generate Tiles' Button");

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Generate Tiles", "Create the selected auto tiles saving the asset in the default folders")))
            {
                //generate the final tiles for the tile palette
                Generate_Tiles(path, sub_blocks, sub_blocks_to_import, sub_block_names, mini_tile_w, mini_tile_h, wBlock, hBlock);
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
                        Generate_Tiles(path, sub_blocks, sub_blocks_to_import, sub_block_names, mini_tile_w, mini_tile_h, wBlock, hBlock,
                            sprite_sheet_path, auto_tile_path);
                    }
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginVertical(GUILayout.Height(Screen.height / 3));
            GUILayout.BeginHorizontal();
            for (int i = 0; i < sub_blocks.Count; i++)
            {
                var sub = sub_blocks[i];
                GUILayout.BeginVertical();
                //toggle to select the sub block of the image
                sub_blocks_to_import[i] = GUILayout.Toggle(sub_blocks_to_import[i],
                    new GUIContent(sub, "Click on the image to (un)toggle it"));
                sub_block_names[i] = GUILayout.TextArea(sub_block_names[i], GUILayout.Width(84));
                GUILayout.EndVertical();

                if (i % 8 == 0)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        //protected override void Select_Image()
        //{
        //    if (GUILayout.Button("Load Image")) //open gile dialog to load the image
        //    {
        //        path = EditorUtility.OpenFilePanel("Load Tile Set", "", "");
        //        if (path != null && path != "" && File.Exists(path))
        //        {
        //            CutLayout();
        //        }
        //        else
        //        {
        //            if (path != null && path != "")
        //                EditorUtility.DisplayDialog("Selection error!", "You have to select a file or an A2 file compatibile with RPG MAKER tile set", "OK");
        //        }
        //    }
        //}

        public static new void Generate_Tiles(string path, List<Texture2D> sub_blocks, List<bool> sub_blocks_to_import, List<string> names,
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
                RPGM_XP_Utility.Generate_Mini_Tile_RPGMXP(sub_piece, mini_tile_w, mini_tile_h, out bottom_left_mini_tiles, out bottom_right_mini_tiles, out top_left_mini_tiles, out top_right_mini_tiles);

                int wb = 8, hb = 7;
                Texture2D sprite_tiles = new Texture2D(wBlock * wb, hBlock * hb);
                int sprite_tile_width = Mathf.Max(wBlock * wb, wBlock * hb);
                //string sprite_sheet_path = string.Format(@"{0}/_{1}_{2}.png", loaded_file_image_path, Path.GetFileNameWithoutExtension(path), sub_block_count);
                //string sprite_sheet_path = Path.Combine(loaded_file_image_path,
                string sprite_sheet_path = Path.Combine(path_for_sprite_sheet,
                    string.Format(@"{0}.png", names[sub_block_count]));

                //generate and iterate the final tile for the subs pieces
                foreach (KeyValuePair<byte, Texture2D> kvp in RPGM_XP_Utility.Generate_Final_Tiles_RPGMXP(mini_tile_w, mini_tile_h,
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
                EditorUtility.SetDirty(AssetDatabase.LoadAssetAtPath<Sprite>(sprite_sheet_path));
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
                    for (int i = 0; i < wb * hb; i++)
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
                Tiles_A2_Utility.Generate_A2_Tile_SS(path, image_path, path_for_auto_tile, rule_tiles, names[sub_block_count]);
            }
            AssetDatabase.Refresh(); //refresh asset database
        }
    }
}