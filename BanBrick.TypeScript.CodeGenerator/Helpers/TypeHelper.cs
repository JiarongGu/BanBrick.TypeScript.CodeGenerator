using BanBrick.TypeScript.CodeGenerator.Annotations;
using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Helpers
{
    internal class TypeHelper
    {
        public bool IsDictionaryType(Type type)
        {
            if (GetGenericTypeDefinition(type) == typeof(IDictionary<,>))
                return true;

            if (GetGenericTypeDefinition(type) == typeof(IReadOnlyDictionary<,>))
                return true;

            if (type.GetInterfaces().Any(x => GetGenericTypeDefinition(x) == typeof(IDictionary<,>)))
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

            // collection type only has 1 generic argument
            if (!(type.IsGenericType && type.GetGenericArguments().Count() == 1))
                return false;

            if (GetGenericTypeDefinition(type) == typeof(IEnumerable<>))
                return true;

            if (type.GetInterfaces().Any(x => GetGenericTypeDefinition(x) == typeof(IEnumerable<>)))
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

        public bool IsNullable(Type type)
        {
            return !type.IsValueType || IsNullAblePrimitiveType(type);
        }

        /// <summary>
        /// Get TypeCategory for Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ProcessingCategory GetProcessingCategory(Type type)
        {
            if (IsPrimitiveType(type))
                return ProcessingCategory.Primitive;

            if (IsDictionaryType(type))
                return ProcessingCategory.Dictionary;

            if (IsCollectionType(type))
                return ProcessingCategory.Collection;

            if (type.IsEnum)
                return ProcessingCategory.Enum;

            if (type.IsGenericType)
                return ProcessingCategory.Generic;

            if (type.IsInterface)
                return ProcessingCategory.Interface;

            return ProcessingCategory.Object;
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
        public TypeDefinition ToTypeDefinition(Type type)
        {
            var nullable = IsNullable(type);
            var category = GetProcessingCategory(type);
            var actualType = GetActualType(type, category, nullable);

            return new TypeDefinition()
            {
                Type = type,
                ActualType = actualType,
                Category = category,
                IsNullable = nullable,
                IsNumeric = IsNumericType(type),
                ProcessType = GetProcessType(type, category)
            };
        }

        private TypeScriptObjectType GetProcessType(Type type, ProcessingCategory category) {
            var info = type.GetCustomAttribute<TypeScriptObjectAttribute>();
            if (info == null || info.Type == TypeScriptObjectType.Default)
            {
                switch (category)
                {
                    case ProcessingCategory.Enum:
                        return TypeScriptObjectType.Enum;
                    case ProcessingCategory.Object:
                        return TypeScriptObjectType.Class;
                    case ProcessingCategory.Interface:
                        return TypeScriptObjectType.Interface;
                    default:
                        return TypeScriptObjectType.Default;
                }
            }
            else {
                return info.Type;
            }
        }

        private Type GetActualType(Type type, ProcessingCategory category, bool nullable ) {
            if (category == ProcessingCategory.Primitive && nullable && type.IsGenericType)
                return type.GenericTypeArguments.First();
            return type;
        }

        public Type GetGenericTypeDefinition(Type type)
        {
            if (type.IsGenericType)
                return type.GetGenericTypeDefinition();
            return null;
        }

        private bool IsNullAblePrimitiveType(Type type)
        {
            if (!type.IsValueType)
                return false;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return true;

            return false;
        }

        private bool IsPurePrimitiveType(Type type)
        {
            if (type.IsPrimitive || IsSpecialPrimitiveType(type))
            {
                return true;
            }
            return false;
        }

        private bool IsSpecialPrimitiveType(Type type)
        {
            if (type == typeof(decimal) || type == typeof(string) || type == typeof(DateTime))
            {
                return true;
            }
            return false;
        }
    }
}