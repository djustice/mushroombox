/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using InfinityCode.UltimateEditorEnhancer.UnityTypes;
using UnityEditor;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace InfinityCode.UltimateEditorEnhancer.InspectorTools
{
    public abstract class InspectorInjector
    {
        private double initTime;
        private List<EditorWindow> failedWindows;

        protected static VisualElement GetMainContainer(EditorWindow wnd)
        {
            return wnd != null ? GetVisualElement(wnd.rootVisualElement, "unity-inspector-main-container") : null;
        }

        protected static VisualElement GetVisualElement(VisualElement element, string className)
        {
            for (int i = 0; i < element.childCount; i++)
            {
                VisualElement el = element[i];
                if (el.ClassListContains(className)) return el;
                el = GetVisualElement(el, className);
                if (el != null) return el;
            }

            return null;
        }

        protected void InitInspector()
        {
            failedWindows = new List<EditorWindow>();

            Object[] windows = UnityEngine.Resources.FindObjectsOfTypeAll(InspectorWindowRef.type);
            foreach (EditorWindow wnd in windows)
            {
                if (wnd == null) continue;
                if (!InjectBar(wnd))
                {
                    failedWindows.Add(wnd);
                }
            }

            if (failedWindows.Count > 0)
            {
                initTime = EditorApplication.timeSinceStartup;
                EditorApplication.update += TryReinit;
            }
        }

        protected bool InjectBar(EditorWindow wnd)
        {
            VisualElement mainContainer = GetMainContainer(wnd);
            if (mainContainer == null) return false;
            if (mainContainer.childCount < 2) return false;
            VisualElement editorsList = GetVisualElement(mainContainer, "unity-inspector-editors-list");

            return OnInject(wnd, mainContainer, editorsList);
        }

        protected abstract bool OnInject(EditorWindow wnd, VisualElement mainContainer, VisualElement editorsList);

        protected void OnMaximizedChanged(EditorWindow w)
        {
            Object[] windows = UnityEngine.Resources.FindObjectsOfTypeAll(InspectorWindowRef.type);
            foreach (EditorWindow wnd in windows)
            {
                if (wnd != null) InjectBar(wnd);
            }
        }

        private void TryReinit()
        {
            if (EditorApplication.timeSinceStartup - initTime <= 0.5) return;
            EditorApplication.update -= TryReinit;
            if (failedWindows != null)
            {
                TryReinit(failedWindows);
                failedWindows = null;
            }
        }

        private void TryReinit(List<EditorWindow> windows)
        {
            foreach (EditorWindow wnd in windows)
            {
                if (wnd == null) continue;
                InjectBar(wnd);
            }
        }
    }
}