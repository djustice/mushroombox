/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using InfinityCode.UltimateEditorEnhancer.JSON;
using InfinityCode.UltimateEditorEnhancer.Windows;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static partial class Prefs
    {
        private const string PrefsKey = "UEE";
        public const string Prefix = PrefsKey + ".";

        private static Action AfterFirstLoad;

        private static bool migrationReplace;

        private static PrefManager[] _managers;
        private static string[] _keywords;
        private static PrefManager[] _generalManagers;
        private static string[] escapeChars = {"%", "%25", ";", "%3B", "(", "%28", ")", "%29"};
        private static bool forceSave = false;
        private static Vector2 scrollPosition;
        private static bool loaded = false;

        internal static PrefManager[] managers
        {
            get
            {
                if (_managers == null)
                {
                    List<PrefManager> items = Reflection.GetInheritedItems<PrefManager>();
                    _managers = items.OrderBy(d => d.order).ToArray();
                }
                return _managers;
            }
        }


        internal static PrefManager[] generalManagers
        {
            get
            {
                if (_generalManagers == null)
                {
                    _generalManagers = managers.Where(i => !i.GetType().IsSubclassOf(typeof(StandalonePrefManager))).ToArray();
                }
                return _generalManagers;
            }
        }

        static Prefs()
        {
            Load();
        }

        private static void CreateIgnore(string filename, bool entireAsset)
        {
            string path = new DirectoryInfo(Resources.assetFolder).Parent.FullName + "/." + filename;
            string content = "";
            if (entireAsset)
            {
                content = @"
/Ultimate Editor Enhancer/*
!/Ultimate Editor Enhancer/Scripts
/Ultimate Editor Enhancer/Scripts/Editor/
";
            }

            content += "/Ultimate Editor Enhancer Settings/";

            File.WriteAllText(path, content, Encoding.UTF8);
        }

        private static void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayoutUtils.ToolbarButton("File"))
            {
                GenericMenuEx menu = GenericMenuEx.Start();
                menu.Add("Export/Settings", ExportSettings);
                menu.Add("Export/Items/Everything", ExportItems, -1 );
                menu.AddSeparator("Export/Items/");
                menu.Add("Export/Items/Bookmarks", ExportItems, 0);
                menu.Add("Export/Items/Favorite Windows", ExportItems, 1 );
                menu.Add("Export/Items/Quick Access Bar", ExportItems, 2 );

                menu.Add("Import/Settings", ImportSettings);
                menu.Add("Import/Items", ImportItems);

                menu.Show();
            }

            if (GUILayoutUtils.ToolbarButton("Version Control"))
            {
                GenericMenuEx menu = GenericMenuEx.Start();
                menu.Add(".gitignore/Exclude Settings", () => { CreateIgnore("gitignore", false); });
                menu.Add(".gitignore/Exclude Entire Asset", () => { CreateIgnore("gitignore", true); });
                menu.Add(".collabignore/Exclude Settings", () => { CreateIgnore("collabignore", false); });
                menu.Add(".collabignore/Exclude Entire Asset", () => { CreateIgnore("collabignore", true); });
                menu.Show();
            }

            if (GUILayoutUtils.ToolbarButton("Restore default settings"))
            {
                if (EditorUtility.DisplayDialog("Restore default settings", "Are you sure you want to restore the default settings?", "Restore", "Cancel"))
                {
                    if (EditorPrefs.HasKey(PrefsKey)) EditorPrefs.DeleteKey(PrefsKey);
                    AssetDatabase.ImportAsset(Resources.assetFolder + "Scripts/Editor/Prefs/Methods.Prefs.cs", ImportAssetOptions.ForceUpdate);
                }
            }

            GUILayout.Label("", EditorStyles.toolbarButton, GUILayout.ExpandWidth(true));
            if (GUILayoutUtils.ToolbarButton("Help"))
            {
                GenericMenuEx menu = GenericMenuEx.Start();
                menu.Add("Welcome", Welcome.OpenWindow);
                menu.Add("Getting Started", GettingStarted.OpenWindow);
                menu.Add("Shortcuts", Shortcuts.OpenWindow);
                menu.AddSeparator();
                menu.Add("Product Page", Links.OpenHomepage);
                menu.Add("Documentation", Links.OpenDocumentation);
                menu.Add("Videos", Links.OpenYouTube);
                menu.AddSeparator();
                menu.Add("Support", Links.OpenSupport);
                menu.Add("Forum", Links.OpenForum);
                menu.Add("Check Updates", Updater.OpenWindow);
                menu.AddSeparator();
                menu.Add("Rate and Review", Welcome.RateAndReview);
                menu.Add("About", About.OpenWindow);

                menu.Show();
            }

            EditorGUILayout.EndHorizontal();
        }

        private static void ExportItems(object data)
        {
            int target = (int)data;
            string name = "UEE-Items-";
            if (target == -1) name += "Everything";
            else if (target == 0) name += "Bookmarks";
            else if (target == 1) name += "Favorite-Windows";
            else if (target == 2) name += "Quick-Access-Bar";

            string filename = EditorUtility.SaveFilePanel("Export Items", EditorApplication.applicationPath, name, "json");
            if (string.IsNullOrEmpty(filename)) return;

            JsonObject obj = new JsonObject();

            if (target == -1 || target == 0) obj.Add("bookmarks", Bookmarks.json);
            if (target == -1 || target == 1) obj.Add("favorite-windows", FavoriteWindowsManager.json);
            if (target == -1 || target == 2) obj.Add("quick-access-bar", QuickAccessBarManager.json);

            File.WriteAllText(filename, obj.ToString(), Encoding.UTF8);
        }

        private static void ExportSettings()
        {
            string filename = EditorUtility.SaveFilePanel("Export Settings", EditorApplication.applicationPath, "UEE-Settings", "ucs");
            if (string.IsNullOrEmpty(filename)) return;

            File.WriteAllText(filename, GetSettings(), Encoding.UTF8);
        }

        private static FieldInfo GetField(FieldInfo[] fields, string key)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].Name == key) return fields[i];
            }

            return null;
        }

        public static IEnumerable<string> GetKeywords()
        {
            if (_keywords == null) _keywords = generalManagers.SelectMany(m => m.keywords).ToArray();
            return _keywords;
        }

        private static string GetSettings()
        {
            FieldInfo[] fields = typeof(Prefs).GetFields(BindingFlags.Static | BindingFlags.Public);
            StaticStringBuilder.Clear();

            try
            {
                SaveFields(fields, null);
                return StaticStringBuilder.GetString(true);
            }
            catch (Exception e)
            {
                Log.Add(e);
            }

            return string.Empty;
        }

        private static void ImportItems()
        {
            string filename = EditorUtility.OpenFilePanel("Import Items", EditorApplication.applicationPath, "json");
            if (string.IsNullOrEmpty(filename)) return;

            string text = File.ReadAllText(filename, Encoding.UTF8);
            JsonItem json = Json.Parse(text);
            JsonItem bookmarksItem = json["bookmarks"];

            migrationReplace = true;

            if (bookmarksItem != null) Bookmarks.json = bookmarksItem as JsonArray;

            JsonItem fwItem = json["favorite-windows"];
            if (fwItem != null) FavoriteWindowsManager.json = fwItem as JsonArray;

            JsonItem qabItem = json["quick-access-bar"];
            if (qabItem != null) QuickAccessBarManager.json = qabItem as JsonArray;

            migrationReplace = false;

            ReferenceManager.Save();
        }

        private static void ImportSettings()
        {
            string filename = EditorUtility.OpenFilePanel("Import Settings", EditorApplication.applicationPath, "ucs");
            if (string.IsNullOrEmpty(filename)) return;

            string prefs = File.ReadAllText(filename, Encoding.UTF8);
            LoadSettings(prefs);
        }

        public static void InvokeAfterFirstLoad(Action action)
        {
            if (loaded) action();
            else AfterFirstLoad += action;
        }

        private static void Load()
        {
            string prefStr = EditorPrefs.GetString(PrefsKey, String.Empty);
            LoadSettings(prefStr);

            loaded = true;

            if (AfterFirstLoad != null)
            {
                Delegate[] invocationList = AfterFirstLoad.GetInvocationList();
                for (int i = 0; i < invocationList.Length; i++)
                {
                    try
                    {
                        Delegate d = invocationList[i];
                        d.DynamicInvoke(null);
                    }
                    catch
                    {
                        
                    }
                }

                AfterFirstLoad = null;
            }
        }

        private static void LoadSettings(string str)
        {
            if (string.IsNullOrEmpty(str)) return;

            Type prefType = typeof(Prefs);
            FieldInfo[] fields = prefType.GetFields(BindingFlags.Static | BindingFlags.Public);

            int i = 0;
            try
            {
                LoadFields(str, fields, ref i, null);
            }
            catch (Exception e)
            {
                Log.Add(e);
            }
        }

        private static void LoadFields(string prefStr, FieldInfo[] fields, ref int i, object target)
        {
            StaticStringBuilder.Clear();
            bool isKey = true;
            string key = null;

            while (i < prefStr.Length)
            {
                char c = prefStr[i];
                i++;
                if (c == ':' && isKey)
                {
                    key = StaticStringBuilder.GetString(true);
                    isKey = false;
                }
                else if (c == ';')
                {
                    string value = StaticStringBuilder.GetString(true);
                    isKey = true;
                    SetValue(target, fields, key, value);
                }
                else if (c == '(')
                {
                    FieldInfo field = GetField(fields, key);
                    if (field == null || (field.FieldType.IsValueType && field.FieldType.IsPrimitive) || field.FieldType == typeof(string))
                    {
                        int indent = 1;
                        i++;
                        while (indent > 0 && i < prefStr.Length)
                        {
                            c = prefStr[i];
                            if (c == ')') indent--;
                            else if (c == '(') indent++;
                            i++;
                        }

                        isKey = true;
                    }
                    else
                    {
                        Type type = field.FieldType; 
                        object newTarget = Activator.CreateInstance(type, false); 

                        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
                        if (type == typeof(Vector2Int)) bindingFlags |= BindingFlags.NonPublic;

                        FieldInfo[] objFields = type.GetFields(bindingFlags);

                        LoadFields(prefStr, objFields, ref i, newTarget);
                        field.SetValue(target, newTarget);
                        i++;
                        isKey = true;
                    }
                }
                else if (c == ')')
                {
                    string value = StaticStringBuilder.GetString(true);
                    SetValue(target, fields, key, value);
                    return;
                }
                else StaticStringBuilder.Append(c);
            }
        }

        public static string ModifierToString(EventModifiers modifiers)
        {
            StaticStringBuilder.Clear();
            if ((modifiers & EventModifiers.Control) == EventModifiers.Control) StaticStringBuilder.Append("CTRL");
            if ((modifiers & EventModifiers.Command) == EventModifiers.Command)
            {
                if (StaticStringBuilder.Length > 0) StaticStringBuilder.Append(" + ");
                StaticStringBuilder.Append("CMD");
            }
            if ((modifiers & EventModifiers.Shift) == EventModifiers.Shift)
            {
                if (StaticStringBuilder.Length > 0) StaticStringBuilder.Append(" + ");
                StaticStringBuilder.Append("SHIFT");
            }
            if ((modifiers & EventModifiers.Alt) == EventModifiers.Alt)
            {
                if (StaticStringBuilder.Length > 0) StaticStringBuilder.Append(" + ");
                StaticStringBuilder.Append("ALT");
            }
            if ((modifiers & EventModifiers.FunctionKey) == EventModifiers.FunctionKey)
            {
                if (StaticStringBuilder.Length > 0) StaticStringBuilder.Append(" + ");
                StaticStringBuilder.Append("FN");
            }

            return StaticStringBuilder.GetString(true);
        }

        public static string ModifierToString(EventModifiers modifiers, string extra)
        {
            string v = ModifierToString(modifiers);
            if (!string.IsNullOrEmpty(v)) v += " + ";
            v += extra;
            return v;
        }

        public static string ModifierToString(EventModifiers modifiers, KeyCode keycode)
        {
            return ModifierToString(modifiers, keycode.ToString());
        }

        public static void OnGUI(string searchContext)
        {
            DrawToolbar();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            foreach (PrefManager manager in generalManagers)
            {
                try
                {
                    EditorGUI.BeginChangeCheck();
                    manager.Draw();
                    EditorGUILayout.Space();
                    if (EditorGUI.EndChangeCheck() || forceSave)
                    {
                        Save();
                        forceSave = false;
                    }
                }
                catch (ExitGUIException e)
                {
                    throw e;
                }
                catch
                {
                    
                }
            }

            EditorGUILayout.EndScrollView();
        }

        public static void Save() 
        {
            string value = GetSettings();
            EditorPrefs.SetString(PrefsKey, value);
        }

        private static void SaveFields(FieldInfo[] fields, object target)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                if (field.IsLiteral || field.IsInitOnly) continue; 
                object value = field.GetValue(target); 

                if (value == null) continue; 

                if (i > 0) StaticStringBuilder.Append(";");
                StaticStringBuilder.Append(field.Name).Append(":");

                Type type = value.GetType();
                if (type == typeof(string)) StaticStringBuilder.AppendEscaped(value as string, escapeChars);
                else if (type.IsEnum) StaticStringBuilder.Append(value.ToString());
                else if (type.IsValueType && type.IsPrimitive) StaticStringBuilder.Append(value);
                else
                {
                    BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
                    if (type == typeof(Vector2Int)) bindingFlags |= BindingFlags.NonPublic;
                    FieldInfo[] objFields = type.GetFields(bindingFlags);
                    if (objFields.Length == 0) continue;

                    StaticStringBuilder.Append("(");

                    SaveFields(objFields, value);

                    StaticStringBuilder.Append(")");
                }
            }
        }

        private static void SetValue(object target, FieldInfo[] fields, string key, object value)
        {
            FieldInfo field = GetField(fields, key);
            if (field == null) return;

            Type type = field.FieldType;
            if (type == typeof(string))
            {
                field.SetValue(target, Unescape(value as string, escapeChars));
            }
            else if (field.FieldType.IsEnum)
            {
                string strVal = value as string;
                if (strVal != null)
                {
                    try
                    {
                        value = Enum.Parse(type, strVal);
                        field.SetValue(target, value);
                    }
                    catch
                    {
                        Debug.Log("Some exception");
                    }
                }
            }
            else if (type.IsValueType)
            {
                try
                {
                    MethodInfo method = type.GetMethod("Parse", new[] { typeof(string) });
                    if (method == null)
                    {
                        Debug.Log("No parse for " + key); 
                        return;
                    }
                    value = method.Invoke(null, new[] { value });
                    if (value != null) field.SetValue(target, value); 
                }
                catch
                {

                }
            }
        }

        private static string Unescape(string value, string[] escapeCodes)
        {
            if (escapeChars == null || escapeChars.Length % 2 != 0) throw new Exception("Length of escapeCodes must be N * 2");

            StaticStringBuilder.Clear();

            for (int i = 0; i < value.Length; i++)
            {
                bool success = false;
                for (int j = 0; j < escapeCodes.Length; j += 2)
                {
                    string code = escapeCodes[j + 1];
                    if (value.Length - i - code.Length <= 0) continue;

                    success = true;

                    for (int k = 0; k < code.Length; k++)
                    {
                        if (code[k] != value[i + k])
                        {
                            success = false;
                            break;
                        }
                    }

                    if (success)
                    {
                        StaticStringBuilder.Append(escapeCodes[j]);
                        i += code.Length - 1;
                        break;
                    }
                }

                if (!success) StaticStringBuilder.Append(value[i]);
            }

            return StaticStringBuilder.GetString(true);
        }
    }
}