﻿/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using InfinityCode.UltimateEditorEnhancer.Attributes;
using InfinityCode.UltimateEditorEnhancer.Behaviors;
using InfinityCode.UltimateEditorEnhancer.Windows;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InfinityCode.UltimateEditorEnhancer.SceneTools.QuickAccessActions
{
    [Title("Open Scene")]
    public class OpenAction : QuickAccessAction
    {
        private GUIContent _content;

        public override GUIContent content
        {
            get
            {
                if (_content == null)
                {
                    _content = new GUIContent(Icons.open, "Open (Left click - open, right - open additively)");
                }

                return _content;
            }
        }

        public override void OnClick()
        {
            bool additive = Event.current.button == 1;
            List<SceneHistoryItem> items = ReferenceManager.sceneHistory;
            GenericMenuEx menu = GenericMenuEx.Start();

            EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;
            StaticStringBuilder.Clear();
            for (int i = 0; i < buildScenes.Length; i++)
            {
                EditorBuildSettingsScene buildScene = buildScenes[i];
                SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(buildScene.path);

                StaticStringBuilder.Append("Scenes in Build/").Append(scene.name);
                menu.Add(StaticStringBuilder.GetString(true), () => OpenScene(buildScene.path, additive));
            }

            if (menu.count > 0) menu.AddSeparator();

            for (int i = 0; i < items.Count; i++)
            {
                SceneHistoryItem item = items[i];
                menu.Add(item.name, () => OpenScene(item.path, additive));
            }

            menu.Show();

            Event.current.Use();
        }

        private void OpenScene(string path, bool additive)
        {
            if (additive)
            {
                EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
                return;
            }

            if (SceneManagerHelper.AskForSave(SceneManager.GetActiveScene()))
            {
                SelectionHistory.Clear();
                EditorSceneManager.OpenScene(path);
            }
        }

        public override void ResetContent()
        {
            _content = null;
        }

        public override bool Validate()
        {
            return !EditorApplication.isPlaying;
        }
    }
}