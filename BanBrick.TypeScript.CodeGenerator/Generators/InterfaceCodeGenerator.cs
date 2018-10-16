using BanBrick.TypeScript.CodeGenerator.Convertors;
using BanBrick.TypeScript.CodeGenerator.Extensions;
using BanBrick.TypeScript.CodeGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Generators
{
    internal class InterfaceCodeGenerator : ICodeGenerator
    {
        private readonly TypeHelper _typeHelper;
        private readonly PropertyHelper _propertyHelper;
        
        private readonly INameConvertor _nameConvertor;

        public InterfaceCodeGenerator(INameConvertor nameConvertor)
        {
            _typeHelper = new TypeHelper();
            _propertyHelper = new PropertyHelper();
            
            _nameConvertor = nameConvertor;
        }

        public string Generate(Type type)
        {
            var stringBuilder = new StringBuilder();

            var typeScriptType = _nameConvertor.GetName(type);

            stringBuilder.AppendLine($"export interface {typeScriptType} {{");

            object instance = null;

            // create new instance if type contains parameterless constractor
            if (type.GetConstructor(Type.EmptyTypes) != null)
            {
                instance = Activator.CreateInstance(type);
            }

            var properties = TypeExtensions.GetProperties(type).ToList().OrderBy(x => x.Name);

            foreach (var property in properties)
            {
                if (_propertyHelper.IsTypeScriptIgnored(property))
                    continue;

                var propertyType = property.PropertyType;
                var propertyName = _nameConvertor.GetName(propertyType);
                var propertyValue = instance == null ? null : property.GetValue(instance);

                var nullableCode = _typeHelper.IsNullable(propertyType) ? "?" : "";

                stringBuilder.AppendLine($"  {property.Name.ToCamelCase()}{nullableCode}: {propertyName};");
            }
            
            stringBuilder.AppendLine("}");

            return stringBuilder.ToString();
        }
    }
}
