/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.Tools
{
    public static class ToolbarTrialIcon
    {
        private static GUIContent content;
        private static GUIStyle style;

        private static void DrawIcon()
        {
            if (style == null)
            {
                style = new GUIStyle
                {
                    margin = new RectOffset(10, 0, 2, 0) 
                };
            }

            if (content == null)
            {
                UpdateContent();
            }

            if (GUILayout.Button(content, style, GUILayout.ExpandWidth(false), GUILayout.Height(18)))
            {
                Links.OpenAssetStore();
            }
        }

        public static void Init()
        {
            ToolbarManager.AddRightToolbar("Trial-NonCommerce", DrawIcon, int.MaxValue);
        }

        public static void UpdateContent()
        {
            if (TrialManager.isTrial)
            {
                if (TrialManager.daysLeft > 0)
                {
                    content = new GUIContent(
                        Resources.LoadIcon("Trial"), 
                        "Trial version of Ultimate Editor Enhancer.\nDays left: " + TrialManager.daysLeft + " days.\nMore details in \"Trial and Non-commercial EULA.txt\"."
                    );
                }
                else
                {
                    content = new GUIContent(
                        Resources.LoadIcon("Non-Commerce"),
                        "The trial period of Ultimate Editor Enhancer is over.\nUse in commercial projects is prohibited.\nPlease buy Ultimate Editor Enhancer in Unity Asset Store or remove the asset from the project."
                    );
                    if (!TrialManager.trialFinishedShown)
                    {
                        EditorApplication.delayCall += TrialManager.ShowTrialFinished;
                    }
                }
            }
            else
            {
                content = new GUIContent(
                    Resources.LoadIcon("Non-Commerce"), 
                    "Non-commercial version of Ultimate Editor Enhancer.\nUse in commercial projects is prohibited.\nMore details in \"Trial and Non-commercial EULA.txt\"."
                );
            }
        }
    }
}