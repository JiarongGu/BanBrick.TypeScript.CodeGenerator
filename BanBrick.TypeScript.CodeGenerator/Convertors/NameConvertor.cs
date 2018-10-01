using BanBrick.TypeScript.CodeGenerator.Annotations;
using BanBrick.TypeScript.CodeGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Convertors
{
    public class NameConvertor
    {
        private readonly TypeHelper _typeHelper;
        private readonly PropertyHelper _propertyHelper;
        private readonly StringHelper _stringHelper;

        public NameConvertor()
        {
            _typeHelper = new TypeHelper();
            _propertyHelper = new PropertyHelper();
            _stringHelper = new StringHelper();
        }

        /// <summary>
        /// Get TypeScript Object Name from type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetTypeScriptName(Type type)
        {
            if (_typeHelper.IsPrimitiveType(type))
                return GetPrimitiveTypeScriptName(type);

            if (_typeHelper.IsDictionaryType(type))
                return GetDictionaryTypeScriptName(type);

            if (_typeHelper.IsCollectionType(type))
                return GetCollectionTypeScriptName(type);

            return GetObjectTypeScriptName(type);
        }

        public string GetObjectTypeScriptName(Type type)
        {
            var isNullable = _typeHelper.IsNullableValueType(type);

            var objectType = isNullable ? type.GetGenericArguments().First() : type;

            var name = objectType.Name;

            var tsObjectInfo = (TypeScriptObjectAttribute)type.GetTypeInfo().GetCustomAttributes()
                .FirstOrDefault(x => x.GetType().Name == nameof(TypeScriptObjectAttribute));

            if (tsObjectInfo != null && !string.IsNullOrEmpty(tsObjectInfo.Name))
                name = tsObjectInfo.Name;

            return name;
        }

        public string GetPrimitiveTypeScriptName(Type type)
        {
            var primitiveType = type;

            if (_typeHelper.IsNullAblePrimitiveType(type))
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

        public string GetDictionaryTypeScriptName(Type type)
        {
            var keyValuetypes = type.GenericTypeArguments;
            var keyType = keyValuetypes[0];
            var valueType = keyValuetypes[1];

            return $"{{ [key: {GetTypeScriptName(keyType)}] : {GetTypeScriptName(valueType)} }}";
        }

        public string GetCollectionTypeScriptName(Type type)
        {
            var collectionType = type.IsArray ? type.GetElementType() : type.GenericTypeArguments.First();
            return $"{GetTypeScriptName(collectionType)}[]";
        }
    }
}
