/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using InfinityCode.UltimateEditorEnhancer.Tools;
using InfinityCode.UltimateEditorEnhancer.Windows;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer
{
    [InitializeOnLoad]
    public static class TrialManager
    {
        private const string KEY = Prefs.Prefix + "TS";
        private const string TRIAL_FINISHED_KEY = Prefs.Prefix + "TF";
        private static DateTime? _trialStarted;

        public static int daysLeft
        {
            get
            {
                if (!_trialStarted.HasValue) return 0;
                DateTime current = DateTime.Now;
                DateTime started = _trialStarted.Value;
                if (current < started) return 0;
                return (int)Math.Ceiling(14 - (current - started).TotalDays);
            }
        }

        public static bool isTrial
        {
            get { return _trialStarted.HasValue; }
        }

        public static bool trialFinishedShown { get; private set; }

        static TrialManager()
        {
            Load();
            ToolbarTrialIcon.Init();
            Welcome.OpenAtStartupValidate += OpenAtStartupValidate;
        }

        private static void Load()
        {
            if (!EditorPrefs.HasKey(KEY)) return;

            string started = EditorPrefs.GetString(KEY);
            long ticks;
            if (!long.TryParse(started, out ticks)) return;
            
            _trialStarted = new DateTime(ticks);
            trialFinishedShown = EditorPrefs.GetBool(TRIAL_FINISHED_KEY, false);
        }

        private static bool OpenAtStartupValidate()
        {
            if (isTrial) return true;
            NonCommerceTrialSelector.OpenWindow();
            return false;
        }

        public static void ShowTrialFinished()
        {
            EditorPrefs.SetBool(TRIAL_FINISHED_KEY, true);
            trialFinishedShown = true;
            int action = EditorUtility.DisplayDialogComplex(
                "Trial period of Ultimate Editor Enhancer is over", 
                "Please buy Ultimate Editor Enhancer in Unity Asset Store or remove it from the project.", 
                "Open Unity Asset Store", "Hide", "Use Non-Commercial");
            if (action == 0) Links.OpenAssetStore();
        }

        [MenuItem(WindowsHelper.MenuPath + "Start Trial", false, 129)]
        public static void StartTrial()
        {
            if (isTrial) return;
            _trialStarted = DateTime.Now;
            EditorPrefs.SetString(KEY, _trialStarted.Value.Ticks.ToString());
            ToolbarTrialIcon.UpdateContent();
        }
    }
}