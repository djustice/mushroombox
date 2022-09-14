/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Reflection;
using UnityEditorInternal;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer.UnityTypes
{
    public static class ReorderableListRef
    {
        private static MethodInfo _doListElementsMethod;

        public static MethodInfo doListElementsMethod
        {
            get
            {
                if (_doListElementsMethod == null)
                {
                    _doListElementsMethod = type.GetMethod("DoListElements", Reflection.InstanceLookup, null, 
                        new []
                        {
                            typeof(Rect),
#if UNITY_2020_2_OR_NEWER
                            typeof(Rect)
#endif
                        }, null);
                }
                return _doListElementsMethod;
            }
        }

        public static Type type
        {
            get => typeof(ReorderableList);
        }

#if UNITY_2021_3_OR_NEWER || UNITY_2022_1_OR_NEWER
        private static MethodInfo _invalidateCacheMethod;

        private static MethodInfo invalidateCacheMethod
        {
            get
            {
                if (_invalidateCacheMethod == null) _invalidateCacheMethod = type.GetMethod("InvalidateCache", Reflection.InstanceLookup);
                return _invalidateCacheMethod;
            }
        }
#elif UNITY_2020_2_OR_NEWER
        private static MethodInfo _clearCacheMethod;

        private static MethodInfo clearCacheMethod
        {
            get
            {
                if (_clearCacheMethod == null) _clearCacheMethod = type.GetMethod("ClearCache", Reflection.InstanceLookup);
                return _clearCacheMethod;
            }
        }
#endif

        public static void ClearCache(ReorderableList list)
        {
#if UNITY_2021_3_OR_NEWER || UNITY_2022_1_OR_NEWER
            invalidateCacheMethod.Invoke(list, null);
#elif UNITY_2020_2_OR_NEWER
            clearCacheMethod.Invoke(list, null);
#endif
        }
    }
}