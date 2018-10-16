using BanBrick.TypeScript.CodeGenerator.Convertors;
using BanBrick.TypeScript.CodeGenerator.Extensions;
using BanBrick.TypeScript.CodeGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Generators
{
    internal class ConstCodeGenerator : ICodeGenerator
    {
        private readonly TypeHelper _typeHelper;
        private readonly PropertyHelper _propertyHelper;

        private readonly IValueConvertor _valueConvertor;
        private readonly INameConvertor _nameConvertor;

        public ConstCodeGenerator(INameConvertor nameConvertor, IValueConvertor valueConvertor)
        {
            _typeHelper = new TypeHelper();
            _propertyHelper = new PropertyHelper();

            _valueConvertor = valueConvertor;
            _nameConvertor = nameConvertor;
        }

        public string Generate(Type type)
        {
            var stringBuilder = new StringBuilder();

            var name = _nameConvertor.GetName(type).ToCamelCase();

            stringBuilder.AppendLine($"export const {name} = {{");

            object instance = null;

            // create new instance if type contains parameterless constractor
            if (type.GetConstructor(Type.EmptyTypes) != null)
            {
                instance = Activator.CreateInstance(type);
            }

            var properties = TypeExtensions.GetProperties(type);
            foreach (var property in properties)
            {
                if (_propertyHelper.IsTypeScriptIgnored(property))
                    continue;

                var propertyName = property.Name.ToCamelCase();
                var propertyType = property.PropertyType;
                var propertyValue = instance == null ? null : property.GetValue(instance);

                var valueCode = _valueConvertor.GetValue(propertyType, propertyValue, 2);
                
                if (!string.IsNullOrEmpty(valueCode))
                {
                    stringBuilder.AppendLine($"  {propertyName}: {valueCode},");
                }   
            }

            stringBuilder.AppendLine("}");

            return stringBuilder.ToString();
        }
    }
}
