using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace IGL_Tech.RPGM.Auto_Tile_Importer
{
    public class RPGM_Auto_Tile_Importer : EditorWindow
    {
        public enum Auto_Tile_Enum
        {
            A1_Full_Layout = 1,
            A2_Full_Layout = 2,
            A3_Full_Layout = 3,
            A4_Full_Layout = 4,
            A1_Single_Floating_Element = 5,
            A1_Single_Waterfall = 6,
            A1_Single_Animated_Water = 7,
            A2_Single_Terrain = 8,
            A3_Single_Wall_or_Ceiling = 9,
            A4_Single_Wall = 10,
            A4_Single_Rooftop = 11,
            A5 = 12,
            RPGM_XP_Water = 13,
            RPGM_XP_Terrain = 14
        }

        string image_path;

        Auto_Tile_Enum auto_tile_type = Auto_Tile_Enum.A1_Full_Layout;

        [MenuItem("Tools/RPGM Importer/Auto Tile/Importer")]
        public static void ShowWindow()
        {
            var tmp = EditorWindow.GetWindow<RPGM_Auto_Tile_Importer>() as RPGM_Auto_Tile_Importer;
            Tiles_Utility.Init_Dir_Values();
        }

        protected virtual void CutLayout(string path)
        {
            switch (auto_tile_type)
            {
                case Auto_Tile_Enum.A1_Full_Layout:
                    {
                        A1_Tiles_Importer_Window.Cut_Layout(path);
                    }
                    break;
                case Auto_Tile_Enum.A2_Full_Layout:
                    {
                        A2_Tiles_Importer_Window.Cut_Layout(path);
                    }
                    break;
                case Auto_Tile_Enum.A3_Full_Layout:
                    {
                        A3_Tiles_Importer_Window.Cut_Layout(path);
                    }
                    break;
                case Auto_Tile_Enum.A4_Full_Layout:
                    {
                        A4_Tiles_Importer_Window.Cut_Layout(path);
                    }
                    break;
                case Auto_Tile_Enum.A1_Single_Animated_Water:
                    {
                        A1_Single_Water_Impoter.Cut_Layout(path);
                    }
                    break;
                case Auto_Tile_Enum.A1_Single_Floating_Element:
                    {
                        A1_Single_Floating_Importer.Cut_Layout(path);
                    }
                    break;
                case Auto_Tile_Enum.A1_Single_Waterfall:
                    {
                        A1_Single_Twister_Importer.Cut_Layout(path);
                    }
                    break;
                case Auto_Tile_Enum.A2_Single_Terrain:
                    {
                        A2_Single_Terrain_Importer.Cut_Layout(path);
                    }
                    break;
                case Auto_Tile_Enum.A3_Single_Wall_or_Ceiling:
                    {
                        A3_Single_Impoter.Cut_Layout(path);
                    }
                    break;
                case Auto_Tile_Enum.A4_Single_Rooftop:
                    {
                        A4_Single_Ceiling_Impoter.Cut_Layout(path);
                    }
                    break;
                case Auto_Tile_Enum.A4_Single_Wall:
                    {
                        A4_Single_Wall_Impoter.Cut_Layout(path);
                    }
                    break;
                case Auto_Tile_Enum.A5:
                    {
                        A5_Tiles_Importer_Window.Cut_Layout(path);
                    }
                    break;
                case Auto_Tile_Enum.RPGM_XP_Water:
                    {
                        RPGMXP_Water_Impoter_Windows.Cut_Layout(path);
                    }
                    break;
                case Auto_Tile_Enum.RPGM_XP_Terrain:
                    {
                        RPGMXP_Terrain_Impoter_Windows.Cut_Layout(path);
                    }
                    break;
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal(GUILayout.Width(250));
            GUILayout.Label("Auto Tile Layout: ");
            Auto_Tile_Enum tmp_auto_tile_type = (Auto_Tile_Enum)EditorGUILayout.EnumPopup(auto_tile_type, GUILayout.Width(200));
            if (tmp_auto_tile_type != auto_tile_type)
            {
                auto_tile_type = tmp_auto_tile_type;
                //reslice the file
                if (image_path != null && image_path.Length != 0 && File.Exists(image_path))
                    CutLayout(image_path);
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Load Image"))
            {
                //path = EditorUtility.OpenFilePanel("Load Tile Set", Tiles_Utility.Last_Opened_Dir, "");
                image_path = EditorUtility.OpenFilePanel("Load Tile Set", Tiles_Utility.Last_Opened_Dir, "");
                if (image_path != null && image_path != "" && File.Exists(image_path))
                {
                    Tiles_Utility.Last_Opened_Dir = Path.GetDirectoryName(image_path);
                    //Tiles_Utility.Last_Opened_Dir = System.IO.Path.GetDirectoryName(path);
                    CutLayout(image_path);
                }
            }

            switch (auto_tile_type)
            {
                case Auto_Tile_Enum.A1_Full_Layout:
                    {
                        A1_Tiles_Importer_Window.OnGui();
                    }
                    break;
                case Auto_Tile_Enum.A2_Full_Layout:
                    {
                        A2_Tiles_Importer_Window.OnGUI();
                    }
                    break;
                case Auto_Tile_Enum.A3_Full_Layout:
                    {
                        A3_Tiles_Importer_Window.OnGUI();
                    }
                    break;
                case Auto_Tile_Enum.A4_Full_Layout:
                    {
                        A4_Tiles_Importer_Window.OnGUI();
                    }
                    break;
                case Auto_Tile_Enum.A1_Single_Animated_Water:
                    {
                        A1_Single_Water_Impoter.OnGui();
                    }
                    break;
                case Auto_Tile_Enum.A1_Single_Floating_Element:
                    {
                        A1_Single_Floating_Importer.OnGui();
                    }
                    break;
                case Auto_Tile_Enum.A1_Single_Waterfall:
                    {
                        A1_Single_Twister_Importer.OnGui();
                    }
                    break;
                case Auto_Tile_Enum.A2_Single_Terrain:
                    {
                        A2_Single_Terrain_Importer.OnGUI();
                    }
                    break;
                case Auto_Tile_Enum.A3_Single_Wall_or_Ceiling:
                    {
                        A3_Single_Impoter.OnGUI();
                    }
                    break;
                case Auto_Tile_Enum.A4_Single_Rooftop:
                    {
                        A4_Single_Ceiling_Impoter.OnGUI();
                    }
                    break;
                case Auto_Tile_Enum.A4_Single_Wall:
                    {
                        A4_Single_Wall_Impoter.OnGUI();
                    }
                    break;
                case Auto_Tile_Enum.A5:
                    {
                        A5_Tiles_Importer_Window.OnGUI();
                    }
                    break;
                case Auto_Tile_Enum.RPGM_XP_Water:
                    {
                        RPGMXP_Water_Impoter_Windows.OnGui();
                    }
                    break;
                case Auto_Tile_Enum.RPGM_XP_Terrain:
                    {
                        RPGMXP_Terrain_Impoter_Windows.OnGUI();
                    }
                    break;
            }
        }

        protected virtual void Update()
        {
            switch (auto_tile_type)
            {
                case Auto_Tile_Enum.A1_Full_Layout:
                case Auto_Tile_Enum.A1_Single_Animated_Water:
                case Auto_Tile_Enum.A1_Single_Waterfall:
                case Auto_Tile_Enum.RPGM_XP_Water:
                    {
                        A1_Tiles_Importer_Window.Update();
                    }
                    break;
            }
            Repaint();
        }
    }
}