using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Linq;

namespace IGL_Tech.RPGM.Auto_Tile_Importer
{
    public class Tiles_Utility
    {

        private static string last_opened_dir = "", final_image_folder_path = "", auto_tile_folder_path = "";
        private static string last_dir = ".";
        /// <summary>
        /// Get or Set the last directory 
        /// </summary>
        public static string Last_Opened_Dir
        {
            get
            {
                Init_Dir_Values();
                return last_dir;
            }
            set
            {
                last_dir = value;
                EditorPrefs.SetString("RPGM_IMP_Last_Dir", last_dir);
            }
        }

        /// <summary>
        /// Path where the script will save the final image of the tileset
        /// </summary>
        public static string Final_image_folder_path
        {
            get
            {
                Init_Dir_Values();
                return final_image_folder_path;
            }
            set
            {
                final_image_folder_path = value;
                EditorPrefs.SetString("RPGM_IMP_Img_Path", final_image_folder_path);
            }
        }

        /// <summary>
        /// Path where the script will save the generated Auto Tile
        /// </summary>
        public static string Auto_Tile_Folder_Path
        {
            get
            {
                Init_Dir_Values();
                return auto_tile_folder_path;
            }
            set
            {
                auto_tile_folder_path = value;
                EditorPrefs.SetString("RPGM_IMP_AutoTile_Path", auto_tile_folder_path);
            }
        }

        /// <summary>
        /// Init the value for the other method
        /// </summary>
        public static void Init_Dir_Values()
        {
            last_opened_dir = EditorPrefs.GetString("RPGM_IMP_Last_Dir");
            if (last_opened_dir == null || last_opened_dir.Length == 0)
            {
                last_opened_dir = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                EditorPrefs.SetString("RPGM_IMP_Last_Dir", last_opened_dir);
            }
            final_image_folder_path = EditorPrefs.GetString("RPGM_IMP_Img_Path");
            if (final_image_folder_path == null || final_image_folder_path.Length == 0)
            {
                final_image_folder_path = Path.Combine("Assets", "RPG_Maker_Tiles_Importer", "_rtp_import");
                EditorPrefs.SetString("RPGM_IMP_Img_Path", final_image_folder_path);
            }
            auto_tile_folder_path = EditorPrefs.GetString("RPGM_IMP_AutoTile_Path");
            if (auto_tile_folder_path == null || auto_tile_folder_path.Length == 0)
            {
                auto_tile_folder_path = Path.Combine("Assets", "RPG_Maker_Tiles_Importer", "_rtp_auto_tiles");
                EditorPrefs.SetString("RPGM_IMP_AutoTile_Path", auto_tile_folder_path);
            }
        }

        /// <summary>
        /// Generate all the byte from 0 to 255
        /// </summary>
        /// <returns></returns>
        public static List<byte> ByteCombination()
        {
            List<byte> list = new List<byte>();
            for (int i = 0; i < 256; i++)
                list.Add((byte)i);
            return list;
        }

        /// <summary>
        /// Generate the 4 combination for the left and right cell only
        /// </summary>
        /// <returns></returns>
        public static List<byte> Left_Right_Combination()
        {
            List<byte> list = new List<byte>();
            byte b1 = 0;

            byte b2 = 0;
            b2 |= (1 << 2);

            byte b3 = 0;
            b3 |= (1 << 6);

            byte b4 = 0;
            b4 |= (1 << 2);
            b4 |= (1 << 6);

            list.Add(b1);
            list.Add(b2);
            list.Add(b3);
            list.Add(b4);
            return list;
        }

        /// <summary>
        /// Generate the combination for the wall tile
        /// </summary>
        /// <returns></returns>
        public static List<byte> Wall_Combination()
        {
            List<byte> list = new List<byte>();
            for (int i = 0; i < 16; i++)
                list.Add(0);
            list[1] |= (0 << 4) | (0 << 6) | (0 << 2) | (1 << 0); //0001
            list[2] |= (0 << 4) | (0 << 6) | (1 << 2) | (0 << 0); //0010
            list[3] |= (0 << 4) | (0 << 6) | (1 << 2) | (1 << 0); //0011
            list[4] |= (0 << 4) | (1 << 6) | (0 << 2) | (0 << 0); //0100
            list[5] |= (0 << 4) | (1 << 6) | (0 << 2) | (1 << 0); //0101
            list[6] |= (0 << 4) | (1 << 6) | (1 << 2) | (0 << 0); //0110
            list[7] |= (0 << 4) | (1 << 6) | (1 << 2) | (1 << 0); //0111
            list[8] |= (1 << 4) | (0 << 6) | (0 << 2) | (0 << 0); //1000
            list[9] |= (1 << 4) | (0 << 6) | (0 << 2) | (1 << 0); //1001
            list[10] |= (1 << 4) | (0 << 6) | (1 << 2) | (0 << 0); //1010
            list[11] |= (1 << 4) | (0 << 6) | (1 << 2) | (1 << 0); //1011
            list[12] |= (1 << 4) | (1 << 6) | (0 << 2) | (0 << 0); //1100
            list[13] |= (1 << 4) | (1 << 6) | (0 << 2) | (1 << 0); //1101
            list[14] |= (1 << 4) | (1 << 6) | (1 << 2) | (0 << 0); //1110
            list[15] |= (1 << 4) | (1 << 6) | (1 << 2) | (1 << 0); //1111

            return list;
        }

        /// <summary>
        /// Set the import setting for the auto contains image tile
        /// </summary>
        /// <param name="fTile"></param>
        /// <param name="wBlock"></param>
        public static void Set_Impoter_Settings(string fTile, int wBlock, int h_tile = 8)
        {
            TextureImporter importer = AssetImporter.GetAtPath(fTile) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.filterMode = FilterMode.Point;
                importer.spritePixelsPerUnit = wBlock;
                importer.compressionQuality = 0;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.maxTextureSize = wBlock * h_tile;
                importer.SaveAndReimport();
            }
        }

        /// <summary>
        /// Select a single bit from the byte
        /// </summary>
        /// <param name="b">the byte</param>
        /// <param name="bitNumber">bit numeber, form 1 to 8</param>
        /// <returns></returns>
        public static bool GetBit(byte b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }

        /// <summary>
        /// Return an ordered list of the Loaded Sprite Sheet to build the auto tile
        /// </summary>
        /// <param name="tile_path"></param>
        /// <returns></returns>
        public static List<UnityEngine.Object> LoadSpriteSheet(string tile_path)
        {
            return AssetDatabase.LoadAllAssetsAtPath(tile_path).OrderBy(p => p.name).ToList();
        }
    }
}