/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InfinityCode.UltimateEditorEnhancer
{
    [Serializable]
    public abstract class BookmarkItem : SearchableItem
    {
        public string title;
        public string type;
        public Object target;

        [NonSerialized]
        public string tooltip;

        [NonSerialized]
        public Texture preview;

        [NonSerialized]
        public bool previewLoaded;

        public GameObject gameObject
        {
            get
            {
                if (target == null) return null;
                if (target is Component) return (target as Component).gameObject;
                if (target is GameObject) return target as GameObject;
                return null;
            }
        }

        public abstract bool isProjectItem { get; }

        public BookmarkItem()
        {

        }

        public BookmarkItem(Object obj)
        {
            target = obj;

            Type t = obj.GetType();
            type = t.AssemblyQualifiedName;

            if (obj is GameObject)
            {
                GameObject go = obj as GameObject;
                title = go.name;
                tooltip = GetTransformPath(go.transform).ToString();
            }
            else if (obj is Component)
            {
                GameObject go = (obj as Component).gameObject;
                title = go.name + " (" + t.Name + ")";
                tooltip = GetTransformPath(go.transform).ToString();
            }
            else
            {
                tooltip = title = obj.name;
            }
        }

        public static StringBuilder GetTransformPath(Transform t)
        {
            StaticStringBuilder.Clear();

            StaticStringBuilder.Append(t.name);
            while ((t = t.parent) != null)
            {
                StaticStringBuilder.Insert(0, '/');
                StaticStringBuilder.Insert(0, t.name);
            }

            return StaticStringBuilder.GetBuilder();
        }

        public void Dispose()
        {
            target = null;
            preview = null;
        }

        protected override int GetSearchCount()
        {
            return 1;
        }

        protected override string GetSearchString(int index)
        {
            return title;
        }

        public float Update(string pattern, string valueType)
        {
            _accuracy = 0;
            if (!string.IsNullOrEmpty(valueType) && !Contains(type, valueType)) return 0;
            return UpdateAccuracy(pattern);
        }
    }
}