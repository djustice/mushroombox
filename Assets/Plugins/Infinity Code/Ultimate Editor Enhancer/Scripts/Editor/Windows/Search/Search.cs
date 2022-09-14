/*           INFINITY CODE          */
/*     https://infinity-code.com    */

#if UNITY_2021_2_OR_NEWER
using UnityEditor.SceneManagement;
#else
using UnityEditor.Experimental.SceneManagement;
#endif

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using InfinityCode.UltimateEditorEnhancer.UnityTypes;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.Windows
{
    [InitializeOnLoad]
    public partial class Search : PopupWindow
    {
        private const int width = 500;
        private const int maxRecords = 50;

        private static Dictionary<int, Record> projectRecords;
        private static Dictionary<int, Record> sceneRecords;
        private static Dictionary<int, Record> windowRecords;

        private static bool focusOnTextField = false;
        public static int searchMode = 0;
        private static Record[] bestRecords;
        private static int countBestRecords = 0;
        private static int bestRecordIndex = 0;
        private static bool updateScroll;
        private static bool needUpdateBestRecords;
        private static bool isDragStarted = false;
        private static string[] searchModeLabels = { "Everywhere", "By Hierarchy", "By Project" };

        public string searchText;
        
        private Vector2 scrollPosition;
        private bool resetSelection;
        public int setSelectionIndex = -1;

        public static Search instance { get; private set; }

        static Search()
        {
            KeyManager.KeyBinding binding = KeyManager.AddBinding();
            binding.OnValidate += OnValidate;
            binding.OnInvoke += OnInvoke;

            binding = KeyManager.AddBinding();
            binding.OnValidate += OnValidateScript;
            binding.OnInvoke += OnInvokeScript;
        }

        private static void CachePrefabWithComponents(Dictionary<int, Record> tempRecords, GameObject go)
        {
            tempRecords.Add(go.GetInstanceID(), new GameObjectRecord(go));
            Component[] components = go.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                Component c = components[i];
                tempRecords.Add(c.GetInstanceID(), new ComponentRecord(c));
            }

            Transform t = go.transform;
            for (int i = 0; i < t.childCount; i++) CachePrefabWithComponents(tempRecords, t.GetChild(i).gameObject);
        }

        private static void CachePrefabWithoutComponents(Dictionary<int, Record> tempRecords, GameObject go)
        {
            tempRecords.Add(go.GetInstanceID(), new GameObjectRecord(go));
            Transform t = go.transform;
            for (int i = 0; i < t.childCount; i++) CachePrefabWithoutComponents(tempRecords, t.GetChild(i).gameObject);
        }

        private static void CacheProject()
        {
            Dictionary<int, Record> tempRecords = new Dictionary<int, Record>();
            string[] assets = AssetDatabase.GetAllAssetPaths();

            if (projectRecords != null)
            {
                foreach (KeyValuePair<int, Record> pair in projectRecords) pair.Value.used = false;
            }

            char[] validPrefix = {'A', 's', 's', 'e', 't', 's', '/'};

            for (int i = 0; i < assets.Length; i++)
            {
                string assetPath = assets[i];
                try
                {
                    if (string.IsNullOrEmpty(assetPath)) continue;
                    if (assetPath.Length < 8) continue;

                    bool hasValidPrefix = true;

                    for (int j = 0; j < validPrefix.Length; j++)
                    {
                        if (validPrefix[j] != assetPath[j])
                        {
                            hasValidPrefix = false;
                            break;
                        }
                    }

                    if (!hasValidPrefix) continue;

                    int hashCode = assetPath.GetHashCode();

                    if (projectRecords != null)
                    {
                        Record r;

                        if (projectRecords.TryGetValue(hashCode, out r))
                        {
                            if (!tempRecords.ContainsKey(hashCode))
                            {
                                tempRecords.Add(hashCode, r);
                                r.used = true;
                            }

                            continue;
                        }
                    }

                    if (!tempRecords.ContainsKey(hashCode))
                    {
                        tempRecords.Add(hashCode, new ProjectRecord(assetPath));
                    }
                }
                catch
                {
                }
            }

            if (projectRecords != null)
            {
                foreach (var pair in projectRecords.Where(p => !p.Value.used)) pair.Value.Dispose();
            }

            projectRecords = tempRecords;
        }

        private static void CacheScene()
        {
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

            if (sceneRecords != null)
            {
                foreach (var record in sceneRecords) record.Value.used = false;
            }

            if (prefabStage != null)
            {
                Dictionary<int, Record> tempRecords = new Dictionary<int, Record>();
                try
                {
                    if (Prefs.searchByComponents) CachePrefabWithComponents(tempRecords, prefabStage.prefabContentsRoot);
                    else CachePrefabWithoutComponents(tempRecords, prefabStage.prefabContentsRoot);
                }
                catch (Exception e)
                {
                    Log.Add(e);
                }

                if (sceneRecords != null)
                {
                    foreach (var record in sceneRecords)
                    {
                        if (!record.Value.used) record.Value.Dispose();
                    }
                }

                sceneRecords = tempRecords;
            }
            else
            {
                if (Prefs.searchByComponents) CacheSceneItems();
                else CacheSceneGameObjects();
            }
        }

        private static void CacheSceneGameObjects()
        {
            Transform[] transforms = FindObjectsOfType<Transform>(true);
            Dictionary<int, Record> tempRecords = new Dictionary<int, Record>(transforms.Length);

            for (int i = 0; i < transforms.Length; i++)
            {
                GameObject go = transforms[i].gameObject;
                int key = go.GetInstanceID();

                Record r;

                if (sceneRecords == null || !sceneRecords.TryGetValue(key, out r)) r = new GameObjectRecord(go);
                else r.UpdateGameObjectName(go);

                r.used = true;
                tempRecords.Add(key, r);
            }

            if (sceneRecords != null)
            {
                foreach (var record in sceneRecords)
                {
                    if (!record.Value.used) record.Value.Dispose();
                }
            }

            sceneRecords = tempRecords;
        }

        private static void CacheSceneItems()
        {
            Component[] components = FindObjectsOfType<Component>(true);
            Dictionary<int, Record> tempRecords = new Dictionary<int, Record>(Mathf.NextPowerOfTwo(components.Length));

            for (int i = 0; i < components.Length; i++)
            {
                Component c = components[i];
                int key = c.GetInstanceID();
                Record r = null;
                if (sceneRecords == null || !sceneRecords.TryGetValue(key, out r)) r = new ComponentRecord(c);
                else r.UpdateGameObjectName(c.gameObject);

                r.used = true;
                tempRecords.Add(key, r);

                GameObject go = c.gameObject;
                key = go.GetInstanceID();

                if (!tempRecords.ContainsKey(key))
                {
                    if (sceneRecords == null || !sceneRecords.TryGetValue(key, out r)) r = new GameObjectRecord(go);
                    else r.UpdateGameObjectName(go);

                    r.used = true;
                    tempRecords.Add(key, r);
                }
            }

            if (sceneRecords != null)
            {
                foreach (var record in sceneRecords)
                {
                    if (!record.Value.used) record.Value.Dispose();
                }
            }

            sceneRecords = tempRecords;
        }

        private static void CacheWindows()
        {
            if (windowRecords != null) return;

            windowRecords = new Dictionary<int, Record>();

            foreach (string submenu in Unsupported.GetSubmenus("Window"))
            {
                string upper = Culture.textInfo.ToUpper(submenu);
                if (upper == "WINDOW/NEXT WINDOW") continue;
                if (upper == "WINDOW/PREVIOUS WINDOW") continue;
                if (Culture.compareInfo.IsPrefix(upper, "WINDOW/LAYOUTS", CompareOptions.None)) continue;

                int lastSlash = 7;

                for (int i = submenu.Length - 1; i >= 8; i--)
                {
                    if (submenu[i] == '/')
                    {
                        lastSlash = i;
                        break;
                    }
                }

                windowRecords.Add(submenu.GetHashCode(), new WindowRecord(submenu, submenu.Substring(lastSlash + 1)));
            }

            foreach (string submenu in Unsupported.GetSubmenus("Tools"))
            {
                int lastSlash = 6;

                for (int i = submenu.Length - 1; i >= 7; i--)
                {
                    if (submenu[i] == '/')
                    {
                        lastSlash = i;
                        break;
                    }
                }

                windowRecords.Add(submenu.GetHashCode(), new WindowRecord(submenu, submenu.Substring(lastSlash + 1)));
            }

            windowRecords.Add("Project Settings...".GetHashCode(), new WindowRecord("Edit/Project Settings...", "Project Settings"));
            windowRecords.Add("Preferences...".GetHashCode(), new WindowRecord("Edit/Preferences...", "Preferences"));
        }

        private void DrawBestRecords()
        {
            if (countBestRecords == 0) return;

            if (bestRecordIndex >= countBestRecords) bestRecordIndex = 0;
            else if (bestRecordIndex < 0) bestRecordIndex = countBestRecords - 1;

            if (updateScroll)
            {
                float bry = 20 * bestRecordIndex - scrollPosition.y;
                if (bry < 0) scrollPosition.y = 20 * bestRecordIndex;
                else if (bry > 80)
                {
                    if (bestRecordIndex != countBestRecords - 1) scrollPosition.y = 20 * bestRecordIndex - 80;
                    else scrollPosition.y = 20 * bestRecordIndex - 80;
                }
            }

            int selectedIndex = -1;
            int selectedState = -1;

            scrollPosition = GUI.BeginScrollView(new Rect(0, 40, position.width, position.height - 40), scrollPosition, new Rect(0, 0, position.width - 40, countBestRecords * 20));

            for (int i = 0; i < countBestRecords; i++)
            {
                int state = bestRecords[i].Draw(i);
                if (state != 0)
                {
                    selectedIndex = i;
                    selectedState = state;
                }
            }

            GUI.EndScrollView();

            if (selectedIndex != -1) SelectRecord(selectedIndex, selectedState);
        }

        private void DrawHeader()
        {
            GUI.Box(new Rect(0, 0, position.width, position.height), GUIContent.none, EditorStyles.toolbar);

            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));

            if (Prefs.searchByProject)
            {
                GUILayout.Space(100);

                EditorGUI.BeginChangeCheck();
                searchMode = GUILayout.Toolbar(searchMode, searchModeLabels, EditorStyles.toolbarButton);
                if (EditorGUI.EndChangeCheck())
                {
                    needUpdateBestRecords = true;
                    focusOnTextField = true;
                }

                GUILayout.Space(80);
            }
            else
            {
                GUILayout.Label("Search", Styles.centeredLabel, GUILayout.Height(20));
            }

            if (GUILayoutUtils.ToolbarButton(TempContent.Get(EditorIconContents.settings.image, "Settings")))
            {
                SettingsService.OpenProjectSettings("Project/Ultimate Editor Enhancer");
            }

            if (GUILayoutUtils.ToolbarButton("?"))
            {
                Links.OpenDocumentation("smart-search");
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawHint()
        {
            GUI.Label(new Rect(0, 15, position.width, position.height), "Enter the name of the object you are looking for.\nSupports fuzzy search, and you can enter a query in part.\nTo search by object type, enter \"query:type\".\n\nUse Tab to quickly switch between searching\neverywhere, in the hierarchy, and in the project.", Styles.centeredLabel);
        }

        private void DrawNothingFound()
        {
            GUI.Label(new Rect(0, 0, position.width, position.height), "Nothing found.", Styles.centeredLabel);
        }

        protected void OnDestroy()
        {
            bestRecords = null;
        }

        private void OnEnable()
        {
            bestRecords = new Record[maxRecords];
            countBestRecords = 0;
            bestRecordIndex = 0;

            CacheScene();
            if (Prefs.searchByProject) CacheProject();
            if (Prefs.searchByWindow) CacheWindows();
        }

        protected override void OnGUI()
        {
            if (focusedWindow != instance)
            {
                if (isDragStarted)
                {
                    if (DragAndDrop.objectReferences.Length == 0) isDragStarted = false;
                    else Repaint();
                }

                if (!isDragStarted)
                {
                    EventManager.BroadcastClosePopup();
                    return;
                }
            }

            if (EditorApplication.isCompiling)
            {
                EventManager.BroadcastClosePopup();
                return;
            }

            if (sceneRecords == null) CacheScene();
            if (projectRecords == null) CacheProject();
            if (windowRecords == null) CacheWindows();

            if (!ProcessEvents()) return;

            DrawHeader();

            GUI.SetNextControlName("UEESearchTextField");
            EditorGUI.BeginChangeCheck();
            searchText = GUILayoutUtils.ToolbarSearchField(searchText);
            bool changed = EditorGUI.EndChangeCheck();

            if (Event.current.type == EventType.Repaint)
            {
                if (resetSelection)
                {
                    TextEditor recycledEditor = EditorGUIRef.GetRecycledEditor() as TextEditor;
                    if (recycledEditor != null)
                    {
                        if (setSelectionIndex == -1)
                        {
                            recycledEditor.MoveTextEnd();
                        }
                        else
                        {
                            recycledEditor.MoveTextStart();
                            setSelectionIndex = -1;
                        }
                    }
                    
                    resetSelection = false;
                    Repaint();
                }
            }

            if (focusOnTextField && Event.current.type == EventType.Repaint)
            {
                GUI.FocusControl("UEESearchTextField");
                focusOnTextField = false;
                if (!string.IsNullOrEmpty(searchText)) resetSelection = true;
            }

            if (changed || needUpdateBestRecords) UpdateBestRecords();

            if (string.IsNullOrEmpty(searchText)) DrawHint();
            else if (countBestRecords == 0) DrawNothingFound();
            else DrawBestRecords();
        }

        public static void OnInvoke()
        {
            Event e = Event.current;
            Vector2 position = e.mousePosition;

            if (focusedWindow != null) position += focusedWindow.position.position;

            Rect rect = new Rect(position + new Vector2(width / -2, -30), new Vector2(width, 140));

#if !UNITY_EDITOR_OSX
            if (rect.y < 5) rect.y = 5;
            else if (rect.yMax > Screen.currentResolution.height - 40) rect.y = Screen.currentResolution.height - 40 - rect.height;
#endif

            Show(rect);
            e.Use();
        }

        private static void OnInvokeScript()
        {
            OnInvoke();
            instance.searchText = ":script";
            instance.setSelectionIndex = 0;
            searchMode = 2;
        }

        private static bool OnValidate()
        {
            if (!Prefs.search) return false;
            Event e = Event.current;

            if (e.keyCode != Prefs.searchKeyCode) return false;
            if (e.modifiers != Prefs.searchModifiers) return false;

            if (Prefs.SearchDoNotShowOnWindows()) return false;
            return true;
        }

        private static bool OnValidateScript()
        {
            if (!Prefs.searchScript) return false;

            Event e = Event.current;
            return e.modifiers == Prefs.searchScriptModifiers && e.keyCode == Prefs.searchScriptKeyCode;
        }

        private static bool ProcessEvents()
        {
            Event e = Event.current;
            updateScroll = false;

            if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.DownArrow)
                {
                    bestRecordIndex++;
                    updateScroll = true;
                    e.Use();
                }
                else if (e.keyCode == KeyCode.UpArrow)
                {
                    bestRecordIndex--;
                    updateScroll = true;
                    e.Use();
                }
                else if (e.keyCode == KeyCode.Return || e.keyCode == KeyCode.KeypadEnter)
                {
                    if (countBestRecords > 0)
                    {
                        Record bestRecord = bestRecords[bestRecordIndex];
                        int state = 1;
                        if (e.modifiers == EventModifiers.Control || e.modifiers == EventModifiers.Command) state = 2;
                        else if (e.modifiers == EventModifiers.Shift) state = 3;
                        bestRecord.Select(state);

                        EventManager.BroadcastClosePopup();

                        return false;
                    }
                }
                else if (e.keyCode == KeyCode.Escape)
                {
                    EventManager.BroadcastClosePopup();
                    return false;
                }
            }
            else if (e.type == EventType.KeyUp)
            {
                if (Prefs.searchByProject && e.keyCode == KeyCode.Tab)
                {
                    focusOnTextField = true;
                    searchMode++;
                    if (searchMode == 3) searchMode = 0;

                    bestRecordIndex = 0;
                    needUpdateBestRecords = true;
                    e.Use();
                }
            }

            return true;
        }

        private static void SelectRecord(int index, int state)
        {
            bestRecords[index].Select(state);
            EventManager.BroadcastClosePopup();
        }

        public static void Show(Rect rect)
        {
            EventManager.BroadcastClosePopup();

            SceneView.RepaintAll();

            if (Prefs.searchPauseInPlayMode && EditorApplication.isPlaying) EditorApplication.isPaused = true;

            instance = CreateInstance<Search>();
            instance.position = rect;
            instance.ShowPopup();
            instance.Focus();
            focusOnTextField = true;
            searchMode = 0;

            EventManager.AddBinding(EventManager.ClosePopupEvent).OnInvoke += b =>
            {
                instance.Close();
                b.Remove();
            };
        }

        private int TakeBestRecords(IEnumerable<KeyValuePair<int, Record>> tempBestRecords)
        {
            int count = 0;
            float minAccuracy = float.MaxValue;

            foreach (var pair in tempBestRecords)
            {
                Record v = pair.Value;
                float a = v.accuracy;

                if (count < maxRecords)
                {
                    bestRecords[count] = v;
                    count++;
                    if (minAccuracy > a) minAccuracy = a;
                    continue;
                }

                if (a <= minAccuracy) continue;

                float newMin = float.MaxValue;
                bool needReplace = true;

                for (int i = 0; i < maxRecords; i++)
                {
                    Record v1 = bestRecords[i];
                    float a1 = v1.accuracy;
                    if (needReplace && a1 == minAccuracy)
                    {
                        bestRecords[i] = v;
                        needReplace = false;
                        if (newMin > v.accuracy) newMin = v.accuracy;
                    }
                    else if (newMin > a1) newMin = a1;
                }

                minAccuracy = newMin;
            }

            if (count > 1)
            {
                Record[] sortedRecords = bestRecords.Take(count)
                    .OrderByDescending(r => r.accuracy)
                    .ThenBy(r => r.label.Length)
                    .ThenBy(r => r.label).ToArray();

                for (int i = 0; i < sortedRecords.Length; i++) bestRecords[i] = sortedRecords[i];
            }

            return count;
        }

        private void UpdateBestRecords()
        {
            needUpdateBestRecords = false;
            bestRecordIndex = 0;
            countBestRecords = 0;
            scrollPosition = Vector2.zero;

            int minStrLen = 1;
            if (searchText == null || searchText.Length < minStrLen) return;

            string assetType;
            string search = SearchableItem.GetPattern(searchText, out assetType);

            IEnumerable <KeyValuePair<int, Record>> tempBestRecords;

            if (searchMode == 0)
            {
                int currentMode = 0;
                tempBestRecords = new List<KeyValuePair<int, Record>>();
                if (search.Length > 0)
                {
                    if (search[0] == '@') currentMode = 1;
                    else if (search[0] == '#') currentMode = 2;
                }

                if (currentMode != 0) search = search.Substring(1);

                if (Prefs.searchByWindow && currentMode == 0) tempBestRecords = tempBestRecords.Concat(windowRecords.Where(r => r.Value.Update(search, assetType) > 0));
                if (currentMode == 0 || currentMode == 1) tempBestRecords = tempBestRecords.Concat(sceneRecords.Where(r => r.Value.Update(search, assetType) > 0));
                if (Prefs.searchByProject && currentMode == 0 || currentMode == 2) tempBestRecords = tempBestRecords.Concat(projectRecords.Where(r => r.Value.Update(search, assetType) > 0));
            }
            else
            {
                tempBestRecords = searchMode == 1? sceneRecords: projectRecords;
                tempBestRecords = tempBestRecords.Where(r => r.Value.Update(search, assetType) > 0);
            }

            countBestRecords = TakeBestRecords(tempBestRecords);
            updateScroll = true;
        }
    }
}