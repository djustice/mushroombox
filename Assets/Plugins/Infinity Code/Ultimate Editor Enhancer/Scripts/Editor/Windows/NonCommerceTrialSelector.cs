/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.Windows
{
    public class NonCommerceTrialSelector : EditorWindow
    {
        private static GUIStyle headerStyle;
        private static GUIStyle sectionTitleStyle;

        private void OnDestroy()
        {
            Welcome.OpenWindow();
        }

        private void OnEnable()
        {
            Vector2 size = new Vector2(609, 260);
            minSize = size;
            maxSize = size;
        }

        private void OnGUI()
        {
            if (headerStyle == null)
            {
                headerStyle = new GUIStyle(Styles.centeredHelpbox)
                {
                    fontSize = 12,
                    fontStyle = FontStyle.Bold
                };
            }

            if (sectionTitleStyle == null)
            {
                sectionTitleStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    alignment = TextAnchor.MiddleCenter
                };
            }

            GUILayout.Label("What version do you want to use?", headerStyle, GUILayout.Height(30));
            EditorGUILayout.BeginHorizontal();

            const int width = 300;
            const int height = 220;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(width), GUILayout.Height(height));
            GUILayout.Label("Trial", sectionTitleStyle);
            GUILayout.Label("Trial period is 14 days, during which you can use the asset in commercial projects.\nAfter the end of the trial period, you must buy Ultimate Editor Enhancer in Unity Asset Store or remove it from the project.", EditorStyles.wordWrappedLabel);

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Start Trial"))
            {
                TrialManager.StartTrial();
                Close();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(width), GUILayout.Height(height));
            GUILayout.Label("Non-Commercial", sectionTitleStyle);
            GUILayout.Label("You can use the non-commercial version of an asset in a project in the following cases:\n- In the project for personal non-commercial purposes;\n-In an open source project;\n-When creating video content for public video services(such as YouTube, Vimeo, etc.), regardless of the presence of monetization on your channel;\n-In a project created for the purposes of education or medicine.", EditorStyles.wordWrappedLabel);

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Use Non-Commercial"))
            {
                Close();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        public static void OpenWindow()
        {
            NonCommerceTrialSelector wnd = GetWindow<NonCommerceTrialSelector>(true, "Choose version of Ultimate Editor Enhancer you want to use?");
        }
    }
}