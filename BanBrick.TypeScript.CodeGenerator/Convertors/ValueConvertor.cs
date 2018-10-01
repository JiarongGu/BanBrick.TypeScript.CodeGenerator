using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Extensions;
using BanBrick.TypeScript.CodeGenerator.Helpers;
using BanBrick.TypeScript.CodeGenerator.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Convertors
{
    internal interface IValueConvertor
    {
        string GetValue(Type type, object value);
    }

    internal sealed class ValueConvertor: IValueConvertor
    {
        private readonly PropertyHelper _propertyHelper;
        private readonly INameConvertor _nameConvertor;
        private readonly IDictionary<Type, TypeDefinition> _typeDictionary;

        public ValueConvertor(IEnumerable<TypeDefinition> typeDefinitions, INameConvertor nameConvertor)
        {
            _propertyHelper = new PropertyHelper();
            _nameConvertor = nameConvertor;
            _typeDictionary = typeDefinitions.ToDictionary(x => x.Type, x => x);
        }

        /// <summary>
        /// Generate value code for typescript in including value check
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetValue(Type type, object value)
        {
            var category = _typeDictionary[type].Category;

            if (category == ProcessingCategory.Primitive)
            {
                if (value == null) return null;
                return GetPrimitive(value);
            }

            if (category == ProcessingCategory.Enum)
            {
                return GetEnumValue(_nameConvertor.GetName(type), value);
            }

            if (category == ProcessingCategory.Collection)
            {
                return GenerateArray(value);
            }

            if (category == ProcessingCategory.Dictionary)
            {
                return GenerateDictionary(value);
            }

            if (category == ProcessingCategory.Object)
            {
                return GenerateObject(value);
            }

            return "";
        }
        
        private string GetPrimitive(object value)
        {
            if (value == null) return "null";

            var type = value.GetType();
            var typeDefinition = _typeDictionary[type];

            if (type == typeof(bool))
                return (bool)value ? "true" : "false";

            if (typeDefinition.IsNumeric)
                return value.ToString();

            if (type == typeof(string))
                return ((string)value).ToTypeScript();

            if (type == typeof(DateTime) && ((DateTime)value).Ticks > 0 )
                return ((DateTime)value).ToUniversalTime().ToString("o", CultureInfo.InvariantCulture).ToTypeScript();

            return null;
        }

        private string GenerateArray(object value)
        {
            if (value == null)
                return "[]";

            var arrayValues = value as IEnumerable;
            var arrayValuesCode = new List<string>();

            foreach (var arrayValue in arrayValues)
            {
                arrayValuesCode.Add(GetValue(arrayValue.GetType(), arrayValue));
            }

            return $"[ {string.Join(", ", arrayValuesCode)} ]";
        }

        private string GenerateDictionary(object value)
        {
            if (value == null)
                return "{}";

            var dictionaryValue = value as IDictionary;
            var dictionaryValuesCode = new List<string>();

            foreach (var key in dictionaryValue.Keys)
            {
                var keyValue = dictionaryValue[key];
                var keyType = key.GetType();
                var keyValueType = keyValue.GetType();

                dictionaryValuesCode.Add($"{GetValue(keyType, key)}: {GetValue(keyValueType, keyValue)}");
            }

            return $"{{ {string.Join(", ", dictionaryValuesCode)} }}";
        }

        private string GenerateObject(object value)
        {
            if (value == null)
                return "";

            var objectType = value.GetType();
            var properties = TypeExtensions.GetProperties(objectType);
            var objectPropertiesCode = new List<string>();

            foreach (var property in properties)
            {
                if (_propertyHelper.IsTypeScriptIgnored(property))
                    continue;

                var propertyType = property.PropertyType;
                var propertyName = _nameConvertor.GetName(propertyType);
                var valueCode = GetValue(propertyType, property.GetValue(value));

                objectPropertiesCode.Add($"{property.Name.ToCamelCase()}: {valueCode}");
            }
            return $"{{ {string.Join(",\n", objectPropertiesCode)} }}";
        }

        private string GetEnumValue(string enumName, object value)
        {
            return $"{enumName}.{value.ToString()}";
        }
    }
}
