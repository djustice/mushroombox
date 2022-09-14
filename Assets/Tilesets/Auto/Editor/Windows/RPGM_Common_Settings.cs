using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace IGL_Tech.RPGM.Auto_Tile_Importer
{
    public class RPGM_Common_Settings : EditorWindow
    {
        static string imgPath, tilePath;
        [MenuItem("Tools/RPGM Importer/Auto Tile/Settings")]
        public static void ShowWindow()
        {
            var tmp = EditorWindow.GetWindow<RPGM_Common_Settings>() as RPGM_Common_Settings;
            tmp.minSize = new Vector2(1024, 100);
            tmp.maxSize = new Vector2(1920, 100);
            Tiles_Utility.Init_Dir_Values();
            imgPath = Tiles_Utility.Final_image_folder_path;
            tilePath = Tiles_Utility.Auto_Tile_Folder_Path;
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Select folder for images", GUILayout.Width(156)))
            {
                imgPath = EditorUtility.OpenFolderPanel("Load png Textures", "", "");
                imgPath = imgPath.Substring(imgPath.IndexOf("Assets"));
            }
            GUILayout.Label(string.Format("Destination path for generated Images: {0}", imgPath));
            GUILayout.EndHorizontal();
            //final auto tile path

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Select folder for tiles", GUILayout.Width(156)))
            {
                tilePath = EditorUtility.OpenFolderPanel("Load png Textures", "", "");
                tilePath = tilePath.Substring(tilePath.IndexOf("Assets"));
            }
            GUILayout.Label(string.Format("Destination path for generated Auto_Tiles: {0}", tilePath));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save"))
            {
                Tiles_Utility.Final_image_folder_path = imgPath;
                Tiles_Utility.Auto_Tile_Folder_Path = tilePath;
            }
            if (GUILayout.Button("Reset"))
            {
                Tiles_Utility.Auto_Tile_Folder_Path = Tiles_Utility.Final_image_folder_path = "";
                Tiles_Utility.Init_Dir_Values();
                imgPath = Tiles_Utility.Final_image_folder_path;
                tilePath = Tiles_Utility.Auto_Tile_Folder_Path;
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Create directories"))
            {
                if (!Directory.Exists(imgPath))
                {
                    Directory.CreateDirectory(imgPath);
                }
                if (!Directory.Exists(tilePath))
                {
                    Directory.CreateDirectory(tilePath);
                }

                AssetDatabase.Refresh();
            }
        }
    }
}