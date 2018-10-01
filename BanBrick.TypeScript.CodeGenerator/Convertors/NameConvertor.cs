using BanBrick.TypeScript.CodeGenerator.Annotations;
using BanBrick.TypeScript.CodeGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Convertors
{
    internal sealed class NameConvertor
    {
        private readonly TypeHelper _typeHelper;

        public NameConvertor()
        {
            _typeHelper = new TypeHelper();
        }

        /// <summary>
        /// Get TypeScript Object Name from type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetName(Type type)
        {
            if (_typeHelper.IsPrimitiveType(type))
                return GetPrimitiveName(type);

            if (_typeHelper.IsDictionaryType(type))
                return GetDictionaryName(type);

            if (_typeHelper.IsCollectionType(type))
                return GetCollectionName(type);

            if (type.IsGenericType)
                return GetGenericName(type);

            return GetObjectName(type);
        }

        public string GetGenericName(Type type)
        {
            throw new NotImplementedException();
        }

        public string GetObjectName(Type type)
        {
            var name = type.Name;

            var tsObjectInfo = (TypeScriptObjectAttribute)type.GetTypeInfo().GetCustomAttributes()
                .FirstOrDefault(x => x.GetType().Name == nameof(TypeScriptObjectAttribute));

            if (tsObjectInfo != null && !string.IsNullOrEmpty(tsObjectInfo.Name))
                name = tsObjectInfo.Name;

            return name;
        }
        
        public string GetPrimitiveName(Type type)
        {
            var primitiveType = type;

            if (_typeHelper.IsNullable(type) && type.IsGenericType)
            {
                primitiveType = type.GenericTypeArguments.First();
            }

            if (_typeHelper.IsNumericType(primitiveType))
                return "number";

            if (primitiveType == typeof(string) || primitiveType == typeof(DateTime))
                return "string";

            if (primitiveType == typeof(bool))
                return "boolean";

            return "any";
        }

        public string GetDictionaryName(Type type)
        {
            var keyValuetypes = type.GenericTypeArguments;
            var keyType = keyValuetypes[0];
            var valueType = keyValuetypes[1];

            return $"{{ [key: {GetName(keyType)}] : {GetName(valueType)} }}";
        }

        public string GetCollectionName(Type type)
        {
            var collectionType = type.IsArray ? type.GetElementType() : type.GenericTypeArguments.First();
            return $"{GetName(collectionType)}[]";
        }
    }
}
