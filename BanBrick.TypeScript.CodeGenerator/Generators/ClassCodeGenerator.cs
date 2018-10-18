using BanBrick.TypeScript.CodeGenerator.Convertors;
using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Extensions;
using BanBrick.TypeScript.CodeGenerator.Helpers;
using BanBrick.TypeScript.CodeGenerator.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Generators
{
    internal class ClassCodeGenerator: ICodeGenerator
    {
        private readonly TypeHelper _typeHelper;

        private readonly IValueConvertor _valueConvertor;
        private readonly INameConvertor _nameConvertor;

        public ClassCodeGenerator(INameConvertor nameConvertor, IValueConvertor valueConvertor)
        {
            _typeHelper = new TypeHelper();

            _valueConvertor = valueConvertor;
            _nameConvertor = nameConvertor;
        }

        public string Generate(Type type)
        {
            var stringBuilder = new StringBuilder();

            var typeScriptType = _nameConvertor.GetName(type);

            stringBuilder.AppendLine($"export class {typeScriptType} {{");
            stringBuilder.AppendLine("  constructor(");

            object instance = null;
            
            // create new instance if type contains parameterless constractor
            if (type.GetConstructor(Type.EmptyTypes) != null)
            {
                instance = Activator.CreateInstance(type);
            }

            var properties = TypeExtensions.GetProperties(type).ToList().OrderBy(x => x.Name);

            foreach (var property in properties)
            {
                if (property.IsTypeScriptIgnored())
                    continue;

                var propertyType = property.PropertyType;
                var propertyName = _nameConvertor.GetName(propertyType);
                var propertyValue = instance == null ? null : property.GetValue(instance);

                var valueCode = _valueConvertor.GetValue(propertyType, propertyValue);
                
                var noValueCode = string.IsNullOrEmpty(valueCode);

                var nullableCode =  _typeHelper.IsNullable(propertyType) && noValueCode ? "?" : "";
                stringBuilder.Append($"  public {property.Name.ToCamelCase()}{nullableCode}: ");
                
                if (!noValueCode)
                {
                    stringBuilder.AppendLine($"{ propertyName} = {valueCode},");
                }
                else
                {
                    stringBuilder.AppendLine($"{ propertyName},");
                }
            }

            stringBuilder.AppendLine("  ) { }");
            stringBuilder.AppendLine("}");

            return stringBuilder.ToString();
        }
    }
}
