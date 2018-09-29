using BanBrick.TypeScript.CodeGenerator.Annotations;
using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Helpers
{
    public class TypeHelper
    {
        public bool IsDictionaryType(Type type)
        {
            if (type.IsGenericType
                && type.GetGenericArguments().Count() == 2
                && type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                return true;

            if (type.IsGenericType
                && type.GetGenericArguments().Count() == 2
                && type.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>))
                return true;

            if (type.IsGenericType 
                && type.GetGenericArguments().Count() == 2
                && type.GetInterfaces().Any(x => x.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
                return true;

            return false;
        }

        /// <summary>
        /// check whether the type is a collection type, ICollection, IEnumerable, Array ...
        /// </summary>
        /// <param name="type">
        /// any type
        /// </param>
        /// <returns>
        /// true if the type is belong to a collection type
        /// </returns>
        public bool IsCollectionType(Type type)
         {
            if (type.IsArray)
                return true;

            if (type.IsGenericType
                && type.GetGenericArguments().Count() == 1
                && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return true;

            if (type.IsGenericType
                && type.GetGenericArguments().Count() == 1
                && type.GetInterfaces().Any(x => x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                return true;

            return false;
        }

        /// <summary>
        /// check whether the type is a c# primitive type, string, double, int ...
        /// </summary>
        /// <param name="type">
        /// any type
        /// </param>
        /// <returns>
        /// if the type is matched to any primitive type return ture.
        /// otherwise return false.
        /// </returns>
        public bool IsPrimitiveType(Type type)
        {
            if (IsPurePrimitiveType(type))
                return true;

            if (IsNullAblePrimitiveType(type))
                return true;

            return false;
        }

        public bool IsNullableType(Type type)
        {
            return !type.IsValueType || IsNullableValueType(type);
        }

        public bool IsNullableValueType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public bool IsNullAblePrimitiveType(Type type)
        {
            return type.IsValueType && IsNullableValueType(type) && IsPurePrimitiveType(type.GenericTypeArguments.First());
        }

        public bool IsPurePrimitiveType(Type type)
        {
            if (type.IsPrimitive || IsSpecialPrimitiveType(type))
            {
                return true;
            }
            return false;
        }

        public bool IsSpecialPrimitiveType(Type type)
        {
            if (type == typeof(Decimal) || type == typeof(String) || type == typeof(DateTime))
            {
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Get TypeCategory for Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public TypeCategory GetTypeCategory(Type type)
        {
            if (IsPrimitiveType(type))
                return TypeCategory.Primitive;

            if (IsDictionaryType(type))
                return TypeCategory.Dictionary;

            if (IsCollectionType(type))
                return TypeCategory.Collection;

            if (type.IsEnum)
                return TypeCategory.Enum;

            if (type.IsGenericType)
                return TypeCategory.Generic;

            return TypeCategory.Object;
        }

        /// <summary>
        /// convert type to typescript processing type
        /// </summary>
        /// <param name="type">
        /// any type
        /// </param>
        /// <returns>
        /// processing type with typescript object name and typescript object type
        /// </returns>
        public ManagedType ToCategoryType(Type type)
        {
            return new ManagedType()
            {
                Type = type,
                Category = GetTypeCategory(type),
            };
        }

        /// <summary>
        /// Get TypeScript Object Name from type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetTypeScriptName(Type type) {
            if (IsPrimitiveType(type))
                return GetPrimitiveTypeScriptName(type);

            if (IsDictionaryType(type))
                return GetDictionaryTypeScriptName(type);

            if (IsCollectionType(type))
                return GetCollectionTypeScriptName(type);
            
            return GetObjectTypeScriptName(type);
        }

        public string GetObjectTypeScriptName(Type type)
        {
            var isNullable = IsNullableValueType(type);

            var objectType = isNullable ? type.GetGenericArguments().First() : type;
            
            var name = objectType.Name;
            
            var tsObjectInfo = (TypeScriptObjectAttribute)type.GetTypeInfo().GetCustomAttributes()
                .FirstOrDefault(x => x.GetType().Name == nameof(TypeScriptObjectAttribute));

            if (tsObjectInfo != null)
                name = tsObjectInfo.Name;

            return name;
        }

        public string GetPrimitiveTypeScriptName(Type type) {
            var primitiveType = type;

            if (IsNullAblePrimitiveType(type))
            {
                primitiveType = type.GenericTypeArguments.First();
            }

            if (IsNumericType(primitiveType))
                return "Number";

            if (primitiveType == typeof(string) || primitiveType == typeof(DateTime))
                return "String";

            if (primitiveType == typeof(bool))
                return "Boolean";

            return "any";
        }

        public string GetDictionaryTypeScriptName(Type type)
        {
            var keyValuetypes = type.GenericTypeArguments;
            var keyType = keyValuetypes[0];
            var valueType = keyValuetypes[1];
            var nullableCode = IsNullableType(valueType) ? "?" : "";

            return $"{{ [key: {GetTypeScriptName(keyType)}] : {GetTypeScriptName(valueType)}{nullableCode} }}";
        }

        public string GetCollectionTypeScriptName(Type type)
        {
            var collectionType = type.IsArray ? type.GetElementType() : type.GenericTypeArguments.First();
            return $"{GetTypeScriptName(collectionType)}[]";
        }

        public bool IsNumericType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
