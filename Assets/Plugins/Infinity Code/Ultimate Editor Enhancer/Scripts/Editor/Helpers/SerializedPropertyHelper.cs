/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static class SerializedPropertyHelper
    {
        public static object EMPTY_VALUE = new object();

        public static object GetBoxedValue(SerializedProperty prop)
        {
            switch (prop.propertyType)
            {
                case SerializedPropertyType.Generic:
                    return EMPTY_VALUE;
                case SerializedPropertyType.Integer:
                case SerializedPropertyType.Enum:
#if UNITY_2022_1_OR_NEWER
                    switch (prop.numericType)
                    {
                        case SerializedPropertyNumericType.Int8:
                            return (sbyte) prop.intValue;
                        case SerializedPropertyNumericType.UInt8:
                            return (byte) prop.uintValue;
                        case SerializedPropertyNumericType.Int16:
                            return (short) prop.intValue;
                        case SerializedPropertyNumericType.UInt16:
                            return (ushort) prop.uintValue;
                        case SerializedPropertyNumericType.UInt32:
                            return prop.uintValue;
                        case SerializedPropertyNumericType.Int64:
                            return prop.longValue;
                        case SerializedPropertyNumericType.UInt64:
                            return prop.ulongValue;
                        default:
                            return prop.intValue;
                    }
#else
                    return prop.intValue;
#endif
                case SerializedPropertyType.Boolean:
                    return prop.boolValue;
                case SerializedPropertyType.Float:
#if UNITY_2022_1_OR_NEWER
                    return prop.numericType == SerializedPropertyNumericType.Double ? prop.doubleValue : prop.floatValue;
#else
                    return prop.floatValue;
#endif
                case SerializedPropertyType.String:
                    return prop.stringValue;
                case SerializedPropertyType.Color:
                    return prop.colorValue;
                case SerializedPropertyType.ObjectReference:
                    return prop.objectReferenceValue;
                case SerializedPropertyType.LayerMask:
                    return (LayerMask) prop.intValue;
                case SerializedPropertyType.Vector2:
                    return prop.vector2Value;
                case SerializedPropertyType.Vector3:
                    return prop.vector3Value;
                case SerializedPropertyType.Vector4:
                    return prop.vector4Value;
                case SerializedPropertyType.Rect:
                    return prop.rectValue;
                case SerializedPropertyType.ArraySize:
                    return prop.intValue;
                case SerializedPropertyType.Character:
#if UNITY_2022_1_OR_NEWER
                    return (ushort) prop.uintValue;
#else
                    return (ushort)prop.intValue;
#endif
                case SerializedPropertyType.AnimationCurve:
                    return prop.animationCurveValue;
                case SerializedPropertyType.Bounds:
                    return prop.boundsValue;
                case SerializedPropertyType.Gradient:
#if UNITY_2022_1_OR_NEWER
                    return prop.gradientValue;
#else
                    return EMPTY_VALUE;
#endif
                case SerializedPropertyType.Quaternion:
                    return prop.quaternionValue;
                case SerializedPropertyType.ExposedReference:
                    return prop.exposedReferenceValue;
                case SerializedPropertyType.FixedBufferSize:
                    return prop.intValue;
                case SerializedPropertyType.Vector2Int:
                    return prop.vector2IntValue;
                case SerializedPropertyType.Vector3Int:
                    return prop.vector3IntValue;
                case SerializedPropertyType.RectInt:
                    return prop.rectIntValue;
                case SerializedPropertyType.BoundsInt:
                    return prop.boundsIntValue;
                case SerializedPropertyType.ManagedReference:
#if UNITY_2022_1_OR_NEWER
                    return prop.managedReferenceValue;
#else
                    return EMPTY_VALUE;
#endif

#if UNITY_2022_1_OR_NEWER
                case SerializedPropertyType.Hash128:
                    return prop.hash128Value;
#endif
                default:
                    return EMPTY_VALUE;
            }
        }

        public static void SetBoxedValue(SerializedProperty prop, object value)
        {
            if (value == EMPTY_VALUE) return;

            try
            {
                switch (prop.propertyType)
                {
                    case SerializedPropertyType.Generic:
                        break;
                    case SerializedPropertyType.Integer:
                    case SerializedPropertyType.Enum:
                    case SerializedPropertyType.ArraySize:
#if UNITY_2022_1_OR_NEWER
                        if (prop.numericType == SerializedPropertyNumericType.UInt64)
                        {
                            prop.ulongValue = Convert.ToUInt64(value);
                            break;
                        }
                        prop.longValue = Convert.ToInt64(value);
#else
                        prop.intValue = Convert.ToInt32(value);
#endif
                        break;
                    case SerializedPropertyType.Boolean:
                        prop.boolValue = (bool)value;
                        break;
                    case SerializedPropertyType.Float:
#if UNITY_2022_1_OR_NEWER
                        if (prop.numericType == SerializedPropertyNumericType.Double)
                        {
                            prop.doubleValue = Convert.ToDouble(value);
                            break;
                        }
#else
                        prop.floatValue = Convert.ToSingle(value);
#endif
                        break;
                    case SerializedPropertyType.String:
                        prop.stringValue = (string)value;
                        break;
                    case SerializedPropertyType.Color:
                        prop.colorValue = (Color)value;
                        break;
                    case SerializedPropertyType.ObjectReference:
                        prop.objectReferenceValue = (UnityEngine.Object)value;
                        break;
                    case SerializedPropertyType.LayerMask:
                        try
                        {
                            prop.intValue = ((LayerMask)value).value;
                            break;
                        }
                        catch (InvalidCastException)
                        {
                            prop.intValue = Convert.ToInt32(value);
                            break;
                        }
                    case SerializedPropertyType.Vector2:
                        prop.vector2Value = (Vector2)value;
                        break;
                    case SerializedPropertyType.Vector3:
                        prop.vector3Value = (Vector3)value;
                        break;
                    case SerializedPropertyType.Vector4:
                        prop.vector4Value = (Vector4)value;
                        break;
                    case SerializedPropertyType.Rect:
                        prop.rectValue = (Rect)value;
                        break;
                    case SerializedPropertyType.Character:
#if UNITY_2022_1_OR_NEWER
                        prop.uintValue = (uint)Convert.ToUInt16(value);
#endif
                        break;
                    case SerializedPropertyType.AnimationCurve:
                        prop.animationCurveValue = (AnimationCurve)value;
                        break;
                    case SerializedPropertyType.Bounds:
                        prop.boundsValue = (Bounds)value;
                        break;
                    case SerializedPropertyType.Gradient:
#if UNITY_2022_1_OR_NEWER
                        prop.gradientValue = (Gradient)value;
#endif
                        break;
                    case SerializedPropertyType.Quaternion:
                        prop.quaternionValue = (Quaternion)value;
                        break;
                    case SerializedPropertyType.ExposedReference:
                        prop.exposedReferenceValue = (UnityEngine.Object)value;
                        break;
                    case SerializedPropertyType.Vector2Int:
                        prop.vector2IntValue = (Vector2Int)value;
                        break;
                    case SerializedPropertyType.Vector3Int:
                        prop.vector3IntValue = (Vector3Int)value;
                        break;
                    case SerializedPropertyType.RectInt:
                        prop.rectIntValue = (RectInt)value;
                        break;
                    case SerializedPropertyType.BoundsInt:
                        prop.boundsIntValue = (BoundsInt)value;
                        break;
                    case SerializedPropertyType.ManagedReference:
                        prop.managedReferenceValue = value;
                        break;
#if UNITY_2022_1_OR_NEWER
                    case SerializedPropertyType.Hash128:
                        prop.hash128Value = (Hash128)value;
                        break;
#endif
                }
            }
            catch
            {
                
            }
        }
    }
}