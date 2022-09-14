/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Reflection;
using InfinityCode.UltimateEditorEnhancer.UnityTypes;
using UnityEditor;

namespace InfinityCode.UltimateEditorEnhancer.Interceptors
{
    public class TransformInspectorInterceptor: StatedInterceptor<TransformInspectorInterceptor>
    {
        public static Func<Editor, bool> DrawInspector3D;

        protected override MethodInfo originalMethod
        {
            get => TransformInspectorRef.inspector3DMethod;
        }

        public override bool state
        {
            get => true;
        }

        protected override string prefixMethodName { get => nameof(Inspector3DPrefix); }

        private static bool Inspector3DPrefix(Editor __instance)
        {
            if (DrawInspector3D != null) return DrawInspector3D(__instance);
            return true;
        }
    }
}