using BanBrick.TypeScript.CodeGenerator.Annotations;
using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Models;
using BanBrick.TypeScript.CodeGenerator.TypeHandlers;
using BanBrick.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BanBrick.TypeScript.CodeGenerator.Resolvers
{
    internal class TypeResolver
    {
        private readonly IDictionary<Type, ITypeHandler> _typeHandlerMap;

        public TypeResolver(IDictionary<Type, ITypeHandler> typeHandlerMap)
        {
            _typeHandlerMap = typeHandlerMap;
        }

        public bool IsDictionaryType(Type type)
        {
            if (type.GetGenericTypeDefinitionOrDefault() == typeof(IDictionary<,>))
                return true;

            if (type.GetGenericTypeDefinitionOrDefault() == typeof(IReadOnlyDictionary<,>))
                return true;

            if (type.GetInterfaces().Any(x => x.GetGenericTypeDefinitionOrDefault() == typeof(IDictionary<,>)))
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
            
            if (type.GetGenericTypeDefinitionOrDefault() == typeof(IEnumerable<>))
                return true;

            if (type.GetInterfaces().Any(x => x.GetGenericTypeDefinitionOrDefault() == typeof(IEnumerable<>)))
                return true;

            return false;
        }
        
        public bool IsNullable(Type type)
        {
            if (_typeHandlerMap.ContainsKey(type))
                return _typeHandlerMap[type].DefaultValue == null;
            return !type.IsValueType;
        }

        /// <summary>
        /// Get TypeCategory for Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ProcessingCategory GetProcessingCategory(Type type)
        {
            if (_typeHandlerMap.ContainsKey(type))
                return _typeHandlerMap[type].ProcessingCategory;

            // default processing category
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
            var category = GetProcessingCategory(type);
            var typeHandler = _typeHandlerMap.ContainsKey(type) ? _typeHandlerMap[type] : null;

            return new TypeDefinition()
            {
                Type = type,
                ProcessingCategory = category,
                TypeHandler = typeHandler,
                IsNullable = IsNullable(type)
            };
        }
    }
}